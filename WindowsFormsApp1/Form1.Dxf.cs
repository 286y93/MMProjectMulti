using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // DXF 頁籤：瀏覽 / 載入 / 打標 / 預覽 / 停止 / 解析（自 Form1.cs 抽出，partial 拆檔，行為不變）。
    // 註：共用的預覽基礎設施（btnStopPreview_Click / timerPreview_Tick / ResetPreviewButtonsAfterStop）
    //     被其他頁籤共用，仍保留在 Form1.cs。
    public partial class Form1
    {
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
    }
}
