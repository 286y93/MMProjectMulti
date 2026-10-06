using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // QR Code 頁籤 + 白底 QR 建立 / 打標（自 Form1.cs 抽出，partial 拆檔，行為不變）。
    // 註：共用後端 BuildWhiteBgQR / MakeWhiteBgParams / ApplyQRInvert / ApplyQRECLevelLow、
    //     序號 helper NextSerial / GetQRCodeParaDir、WriteWhiteBgParaLog、consts 仍保留在 Form1.cs。
    public partial class Form1
    {
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
    }
}
