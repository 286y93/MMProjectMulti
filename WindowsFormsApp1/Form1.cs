using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AxMMMarkx641Lib;
using AxMMEditx641Lib;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // 四個晶片板的多系統控件陣列
        private AxMMMarkx641[] m_MMMark = new AxMMMarkx641[4];
        private AxMMEditx641[] m_MMEdit = new AxMMEditx641[4];

        private Panel[] m_Panels;
        private bool m_bInit = false;
        private bool m_bPreviewing = false; // 追蹤是否在紅光預覽中（DXF / QR / 手動 三頁籤共用）
        // 並行驗證用：獨立於 m_bPreviewing/timerPreview，避免污染既有狀態
        private bool m_bParallelTesting = false;
        private List<int> m_ParallelTestBoards = new List<int>();
        // 命令頁籤的 per-board 狀態 — 允許多板同時跑各自的紅光預覽
        private bool[] m_bCmdPreviewing = new bool[4];
        private System.Windows.Forms.Timer[] m_TimerCmdPreview = new System.Windows.Forms.Timer[4];

        // Daemon mode
        private MarkingMateDaemon m_Daemon;
        private int m_iCurrentBoard = 0;
        private int m_iPreviewBoard = -1;   // 目前紅光預覽的板索引（DXF/QR/手動共用 timerPreview）

        // 每個晶片板的配置路徑
        // 注意：MarkingMate MultiMM SDK 預設支援 MM1 和 MM2
        // 若需要 MM3、MM4，需在 MarkingMate 中建立對應的配置
        private string[] m_ConfigPaths = new string[]
        {
            "/cfg_config_MM1",
            "/cfg_config_MM2",
            "/cfg_config_MM3",
            "/cfg_config_MM4"
        };

        // 記錄每個板是否成功初始化
        private bool[] m_bBoardInit = new bool[4];

        // 雷射頭設定來源（專案內，相對於 .csproj 根；初始化時會部署到 MarkingMate 安裝目錄）
        // 各 MMx 的 IP 檔仍會部署，但 DEV0 在初始化時會由「主 IP 表」依固定 CARD 對應自動同步
        private static readonly string[] m_IPConfigRelativePaths = new string[]
        {
            @"Drivers\EMC6_MM1\DevIPAddress.ini",
            @"Drivers\EMC6_MM2\DevIPAddress.ini",
            @"Drivers\EMC6_MM3\DevIPAddress.ini",
            @"Drivers\EMC6_MM4\DevIPAddress.ini"
        };

        // 每片板的雷射機台設定檔（可選；若不存在則僅使用共用 config\config.ini）
        // 早期設計每板一份，現在統一以共用 config\config.ini 為主，這裡保留以兼容舊配置
        private static readonly string[] m_LaserConfigRelativePaths = new string[]
        {
            @"config\config_MM1.ini",
            @"config\config_MM2.ini",
            @"config\config_MM3.ini",
            @"config\config_MM4.ini"
        };

        // 「卡片 IP 主表」：UI 與初始化的單一來源，DEV0~DEV3 對應 CARD0~CARD3 → MM1~MM4
        private const string MasterIPRelativePath = @"Drivers\EMC6\DevIPAddress.ini";

        // EMC6 共用驅動目錄（含驅動 / cfg / 主 IP 表），初始化時整個遞迴部署
        private const string SharedDriverDirRelativePath = @"Drivers\EMC6";

        // 共用主 config.ini，初始化時部署到 MarkingMate\config.ini
        private const string SharedConfigRelativePath = @"config\config.ini";

        // MarkingMate 安裝根目錄（部署目標）
        private const string MarkingMateRoot = @"C:\Program Files (x86)\MarkingMate";

        private TextBox[] m_txtIPs;

        // 工作區大小設定（mm），影響 SetDesktopSize 和 DXF 縮放
        // m_WorkspaceSize 代表「寬 W」，m_WorkspaceHeight 代表「高 H」；
        // 允許 W != H，DXF 縮放會用 min(W, H) 以保持原始比例。
        private double m_WorkspaceSize = 150.0;
        private double m_WorkspaceHeight = 150.0;
        private double m_MarginPercent = 0.9;

        // 自動模式相關
        private CommandLineArgs m_AutoModeArgs = null;
        private bool m_IsAutoMode = false;
        public int ExitCode { get; private set; } = 0;

        // QR Code 條碼類型常數
        // TODO: 從 SDK 文件 (MultiMM OCX Manual) 確認正確的 lType 值
        private const int BARCODE_TYPE_QRCODE = 23;

        // QR 線段延伸量（SetBarcodeLineExtend 的 dLineExtend）。
        // 0 = 不延伸（等同現行行為）。線段填滿的模組邊緣若有未打滿的縫，調大這個值。
        private const double QR_LINE_EXTEND = 0.0;

        // GUI 模式建構子
        public Form1()
        {
            InitializeComponent();

            m_Panels = new Panel[] { panelBoard1, panelBoard2, panelBoard3, panelBoard4 };
            m_txtIPs = new TextBox[] { txtIP1, txtIP2, txtIP3, txtIP4 };
            comboBoard.SelectedIndex = 0;
            comboBoardDXF.SelectedIndex = 0;
            comboBoardLaser.SelectedIndex = 0;
            comboBoardQR.SelectedIndex = 0;
            comboBoardQR2.SelectedIndex = 0;
            comboBoardCmd.SelectedIndex = 0;
            // txtPulseWidth 預設值 5 (在 Designer 中設定)
            m_IsAutoMode = false;

            // 同步 UI 顯示初始值
            txtWorkspace.Text = m_WorkspaceSize.ToString();
            txtWorkspaceHeight.Text = m_WorkspaceHeight.ToString();
            txtMargin.Text = (m_MarginPercent * 100).ToString();

            this.btnPreviewDXF.Visible = true;
            this.btnClearDXF.Visible = true;

            // 命令頁籤 per-board Timer：各板獨立倒數，於 Tag 紀錄 board index
            for (int i = 0; i < m_TimerCmdPreview.Length; i++)
            {
                var t = new System.Windows.Forms.Timer { Interval = 15000 };
                t.Tag = i;
                t.Tick += OnCmdPreviewTimerTick;
                m_TimerCmdPreview[i] = t;
            }

            // 「命令提示」tab：初次填入 5 組隨機指令
            GenerateCmdPreviews();
        }

        // 自動模式建構子
        public Form1(CommandLineArgs args) : this()
        {
            m_AutoModeArgs = args;
            // 改為依據 args.IsAutoMode 來設定，而非強制設為 true
            m_IsAutoMode = args.IsAutoMode;
            m_WorkspaceSize = args.WorkspaceSize;
            m_WorkspaceHeight = args.WorkspaceHeight;
            // 預設建構子已把 textbox 設成預設 150，這裡同步成 args 帶進來的值，
            // 後續若呼叫 ReadWorkspaceSettings() 才不會把正確值蓋回 stale UI。
            txtWorkspace.Text = m_WorkspaceSize.ToString();
            txtWorkspaceHeight.Text = m_WorkspaceHeight.ToString();

            if (m_IsAutoMode)
            {
                // Debug: 確認確實進入 AutoMode
                // MessageBox.Show($"Auto Mode: {m_IsAutoMode}, Lines: {args.Lines.Count}, DXF: {args.DxfPath}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadIPSettings();

            // 如果是自動模式，啟動自動執行
            if (m_IsAutoMode && m_AutoModeArgs != null)
            {
                System.Windows.Forms.Timer startTimer = new System.Windows.Forms.Timer();
                startTimer.Interval = 100;
                startTimer.Tick += (s, ev) =>
                {
                    startTimer.Stop();
                    if (m_AutoModeArgs.DaemonMode)
                        StartDaemonMode();
                    else
                        ExecuteAutoMode();
                };
                startTimer.Start();
            }

            // 自動模式（包含 daemon）：最小化但不隱藏（OCX 需要 Window Handle）
            if (m_IsAutoMode)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        /// <summary>
        /// Daemon mode：一次 init 全 4 板後，啟動 HttpListener 等命令。
        /// 不會自動 Close，必須由 /shutdown 或使用者強制結束。
        /// </summary>
        private void StartDaemonMode()
        {
            try
            {
                Console.WriteLine("[Daemon] 啟動中，初始化全部 4 塊板...");
                if (!InitializeBoardAuto(m_bBoardInit.Length - 1, m_ConfigPaths[m_bBoardInit.Length - 1]))
                {
                    Console.Error.WriteLine("[Daemon] 初始化失敗，退出。");
                    ExitCode = 1;
                    this.Close();
                    return;
                }

                m_Daemon = new MarkingMateDaemon(this, m_AutoModeArgs.DaemonPort);
                m_Daemon.Start();
                Console.WriteLine($"[Daemon] 就緒，listening on http://localhost:{m_AutoModeArgs.DaemonPort}/");
                Console.WriteLine("[Daemon] Endpoints: POST /cmd, POST /shutdown, GET /health");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Daemon] 啟動失敗：{ex.Message}");
                ExitCode = 1;
                this.Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try { m_Daemon?.Stop(); } catch { }
            // 結束時收掉本場次啟動的 MM27Dx64 驅動宿主。
            // 這些是 EMC6 卡的驅動 process，過去只在「啟動 init 前」清、結束時不清，
            // 導致場次結束後變成孤兒 process，一直霸佔 EMC6 卡 / SDK 的 process-global OCX 鎖，
            // 害下一次 init 撞 "Please initial MMMark_1 OCX first!"（過去以為要重開機才能恢復）。
            CleanupMM27Dx64OnExit();
            base.OnFormClosed(e);
        }

        /// <summary>
        /// 結束時清理殘留的 MM27Dx64 驅動宿主，避免累積成孤兒 process 霸佔 SDK OCX 鎖。
        /// 僅在「沒有其他 MarkingMate 實例」時才殺，避免並行的其他實例正在使用它們。
        /// </summary>
        private static void CleanupMM27Dx64OnExit()
        {
            try
            {
                if (HasOtherMarkingMateInstance()) return; // 有其他實例 → 那些驅動可能還有人在用，不動

                var processes = System.Diagnostics.Process.GetProcessesByName("MM27Dx64");
                foreach (var proc in processes)
                {
                    try { proc.Kill(); }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"結束清理 MM27Dx64 失敗 (PID {proc.Id})：{ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CleanupMM27Dx64OnExit 例外：{ex.Message}");
            }
        }

        /// <summary>
        /// Daemon 派發進入點：解析 CLI 字串成 spec、在 UI thread 上備內容、啟動 marking、
        /// 用一個 per-spec 的 Timer 等完成（mode 3 看 preview-time，mode 4 看 IsMarking==0），
        /// 完成時 SetResult 給 TCS。回傳的 Task 可以被 HttpListener 的 handler thread await。
        /// </summary>
        public System.Threading.Tasks.Task<DaemonSpecResult> RunDaemonSpec(string cliArgs)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<DaemonSpecResult>();

            // 必須切到 UI thread（SDK STA 要求）
            this.BeginInvoke((Action)(() =>
            {
                var sbLog = new StringBuilder();
                int board = -1;
                try
                {
                    // 每次接到指令前重新讀取工作區設定，確保 m_WorkspaceSize 與 UI 一致，
                    // 後續 SetDesktopSize / 座標檢查不會用到過期值。
                    ReadWorkspaceSettings();

                    var argv = SplitCommandLine(cliArgs ?? "");
                    // 去掉開頭的 exe 名稱（網頁直接送 args 不會有，CLI client 也不會送）
                    if (argv.Length > 0 && !argv[0].StartsWith("-") &&
                        argv[0].EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        var rest = new string[argv.Length - 1];
                        Array.Copy(argv, 1, rest, 0, rest.Length);
                        argv = rest;
                    }

                    CommandLineArgs spec;
                    try { spec = CommandLineArgs.Parse(argv); }
                    catch (Exception parseEx)
                    {
                        tcs.SetResult(new DaemonSpecResult { ExitCode = 4, Logs = $"parse failed: {parseEx.Message}" });
                        return;
                    }

                    board = spec.BoardIndex;
                    if (board < 0 || board >= m_bBoardInit.Length || !m_bBoardInit[board])
                    {
                        tcs.SetResult(new DaemonSpecResult { ExitCode = 4, Logs = $"board {board + 1} not initialized" });
                        return;
                    }

                    if (IsBoardBusy(board))
                    {
                        tcs.SetResult(new DaemonSpecResult { ExitCode = 5, Logs = $"board {board + 1} busy" });
                        return;
                    }

                    if (spec.Lines == null || spec.Lines.Count == 0)
                    {
                        if (string.IsNullOrEmpty(spec.QRContent))
                        {
                            tcs.SetResult(new DaemonSpecResult { ExitCode = 4, Logs = "no content (need --line / --lines / --qrcode)" });
                            return;
                        }
                    }

                    // 1. 清板 + 加入內容
                    // ResetFile 後 SDK 文件的工作區會回到 config 預設值（通常小於 m_WorkspaceSize），
                    // 必須重新 SetDesktopCenter/SetDesktopSize，否則首次 AddLine 會被 OCX 判定為「超出工作範圍」。
                    //
                    // 工作區 W/H 決定：
                    //   - 非 QR + spec 有明確 --workspace-w/h → 以 spec 為準（line/DXF 需要）
                    //   - 其餘（QR、或沒帶 workspace 參數） → 沿用 UI 讀進來的 m_WorkspaceSize/Height
                    //   QR 物件位置固定 (0,0) 且不做工作範圍平移，這裡 SetDesktopSize 的值對 QR 無實際影響。
                    bool isQR = !string.IsNullOrEmpty(spec.QRContent);
                    double effectiveW = (!isQR && spec.WorkspaceWidthExplicit) ? spec.WorkspaceSize : m_WorkspaceSize;
                    double effectiveH = (!isQR && spec.WorkspaceHeightExplicit) ? spec.WorkspaceHeight : m_WorkspaceHeight;

                    m_MMMark[board].ResetFile();
                    m_MMMark[board].SetDesktopCenter(0, 0);
                    m_MMMark[board].SetDesktopSize(effectiveW, effectiveH);
                    Application.DoEvents();
                    Thread.Sleep(50);

                    if (spec.Lines != null && spec.Lines.Count > 0)
                    {
                        double halfW = effectiveW / 2.0;
                        double halfH = effectiveH / 2.0;
                        foreach (var line in spec.Lines)
                        {
                            bool isCenterBased = line.X1 < 0 || line.X2 < 0 || line.Y1 < 0 || line.Y2 < 0;
                            double x1, y1, x2, y2;
                            if (isCenterBased)
                            {
                                x1 = line.X1; y1 = line.Y1; x2 = line.X2; y2 = line.Y2;
                            }
                            else
                            {
                                x1 = line.X1 - halfW; y1 = line.Y1 - halfH;
                                x2 = line.X2 - halfW; y2 = line.Y2 - halfH;
                            }
                            m_MMEdit[board].AddLine(x1, y1, x2, y2, "", "");
                        }
                        sbLog.AppendLine($"[Board {board + 1}] added {spec.Lines.Count} line(s) (W={effectiveW},H={effectiveH})");
                    }
                    else if (spec.QRWhiteBg)
                    {
                        // 白底反相 QR：白底矩形 + 反相 QR 雙圖層（參數寫死，可用 CLI 覆寫）。
                        // 內含各物件雷射參數設定，故下方跳過 ApplyLaserParamsAuto 以免覆寫。
                        if (!BuildWhiteBgQR(board, MakeWhiteBgParams(spec)))
                        {
                            tcs.SetResult(new DaemonSpecResult { ExitCode = 2, Logs = sbLog.ToString() + $"[Board {board + 1}] 白底 QR 建立失敗" });
                            return;
                        }
                        sbLog.AppendLine($"[Board {board + 1}] added white-bg QR \"{spec.QRContent}\"");
                    }
                    else
                    {
                        m_MMMark[board].AddBarcode(BARCODE_TYPE_QRCODE, spec.QRContent,
                            0, 0, spec.QRWidth, spec.QRHeight, "", "");
                        Application.DoEvents();
                        Thread.Sleep(50);
                        ApplyQRECLevelLow(board);   // QR 固定容錯等級 LOW（EC=0）
                        if (spec.QRInvert)
                        {
                            Application.DoEvents();
                            Thread.Sleep(50);
                            ApplyQRInvert(board);
                        }
                        sbLog.AppendLine($"[Board {board + 1}] added QR \"{spec.QRContent}\"{(spec.QRInvert ? " (invert)" : "")}");
                    }

                    Application.DoEvents();
                    Thread.Sleep(50);
                    m_MMMark[board].Redraw();
                    Thread.Sleep(100);

                    // 2. 套用雷射參數（若有指定）— 用臨時換 m_AutoModeArgs 的小 hack 重用既有 method。
                    //    白底 QR 已在 BuildWhiteBgQR 內對各物件個別設好參數，這裡必須跳過，否則會被覆寫。
                    if (!spec.QRWhiteBg && (spec.Power.HasValue || spec.Speed.HasValue || spec.Frequency.HasValue
                        || spec.PulseWidth.HasValue || spec.MarkRepeat.HasValue || spec.WobbleWidth.HasValue))
                    {
                        var savedArgs = m_AutoModeArgs;
                        m_AutoModeArgs = spec;
                        try { ApplyLaserParamsAuto(board); }
                        finally { m_AutoModeArgs = savedArgs; }
                    }

                    // 3. 啟動 marking
                    int markMode = spec.PreviewMode > 0 ? 3 : 4;
                    if (spec.PreviewMode > 0)
                        m_MMMark[board].SetPreviewMode(spec.PreviewMode);
                    m_MMMark[board].MarkStandBy();
                    Application.DoEvents();

                    int rc = m_MMMark[board].StartMarking(markMode);
                    if (rc == 6)
                    {
                        // rc=6：該板仍在 marking/previewing。連續打（client 連發、板還沒打完）時很常見。
                        // ⚠ 絕不能 StopMarking 去中止「正在跑的前一條工作」——那會把線段打到一半砍掉、
                        //   且在 UI 執行緒上 Stop/Sleep 會拖累其他板的完成監控，造成整體卡住。
                        //   正確做法：回報「板忙碌」讓 client 稍後重送，完全不動正在跑的工作。
                        tcs.SetResult(new DaemonSpecResult
                        {
                            ExitCode = 5,
                            Logs = sbLog.ToString() + $"[Board {board + 1}] busy (StartMarking returned 6，前一條尚未打完，請稍後重送)"
                        });
                        return;
                    }
                    if (rc != 0)
                    {
                        tcs.SetResult(new DaemonSpecResult
                        {
                            ExitCode = 3,
                            Logs = sbLog.ToString() + $"[Board {board + 1}] StartMarking({markMode}) returned {rc}"
                        });
                        return;
                    }

                    m_bCmdPreviewing[board] = true;
                    sbLog.AppendLine($"[Board {board + 1}] StartMarking({markMode}) OK");

                    // 4. per-spec 監控 Timer：mode 3 計時 preview-time + 自動重啟；mode 4 等 IsMarking==0
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    int previewTimeMs = Math.Max(spec.PreviewTime, 1) * 1000;
                    // 白底 QR 密填較久 → 5 分鐘；一般線段/QR → 60 秒（與 699d4ac 相同，卡住能較快恢復，
                    // 不會讓某板連續佔用 5 分鐘拖累連續打標）。
                    int markTimeoutMs = spec.QRWhiteBg ? 300000 : 60000;
                    long lastRestartMs = 0;
                    int idleTicks = 0;   // mode 4：連續讀到 IsMarking==0 的次數，達門檻才算真的打完
                    System.Windows.Forms.Timer monitor = new System.Windows.Forms.Timer { Interval = 100 };
                    int capturedBoard = board;
                    int capturedMode = markMode;
                    monitor.Tick += (mts, mte) =>
                    {
                        try
                        {
                            long elapsed = sw.ElapsedMilliseconds;
                            if (capturedMode == 3)
                            {
                                if (elapsed >= previewTimeMs)
                                {
                                    monitor.Stop();
                                    monitor.Dispose();
                                    try { m_MMMark[capturedBoard].StopMarking(); } catch { }
                                    // 預覽收尾也做 MarkShutdown，徹底清掉「still previewing」狀態，
                                    // 避免下一命令 StartMarking 回 6。
                                    try { m_MMMark[capturedBoard].MarkShutdown(); } catch { }
                                    m_bCmdPreviewing[capturedBoard] = false;
                                    sbLog.AppendLine($"[Board {capturedBoard + 1}] preview done @{elapsed}ms");
                                    tcs.SetResult(new DaemonSpecResult { ExitCode = 0, Logs = sbLog.ToString() });
                                    return;
                                }
                                long sinceRestart = elapsed - lastRestartMs;
                                if (sinceRestart >= 1000
                                    && m_MMMark[capturedBoard].IsMarking() == 0
                                    && elapsed < previewTimeMs - 500)
                                {
                                    m_MMMark[capturedBoard].MarkStandBy();
                                    Application.DoEvents();
                                    m_MMMark[capturedBoard].StartMarking(3);
                                    lastRestartMs = elapsed;
                                }
                            }
                            else
                            {
                                long im = 0;
                                try { im = m_MMMark[capturedBoard].IsMarking(); } catch { }
                                // 需「開打 300ms 後、且連續 2 次讀到 0」才算真的完成。
                                // 否則 IsMarking 一次瞬間誤報 0 就提早回完成 → client 立刻發下一條，
                                // 在本板還在打時 ResetFile 破壞正在跑的工作 → 下一條 StartMarking 回 6。
                                if (elapsed >= 300 && im == 0)
                                {
                                    idleTicks++;
                                    if (idleTicks >= 2)
                                    {
                                        monitor.Stop();
                                        monitor.Dispose();
                                        try { m_MMMark[capturedBoard].MarkShutdown(); } catch { }
                                        m_bCmdPreviewing[capturedBoard] = false;
                                        sbLog.AppendLine($"[Board {capturedBoard + 1}] mark done @{elapsed}ms");
                                        tcs.SetResult(new DaemonSpecResult { ExitCode = 0, Logs = sbLog.ToString() });
                                        return;
                                    }
                                }
                                else
                                {
                                    idleTicks = 0;   // 還在打（im!=0）或還沒過緩衝 → 重新計數
                                }
                                if (elapsed > markTimeoutMs)
                                {
                                    monitor.Stop();
                                    monitor.Dispose();
                                    try { m_MMMark[capturedBoard].StopMarking(); } catch { }
                                    try { m_MMMark[capturedBoard].MarkShutdown(); } catch { }
                                    m_bCmdPreviewing[capturedBoard] = false;
                                    sbLog.AppendLine($"[Board {capturedBoard + 1}] mark timeout @{elapsed}ms");
                                    tcs.SetResult(new DaemonSpecResult { ExitCode = 3, Logs = sbLog.ToString() });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            monitor.Stop();
                            try { monitor.Dispose(); } catch { }
                            m_bCmdPreviewing[capturedBoard] = false;
                            tcs.SetResult(new DaemonSpecResult { ExitCode = 1, Logs = sbLog.ToString() + $"monitor exception: {ex.Message}" });
                        }
                    };
                    monitor.Start();
                }
                catch (Exception ex)
                {
                    if (board >= 0 && board < m_bCmdPreviewing.Length)
                        m_bCmdPreviewing[board] = false;
                    tcs.SetResult(new DaemonSpecResult { ExitCode = 1, Logs = sbLog.ToString() + $"exception: {ex.Message}" });
                }
            }));

            return tcs.Task;
        }

        public class DaemonSpecResult
        {
            public int ExitCode;
            public string Logs;
        }

        /// <summary>
        /// 從 UI 讀取工作區參數
        /// </summary>
        private void ReadWorkspaceSettings()
        {
            if (double.TryParse(txtWorkspace.Text.Trim(), out double ws) && ws > 0)
            {
                m_WorkspaceSize = ws;
            }

            if (double.TryParse(txtWorkspaceHeight.Text.Trim(), out double wh) && wh > 0)
            {
                m_WorkspaceHeight = wh;
            }

            if (double.TryParse(txtMargin.Text.Trim(), out double mg) && mg > 0 && mg <= 100)
            {
                m_MarginPercent = mg / 100.0;
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if (m_bInit)
            {
                MessageBox.Show("已經初始化過了！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 檢查是否還有 MM27Dx64.exe 在背景執行，若有則強制結束
            // 這是為了避免與 MarkingMate 主程式衝突，導致初始化失敗
            try
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("MM27Dx64");
                if (processes.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"發現 {processes.Length} 個 MM27Dx64.exe 背景進程，正在結束...");

                    foreach (System.Diagnostics.Process proc in processes)
                    {
                        try
                        {
                            proc.Kill();
                            // proc.WaitForExit(1000); // 等待進程結束
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"結束 MM27Dx64.exe 失敗 (PID: {proc.Id}): {ex.Message}");
                        }
                    }

                    // 確保資源完全釋放
                    System.Threading.Thread.Sleep(500); // 縮短等待時間
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"檢查背景程序失敗: {ex.Message}");
            }

            // 讀取 UI 工作區參數
            ReadWorkspaceSettings();

            // 重要：清理可能存在的舊控件（防止殘留）
            CleanupOldControls();

            int successCount = 0;
            string failInfo = "";
            int boardCount = (int)numBoardCount.Value;

            // 在 InitialExt 之前部署並驗證 MM1~MMn 雷射頭設定
            if (!DeployAndValidateLaserHeadConfigs(boardCount, out string deployError))
            {
                MessageBox.Show(
                    "雷射頭設定部署 / 驗證失敗，無法初始化：\n\n" + deployError +
                    "\n請修正後重試（可在 UI 填入 IP 並按「儲存IP」，或以系統管理員身分執行本程式）。",
                    "初始化前檢查失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 重要：MultiMM SDK 要求逐個板建立、初始化，而非批次處理
            // 必須：建立板 0 → 初始化板 0 → 建立板 1 → 初始化板 1 → ...

            for (int i = 0; i < boardCount; i++)
            {
                try
                {
                    // 步驟 1：建立 MMMark 控件
                    m_MMMark[i] = new AxMMMarkx641();
                    m_MMMark[i].Left = 0;
                    m_MMMark[i].Top = 0;
                    m_MMMark[i].Width = m_Panels[i].Width;
                    m_MMMark[i].Height = m_Panels[i].Height;
                    m_Panels[i].Controls.Add(m_MMMark[i]);

                    Application.DoEvents();
                    // System.Threading.Thread.Sleep(100); // 減少不必要的等待

                    // 步驟 2：立即初始化 MMMark
                    m_MMMark[i].InitialExt(m_ConfigPaths[i]);
                    m_MMMark[i].SetDesktopCenter(0, 0);
                    m_MMMark[i].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
                    m_MMMark[i].SetActiveDB(0);
                    m_MMMark[i].MarkStandBy();
                    m_MMMark[i].SetCurEditFun(2);
                    // 不再自動覆寫鏡頭 X 反向設定，尊重 MarkingMate GUI 中各板手動校正的鏡頭設定
                    m_MMMark[i].Redraw();

                    m_bBoardInit[i] = true;

                    Application.DoEvents();
                    // System.Threading.Thread.Sleep(200); // 減少初始化後的長時間等待

                    // 步驟 3：建立並初始化 MMEdit
                    m_MMEdit[i] = new AxMMEditx641();
                    this.Controls.Add(m_MMEdit[i]);
                    m_MMEdit[i].Visible = false;

                    Application.DoEvents();
                    // System.Threading.Thread.Sleep(100);

                    m_MMEdit[i].InitialExt(m_ConfigPaths[i]);

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50); // 保留極短的等待確保穩定

                    successCount++;

                    System.Diagnostics.Debug.WriteLine($"晶片板 {i + 1} 初始化成功");
                }
                catch (Exception ex)
                {
                    m_bBoardInit[i] = false;
                    failInfo += $"晶片板 {i + 1}：{ex.Message}\n";

                    System.Diagnostics.Debug.WriteLine($"晶片板 {i + 1} 初始化失敗：{ex.Message}");

                    // 如果第一個板失敗，後續的板也會失敗，直接中斷
                    if (i == 0)
                    {
                        MessageBox.Show($"第一個晶片板初始化失敗，無法繼續！\n\n錯誤：{ex.Message}\n\n" +
                            "提示：\n" +
                            "1. 確認 MarkingMate SDK 已安裝\n" +
                            "2. 確認硬體已連接\n" +
                            "3. 嘗試將板數量設為 1\n" +
                            "4. 先用 MarkingMate 軟體測試連接",
                            "初始化失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 如果不是第一個板失敗，記錄錯誤但繼續
                    // 這樣至少可以使用已成功的板
                }
            }

            m_bInit = successCount > 0;

            if (successCount == boardCount)
            {
                MessageBox.Show($"{successCount} 個晶片板全部初始化完成！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (successCount > 0)
            {
                MessageBox.Show($"已成功初始化 {successCount}/{boardCount} 個晶片板。\n\n以下晶片板初始化失敗：\n{failInfo}",
                    "部分初始化", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show($"所有晶片板初始化失敗！\n\n{failInfo}", "初始化失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_iCurrentBoard = comboBoard.SelectedIndex;

                if (!m_bBoardInit[m_iCurrentBoard])
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double x1 = double.Parse(txtX1.Text);
                double y1 = double.Parse(txtY1.Text);
                double x2 = double.Parse(txtX2.Text);
                double y2 = double.Parse(txtY2.Text);

                // 使用多系統 MMEdit 的 AddLine 方法
                int result = m_MMEdit[m_iCurrentBoard].AddLine(x1, y1, x2, y2, "", "");

                if (result == 0)
                {
                    m_MMMark[m_iCurrentBoard].Redraw();
                    MessageBox.Show($"已在晶片板 {m_iCurrentBoard + 1} 繪製線段！\n起點({x1}, {y1}) -> 終點({x2}, {y2})",
                        "繪製成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"繪製線段失敗！錯誤碼：{result}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"繪製線段失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_iCurrentBoard = comboBoard.SelectedIndex;

                if (!m_bBoardInit[m_iCurrentBoard])
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsBoardBusy(m_iCurrentBoard))
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                        "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 打標前自動套用雷射參數
                if (!ApplyLaserParamsFromUI(m_iCurrentBoard))
                    return;

                m_MMMark[m_iCurrentBoard].MarkStandBy();

                if (m_MMMark[m_iCurrentBoard].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 啟動 Timer 來監控打標狀態
                timerMark.Tag = m_iCurrentBoard;
                timerMark.Start();

                btnMark.Enabled = false;
                btnPreviewManual.Enabled = false;
                btnStop.Enabled = true;
                // 鎖住其他頁籤的預覽 / 打標按鈕避免衝突
                btnPreviewDXF.Enabled = false;
                btnMarkDXF.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnMarkQR.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 手動繪圖：紅光預覽（不打雷射，跑全路徑）
        /// </summary>
        private void btnPreviewManual_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int boardIndex = comboBoard.SelectedIndex;

                if (!m_bBoardInit[boardIndex])
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsBoardBusy(boardIndex))
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                        "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 設定預覽模式（全路徑預覽）並啟動紅光預覽
                m_MMMark[boardIndex].SetPreviewMode(2);
                m_MMMark[boardIndex].MarkStandBy();
                Application.DoEvents();

                if (m_MMMark[boardIndex].StartMarking(3) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 預覽啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_bPreviewing = true;
                m_iPreviewBoard = boardIndex;

                // 啟動 15 秒自動關閉 Timer
                timerPreview.Stop();
                timerPreview.Start();

                // 停用按鈕，防止重複操作
                btnMark.Enabled = false;
                btnPreviewManual.Enabled = false;
                btnStopPreviewManual.Enabled = true;
                btnStop.Enabled = true;
                // 停用 DXF 頁籤
                btnMarkDXF.Enabled = false;
                btnPreviewDXF.Enabled = false;
                btnLoadDXF.Enabled = false;
                btnLoadDXFFile.Enabled = false;
                // 停用 QR 頁籤
                btnMarkQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnLoadQR.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 手動繪圖：停止紅光預覽
        /// </summary>
        private void btnStopPreviewManual_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            try
            {
                timerPreview.Stop();

                int boardIndex = (m_iPreviewBoard >= 0) ? m_iPreviewBoard : comboBoard.SelectedIndex;
                if (boardIndex >= 0 && boardIndex < m_bBoardInit.Length && m_bBoardInit[boardIndex])
                {
                    m_MMMark[boardIndex].StopMarking();
                }
                m_bPreviewing = false;
                m_iPreviewBoard = -1;

                ResetPreviewButtonsAfterStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_iCurrentBoard = comboBoard.SelectedIndex;

                if (!m_bBoardInit[m_iCurrentBoard])
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_MMMark[m_iCurrentBoard].StopMarking();
                timerMark.Stop();
                timerPreview.Stop();
                m_bPreviewing = false;
                m_iPreviewBoard = -1;

                // 恢復所有按鈕狀態（含 QR / 預覽）
                ResetPreviewButtonsAfterStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerMark_Tick(object sender, EventArgs e)
        {
            // 取得目前操作的板索引
            int boardIndex = (int)timerMark.Tag;

            // 預覽模式不使用 timerMark（IsMarking 在預覽模式下回傳不正確）
            // 此 timer 僅用於正常打標模式
            if (m_bPreviewing)
            {
                timerMark.Stop();
                return;
            }

            // 檢查打標是否已完成 (IsMarking 回傳 0 表示已停止)
            if (m_MMMark[boardIndex].IsMarking() == 0)
            {
                // 停止計時器
                timerMark.Stop();

                // 關閉打標引擎
                m_MMMark[boardIndex].MarkShutdown();

                // 恢復 UI 按鈕狀態
                btnMark.Enabled = true;
                btnMarkDXF.Enabled = true;
                btnStopMarkDXF.Enabled = false;
                btnPreviewDXF.Enabled = true;
                btnLoadDXF.Enabled = true;
                btnLoadDXFFile.Enabled = true;
                btnClearDXF.Enabled = true;
                btnStop.Enabled = false;
                // QR Code 頁籤按鈕
                btnMarkQR.Enabled = true;
                btnStopMarkQR.Enabled = false;
                btnLoadQR.Enabled = true;
                btnPreviewQR.Enabled = true;
                btnClearQR.Enabled = true;
                btnQRWhiteBgMark.Enabled = true;
                btnQRWhiteBgPreview.Enabled = true;
                btnCLIExecuteMark.Enabled = true;

                MessageBox.Show($"晶片板 {boardIndex + 1} 完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string statusInfo = "";

            for (int i = 0; i < 4; i++)
            {
                string ip = m_txtIPs[i].Text.Trim();
                string ipStr = string.IsNullOrEmpty(ip) ? "未設定" : ip;

                if (!m_bBoardInit[i])
                {
                    statusInfo += $"晶片板 {i + 1} (IP: {ipStr})：未初始化\n";
                    continue;
                }

                try
                {
                    long cardConnect = m_MMMark[i].IsCardConnect();
                    long headStatus = m_MMMark[i].GetHeadStatus(0);

                    string connectStr = cardConnect != 0 ? "已連接" : "未連接";
                    string headStr = headStatus != 0 ? "正常" : "異常";

                    statusInfo += $"晶片板 {i + 1} (IP: {ipStr})：控制卡 {connectStr}，掃描頭 {headStr}\n";
                }
                catch (Exception ex)
                {
                    statusInfo += $"晶片板 {i + 1} (IP: {ipStr})：查詢失敗 - {ex.Message}\n";
                }
            }

            MessageBox.Show(statusInfo, "連接狀態", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadIPSettings()
        {
            // 從主 IP 表讀 DEV0~DEV3 → 對應 MM1~MM4 的 UI 欄位
            string masterIp = ResolveProjectFile(MasterIPRelativePath);
            for (int i = 0; i < 4; i++)
            {
                m_txtIPs[i].Text = (masterIp != null) ? ReadDevFromIni(masterIp, i) : "";
            }
        }

        private static string ReadDevFromIni(string iniPath, int devIndex)
        {
            if (!File.Exists(iniPath))
                return "";

            string key = $"DEV{devIndex}=";
            foreach (string line in File.ReadAllLines(iniPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed.Substring(key.Length).Trim();
                }
            }
            return "";
        }

        private void btnReadIP_Click(object sender, EventArgs e)
        {
            LoadIPSettings();
            MessageBox.Show("已從 DevIPAddress.ini 讀取 IP 設定。", "讀取IP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSaveIP_Click(object sender, EventArgs e)
        {
            // 寫入主 IP 表（單一來源），同時把 DEV0~3 同步至 4 支 EMC6_MMx 的 DEV0
            // 固定 CARD 對應：MM1→CARD0, MM2→CARD1, MM3→CARD2, MM4→CARD3
            string masterIp = ResolveProjectFile(MasterIPRelativePath);
            if (masterIp == null)
            {
                MessageBox.Show($"找不到主 IP 表：{MasterIPRelativePath}\n請先確認檔案存在。",
                    "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string errorInfo = "";

            // 1. 寫主表
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    WriteDevToIni(masterIp, i, m_txtIPs[i].Text.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"寫入主 IP 表失敗：{ex.Message}", "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. 同步至各 EMC6_MMx 的 DEV0
            int syncedCount = 0;
            for (int i = 0; i < 4; i++)
            {
                string src = ResolveProjectFile(m_IPConfigRelativePaths[i]);
                if (src == null)
                {
                    errorInfo += $"MM{i + 1}：找不到專案 IP 檔（{m_IPConfigRelativePaths[i]}）\n";
                    continue;
                }
                try
                {
                    WriteDevToIni(src, 0, m_txtIPs[i].Text.Trim());
                    syncedCount++;
                }
                catch (Exception ex)
                {
                    errorInfo += $"MM{i + 1}：{ex.Message}\n";
                }
            }

            if (syncedCount == 4)
            {
                MessageBox.Show("已寫入主 IP 表，並同步至 4 支 EMC6_MMx 的 DEV0。\n\n下次按「初始化」會自動部署到 MarkingMate 安裝目錄並生效。",
                    "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"主表已寫入；MMx 同步 {syncedCount}/4 組。\n\n問題：\n{errorInfo}",
                    "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void WriteDevToIni(string iniPath, int devIndex, string value)
        {
            if (!File.Exists(iniPath))
                throw new FileNotFoundException($"找不到設定檔：{iniPath}");

            string key = $"DEV{devIndex}=";
            string[] lines = File.ReadAllLines(iniPath);
            bool written = false;
            for (int j = 0; j < lines.Length; j++)
            {
                if (lines[j].Trim().StartsWith(key, StringComparison.OrdinalIgnoreCase))
                {
                    lines[j] = $"DEV{devIndex}={value}";
                    written = true;
                    break;
                }
            }
            if (!written)
                throw new InvalidOperationException($"{iniPath} 找不到 DEV{devIndex}= 條目");
            File.WriteAllLines(iniPath, lines);
        }

        // -----------------------------------------------------------------
        // 雷射頭設定部署與驗證（MM1~MM4）
        // -----------------------------------------------------------------

        /// <summary>
        /// 從 exe 所在資料夾起向上搜尋專案來源檔；用於 dev (bin\x64\Debug) 與部署 (exe 同層) 兩種情境。
        /// </summary>
        private static string ResolveProjectFile(string relativePath)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string direct = Path.Combine(baseDir, relativePath);
            if (File.Exists(direct)) return direct;

            var dir = new DirectoryInfo(baseDir);
            for (int i = 0; i < 6 && dir != null; i++)
            {
                string candidate = Path.Combine(dir.FullName, relativePath);
                if (File.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
            return null;
        }

        /// <summary>
        /// 同上但找的是目錄。
        /// </summary>
        private static string ResolveProjectDir(string relativePath)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string direct = Path.Combine(baseDir, relativePath);
            if (Directory.Exists(direct)) return direct;

            var dir = new DirectoryInfo(baseDir);
            for (int i = 0; i < 6 && dir != null; i++)
            {
                string candidate = Path.Combine(dir.FullName, relativePath);
                if (Directory.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
            return null;
        }

        // 部署統計：用於 skip-if-same 機制下追蹤實際寫入 / 跳過數量
        private class DeployStats
        {
            public int Written;
            public int Skipped;
        }

        /// <summary>
        /// 比較兩個檔案內容是否完全相同（用於 skip-if-same 部署機制）。
        /// 流程：dest 不存在 → false；size 不同 → false；逐 byte 比較。
        /// </summary>
        private static bool FilesAreIdentical(string srcPath, string destPath)
        {
            try
            {
                if (!File.Exists(destPath)) return false;
                var srcInfo = new FileInfo(srcPath);
                var destInfo = new FileInfo(destPath);
                if (srcInfo.Length != destInfo.Length) return false;
                if (srcInfo.Length == 0) return true;

                const int bufSize = 64 * 1024;
                var b1 = new byte[bufSize];
                var b2 = new byte[bufSize];
                using (var s1 = File.OpenRead(srcPath))
                using (var s2 = File.OpenRead(destPath))
                {
                    while (true)
                    {
                        int r1 = ReadFully(s1, b1, bufSize);
                        int r2 = ReadFully(s2, b2, bufSize);
                        if (r1 != r2) return false;
                        if (r1 == 0) return true;
                        for (int i = 0; i < r1; i++)
                            if (b1[i] != b2[i]) return false;
                    }
                }
            }
            catch
            {
                // 任何 IO 例外都當作「不確定相同」，讓後續嘗試寫入
                return false;
            }
        }

        private static int ReadFully(Stream s, byte[] buf, int count)
        {
            int total = 0;
            while (total < count)
            {
                int r = s.Read(buf, total, count - total);
                if (r == 0) break;
                total += r;
            }
            return total;
        }

        /// <summary>
        /// 複製單檔到目標路徑；若目標已存在且內容相同，跳過寫入並回傳 false。
        /// </summary>
        /// <returns>true=實際寫入；false=跳過（內容相同）</returns>
        private static bool CopyWithBackup(string src, string dest)
        {
            if (FilesAreIdentical(src, dest)) return false;

            string destDir = Path.GetDirectoryName(dest);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            if (File.Exists(dest))
            {
                try { File.Copy(dest, dest + ".bak", overwrite: true); }
                catch { /* 備份失敗不阻斷部署 */ }
            }
            File.Copy(src, dest, overwrite: true);
            return true;
        }

        private static void DeployCfgDirectory(string srcDir, string destDir, DeployStats stats)
        {
            if (!Directory.Exists(srcDir)) return;

            foreach (string srcFile in Directory.GetFiles(srcDir, "*.cfg"))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(srcFile));
                if (FilesAreIdentical(srcFile, destFile))
                {
                    stats.Skipped++;
                    continue;
                }
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                File.Copy(srcFile, destFile, overwrite: true);
                stats.Written++;
            }
        }

        /// <summary>
        /// 遞迴複製整個目錄（用於部署 EMC6 共用驅動目錄）。已存在且內容相同的檔案會跳過。
        /// </summary>
        private static void CopyDirectoryRecursive(string srcDir, string destDir, DeployStats stats)
        {
            if (!Directory.Exists(srcDir)) return;

            foreach (string srcFile in Directory.GetFiles(srcDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(srcFile));
                if (FilesAreIdentical(srcFile, destFile))
                {
                    stats.Skipped++;
                    continue;
                }
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                File.Copy(srcFile, destFile, overwrite: true);
                stats.Written++;
            }

            foreach (string subDir in Directory.GetDirectories(srcDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                CopyDirectoryRecursive(subDir, destSubDir, stats);
            }
        }

        /// <summary>
        /// 在 InitialExt 之前，依「主 IP 表」驗證並把專案設定部署到 MarkingMate 安裝目錄。
        /// 流程：
        ///   1. 從 Drivers\EMC6\DevIPAddress.ini 讀 DEV0~DEV(boardCount-1)，全部須非空
        ///   2. 依固定 CARD 對應（MM1→0, MM2→1, MM3→2, MM4→3）把主表 IP 同步到各 EMC6_MMx\DevIPAddress.ini 的 DEV0
        ///   3. 整個 Drivers\EMC6\ 遞迴部署到 MarkingMate（含主表、驅動、cfg）
        ///   4. 共用 config\config.ini 部署到 MarkingMate\config.ini
        ///   5. 各 MMx 的 DevIPAddress.ini / config_MMx.ini / cfg\*.cfg 部署到對應位置
        /// </summary>
        /// <param name="boardCount">要啟用的雷射頭數（1~4）</param>
        /// <param name="errorInfo">回傳問題明細；空字串代表全部 OK</param>
        /// <returns>true=可繼續初始化；false=需中止</returns>
        private bool DeployAndValidateLaserHeadConfigs(int boardCount, out string errorInfo)
        {
            var sb = new StringBuilder();
            var stats = new DeployStats();

            // === 1. 主 IP 表（單一來源）===
            string srcMasterIp = ResolveProjectFile(MasterIPRelativePath);
            if (srcMasterIp == null)
            {
                errorInfo = $"找不到主 IP 表：{MasterIPRelativePath}";
                return false;
            }

            string[] cardIps = new string[boardCount];
            for (int i = 0; i < boardCount; i++)
            {
                cardIps[i] = ReadDevFromIni(srcMasterIp, i);
                if (string.IsNullOrWhiteSpace(cardIps[i]))
                {
                    sb.AppendLine($"主 IP 表 DEV{i} 為空（對應 MM{i + 1} / CARD{i}），請於 UI 填入 IP 後按「儲存IP」：{srcMasterIp}");
                }
            }
            if (sb.Length > 0)
            {
                errorInfo = sb.ToString();
                return false;
            }

            // === 2. 主表 → 各 EMC6_MMx\DEV0 同步（寫專案內，不會碰 Program Files 權限）===
            for (int i = 0; i < boardCount; i++)
            {
                string mmxSrcIp = ResolveProjectFile(m_IPConfigRelativePaths[i]);
                if (mmxSrcIp == null)
                {
                    sb.AppendLine($"[MM{i + 1}] 找不到專案 IP 來源檔：{m_IPConfigRelativePaths[i]}");
                    continue;
                }
                try
                {
                    if (!string.Equals(ReadDevFromIni(mmxSrcIp, 0), cardIps[i], StringComparison.Ordinal))
                    {
                        WriteDevToIni(mmxSrcIp, 0, cardIps[i]);
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"[MM{i + 1}] 主表→DEV0 同步失敗：{ex.Message}");
                }
            }

            // === 3. EMC6 共用目錄（含主表、驅動、cfg）整個遞迴部署 ===
            string srcSharedDir = ResolveProjectDir(SharedDriverDirRelativePath);
            string destSharedDir = Path.Combine(MarkingMateRoot, SharedDriverDirRelativePath);
            if (srcSharedDir == null)
            {
                sb.AppendLine($"找不到專案 EMC6 共用目錄：{SharedDriverDirRelativePath}");
            }
            else
            {
                try
                {
                    CopyDirectoryRecursive(srcSharedDir, destSharedDir, stats);
                }
                catch (UnauthorizedAccessException)
                {
                    AppendPermissionError(sb, "部署 EMC6 共用目錄");
                    errorInfo = sb.ToString();
                    return false;
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"部署 EMC6 共用目錄失敗：{ex.Message}");
                }
            }

            // === 4. 共用 config.ini ===
            string srcSharedConfig = ResolveProjectFile(SharedConfigRelativePath);
            if (srcSharedConfig != null)
            {
                string destSharedConfig = Path.Combine(MarkingMateRoot, "config.ini");
                try
                {
                    if (CopyWithBackup(srcSharedConfig, destSharedConfig)) stats.Written++;
                    else stats.Skipped++;
                }
                catch (UnauthorizedAccessException)
                {
                    AppendPermissionError(sb, "部署共用 config.ini");
                    errorInfo = sb.ToString();
                    return false;
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"部署共用 config.ini 失敗：{ex.Message}");
                }
            }

            // === 5. 各 MMx 設定 ===
            for (int i = 0; i < boardCount; i++)
            {
                string mmName = $"MM{i + 1}";

                string srcIp = ResolveProjectFile(m_IPConfigRelativePaths[i]);
                string srcCfg = ResolveProjectFile(m_LaserConfigRelativePaths[i]); // 可選

                if (srcIp == null)
                {
                    sb.AppendLine($"[{mmName}] 找不到專案 IP 來源檔：{m_IPConfigRelativePaths[i]}");
                    continue;
                }

                string destIp = Path.Combine(MarkingMateRoot, "Drivers", $"EMC6_{mmName}", "DevIPAddress.ini");
                string destCfg = Path.Combine(MarkingMateRoot, $"config_{mmName}.ini");

                // 雷射頭 cfg 目錄（含 DEFAULT CARD、MultiCard、MultiSYN* 等多卡同步參數）
                string srcCfgDir = Path.Combine(Path.GetDirectoryName(srcIp), "cfg");
                string destCfgDir = Path.Combine(MarkingMateRoot, "Drivers", $"EMC6_{mmName}", "cfg");

                try
                {
                    if (CopyWithBackup(srcIp, destIp)) stats.Written++; else stats.Skipped++;
                    if (srcCfg != null)
                    {
                        if (CopyWithBackup(srcCfg, destCfg)) stats.Written++; else stats.Skipped++;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"[{mmName}] 未提供專案 config_{mmName}.ini，跳過部署（將使用共用 config.ini）");
                    }
                    DeployCfgDirectory(srcCfgDir, destCfgDir, stats);
                }
                catch (UnauthorizedAccessException)
                {
                    AppendPermissionError(sb, $"[{mmName}] 部署");
                    break; // 後續皆會失敗，提早結束
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"[{mmName}] 部署失敗：{ex.Message}");
                }
            }

            System.Diagnostics.Debug.WriteLine(
                $"[Deploy] 完成：寫入 {stats.Written} 檔、跳過 {stats.Skipped} 檔（內容相同）");

            errorInfo = sb.ToString();
            return errorInfo.Length == 0;
        }

        /// <summary>
        /// 統一的權限不足錯誤訊息。在 CLI 模式下，會額外建議「先用 GUI 模式（系統管理員）部署一次」。
        /// </summary>
        private void AppendPermissionError(StringBuilder sb, string actionLabel)
        {
            sb.AppendLine($"{actionLabel}失敗：寫入 {MarkingMateRoot} 權限不足。");
            if (m_IsAutoMode)
            {
                sb.AppendLine("提示：偵測到設定檔與安裝目錄內容不同，CLI（非管理員）無法寫入。");
                sb.AppendLine("解法 A：以「系統管理員」身分開啟 cmd / PowerShell 後再執行此 CLI；");
                sb.AppendLine("解法 B：先以系統管理員身分執行 GUI 模式按一次「初始化」完成同步，之後設定若未變動，CLI 可用一般權限執行。");
            }
            else
            {
                sb.AppendLine("請以系統管理員身分執行本程式。");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 執行自動模式
        /// </summary>
        private void ExecuteAutoMode()
        {
            try
            {
                // 檢查是否還有 MM27Dx64.exe 在背景執行，若有則強制結束
                // 注意：若有其他 MarkingMateMulti 實例正在執行（多 process 並行模式），則跳過 kill
                // 以免殺掉其他實例正在使用的驅動程序
                try
                {
                    if (HasOtherMarkingMateInstance())
                    {
                        System.Diagnostics.Debug.WriteLine("偵測到其他 MarkingMateMulti 實例執行中，跳過 MM27Dx64 清理以避免干擾其他實例");
                    }
                    else
                    {
                        System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("MM27Dx64");
                        if (processes.Length > 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"發現 {processes.Length} 個 MM27Dx64.exe 背景進程，正在結束...");
                            foreach (System.Diagnostics.Process proc in processes)
                            {
                                try
                                {
                                    proc.Kill();
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"結束 MM27Dx64.exe 失敗 (PID: {proc.Id}): {ex.Message}");
                                }
                            }
                            System.Threading.Thread.Sleep(500); // 確保資源釋放
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"檢查背景程序失敗: {ex.Message}");
                }

                // 步驟 1: 初始化指定的板
                if (!InitializeBoardAuto(m_AutoModeArgs.BoardIndex, m_AutoModeArgs.ConfigPath))
                {
                    // 初始化失敗：詳細錯誤已由 InitializeBoardAuto 內部輸出到 stderr
                    Console.Error.WriteLine("Error: Failed to initialize board.");
                    ExitCode = 1;
                    this.Close();
                    return;
                }

                // Debug: 輸出解析後的參數
                Console.WriteLine($"[AutoMode] DxfPath={m_AutoModeArgs.DxfPath ?? "(null)"}, Lines={m_AutoModeArgs.Lines?.Count ?? 0}, QRContent={m_AutoModeArgs.QRContent ?? "(null)"}, QRSize={m_AutoModeArgs.QRWidth}x{m_AutoModeArgs.QRHeight}, QRInvert={m_AutoModeArgs.QRInvert}");

                // 步驟 2: 載入 DXF 檔案或繪製手動線段
                bool hasContent = false;
                if (!string.IsNullOrEmpty(m_AutoModeArgs.DxfPath))
                {
                    if (!LoadDxfAuto(m_AutoModeArgs.BoardIndex, m_AutoModeArgs.DxfPath))
                    {
                        Console.Error.WriteLine("Error: Failed to load DXF.");
                        ExitCode = 2; // DXF 載入失敗
                        this.Close();
                        return;
                    }
                    hasContent = true;
                }
                else if (m_AutoModeArgs.Lines != null && m_AutoModeArgs.Lines.Count > 0)
                {
                    // 先計算所有線段的範圍，然後進行整體自動置中（類似 DXF 載入行為）
                    double minX = double.MaxValue, maxX = double.MinValue;
                    double minY = double.MaxValue, maxY = double.MinValue;

                    foreach (var line in m_AutoModeArgs.Lines)
                    {
                        minX = Math.Min(minX, Math.Min(line.X1, line.X2));
                        maxX = Math.Max(maxX, Math.Max(line.X1, line.X2));
                        minY = Math.Min(minY, Math.Min(line.Y1, line.Y2));
                        maxY = Math.Max(maxY, Math.Max(line.Y1, line.Y2));
                    }

                    double centerX = (minX + maxX) / 2.0;
                    double centerY = (minY + maxY) / 2.0;

                    // 計算偏移量，使圖形中心對齊工作區原點 (0,0)
                    double offsetX = -centerX;
                    double offsetY = -centerY;

                    Console.WriteLine($"Auto-Centering Lines: Center=({centerX:F2}, {centerY:F2}) Offset=({offsetX:F2}, {offsetY:F2})");

                    foreach (var line in m_AutoModeArgs.Lines)
                    {
                        // 建立平移後的線段版本（不直接修改原始物件，避免副作用）
                        var centeredLine = new LineSegment(
                            line.X1 + offsetX,
                            line.Y1 + offsetY,
                            line.X2 + offsetX,
                            line.Y2 + offsetY
                        );

                        if (!DrawLineAuto(m_AutoModeArgs.BoardIndex, centeredLine))
                        {
                            Console.Error.WriteLine("Error: Failed to draw line.");
                            ExitCode = 2; // 繪圖失敗
                            this.Close();
                            return;
                        }
                    }

                    // 所有線段繪製完成後，確保全部載入
                    m_MMMark[m_AutoModeArgs.BoardIndex].Redraw();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(300); // 等待繪圖完成
                    System.Diagnostics.Debug.WriteLine($"已繪製 {m_AutoModeArgs.Lines.Count} 條線段");
                    hasContent = true;
                }
                else if (!string.IsNullOrEmpty(m_AutoModeArgs.QRContent))
                {
                    bool qrOk;
                    if (m_AutoModeArgs.QRWhiteBg)
                        qrOk = BuildWhiteBgQR(m_AutoModeArgs.BoardIndex, MakeWhiteBgParams(m_AutoModeArgs));
                    else
                        qrOk = DrawQRCodeAuto(m_AutoModeArgs.BoardIndex, m_AutoModeArgs.QRContent,
                            m_AutoModeArgs.QRWidth, m_AutoModeArgs.QRHeight,
                            m_AutoModeArgs.QRInvert);
                    if (!qrOk)
                    {
                        Console.Error.WriteLine("Error: Failed to draw QR Code.");
                        ExitCode = 2;
                        this.Close();
                        return;
                    }
                    hasContent = true;
                }

                if (!hasContent)
                {
                    // 如果沒有內容，是否需要打標？
                    Console.WriteLine("Warning: No content to mark.");
                }

                // 步驟 2.5: 套用雷射參數（如有指定）。
                // 白底 QR 已在 BuildWhiteBgQR 內對各物件個別設好參數，這裡跳過以免覆寫。
                if (hasContent && !m_AutoModeArgs.QRWhiteBg && (m_AutoModeArgs.Power.HasValue || m_AutoModeArgs.Speed.HasValue ||
                    m_AutoModeArgs.Frequency.HasValue || m_AutoModeArgs.PulseWidth.HasValue ||
                    m_AutoModeArgs.MarkRepeat.HasValue || m_AutoModeArgs.WobbleWidth.HasValue))
                {
                    ApplyLaserParamsAuto(m_AutoModeArgs.BoardIndex);
                }

                // 步驟 3: 如果需要自動打標
                if (m_AutoModeArgs.AutoMark && hasContent)
                {
                    // 確保 MarkStandBy 狀態
                    // m_MMMark[m_AutoModeArgs.BoardIndex].MarkStandBy(); // InitializeAuto 已經設定過了

                    if (!ExecuteMarkingAuto(m_AutoModeArgs.BoardIndex))
                    {
                        Console.Error.WriteLine("Error: Marking failed.");
                        ExitCode = 3; // 打標失敗
                        this.Close();
                        return;
                    }
                }
                else
                {
                    // 如果不打標，可以選擇保持開啟，或是直接結束
                    // 如果是純測試 DXF 解析，可能希望看到結果
                }

                // 成功
                ExitCode = 0;
                // 注意：如果只是載入而不打標，可能希望使用者查看，所以不 Close？
                // 目前邏輯是如果執行了 AutoMark 就 Close，否則... 也 Close？
                // 原本邏輯是全都 Close
                this.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"自動模式執行失敗：{ex.Message}");
                Console.Error.WriteLine($"Error: Exception in AutoMode - {ex.Message}");
                ExitCode = 1;
                this.Close();
            }
        }

        /// <summary>
        /// 偵測是否有其他 MarkingMate 實例（同 process name）正在執行。
        /// 用於 multi-process 並行模式：判斷是否要跳過會干擾他人的破壞性動作
        /// （如 MM27Dx64 kill、配置檔覆寫部署）。
        /// </summary>
        private static bool HasOtherMarkingMateInstance()
        {
            try
            {
                int currentPid = System.Diagnostics.Process.GetCurrentProcess().Id;
                string currentName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                var others = System.Diagnostics.Process.GetProcessesByName(currentName);
                foreach (var p in others)
                {
                    if (p.Id != currentPid) return true;
                }
            }
            catch { /* 查詢失敗就當作沒有 */ }
            return false;
        }

        /// <summary>
        /// Multi-process 模式：驗證已部署的主 IP 表存在且 DEV0~DEV(boardCount-1) 都已填值。
        /// 跳過實際檔案部署時用，避免覆寫其他 process 正在使用的設定。
        /// </summary>
        private bool ValidateDeployedMasterIpTable(int boardCount, out string errorInfo)
        {
            errorInfo = null;
            string deployedMasterIp = Path.Combine(MarkingMateRoot, MasterIPRelativePath);
            if (!File.Exists(deployedMasterIp))
            {
                errorInfo = $"已部署的主 IP 表不存在：{deployedMasterIp}\n（請先以單一 process 模式跑一次完成初次部署）";
                return false;
            }
            var sb = new StringBuilder();
            for (int i = 0; i < boardCount; i++)
            {
                string ip = ReadDevFromIni(deployedMasterIp, i);
                if (string.IsNullOrWhiteSpace(ip))
                {
                    sb.AppendLine($"已部署的主 IP 表 DEV{i} 為空（對應 MM{i + 1} / CARD{i}）：{deployedMasterIp}");
                }
            }
            if (sb.Length > 0)
            {
                errorInfo = sb.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 自動模式：初始化指定的板
        /// </summary>
        private bool InitializeBoardAuto(int boardIndex, string configPath)
        {
            try
            {
                int totalBoards = m_bBoardInit.Length;

                // 在 InitialExt 之前部署並驗證所有 MM1~MM{totalBoards} 雷射頭設定
                // 來源：主 IP 表 Drivers\EMC6\DevIPAddress.ini（DEV0~3 = MM1~4 對應 CARD0~3）
                // multi-process 並行：若已有其他 MarkingMate 實例在跑，跳過實際檔案部署，
                // 只驗證已部署的設定可用，避免覆寫對方正在使用的檔案。
                if (HasOtherMarkingMateInstance())
                {
                    if (!ValidateDeployedMasterIpTable(totalBoards, out string vErr))
                    {
                        Console.Error.WriteLine("Error: 跳過部署但驗證已部署設定失敗：");
                        Console.Error.WriteLine(vErr);
                        return false;
                    }
                    Console.WriteLine("[AutoMode] 偵測到其他 MarkingMate 實例，沿用已部署配置，跳過部署寫檔。");
                }
                else if (!DeployAndValidateLaserHeadConfigs(totalBoards, out string deployError))
                {
                    Console.Error.WriteLine("Error: 雷射頭設定部署 / 驗證失敗：");
                    Console.Error.WriteLine(deployError);
                    System.Diagnostics.Debug.WriteLine(
                        $"自動模式：雷射頭設定部署 / 驗證失敗，無法初始化：\n{deployError}");
                    return false;
                }

                // 一律 init 全部 4 塊板的 OCX：
                // - SDK 要求按序 init（要用 board N 必須先 init 0..N-1）
                // - 統一 init 全部，target 之外的板雖然這個 process 不會主動操作，
                //   但 SDK 需要他們存在；同 process 內若已 init 過則跳過。
                // target board 用傳入的 configPath（允許 --config 覆寫），其餘用預設。
                for (int i = 0; i < totalBoards; i++)
                {
                    if (m_bBoardInit[i]) continue;

                    string cfg = (i == boardIndex) ? configPath : m_ConfigPaths[i];

                    m_MMMark[i] = new AxMMMarkx641();
                    m_MMEdit[i] = new AxMMEditx641();

                    m_MMMark[i].Left = 0;
                    m_MMMark[i].Top = 0;
                    m_MMMark[i].Width = m_Panels[i].Width;
                    m_MMMark[i].Height = m_Panels[i].Height;

                    m_Panels[i].Controls.Add(m_MMMark[i]);
                    this.Controls.Add(m_MMEdit[i]);
                    m_MMEdit[i].Visible = false;

                    Application.DoEvents();

                    m_MMMark[i].InitialExt(cfg);
                    m_MMMark[i].SetDesktopCenter(0, 0);
                    m_MMMark[i].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
                    m_MMMark[i].SetActiveDB(0);
                    m_MMMark[i].MarkStandBy();
                    m_MMMark[i].SetCurEditFun(2);
                    // 不再自動覆寫鏡頭 X 反向設定，尊重 MarkingMate GUI 中各板手動校正的鏡頭設定
                    m_MMMark[i].Redraw();

                    m_bBoardInit[i] = true;

                    Application.DoEvents();

                    m_MMEdit[i].InitialExt(cfg);

                    string role = (i == boardIndex) ? "TARGET" : "prereq";
                    Console.WriteLine($"[Board {i + 1}] OCX 初始化完成 ({role})");
                }

                m_bInit = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: 初始化板 {boardIndex} 失敗：{ex.Message}");
                System.Diagnostics.Debug.WriteLine($"初始化板 {boardIndex} 失敗：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自動模式：繪製線段
        /// </summary>
        private bool DrawLineAuto(int boardIndex, LineSegment line)
        {
            try
            {
                // 使用 MMEdit 的 AddLine 方法新增線段
                // 座標轉換：MarkingMate 的原點 (0,0) 是鏡頭中心
                // CLI 傳入的座標如果是基於左上角(0,0)，需要轉換成中心為(0,0)
                // 假設 workspaceSize = 150，範圍是 [-75, 75]
                // 如果傳入的是 [0, 150]，則需要平移 -75

                double halfW = m_WorkspaceSize / 2.0;
                double halfH = m_WorkspaceHeight / 2.0;

                // 先套用一個簡單的平移修正：將輸入座標視為以左下角為(0,0)的絕對座標，轉換為以中心為(0,0)的相對座標
                // 但是！如果使用者已經提供了負座標（例如 -111, 50），這表示他們可能已經使用了中心原點座標
                // 我們應該檢查輸入座標的範圍
                bool isCenterBased = false;
                if (line.X1 < 0 || line.X2 < 0 || line.Y1 < 0 || line.Y2 < 0)
                {
                    isCenterBased = true;
                }

                double x1, y1, x2, y2;
                if (isCenterBased)
                {
                    // 已包含負數，假設已經是中心座標，不進行平移
                    x1 = line.X1;
                    y1 = line.Y1;
                    x2 = line.X2;
                    y2 = line.Y2;
                }
                else
                {
                    // 全正數，假設是 Corner 原點，進行平移（X 用半寬、Y 用半高）
                    x1 = line.X1 - halfW;
                    y1 = line.Y1 - halfH;
                    x2 = line.X2 - halfW;
                    y2 = line.Y2 - halfH;
                }

                // 改用轉換後的座標
                int result = m_MMEdit[boardIndex].AddLine(x1, y1, x2, y2, "", "");

                if (result == 0)
                {
                    // 只新增到資料庫，不立即 Redraw（批次繪製時效能更好）
                    // Redraw 會在所有線段繪製完成後統一執行
                    System.Diagnostics.Debug.WriteLine($"已繪製線段：{line}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"繪製線段失敗，錯誤碼：{result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"繪製線段失敗：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自動模式：繪製 QR Code（位置固定為鏡頭中心 0,0）
        /// </summary>
        private bool DrawQRCodeAuto(int boardIndex, string content, double width, double height, bool invert)
        {
            try
            {
                long result = m_MMMark[boardIndex].AddBarcode(
                    BARCODE_TYPE_QRCODE, content, 0, 0, width, height, "", "");

                if (result != 0)
                {
                    Console.Error.WriteLine($"Error: AddBarcode failed with code {result}");
                    return false;
                }

                Application.DoEvents();
                Thread.Sleep(100);

                ApplyQRECLevelLow(boardIndex);   // QR 固定容錯等級 LOW（EC=0）

                if (invert)
                {
                    ApplyQRInvert(boardIndex);
                }

                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(300);

                System.Diagnostics.Debug.WriteLine($"已繪製 QR Code: content={content}, size={width}x{height}, invert={invert}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"繪製 QR Code 失敗：{ex.Message}");
                Console.Error.WriteLine($"Error: DrawQRCode exception - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 對指定板剛 AddBarcode 加入的 QR 物件套用反相（黑白互換）。
        /// 必須在 AddBarcode 之後、Redraw 之前呼叫；以「列舉到的最後一個物件」當作剛加入的 QR。
        /// SDK API：m_MMEdit[i].SetBarcodeInvert(objName, 1)。SetBarcodeInvert 對 QR 安全（不同於 SetWobble）。
        /// 診斷輸出寫到 stderr（CLI/daemon log 都看得到）。
        /// </summary>
        private void ApplyQRInvert(int boardIndex)
        {
            try
            {
                m_MMMark[boardIndex].SelectAllObjects();
                long objCount = m_MMMark[boardIndex].SelectGetCount();
                if (objCount <= 0)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] ApplyQRInvert: objCount=0，沒物件可設");
                    return;
                }

                string qrObjName = "";
                m_MMMark[boardIndex].SelectEnum((int)(objCount - 1), ref qrObjName);
                if (string.IsNullOrEmpty(qrObjName))
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] ApplyQRInvert: SelectEnum 拿不到物件名");
                    return;
                }

                long setRc = m_MMEdit[boardIndex].SetBarcodeInvert(qrObjName, 1);
                Application.DoEvents();
                Thread.Sleep(30);
                long readBack = m_MMEdit[boardIndex].GetBarcodeInvert(qrObjName);
                Console.Error.WriteLine(
                    $"[Board {boardIndex + 1}] ApplyQRInvert obj=[{qrObjName}] " +
                    $"SetBarcodeInvert(1) rc={setRc} readback={readBack}" +
                    (setRc != 0 ? " ** SET FAILED **" : "") +
                    (readBack != 1 ? " ** READBACK MISMATCH **" : ""));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] ApplyQRInvert 例外：{ex.Message}");
                System.Diagnostics.Debug.WriteLine($"ApplyQRInvert 失敗（板 {boardIndex + 1}）：{ex.Message}");
            }
        }

        // QR 容錯等級（EC Level）：SDK Set2DBarcodeQRECLevel 定義 0=Low 1=Medium 2=Quartil 3=High。
        private const int QR_EC_LEVEL_LOW = 2;

        /// <summary>
        /// 對指定板剛 AddBarcode 加入的 QR 物件設定容錯等級為 LOW（EC Level=0）。
        /// 必須在 AddBarcode 之後、Redraw 之前呼叫；以「列舉到的最後一個物件」當作剛加入的 QR。
        /// SDK API：m_MMEdit[i].Set2DBarcodeQRECLevel(objName, 0)。
        /// 適用於「AddBarcode 後不再加入其他物件」的路徑（daemon / 模式 A / 命令提示）；
        /// GUI QR 頁面因為之後還會加矩形，改用已捕捉的 qrName 直接呼叫。
        /// 診斷輸出寫到 stderr（CLI/daemon log 都看得到）。
        /// </summary>
        private void ApplyQRECLevelLow(int boardIndex)
        {
            try
            {
                m_MMMark[boardIndex].SelectAllObjects();
                long objCount = m_MMMark[boardIndex].SelectGetCount();
                if (objCount <= 0)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] ApplyQRECLevelLow: objCount=0，沒物件可設");
                    return;
                }

                string qrObjName = "";
                m_MMMark[boardIndex].SelectEnum((int)(objCount - 1), ref qrObjName);
                if (string.IsNullOrEmpty(qrObjName))
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] ApplyQRECLevelLow: SelectEnum 拿不到物件名");
                    return;
                }

                long setRc = m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrObjName, QR_EC_LEVEL_LOW);
                Application.DoEvents();
                Thread.Sleep(30);
                long readBack = m_MMEdit[boardIndex].Get2DBarcodeQRECLevel(qrObjName);
                Console.Error.WriteLine(
                    $"[Board {boardIndex + 1}] ApplyQRECLevelLow obj=[{qrObjName}] " +
                    $"Set2DBarcodeQRECLevel({QR_EC_LEVEL_LOW}) rc={setRc} readback={readBack}" +
                    (setRc != 0 ? " ** SET FAILED **" : "") +
                    (readBack != QR_EC_LEVEL_LOW ? " ** READBACK MISMATCH **" : ""));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] ApplyQRECLevelLow 例外：{ex.Message}");
                System.Diagnostics.Debug.WriteLine($"ApplyQRECLevelLow 失敗（板 {boardIndex + 1}）：{ex.Message}");
            }
        }

        /// <summary>
        /// StartMarking，遇 rc==6（該板仍在 marking/previewing 的殘留狀態，多半是前一次
        /// 預覽 StopMarking 未完全生效）時，先 StopMarking + MarkShutdown 清乾淨再重試一次。
        /// 回傳最終 rc（0=成功）。診斷寫到 stderr（CLI/daemon log 可見）。
        /// </summary>
        private int StartMarkingWithRetry(int boardIndex, int mode)
        {
            int rc = m_MMMark[boardIndex].StartMarking(mode);
            if (rc == 6)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] StartMarking({mode}) returned 6 (still marking) → StopMarking 後重試");
                try { m_MMMark[boardIndex].StopMarking(); } catch { }
                try { m_MMMark[boardIndex].MarkShutdown(); } catch { }
                Application.DoEvents();
                Thread.Sleep(300);
                m_MMMark[boardIndex].MarkStandBy();
                Application.DoEvents();
                rc = m_MMMark[boardIndex].StartMarking(mode);
            }
            return rc;
        }

        // ============================================================
        // 白底反相 QR（白底矩形 + 反相 QR 雙圖層）— CLI / daemon 共用
        // 邏輯與 GUI btnQRWhiteBgMark_Click 的「全部」情況一致；
        // 這裡把可調值抽成 WhiteBgQRParams，其餘製程常數（頻率/脈寬/填滿間距/
        // 線段遍數等）維持與 GUI 相同的寫死值。若日後調 GUI 白底參數，這裡要同步。
        // ============================================================
        private class WhiteBgQRParams
        {
            public string Content = "1234567";
            public double QrWidth = 15.0;    // QR 資料模組區寬 (mm)
            public double QrHeight = 15.0;   // QR 資料模組區高 (mm)
            public int Border = 2;           // 外框單元數 (cell)
            public double QrSpeed = 1000;    // QR 打標速度
            public double QrPower = 80;      // QR 功率
            public double RectSpeed = 800;   // 白底矩形速度
            public double RectPower = 100;   // 白底矩形功率
            public double RectExtra = 0;     // 矩形額外加大量 (mm)
        }

        /// <summary>
        /// 依 CLI 參數組出白底 QR 參數：content 必填，其餘用寫死預設，
        /// 有明確帶 --qr-width/--qr-height/--qr-border/--power/--speed 才覆寫。
        /// （--power/--speed 覆寫的是 QR 圖層；矩形參數目前維持寫死。）
        /// </summary>
        private WhiteBgQRParams MakeWhiteBgParams(CommandLineArgs a)
        {
            var p = new WhiteBgQRParams { Content = a.QRContent };
            if (a.QRWidthExplicit) p.QrWidth = a.QRWidth;
            if (a.QRHeightExplicit) p.QrHeight = a.QRHeight;
            if (a.QRBorderExplicit) p.Border = a.QRBorder;
            if (a.Power.HasValue) p.QrPower = a.Power.Value;
            if (a.Speed.HasValue) p.QrSpeed = a.Speed.Value;
            return p;
        }

        /// <summary>
        /// 建立白底反相 QR（白底矩形 + 反相 QR），矩形排在 QR 前面先打標。
        /// 前提：呼叫端已 ResetFile + SetDesktopCenter/SetDesktopSize。
        /// 本方法只建立物件與設定，不呼叫 StartMarking（交給各呼叫端的打標流程）。
        /// 回傳 true=成功。診斷寫到 stderr（CLI/daemon log 可見）。
        /// </summary>
        private bool BuildWhiteBgQR(int boardIndex, WhiteBgQRParams p)
        {
            string qrName = "QRWhiteBg_QR";
            string rectName = "QRWhiteBg_Rect";
            double rectBaseW = p.QrWidth, rectBaseH = p.QrHeight;

            // ---- QR 物件 ----
            long rQR = m_MMMark[boardIndex].AddBarcode(
                BARCODE_TYPE_QRCODE, p.Content, 0, 0, p.QrWidth, p.QrHeight, "", qrName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg AddBarcode rc={rQR} content=\"{p.Content}\"");
            if (rQR != 0)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg 建立 QR 失敗 rc={rQR}");
                return false;
            }
            Application.DoEvents();
            Thread.Sleep(200);

            m_MMEdit[boardIndex].SetBarcodeInvert(qrName, 1);                       // 反相（白底反相 QR）
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrName, QR_EC_LEVEL_LOW);    // 容錯 LOW
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(qrName, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, 1, 1);           // 占位，稍後反推
            ApplyQRBorder(boardIndex, qrName, p.Border);                            // 外框單元 + 線段延伸
            m_MMEdit[boardIndex].SetBarcodeMarkStyle(qrName, 2);
            m_MMEdit[boardIndex].SetBarcodeLineType(qrName, 0);
            m_MMEdit[boardIndex].SetBarcodeSpotSize(qrName, 0.04);                  // 0.02 → 0.04
            m_MMEdit[boardIndex].SetBarcodeLineTimes(qrName, 4);                    // 1 → 4
            m_MMEdit[boardIndex].SetFillStartAngle(qrName, 0);
            m_MMEdit[boardIndex].SetFillStepAngle(qrName, 45);                      // 90 → 45
            m_MMEdit[boardIndex].SetBarcodeLineTwoway(qrName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(qrName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(qrName, p.QrSpeed);
            m_MMMark[boardIndex].SetPower(qrName, p.QrPower);
            m_MMMark[boardIndex].SetFrequency(qrName, 80);                          // 200 → 80
            m_MMMark[boardIndex].SetMarkRepeat(qrName, 1);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(qrName, 1000);
            m_MMMark[boardIndex].SetPulseWidth(qrName, 30);                         // 13 → 30

            // 反推 cellSize，讓 QR 資料區渲染 = UI 長寬
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(300);
            long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(qrName);
            if (qrVersion < 1) qrVersion = 1;
            int modules = 17 + 4 * (int)qrVersion;
            double cellW = p.QrWidth / modules;
            double cellH = p.QrHeight / modules;
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, cellW, cellH);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(300);

            double actualW = m_MMEdit[boardIndex].GetWidth(qrName);
            double actualH = m_MMEdit[boardIndex].GetHeight(qrName);
            if (actualW > 0) rectBaseW = actualW;
            if (actualH > 0) rectBaseH = actualH;
            Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg QR ver={qrVersion} modules={modules} cell={cellW:F4}x{cellH:F4} rendered={actualW}x{actualH}");

            // ---- 白底矩形（= QR 實際渲染尺寸 + RectExtra）----
            double rectW = rectBaseW + p.RectExtra;
            double rectH = rectBaseH + p.RectExtra;
            double rectHalfW = rectW / 2.0, rectHalfH = rectH / 2.0;
            long rR = m_MMEdit[boardIndex].AddRect(-rectHalfW, -rectHalfH, rectHalfW, rectHalfH, 0, "", rectName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg AddRect rc={rR} size={rectW}x{rectH}");
            if (rR != 0)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg 建立矩形失敗 rc={rR}");
                return false;
            }
            Application.DoEvents();
            Thread.Sleep(200);

            m_MMEdit[boardIndex].SetFillStyle(rectName, 3);                         // 1 → 3
            m_MMEdit[boardIndex].SetFrameLineType(rectName, 1);
            m_MMEdit[boardIndex].SetFillRoundPitch(rectName, 0.04);
            m_MMEdit[boardIndex].SetFillPitch(rectName, 0.04);
            m_MMEdit[boardIndex].SetFillTimes(rectName, 4);                         // 1 → 4
            m_MMEdit[boardIndex].SetFillInsideOut(rectName, 1);                     // 向內
            m_MMEdit[boardIndex].SetFillStartAngle(rectName, 90);
            m_MMEdit[boardIndex].SetFillStepAngle(rectName, 45);
            m_MMEdit[boardIndex].SetFillAverageDistribution(rectName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(rectName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(rectName, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(rectName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(rectName, p.RectSpeed);
            m_MMMark[boardIndex].SetPower(rectName, p.RectPower);
            m_MMMark[boardIndex].SetFrequency(rectName, 80);                        // 20 → 80
            m_MMMark[boardIndex].SetMarkRepeat(rectName, 1);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(rectName, 100);
            m_MMMark[boardIndex].SetPulseWidth(rectName, 250);                      // 200 → 250

            // 矩形排到 QR 前面 → 先打白底、再打 QR
            long rOrd = m_MMEdit[boardIndex].ChangeObjectOrder(rectName, qrName, 0);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg ChangeObjectOrder rc={rOrd}");

            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(300);
            return true;
        }

        /// <summary>
        /// 自動模式：載入 DXF 檔案（使用 ParseDXFFile 解析線段）
        /// </summary>
        private bool LoadDxfAuto(int boardIndex, string dxfPath)
        {
            try
            {
                // 如果是相對路徑，轉換為絕對路徑
                if (!Path.IsPathRooted(dxfPath))
                {
                    string exeDir = Path.GetDirectoryName(Application.ExecutablePath);
                    dxfPath = Path.Combine(exeDir, dxfPath);
                }

                if (!File.Exists(dxfPath))
                {
                    System.Diagnostics.Debug.WriteLine($"找不到 DXF 檔案：{dxfPath}");
                    return false;
                }

                var lines = ParseDXFFile(dxfPath);

                if (lines.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"DXF 檔案中沒有找到線段：{dxfPath}");
                    return false;
                }

                // 計算座標範圍並自動縮放
                double minX = double.MaxValue, maxX = double.MinValue;
                double minY = double.MaxValue, maxY = double.MinValue;

                foreach (var line in lines)
                {
                    minX = Math.Min(minX, Math.Min(line.X1, line.X2));
                    maxX = Math.Max(maxX, Math.Max(line.X1, line.X2));
                    minY = Math.Min(minY, Math.Min(line.Y1, line.Y2));
                    maxY = Math.Max(maxY, Math.Max(line.Y1, line.Y2));
                }

                double origWidth = maxX - minX;
                double origHeight = maxY - minY;
                double origCenterX = (minX + maxX) / 2.0;
                double origCenterY = (minY + maxY) / 2.0;

                double maxSpan = Math.Max(origWidth, origHeight);
                // 工作區短邊決定縮放極限，避免長邊裝得下、短邊卻溢出
                double minWorkspace = Math.Min(m_WorkspaceSize, m_WorkspaceHeight);
                double scaleFactor = (minWorkspace * m_MarginPercent) / maxSpan;

                System.Diagnostics.Debug.WriteLine($"DXF 解析完成：{lines.Count} 條線段，工作區：{m_WorkspaceSize}x{m_WorkspaceHeight}，縮放比例：{scaleFactor:F4}");

                // 轉換座標並加入到 MMEdit
                foreach (var line in lines)
                {
                    double tx1 = (line.X1 - origCenterX) * scaleFactor;
                    double ty1 = (line.Y1 - origCenterY) * scaleFactor;
                    double tx2 = (line.X2 - origCenterX) * scaleFactor;
                    double ty2 = (line.Y2 - origCenterY) * scaleFactor;

                    m_MMEdit[boardIndex].AddLine(tx1, ty1, tx2, ty2, "", "");
                }

                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Thread.Sleep(300);

                System.Diagnostics.Debug.WriteLine($"DXF 載入完成：{lines.Count} 梡線段已加入晶片板 {boardIndex + 1}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DXF 載入失敗：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自動模式：執行打標（同步等待完成）
        /// </summary>
        private bool ExecuteMarkingAuto(int boardIndex)
        {
            try
            {
                // 確保圖形已重繪並載入
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                System.Threading.Thread.Sleep(200); // 給予時間讓圖形載入

                int previewMode = (m_AutoModeArgs != null) ? m_AutoModeArgs.PreviewMode : 0;

                // 預覽模式下套用預覽速度
                if (previewMode > 0 && m_AutoModeArgs.PreviewSpeed.HasValue)
                {
                    m_MMMark[boardIndex].SelectAllObjects();
                    long objCount = m_MMMark[boardIndex].SelectGetCount();
                    for (int i = 0; i < objCount; i++)
                    {
                        string objName = "";
                        m_MMMark[boardIndex].SelectEnum(i, ref objName);
                        if (!string.IsNullOrEmpty(objName))
                        {
                            m_MMMark[boardIndex].SetSpeed(objName, m_AutoModeArgs.PreviewSpeed.Value);
                        }
                    }
                    Console.WriteLine($"Preview speed set to {m_AutoModeArgs.PreviewSpeed.Value} mm/s for {objCount} objects.");
                }

                if (previewMode > 0)
                {
                    // === 紅光預覽模式 ===
                    // 設定預覽模式：1=外框預覽, 2=全路徑預覽
                    m_MMMark[boardIndex].SetPreviewMode(previewMode);
                    m_MMMark[boardIndex].MarkStandBy();
                    Application.DoEvents();

                    // 注意：IsMarking() 在預覽模式下回傳值不正確（已知 SDK 問題）
                    int startResult = m_MMMark[boardIndex].StartMarking(3);
                    if (startResult != 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"預覽啟動失敗，錯誤碼：{startResult}");
                        Console.Error.WriteLine($"Error: StartMarking(3) failed with code {startResult}");
                        return false;
                    }

                    int previewTimeSec = (m_AutoModeArgs != null) ? m_AutoModeArgs.PreviewTime : 15;
                    string modeText = previewMode == 1 ? "Preview(Outline)" : "Preview(Full)";
                    Console.WriteLine($"{modeText} started... ({previewTimeSec}s)");

                    // 以指定時間持續預覽，紅光跑完一輪後重新啟動
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    int previewTimeMs = previewTimeSec * 1000;
                    long lastStartMs = 0;

                    while (sw.ElapsedMilliseconds < previewTimeMs)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);

                        long elapsed = sw.ElapsedMilliseconds;
                        long sinceLastStart = elapsed - lastStartMs;

                        // 預覽跑完一輪後重啟：至少間隔 1 秒，且需 MarkStandBy 重置狀態
                        if (sinceLastStart >= 1000 && m_MMMark[boardIndex].IsMarking() == 0
                            && elapsed < previewTimeMs - 500)
                        {
                            m_MMMark[boardIndex].MarkStandBy();
                            Application.DoEvents();
                            m_MMMark[boardIndex].StartMarking(3);
                            lastStartMs = elapsed;
                            System.Diagnostics.Debug.WriteLine($"預覽重啟 @{elapsed}ms");
                        }
                    }

                    // 用 StopMarking() 結束預覽（SDK 範例做法）
                    m_MMMark[boardIndex].StopMarking();
                    Console.WriteLine("Preview completed.");
                    return true;
                }
                else
                {
                    // === 正常打標模式 ===
                    m_MMMark[boardIndex].MarkStandBy();
                    Application.DoEvents();

                    int startResult = StartMarkingWithRetry(boardIndex, 4);
                    if (startResult != 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"打標啟動失敗，錯誤碼：{startResult}");
                        Console.Error.WriteLine($"Error: StartMarking(4) failed with code {startResult}");
                        return false;
                    }

                    Console.WriteLine("Marking started... Waiting for completion.");

                    int loopCount = 0;
                    while (m_MMMark[boardIndex].IsMarking() != 0)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        loopCount++;

                        if (loopCount % 10 == 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"打標進行中... ({loopCount * 100}ms)");
                        }

                        if (loopCount > 3000) // 300 秒超時（白底 QR 密填可能較久）
                        {
                            System.Diagnostics.Debug.WriteLine("打標超時，強制停止");
                            m_MMMark[boardIndex].StopMarking();
                            return false;
                        }
                    }

                    m_MMMark[boardIndex].MarkShutdown();
                    System.Diagnostics.Debug.WriteLine($"打標完成（耗時 {loopCount * 100}ms）");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打標失敗：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自動模式：套用雷射參數到所有物件
        /// </summary>
        private void ApplyLaserParamsAuto(int boardIndex)
        {
            try
            {
                m_MMMark[boardIndex].SelectAllObjects();
                long objCount = m_MMMark[boardIndex].SelectGetCount();

                if (objCount == 0)
                {
                    Console.WriteLine($"[Board {boardIndex + 1}] Warning: No objects to apply laser params.");
                    return;
                }

                for (int i = 0; i < objCount; i++)
                {
                    string objName = "";
                    m_MMMark[boardIndex].SelectEnum(i, ref objName);

                    if (string.IsNullOrEmpty(objName))
                        continue;

                    if (m_AutoModeArgs.Power.HasValue)
                        m_MMMark[boardIndex].SetPower(objName, m_AutoModeArgs.Power.Value);

                    if (m_AutoModeArgs.Speed.HasValue)
                        m_MMMark[boardIndex].SetSpeed(objName, m_AutoModeArgs.Speed.Value);

                    if (m_AutoModeArgs.Frequency.HasValue)
                        m_MMMark[boardIndex].SetFrequency(objName, m_AutoModeArgs.Frequency.Value);

                    if (m_AutoModeArgs.PulseWidth.HasValue)
                        m_MMMark[boardIndex].SetPulseWidth(objName, m_AutoModeArgs.PulseWidth.Value);

                    if (m_AutoModeArgs.MarkRepeat.HasValue)
                        m_MMMark[boardIndex].SetMarkRepeat(objName, m_AutoModeArgs.MarkRepeat.Value);

                    // 擺動：有指定寬度則啟動
                    // 頻率 = 擺動速度 / (π × 擺動寬度)，不覆蓋標記速度
                    // QR 物件不套用 wobble — EMC6 對 QR 套擺動會回 Unknown Commands；
                    // GUI 的 QR 載入按鈕也刻意沒呼叫 SetWobble。要加粗請改 --qr-width/--qr-height。
                    if (m_AutoModeArgs.WobbleWidth.HasValue
                        && string.IsNullOrEmpty(m_AutoModeArgs.QRContent))
                    {
                        double wobSpeed = m_AutoModeArgs.WobbleSpeed ?? 5026.55;
                        double wobWidth = m_AutoModeArgs.WobbleWidth.Value;
                        int wobbleFreq = (int)(wobSpeed / (Math.PI * wobWidth));
                        long r6 = m_MMMark[boardIndex].SetWobble(objName, wobWidth, wobbleFreq);
                        long r7 = m_MMMark[boardIndex].SetWobbleSwitch(objName, 1);
                        double readWt = m_MMMark[boardIndex].GetWobbleThick(objName);
                        long readWf = m_MMMark[boardIndex].GetWobbleFreq(objName);
                        long readWs = m_MMMark[boardIndex].GetWobbleSwitch(objName);
                        Console.Error.WriteLine(
                            $"[Board {boardIndex + 1}] Wobble obj=[{objName}] " +
                            $"Set: Wobble={r6} Switch={r7} | " +
                            $"Req: width={wobWidth} freq={wobbleFreq} | " +
                            $"Read: thick={readWt:F3} freq={readWf} switch={readWs}" +
                            ((r6 != 0 || r7 != 0) ? " ** SET FAILED **" : "") +
                            ((Math.Abs(readWt - wobWidth) > 0.001 || readWf != wobbleFreq || readWs != 1)
                                ? " ** READBACK MISMATCH **" : ""));
                    }
                }

                // 套用參數後 Redraw，確保標記引擎載入新設定
                m_MMMark[boardIndex].Redraw();

                Console.WriteLine($"[Board {boardIndex + 1}] Laser params applied to {objCount} objects." +
                    $" Power={m_AutoModeArgs.Power?.ToString() ?? "default"}" +
                    $" Speed={m_AutoModeArgs.Speed?.ToString() ?? "default"}" +
                    $" Freq={m_AutoModeArgs.Frequency?.ToString() ?? "default"}" +
                    $" PW={m_AutoModeArgs.PulseWidth?.ToString() ?? "default"}" +
                    $" Repeat={m_AutoModeArgs.MarkRepeat?.ToString() ?? "default"}" +
                    $" Wobble={m_AutoModeArgs.WobbleWidth?.ToString() ?? "off"}" +
                    $" WobbleOverlap={m_AutoModeArgs.WobbleOverlap?.ToString() ?? "default"}" +
                    $" WobbleSpeed={m_AutoModeArgs.WobbleSpeed?.ToString() ?? "default"}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] Warning: Failed to apply laser params - {ex.Message}");
            }
        }

        /// <summary>
        /// DXF: 瀏覽 DXF 檔案
        /// </summary>
        private void btnBrowseDXF_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "DXF 檔案 (*.dxf)|*.dxf|所有檔案 (*.*)|*.*";
            dlg.Title = "選擇 DXF 檔案";
            dlg.InitialDirectory = Path.Combine(Application.StartupPath, "File");

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtDXFPath.Text = dlg.FileName;
            }
        }

        /// <summary>
        /// DXF: 載入 DXF 檔案
        /// </summary>
        private void btnLoadDXF_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先在「連接設定」頁簽初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 讀取 UI 工作區參數
            ReadWorkspaceSettings();

            try
            {
                int boardIndex = comboBoardDXF.SelectedIndex;

                if (!m_bBoardInit[boardIndex])
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string dxfPath = txtDXFPath.Text.Trim();

                // 如果是相對路徑，轉換為絕對路徑
                if (!Path.IsPathRooted(dxfPath))
                {
                    string exeDir = Path.GetDirectoryName(Application.ExecutablePath);
                    dxfPath = Path.Combine(exeDir, dxfPath);
                }

                if (!File.Exists(dxfPath))
                {
                    MessageBox.Show($"找不到 DXF 檔案：\n{dxfPath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 使用一般檔案讀取方法解析 DXF
                var lines = ParseDXFFile(dxfPath);

                if (lines.Count == 0)
                {
                    MessageBox.Show($"無法從 DXF 檔案中解析出線段！\n路徑：{dxfPath}\n\n可能原因：\n1. 檔案中沒有 LINE 實體\n2. DXF 格式不正確",
                        "解析失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 計算原始座標範圍
                double minX = double.MaxValue, maxX = double.MinValue;
                double minY = double.MaxValue, maxY = double.MinValue;

                foreach (var line in lines)
                {
                    minX = Math.Min(minX, Math.Min(line.X1, line.X2));
                    maxX = Math.Max(maxX, Math.Max(line.X1, line.X2));
                    minY = Math.Min(minY, Math.Min(line.Y1, line.Y2));
                    maxY = Math.Max(maxY, Math.Max(line.Y1, line.Y2));
                }

                double origWidth = maxX - minX;
                double origHeight = maxY - minY;
                double origCenterX = (minX + maxX) / 2.0;
                double origCenterY = (minY + maxY) / 2.0;

                // 自動縮放和平移到工作區
                // 工作區短邊決定縮放極限，避免長邊裝得下、短邊卻溢出
                double maxSpan = Math.Max(origWidth, origHeight);
                double minWorkspace = Math.Min(m_WorkspaceSize, m_WorkspaceHeight);
                double scaleFactor = (minWorkspace * m_MarginPercent) / maxSpan;

                // 轉換座標
                var transformedLines = new List<DXFLine>();
                foreach (var line in lines)
                {
                    // 1. 平移到原點
                    double tx1 = line.X1 - origCenterX;
                    double ty1 = line.Y1 - origCenterY;
                    double tx2 = line.X2 - origCenterX;
                    double ty2 = line.Y2 - origCenterY;

                    // 2. 縮放
                    tx1 *= scaleFactor;
                    ty1 *= scaleFactor;
                    tx2 *= scaleFactor;
                    ty2 *= scaleFactor;

                    transformedLines.Add(new DXFLine(tx1, ty1, tx2, ty2));
                }

                // 顯示解析出的線段資訊（顯示轉換後的座標）
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine($"已解析 DXF 檔案");
                sb.AppendLine($"路徑：{dxfPath}");
                sb.AppendLine($"共找到 {lines.Count} 梊線段\n");
                sb.AppendLine("=== 座標轉換資訊 ===");
                sb.AppendLine($"原始範圍：X[{minX:F2}, {maxX:F2}] Y[{minY:F2}, {maxY:F2}]");
                sb.AppendLine($"原始大小：{origWidth:F2} x {origHeight:F2} mm");
                sb.AppendLine($"縮放比例：{scaleFactor:F4}");
                sb.AppendLine($"轉換後已置中於工作區 (0, 0)\n");
                sb.AppendLine("線段座標（轉換後）：");
                sb.AppendLine(new string('-', 60));

                // 只顯示前 10 條線段，避免資訊過多
                int displayCount = Math.Min(10, transformedLines.Count);
                for (int i = 0; i < displayCount; i++)
                {
                    var line = transformedLines[i];
                    sb.AppendLine($"線段 {i + 1}:");
                    sb.AppendLine($"  起點: ({line.X1:F3}, {line.Y1:F3})");
                    sb.AppendLine($"  終點: ({line.X2:F3}, {line.Y2:F3})");
                    sb.AppendLine($"  長度: {line.Length:F3} mm");
                    sb.AppendLine();
                }

                if (transformedLines.Count > displayCount)
                {
                    sb.AppendLine($"... 還有 {transformedLines.Count - displayCount} 梡線段");
                }

                txtDXFInfo.Text = sb.ToString();

                // 將轉換後的線段加入到 MMEdit
                foreach (var line in transformedLines)
                {
                    m_MMEdit[boardIndex].AddLine(line.X1, line.Y1, line.X2, line.Y2, "", "");
                }

                // 統一 Redraw (重要：只呼叫一次)
                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Thread.Sleep(300);

                btnMarkDXF.Enabled = true;

                MessageBox.Show($"已在晶片板 {boardIndex + 1} 載入並解析 DXF 檔案！\n\n" +
                    $"共解析出 {lines.Count} 梡線段\n" +
                    $"原始大小：{origWidth:F2} x {origHeight:F2} mm\n" +
                    $"縮放比例：{scaleFactor:F4}\n" +
                    $"已自動調整到工作區範圍內\n\n" +
                    $"路徑：{dxfPath}",
                    "載入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入 DXF 檔案失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DXF: 使用 LoadFile 直接載入完整 DXF 檔案
        /// </summary>
        private void btnLoadDXFFile_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先在「連接設定」頁簽初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int boardIndex = comboBoardDXF.SelectedIndex;

                if (!m_bBoardInit[boardIndex])
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string dxfPath = txtDXFPath.Text.Trim();

                // 如果是相對路徑，轉換為絕對路徑
                if (!Path.IsPathRooted(dxfPath))
                {
                    string exeDir = Path.GetDirectoryName(Application.ExecutablePath);
                    dxfPath = Path.Combine(exeDir, dxfPath);
                }

                if (!File.Exists(dxfPath))
                {
                    MessageBox.Show($"找不到 DXF 檔案：\n{dxfPath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 使用 MMMark.LoadFile 直接載入完整 DXF
                int result = m_MMMark[boardIndex].LoadFile(dxfPath);

                if (result != 0)
                {
                    MessageBox.Show($"載入 DXF 檔案失敗！錯誤碼：{result}\n\n路徑：{dxfPath}\n\n" +
                        "可能原因：\n" +
                        "1. DXF 版本不支援（僅支援 R12, 2000）\n" +
                        "2. 檔案格式不正確\n" +
                        "3. 嘗試使用「載入 DXF 線段」按鈕",
                        "載入失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 重繪顯示
                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Thread.Sleep(300);

                btnMarkDXF.Enabled = true;

                txtDXFInfo.Text = $"已使用 LoadFile 載入 DXF\r\n路徑：{dxfPath}\r\n目標：晶片板 {boardIndex + 1}";

                MessageBox.Show($"已在晶片板 {boardIndex + 1} 載入 DXF 檔案！\n\n路徑：{dxfPath}",
                    "載入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入 DXF 檔案失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DXF: 執行打標
        /// </summary>
        private void btnMarkDXF_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int boardIndex = comboBoardDXF.SelectedIndex;

                if (!m_bBoardInit[boardIndex])
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsBoardBusy(boardIndex))
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                        "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 打標前自動套用雷射參數
                if (!ApplyLaserParamsFromUI(boardIndex))
                    return;

                m_MMMark[boardIndex].MarkStandBy();

                if (m_MMMark[boardIndex].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 啟動 Timer 來監控打標狀態
                timerMark.Tag = boardIndex;
                timerMark.Start();

                btnMarkDXF.Enabled = false;
                btnStopMarkDXF.Enabled = true;
                btnLoadDXF.Enabled = false;
                btnLoadDXFFile.Enabled = false;
                btnMark.Enabled = false;
                btnStop.Enabled = true;
                // 停用 QR 頁籤按鈕
                btnMarkQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnLoadQR.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DXF: 預覽結果（紅光標示，不打雷射）
        /// </summary>
        private void btnPreviewDXF_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int boardIndex = comboBoardDXF.SelectedIndex;

                if (!m_bBoardInit[boardIndex])
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsBoardBusy(boardIndex))
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                        "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 設定預覽模式（全路徑預覽）並啟動紅光預覽
                m_MMMark[boardIndex].SetPreviewMode(2);
                m_MMMark[boardIndex].MarkStandBy();
                Application.DoEvents();

                if (m_MMMark[boardIndex].StartMarking(3) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 預覽啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 注意：IsMarking() 在預覽模式下回傳值不正確（SDK 已知問題）
                m_bPreviewing = true;
                m_iPreviewBoard = boardIndex;

                // 啟動 15 秒自動關閉 Timer
                timerPreview.Stop();
                timerPreview.Start();

                // 停用按鈕，防止重複操作
                btnMarkDXF.Enabled = false;
                btnPreviewDXF.Enabled = false;
                btnStopPreview.Enabled = true;
                btnLoadDXF.Enabled = false;
                btnLoadDXFFile.Enabled = false;
                btnClearDXF.Enabled = false;
                btnMark.Enabled = false;
                btnPreviewManual.Enabled = false;
                btnStop.Enabled = true;
                // 停用 QR 頁籤按鈕
                btnMarkQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnLoadQR.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DXF: 停止紅光預覽
        /// </summary>
        private void btnStopPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            try
            {
                timerPreview.Stop();

                int boardIndex = (m_iPreviewBoard >= 0) ? m_iPreviewBoard : comboBoardDXF.SelectedIndex;
                if (boardIndex >= 0 && boardIndex < m_bBoardInit.Length && m_bBoardInit[boardIndex])
                {
                    m_MMMark[boardIndex].StopMarking();
                }
                m_bPreviewing = false;
                m_iPreviewBoard = -1;

                ResetPreviewButtonsAfterStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerPreview_Tick(object sender, EventArgs e)
        {
            timerPreview.Stop();

            if (!m_bInit || !m_bPreviewing) return;

            try
            {
                // 使用 m_iPreviewBoard 而非硬編碼的 comboBoardDXF；
                // 來源 tab 不同（DXF / QR / 手動）時都能停在正確的板上
                int boardIndex = (m_iPreviewBoard >= 0) ? m_iPreviewBoard : comboBoardDXF.SelectedIndex;
                if (boardIndex >= 0 && boardIndex < m_bBoardInit.Length && m_bBoardInit[boardIndex])
                {
                    m_MMMark[boardIndex].StopMarking();
                }
                m_bPreviewing = false;
                m_iPreviewBoard = -1;

                ResetPreviewButtonsAfterStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 紅光預覽結束 / 停止後，把三個 tab 的按鈕狀態恢復成「閒置」。
        /// </summary>
        private void ResetPreviewButtonsAfterStop()
        {
            // 手動繪圖頁籤
            btnMark.Enabled = true;
            btnStop.Enabled = false;
            btnPreviewManual.Enabled = true;
            btnStopPreviewManual.Enabled = false;
            // DXF 頁籤
            btnMarkDXF.Enabled = true;
            btnStopMarkDXF.Enabled = false;
            btnPreviewDXF.Enabled = true;
            btnStopPreview.Enabled = false;
            btnLoadDXF.Enabled = true;
            btnLoadDXFFile.Enabled = true;
            btnClearDXF.Enabled = true;
            // QR Code 頁籤
            btnMarkQR.Enabled = true;
            btnStopMarkQR.Enabled = false;
            btnPreviewQR.Enabled = true;
            btnStopPreviewQR.Enabled = false;
            btnLoadQR.Enabled = true;
            btnClearQR.Enabled = true;
            // 命令提示頁籤
            btnCmd1.Enabled = true;
            btnCmd2.Enabled = true;
            btnCmd3.Enabled = true;
            btnCmd4.Enabled = true;
            btnCmd5.Enabled = true;
            btnCmdRegen.Enabled = true;
            // 命令預覽會臨時調整 timerPreview.Interval，恢復成預設 15 秒
            timerPreview.Interval = 15000;
        }

        // ============================================================
        // 命令提示頁籤：隨機產生 5 組指令（含 --wobble-width，正式打標模式）。
        // 使用者可手動編輯 textbox 加入 --preview 切回紅光預覽。
        // ============================================================

        private class CmdPreviewSpec
        {
            public int BoardIndex;
            public List<LineSegment> Lines;     // 線段內容（單條或多條）；null = 非線段類
            public string QRContent;            // QR 內容；null = 非 QR
            public double QRWidth, QRHeight;
            public bool QRInvert;               // QR 反相（黑白互換）
            public int PreviewMode;             // 0=正式打標, 1=outline 預覽, 2=full 預覽
            public int PreviewTime;             // 預覽模式秒數；PreviewMode=0 時忽略
            public double? WobbleWidth;         // 線條寬度 mm（雷射加粗），null=不啟動 wobble
            public double? WobbleSpeed;         // 擺動速度 mm/s，null=用 SDK 預設 5026.55
            public bool QRWhiteBg;              // 白底反相 QR（--qr-whitebg）
            public CommandLineArgs Cli;         // 解析後的原始 CLI 參數（白底 QR 用來組 WhiteBgQRParams）
            public string DisplayText;          // 顯示在 textbox 內的命令字串
        }

        private readonly Random m_CmdRandom = new Random();

        private void GenerateCmdPreviews()
        {
            var textboxes = new[] { txtCmd1, txtCmd2, txtCmd3, txtCmd4, txtCmd5 };
            // round-robin 分配到所有已初始化板，讓 5 條命令可並行打到不同板
            var initBoards = new List<int>();
            for (int i = 0; i < m_bBoardInit.Length; i++)
                if (m_bBoardInit[i]) initBoards.Add(i);
            // 尚未初始化（如初次啟動）就退回到 comboBoardCmd 的選擇
            if (initBoards.Count == 0)
            {
                int fallback = (comboBoardCmd != null && comboBoardCmd.SelectedIndex >= 0)
                    ? comboBoardCmd.SelectedIndex : 0;
                initBoards.Add(fallback);
            }
            for (int i = 0; i < 5; i++)
            {
                int boardIdx = initBoards[i % initBoards.Count];
                textboxes[i].Text = GenerateOneCmdSpec(boardIdx).DisplayText;
            }
        }

        private CmdPreviewSpec GenerateOneCmdSpec(int boardIdx)
        {
            var spec = new CmdPreviewSpec
            {
                BoardIndex = boardIdx,
                PreviewMode = 0,    // 取消預覽，按鈕按下即正式打標
                PreviewTime = 0
            };

            var sb = new StringBuilder();
            // 固定輸出 --client：範例字串是給人複製到終端機用的，daemon 架構下必須走 client 派工，
            // 否則會變成獨立模式 A process 去 init OCX、撞上 daemon 的 SDK 鎖（需重開機）。
            sb.Append("MarkingMate.exe --client");
            sb.Append($" --board {spec.BoardIndex}");
            sb.Append($" --config /cfg_config_MM{spec.BoardIndex + 1}");

            // 隨機選一種內容：多線段 / QR（取消單線範例，命令提示一律用 --lines 多段示範）
            int contentType = m_CmdRandom.Next(0, 2);
            if (contentType == 0)
            {
                int n = m_CmdRandom.Next(2, 5);
                spec.Lines = new List<LineSegment>();
                var parts = new List<string>();
                for (int j = 0; j < n; j++)
                {
                    int x1 = m_CmdRandom.Next(-50, 51);
                    int y1 = m_CmdRandom.Next(-50, 51);
                    int x2 = m_CmdRandom.Next(-50, 51);
                    int y2 = m_CmdRandom.Next(-50, 51);
                    spec.Lines.Add(new LineSegment(x1, y1, x2, y2));
                    parts.Add($"{x1},{y1},{x2},{y2}");
                }
                sb.Append($" --lines \"{string.Join(";", parts)}\"");

                // 線條寬度（wobble）：固定 0.5 mm，只有線段類套用（QR 不套 wobble）
                double wobbleWidth = 0.5;
                spec.WobbleWidth = wobbleWidth;
                sb.Append($" --wobble-width {wobbleWidth:0.0}");
            }
            else
            {
                string[] samples = { "DEMO-001", "TEST", "ABC-123", "QR-XYZ", "Hello", "MarkingMate" };
                spec.QRContent = samples[m_CmdRandom.Next(samples.Length)];
                spec.QRWhiteBg = true;
                // 白底反相 QR：只帶內容 + --qr-whitebg，長寬 / 外框 / 雷射參數等
                // 全部由後端 BuildWhiteBgQR 依 QRCODE_白底 的值寫死。
                sb.Append($" --qrcode \"{spec.QRContent}\"");
                sb.Append(" --qr-whitebg");
            }

            sb.Append(" --mark");
            spec.DisplayText = sb.ToString();
            return spec;
        }

        private void btnCmdRegen_Click(object sender, EventArgs e) => GenerateCmdPreviews();
        private void btnCmd1_Click(object sender, EventArgs e) => RunCmdPreview(0);
        private void btnCmd2_Click(object sender, EventArgs e) => RunCmdPreview(1);
        private void btnCmd3_Click(object sender, EventArgs e) => RunCmdPreview(2);
        private void btnCmd4_Click(object sender, EventArgs e) => RunCmdPreview(3);
        private void btnCmd5_Click(object sender, EventArgs e) => RunCmdPreview(4);

        /// <summary>
        /// 命令提示頁籤：板下拉選單變動 → 不再強制同步 textbox，
        /// 因為 5 條命令現在可獨立指向不同板並行執行。
        /// combo 僅作為「初始化前的 fallback 預設板號」。
        /// </summary>
        private void comboBoardCmd_SelectedIndexChanged(object sender, EventArgs e)
        {
            // no-op: 每條命令的 --board 由使用者編輯 textbox 自行決定
        }

        /// <summary>
        /// 查詢指定板是否正在被任一頁籤佔用（DXF/QR/手動 共用 m_bPreviewing，
        /// 命令頁籤用 m_bCmdPreviewing 陣列）。同板撞車的所有 click handler 都應先查這個。
        /// </summary>
        private bool IsBoardBusy(int board)
        {
            if (board < 0 || board >= m_bBoardInit.Length) return false;
            if (m_bCmdPreviewing[board]) return true;
            if (m_bPreviewing && m_iPreviewBoard == board) return true;
            return false;
        }

        /// <summary>
        /// 把 textbox 內的命令字串切成 argv（支援雙引號包夾，無需處理 \ 跳脫）。
        /// </summary>
        private static string[] SplitCommandLine(string s)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            foreach (var c in s ?? "")
            {
                if (c == '"') { inQuotes = !inQuotes; }
                else if (char.IsWhiteSpace(c) && !inQuotes)
                {
                    if (sb.Length > 0) { result.Add(sb.ToString()); sb.Clear(); }
                }
                else { sb.Append(c); }
            }
            if (sb.Length > 0) result.Add(sb.ToString());
            return result.ToArray();
        }

        /// <summary>
        /// 把編輯過的命令字串解析成 CmdPreviewSpec。
        /// 內部複用 CommandLineArgs.Parse，所以支援 CLI 模式完整 schema。
        /// </summary>
        private CmdPreviewSpec ParseCmdToSpec(string cmdLine)
        {
            var args = SplitCommandLine(cmdLine);
            // 把開頭的 exe 名稱（不以 - 開頭的第一個 token）去掉
            if (args.Length > 0 && !args[0].StartsWith("-"))
            {
                var rest = new string[args.Length - 1];
                Array.Copy(args, 1, rest, 0, rest.Length);
                args = rest;
            }

            var cli = CommandLineArgs.Parse(args);

            var spec = new CmdPreviewSpec
            {
                BoardIndex = cli.BoardIndex,
                // 沒指定 --preview → PreviewMode=0 = 正式打標
                PreviewMode = cli.PreviewMode,
                PreviewTime = cli.PreviewTime,
                // 沒指定 --wobble-width → 用預設 0.5；有指定就用使用者的值
                WobbleWidth = cli.WobbleWidth ?? 0.5,
                WobbleSpeed = cli.WobbleSpeed,
                QRWhiteBg = cli.QRWhiteBg,
                Cli = cli,
                DisplayText = cmdLine
            };

            if (cli.Lines != null && cli.Lines.Count > 0)
            {
                spec.Lines = cli.Lines;
            }
            else if (!string.IsNullOrEmpty(cli.QRContent))
            {
                spec.QRContent = cli.QRContent;
                spec.QRWidth = cli.QRWidth;
                spec.QRHeight = cli.QRHeight;
                spec.QRInvert = cli.QRInvert;
            }
            return spec;
        }

        private void RunCmdPreview(int idx)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先到「連接設定」頁按「初始化」！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 每次按下按鈕都重新從 textbox 解析（讓編輯生效）
            var textboxes = new[] { txtCmd1, txtCmd2, txtCmd3, txtCmd4, txtCmd5 };
            string cmdText = textboxes[idx].Text;

            CmdPreviewSpec spec;
            try { spec = ParseCmdToSpec(cmdText); }
            catch (Exception parseEx)
            {
                MessageBox.Show($"命令 #{idx + 1} 解析失敗：{parseEx.Message}", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (spec == null || (spec.Lines == null && string.IsNullOrEmpty(spec.QRContent)))
            {
                MessageBox.Show($"命令 #{idx + 1} 缺少內容：需要 --line / --lines / --qrcode 至少其中一項。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int board = spec.BoardIndex;
            if (board < 0 || board >= m_bBoardInit.Length || !m_bBoardInit[board])
            {
                MessageBox.Show($"命令 #{idx + 1} 指定的晶片板 {board + 1} 未初始化。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsBoardBusy(board))
            {
                MessageBox.Show($"晶片板 {board + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                    "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ResetFile 後 SDK 文件的工作區會回到 config 預設值（通常小於 m_WorkspaceSize），
                // 必須重新 SetDesktopCenter/SetDesktopSize，否則首次 AddLine 會被 OCX 判定為「超出工作範圍」。
                m_MMMark[board].ResetFile();
                m_MMMark[board].SetDesktopCenter(0, 0);
                m_MMMark[board].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
                Application.DoEvents();
                Thread.Sleep(100);

                if (spec.Lines != null && spec.Lines.Count > 0)
                {
                    double halfW = m_WorkspaceSize / 2.0;
                    double halfH = m_WorkspaceHeight / 2.0;
                    foreach (var line in spec.Lines)
                    {
                        // 與 DrawLineAuto 相同的座標規則：任一負值 → 中心原點；全正 → 左下角原點需平移
                        bool isCenterBased = line.X1 < 0 || line.X2 < 0 || line.Y1 < 0 || line.Y2 < 0;
                        double x1, y1, x2, y2;
                        if (isCenterBased)
                        {
                            x1 = line.X1; y1 = line.Y1; x2 = line.X2; y2 = line.Y2;
                        }
                        else
                        {
                            x1 = line.X1 - halfW; y1 = line.Y1 - halfH;
                            x2 = line.X2 - halfW; y2 = line.Y2 - halfH;
                        }
                        m_MMEdit[board].AddLine(x1, y1, x2, y2, "", "");
                    }
                }
                else if (!string.IsNullOrEmpty(spec.QRContent))
                {
                    if (spec.QRWhiteBg)
                    {
                        // 白底反相 QR：與 daemon / 模式 A 共用同一套後端建構（參數寫死可覆寫）
                        var wp = spec.Cli != null
                            ? MakeWhiteBgParams(spec.Cli)
                            : new WhiteBgQRParams { Content = spec.QRContent };
                        if (!BuildWhiteBgQR(board, wp))
                        {
                            MessageBox.Show($"晶片板 {board + 1} 白底 QR 建立失敗！", "錯誤",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        m_MMMark[board].AddBarcode(BARCODE_TYPE_QRCODE, spec.QRContent,
                            0, 0, spec.QRWidth, spec.QRHeight, "", "");
                        Application.DoEvents();
                        Thread.Sleep(50);
                        ApplyQRECLevelLow(board);   // QR 固定容錯等級 LOW（EC=0）
                        if (spec.QRInvert)
                        {
                            Application.DoEvents();
                            Thread.Sleep(50);
                            ApplyQRInvert(board);
                        }
                    }
                }

                Application.DoEvents();
                Thread.Sleep(100);

                // 套用線條寬度（wobble）到所有物件 — 同 ApplyLaserParamsAuto 流程
                // QR 物件跳過：EMC6 對 QR 套擺動會回 Unknown Commands
                if (spec.WobbleWidth.HasValue && spec.WobbleWidth.Value > 0
                    && string.IsNullOrEmpty(spec.QRContent))
                {
                    double wobSpeed = spec.WobbleSpeed ?? 5026.55;
                    double wobWidth = spec.WobbleWidth.Value;
                    int wobbleFreq = (int)(wobSpeed / (Math.PI * wobWidth));

                    m_MMMark[board].SelectAllObjects();
                    long objCount = m_MMMark[board].SelectGetCount();
                    System.Diagnostics.Debug.WriteLine($"[Cmd] Board {board + 1} wobble apply: objCount={objCount}");
                    for (int i = 0; i < objCount; i++)
                    {
                        string objName = "";
                        m_MMMark[board].SelectEnum(i, ref objName);
                        if (string.IsNullOrEmpty(objName)) continue;
                        m_MMMark[board].SetWobble(objName, wobWidth, wobbleFreq);
                        m_MMMark[board].SetWobbleSwitch(objName, 1);
                    }
                }

                m_MMMark[board].Redraw();
                Thread.Sleep(200);

                if (spec.PreviewMode > 0)
                {
                    // === 紅光預覽（保留原行為，可由 textbox 編輯 --preview 觸發） ===
                    m_MMMark[board].SetPreviewMode(spec.PreviewMode);
                    m_MMMark[board].MarkStandBy();
                    Application.DoEvents();

                    if (m_MMMark[board].StartMarking(3) != 0)
                    {
                        MessageBox.Show($"晶片板 {board + 1} 預覽啟動失敗！", "錯誤",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int previewSec = spec.PreviewTime > 0 ? spec.PreviewTime : 15;
                    m_bCmdPreviewing[board] = true;
                    m_TimerCmdPreview[board].Stop();
                    m_TimerCmdPreview[board].Interval = previewSec * 1000;
                    m_TimerCmdPreview[board].Start();
                }
                else
                {
                    // === 正式打標：StartMarking(4) + 阻塞輪詢直到完成 ===
                    m_MMMark[board].MarkStandBy();
                    Application.DoEvents();

                    int startResult = StartMarkingWithRetry(board, 4);
                    if (startResult != 0)
                    {
                        MessageBox.Show($"晶片板 {board + 1} 打標啟動失敗，錯誤碼：{startResult}",
                            "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    m_bCmdPreviewing[board] = true;
                    int loopCount = 0;
                    while (m_MMMark[board].IsMarking() != 0)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                        loopCount++;
                        if (loopCount > 3000) // 300 秒安全超時（白底 QR 密填可能較久）
                        {
                            m_MMMark[board].StopMarking();
                            MessageBox.Show($"晶片板 {board + 1} 打標超時 300s，已強制停止。",
                                "超時", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                    }
                    try { m_MMMark[board].MarkShutdown(); } catch { /* 容忍 */ }
                    m_bCmdPreviewing[board] = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動命令 #{idx + 1} 預覽失敗：{ex.Message}", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 命令頁籤 per-board Timer 到期 → 該板自動停止紅光預覽。
        /// Tag 由建構子設成 boardIndex。
        /// </summary>
        private void OnCmdPreviewTimerTick(object sender, EventArgs e)
        {
            var timer = sender as System.Windows.Forms.Timer;
            if (timer == null || !(timer.Tag is int)) return;
            int board = (int)timer.Tag;
            timer.Stop();
            if (board >= 0 && board < m_bBoardInit.Length && m_bBoardInit[board])
            {
                try { m_MMMark[board].StopMarking(); } catch { /* 容忍硬體未就緒 */ }
            }
            if (board >= 0 && board < m_bCmdPreviewing.Length)
            {
                m_bCmdPreviewing[board] = false;
            }
        }

        // ============================================================
        // 並行驗證：對所有已初始化板同時觸發紅光預覽，驗證 SDK 是否支援
        // 多板並行（背靠背呼叫 StartMarking(3)，5 秒後一起停止）
        // ============================================================
        private void btnParallelTest_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先到「連接設定」頁按「初始化」！", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_bPreviewing || m_bParallelTesting)
            {
                MessageBox.Show("已有預覽正在進行，請先停止。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var initBoards = new List<int>();
            for (int i = 0; i < m_bBoardInit.Length; i++)
                if (m_bBoardInit[i]) initBoards.Add(i);

            if (initBoards.Count < 2)
            {
                MessageBox.Show($"並行驗證至少需要 2 塊板已初始化（目前 {initBoards.Count} 塊）。",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtParallelResult.Clear();
            AppendParallelLog($"=== 並行驗證開始：{initBoards.Count} 塊板 ===");

            // 對照 RunCmdPreview 的可運作流程，加入必要 sleeps 讓 SDK 內部 file 落地
            foreach (var b in initBoards)
            {
                try
                {
                    // ResetFile 後須重新 SetDesktopCenter/SetDesktopSize，否則首次 AddLine 會被 OCX 判定為「超出工作範圍」。
                    m_MMMark[b].ResetFile();
                    m_MMMark[b].SetDesktopCenter(0, 0);
                    m_MMMark[b].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
                    Application.DoEvents();
                    Thread.Sleep(100);
                    m_MMEdit[b].AddLine(-30, -30, 30, 30, "", "");
                    Application.DoEvents();
                    Thread.Sleep(100);
                    m_MMMark[b].Redraw();
                    Thread.Sleep(200);
                    m_MMMark[b].SetPreviewMode(2);
                    m_MMMark[b].MarkStandBy();
                    Application.DoEvents();
                    AppendParallelLog($"板 {b + 1}: 內容就緒（對角線）");
                }
                catch (Exception ex)
                {
                    AppendParallelLog($"板 {b + 1}: 內容準備失敗 - {ex.Message}");
                }
            }

            AppendParallelLog("--- 連續觸發 StartMarking(3)，呼叫後立刻查 IsMarking ---");
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var immediateMarking = new Dictionary<int, long>();
            foreach (var b in initBoards)
            {
                long t0 = sw.ElapsedMilliseconds;
                try
                {
                    int rc = m_MMMark[b].StartMarking(3);
                    long t1 = sw.ElapsedMilliseconds;
                    long isNow = 0;
                    try { isNow = m_MMMark[b].IsMarking(); } catch { }
                    immediateMarking[b] = isNow;
                    AppendParallelLog($"板 {b + 1}: StartMarking 返回 {rc} (t={t0}→{t1}ms), 緊接 IsMarking={isNow}");
                }
                catch (Exception ex)
                {
                    AppendParallelLog($"板 {b + 1}: StartMarking 例外 - {ex.Message}");
                }
            }

            Application.DoEvents();
            Thread.Sleep(500);
            AppendParallelLog("--- 500ms 後再查 IsMarking ---");
            int activeCount = 0;
            foreach (var b in initBoards)
            {
                try
                {
                    long isMarking = m_MMMark[b].IsMarking();
                    bool active = isMarking != 0;
                    if (active) activeCount++;
                    AppendParallelLog($"板 {b + 1}: IsMarking = {isMarking} ({(active ? "✓ 紅光中" : "✗ 未啟動")})");
                }
                catch (Exception ex)
                {
                    AppendParallelLog($"板 {b + 1}: IsMarking 例外 - {ex.Message}");
                }
            }
            AppendParallelLog($">>> 500ms 後仍在紅光中的板數：{activeCount}/{initBoards.Count}");

            m_ParallelTestBoards = initBoards;
            m_bParallelTesting = true;
            btnParallelTest.Enabled = false;
            AppendParallelLog("--- 5 秒後自動停止 ---");
            AppendParallelLog("** 請目視確認：所有板的紅光是否同時亮起 **");

            timerParallelTest.Start();
        }

        private void timerParallelTest_Tick(object sender, EventArgs e)
        {
            timerParallelTest.Stop();
            foreach (var b in m_ParallelTestBoards)
            {
                try
                {
                    m_MMMark[b].StopMarking();
                    AppendParallelLog($"板 {b + 1}: StopMarking OK");
                }
                catch (Exception ex)
                {
                    AppendParallelLog($"板 {b + 1}: StopMarking 失敗 - {ex.Message}");
                }
            }
            AppendParallelLog("=== 並行驗證結束 ===");
            m_bParallelTesting = false;
            btnParallelTest.Enabled = true;
        }

        private void AppendParallelLog(string line)
        {
            txtParallelResult.AppendText(line + "\r\n");
        }

        private void btnStopMarkDXF_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            try
            {
                int boardIndex = comboBoardDXF.SelectedIndex;
                if (m_bBoardInit[boardIndex])
                {
                    m_MMMark[boardIndex].StopMarking();
                }
                timerMark.Stop();

                // 恢復按鈕狀態
                btnMarkDXF.Enabled = true;
                btnStopMarkDXF.Enabled = false;
                btnPreviewDXF.Enabled = true;
                btnStopPreview.Enabled = false;
                btnLoadDXF.Enabled = true;
                btnLoadDXFFile.Enabled = true;
                btnClearDXF.Enabled = true;
                btnMark.Enabled = true;
                btnStop.Enabled = false;
                // QR Code 頁籤按鈕
                btnMarkQR.Enabled = true;
                btnStopMarkQR.Enabled = false;
                btnLoadQR.Enabled = true;
                btnPreviewQR.Enabled = true;
                btnClearQR.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止打標失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DXF: 清除畫面（刪除所有繪圖物件）
        /// </summary>
        private void btnClearDXF_Click(object sender, EventArgs e)
        {
            // 此功能已移除
        }

        /// <summary>
        /// 解析 DXF 檔案，提取線段
        /// </summary>
        private List<DXFLine> ParseDXFFile(string filePath)
        {
            List<DXFLine> lines = new List<DXFLine>();

            try
            {
                string[] dxfContent = File.ReadAllLines(filePath);
                System.Diagnostics.Debug.WriteLine($"=== 開始解析 DXF 檔案 ===");
                System.Diagnostics.Debug.WriteLine($"檔案路徑: {filePath}");
                System.Diagnostics.Debug.WriteLine($"總行數: {dxfContent.Length}");

                bool inEntities = false;
                bool inLine = false;
                double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                int coordCount = 0;

                // 改用配對讀取：群組碼 + 數值
                for (int i = 0; i < dxfContent.Length - 1; i += 2)
                {
                    string groupCode = dxfContent[i].Trim();
                    string value = dxfContent[i + 1].Trim();

                    // 檢查是否進入 ENTITIES 區段
                    if (groupCode == "2" && value == "ENTITIES")
                    {
                        inEntities = true;
                        System.Diagnostics.Debug.WriteLine($"[行 {i}] 進入 ENTITIES 區段");
                        continue;
                    }

                    // 檢查是否結束 ENTITIES 區段
                    if (groupCode == "0" && value == "ENDSEC" && inEntities)
                    {
                        break;
                    }

                    if (!inEntities)
                        continue;

                    // 檢查是否是 LINE 實體
                    if (groupCode == "0" && value == "LINE")
                    {
                        // 如果前一個 LINE 已收集完所有座標，先加入
                        if (inLine && coordCount == 15)
                        {
                            lines.Add(new DXFLine(x1, y1, x2, y2));
                            System.Diagnostics.Debug.WriteLine($"  → 加入線段 {lines.Count}: ({x1:F2}, {y1:F2}) -> ({x2:F2}, {y2:F2})");
                        }

                        inLine = true;
                        coordCount = 0;
                        x1 = y1 = x2 = y2 = 0;
                        System.Diagnostics.Debug.WriteLine($"[行 {i}] 發現 LINE 實體");
                        continue;
                    }

                    if (inLine)
                    {
                        // 讀取座標
                        if (groupCode == "10")
                        {
                            if (double.TryParse(value, out x1))
                                coordCount |= 1; // bit 0
                        }
                        else if (groupCode == "20")
                        {
                            if (double.TryParse(value, out y1))
                                coordCount |= 2; // bit 1
                        }
                        else if (groupCode == "11")
                        {
                            if (double.TryParse(value, out x2))
                                coordCount |= 4; // bit 2
                        }
                        else if (groupCode == "21")
                        {
                            if (double.TryParse(value, out y2))
                                coordCount |= 8; // bit 3
                        }
                        else if (groupCode == "0" && value != "LINE")
                        {
                            // 遇到新實體，結束當前 LINE
                            if (coordCount == 15) // 所有4個座標都有 (1|2|4|8 = 15)
                            {
                                lines.Add(new DXFLine(x1, y1, x2, y2));
                                System.Diagnostics.Debug.WriteLine($"  → 加入線段 {lines.Count}: ({x1:F2}, {y1:F2}) -> ({x2:F2}, {y2:F2})");
                            }
                            else if (coordCount > 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"  ! 警告：LINE 座標不完整 (coordCount={coordCount:X})");
                            }
                            inLine = false;
                            coordCount = 0;
                            i -= 2; // 退回，讓下一次迴圈處理這個新實體
                        }

                        // 檢查是否已收集完所有座標
                        if (coordCount == 15)
                        {
                            lines.Add(new DXFLine(x1, y1, x2, y2));
                            System.Diagnostics.Debug.WriteLine($"  → 加入線段 {lines.Count}: ({x1:F2}, {y1:F2}) -> ({x2:F2}, {y2:F2})");
                            inLine = false;
                            coordCount = 0;
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"解析 DXF 完成，共找到 {lines.Count} 梊線段");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解析 DXF 失敗：{ex.Message}");
            }

            return lines;
        }

        /// <summary>
        /// 呼叫指定 action 並限制執行時間；若超時則放棄（背景執行緒不阻塞）。
        /// 用 STA 執行緒 + pumping join，讓可能 COM 回呼 UI 執行緒的方法不會 deadlock。
        /// </summary>
        private static bool CallWithTimeout(Action action, int timeoutMs)
        {
            var t = new Thread(() =>
            {
                try { action(); } catch { /* 忽略 */ }
            });
            t.IsBackground = true;
            try { t.SetApartmentState(ApartmentState.STA); } catch { }
            t.Start();

            // 用 pumping join：每 50ms 嘗試 join 並打 DoEvents，避免 COM 卡在 UI 訊息佇列
            var start = Environment.TickCount;
            while (!t.Join(50))
            {
                try { Application.DoEvents(); } catch { }
                if (Environment.TickCount - start > timeoutMs) return false;
            }
            return true;
        }

        /// <summary>
        /// 清理舊的控件（防止殘留導致初始化失敗）
        /// Finish() 可能卡在等硬體，使用 timeout 包裝避免整支程式卡住。
        /// </summary>
        private void CleanupOldControls()
        {
            try
            {
                // Step 1: 先停打標 timer / 停打標本身 / 關閉打標引擎（皆 timeout 保護）
                try { timerMark.Stop(); } catch { }
                try { timerPreview.Stop(); } catch { }
                for (int i = 0; i < 4; i++)
                {
                    if (m_MMMark[i] != null && m_bBoardInit[i])
                    {
                        int idx = i;
                        CallWithTimeout(() => { try { m_MMMark[idx].StopMarking(); } catch { } }, 1000);
                        CallWithTimeout(() => { try { m_MMMark[idx].MarkShutdown(); } catch { } }, 1000);
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    // 清理 MMMark
                    if (m_MMMark[i] != null)
                    {
                        try
                        {
                            if (m_bBoardInit[i])
                            {
                                int idx = i; // 避免 closure 抓錯
                                bool finished = CallWithTimeout(() =>
                                {
                                    try { m_MMMark[idx].Finish(); } catch { }
                                }, 2000);
                                if (!finished)
                                    System.Diagnostics.Debug.WriteLine($"MMMark[{idx}].Finish() 超時 2s，強制略過");
                            }

                            if (m_Panels[i].Controls.Contains(m_MMMark[i]))
                            {
                                try { m_Panels[i].Controls.Remove(m_MMMark[i]); } catch { }
                            }

                            int idxD1 = i;
                            var refD1 = m_MMMark[i];
                            CallWithTimeout(() => { try { refD1.Dispose(); } catch { } }, 1000);
                            m_MMMark[i] = null;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"清理 MMMark[{i}] 失敗：{ex.Message}");
                        }
                    }

                    // 清理 MMEdit
                    if (m_MMEdit[i] != null)
                    {
                        try
                        {
                            if (m_bBoardInit[i])
                            {
                                int idx = i;
                                bool finished = CallWithTimeout(() =>
                                {
                                    try { m_MMEdit[idx].Finish(); } catch { }
                                }, 2000);
                                if (!finished)
                                    System.Diagnostics.Debug.WriteLine($"MMEdit[{idx}].Finish() 超時 2s，強制略過");
                            }

                            if (this.Controls.Contains(m_MMEdit[i]))
                            {
                                try { this.Controls.Remove(m_MMEdit[i]); } catch { }
                            }

                            int idxD2 = i;
                            var refD2 = m_MMEdit[i];
                            CallWithTimeout(() => { try { refD2.Dispose(); } catch { } }, 1000);
                            m_MMEdit[i] = null;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"清理 MMEdit[{i}] 失敗：{ex.Message}");
                        }
                    }

                    m_bBoardInit[i] = false;
                }

                // Step 3: 最後掃殘留的 MM27Dx64 背景進程，避免下次啟動失敗
                try
                {
                    var procs = System.Diagnostics.Process.GetProcessesByName("MM27Dx64");
                    foreach (var proc in procs)
                    {
                        try { proc.Kill(); } catch { }
                    }
                }
                catch { }

                m_bInit = false;
                Application.DoEvents();
                System.Threading.Thread.Sleep(500); // 等待清理完成

                System.Diagnostics.Debug.WriteLine("舊控件清理完成");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清理過程發生錯誤：{ex.Message}");
            }
        }

        // ===== 雷射功率頁籤事件 =====

        private void trkPower_Scroll(object sender, EventArgs e)
        {
            numPower.Value = trkPower.Value;
        }

        private void numPower_ValueChanged(object sender, EventArgs e)
        {
            trkPower.Value = (int)Math.Round(numPower.Value);
        }

        private void chkWobble_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = chkWobble.Checked;
            lblWobbleWidth.Enabled = enabled;
            txtWobbleWidth.Enabled = enabled;
            lblWobbleOverlap.Enabled = enabled;
            txtWobbleOverlap.Enabled = enabled;
            lblWobbleSpeed.Enabled = enabled;
            txtWobbleSpeed.Enabled = enabled;
        }

        /// <summary>
        /// 從 UI 讀取雷射參數並套用到指定晶片板的所有物件
        /// </summary>
        /// <param name="boardIndex">晶片板編號 0-3</param>
        /// <returns>成功回傳 true，參數無效或失敗回傳 false</returns>
        private bool ApplyLaserParamsFromUI(int boardIndex)
        {
            if (!m_bInit || !m_bBoardInit[boardIndex])
                return false;

            // 讀取 UI 參數
            double power = (double)numPower.Value;

            if (!double.TryParse(txtSpeed.Text.Trim(), out double speed) || speed <= 0)
            {
                MessageBox.Show("請輸入有效的速度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!double.TryParse(txtFrequency.Text.Trim(), out double frequency) || frequency <= 0)
            {
                MessageBox.Show("請輸入有效的頻率值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!double.TryParse(txtPulseWidth.Text.Trim(), out double pulseWidth) || pulseWidth < 0)
            {
                MessageBox.Show("請輸入有效的脈波寬度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            long markRepeat = (long)numMarkRepeat.Value;

            // 擺動參數
            bool wobbleEnabled = chkWobble.Checked;
            double wobbleWidth = 0;
            double wobbleSpeed = 5026.55;
            if (wobbleEnabled)
            {
                if (!double.TryParse(txtWobbleWidth.Text.Trim(), out wobbleWidth) || wobbleWidth <= 0)
                {
                    MessageBox.Show("請輸入有效的擺動寬度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (!double.TryParse(txtWobbleSpeed.Text.Trim(), out wobbleSpeed) || wobbleSpeed <= 0)
                {
                    MessageBox.Show("請輸入有效的擺動速度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            int wobbleFreqCalc = wobbleEnabled && wobbleWidth > 0
                ? (int)(wobbleSpeed / (Math.PI * wobbleWidth))
                : 0;

            try
            {
                m_MMMark[boardIndex].SelectAllObjects();
                long objCount = m_MMMark[boardIndex].SelectGetCount();

                if (objCount == 0)
                    return true; // 沒有物件，不算失敗

                for (int i = 0; i < objCount; i++)
                {
                    string objName = "";
                    m_MMMark[boardIndex].SelectEnum(i, ref objName);

                    if (string.IsNullOrEmpty(objName))
                        continue;

                    m_MMMark[boardIndex].SetPower(objName, power);
                    m_MMMark[boardIndex].SetSpeed(objName, speed);
                    m_MMMark[boardIndex].SetFrequency(objName, frequency);
                    m_MMMark[boardIndex].SetPulseWidth(objName, pulseWidth);
                    m_MMMark[boardIndex].SetMarkRepeat(objName, (int)markRepeat);
                    m_MMMark[boardIndex].SetWobble(objName, wobbleEnabled ? wobbleWidth : 0, wobbleFreqCalc);
                    m_MMMark[boardIndex].SetWobbleSwitch(objName, wobbleEnabled ? 1 : 0);
                }

                m_MMMark[boardIndex].Redraw();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"套用參數失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnApplyLaser_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先在「連接設定」頁簽初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = comboBoardLaser.SelectedIndex;

            if (!m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 讀取 UI 參數
            double power = (double)numPower.Value;

            if (!double.TryParse(txtSpeed.Text.Trim(), out double speed) || speed <= 0)
            {
                MessageBox.Show("請輸入有效的速度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(txtFrequency.Text.Trim(), out double frequency) || frequency <= 0)
            {
                MessageBox.Show("請輸入有效的頻率值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(txtPulseWidth.Text.Trim(), out double pulseWidth) || pulseWidth < 0)
            {
                MessageBox.Show("請輸入有效的脈波寬度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            long markRepeat = (long)numMarkRepeat.Value;

            // 擺動參數
            bool wobbleEnabled = chkWobble.Checked;
            double wobbleWidth = 0;
            double wobbleOverlap = 50.0;
            double wobbleSpeed = 5026.55;
            if (wobbleEnabled)
            {
                if (!double.TryParse(txtWobbleWidth.Text.Trim(), out wobbleWidth) || wobbleWidth <= 0)
                {
                    MessageBox.Show("請輸入有效的擺動寬度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(txtWobbleOverlap.Text.Trim(), out wobbleOverlap) || wobbleOverlap < 0)
                {
                    MessageBox.Show("請輸入有效的重疊率值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(txtWobbleSpeed.Text.Trim(), out wobbleSpeed) || wobbleSpeed <= 0)
                {
                    MessageBox.Show("請輸入有效的擺動速度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // 計算擺動頻率：lFreq = WobbleSpeed / (π × WobbleWidth)
            int wobbleFreqCalc = wobbleEnabled && wobbleWidth > 0
                ? (int)(wobbleSpeed / (Math.PI * wobbleWidth))
                : 0;

            try
            {
                // 選取所有物件
                m_MMMark[boardIndex].SelectAllObjects();
                long objCount = m_MMMark[boardIndex].SelectGetCount();

                if (objCount == 0)
                {
                    txtLaserStatus.Text = "目前沒有任何物件，請先載入 DXF 或繪製圖形。";
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"晶片板 {boardIndex + 1} - 套用參數");
                sb.AppendLine($"功率: {power}%  速度: {speed} mm/s");
                sb.AppendLine($"頻率: {frequency} kHz  脈波寬度: {pulseWidth}");
                sb.AppendLine($"雷射次數: {markRepeat}");
                sb.AppendLine($"擺動: {(wobbleEnabled ? $"啟動 (寬度: {wobbleWidth}, 重疊率: {wobbleOverlap}%, 速度: {wobbleSpeed}, 計算頻率: {wobbleFreqCalc})" : "關閉")}");
                sb.AppendLine(new string('-', 40));

                int successCount = 0;
                for (int i = 0; i < objCount; i++)
                {
                    string objName = "";
                    m_MMMark[boardIndex].SelectEnum(i, ref objName);

                    if (string.IsNullOrEmpty(objName))
                        continue;

                    long r1 = m_MMMark[boardIndex].SetPower(objName, power);
                    long r2 = m_MMMark[boardIndex].SetSpeed(objName, speed);
                    long r3 = m_MMMark[boardIndex].SetFrequency(objName, frequency);
                    long r4 = m_MMMark[boardIndex].SetPulseWidth(objName, pulseWidth);
                    long r5 = m_MMMark[boardIndex].SetMarkRepeat(objName, (int)markRepeat);
                    // 擺動：設定參數（寬度 + 計算頻率）、開關
                    // 頻率 = 擺動速度 / (π × 擺動寬度)，不覆蓋標記速度
                    long r6 = m_MMMark[boardIndex].SetWobble(objName, wobbleEnabled ? wobbleWidth : 0, wobbleFreqCalc);
                    long r7 = m_MMMark[boardIndex].SetWobbleSwitch(objName, wobbleEnabled ? 1 : 0);
                    successCount++;

                    sb.AppendLine($"物件 [{objName}]:");
                    sb.AppendLine($"  Set: Power={r1} Speed={r2} Freq={r3} PW={r4} Repeat={r5} Wobble={r6} WobbleSwitch={r7}");
                    if (r1 != 0 || r2 != 0 || r3 != 0 || r4 != 0 || r5 != 0 || r6 != 0 || r7 != 0)
                        sb.AppendLine($"  ** 有參數設定失敗 (非0=失敗) **");

                    // 讀回擺動參數驗證是否真的寫入
                    if (wobbleEnabled)
                    {
                        double readWt = m_MMMark[boardIndex].GetWobbleThick(objName);
                        long readWf = m_MMMark[boardIndex].GetWobbleFreq(objName);
                        long readWs = m_MMMark[boardIndex].GetWobbleSwitch(objName);
                        sb.AppendLine($"  驗證: WobbleThick={readWt:F3} WobbleFreq={readWf} WobbleSwitch={readWs}");
                    }
                }

                // 套用參數後 Redraw，確保標記引擎載入新設定
                m_MMMark[boardIndex].Redraw();

                sb.AppendLine(new string('-', 40));
                sb.AppendLine($"已套用到 {successCount}/{objCount} 個物件");

                txtLaserStatus.Text = sb.ToString();
                MessageBox.Show($"已將參數套用到 {successCount} 個物件！", "套用成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                txtLaserStatus.Text = $"套用參數失敗：{ex.Message}";
                MessageBox.Show($"套用參數失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReadLaser_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先在「連接設定」頁簽初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = comboBoardLaser.SelectedIndex;

            if (!m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_MMMark[boardIndex].SelectAllObjects();
                long objCount = m_MMMark[boardIndex].SelectGetCount();

                if (objCount == 0)
                {
                    txtLaserStatus.Text = "目前沒有任何物件，請先載入 DXF 或繪製圖形。";
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"晶片板 {boardIndex + 1} - 讀取參數");
                sb.AppendLine($"共 {objCount} 個物件");
                sb.AppendLine(new string('-', 40));

                // 讀取每個物件的參數
                for (int i = 0; i < objCount; i++)
                {
                    string objName = "";
                    m_MMMark[boardIndex].SelectEnum(i, ref objName);

                    if (string.IsNullOrEmpty(objName))
                        continue;

                    double p = m_MMMark[boardIndex].GetPower(objName);
                    double s = m_MMMark[boardIndex].GetSpeed(objName);
                    double f = m_MMMark[boardIndex].GetFrequency(objName);
                    double pw = m_MMMark[boardIndex].GetPulseWidth(objName);
                    long mr = m_MMMark[boardIndex].GetMarkRepeat(objName);
                    double wt = m_MMMark[boardIndex].GetWobbleThick(objName);
                    long wf = m_MMMark[boardIndex].GetWobbleFreq(objName);
                    long ws = m_MMMark[boardIndex].GetWobbleSwitch(objName);

                    sb.AppendLine($"物件 [{objName}]:");
                    sb.AppendLine($"  功率: {p:F1}%");
                    sb.AppendLine($"  速度: {s:F1} mm/s");
                    sb.AppendLine($"  頻率: {f:F1} kHz");
                    sb.AppendLine($"  脈波寬度: {pw:F1}");
                    sb.AppendLine($"  雷射次數: {mr}");
                    // 從頻率反算擺動速度：wobbleSpeed = freq × π × width
                    double wobbleSpeedCalc = wt > 0 ? wf * Math.PI * wt : 0;
                    sb.AppendLine($"  擺動: {(ws != 0 ? "啟動" : "關閉")}  寬度: {wt:F3}  SDK頻率: {wf}  擺動速度: {wobbleSpeedCalc:F1}");
                    sb.AppendLine();

                    // 以第一個物件的值回填到 UI
                    if (i == 0)
                    {
                        numPower.Value = (decimal)Math.Max(0, Math.Min(100, p));
                        txtSpeed.Text = s.ToString("F1");
                        txtFrequency.Text = f.ToString("F1");
                        txtPulseWidth.Text = pw.ToString("F1");
                        numMarkRepeat.Value = Math.Max(1, Math.Min(9999, (decimal)mr));
                        chkWobble.Checked = ws != 0;
                        txtWobbleWidth.Text = wt > 0 ? wt.ToString("F3") : "0.1";
                        // 從 SDK 頻率反算擺動速度：wobbleSpeed = freq × π × width
                        double readWobbleSpeed = wt > 0 ? wf * Math.PI * wt : 5026.55;
                        txtWobbleOverlap.Text = "50.000"; // 重疊率保持預設（SDK 無直接讀取介面）
                        txtWobbleSpeed.Text = readWobbleSpeed.ToString("F2");
                    }
                }

                txtLaserStatus.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtLaserStatus.Text = $"讀取參數失敗：{ex.Message}";
                MessageBox.Show($"讀取參數失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== QR Code 頁籤事件 =====

        /// <summary>
        /// 載入 QR Code 到指定晶片板
        /// </summary>
        private void btnLoadQR_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先在「連接設定」頁簽初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = comboBoardQR.SelectedIndex;

            if (!m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string content = txtQRContent.Text.Trim();
            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("請輸入 QR Code 內容！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(txtQRPosX.Text.Trim(), out double posX))
            {
                MessageBox.Show("請輸入有效的 X 位置值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!double.TryParse(txtQRPosY.Text.Trim(), out double posY))
            {
                MessageBox.Show("請輸入有效的 Y 位置值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!double.TryParse(txtQRWidth.Text.Trim(), out double width) || width <= 0)
            {
                MessageBox.Show("請輸入有效的寬度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!double.TryParse(txtQRHeight.Text.Trim(), out double height) || height <= 0)
            {
                MessageBox.Show("請輸入有效的高度值！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 建立 QR Code 條碼物件
                long result = m_MMMark[boardIndex].AddBarcode(
                    BARCODE_TYPE_QRCODE, content, posX, posY, width, height, "", "");

                if (result != 0)
                {
                    MessageBox.Show($"建立 QR Code 失敗！回傳碼: {result}\n" +
                        "可能需要調整 BARCODE_TYPE_QRCODE 常數值。",
                        "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Application.DoEvents();
                Thread.Sleep(100);

                // 列舉物件取得新建的 QR Code 物件名稱
                m_MMMark[boardIndex].SelectAllObjects();
                long objCount = m_MMMark[boardIndex].SelectGetCount();
                string qrObjName = "";

                if (objCount > 0)
                {
                    m_MMMark[boardIndex].SelectEnum((int)(objCount - 1), ref qrObjName);
                }

                // 套用設定
                if (!string.IsNullOrEmpty(qrObjName))
                {
                    // 反轉黑白
                    m_MMEdit[boardIndex].SetBarcodeInvert(qrObjName, chkQRInvert.Checked ? 1 : 0);
                    // QR 固定容錯等級 LOW（EC=0）
                    m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrObjName, QR_EC_LEVEL_LOW);
                }

                // 重繪畫面
                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Thread.Sleep(300);

                btnMarkQR.Enabled = true;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"QR Code 已載入至晶片板 {boardIndex + 1}");
                sb.AppendLine($"內容: {content}");
                sb.AppendLine($"位置: ({posX}, {posY})  大小: {width}x{height}mm");
                if (!string.IsNullOrEmpty(qrObjName))
                    sb.AppendLine($"物件名稱: {qrObjName}");
                txtQRStatus.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtQRStatus.Text = $"載入 QR Code 失敗：{ex.Message}";
                MessageBox.Show($"載入 QR Code 失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// QR Code: 執行打標
        /// </summary>
        private void btnMarkQR_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int boardIndex = comboBoardQR.SelectedIndex;

                if (!m_bBoardInit[boardIndex])
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsBoardBusy(boardIndex))
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                        "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 打標前自動套用雷射參數
                if (!ApplyLaserParamsFromUI(boardIndex))
                    return;

                m_MMMark[boardIndex].MarkStandBy();

                if (m_MMMark[boardIndex].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 啟動 Timer 來監控打標狀態
                timerMark.Tag = boardIndex;
                timerMark.Start();

                btnMarkQR.Enabled = false;
                btnStopMarkQR.Enabled = true;
                btnLoadQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnClearQR.Enabled = false;
                // 停用其他頁籤的打標按鈕
                btnMarkDXF.Enabled = false;
                btnMark.Enabled = false;
                btnStop.Enabled = true;

                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 正在打標...";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// QR Code: 停止打標
        /// </summary>
        private void btnStopMarkQR_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            try
            {
                int boardIndex = comboBoardQR.SelectedIndex;
                if (m_bBoardInit[boardIndex])
                {
                    m_MMMark[boardIndex].StopMarking();
                }
                timerMark.Stop();

                // 恢復按鈕狀態
                btnMarkQR.Enabled = true;
                btnStopMarkQR.Enabled = false;
                btnLoadQR.Enabled = true;
                btnPreviewQR.Enabled = true;
                btnStopPreviewQR.Enabled = false;
                btnClearQR.Enabled = true;
                btnQRWhiteBgMark.Enabled = true;
                btnMarkDXF.Enabled = true;
                btnMark.Enabled = true;
                btnStop.Enabled = false;

                txtQRStatus.Text = "打標已停止。";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止打標失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// QR Code: 紅光預覽
        /// </summary>
        private void btnPreviewQR_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int boardIndex = comboBoardQR.SelectedIndex;

                if (!m_bBoardInit[boardIndex])
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsBoardBusy(boardIndex))
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                        "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 設定預覽模式（全路徑預覽）並啟動紅光預覽
                m_MMMark[boardIndex].SetPreviewMode(2);
                m_MMMark[boardIndex].MarkStandBy();
                Application.DoEvents();

                if (m_MMMark[boardIndex].StartMarking(3) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 預覽啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_bPreviewing = true;
                m_iPreviewBoard = boardIndex;

                // 啟動 15 秒自動關閉 Timer
                timerPreview.Stop();
                timerPreview.Start();

                // 停用按鈕
                btnMarkQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnStopPreviewQR.Enabled = true;
                btnLoadQR.Enabled = false;
                btnClearQR.Enabled = false;
                btnMarkDXF.Enabled = false;
                btnPreviewDXF.Enabled = false;
                btnMark.Enabled = false;
                btnPreviewManual.Enabled = false;
                btnStop.Enabled = true;

                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 紅光預覽中...（15秒後自動停止）";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// QR Code: 停止紅光預覽
        /// </summary>
        private void btnStopPreviewQR_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            try
            {
                timerPreview.Stop();

                int boardIndex = (m_iPreviewBoard >= 0) ? m_iPreviewBoard : comboBoardQR.SelectedIndex;
                if (boardIndex >= 0 && boardIndex < m_bBoardInit.Length && m_bBoardInit[boardIndex])
                {
                    m_MMMark[boardIndex].StopMarking();
                }
                m_bPreviewing = false;
                m_iPreviewBoard = -1;

                ResetPreviewButtonsAfterStop();
                txtQRStatus.Text = "預覽已停止。";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// QR Code: 清除畫面
        /// </summary>
        private void btnClearQR_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            int boardIndex = comboBoardQR.SelectedIndex;
            if (!m_bBoardInit[boardIndex]) return;

            try
            {
                // 刪除所有物件後重繪
                // ResetFile 後須重新 SetDesktopCenter/SetDesktopSize，保持工作區與 m_WorkspaceSize 一致，
                // 後續若立即 AddLine/AddBarcode 才不會被 OCX 判定為「超出工作範圍」。
                m_MMMark[boardIndex].ResetFile();
                m_MMMark[boardIndex].SetDesktopCenter(0, 0);
                m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();

                btnMarkQR.Enabled = false;
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 已清除。";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取得目前畫面上「最後加入」的物件名稱（一般等同剛 AddBarcode/AddRect 加入的那個）。
        /// 用 SelectAllObjects + SelectGetCount + SelectEnum(count-1) 取得。
        /// </summary>
        private string GetLastAddedObjectName(int boardIndex)
        {
            m_MMMark[boardIndex].SelectAllObjects();
            long count = m_MMMark[boardIndex].SelectGetCount();
            if (count <= 0) return string.Empty;
            string name = "";
            m_MMMark[boardIndex].SelectEnum((int)(count - 1), ref name);
            return name ?? string.Empty;
        }

        // ===== QR / 2D barcode 輔助方法 =====
        //
        // 專案引用的 wrapper（C:\Program Files (x86)\MarkingMate\MmAssembly\AxMMEdit_x64_1.dll）
        // 已包含 Set2DBarcodeBorder / Get2DBarcodeBorder / SetBarcodeLineExtend /
        // GetBarcodeLineExtend（與 Set2DBarcodeQRECLevel、Set2DBarcodeFixedCellSize 同一批）。
        // 直接強型別呼叫即可，不需晚期繫結。
        //
        // wrapper 簽章（AxMMEditx641，實測反射得出）：
        //   int    Set2DBarcodeBorder  (string strName, int    lBorder)
        //   int    Get2DBarcodeBorder  (string strName)
        //   int    SetBarcodeLineExtend(string strName, double dLineExtend)
        //   double GetBarcodeLineExtend(string strName)

        /// <summary>設定 QR 外框單元數（單位＝cell/模組，整數）。</summary>
        private int SetQRBorder(int boardIndex, string objName, int borderCells)
            => m_MMEdit[boardIndex].Set2DBarcodeBorder(objName, borderCells);

        /// <summary>讀回 QR 外框單元數。</summary>
        private int GetQRBorder(int boardIndex, string objName)
            => m_MMEdit[boardIndex].Get2DBarcodeBorder(objName);

        /// <summary>設定條碼線段延伸量（配合 SetBarcodeMarkStyle 線段填滿使用）。</summary>
        private int SetQRLineExtend(int boardIndex, string objName, double lineExtend)
            => m_MMEdit[boardIndex].SetBarcodeLineExtend(objName, lineExtend);

        /// <summary>讀回條碼線段延伸量。</summary>
        private double GetQRLineExtend(int boardIndex, string objName)
            => m_MMEdit[boardIndex].GetBarcodeLineExtend(objName);

        /// <summary>
        /// 套用 QR 外框單元 + 線段延伸，並把 Border / QuietZone / 渲染尺寸一起 log 出來。
        /// Border（新 API，整數 cell）與 QuietZone（舊 API，double）是否為同一個底層屬性，
        /// SDK 文件沒寫明 — 第一次上機時看這行 log 的 readback 即可確認。
        /// </summary>
        private void ApplyQRBorder(int boardIndex, string objName, int borderCells)
        {
            long rcBorder = SetQRBorder(boardIndex, objName, borderCells);
            long rcExtend = SetQRLineExtend(boardIndex, objName, QR_LINE_EXTEND);

            Console.Error.WriteLine(
                $"[Board {boardIndex + 1}] Set2DBarcodeBorder({borderCells}) rc={rcBorder} " +
                $"SetBarcodeLineExtend({QR_LINE_EXTEND}) rc={rcExtend} → " +
                $"readback Border={GetQRBorder(boardIndex, objName)} " +
                $"QuietZone={m_MMEdit[boardIndex].GetBarcodeQuietZone(objName)} " +
                $"LineExtend={GetQRLineExtend(boardIndex, objName)}");
        }

        /// <summary>
        /// QRCODE_白底：依使用者 TextBox 設定建立 QR Code（不建立矩形、不打標）。
        /// 主要供使用者預覽 / 確認 QR 渲染後實際大小。
        /// </summary>
        private void btnQRWhiteBgCreate_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = comboBoardQR.SelectedIndex;
            if (!m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsBoardBusy(boardIndex))
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                    "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 讀取 TextBox（與 Mark handler 同步）
            if (!double.TryParse(txtWBQRSpeed.Text.Trim(), out double qrSpeed)) qrSpeed = 1000;
            if (!double.TryParse(txtWBQRPower.Text.Trim(), out double qrPower)) qrPower = 80;
            if (!double.TryParse(txtWBQRWidth.Text.Trim(), out double qrWidth) || qrWidth <= 0) qrWidth = 15;
            if (!double.TryParse(txtWBQRHeight.Text.Trim(), out double qrHeight) || qrHeight <= 0) qrHeight = 15;
            // 外框單元：Set2DBarcodeBorder 的 lBorder 是整數 cell 數，不是 mm
            if (!int.TryParse(txtWBQuietZone.Text.Trim(), out int qrBorder) || qrBorder < 0) qrBorder = 2;

            try
            {
                // 清空畫面
                m_MMMark[boardIndex].ResetFile();
                m_MMMark[boardIndex].SetDesktopCenter(0, 0);
                m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(200);

                // AddBarcode QR
                string qrName = "QRWhiteBg_QR";
                long rQR = m_MMMark[boardIndex].AddBarcode(
                    BARCODE_TYPE_QRCODE, "1234567", 0, 0, qrWidth, qrHeight, "", qrName);
                if (rQR != 0)
                {
                    MessageBox.Show($"建立 QR Code 失敗！回傳碼: {rQR}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Application.DoEvents();
                Thread.Sleep(200);

                // 套用屬性 + 雷射參數
                m_MMEdit[boardIndex].SetBarcodeInvert(qrName, 1);              // 反相打開
                m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrName, QR_EC_LEVEL_LOW);
                m_MMEdit[boardIndex].Set2DBarcodeFixedType(qrName, 0);
                m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, 1, 1);
                ApplyQRBorder(boardIndex, qrName, qrBorder);
                m_MMEdit[boardIndex].SetBarcodeMarkStyle(qrName, 2);
                m_MMEdit[boardIndex].SetBarcodeLineType(qrName, 0);
                m_MMEdit[boardIndex].SetBarcodeSpotSize(qrName, 0.04);         // 0.02 → 0.04
                m_MMEdit[boardIndex].SetBarcodeLineTimes(qrName, 4);           // 1 → 4
                m_MMEdit[boardIndex].SetFillStartAngle(qrName, 0);
                m_MMEdit[boardIndex].SetFillStepAngle(qrName, 45);             // 90 → 45
                m_MMEdit[boardIndex].SetBarcodeLineTwoway(qrName, 1);
                m_MMEdit[boardIndex].SetFrameSwitch(qrName, 1);
                m_MMEdit[boardIndex].SetFillSwitch(qrName, 1);
                m_MMEdit[boardIndex].SetFillFirstExt(qrName, 0, 1);
                m_MMMark[boardIndex].SetSpeed(qrName, qrSpeed);
                m_MMMark[boardIndex].SetPower(qrName, qrPower);
                m_MMMark[boardIndex].SetFrequency(qrName, 80);                 // 200 → 80
                m_MMMark[boardIndex].SetMarkRepeat(qrName, 1);
                m_MMEdit[boardIndex].SetBarcodeSpotDelay(qrName, 1000);
                m_MMMark[boardIndex].SetPulseWidth(qrName, 30);                // 13 → 30

                // 第一次 Redraw → 拿 QR Version → 反推 cellSize
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(300);

                long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(qrName);
                if (qrVersion < 1) qrVersion = 1;
                int modules = 17 + 4 * (int)qrVersion;
                // Option B：UI 長寬 = 模組區大小，cellSize 只由 modules 決定，外框會額外加大 QR 總尺寸
                double cellW = qrWidth / modules;
                double cellH = qrHeight / modules;

                m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, cellW, cellH);
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(300);

                double qrActualW = m_MMEdit[boardIndex].GetWidth(qrName);
                double qrActualH = m_MMEdit[boardIndex].GetHeight(qrName);

                // 產生序號並顯示 + 建立序號文字物件（位置依 QUEST3 border × cellW 邏輯）
                string serial = NextSerial(WhiteBgCounterFileName);
                if (txtWBSerial != null) txtWBSerial.Text = serial;

                double qrModuleTopY = qrHeight / 2.0;
                double borderZoneWidth = qrBorder * cellW;
                double borderTopY = qrModuleTopY + borderZoneWidth;
                double serialFontSize = Math.Max(qrHeight / 8.0, 1.5);
                double serialCx = -qrWidth / 2.0;
                double serialCy;
                if (qrBorder > 0 && borderZoneWidth > 0)
                {
                    const double marginAboveBorder = 0.5;
                    serialCy = borderTopY + serialFontSize / 2.0 + marginAboveBorder;
                }
                else
                {
                    serialCy = qrModuleTopY + serialFontSize / 2.0 + 1.0;
                }

                string serialName = "QRWhiteBg_Serial";
                long rT = m_MMMark[boardIndex].AddText(serial, serialCx, serialCy, "", serialName);
                Console.Error.WriteLine(
                    $"[Board {boardIndex + 1}] WhiteBg Create AddText serial=\"{serial}\" rc={rT} @({serialCx:F2},{serialCy:F2}) h={serialFontSize:F2}");
                if (rT == 0)
                {
                    try
                    {
                        m_MMEdit[boardIndex].SetFontSize(serialName, serialFontSize);
                        m_MMMark[boardIndex].SetSpeed(serialName, qrSpeed);
                        m_MMMark[boardIndex].SetPower(serialName, qrPower);
                        m_MMMark[boardIndex].SetFrequency(serialName, 200);
                        m_MMMark[boardIndex].SetMarkRepeat(serialName, 2);
                        m_MMMark[boardIndex].SetPulseWidth(serialName, 13);
                    }
                    catch (Exception exS)
                    {
                        Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg Create serial text 屬性設定失敗（略過）: {exS.Message}");
                    }
                }

                // Redraw 讓序號文字顯示在畫面
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(200);

                txtQRStatus.Text =
                    $"晶片板 {boardIndex + 1} 已建立 QR Code\r\n" +
                    $"序號: {serial}\r\n" +
                    $"目標尺寸: {qrWidth} × {qrHeight} mm\r\n" +
                    $"渲染尺寸: {qrActualW:F2} × {qrActualH:F2} mm\r\n" +
                    $"QR Version: {qrVersion} (modules={modules})\r\n" +
                    $"單元寬高: {cellW:F4} × {cellH:F4} mm\r\n" +
                    $"外框: {qrBorder} 單元";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"建立 QR Code 失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// QRCODE_白底：一鍵建立並打標兩個圖層（順序：先矩形後 QR）。
        /// Layer 1（先打標）：4cm x 4cm 矩形外框（短虛線+點虛線填滿），形成白底
        /// Layer 2（後打標）：反相 QR Code "AAA"，30x30mm，連續線段填滿
        /// 兩個物件各自設定不同的雷射參數 → StartMarking(4) 依加入順序連續打標。
        ///
        /// 注意（暫定值，依實測調整）：
        ///   - SetBarcodeMarkStyle = 3 對應「連續線段」
        ///   - SetFrameLineType = 1 對應「短虛線」
        ///   - SetFillStyle = 0（用戶明示，含義依 SDK 解釋）
        ///   - SetBarcodeSpotDelay 內部單位假設為 μs：1ms=1000, 0.1ms=100
        /// </summary>
        private void btnQRWhiteBgMark_Click(object sender, EventArgs e) => ExecuteQRWhiteBg(isPreview: false);
        private void btnQRWhiteBgPreview_Click(object sender, EventArgs e) => ExecuteQRWhiteBg(isPreview: true);

        /// <summary>白底 QR：停止紅光預覽 / 打標。</summary>
        private void btnQRWhiteBgStopPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;
            try
            {
                timerMark.Stop();
                timerPreview.Stop();
                for (int i = 0; i < m_bBoardInit.Length; i++)
                {
                    if (m_bBoardInit[i])
                    {
                        try { m_MMMark[i].StopMarking(); } catch { }
                    }
                }
                m_bPreviewing = false;
                m_iPreviewBoard = -1;
                ResetPreviewButtonsAfterStop();
                btnQRWhiteBgMark.Enabled = true;
                btnQRWhiteBgPreview.Enabled = true;
                txtQRStatus.Text = "白底 QR 已停止。";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 白底 QR 雙圖層核心流程：建立矩形 / QR / 序號文字 → 依 isPreview 決定 StartMarking(3) 紅光預覽或 StartMarking(4) 正常打標。
        /// 兩個按鈕（打標、紅光預覽）共用此方法。
        /// </summary>
        private void ExecuteQRWhiteBg(bool isPreview)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = comboBoardQR.SelectedIndex;
            if (!m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsBoardBusy(boardIndex))
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                    "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 讀取 TextBox 內的可調參數（解析失敗則用預設值）
            if (!double.TryParse(txtWBRectSpeed.Text.Trim(), out double rectSpeed)) rectSpeed = 800;
            if (!double.TryParse(txtWBRectPower.Text.Trim(), out double rectPower)) rectPower = 100;
            if (!double.TryParse(txtWBQRSpeed.Text.Trim(), out double qrSpeed)) qrSpeed = 1000;
            if (!double.TryParse(txtWBQRPower.Text.Trim(), out double qrPower)) qrPower = 80;
            if (!double.TryParse(txtWBQRWidth.Text.Trim(), out double qrWidth) || qrWidth <= 0) qrWidth = 15;
            if (!double.TryParse(txtWBQRHeight.Text.Trim(), out double qrHeight) || qrHeight <= 0) qrHeight = 15;
            // 外框單元：Set2DBarcodeBorder 的 lBorder 是整數 cell 數，不是 mm
            if (!int.TryParse(txtWBQuietZone.Text.Trim(), out int qrBorder) || qrBorder < 0) qrBorder = 2;
            if (!double.TryParse(txtWBRectExtra.Text.Trim(), out double rectExtra)) rectExtra = 0;
            // 矩形長寬將在 QR 建立後從 GetWidth/GetHeight 動態取得 + rectExtra (TextBox 設定的 X)

            // 依 RadioButton 決定要打哪些圖層
            bool markQR = rdoWBMarkQR.Checked || rdoWBMarkAll.Checked;
            bool markRect = rdoWBMarkRect.Checked || rdoWBMarkAll.Checked;
            if (!markQR && !markRect)
            {
                MessageBox.Show("請至少選擇 QR、矩形 或 全部 其中一項作為打標目標！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // === 0. 清空畫面（ResetFile 後一定要 Redraw 並等待 OCX 同步，
                //         否則後續 AddBarcode + SelectAllObjects 取不到新物件） ===
                m_MMMark[boardIndex].ResetFile();
                m_MMMark[boardIndex].SetDesktopCenter(0, 0);
                m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(200);

                // === 依 RadioButton 選擇性建立 QR / 矩形 / 序號文字 ===
                string qrName = "QRWhiteBg_QR";
                string rectName = "QRWhiteBg_Rect";
                string serialName = "QRWhiteBg_Serial";

                // 矩形基準尺寸：預設 UI 設定值；若 QR 有建立則改用 QR 實際渲染尺寸（消除 SDK cellSize 捨入誤差）
                double rectBaseW = qrWidth;
                double rectBaseH = qrHeight;

                // cellW / cellH 提升到外層 scope，讓後面序號文字位置也能用（依 border × cellW 計算）
                double cellW = 0, cellH = 0;

                if (markQR)
                {
                    // 1) 建立 QR Code 物件（先用 cellSize=1 占位）
                    long rQR = m_MMMark[boardIndex].AddBarcode(
                        BARCODE_TYPE_QRCODE, "1234567", 0, 0, qrWidth, qrHeight, "", qrName);
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] AddBarcode rc={rQR} name=[{qrName}]");
                    if (rQR != 0)
                    {
                        MessageBox.Show($"建立 QR Code 失敗！回傳碼: {rQR}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Application.DoEvents();
                    Thread.Sleep(200);

                    // 2) QR 屬性 + 雷射參數
                    m_MMEdit[boardIndex].SetBarcodeInvert(qrName, 1);          // 反相打開
                    m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrName, QR_EC_LEVEL_LOW);
                    m_MMEdit[boardIndex].Set2DBarcodeFixedType(qrName, 0);
                    m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, 1, 1);
                    ApplyQRBorder(boardIndex, qrName, qrBorder);
                    m_MMEdit[boardIndex].SetBarcodeMarkStyle(qrName, 2);
                    m_MMEdit[boardIndex].SetBarcodeLineType(qrName, 0);
                    m_MMEdit[boardIndex].SetBarcodeSpotSize(qrName, 0.04);     // 0.02 → 0.04
                    m_MMEdit[boardIndex].SetBarcodeLineTimes(qrName, 4);       // 1 → 4
                    m_MMEdit[boardIndex].SetFillStartAngle(qrName, 0);
                    m_MMEdit[boardIndex].SetFillStepAngle(qrName, 45);         // 90 → 45
                    m_MMEdit[boardIndex].SetBarcodeLineTwoway(qrName, 1);
                    m_MMEdit[boardIndex].SetFrameSwitch(qrName, 1);
                    m_MMEdit[boardIndex].SetFillSwitch(qrName, 1);
                    m_MMEdit[boardIndex].SetFillFirstExt(qrName, 0, 1);
                    m_MMMark[boardIndex].SetSpeed(qrName, qrSpeed);
                    m_MMMark[boardIndex].SetPower(qrName, qrPower);
                    m_MMMark[boardIndex].SetFrequency(qrName, 80);             // 200 → 80
                    m_MMMark[boardIndex].SetMarkRepeat(qrName, 1);
                    m_MMEdit[boardIndex].SetBarcodeSpotDelay(qrName, 1000);
                    m_MMMark[boardIndex].SetPulseWidth(qrName, 30);             // 13 → 30

                    // 3) Redraw → 反推 cellSize 讓 QR 渲染等於 UI 設定
                    m_MMMark[boardIndex].Redraw();
                    Application.DoEvents();
                    Thread.Sleep(300);
                    long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(qrName);
                    if (qrVersion < 1) qrVersion = 1;
                    int modules = 17 + 4 * (int)qrVersion;
                    // Option B：UI 長寬 = 模組區大小，cellSize 只由 modules 決定，外框會額外加大 QR 總尺寸
                    cellW = qrWidth / modules;
                    cellH = qrHeight / modules;
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] QR Version={qrVersion} modules={modules} border={qrBorder} → cellW={cellW:F4} cellH={cellH:F4}");
                    m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, cellW, cellH);
                    m_MMMark[boardIndex].Redraw();
                    Application.DoEvents();
                    Thread.Sleep(300);

                    // 取 QR 實際渲染尺寸作為矩形基準（避免 SDK cellSize 內部捨入造成的視覺不對齊）
                    double actualW = m_MMEdit[boardIndex].GetWidth(qrName);
                    double actualH = m_MMEdit[boardIndex].GetHeight(qrName);
                    if (actualW > 0) rectBaseW = actualW;
                    if (actualH > 0) rectBaseH = actualH;
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] QR rendered = {actualW}x{actualH} (UI {qrWidth}x{qrHeight}) → rectBase={rectBaseW}x{rectBaseH}");
                }

                if (markRect)
                {
                    // 4) 矩形 SIZE = QR 實際渲染大小 + X（若沒有 QR，fallback 用 UI 值）
                    double rectW = rectBaseW + rectExtra;
                    double rectH = rectBaseH + rectExtra;
                    double rectHalfW = rectW / 2.0;
                    double rectHalfH = rectH / 2.0;
                    long rR = m_MMEdit[boardIndex].AddRect(-rectHalfW, -rectHalfH, rectHalfW, rectHalfH, 0, "", rectName);
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] AddRect rc={rR} name=[{rectName}] size={rectW}x{rectH} (base={rectBaseW}x{rectBaseH} + X={rectExtra})");
                    if (rR != 0)
                    {
                        MessageBox.Show($"建立矩形失敗！回傳碼: {rR}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Application.DoEvents();
                    Thread.Sleep(200);

                    // 5) 矩形屬性 + 雷射參數
                    m_MMEdit[boardIndex].SetFillStyle(rectName, 3);             // 1 → 3
                    m_MMEdit[boardIndex].SetFrameLineType(rectName, 1);
                    m_MMEdit[boardIndex].SetFillRoundPitch(rectName, 0.04);
                    m_MMEdit[boardIndex].SetFillPitch(rectName, 0.04);
                    m_MMEdit[boardIndex].SetFillTimes(rectName, 4);             // 1 → 4
                    m_MMEdit[boardIndex].SetFillInsideOut(rectName, 1);         // 向內
                    m_MMEdit[boardIndex].SetFillStartAngle(rectName, 90);
                    m_MMEdit[boardIndex].SetFillStepAngle(rectName, 45);
                    m_MMEdit[boardIndex].SetFillAverageDistribution(rectName, 1);
                    m_MMEdit[boardIndex].SetFrameSwitch(rectName, 1);
                    m_MMEdit[boardIndex].SetFillSwitch(rectName, 1);
                    m_MMEdit[boardIndex].SetFillFirstExt(rectName, 0, 1);
                    m_MMMark[boardIndex].SetSpeed(rectName, rectSpeed);
                    m_MMMark[boardIndex].SetPower(rectName, rectPower);
                    m_MMMark[boardIndex].SetFrequency(rectName, 80);            // 20 → 80
                    m_MMMark[boardIndex].SetMarkRepeat(rectName, 1);
                    m_MMEdit[boardIndex].SetBarcodeSpotDelay(rectName, 100);
                    m_MMMark[boardIndex].SetPulseWidth(rectName, 250);          // 200 → 250
                }

                // 6) 若兩者都建立，矩形要先打標
                if (markQR && markRect)
                {
                    long rOrd = m_MMEdit[boardIndex].ChangeObjectOrder(rectName, qrName, 0);
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] ChangeObjectOrder rect→before(qr) rc={rOrd}");
                }

                // 7) 取序號（提前，讓文字物件可用序號當內容）
                string serial = NextSerial(WhiteBgCounterFileName);
                if (txtWBSerial != null) txtWBSerial.Text = serial;

                // 7.5) 新增序號文字物件（位置依 QUEST3 邏輯：border × cellW 顯式計算）
                //      文字最下緣 > border 最高 y（若 border=0 fallback 到 QR 模組頂邊 + 1mm）
                double effCellW = cellW > 0 ? cellW : qrWidth / 21.0;   // 若 QR 沒建立，用 Version 1 假設
                double qrModuleTopY = qrHeight / 2.0;
                double borderZoneWidth = qrBorder * effCellW;
                double borderTopY = qrModuleTopY + borderZoneWidth;
                double serialFontSize = Math.Max(qrHeight / 8.0, 1.5);
                double serialCx = -qrWidth / 2.0;
                double serialCy;
                if (qrBorder > 0 && borderZoneWidth > 0)
                {
                    const double marginAboveBorder = 0.5;
                    serialCy = borderTopY + serialFontSize / 2.0 + marginAboveBorder;
                }
                else
                {
                    serialCy = qrModuleTopY + serialFontSize / 2.0 + 1.0;
                }

                long rT = m_MMMark[boardIndex].AddText(serial, serialCx, serialCy, "", serialName);
                Console.Error.WriteLine(
                    $"[Board {boardIndex + 1}] WhiteBg AddText serial=\"{serial}\" rc={rT} @({serialCx:F2},{serialCy:F2}) h={serialFontSize:F2} border={qrBorder} cellW={effCellW:F4}");
                bool serialAdded = (rT == 0);
                if (serialAdded)
                {
                    try
                    {
                        m_MMEdit[boardIndex].SetFontSize(serialName, serialFontSize);
                        m_MMMark[boardIndex].SetSpeed(serialName, qrSpeed);
                        m_MMMark[boardIndex].SetPower(serialName, qrPower);
                        m_MMMark[boardIndex].SetFrequency(serialName, 200);
                        m_MMMark[boardIndex].SetMarkRepeat(serialName, 2);
                        m_MMMark[boardIndex].SetPulseWidth(serialName, 13);
                    }
                    catch (Exception exS)
                    {
                        Console.Error.WriteLine($"[Board {boardIndex + 1}] WhiteBg serial text 屬性設定失敗（略過）: {exS.Message}");
                    }
                }

                // 8) 最終 Redraw
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(300);

                double estTotalSec = 0, estTotalLen = 0, estRectSec = 0, estQrSec = 0, estSerialSec = 0;
                try
                {
                    estTotalSec = m_MMMark[boardIndex].GetEstimatedTotalTime();
                    estTotalLen = m_MMMark[boardIndex].GetEstimatedTotalLength();
                    if (markRect) estRectSec = m_MMMark[boardIndex].GetSKWObjTime(rectName);
                    if (markQR) estQrSec = m_MMMark[boardIndex].GetSKWObjTime(qrName);
                    if (serialAdded) estSerialSec = m_MMMark[boardIndex].GetSKWObjTime(serialName);
                    Console.Error.WriteLine(
                        $"[Board {boardIndex + 1}] WhiteBg time est: total={estTotalSec:F2}s len={estTotalLen:F2}mm " +
                        $"rect={estRectSec:F2}s qr={estQrSec:F2}s serial={estSerialSec:F2}s");
                }
                catch (Exception exT) { Console.Error.WriteLine($"WhiteBg est time 失敗: {exT.Message}"); }

                WriteWhiteBgParaLog(serial, boardIndex,
                    markQR, markRect,
                    qrWidth, qrHeight, qrBorder, rectExtra,
                    qrSpeed, qrPower,
                    rectSpeed, rectPower,
                    estTotalSec, estTotalLen, estRectSec, estQrSec, estSerialSec);

                // 9) 打標 or 紅光預覽
                if (isPreview)
                {
                    m_MMMark[boardIndex].SetPreviewMode(2);
                    m_MMMark[boardIndex].MarkStandBy();
                    if (m_MMMark[boardIndex].StartMarking(3) != 0)
                    {
                        MessageBox.Show($"晶片板 {boardIndex + 1} 預覽啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    m_bPreviewing = true;
                    m_iPreviewBoard = boardIndex;
                    timerPreview.Tag = new object[] { boardIndex, 15 };
                    timerPreview.Start();
                }
                else
                {
                    m_MMMark[boardIndex].MarkStandBy();
                    if (m_MMMark[boardIndex].StartMarking(4) != 0)
                    {
                        MessageBox.Show($"晶片板 {boardIndex + 1} 打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    timerMark.Tag = boardIndex;
                    timerMark.Start();
                }

                // 停用相關按鈕
                btnQRWhiteBgMark.Enabled = false;
                btnQRWhiteBgPreview.Enabled = false;
                btnMarkQR.Enabled = false;
                btnLoadQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnClearQR.Enabled = false;
                btnStopMarkQR.Enabled = true;
                btnMarkDXF.Enabled = false;
                btnMark.Enabled = false;
                btnStop.Enabled = true;

                string targetDesc = (markQR && markRect) ? $"先 {rectName} 後 {qrName}"
                                   : markQR ? qrName
                                   : rectName;
                string actionDesc = isPreview ? "紅光預覽" : "打標";
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} {actionDesc}中（{targetDesc}），序號 {serial}，預估 {estTotalSec:F1}s";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"白底 QR 打標失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === 鋼鐵 Quest3 QR 序號 & 參數記錄 =====================================
        // 序號格式 YYYYMMDD-NNN。同日 NNN 累加（001,002,003...），換日自動重置為 001。
        // 狀態存在 QRCodePara\counter.txt，內容一行 "YYYYMMDD NNN"。

        private const string QRCodeCounterFileName = "counter.txt";
        private const string WhiteBgCounterFileName = "counter_whitebg.txt";
        private const string WhiteBgFilePrefix = "QRCODE_白底";

        /// <summary>
        /// 取得下一組序號（YYYYMMDD-NNN）。counterFile 決定使用哪個計數器檔（Steel/白底 分別維護）。
        /// </summary>
        private static string NextSerial(string counterFile)
        {
            string dir = GetQRCodeParaDir();
            Directory.CreateDirectory(dir);
            string counterPath = Path.Combine(dir, counterFile);

            string today = DateTime.Now.ToString("yyyyMMdd");
            int nextSeq = 1;
            if (File.Exists(counterPath))
            {
                try
                {
                    string line = File.ReadAllText(counterPath, Encoding.UTF8).Trim();
                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && parts[0] == today && int.TryParse(parts[1], out int lastSeq))
                        nextSeq = lastSeq + 1;
                }
                catch { /* 檔案壞掉就從 001 重來 */ }
            }
            File.WriteAllText(counterPath, $"{today} {nextSeq}", Encoding.UTF8);
            return $"{today}-{nextSeq:D3}";
        }

        /// <summary>取得 QRCodePara 資料夾絕對路徑（跟 exe 同層，跨機器都能用）。</summary>
        private static string GetQRCodeParaDir()
        {
            string exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.Combine(exeDir, "QRCodePara");
        }

        /// <summary>
        /// 取得下一組鋼鐵 Quest3 QR 序號（YYYYMMDD-NNN）。
        /// </summary>
        private static string NextSteelSerial() => NextSerial(QRCodeCounterFileName);

        /// <summary>
        /// 將本次鋼鐵 Quest3 QR 使用的所有參數寫成 txt，檔名為序號.txt。
        /// action = "PREVIEW" 或 "MARK"，便於事後回溯。
        /// </summary>
        private void WriteSteelQRParaLog(string serial, string action, string content,
            int boardIndex,
            double qrWidth, double qrHeight, int border, double rectExtra,
            int ecLevel, int markStyle, double spotSize, int qrRepeat,
            double rectPower, double rectSpeed, double rectFreq, int rectRepeat,
            double qrPower, double qrSpeed, double qrFreq, double qrPulseWidth,
            double estTotalSec = 0, double estTotalLen = 0,
            double estRectSec = 0, double estQrSec = 0, double estSerialSec = 0)
        {
            try
            {
                string dir = GetQRCodeParaDir();
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, $"{serial}.txt");

                var sb = new StringBuilder();
                sb.AppendLine($"# MarkingMate 鋼鐵Quest3 QR 參數記錄");
                sb.AppendLine($"Serial       : {serial}");
                sb.AppendLine($"Action       : {action}");
                sb.AppendLine($"Timestamp    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Board        : {boardIndex + 1}");
                sb.AppendLine($"QR Content   : {content}");
                sb.AppendLine();
                sb.AppendLine("[QR 幾何]");
                sb.AppendLine($"QR Width     : {qrWidth} mm");
                sb.AppendLine($"QR Height    : {qrHeight} mm");
                sb.AppendLine($"Border       : {border} cells");
                sb.AppendLine($"Rect Extra   : {rectExtra} mm");
                sb.AppendLine();
                sb.AppendLine("[QR 屬性]");
                sb.AppendLine($"EC Level     : {ecLevel} (0=L 1=M 2=Q 3=H)");
                sb.AppendLine($"Mark Style   : {markStyle} (1=dot 2=fill)");
                sb.AppendLine($"Spot Size    : {spotSize} mm");
                sb.AppendLine($"QR Repeat    : {qrRepeat}");
                sb.AppendLine();
                sb.AppendLine("[白底矩形 雷射參數]");
                sb.AppendLine($"Rect Power   : {rectPower} %");
                sb.AppendLine($"Rect Speed   : {rectSpeed} mm/s");
                sb.AppendLine($"Rect Freq    : {rectFreq} kHz");
                sb.AppendLine($"Rect Repeat  : {rectRepeat}");
                sb.AppendLine();
                sb.AppendLine("[QR 雷射參數]");
                sb.AppendLine($"QR Power     : {qrPower} %");
                sb.AppendLine($"QR Speed     : {qrSpeed} mm/s");
                sb.AppendLine($"QR Freq      : {qrFreq} kHz");
                sb.AppendLine($"QR PulseWidth: {qrPulseWidth}");
                sb.AppendLine();
                sb.AppendLine("[工作區]");
                sb.AppendLine($"Workspace W  : {m_WorkspaceSize} mm");
                sb.AppendLine($"Workspace H  : {m_WorkspaceHeight} mm");
                sb.AppendLine();
                sb.AppendLine("[SDK 預估打標時間 & 路徑長度]");
                sb.AppendLine($"Total Time   : {estTotalSec:F3} sec");
                sb.AppendLine($"Total Length : {estTotalLen:F3} mm");
                sb.AppendLine($"  ├ Rect Time  : {estRectSec:F3} sec");
                sb.AppendLine($"  ├ QR Time    : {estQrSec:F3} sec");
                sb.AppendLine($"  └ Serial Time: {estSerialSec:F3} sec");

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel QR para log → {path}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WriteSteelQRParaLog failed: {ex.Message}");
            }
        }

        /// <summary>
        /// 將 QRCODE_白底 雙圖層打標的參數 + 預估時間寫入 QRCODE_白底&lt;serial&gt;.txt。
        /// </summary>
        private void WriteWhiteBgParaLog(string serial, int boardIndex,
            bool markQR, bool markRect,
            double qrWidth, double qrHeight, double quietZone, double rectExtra,
            double qrSpeed, double qrPower,
            double rectSpeed, double rectPower,
            double estTotalSec, double estTotalLen,
            double estRectSec, double estQrSec, double estSerialSec = 0)
        {
            try
            {
                string dir = GetQRCodeParaDir();
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, $"{WhiteBgFilePrefix}{serial}.txt");

                var sb = new StringBuilder();
                sb.AppendLine("# MarkingMate QRCODE_白底 參數記錄");
                sb.AppendLine($"Serial       : {serial}");
                sb.AppendLine($"Timestamp    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Board        : {boardIndex + 1}");
                sb.AppendLine($"Mark Target  : QR={markQR} Rect={markRect}");
                sb.AppendLine();
                sb.AppendLine("[QR 幾何]");
                sb.AppendLine($"QR Width     : {qrWidth} mm");
                sb.AppendLine($"QR Height    : {qrHeight} mm");
                sb.AppendLine($"Border/QuietZone : {quietZone} units");
                sb.AppendLine($"Rect Extra X : {rectExtra} mm");
                sb.AppendLine();
                sb.AppendLine("[QR 雷射參數]");
                sb.AppendLine($"QR Speed     : {qrSpeed} mm/s");
                sb.AppendLine($"QR Power     : {qrPower} %");
                sb.AppendLine();
                sb.AppendLine("[矩形雷射參數]");
                sb.AppendLine($"Rect Speed   : {rectSpeed} mm/s");
                sb.AppendLine($"Rect Power   : {rectPower} %");
                sb.AppendLine();
                sb.AppendLine("[工作區]");
                sb.AppendLine($"Workspace W  : {m_WorkspaceSize} mm");
                sb.AppendLine($"Workspace H  : {m_WorkspaceHeight} mm");
                sb.AppendLine();
                sb.AppendLine("[SDK 預估打標時間 & 路徑長度]");
                sb.AppendLine($"Total Time   : {estTotalSec:F3} sec");
                sb.AppendLine($"Total Length : {estTotalLen:F3} mm");
                sb.AppendLine($"  ├ Rect Time  : {estRectSec:F3} sec");
                sb.AppendLine($"  ├ QR Time    : {estQrSec:F3} sec");
                sb.AppendLine($"  └ Serial Time: {estSerialSec:F3} sec");

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WriteWhiteBgParaLog → {path}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WriteWhiteBgParaLog failed: {ex.Message}");
            }
        }

        /// <summary>
        /// 依 groupBoxQRSteel 內 TextBox 的參數，在指定板建立「白底矩形 + 反相 QR」兩個物件。
        /// 不呼叫 StartMarking — 交給呼叫端決定要打標（mode 4）或紅光預覽（mode 3）。
        /// 內容取自 txtQRContent，若空則用「STEEL」佔位。
        /// 白底策略：低功率高速多次疊層 → 表面退火霧化（灰白）
        /// QR 策略：高功率低速長脈衝 + dot mode + repeat → 深黑碳化點
        /// 回傳 true=兩個物件都建立成功；失敗會自行 MessageBox 提示。
        /// </summary>
        private bool BuildSteelQRLayers(int boardIndex, string action, out string serial)
        {
            serial = null;
            // 從 TextBox 讀取參數（解析失敗回落至鋼鐵建議預設）
            if (!double.TryParse(txtSteelQrWidth.Text.Trim(), out double qrWidth) || qrWidth <= 0) qrWidth = 20;
            if (!double.TryParse(txtSteelQrHeight.Text.Trim(), out double qrHeight) || qrHeight <= 0) qrHeight = 20;
            if (!int.TryParse(txtSteelBorder.Text.Trim(), out int border) || border < 0) border = 4;
            if (!double.TryParse(txtSteelRectExtra.Text.Trim(), out double rectExtra)) rectExtra = 0;
            if (!int.TryParse(txtSteelECLevel.Text.Trim(), out int ecLevel) || ecLevel < 0 || ecLevel > 3) ecLevel = 1;
            if (!int.TryParse(txtSteelMarkStyle.Text.Trim(), out int markStyle)) markStyle = 1;
            if (!double.TryParse(txtSteelSpotSize.Text.Trim(), out double spotSize) || spotSize <= 0) spotSize = 0.05;
            if (!int.TryParse(txtSteelQrRepeat.Text.Trim(), out int qrRepeat) || qrRepeat < 1) qrRepeat = 2;
            if (!double.TryParse(txtSteelRectPower.Text.Trim(), out double rectPower)) rectPower = 45;
            if (!double.TryParse(txtSteelRectSpeed.Text.Trim(), out double rectSpeed) || rectSpeed <= 0) rectSpeed = 3000;
            if (!double.TryParse(txtSteelRectFreq.Text.Trim(), out double rectFreq) || rectFreq <= 0) rectFreq = 80;
            if (!int.TryParse(txtSteelRectRepeat.Text.Trim(), out int rectRepeat) || rectRepeat < 1) rectRepeat = 2;
            if (!double.TryParse(txtSteelQrPower.Text.Trim(), out double qrPower)) qrPower = 85;
            if (!double.TryParse(txtSteelQrSpeed.Text.Trim(), out double qrSpeed) || qrSpeed <= 0) qrSpeed = 500;
            if (!double.TryParse(txtSteelQrFreq.Text.Trim(), out double qrFreq) || qrFreq <= 0) qrFreq = 25;
            if (!double.TryParse(txtSteelQrPulseWidth.Text.Trim(), out double qrPulseWidth) || qrPulseWidth <= 0) qrPulseWidth = 400;

            // QR 內容：與其他 QR 按鈕共用 txtQRContent
            string content = txtQRContent.Text?.Trim();
            if (string.IsNullOrEmpty(content)) content = "STEEL";

            // 取得本次序號（YYYYMMDD-NNN，同日累加）
            serial = NextSteelSerial();
            if (txtSteelSerial != null) txtSteelSerial.Text = serial;

            string qrName = "QRSteel_QR";
            string rectName = "QRSteel_Rect";
            string serialName = "QRSteel_Serial";

            // 0) 清板 + 重設工作區
            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents();
            Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);

            // === 為了決定 QR 實際渲染尺寸，先建立一個「暫存 QR」查詢 Version → 算完 cellSize 後刪除 → 依正確順序重建 ===

            // Step A) 建立暫存 QR，查 Version + 算 cellSize
            string tempQrName = "QRSteel_TempQR";
            long rTemp = m_MMMark[boardIndex].AddBarcode(
                BARCODE_TYPE_QRCODE, content, 0, 0, qrWidth, qrHeight, "", tempQrName);
            if (rTemp != 0)
            {
                MessageBox.Show($"建立暫存 QR 失敗！回傳碼: {rTemp}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(tempQrName, ecLevel);
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(tempQrName, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(tempQrName, 1, 1);
            ApplyQRBorder(boardIndex, tempQrName, border);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(300);

            long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(tempQrName);
            if (qrVersion < 1) qrVersion = 1;
            int modules = 17 + 4 * (int)qrVersion;
            double cellW = qrWidth / modules;
            double cellH = qrHeight / modules;

            // 讀取暫存 QR 實際渲染尺寸（含 border）→ 給矩形用
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(tempQrName, cellW, cellH);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);
            double actualW = m_MMEdit[boardIndex].GetWidth(tempQrName);
            double actualH = m_MMEdit[boardIndex].GetHeight(tempQrName);
            double rectBaseW = actualW > 0 ? actualW : qrWidth;
            double rectBaseH = actualH > 0 ? actualH : qrHeight;
            Console.Error.WriteLine(
                $"[Board {boardIndex + 1}] Steel probe QR ver={qrVersion} modules={modules} EC={ecLevel} " +
                $"Border={border} cell={cellW:F4}x{cellH:F4} rendered={actualW:F2}x{actualH:F2}");

            // 刪除暫存 QR
            try { m_MMEdit[boardIndex].DeleteObject("", tempQrName); } catch { /* 忽略 */ }
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);

            // Step B) 依「打標順序」重建 3 個物件：矩形 → QR → 文字
            //   AddXxx 順序 = SDK 打標順序，不依賴 ChangeObjectOrder。

            // 1) AddRect（Layer 1，最先打）
            double rectW = rectBaseW + rectExtra;
            double rectH = rectBaseH + rectExtra;
            double rectHalfW = rectW / 2.0, rectHalfH = rectH / 2.0;
            long rR = m_MMEdit[boardIndex].AddRect(-rectHalfW, -rectHalfH, rectHalfW, rectHalfH, 0, "", rectName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel AddRect rc={rR} size={rectW}x{rectH}");
            if (rR != 0)
            {
                MessageBox.Show($"建立矩形失敗！回傳碼: {rR}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Application.DoEvents();
            Thread.Sleep(200);

            m_MMEdit[boardIndex].SetFillStyle(rectName, 1);
            m_MMEdit[boardIndex].SetFrameLineType(rectName, 1);
            m_MMEdit[boardIndex].SetFillRoundPitch(rectName, 0.05);   // 0.05 = 白度與時間折衷
            m_MMEdit[boardIndex].SetFillPitch(rectName, 0.05);        // 同上
            m_MMEdit[boardIndex].SetFillTimes(rectName, 1);
            m_MMEdit[boardIndex].SetFillAverageDistribution(rectName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(rectName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(rectName, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(rectName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(rectName, rectSpeed);
            m_MMMark[boardIndex].SetPower(rectName, rectPower);
            m_MMMark[boardIndex].SetFrequency(rectName, rectFreq);
            m_MMMark[boardIndex].SetMarkRepeat(rectName, rectRepeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(rectName, 100);
            m_MMMark[boardIndex].SetPulseWidth(rectName, 200);

            // 2) AddBarcode QR（Layer 2，中間打）
            long rQR = m_MMMark[boardIndex].AddBarcode(
                BARCODE_TYPE_QRCODE, content, 0, 0, qrWidth, qrHeight, "", qrName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel AddBarcode rc={rQR} content=\"{content}\"");
            if (rQR != 0)
            {
                MessageBox.Show($"建立 QR Code 失敗！回傳碼: {rQR}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Application.DoEvents();
            Thread.Sleep(200);

            m_MMEdit[boardIndex].SetBarcodeInvert(qrName, 1);
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrName, ecLevel);
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(qrName, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, cellW, cellH);
            ApplyQRBorder(boardIndex, qrName, border);
            m_MMEdit[boardIndex].SetBarcodeMarkStyle(qrName, markStyle);
            m_MMEdit[boardIndex].SetBarcodeLineType(qrName, 0);
            m_MMEdit[boardIndex].SetBarcodeSpotSize(qrName, spotSize);
            m_MMEdit[boardIndex].SetBarcodeLineTimes(qrName, 1);
            m_MMEdit[boardIndex].SetFillStartAngle(qrName, 0);
            m_MMEdit[boardIndex].SetFillStepAngle(qrName, 90);
            m_MMEdit[boardIndex].SetBarcodeLineTwoway(qrName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(qrName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(qrName, qrSpeed);
            m_MMMark[boardIndex].SetPower(qrName, qrPower);
            m_MMMark[boardIndex].SetFrequency(qrName, qrFreq);
            m_MMMark[boardIndex].SetMarkRepeat(qrName, qrRepeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(qrName, 1000);
            m_MMMark[boardIndex].SetPulseWidth(qrName, qrPulseWidth);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);

            // 6) 序號文字物件：放在 QR 左上角外側上方（不會擋到資料區）
            //    字高採 QR 高的 1/8，最小 1.5 mm；y 座標在矩形上方 1 mm。
            // 序號文字位置：改用 border × cellW 顯式計算 border 白邊寬度（不依賴 GetWidth 是否含 border）
            //   border 最高 y = QR 模組頂邊 + border 白邊寬度 = qrHeight/2 + border*cellW
            //   文字最下緣 > 該 y，因此把文字中心 Y 設在 (border最高y + margin + fontSize/2)
            double qrModuleTopY = qrHeight / 2.0;
            double borderZoneWidth = border * cellW;                   // 顯式：border 白邊寬度 (mm)
            double borderTopY = qrModuleTopY + borderZoneWidth;        // border 最高 y
            double serialFontSize = Math.Max(qrHeight / 8.0, 1.5);
            double serialCx = -qrWidth / 2.0;                          // 左對齊到 QR 模組左邊
            double serialCy;
            if (border > 0 && borderZoneWidth > 0)
            {
                // 文字最下緣 = serialCy - fontSize/2 > borderTopY
                const double marginAboveBorder = 0.5;    // 0.5 mm 安全間距
                serialCy = borderTopY + serialFontSize / 2.0 + marginAboveBorder;
            }
            else
            {
                // border = 0 fallback：QR 模組頂邊 + 半字高 + 1mm 間距
                serialCy = qrModuleTopY + serialFontSize / 2.0 + 1.0;
            }
            Console.Error.WriteLine(
                $"[Board {boardIndex + 1}] Steel Text pos: border={border} cellW={cellW:F4} " +
                $"borderZone={borderZoneWidth:F3}mm borderTopY={borderTopY:F3} → serialCy={serialCy:F3}");
            long rT = m_MMMark[boardIndex].AddText(serial, serialCx, serialCy, "", serialName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel AddText serial=\"{serial}\" rc={rT} @({serialCx:F2},{serialCy:F2}) h={serialFontSize:F2}");
            if (rT == 0)
            {
                try
                {
                    m_MMEdit[boardIndex].SetFontSize(serialName, serialFontSize);
                    m_MMMark[boardIndex].SetSpeed(serialName, qrSpeed);
                    m_MMMark[boardIndex].SetPower(serialName, qrPower);
                    m_MMMark[boardIndex].SetFrequency(serialName, qrFreq);
                    m_MMMark[boardIndex].SetMarkRepeat(serialName, 2);
                    m_MMMark[boardIndex].SetPulseWidth(serialName, qrPulseWidth);
                }
                catch (Exception exSerial)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel serial text 屬性設定失敗（略過）: {exSerial.Message}");
                }
            }

            // 7) 圖層順序由 AddXxx 呼叫順序保證：矩形（先） → QR（中） → 序號文字（後）
            //    不再依賴 ChangeObjectOrder。

            // 8) 最終 Redraw（呼叫端決定要 StartMarking(4) 打標或 StartMarking(3) 紅光預覽）
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(300);

            // 8.5) 估算打標時間（SDK GetEstimatedTotalTime + 各物件 GetSKWObjTime）
            double estTotalSec = 0;
            double estRectSec = 0, estQrSec = 0, estSerialSec = 0, estTotalLen = 0;
            try
            {
                estTotalSec = m_MMMark[boardIndex].GetEstimatedTotalTime();
                estTotalLen = m_MMMark[boardIndex].GetEstimatedTotalLength();
                estRectSec = m_MMMark[boardIndex].GetSKWObjTime(rectName);
                estQrSec = m_MMMark[boardIndex].GetSKWObjTime(qrName);
                if (rT == 0)
                    estSerialSec = m_MMMark[boardIndex].GetSKWObjTime(serialName);
                Console.Error.WriteLine(
                    $"[Board {boardIndex + 1}] Steel time est: total={estTotalSec:F2}s len={estTotalLen:F2}mm | " +
                    $"rect={estRectSec:F2}s qr={estQrSec:F2}s serial={estSerialSec:F2}s");
            }
            catch (Exception exTime)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel time est 失敗: {exTime.Message}");
            }

            // 序號欄還原為只顯示序號
            if (txtSteelSerial != null)
            {
                txtSteelSerial.Text = serial;
            }

            // 預估時間 GroupBox 內的 TextBox 更新
            if (txtSteelTimeInfo != null)
            {
                var timeSb = new StringBuilder();
                timeSb.AppendLine($"Serial       : {serial}");
                timeSb.AppendLine($"Action       : {action}");
                timeSb.AppendLine($"Total Time   : {estTotalSec:F2} 秒");
                timeSb.AppendLine($"Total Length : {estTotalLen:F2} mm");
                timeSb.AppendLine($"──────────────────────────");
                timeSb.AppendLine($"矩形 (Rect)  : {estRectSec:F2} 秒");
                timeSb.AppendLine($"QR Code      : {estQrSec:F2} 秒");
                timeSb.AppendLine($"序號文字     : {estSerialSec:F2} 秒");
                txtSteelTimeInfo.Text = timeSb.ToString();
            }

            // 9) 寫參數 log（PREVIEW / MARK 兩種呼叫都會落檔，方便對照）
            WriteSteelQRParaLog(serial, action, content, boardIndex,
                qrWidth, qrHeight, border, rectExtra,
                ecLevel, markStyle, spotSize, qrRepeat,
                rectPower, rectSpeed, rectFreq, rectRepeat,
                qrPower, qrSpeed, qrFreq, qrPulseWidth,
                estTotalSec, estTotalLen, estRectSec, estQrSec, estSerialSec);

            return true;
        }

        /// <summary>
        /// 鋼鐵 + Quest 3 QR 專用打標：建立雙圖層後直接以 mode 4 打標。
        /// </summary>
        private void btnQRSteelMark_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = comboBoardQR.SelectedIndex;
            if (!m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsBoardBusy(boardIndex))
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                    "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!BuildSteelQRLayers(boardIndex, "MARK", out string serial)) return;

                m_MMMark[boardIndex].MarkStandBy();
                if (m_MMMark[boardIndex].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 啟動 Timer 監控 + 停用相關按鈕
                timerMark.Tag = boardIndex;
                timerMark.Start();

                btnQRSteelMark.Enabled = false;
                btnQRSteelPreview.Enabled = false;
                btnQRWhiteBgMark.Enabled = false;
                btnMarkQR.Enabled = false;
                btnLoadQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnClearQR.Enabled = false;
                btnStopMarkQR.Enabled = true;
                btnMarkDXF.Enabled = false;
                btnMark.Enabled = false;
                btnStop.Enabled = true;

                string ct = string.IsNullOrEmpty(txtQRContent.Text?.Trim()) ? "STEEL" : txtQRContent.Text.Trim();
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 鋼鐵Quest3 QR 打標中（{serial} / {ct}）";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"鋼鐵 QR 打標失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 鋼鐵 + Quest 3 QR 紅光預覽：與打標按鈕使用相同參數建立物件，
        /// 但改為紅光全路徑預覽（SetPreviewMode(2) + StartMarking(3)），15 秒自動停止。
        /// 讓使用者實際畫在晶片板上校準位置、確認 QR 尺寸與白底範圍。
        /// </summary>
        private void btnQRSteelPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = comboBoardQR.SelectedIndex;
            if (!m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsBoardBusy(boardIndex))
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他預覽 / 打標，請先停止再試。",
                    "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!BuildSteelQRLayers(boardIndex, "PREVIEW", out string serial)) return;

                // 紅光全路徑預覽
                m_MMMark[boardIndex].SetPreviewMode(2);
                m_MMMark[boardIndex].MarkStandBy();
                Application.DoEvents();

                if (m_MMMark[boardIndex].StartMarking(3) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 預覽啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_bPreviewing = true;
                m_iPreviewBoard = boardIndex;

                // 15 秒自動停止
                timerPreview.Stop();
                timerPreview.Start();

                // 停用相關按鈕
                btnQRSteelMark.Enabled = false;
                btnQRSteelPreview.Enabled = false;
                btnQRWhiteBgMark.Enabled = false;
                btnMarkQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnStopPreviewQR.Enabled = true;
                btnLoadQR.Enabled = false;
                btnClearQR.Enabled = false;
                btnMarkDXF.Enabled = false;
                btnPreviewDXF.Enabled = false;
                btnMark.Enabled = false;
                btnPreviewManual.Enabled = false;
                btnStop.Enabled = true;

                string ct = string.IsNullOrEmpty(txtQRContent.Text?.Trim()) ? "STEEL" : txtQRContent.Text.Trim();
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 鋼鐵Quest3 QR 紅光預覽中（{serial} / {ct}，15秒後停止）";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"鋼鐵 QR 預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // 6-1. QR Code 頁：三個獨立 GroupBox（打黑矩形 / 打白矩形 / 打 QR）
        //   每個 GroupBox 都有紅光預覽 / 取消預覽 / 雷射打標三顆按鈕。
        //   共用 comboBoardQR（跟 6. QR Code 頁的板選擇一致）。
        // ============================================================

        /// <summary>雙精度 TryParse，失敗回落預設；>0 才視為有效。</summary>
        private static double PD(string s, double def) => (double.TryParse(s?.Trim(), out double v) && v > 0) ? v : def;
        /// <summary>雙精度 TryParse（允許 0/負）。</summary>
        private static double PDx(string s, double def) => double.TryParse(s?.Trim(), out double v) ? v : def;
        /// <summary>整數 TryParse，失敗回落預設；>0 才視為有效。</summary>
        private static int PI(string s, int def) => (int.TryParse(s?.Trim(), out int v) && v > 0) ? v : def;

        private const string BR_NAME = "BR_Rect";
        private const string WR_NAME = "WR_Rect";
        private const string QO_NAME = "QO_QR";

        /// <summary>建立「打黑」矩形物件（不 StartMarking）。回傳 true=成功。</summary>
        private bool BuildBlackRect(int boardIndex)
        {
            double w = PD(txtBRWidth.Text, 50);
            double h = PD(txtBRHeight.Text, 50);
            double speed = PD(txtBRSpeed.Text, 2000);
            double power = PDx(txtBRPower.Text, 70);
            double freqKHz = PD(txtBRFreq.Text, 30);
            int repeat = PI(txtBRRepeat.Text, 1);
            double spotDelayMs = PD(txtBRSpotDelay.Text, 2);
            double pulseWidth = PD(txtBRPulseWidth.Text, 150);
            double fillPitch = PD(txtBRFillPitch.Text, 0.04);
            double fillRoundPitch = PD(txtBRFillRoundPitch.Text, 0.04);
            int fillTimes = PI(txtBRFillTimes.Text, 3);
            double stepAngle = PDx(txtBRFillStepAngle.Text, 45);

            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            double hw = w / 2.0, hh = h / 2.0;
            long rc = m_MMEdit[boardIndex].AddRect(-hw, -hh, hw, hh, 0, "", BR_NAME);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] BR AddRect rc={rc} {w}x{h}");
            if (rc != 0) { MessageBox.Show($"黑矩形建立失敗 rc={rc}", "錯誤"); return false; }

            m_MMEdit[boardIndex].SetFillStyle(BR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameLineType(BR_NAME, 1);        // 短虛線
            m_MMEdit[boardIndex].SetFillPitch(BR_NAME, fillPitch);
            m_MMEdit[boardIndex].SetFillRoundPitch(BR_NAME, fillRoundPitch);
            m_MMEdit[boardIndex].SetFillTimes(BR_NAME, fillTimes);
            m_MMEdit[boardIndex].SetFillStepAngle(BR_NAME, stepAngle);
            m_MMEdit[boardIndex].SetFillAverageDistribution(BR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(BR_NAME, 1);          // 外框打勾
            m_MMEdit[boardIndex].SetFillSwitch(BR_NAME, 1);           // 填滿打勾
            m_MMEdit[boardIndex].SetFillFirstExt(BR_NAME, 0, 1);      // 填滿優先
            m_MMMark[boardIndex].SetSpeed(BR_NAME, speed);
            m_MMMark[boardIndex].SetPower(BR_NAME, power);
            m_MMMark[boardIndex].SetFrequency(BR_NAME, freqKHz);
            m_MMMark[boardIndex].SetMarkRepeat(BR_NAME, repeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(BR_NAME, (int)(spotDelayMs * 1000));
            m_MMMark[boardIndex].SetPulseWidth(BR_NAME, pulseWidth);

            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);
            return true;
        }

        /// <summary>建立「打白」矩形物件（不 StartMarking）。</summary>
        private bool BuildWhiteRect(int boardIndex)
        {
            double w = PD(txtWRWidth.Text, 50);
            double h = PD(txtWRHeight.Text, 50);
            double speed = PD(txtWRSpeed.Text, 3000);
            double power = PDx(txtWRPower.Text, 40);
            double freqKHz = PD(txtWRFreq.Text, 500);
            int repeat = PI(txtWRRepeat.Text, 1);
            double spotDelayMs = PD(txtWRSpotDelay.Text, 2);
            double pulseWidth = PD(txtWRPulseWidth.Text, 200);
            double fillPitch = PD(txtWRFillPitch.Text, 0.04);
            double fillRoundPitch = PD(txtWRFillRoundPitch.Text, 0.04);
            int fillTimes = PI(txtWRFillTimes.Text, 2);
            double stepAngle = PDx(txtWRFillStepAngle.Text, 45);

            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            double hw = w / 2.0, hh = h / 2.0;
            long rc = m_MMEdit[boardIndex].AddRect(-hw, -hh, hw, hh, 0, "", WR_NAME);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] WR AddRect rc={rc} {w}x{h}");
            if (rc != 0) { MessageBox.Show($"白矩形建立失敗 rc={rc}", "錯誤"); return false; }

            m_MMEdit[boardIndex].SetFillStyle(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameLineType(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFillPitch(WR_NAME, fillPitch);
            m_MMEdit[boardIndex].SetFillRoundPitch(WR_NAME, fillRoundPitch);
            m_MMEdit[boardIndex].SetFillTimes(WR_NAME, fillTimes);
            m_MMEdit[boardIndex].SetFillStepAngle(WR_NAME, stepAngle);
            m_MMEdit[boardIndex].SetFillAverageDistribution(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFillSwitch(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(WR_NAME, 0, 1);
            m_MMMark[boardIndex].SetSpeed(WR_NAME, speed);
            m_MMMark[boardIndex].SetPower(WR_NAME, power);
            m_MMMark[boardIndex].SetFrequency(WR_NAME, freqKHz);
            m_MMMark[boardIndex].SetMarkRepeat(WR_NAME, repeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(WR_NAME, (int)(spotDelayMs * 1000));
            m_MMMark[boardIndex].SetPulseWidth(WR_NAME, pulseWidth);

            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);
            return true;
        }

        /// <summary>建立單一 QR Code 物件（不含白底、不含序號）。</summary>
        private bool BuildQROnly(int boardIndex)
        {
            string content = string.IsNullOrEmpty(txtQOContent.Text?.Trim()) ? "TEST" : txtQOContent.Text.Trim();
            double w = PD(txtQOWidth.Text, 30);
            double h = PD(txtQOHeight.Text, 30);
            int border = int.TryParse(txtQOBorder.Text.Trim(), out int b) && b >= 0 ? b : 2;
            bool invert = chkQOInvert.Checked;
            int markStyle = PI(txtQOMarkStyle.Text, 3);
            int repeat = PI(txtQORepeat.Text, 2);
            double stepAngle = PDx(txtQOStepAngle.Text, 90);
            double power = PDx(txtQOPower.Text, 70);
            double speed = PD(txtQOSpeed.Text, 500);
            double freqKHz = PD(txtQOFreq.Text, 25);
            double pulseWidth = PD(txtQOPulseWidth.Text, 200);

            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            long rc = m_MMMark[boardIndex].AddBarcode(BARCODE_TYPE_QRCODE, content, 0, 0, w, h, "", QO_NAME);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] QO AddBarcode rc={rc} \"{content}\"");
            if (rc != 0) { MessageBox.Show($"QR 建立失敗 rc={rc}", "錯誤"); return false; }
            Application.DoEvents(); Thread.Sleep(200);

            m_MMEdit[boardIndex].SetBarcodeInvert(QO_NAME, invert ? 1 : 0);
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(QO_NAME, QR_EC_LEVEL_LOW);
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(QO_NAME, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(QO_NAME, 1, 1);   // 使用者指定「1單元為1*1」佔位，稍後反推
            ApplyQRBorder(boardIndex, QO_NAME, border);
            m_MMEdit[boardIndex].SetBarcodeMarkStyle(QO_NAME, markStyle);    // 3 = 連續線段
            m_MMEdit[boardIndex].SetBarcodeLineType(QO_NAME, 0);
            m_MMEdit[boardIndex].SetFillStartAngle(QO_NAME, 0);
            m_MMEdit[boardIndex].SetFillStepAngle(QO_NAME, stepAngle);
            m_MMEdit[boardIndex].SetBarcodeLineTwoway(QO_NAME, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(QO_NAME, 1);
            m_MMEdit[boardIndex].SetFillSwitch(QO_NAME, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(QO_NAME, 0, 1);
            m_MMMark[boardIndex].SetSpeed(QO_NAME, speed);
            m_MMMark[boardIndex].SetPower(QO_NAME, power);
            m_MMMark[boardIndex].SetFrequency(QO_NAME, freqKHz);
            m_MMMark[boardIndex].SetMarkRepeat(QO_NAME, repeat);
            m_MMMark[boardIndex].SetPulseWidth(QO_NAME, pulseWidth);

            // Redraw 後反推 cellSize，讓 QR 資料區渲染 = UI 長寬
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(300);
            long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(QO_NAME);
            if (qrVersion < 1) qrVersion = 1;
            int modules = 17 + 4 * (int)qrVersion;
            double cellW = w / modules;
            double cellH = h / modules;
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(QO_NAME, cellW, cellH);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] QO ver={qrVersion} modules={modules} cell={cellW:F4}");
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(300);
            return true;
        }

        /// <summary>共用啟動打標 / 預覽的收尾邏輯。startMode: 3=紅光預覽,4=實際打標。</summary>
        private void StartTabQR2(int boardIndex, int startMode, string desc)
        {
            m_MMMark[boardIndex].MarkStandBy();
            if (startMode == 3)
            {
                m_MMMark[boardIndex].SetPreviewMode(2);
                Application.DoEvents();
                if (m_MMMark[boardIndex].StartMarking(3) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 預覽啟動失敗！", "錯誤"); return;
                }
                m_bPreviewing = true;
                m_iPreviewBoard = boardIndex;
                timerPreview.Stop(); timerPreview.Start();
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 6-1 {desc} 紅光預覽中（15秒後停止）";
            }
            else
            {
                if (m_MMMark[boardIndex].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 打標啟動失敗！", "錯誤"); return;
                }
                timerMark.Tag = boardIndex;
                timerMark.Start();
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 6-1 {desc} 打標中";
            }
        }

        /// <summary>檢查 6-1 頁選擇的板可用性。回傳 -1 = 無效。</summary>
        private int GetTabQR2Board()
        {
            if (!m_bInit) { MessageBox.Show("請先初始化！", "錯誤"); return -1; }
            int b = comboBoardQR2.SelectedIndex;
            if (b < 0 || b >= m_bBoardInit.Length || !m_bBoardInit[b])
            {
                MessageBox.Show($"晶片板 {b + 1} 未成功初始化", "錯誤"); return -1;
            }
            if (IsBoardBusy(b))
            {
                MessageBox.Show($"晶片板 {b + 1} 忙碌中", "板忙碌中"); return -1;
            }
            return b;
        }

        // ---- 打黑矩形 ----
        private void btnBRPreview_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            try { if (BuildBlackRect(b)) StartTabQR2(b, 3, "黑矩形"); }
            catch (Exception ex) { MessageBox.Show($"黑矩形預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnBRStopPreview_Click(object sender, EventArgs e)
        {
            try { if (m_bPreviewing) { m_MMMark[m_iPreviewBoard].StopMarking(); timerPreview.Stop(); m_bPreviewing = false; txtQRStatus.Text = "已停止預覽"; } }
            catch (Exception ex) { MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnBRMark_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            try { if (BuildBlackRect(b)) StartTabQR2(b, 4, "黑矩形"); }
            catch (Exception ex) { MessageBox.Show($"黑矩形打標失敗：{ex.Message}", "錯誤"); }
        }

        // ---- 打白矩形 ----
        private void btnWRPreview_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            try { if (BuildWhiteRect(b)) StartTabQR2(b, 3, "白矩形"); }
            catch (Exception ex) { MessageBox.Show($"白矩形預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnWRStopPreview_Click(object sender, EventArgs e)
        {
            try { if (m_bPreviewing) { m_MMMark[m_iPreviewBoard].StopMarking(); timerPreview.Stop(); m_bPreviewing = false; txtQRStatus.Text = "已停止預覽"; } }
            catch (Exception ex) { MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnWRMark_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            try { if (BuildWhiteRect(b)) StartTabQR2(b, 4, "白矩形"); }
            catch (Exception ex) { MessageBox.Show($"白矩形打標失敗：{ex.Message}", "錯誤"); }
        }

        // ---- 單打 QR ----
        private void btnQOPreview_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            try { if (BuildQROnly(b)) StartTabQR2(b, 3, "QR"); }
            catch (Exception ex) { MessageBox.Show($"QR 預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnQOStopPreview_Click(object sender, EventArgs e)
        {
            try { if (m_bPreviewing) { m_MMMark[m_iPreviewBoard].StopMarking(); timerPreview.Stop(); m_bPreviewing = false; txtQRStatus.Text = "已停止預覽"; } }
            catch (Exception ex) { MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnQOMark_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            try { if (BuildQROnly(b)) StartTabQR2(b, 4, "QR"); }
            catch (Exception ex) { MessageBox.Show($"QR 打標失敗：{ex.Message}", "錯誤"); }
        }

        /// <summary>
        /// 6-1 三合一：在同一板上依序建立黑矩形、白矩形、QR 三個物件，
        /// 用 ChangeObjectOrder 排成 打標順序 1(黑) → 2(白) → 3(QR)。
        /// 不呼叫 StartMarking — 交呼叫端決定 mode 3 (紅光) 或 mode 4 (打標)。
        /// </summary>
        private bool BuildAllThree(int boardIndex)
        {
            // ---- 讀所有參數 ----
            double bW = PD(txtBRWidth.Text, 50), bH = PD(txtBRHeight.Text, 50);
            double bSpeed = PD(txtBRSpeed.Text, 2000);
            double bPower = PDx(txtBRPower.Text, 70);
            double bFreq = PD(txtBRFreq.Text, 30);
            int bRepeat = PI(txtBRRepeat.Text, 1);
            double bSpotMs = PD(txtBRSpotDelay.Text, 2);
            double bPW = PD(txtBRPulseWidth.Text, 150);
            double bPitch = PD(txtBRFillPitch.Text, 0.04);
            double bRoundPitch = PD(txtBRFillRoundPitch.Text, 0.04);
            int bFillTimes = PI(txtBRFillTimes.Text, 3);
            double bStepAng = PDx(txtBRFillStepAngle.Text, 45);

            double wW = PD(txtWRWidth.Text, 50), wH = PD(txtWRHeight.Text, 50);
            double wSpeed = PD(txtWRSpeed.Text, 3000);
            double wPower = PDx(txtWRPower.Text, 40);
            double wFreq = PD(txtWRFreq.Text, 500);
            int wRepeat = PI(txtWRRepeat.Text, 1);
            double wSpotMs = PD(txtWRSpotDelay.Text, 2);
            double wPW = PD(txtWRPulseWidth.Text, 200);
            double wPitch = PD(txtWRFillPitch.Text, 0.04);
            double wRoundPitch = PD(txtWRFillRoundPitch.Text, 0.04);
            int wFillTimes = PI(txtWRFillTimes.Text, 2);
            double wStepAng = PDx(txtWRFillStepAngle.Text, 45);

            string qContent = string.IsNullOrEmpty(txtQOContent.Text?.Trim()) ? "TEST" : txtQOContent.Text.Trim();
            double qW = PD(txtQOWidth.Text, 30), qH = PD(txtQOHeight.Text, 30);
            int qBorder = int.TryParse(txtQOBorder.Text.Trim(), out int qb) && qb >= 0 ? qb : 2;
            bool qInvert = chkQOInvert.Checked;
            int qMarkStyle = PI(txtQOMarkStyle.Text, 3);
            int qRepeat = PI(txtQORepeat.Text, 2);
            double qStepAng = PDx(txtQOStepAngle.Text, 90);
            double qPower = PDx(txtQOPower.Text, 70);
            double qSpeed = PD(txtQOSpeed.Text, 500);
            double qFreq = PD(txtQOFreq.Text, 25);
            double qPW = PD(txtQOPulseWidth.Text, 200);

            // ---- 清板 + 重設工作區 ----
            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            // ---- ① 黑矩形 ----
            double bHw = bW / 2.0, bHh = bH / 2.0;
            long r1 = m_MMEdit[boardIndex].AddRect(-bHw, -bHh, bHw, bHh, 0, "", BR_NAME);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] AllThree AddBlackRect rc={r1}");
            if (r1 != 0) { MessageBox.Show($"黑矩形建立失敗 rc={r1}", "錯誤"); return false; }
            m_MMEdit[boardIndex].SetFillStyle(BR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameLineType(BR_NAME, 1);
            m_MMEdit[boardIndex].SetFillPitch(BR_NAME, bPitch);
            m_MMEdit[boardIndex].SetFillRoundPitch(BR_NAME, bRoundPitch);
            m_MMEdit[boardIndex].SetFillTimes(BR_NAME, bFillTimes);
            m_MMEdit[boardIndex].SetFillStepAngle(BR_NAME, bStepAng);
            m_MMEdit[boardIndex].SetFillAverageDistribution(BR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(BR_NAME, 1);
            m_MMEdit[boardIndex].SetFillSwitch(BR_NAME, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(BR_NAME, 0, 1);
            m_MMMark[boardIndex].SetSpeed(BR_NAME, bSpeed);
            m_MMMark[boardIndex].SetPower(BR_NAME, bPower);
            m_MMMark[boardIndex].SetFrequency(BR_NAME, bFreq);
            m_MMMark[boardIndex].SetMarkRepeat(BR_NAME, bRepeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(BR_NAME, (int)(bSpotMs * 1000));
            m_MMMark[boardIndex].SetPulseWidth(BR_NAME, bPW);
            Application.DoEvents(); Thread.Sleep(150);

            // ---- ② 白矩形 ----
            double wHw = wW / 2.0, wHh = wH / 2.0;
            long r2 = m_MMEdit[boardIndex].AddRect(-wHw, -wHh, wHw, wHh, 0, "", WR_NAME);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] AllThree AddWhiteRect rc={r2}");
            if (r2 != 0) { MessageBox.Show($"白矩形建立失敗 rc={r2}", "錯誤"); return false; }
            m_MMEdit[boardIndex].SetFillStyle(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameLineType(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFillPitch(WR_NAME, wPitch);
            m_MMEdit[boardIndex].SetFillRoundPitch(WR_NAME, wRoundPitch);
            m_MMEdit[boardIndex].SetFillTimes(WR_NAME, wFillTimes);
            m_MMEdit[boardIndex].SetFillStepAngle(WR_NAME, wStepAng);
            m_MMEdit[boardIndex].SetFillAverageDistribution(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFillSwitch(WR_NAME, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(WR_NAME, 0, 1);
            m_MMMark[boardIndex].SetSpeed(WR_NAME, wSpeed);
            m_MMMark[boardIndex].SetPower(WR_NAME, wPower);
            m_MMMark[boardIndex].SetFrequency(WR_NAME, wFreq);
            m_MMMark[boardIndex].SetMarkRepeat(WR_NAME, wRepeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(WR_NAME, (int)(wSpotMs * 1000));
            m_MMMark[boardIndex].SetPulseWidth(WR_NAME, wPW);
            Application.DoEvents(); Thread.Sleep(150);

            // ---- ③ QR ----
            long r3 = m_MMMark[boardIndex].AddBarcode(BARCODE_TYPE_QRCODE, qContent, 0, 0, qW, qH, "", QO_NAME);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] AllThree AddQR rc={r3} \"{qContent}\"");
            if (r3 != 0) { MessageBox.Show($"QR 建立失敗 rc={r3}", "錯誤"); return false; }
            Application.DoEvents(); Thread.Sleep(200);

            m_MMEdit[boardIndex].SetBarcodeInvert(QO_NAME, qInvert ? 1 : 0);
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(QO_NAME, QR_EC_LEVEL_LOW);
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(QO_NAME, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(QO_NAME, 1, 1);
            ApplyQRBorder(boardIndex, QO_NAME, qBorder);
            m_MMEdit[boardIndex].SetBarcodeMarkStyle(QO_NAME, qMarkStyle);
            m_MMEdit[boardIndex].SetBarcodeLineType(QO_NAME, 0);
            m_MMEdit[boardIndex].SetFillStartAngle(QO_NAME, 0);
            m_MMEdit[boardIndex].SetFillStepAngle(QO_NAME, qStepAng);
            m_MMEdit[boardIndex].SetBarcodeLineTwoway(QO_NAME, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(QO_NAME, 1);
            m_MMEdit[boardIndex].SetFillSwitch(QO_NAME, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(QO_NAME, 0, 1);
            m_MMMark[boardIndex].SetSpeed(QO_NAME, qSpeed);
            m_MMMark[boardIndex].SetPower(QO_NAME, qPower);
            m_MMMark[boardIndex].SetFrequency(QO_NAME, qFreq);
            m_MMMark[boardIndex].SetMarkRepeat(QO_NAME, qRepeat);
            m_MMMark[boardIndex].SetPulseWidth(QO_NAME, qPW);

            // 反推 QR cellSize
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(300);
            long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(QO_NAME);
            if (qrVersion < 1) qrVersion = 1;
            int modules = 17 + 4 * (int)qrVersion;
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(QO_NAME, qW / modules, qH / modules);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(300);

            // ---- 排序：黑(BR) 最先 → 白(WR) → QR(QO) 最後 ----
            // ChangeObjectOrder(A, B, 0) = 把 A 移到 B 之前
            // 目標順序：BR, WR, QO
            long rOrd1 = m_MMEdit[boardIndex].ChangeObjectOrder(BR_NAME, WR_NAME, 0);   // 黑 → 白之前
            long rOrd2 = m_MMEdit[boardIndex].ChangeObjectOrder(WR_NAME, QO_NAME, 0);   // 白 → QR 之前
            Console.Error.WriteLine($"[Board {boardIndex + 1}] AllThree order rc=({rOrd1},{rOrd2})");

            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(300);
            return true;
        }

        private void btnAllPreview_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            try { if (BuildAllThree(b)) StartTabQR2(b, 3, "三合一(黑→白→QR)"); }
            catch (Exception ex) { MessageBox.Show($"三合一預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnAllStopPreview_Click(object sender, EventArgs e)
        {
            try { if (m_bPreviewing) { m_MMMark[m_iPreviewBoard].StopMarking(); timerPreview.Stop(); m_bPreviewing = false; txtQRStatus.Text = "已停止預覽"; } }
            catch (Exception ex) { MessageBox.Show($"停止預覽失敗：{ex.Message}", "錯誤"); }
        }
        private void btnAllMark_Click(object sender, EventArgs e)
        {
            int b = GetTabQR2Board(); if (b < 0) return;
            var confirm = MessageBox.Show(
                "將依序執行：\n① 黑矩形（凹刻）\n② 白矩形（霧化）\n③ QR Code\n\n確認開始打標？",
                "三合一打標確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (confirm != DialogResult.OK) return;
            try { if (BuildAllThree(b)) StartTabQR2(b, 4, "三合一(黑→白→QR)"); }
            catch (Exception ex) { MessageBox.Show($"三合一打標失敗：{ex.Message}", "錯誤"); }
        }

        /// <summary>鋼鐵Quest3 QR：停止紅光預覽 / 打標。</summary>
        private void btnQRSteelStopPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            try
            {
                timerMark.Stop();
                timerPreview.Stop();

                // 對所有已初始化的板呼叫 StopMarking，覆蓋所有可能的板號
                for (int i = 0; i < m_bBoardInit.Length; i++)
                {
                    if (m_bBoardInit[i])
                    {
                        try { m_MMMark[i].StopMarking(); } catch { /* 忽略單板停止失敗 */ }
                    }
                }

                m_bPreviewing = false;
                m_iPreviewBoard = -1;

                ResetPreviewButtonsAfterStop();

                // 恢復鋼鐵Quest3 兩顆執行按鈕
                btnQRSteelPreview.Enabled = true;
                btnQRSteelMark.Enabled = true;

                txtQRStatus.Text = "鋼鐵Quest3 已停止。";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"鋼鐵 QR 停止失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================================================================
        // === 矩形獨立 / QR 獨立 GroupBox handler ===========================
        // ===================================================================

        /// <summary>依 groupBoxRectAlone TextBox 建立單一矩形物件（不呼叫 StartMarking）。</summary>
        private bool BuildRectAloneObject(int boardIndex, out string rectName)
        {
            rectName = "RectAlone_Rect";
            if (!double.TryParse(txtRAWidth.Text.Trim(), out double w) || w <= 0) w = 40;
            if (!double.TryParse(txtRAHeight.Text.Trim(), out double h) || h <= 0) h = 40;
            if (!double.TryParse(txtRAX.Text.Trim(), out double cx)) cx = 0;
            if (!double.TryParse(txtRAY.Text.Trim(), out double cy)) cy = 0;
            if (!double.TryParse(txtRASpeed.Text.Trim(), out double speed) || speed <= 0) speed = 1000;
            if (!double.TryParse(txtRAPower.Text.Trim(), out double power)) power = 50;
            if (!double.TryParse(txtRAFreq.Text.Trim(), out double freq) || freq <= 0) freq = 20;
            if (!int.TryParse(txtRARepeat.Text.Trim(), out int repeat) || repeat < 1) repeat = 1;
            if (!double.TryParse(txtRAPulseWidth.Text.Trim(), out double pw) || pw <= 0) pw = 100;
            if (!int.TryParse(txtRAFillStyle.Text.Trim(), out int fillStyle)) fillStyle = 0;
            if (!int.TryParse(txtRAFrameLineType.Text.Trim(), out int frameLineType)) frameLineType = 0;

            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            double hw = w / 2.0, hh = h / 2.0;
            long rc = m_MMEdit[boardIndex].AddRect(cx - hw, cy - hh, cx + hw, cy + hh, 0, "", rectName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] RectAlone AddRect rc={rc} size={w}x{h} @({cx},{cy})");
            if (rc != 0)
            {
                MessageBox.Show($"建立矩形失敗！回傳碼: {rc}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Application.DoEvents(); Thread.Sleep(200);

            m_MMEdit[boardIndex].SetFillStyle(rectName, fillStyle);
            m_MMEdit[boardIndex].SetFrameLineType(rectName, frameLineType);
            m_MMEdit[boardIndex].SetFillRoundPitch(rectName, 0.05);
            m_MMEdit[boardIndex].SetFillPitch(rectName, 0.05);
            m_MMEdit[boardIndex].SetFillTimes(rectName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(rectName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(rectName, fillStyle > 0 ? 1 : 0);   // FillStyle=0 → 只描外框
            m_MMEdit[boardIndex].SetFillFirstExt(rectName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(rectName, speed);
            m_MMMark[boardIndex].SetPower(rectName, power);
            m_MMMark[boardIndex].SetFrequency(rectName, freq);
            m_MMMark[boardIndex].SetMarkRepeat(rectName, repeat);
            m_MMMark[boardIndex].SetPulseWidth(rectName, pw);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);
            return true;
        }

        /// <summary>依 groupBoxQRAlone TextBox 建立單一 QR 物件（不呼叫 StartMarking）。</summary>
        private bool BuildQRAloneObject(int boardIndex, out string qrName)
        {
            qrName = "QRAlone_QR";
            string content = string.IsNullOrEmpty(txtQAContent.Text?.Trim()) ? "AAA" : txtQAContent.Text.Trim();
            if (!double.TryParse(txtQAWidth.Text.Trim(), out double w) || w <= 0) w = 20;
            if (!double.TryParse(txtQAHeight.Text.Trim(), out double h) || h <= 0) h = 20;
            if (!double.TryParse(txtQAX.Text.Trim(), out double cx)) cx = 0;
            if (!double.TryParse(txtQAY.Text.Trim(), out double cy)) cy = 0;
            if (!int.TryParse(txtQABorder.Text.Trim(), out int border) || border < 0) border = 4;
            if (!int.TryParse(txtQAECLevel.Text.Trim(), out int ecLevel) || ecLevel < 0 || ecLevel > 3) ecLevel = 1;
            if (!int.TryParse(txtQAMarkStyle.Text.Trim(), out int markStyle)) markStyle = 1;
            bool invert = chkQAInvert.Checked;
            if (!double.TryParse(txtQASpeed.Text.Trim(), out double speed) || speed <= 0) speed = 500;
            if (!double.TryParse(txtQAPower.Text.Trim(), out double power)) power = 80;
            if (!double.TryParse(txtQAFreq.Text.Trim(), out double freq) || freq <= 0) freq = 25;
            if (!int.TryParse(txtQARepeat.Text.Trim(), out int repeat) || repeat < 1) repeat = 2;
            if (!double.TryParse(txtQAPulseWidth.Text.Trim(), out double pw) || pw <= 0) pw = 300;

            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            long rc = m_MMMark[boardIndex].AddBarcode(BARCODE_TYPE_QRCODE, content, cx, cy, w, h, "", qrName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] QRAlone AddBarcode rc={rc} content=\"{content}\" @({cx},{cy}) size={w}x{h}");
            if (rc != 0)
            {
                MessageBox.Show($"建立 QR 失敗！回傳碼: {rc}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Application.DoEvents(); Thread.Sleep(200);

            m_MMEdit[boardIndex].SetBarcodeInvert(qrName, invert ? 1 : 0);
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrName, ecLevel);
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(qrName, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, 1, 1);
            ApplyQRBorder(boardIndex, qrName, border);
            m_MMEdit[boardIndex].SetBarcodeMarkStyle(qrName, markStyle);
            m_MMEdit[boardIndex].SetBarcodeLineType(qrName, 0);
            m_MMEdit[boardIndex].SetBarcodeSpotSize(qrName, 0.05);
            m_MMEdit[boardIndex].SetBarcodeLineTimes(qrName, 1);
            m_MMEdit[boardIndex].SetFillStartAngle(qrName, 0);
            m_MMEdit[boardIndex].SetFillStepAngle(qrName, 90);
            m_MMEdit[boardIndex].SetBarcodeLineTwoway(qrName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(qrName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(qrName, speed);
            m_MMMark[boardIndex].SetPower(qrName, power);
            m_MMMark[boardIndex].SetFrequency(qrName, freq);
            m_MMMark[boardIndex].SetMarkRepeat(qrName, repeat);
            m_MMMark[boardIndex].SetPulseWidth(qrName, pw);

            // Redraw → 反推 cellSize，使渲染大小等於 UI 設定
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(300);
            long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(qrName);
            if (qrVersion < 1) qrVersion = 1;
            int modules = 17 + 4 * (int)qrVersion;
            double cellW = w / modules;
            double cellH = h / modules;
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, cellW, cellH);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);
            return true;
        }

        private int GetQRAloneBoardIndex() { return comboBoardQR.SelectedIndex; }

        private void btnRAPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) { MessageBox.Show("請先初始化！", "錯誤"); return; }
            int b = GetQRAloneBoardIndex();
            if (b < 0 || !m_bBoardInit[b]) { MessageBox.Show("板未初始化！", "錯誤"); return; }
            if (IsBoardBusy(b)) { MessageBox.Show("板忙碌中！", "錯誤"); return; }
            try
            {
                if (!BuildRectAloneObject(b, out _)) return;
                m_MMMark[b].SetPreviewMode(2);
                m_MMMark[b].MarkStandBy();
                if (m_MMMark[b].StartMarking(3) != 0) { MessageBox.Show("預覽啟動失敗！", "錯誤"); return; }
                m_bPreviewing = true; m_iPreviewBoard = b;
                timerPreview.Tag = new object[] { b, 15 }; timerPreview.Start();
                btnRAPreview.Enabled = false; btnRAMark.Enabled = false;
                txtQRStatus.Text = $"矩形（獨立）紅光預覽中（15 秒）";
            }
            catch (Exception ex) { MessageBox.Show($"預覽失敗：{ex.Message}", "錯誤"); }
        }

        private void btnRAMark_Click(object sender, EventArgs e)
        {
            if (!m_bInit) { MessageBox.Show("請先初始化！", "錯誤"); return; }
            int b = GetQRAloneBoardIndex();
            if (b < 0 || !m_bBoardInit[b]) { MessageBox.Show("板未初始化！", "錯誤"); return; }
            if (IsBoardBusy(b)) { MessageBox.Show("板忙碌中！", "錯誤"); return; }
            try
            {
                if (!BuildRectAloneObject(b, out _)) return;
                m_MMMark[b].MarkStandBy();
                if (m_MMMark[b].StartMarking(4) != 0) { MessageBox.Show("打標啟動失敗！", "錯誤"); return; }
                timerMark.Tag = b; timerMark.Start();
                btnRAPreview.Enabled = false; btnRAMark.Enabled = false;
                txtQRStatus.Text = "矩形（獨立）打標中";
            }
            catch (Exception ex) { MessageBox.Show($"打標失敗：{ex.Message}", "錯誤"); }
        }

        private void btnRAStopPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;
            try
            {
                timerMark.Stop(); timerPreview.Stop();
                for (int i = 0; i < m_bBoardInit.Length; i++)
                    if (m_bBoardInit[i]) try { m_MMMark[i].StopMarking(); } catch { }
                m_bPreviewing = false; m_iPreviewBoard = -1;
                ResetPreviewButtonsAfterStop();
                btnRAPreview.Enabled = true; btnRAMark.Enabled = true;
                txtQRStatus.Text = "矩形（獨立）已停止";
            }
            catch (Exception ex) { MessageBox.Show($"停止失敗：{ex.Message}", "錯誤"); }
        }

        private void btnQAPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) { MessageBox.Show("請先初始化！", "錯誤"); return; }
            int b = GetQRAloneBoardIndex();
            if (b < 0 || !m_bBoardInit[b]) { MessageBox.Show("板未初始化！", "錯誤"); return; }
            if (IsBoardBusy(b)) { MessageBox.Show("板忙碌中！", "錯誤"); return; }
            try
            {
                if (!BuildQRAloneObject(b, out _)) return;
                m_MMMark[b].SetPreviewMode(2);
                m_MMMark[b].MarkStandBy();
                if (m_MMMark[b].StartMarking(3) != 0) { MessageBox.Show("預覽啟動失敗！", "錯誤"); return; }
                m_bPreviewing = true; m_iPreviewBoard = b;
                timerPreview.Tag = new object[] { b, 15 }; timerPreview.Start();
                btnQAPreview.Enabled = false; btnQAMark.Enabled = false;
                txtQRStatus.Text = $"QR（獨立）紅光預覽中（15 秒）";
            }
            catch (Exception ex) { MessageBox.Show($"預覽失敗：{ex.Message}", "錯誤"); }
        }

        private void btnQAMark_Click(object sender, EventArgs e)
        {
            if (!m_bInit) { MessageBox.Show("請先初始化！", "錯誤"); return; }
            int b = GetQRAloneBoardIndex();
            if (b < 0 || !m_bBoardInit[b]) { MessageBox.Show("板未初始化！", "錯誤"); return; }
            if (IsBoardBusy(b)) { MessageBox.Show("板忙碌中！", "錯誤"); return; }
            try
            {
                if (!BuildQRAloneObject(b, out _)) return;
                m_MMMark[b].MarkStandBy();
                if (m_MMMark[b].StartMarking(4) != 0) { MessageBox.Show("打標啟動失敗！", "錯誤"); return; }
                timerMark.Tag = b; timerMark.Start();
                btnQAPreview.Enabled = false; btnQAMark.Enabled = false;
                txtQRStatus.Text = "QR（獨立）打標中";
            }
            catch (Exception ex) { MessageBox.Show($"打標失敗：{ex.Message}", "錯誤"); }
        }

        private void btnQAStopPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;
            try
            {
                timerMark.Stop(); timerPreview.Stop();
                for (int i = 0; i < m_bBoardInit.Length; i++)
                    if (m_bBoardInit[i]) try { m_MMMark[i].StopMarking(); } catch { }
                m_bPreviewing = false; m_iPreviewBoard = -1;
                ResetPreviewButtonsAfterStop();
                btnQAPreview.Enabled = true; btnQAMark.Enabled = true;
                txtQRStatus.Text = "QR（獨立）已停止";
            }
            catch (Exception ex) { MessageBox.Show($"停止失敗：{ex.Message}", "錯誤"); }
        }

        // === CLI Builder 頁籤 ===

        /// <summary>從 CLI Builder TextBox 讀值組合成命令列參數陣列。</summary>
        /// <summary>組左側「命令參數編輯」區塊的命令字串（DXF / lines / laser / wobble / preview）。
        /// 不含 QR 相關參數 — QR 命令由 BuildCLIQRArgsList 獨立負責。</summary>
        private List<string> BuildCLIBuilderArgsList()
        {
            var args = new List<string>();
            if (!string.IsNullOrWhiteSpace(txtCLIBoard.Text)) args.AddRange(new[] { "--board", txtCLIBoard.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIConfig.Text)) args.AddRange(new[] { "--config", txtCLIConfig.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIWsW.Text)) args.AddRange(new[] { "--workspace-w", txtCLIWsW.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIWsH.Text)) args.AddRange(new[] { "--workspace-h", txtCLIWsH.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIDxf.Text)) args.AddRange(new[] { "--dxf", txtCLIDxf.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLILines.Text))
            {
                string linesStr = txtCLILines.Text.Trim().Replace("\r\n", "").Replace("\n", "").Replace(" ", "");
                args.AddRange(new[] { "--lines", linesStr });
            }
            if (!string.IsNullOrWhiteSpace(txtCLIPower.Text)) args.AddRange(new[] { "--power", txtCLIPower.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLISpeed.Text)) args.AddRange(new[] { "--speed", txtCLISpeed.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIFreq.Text)) args.AddRange(new[] { "--freq", txtCLIFreq.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIPulseWidth.Text)) args.AddRange(new[] { "--pulse-width", txtCLIPulseWidth.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIRepeat.Text)) args.AddRange(new[] { "--repeat", txtCLIRepeat.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIWobbleWidth.Text)) args.AddRange(new[] { "--wobble-width", txtCLIWobbleWidth.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIWobbleOverlap.Text)) args.AddRange(new[] { "--wobble-overlap", txtCLIWobbleOverlap.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIWobbleSpeed.Text)) args.AddRange(new[] { "--wobble-speed", txtCLIWobbleSpeed.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIPreview.Text)) args.AddRange(new[] { "--preview", txtCLIPreview.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIPreviewSpeed.Text)) args.AddRange(new[] { "--preview-speed", txtCLIPreviewSpeed.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIPreviewTime.Text)) args.AddRange(new[] { "--preview-time", txtCLIPreviewTime.Text.Trim() });
            if (chkCLIMark.Checked) args.Add("--mark");
            return args;
        }

        /// <summary>組 QRCODE 區塊專用命令：僅含 board/config/workspace 環境 + QR 白底所有固定參數 + Content + mark。
        /// 不含左側線段/DXF/laser/wobble/preview 任何欄位。</summary>
        private List<string> BuildCLIQRArgsList()
        {
            var args = new List<string>();
            if (!string.IsNullOrWhiteSpace(txtCLIBoard.Text)) args.AddRange(new[] { "--board", txtCLIBoard.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIConfig.Text)) args.AddRange(new[] { "--config", txtCLIConfig.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIWsW.Text)) args.AddRange(new[] { "--workspace-w", txtCLIWsW.Text.Trim() });
            if (!string.IsNullOrWhiteSpace(txtCLIWsH.Text)) args.AddRange(new[] { "--workspace-h", txtCLIWsH.Text.Trim() });
            string content = txtCLIQRContent != null ? txtCLIQRContent.Text.Trim() : "";
            if (string.IsNullOrEmpty(content)) return args;   // 沒 content 就不發 QR 命令
            args.AddRange(new[] { "--qrcode", content });
            args.Add("--qr-whitebg");
            args.AddRange(new[] { "--qr-width", "15" });
            args.AddRange(new[] { "--qr-height", "15" });
            args.AddRange(new[] { "--qr-border", "2" });
            args.AddRange(new[] { "--power", "80" });
            args.AddRange(new[] { "--speed", "1000" });
            if (chkCLIMark.Checked) args.Add("--mark");
            return args;
        }

        /// <summary>把 args list 組成 "MarkingMate.exe --client ..." 字串（含引號跳脫）。</summary>
        private string FormatCLICommand(List<string> args)
        {
            // 固定輸出 --client：範例字串複製到終端機時必須走 daemon client 派工，
            // 避免變成獨立模式 A process 撞 SDK OCX 鎖（需重開機）。
            var sb = new StringBuilder("MarkingMate.exe --client");
            foreach (var arg in args)
            {
                bool needQuote = arg.Contains(" ") || arg.Contains(";");
                if (needQuote)
                    sb.Append(" \"").Append(arg).Append("\"");
                else
                    sb.Append(" ").Append(arg);
            }
            return sb.ToString();
        }

        /// <summary>更新左側「命令參數編輯」的組合後命令輸出。</summary>
        private void RefreshCLIBuilderCommand()
        {
            txtCLIOutput.Text = FormatCLICommand(BuildCLIBuilderArgsList());
        }

        /// <summary>更新 QRCODE 區塊下方的組合後命令輸出（QR 專用，不含左側線段參數）。</summary>
        private void RefreshCLIQRCommand()
        {
            if (txtCLIQROutput == null) return;
            var qrArgs = BuildCLIQRArgsList();
            txtCLIQROutput.Text = qrArgs.Count > 0 ? FormatCLICommand(qrArgs) : "";
        }

        private void btnCLIRefresh_Click(object sender, EventArgs e)
        {
            RefreshCLIBuilderCommand();
        }

        /// <summary>QRCODE 區塊「重新組合命令」— 僅重組 QR 專用命令。</summary>
        private void btnCLIQRRefresh_Click(object sender, EventArgs e)
        {
            RefreshCLIQRCommand();
        }

        /// <summary>所有 CLI TextBox / CheckBox 共用的即時更新 handler。
        /// 同時刷新左側線段命令 + QR 命令兩個輸出框，各自獨立來源。</summary>
        private void OnCLIBuilderInputChanged(object sender, EventArgs e)
        {
            RefreshCLIBuilderCommand();
            RefreshCLIQRCommand();
        }

        /// <summary>CLI 編輯器：停止預覽 / 打標。同時處理預覽與正常打標兩種狀態。</summary>
        private void btnCLIStopPreview_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;

            try
            {
                // 停止兩個 timer（正常打標用 timerMark、預覽用 timerPreview）
                timerMark.Stop();
                timerPreview.Stop();

                // 對所有已初始化的板呼叫 StopMarking，確保任何進行中的打標/預覽都停下
                for (int i = 0; i < m_bBoardInit.Length; i++)
                {
                    if (m_bBoardInit[i])
                    {
                        try { m_MMMark[i].StopMarking(); } catch { /* 忽略單板停止失敗 */ }
                    }
                }

                m_bPreviewing = false;
                m_iPreviewBoard = -1;

                // 讓其他頁籤的預覽按鈕狀態也一併復原
                ResetPreviewButtonsAfterStop();

                btnCLIExecuteMark.Enabled = true;
                if (btnCLIQRExecuteMark != null) btnCLIQRExecuteMark.Enabled = true;
                txtCLIOutput.Text += "\r\n[已停止]";
                if (txtCLIQROutput != null) txtCLIQROutput.Text += "\r\n[已停止]";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>依左側「命令參數編輯」區塊的參數執行打標（DXF / Lines）。</summary>
        private void btnCLIExecuteMark_Click(object sender, EventArgs e)
        {
            RefreshCLIBuilderCommand();
            var argsList = BuildCLIBuilderArgsList();
            ExecuteCLIArgs(argsList, outputTarget: txtCLIOutput);
        }

        /// <summary>QRCODE 區塊「依此命令打標」— 僅使用 QR 專用命令執行。</summary>
        private void btnCLIQRExecuteMark_Click(object sender, EventArgs e)
        {
            if (txtCLIQRContent == null || string.IsNullOrWhiteSpace(txtCLIQRContent.Text))
            {
                MessageBox.Show("請先輸入 QR Content！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            RefreshCLIQRCommand();
            var argsList = BuildCLIQRArgsList();
            ExecuteCLIArgs(argsList, outputTarget: txtCLIQROutput);
        }

        /// <summary>共用 CLI 打標執行流程：驗證參數 → 載入內容（DXF / Lines / QR 白底）→ StartMarking。
        /// 依 argsList 內容自動判斷模式。outputTarget 指定執行訊息要附加到哪個輸出框。</summary>
        private void ExecuteCLIArgs(List<string> argsList, TextBox outputTarget)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先在連接設定頁籤初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 強制加 --mark（按下此按鈕就代表要打標）
            if (!argsList.Contains("--mark")) argsList.Add("--mark");

            // 透過 CommandLineArgs.Parse 驗證並轉成結構
            CommandLineArgs cliArgs;
            try
            {
                cliArgs = CommandLineArgs.Parse(argsList.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"參數解析失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string errMsg;
            if (!cliArgs.Validate(out errMsg))
            {
                MessageBox.Show($"參數錯誤：{errMsg}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int boardIndex = cliArgs.BoardIndex;
            if (boardIndex < 0 || boardIndex >= m_bBoardInit.Length || !m_bBoardInit[boardIndex])
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 未成功初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsBoardBusy(boardIndex))
            {
                MessageBox.Show($"晶片板 {boardIndex + 1} 正在執行其他操作，請先停止再試。", "板忙碌中", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 暫存 m_AutoModeArgs，讓既有 ApplyLaserParamsAuto / LoadDxfAuto / DrawLineAuto 能正確套用 cliArgs
            var savedArgs = m_AutoModeArgs;
            m_AutoModeArgs = cliArgs;

            try
            {
                // 清空畫面
                m_MMMark[boardIndex].ResetFile();
                m_MMMark[boardIndex].SetDesktopCenter(0, 0);
                double wsW = cliArgs.WorkspaceWidthExplicit ? cliArgs.WorkspaceSize : m_WorkspaceSize;
                double wsH = cliArgs.WorkspaceHeightExplicit ? cliArgs.WorkspaceHeight : m_WorkspaceHeight;
                m_MMMark[boardIndex].SetDesktopSize(wsW, wsH);
                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(200);

                // 載入內容（DXF / Lines / QR 三擇一，依 cliArgs 出現的欄位決定）
                bool hasContent = false;
                bool isQRWhiteBg = false;
                if (!string.IsNullOrEmpty(cliArgs.DxfPath))
                {
                    if (!LoadDxfAuto(boardIndex, cliArgs.DxfPath))
                    {
                        MessageBox.Show("DXF 載入失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    hasContent = true;
                }
                else if (cliArgs.Lines != null && cliArgs.Lines.Count > 0)
                {
                    // 取所有線段範圍中心後置中
                    double minX = double.MaxValue, maxX = double.MinValue;
                    double minY = double.MaxValue, maxY = double.MinValue;
                    foreach (var line in cliArgs.Lines)
                    {
                        minX = Math.Min(minX, Math.Min(line.X1, line.X2));
                        maxX = Math.Max(maxX, Math.Max(line.X1, line.X2));
                        minY = Math.Min(minY, Math.Min(line.Y1, line.Y2));
                        maxY = Math.Max(maxY, Math.Max(line.Y1, line.Y2));
                    }
                    double offX = -(minX + maxX) / 2.0;
                    double offY = -(minY + maxY) / 2.0;
                    foreach (var line in cliArgs.Lines)
                    {
                        var centered = new LineSegment(line.X1 + offX, line.Y1 + offY, line.X2 + offX, line.Y2 + offY);
                        if (!DrawLineAuto(boardIndex, centered))
                        {
                            MessageBox.Show("繪製線段失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    m_MMMark[boardIndex].Redraw();
                    Application.DoEvents();
                    Thread.Sleep(300);
                    hasContent = true;
                }
                else if (!string.IsNullOrEmpty(cliArgs.QRContent) && cliArgs.QRWhiteBg)
                {
                    if (!BuildWhiteBgQR(boardIndex, MakeWhiteBgParams(cliArgs)))
                    {
                        MessageBox.Show("白底 QR 建立失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    hasContent = true;
                    isQRWhiteBg = true;
                }

                if (!hasContent)
                {
                    MessageBox.Show("沒有可打標的內容（請輸入 DXF 路徑、線段或 QR Content）。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 套用雷射參數（如有指定）
                // 白底 QR 已在 BuildWhiteBgQR 內對各物件個別設好參數，這裡跳過以免覆寫。
                if (!isQRWhiteBg && (cliArgs.Power.HasValue || cliArgs.Speed.HasValue || cliArgs.Frequency.HasValue ||
                    cliArgs.PulseWidth.HasValue || cliArgs.MarkRepeat.HasValue || cliArgs.WobbleWidth.HasValue))
                {
                    ApplyLaserParamsAuto(boardIndex);
                }

                // 預覽模式或正常打標
                if (cliArgs.PreviewMode > 0)
                {
                    // 預覽：用既有 ExecutePreviewAuto 流程的精簡版（不關閉視窗）
                    m_MMMark[boardIndex].SetPreviewMode(2);
                    m_MMMark[boardIndex].MarkStandBy();
                    if (m_MMMark[boardIndex].StartMarking(3) != 0)
                    {
                        MessageBox.Show("預覽啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    m_bPreviewing = true;
                    timerPreview.Tag = new object[] { boardIndex, cliArgs.PreviewTime > 0 ? cliArgs.PreviewTime : 15 };
                    timerPreview.Start();
                }
                else
                {
                    // 正常打標
                    m_MMMark[boardIndex].MarkStandBy();
                    if (m_MMMark[boardIndex].StartMarking(4) != 0)
                    {
                        MessageBox.Show("打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    timerMark.Tag = boardIndex;
                    timerMark.Start();
                }

                btnCLIExecuteMark.Enabled = false;
                if (btnCLIQRExecuteMark != null) btnCLIQRExecuteMark.Enabled = false;
                if (outputTarget != null) outputTarget.Text += "\r\n\r\n[執行中] 晶片板 " + (boardIndex + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"執行失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 還原 m_AutoModeArgs，避免影響其他既有 CLI/自動模式流程
                m_AutoModeArgs = savedArgs;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private const uint WM_CLOSE = 0x0010;

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Failsafe：若清理程序卡住 > 6 秒仍未結束，強制結束 process，避免掛住 UI
            var forceExit = new Thread(() =>
            {
                Thread.Sleep(6000);
                try { System.Diagnostics.Debug.WriteLine("FormClosed 清理超時 6s，強制 Environment.Exit"); } catch { }
                try { Environment.Exit(ExitCode); } catch { }
            });
            forceExit.IsBackground = true;
            forceExit.Start();

            CleanupOldControls();
            // this.Hide();

            // // 啟動計時器，自動偵測並關閉 SDK 彈出的「程式關閉中」視窗
            // var closerThread = new Thread(() =>
            // {
            //     for (int attempt = 0; attempt < 50; attempt++) // 最多偵測 5 秒
            //     {
            //         Thread.Sleep(100);
            //         IntPtr hwnd = FindWindow(null, "程式關閉中");
            //         if (hwnd != IntPtr.Zero)
            //         {
            //             PostMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            //         }
            //     }
            // });
            // closerThread.IsBackground = true;
            // closerThread.Start();

            // // 呼叫 Finish() 正確關閉雷射硬體（含紅外線）
            // for (int i = 0; i < 4; i++)
            // {
            //     if (m_MMMark[i] != null && m_bBoardInit[i])
            //     {
            //         try { m_MMMark[i].Finish(); } catch { }
            //     }
            //     if (m_MMEdit[i] != null && m_bBoardInit[i])
            //     {
            //         try { m_MMEdit[i].Finish(); } catch { }
            //     }
            // }

            // Environment.Exit(ExitCode);
        }
    }

    /// <summary>
    /// DXF 線段資料結構
    /// </summary>
    public class DXFLine
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public double Length
        {
            get
            {
                double dx = X2 - X1;
                double dy = Y2 - Y1;
                return Math.Sqrt(dx * dx + dy * dy);
            }
        }

        public DXFLine(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override string ToString()
        {
            return $"({X1:F2}, {Y1:F2}) -> ({X2:F2}, {Y2:F2}), 長度: {Length:F2}";
        }
    }
}
