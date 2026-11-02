using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace GuanDaLaserApp
{
    /// <summary>
    /// Data Matrix (ECC200) 正方形尺寸與 SDK Set2DBarcodeSize 代碼對照。
    /// 出自 MultiMM OCX Manual v2.7 Set2DBarcodeSize：0 = 自動，1~24 依序為下列正方形（無 28x28），
    /// 25~30 為長方形（本程式未使用）。
    /// </summary>
    internal static class DataMatrixSizes
    {
        public static readonly int[] Square =
            { 10, 12, 14, 16, 18, 20, 22, 24, 26, 32, 36, 40, 44, 48, 52, 64, 72, 80, 88, 96, 104, 120, 132, 144 };

        /// <summary>格數 → Set2DBarcodeSize 代碼；不是合法正方形尺寸回 -1。</summary>
        public static int ToSdkCode(int symbol)
        {
            int idx = Array.IndexOf(Square, symbol);
            return idx < 0 ? -1 : idx + 1;
        }

        public static bool IsSquare(int symbol) => Array.IndexOf(Square, symbol) >= 0;
    }

    // 9. DataMatrix 頁 + CLI / daemon 共用後端：底部矩形 + 反相 Data Matrix 雙圖層（比照 8. AprilTag）。
    //   Data Matrix 由 SDK AddBarcode(21 = ECC200) 產生，反相（SetBarcodeInvert）+ 反相外框（Set2DBarcodeBorder）。
    //   長寬 = 含反相外框的實際總長寬；格數由 SDK 依內容自動決定（或指定），每格大小 = 長寬 / (格數 + 2 × 外框)。
    //   矩形層：蓋住整個 DM（實際渲染尺寸）+ RectExtra，先打。
    //   打標目標可選：只打矩形 / 只打 Data Matrix / 矩形 + Data Matrix。
    public partial class Form1
    {
        private const int BARCODE_TYPE_DATAMATRIX = 21;   // SDK：21 = ECC 200（Data Matrix）
        private const string DM_CODE_NAME = "DM_Code";
        private const string DM_RECT_NAME = "DM_Rect";
        // 量測格數用的暫定單元大小 (mm)：先以此大小渲染，由實際寬度反推格數
        private const double DM_PROBE_CELL = 1.0;

        // GUI 預覽示意圖用：最近一次建立時 SDK 實際決定的格數（0 = 尚未建立過）
        private int m_DMLastSymbol = 0;

        // ============================================================
        // 共用後端（GUI / CLI 模式 A / daemon）
        // ============================================================

        /// <summary>
        /// 依 CLI 參數組出 Data Matrix 參數：未帶的用 DataMatrixParams 預設值。
        /// Data Matrix 層：--dm-*；功率 / 速度未帶時 fallback 到通用 --power/--speed。
        /// 矩形層：共用 --rect-*（不吃 fallback）。
        /// </summary>
        private DataMatrixParams MakeDataMatrixParams(CommandLineArgs a)
        {
            var p = new DataMatrixParams
            {
                Content = a.DMContent,
                Width = a.DMWidth,
                Height = a.DMHeight,
                SymbolSize = a.DMSymbol,
                Border = a.DMBorder,
                RectExtra = a.DMRectExtra,
                MarkRect = a.DMTarget == "rect" || a.DMTarget == "all",
                MarkCode = a.DMTarget == "dm" || a.DMTarget == "all",
            };
            if (a.DMPower.HasValue) p.CodePower = a.DMPower.Value;
            else if (a.Power.HasValue) p.CodePower = a.Power.Value;
            if (a.DMSpeed.HasValue) p.CodeSpeed = a.DMSpeed.Value;
            else if (a.Speed.HasValue) p.CodeSpeed = a.Speed.Value;
            if (a.DMFreq.HasValue) p.CodeFreq = a.DMFreq.Value;
            if (a.DMPulseWidth.HasValue) p.CodePulseWidth = a.DMPulseWidth.Value;
            if (a.RectPower.HasValue) p.RectPower = a.RectPower.Value;
            if (a.RectSpeed.HasValue) p.RectSpeed = a.RectSpeed.Value;
            if (a.RectFreq.HasValue) p.RectFreq = a.RectFreq.Value;
            if (a.RectPulseWidth.HasValue) p.RectPulseWidth = a.RectPulseWidth.Value;
            return p;
        }

        /// <summary>矩形長寬是否放得進工作區。放不下時回 false 並給出原因。</summary>
        private bool DataMatrixFitsWorkspace(DataMatrixParams p, double workspaceW, double workspaceH, out string error)
        {
            if (p.RectWidth > workspaceW || p.RectHeight > workspaceH)
            {
                error = $"Data Matrix 矩形 {p.RectWidth:F2}×{p.RectHeight:F2} mm 超過工作區 {workspaceW}×{workspaceH} mm";
                return false;
            }
            error = null;
            return true;
        }

        /// <summary>
        /// 建立 Data Matrix 雙圖層物件（不 StartMarking）。中心在 (0,0)。
        /// 前提：呼叫端已 ResetFile + SetDesktopCenter/SetDesktopSize。
        /// 各物件雷射參數在此設定，呼叫端不可再套用通用參數。矩形以 ChangeObjectOrder 排到 DM 前面先打。
        /// 回傳 true=成功；診斷寫到 stderr（CLI / daemon log 可見）。
        /// </summary>
        private bool BuildDataMatrixLayers(int boardIndex, DataMatrixParams p)
        {
            if (!p.MarkRect && !p.MarkCode)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix 未選擇任何打標目標");
                return false;
            }

            // 只打矩形：矩形直接用目標長寬（DM 會被校正到目標長寬，兩者一致）
            double rectBaseW = p.Width, rectBaseH = p.Height;

            // ---- ① Data Matrix（反相：只打白格）----
            if (p.MarkCode)
            {
                long rc = m_MMMark[boardIndex].AddBarcode(
                    BARCODE_TYPE_DATAMATRIX, p.Content, 0, 0, p.Width, p.Height, "", DM_CODE_NAME);
                Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix AddBarcode rc={rc} content=\"{p.Content}\"");
                if (rc != 0)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix 建立條碼失敗 rc={rc}");
                    return false;
                }
                Application.DoEvents();
                Thread.Sleep(200);

                // 格數：0 = SDK 依內容自動決定
                int sizeCode = p.SymbolSize > 0 ? DataMatrixSizes.ToSdkCode(p.SymbolSize) : 0;
                long rcSize = m_MMEdit[boardIndex].Set2DBarcodeSize(DM_CODE_NAME, sizeCode);
                // 反相雕刻（只打白格）
                m_MMEdit[boardIndex].SetBarcodeInvert(DM_CODE_NAME, 1);
                // 固定單元大小，先用暫定值，稍後依格數反推
                m_MMEdit[boardIndex].Set2DBarcodeFixedType(DM_CODE_NAME, 0);
                m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(DM_CODE_NAME, DM_PROBE_CELL, DM_PROBE_CELL);
                ApplyQRBorder(boardIndex, DM_CODE_NAME, p.Border);                       // 反相外框單元 + 線段延伸
                // 以下製程設定與 QRCODE_白底（BuildWhiteBgQR）的 QR 層相同
                m_MMEdit[boardIndex].SetBarcodeMarkStyle(DM_CODE_NAME, 2);
                m_MMEdit[boardIndex].SetBarcodeLineType(DM_CODE_NAME, 0);
                m_MMEdit[boardIndex].SetBarcodeSpotSize(DM_CODE_NAME, 0.04);
                m_MMEdit[boardIndex].SetBarcodeLineTimes(DM_CODE_NAME, 4);
                m_MMEdit[boardIndex].SetFillStartAngle(DM_CODE_NAME, 0);
                m_MMEdit[boardIndex].SetFillStepAngle(DM_CODE_NAME, 45);
                m_MMEdit[boardIndex].SetBarcodeLineTwoway(DM_CODE_NAME, 1);
                m_MMEdit[boardIndex].SetFrameSwitch(DM_CODE_NAME, 1);
                m_MMEdit[boardIndex].SetFillSwitch(DM_CODE_NAME, 1);
                m_MMEdit[boardIndex].SetFillFirstExt(DM_CODE_NAME, 1, 1);
                m_MMMark[boardIndex].SetMarkRepeat(DM_CODE_NAME, 1);
                m_MMEdit[boardIndex].SetBarcodeSpotDelay(DM_CODE_NAME, 1000);
                m_MMMark[boardIndex].SetSpeed(DM_CODE_NAME, p.CodeSpeed);
                m_MMMark[boardIndex].SetPower(DM_CODE_NAME, p.CodePower);
                m_MMMark[boardIndex].SetFrequency(DM_CODE_NAME, p.CodeFreq);
                m_MMMark[boardIndex].SetPulseWidth(DM_CODE_NAME, p.CodePulseWidth);

                // 量測格數：以暫定單元渲染，實際寬度 / 單元 = 總格數
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(300);
                double probeW = m_MMEdit[boardIndex].GetWidth(DM_CODE_NAME);
                int cellsAcross = (int)Math.Round(probeW / DM_PROBE_CELL);
                // 正常情況 GetWidth 含反相外框（同白底 QR outer 模式）；若扣掉外框不是合法尺寸、
                // 而總格數本身是合法尺寸，代表 SDK 回傳的寬度不含外框。
                int symbol = cellsAcross - 2 * p.Border;
                bool widthHasBorder = true;
                if (!DataMatrixSizes.IsSquare(symbol) && DataMatrixSizes.IsSquare(cellsAcross))
                {
                    symbol = cellsAcross;
                    widthHasBorder = false;
                }
                Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix Set2DBarcodeSize({sizeCode}) rc={rcSize} " +
                    $"readback={m_MMEdit[boardIndex].Get2DBarcodeSize(DM_CODE_NAME)} probeW={probeW} → symbol={symbol} " +
                    $"(寬度{(widthHasBorder ? "含" : "不含")}外框)");
                if (symbol <= 0)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix 無法判定格數（probeW={probeW}）");
                    return false;
                }
                if (!DataMatrixSizes.IsSquare(symbol))
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] Warning: DataMatrix 判定格數 {symbol} 不是標準正方形尺寸，請確認 log 與成品");
                if (p.SymbolSize > 0 && symbol != p.SymbolSize)
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] Warning: 指定 {p.SymbolSize}×{p.SymbolSize}，SDK 實際為 {symbol}×{symbol}（內容可能超出容量）");
                m_DMLastSymbol = symbol;

                // 反推單元：含外框總長寬 = 單元 × (格數 + 2 × 外框)
                int totalCells = symbol + 2 * p.Border;
                double cellW = p.Width / totalCells;
                double cellH = p.Height / totalCells;
                m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(DM_CODE_NAME, cellW, cellH);
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                Thread.Sleep(300);

                // SDK 量到的寬度若不含外框，比對目標也要扣掉外框
                double expectW = widthHasBorder ? p.Width : cellW * symbol;
                double expectH = widthHasBorder ? p.Height : cellH * symbol;
                double actualW = m_MMEdit[boardIndex].GetWidth(DM_CODE_NAME);
                double actualH = m_MMEdit[boardIndex].GetHeight(DM_CODE_NAME);

                // SDK 對 cellSize 有內部捨入，依實際渲染尺寸等比修正一次（同白底 QR outer 模式）
                if (actualW > 0 && actualH > 0
                    && (Math.Abs(actualW - expectW) > QR_OUTER_TOLERANCE || Math.Abs(actualH - expectH) > QR_OUTER_TOLERANCE))
                {
                    cellW *= expectW / actualW;
                    cellH *= expectH / actualH;
                    m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(DM_CODE_NAME, cellW, cellH);
                    m_MMMark[boardIndex].Redraw();
                    Application.DoEvents();
                    Thread.Sleep(300);
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix 校正：rendered {actualW}x{actualH} → cell={cellW:F4}x{cellH:F4}");
                    actualW = m_MMEdit[boardIndex].GetWidth(DM_CODE_NAME);
                    actualH = m_MMEdit[boardIndex].GetHeight(DM_CODE_NAME);
                }

                // 矩形依 DM 實際渲染的含外框總尺寸
                if (actualW > 0) rectBaseW = widthHasBorder ? actualW : actualW + 2 * p.Border * cellW;
                if (actualH > 0) rectBaseH = widthHasBorder ? actualH : actualH + 2 * p.Border * cellH;
                Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix symbol={symbol}x{symbol} border={p.Border} " +
                    $"target={p.Width}x{p.Height} cell={cellW:F4}x{cellH:F4} rendered={actualW}x{actualH} total={rectBaseW:F3}x{rectBaseH:F3}");
                if (Math.Min(cellW, cellH) < QR_MIN_CELL_WARN)
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] Warning: DataMatrix 單元 {Math.Min(cellW, cellH):F4} mm 小於 {QR_MIN_CELL_WARN} mm，" +
                        "可能無法正確雕刻或讀取，請加大長寬或縮短內容");
            }

            // ---- ② 底部矩形（= DM 含外框總尺寸 + RectExtra）----
            if (p.MarkRect)
            {
                double rectW = rectBaseW + p.RectExtra;
                double rectH = rectBaseH + p.RectExtra;
                double halfW = rectW / 2.0, halfH = rectH / 2.0;
                long rR = m_MMEdit[boardIndex].AddRect(-halfW, -halfH, halfW, halfH, 0, "", DM_RECT_NAME);
                Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix AddRect rc={rR} size={rectW:F3}x{rectH:F3}");
                if (rR != 0)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix 建立矩形失敗 rc={rR}");
                    return false;
                }
                Application.DoEvents();
                Thread.Sleep(100);

                // 與 AprilTag（BuildAprilTagLayers）的底部矩形同一套填滿設定
                m_MMEdit[boardIndex].SetFillStyle(DM_RECT_NAME, 3);
                m_MMEdit[boardIndex].SetFrameLineType(DM_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillRoundPitch(DM_RECT_NAME, 0.04);
                m_MMEdit[boardIndex].SetFillPitch(DM_RECT_NAME, 0.04);
                m_MMEdit[boardIndex].SetFillTimes(DM_RECT_NAME, 4);
                m_MMEdit[boardIndex].SetFillInsideOut(DM_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillStartAngle(DM_RECT_NAME, 0);
                m_MMEdit[boardIndex].SetFillStepAngle(DM_RECT_NAME, 45);
                m_MMEdit[boardIndex].SetFillAverageDistribution(DM_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFrameSwitch(DM_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillSwitch(DM_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillFirstExt(DM_RECT_NAME, 1, 1);
                m_MMMark[boardIndex].SetMarkRepeat(DM_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetBarcodeSpotDelay(DM_RECT_NAME, 100);
                m_MMMark[boardIndex].SetSpeed(DM_RECT_NAME, p.RectSpeed);
                m_MMMark[boardIndex].SetPower(DM_RECT_NAME, p.RectPower);
                m_MMMark[boardIndex].SetFrequency(DM_RECT_NAME, p.RectFreq);
                m_MMMark[boardIndex].SetPulseWidth(DM_RECT_NAME, p.RectPulseWidth);

                // 矩形後建立，排到 DM 前面 → 先打矩形、再打 DM
                if (p.MarkCode)
                {
                    long rOrd = m_MMEdit[boardIndex].ChangeObjectOrder(DM_RECT_NAME, DM_CODE_NAME, 0);
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] DataMatrix ChangeObjectOrder rc={rOrd}");
                }
            }

            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);
            return true;
        }

        // ============================================================
        // GUI：9. DataMatrix 頁
        // ============================================================

        /// <summary>尺寸下拉選單 → 格數（0 = 自動）。項目 0 為「自動」，其後依序為 DataMatrixSizes.Square。</summary>
        private int SelectedDMSymbol() =>
            comboDMSymbol.SelectedIndex > 0 ? DataMatrixSizes.Square[comboDMSymbol.SelectedIndex - 1] : 0;

        /// <summary>讀取 UI 參數。驗證失敗回 null 並提示。</summary>
        private DataMatrixParams ReadDataMatrixParamsFromUI()
        {
            var p = new DataMatrixParams { Content = txtDMContent.Text };
            if (string.IsNullOrEmpty(p.Content))
            {
                MessageBox.Show("請輸入 Data Matrix 內容！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            if (!double.TryParse(txtDMWidth.Text.Trim(), out double w) || w <= 0
                || !double.TryParse(txtDMHeight.Text.Trim(), out double h) || h <= 0)
            {
                MessageBox.Show("請輸入有效的長、寬（mm，需大於 0）！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            p.Width = w;
            p.Height = h;
            p.SymbolSize = SelectedDMSymbol();
            p.Border = (int)numDMBorder.Value;
            if (!double.TryParse(txtDMRectExtra.Text.Trim(), out double extra) || extra < 0) extra = 0;
            p.RectExtra = extra;

            p.MarkRect = rdoDMMarkRect.Checked || rdoDMMarkAll.Checked;
            p.MarkCode = rdoDMMarkCode.Checked || rdoDMMarkAll.Checked;

            p.CodePower = PDx(txtDMCodePower.Text, p.CodePower);
            p.CodeSpeed = PD(txtDMCodeSpeed.Text, p.CodeSpeed);
            p.CodeFreq = PD(txtDMCodeFreq.Text, p.CodeFreq);
            p.CodePulseWidth = PD(txtDMCodePW.Text, p.CodePulseWidth);
            p.RectPower = PDx(txtDMRectPower.Text, p.RectPower);
            p.RectSpeed = PD(txtDMRectSpeed.Text, p.RectSpeed);
            p.RectFreq = PD(txtDMRectFreq.Text, p.RectFreq);
            p.RectPulseWidth = PD(txtDMRectPW.Text, p.RectPulseWidth);

            ReadWorkspaceSettings();
            if (!DataMatrixFitsWorkspace(p, m_WorkspaceSize, m_WorkspaceHeight, out string err))
            {
                MessageBox.Show(err + "，請縮小長寬或加大量。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return p;
        }

        private static string DataMatrixTargetText(DataMatrixParams p) =>
            p.MarkRect && p.MarkCode ? "矩形+DataMatrix" : p.MarkRect ? "矩形" : "DataMatrix";

        /// <summary>檢查 DataMatrix 頁選擇的板可用性。回傳 -1 = 無效。</summary>
        private int GetDataMatrixBoard()
        {
            if (!m_bInit) { MessageBox.Show("請先初始化！", "錯誤"); return -1; }
            int b = comboBoardDM.SelectedIndex;
            if (b < 0 || b >= m_bBoardInit.Length || !m_bBoardInit[b])
            {
                MessageBox.Show($"晶片板 {b + 1} 未成功初始化", "錯誤"); return -1;
            }
            if (IsBoardBusy(b))
            {
                MessageBox.Show($"晶片板 {b + 1} 正在執行其他預覽 / 打標，請先停止再試。", "板忙碌中"); return -1;
            }
            return b;
        }

        /// <summary>GUI：清板後建立 Data Matrix 物件。</summary>
        private bool BuildDataMatrixForGui(int boardIndex, DataMatrixParams p)
        {
            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            bool ok = BuildDataMatrixLayers(boardIndex, p);
            // 建立後已知實際格數，更新尺寸資訊與示意圖
            UpdateDataMatrixInfo();
            pnlDMPreview.Invalidate();
            if (!ok)
            {
                MessageBox.Show("Data Matrix 建立失敗！詳細原因請看 log。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnDMPreview_Click(object sender, EventArgs e)
        {
            int b = GetDataMatrixBoard(); if (b < 0) return;
            var p = ReadDataMatrixParamsFromUI(); if (p == null) return;
            try
            {
                if (!BuildDataMatrixForGui(b, p)) return;
                m_MMMark[b].SetPreviewMode(2);
                m_MMMark[b].MarkStandBy();
                Application.DoEvents();
                if (m_MMMark[b].StartMarking(3) != 0)
                {
                    MessageBox.Show($"晶片板 {b + 1} 預覽啟動失敗！", "錯誤"); return;
                }
                m_bPreviewing = true;
                m_iPreviewBoard = b;
                timerPreview.Stop(); timerPreview.Start();
                btnDMPreview.Enabled = false;
                btnDMMark.Enabled = false;
                txtDMStatus.Text = $"晶片板 {b + 1} DataMatrix \"{p.Content}\"（{DataMatrixTargetText(p)}）紅光預覽中（15 秒後停止）";
            }
            catch (Exception ex) { MessageBox.Show($"Data Matrix 預覽失敗：{ex.Message}", "錯誤"); }
        }

        private void btnDMMark_Click(object sender, EventArgs e)
        {
            int b = GetDataMatrixBoard(); if (b < 0) return;
            var p = ReadDataMatrixParamsFromUI(); if (p == null) return;
            try
            {
                if (!BuildDataMatrixForGui(b, p)) return;
                m_MMMark[b].MarkStandBy();
                if (m_MMMark[b].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {b + 1} 打標啟動失敗！", "錯誤"); return;
                }
                timerMark.Tag = b;
                timerMark.Start();
                btnDMPreview.Enabled = false;
                btnDMMark.Enabled = false;
                txtDMStatus.Text = $"晶片板 {b + 1} DataMatrix \"{p.Content}\"（{p.Width}×{p.Height} mm，{DataMatrixTargetText(p)}）打標中";
            }
            catch (Exception ex) { MessageBox.Show($"Data Matrix 打標失敗：{ex.Message}", "錯誤"); }
        }

        private void btnDMStop_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;
            try
            {
                timerMark.Stop();
                timerPreview.Stop();
                int b = comboBoardDM.SelectedIndex;
                if (b >= 0 && b < m_bBoardInit.Length && m_bBoardInit[b])
                {
                    try { m_MMMark[b].StopMarking(); } catch { }
                    try { m_MMMark[b].MarkShutdown(); } catch { }
                }
                m_bPreviewing = false;
                m_iPreviewBoard = -1;
                ResetPreviewButtonsAfterStop();
                txtDMStatus.Text = "Data Matrix 已停止";
            }
            catch (Exception ex) { MessageBox.Show($"停止失敗：{ex.Message}", "錯誤"); }
        }

        // ---- 尺寸資訊與示意圖 ----

        private void DataMatrixInputs_Changed(object sender, EventArgs e)
        {
            // 內容 / 指定格數改變後，上次量到的格數不再適用
            if (sender == txtDMContent || sender == comboDMSymbol) m_DMLastSymbol = 0;
            UpdateDataMatrixInfo();
            pnlDMPreview.Invalidate();
        }

        /// <summary>目前可確定的格數：指定值 → 上次建立量到的值 → 0（未知）。</summary>
        private int KnownDMSymbol()
        {
            int s = SelectedDMSymbol();
            return s > 0 ? s : m_DMLastSymbol;
        }

        private void UpdateDataMatrixInfo()
        {
            if (!double.TryParse(txtDMWidth.Text.Trim(), out double w) || w <= 0
                || !double.TryParse(txtDMHeight.Text.Trim(), out double h) || h <= 0)
            {
                lblDMInfo.Text = "請輸入有效的長、寬";
                return;
            }
            if (!double.TryParse(txtDMRectExtra.Text.Trim(), out double extra) || extra < 0) extra = 0;
            int border = (int)numDMBorder.Value;
            int symbol = KnownDMSymbol();
            string rect = $"矩形 {w + extra:F2}×{h + extra:F2} mm";
            if (symbol > 0)
            {
                int total = symbol + 2 * border;
                lblDMInfo.Text = $"{symbol}×{symbol} 格 + 外框 {border}，每格 {w / total:F3}×{h / total:F3} mm\r\n{rect}";
            }
            else
            {
                lblDMInfo.Text = $"格數由 SDK 依內容決定（預覽 / 打標後顯示）\r\n{rect}";
            }
        }

        /// <summary>
        /// 成品外觀示意圖（非實際編碼）：外圍白色外框、左 / 下實線 L 形定位邊、上 / 右黑白交錯時脈邊，
        /// 資料區以灰色表示。格數未知時以 14×14 示意。
        /// </summary>
        private void pnlDMPreview_Paint(object sender, PaintEventArgs e)
        {
            int symbol = KnownDMSymbol();
            int drawSymbol = symbol > 0 ? symbol : 14;
            int border = (int)numDMBorder.Value;
            int n = drawSymbol + 2 * border;
            float cellPx = (float)Math.Min(pnlDMPreview.Width, pnlDMPreview.Height - 16) / n;
            float ox = (pnlDMPreview.Width - cellPx * n) / 2f;
            float oy = (pnlDMPreview.Height - 16 - cellPx * n) / 2f;

            var g = e.Graphics;
            g.Clear(Color.White);
            using (var dataBrush = new SolidBrush(Color.FromArgb(150, 150, 150)))
            {
                // 符號本體底色先畫資料區灰色
                g.FillRectangle(dataBrush, ox + border * cellPx, oy + border * cellPx, drawSymbol * cellPx, drawSymbol * cellPx);
            }
            for (int i = 0; i < drawSymbol; i++)
            {
                float x = ox + (border + i) * cellPx;
                float y = oy + (border + i) * cellPx;
                float top = oy + border * cellPx;
                float bottom = oy + (border + drawSymbol - 1) * cellPx;
                float left = ox + border * cellPx;
                float right = ox + (border + drawSymbol - 1) * cellPx;
                // L 形定位邊：左欄、下列實心
                g.FillRectangle(Brushes.Black, left, y, cellPx, cellPx);
                g.FillRectangle(Brushes.Black, x, bottom, cellPx, cellPx);
                // 時脈邊：上列、右欄黑白交錯（左上角起為黑、右下角為黑）
                g.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, x, top, cellPx, cellPx);
                g.FillRectangle((drawSymbol - 1 - i) % 2 == 0 ? Brushes.Black : Brushes.White, right, y, cellPx, cellPx);
            }
            // 外框（反相外框 = 白邊）輔助線
            g.DrawRectangle(Pens.LightGray, ox, oy, cellPx * n - 1, cellPx * n - 1);
            string caption = symbol > 0 ? $"{symbol}×{symbol} 格（示意，資料區為灰色）" : "格數未定（14×14 示意）";
            g.DrawString(caption, Font, Brushes.DimGray, 4, pnlDMPreview.Height - 16);
        }
    }
}
