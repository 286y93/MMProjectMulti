using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // CLI Builder 頁籤：組合 / 刷新 / 執行命令列參數（自 Form1.cs 抽出，partial 拆檔，行為不變）。
    public partial class Form1
    {
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
    }
}
