using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // 命令提示頁籤 + 並行驗證（自 Form1.cs 抽出，partial 拆檔，行為不變）。
    // 註：共用 helper IsBoardBusy / SplitCommandLine（後者 daemon 也用）仍保留在 Form1.cs。
    public partial class Form1
    {
        // ============================================================
        // 命令提示頁籤：隨機產生 5 組指令（含 --wobble-width，正式打標模式）。
        // 使用者可手動編輯 textbox 加入 --preview 切回紅光預覽。
        // CmdPreviewSpec 定義見 Form1.Models.cs。
        // ============================================================

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
    }
}
