using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private int m_iCurrentBoard = 0;

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

        // EMC6 IP 設定檔路徑
        private string[] m_IPConfigPaths = new string[]
        {
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM1\DevIPAddress.ini",
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM2\DevIPAddress.ini",
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM3\DevIPAddress.ini",
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM4\DevIPAddress.ini"
        };

        private TextBox[] m_txtIPs;

        // 工作區大小設定（mm），影響 SetDesktopSize 和 DXF 縮放
        private double m_WorkspaceSize = 150.0;
        private double m_MarginPercent = 0.9;

        // 自動模式相關
        private CommandLineArgs m_AutoModeArgs = null;
        private bool m_IsAutoMode = false;
        public int ExitCode { get; private set; } = 0;

        // GUI 模式建構子
        public Form1()
        {
            InitializeComponent();

            m_Panels = new Panel[] { panelBoard1, panelBoard2, panelBoard3, panelBoard4 };
            m_txtIPs = new TextBox[] { txtIP1, txtIP2, txtIP3, txtIP4 };
            comboBoard.SelectedIndex = 0;
            comboBoardDXF.SelectedIndex = 0;
            comboBoardLaser.SelectedIndex = 0;
            // txtPulseWidth 預設值 5 (在 Designer 中設定)
            m_IsAutoMode = false;

            // 同步 UI 顯示初始值
            txtWorkspace.Text = m_WorkspaceSize.ToString();
            txtMargin.Text = (m_MarginPercent * 100).ToString();

            this.btnPreviewDXF.Visible = false;
            this.btnClearDXF.Visible = false;
        }

        // 自動模式建構子
        public Form1(CommandLineArgs args) : this()
        {
            m_AutoModeArgs = args;
            // 改為依據 args.IsAutoMode 來設定，而非強制設為 true
            m_IsAutoMode = args.IsAutoMode;
            m_WorkspaceSize = args.WorkspaceSize;

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
                // 自動模式下，為了避免顯示不必要的視窗，可以設定 Opacity = 0 或WindowState = Minimized
                // 但不能 Hide，否則 Handle 不會建立，導致 OCX 初始化異常
                // this.WindowState = FormWindowState.Minimized; // 可選

                // 使用 Timer 延遲執行，確保 Form 完全載入
                System.Windows.Forms.Timer startTimer = new System.Windows.Forms.Timer();
                startTimer.Interval = 100;
                startTimer.Tick += (s, ev) =>
                {
                    startTimer.Stop();
                    ExecuteAutoMode();
                };
                startTimer.Start();
            }

            // 如果是自動模式，最小化視窗以減少干擾，但不能隱藏（OCX 需要 Window Handle）
            if (m_IsAutoMode)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
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
                    m_MMMark[i].SetDesktopSize(m_WorkspaceSize, m_WorkspaceSize);
                    m_MMMark[i].SetActiveDB(0);
                    m_MMMark[i].MarkStandBy();
                    m_MMMark[i].SetCurEditFun(2);
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
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                btnMark.Enabled = true;
                btnStop.Enabled = false;
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

            // 檢查打標/預覽是否已完成 (IsMarking 回傳 0 表示已停止)
            if (m_MMMark[boardIndex].IsMarking() == 0)
            {
                // 停止計時器
                timerMark.Stop();

                // 重要：打標或預覽結束後，必須將預覽模式轉回正常模式 (Mode 0)
                // 這樣下一次執行打標 (btnMarkDXF_Click) 時，才會正常的射出雷射
                m_MMMark[boardIndex].SetPreviewMode(0);

                // 關閉打標引擎
                m_MMMark[boardIndex].MarkShutdown();

                // 恢復 UI 按鈕狀態
                btnMark.Enabled = true;
                btnMarkDXF.Enabled = true;
                btnPreviewDXF.Enabled = true;
                btnLoadDXF.Enabled = true;
                btnLoadDXFFile.Enabled = true;
                btnClearDXF.Enabled = true;
                btnStop.Enabled = false;

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
            for (int i = 0; i < 4; i++)
            {
                m_txtIPs[i].Text = ReadIPFromIni(m_IPConfigPaths[i]);
            }
        }

        private string ReadIPFromIni(string iniPath)
        {
            if (!File.Exists(iniPath))
                return "";

            foreach (string line in File.ReadAllLines(iniPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("DEV0=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed.Substring(5).Trim();
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
            int successCount = 0;
            string errorInfo = "";

            for (int i = 0; i < 4; i++)
            {
                string ip = m_txtIPs[i].Text.Trim();
                try
                {
                    SaveIPToIni(m_IPConfigPaths[i], ip);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errorInfo += $"MM{i + 1}：{ex.Message}\n";
                }
            }

            if (successCount == 4)
            {
                MessageBox.Show("四組 IP 設定已儲存！\n\n注意：IP 變更需重新啟動程式並重新初始化才會生效。",
                    "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"已儲存 {successCount}/4 組。\n\n儲存失敗：\n{errorInfo}\n" +
                    "提示：可能需要以管理員身分執行程式才能寫入 Program Files 目錄。",
                    "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveIPToIni(string iniPath, string ip)
        {
            if (!File.Exists(iniPath))
                throw new FileNotFoundException($"找不到設定檔：{iniPath}");

            string[] lines = File.ReadAllLines(iniPath);
            for (int j = 0; j < lines.Length; j++)
            {
                if (lines[j].Trim().StartsWith("DEV0=", StringComparison.OrdinalIgnoreCase))
                {
                    lines[j] = $"DEV0={ip}";
                    break;
                }
            }
            File.WriteAllLines(iniPath, lines);
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
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"結束 MM27Dx64.exe 失敗 (PID: {proc.Id}): {ex.Message}");
                            }
                        }
                        System.Threading.Thread.Sleep(500); // 確保資源釋放
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"檢查背景程序失敗: {ex.Message}");
                }

                // 步驟 1: 初始化指定的板
                if (!InitializeBoardAuto(m_AutoModeArgs.BoardIndex, m_AutoModeArgs.ConfigPath))
                {
                    // ExitCode = 1; // 初始化失敗
                    // this.Close();
                    // 改為：不立刻關閉，讓使用者看到錯誤訊息，或者 Log 後再關閉
                    // 如要在 CLI 靜默模式下，應該輸出到 StdErr 並關閉
                    Console.Error.WriteLine("Error: Failed to initialize board.");
                    ExitCode = 1;
                    this.Close();
                    return; // 重要 return
                }

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

                if (!hasContent)
                {
                    // 如果沒有內容，是否需要打標？
                    Console.WriteLine("Warning: No content to mark.");
                }

                // 步驟 2.5: 套用雷射參數（如有指定）
                if (hasContent && (m_AutoModeArgs.Power.HasValue || m_AutoModeArgs.Speed.HasValue ||
                    m_AutoModeArgs.Frequency.HasValue || m_AutoModeArgs.PulseWidth.HasValue ||
                    m_AutoModeArgs.MarkRepeat.HasValue))
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
        /// 自動模式：初始化指定的板
        /// </summary>
        private bool InitializeBoardAuto(int boardIndex, string configPath)
        {
            try
            {
                // 建立控件
                m_MMMark[boardIndex] = new AxMMMarkx641();
                m_MMEdit[boardIndex] = new AxMMEditx641();

                m_MMMark[boardIndex].Left = 0;
                m_MMMark[boardIndex].Top = 0;
                m_MMMark[boardIndex].Width = m_Panels[boardIndex].Width;
                m_MMMark[boardIndex].Height = m_Panels[boardIndex].Height;

                m_Panels[boardIndex].Controls.Add(m_MMMark[boardIndex]);

                this.Controls.Add(m_MMEdit[boardIndex]);
                m_MMEdit[boardIndex].Visible = false;

                Application.DoEvents();

                // 初始化 MMMark
                m_MMMark[boardIndex].InitialExt(configPath);
                m_MMMark[boardIndex].SetDesktopCenter(0, 0);
                m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceSize);
                m_MMMark[boardIndex].SetActiveDB(0);
                m_MMMark[boardIndex].MarkStandBy();
                m_MMMark[boardIndex].SetCurEditFun(2);
                m_MMMark[boardIndex].Redraw();

                m_bBoardInit[boardIndex] = true;

                Application.DoEvents();

                // 初始化 MMEdit
                m_MMEdit[boardIndex].InitialExt(configPath);

                m_bInit = true;
                return true;
            }
            catch (Exception ex)
            {
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

                double halfSize = m_WorkspaceSize / 2.0;

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
                    // 全正數，假設是 Corner 原點，進行平移
                    x1 = line.X1 - halfSize;
                    y1 = line.Y1 - halfSize;
                    x2 = line.X2 - halfSize;
                    y2 = line.Y2 - halfSize;
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
                double scaleFactor = (m_WorkspaceSize * m_MarginPercent) / maxSpan;

                System.Diagnostics.Debug.WriteLine($"DXF 解析完成：{lines.Count} 條線段，工作區：{m_WorkspaceSize}，縮放比例：{scaleFactor:F4}");

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

                // 進入待命狀態
                m_MMMark[boardIndex].MarkStandBy();
                Application.DoEvents();

                // 啟動打標（模式 4 = 非阻塞，但 MarkingMate 建議使用模式 4）
                // 這裡使用模式 1 (同步) 可能更適合 CLI，但為了相容性保持模式 4 並手動等待
                int startResult = m_MMMark[boardIndex].StartMarking(4);
                if (startResult != 0)
                {
                    System.Diagnostics.Debug.WriteLine($"打標啟動失敗，錯誤碼：{startResult}");
                    Console.Error.WriteLine($"Error: StartMarking failed with code {startResult}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine("打標已啟動，等待完成...");
                Console.WriteLine("Marking started... Waiting for completion.");

                // 同步等待打標完成
                int loopCount = 0;
                // 注意：IsMarking() 返回非 0 表示正在打標
                while (m_MMMark[boardIndex].IsMarking() != 0)
                {
                    Application.DoEvents(); // 讓 UI 保持響應，這對 OCX 很重要
                    System.Threading.Thread.Sleep(100);
                    loopCount++;

                    // 每 10 次迴圈輸出一次狀態（每秒）
                    if (loopCount % 10 == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"打標進行中... ({loopCount * 100}ms)");
                    }

                    // 超時保護（60 秒）
                    if (loopCount > 600)
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
                    Console.WriteLine("Warning: No objects to apply laser params.");
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
                }

                Console.WriteLine($"Laser params applied to {objCount} objects." +
                    $" Power={m_AutoModeArgs.Power?.ToString() ?? "default"}" +
                    $" Speed={m_AutoModeArgs.Speed?.ToString() ?? "default"}" +
                    $" Freq={m_AutoModeArgs.Frequency?.ToString() ?? "default"}" +
                    $" PW={m_AutoModeArgs.PulseWidth?.ToString() ?? "default"}" +
                    $" Repeat={m_AutoModeArgs.MarkRepeat?.ToString() ?? "default"}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Warning: Failed to apply laser params - {ex.Message}");
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
                double maxSpan = Math.Max(origWidth, origHeight);
                double scaleFactor = (m_WorkspaceSize * m_MarginPercent) / maxSpan;

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
                btnLoadDXF.Enabled = false;
                btnLoadDXFFile.Enabled = false;
                btnMark.Enabled = false;
                btnStop.Enabled = true;
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
            // 此功能已移除
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
        /// 清理舊的控件（防止殘留導致初始化失敗）
        /// </summary>
        private void CleanupOldControls()
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    // 清理 MMMark
                    if (m_MMMark[i] != null)
                    {
                        try
                        {
                            if (m_bBoardInit[i])
                            {
                                m_MMMark[i].Finish();
                            }

                            if (m_Panels[i].Controls.Contains(m_MMMark[i]))
                            {
                                m_Panels[i].Controls.Remove(m_MMMark[i]);
                            }

                            m_MMMark[i].Dispose();
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
                                m_MMEdit[i].Finish();
                            }

                            if (this.Controls.Contains(m_MMEdit[i]))
                            {
                                this.Controls.Remove(m_MMEdit[i]);
                            }

                            m_MMEdit[i].Dispose();
                            m_MMEdit[i] = null;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"清理 MMEdit[{i}] 失敗：{ex.Message}");
                        }
                    }

                    m_bBoardInit[i] = false;
                }

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
                    successCount++;

                    sb.AppendLine($"物件 [{objName}]:");
                    sb.AppendLine($"  Power={r1} Speed={r2} Freq={r3} PW={r4} Repeat={r5}");
                    if (r1 != 0 || r2 != 0 || r3 != 0 || r4 != 0 || r5 != 0)
                        sb.AppendLine($"  ** 有參數設定失敗 (非0=失敗) **");
                }

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

                    sb.AppendLine($"物件 [{objName}]:");
                    sb.AppendLine($"  功率: {p:F1}%");
                    sb.AppendLine($"  速度: {s:F1} mm/s");
                    sb.AppendLine($"  頻率: {f:F1} kHz");
                    sb.AppendLine($"  脈波寬度: {pw:F1}");
                    sb.AppendLine($"  雷射次數: {mr}");
                    sb.AppendLine();

                    // 以第一個物件的值回填到 UI
                    if (i == 0)
                    {
                        numPower.Value = (decimal)Math.Max(0, Math.Min(100, p));
                        txtSpeed.Text = s.ToString("F1");
                        txtFrequency.Text = f.ToString("F1");
                        txtPulseWidth.Text = pw.ToString("F1");
                        numMarkRepeat.Value = Math.Max(1, Math.Min(9999, (decimal)mr));
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
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

            // 直接強制結束程序，跳過 SDK Finish() 避免「程式關閉中」視窗
            // OS 會自動回收所有資源（記憶體、Handle、COM 物件等）
            Environment.Exit(ExitCode);
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
