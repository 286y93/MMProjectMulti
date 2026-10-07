using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // 8. AprilTag 頁 + CLI / daemon 共用後端：底部矩形 + 反相 tag36h11 雙圖層（比照 QRCODE_白底）。
    //   矩形層：蓋住整個 tag（含外圈白邊）+ RectExtra，先打。
    //   AprilTag 層：反相，打「白格 + 外圈白邊」，黑框與黑色資料格保留矩形底色。
    //   打標目標可選：只打矩形 / 只打 AprilTag / 矩形 + AprilTag。
    public partial class Form1
    {
        private const string AT_RECT_NAME = "AT_Rect";
        private const string AT_TAG_PREFIX = "AT_Tag_";

        // AprilTag 層填滿參數（外框 + 填滿，讓格子邊緣銳利）
        private const int AT_FILL_STYLE = 1;
        private const double AT_FILL_PITCH = 0.04;
        private const double AT_FILL_ROUND_PITCH = 0.04;
        private const int AT_FILL_TIMES = 2;
        private const double AT_FILL_STEP_ANGLE = 90;

        // ============================================================
        // 共用後端（GUI / CLI 模式 A / daemon）
        // ============================================================

        /// <summary>
        /// 依 CLI 參數組出 AprilTag 參數：未帶的用 AprilTagParams 預設值。
        /// AprilTag 層：--tag-*；功率 / 速度未帶時 fallback 到通用 --power/--speed。
        /// 矩形層：共用 --rect-*（不吃 fallback）。
        /// </summary>
        private AprilTagParams MakeAprilTagParams(CommandLineArgs a)
        {
            var p = new AprilTagParams
            {
                TagId = a.AprilTagId ?? 0,
                Size = a.TagSize,
                RectExtra = a.TagRectExtra,
                MarkRect = a.TagTarget == "rect" || a.TagTarget == "all",
                MarkTag = a.TagTarget == "tag" || a.TagTarget == "all",
            };
            if (a.TagPower.HasValue) p.TagPower = a.TagPower.Value;
            else if (a.Power.HasValue) p.TagPower = a.Power.Value;
            if (a.TagSpeed.HasValue) p.TagSpeed = a.TagSpeed.Value;
            else if (a.Speed.HasValue) p.TagSpeed = a.Speed.Value;
            if (a.TagFreq.HasValue) p.TagFreq = a.TagFreq.Value;
            if (a.TagPulseWidth.HasValue) p.TagPulseWidth = a.TagPulseWidth.Value;
            if (a.RectPower.HasValue) p.RectPower = a.RectPower.Value;
            if (a.RectSpeed.HasValue) p.RectSpeed = a.RectSpeed.Value;
            if (a.RectFreq.HasValue) p.RectFreq = a.RectFreq.Value;
            if (a.RectPulseWidth.HasValue) p.RectPulseWidth = a.RectPulseWidth.Value;
            return p;
        }

        /// <summary>矩形邊長是否放得進工作區。放不下時回 false 並給出原因。</summary>
        private bool AprilTagFitsWorkspace(AprilTagParams p, double workspaceW, double workspaceH, out string error)
        {
            double minWorkspace = Math.Min(workspaceW, workspaceH);
            if (p.RectWidth > minWorkspace)
            {
                error = $"AprilTag 矩形邊長 {p.RectWidth:F2} mm 超過工作區 {minWorkspace} mm";
                return false;
            }
            error = null;
            return true;
        }

        /// <summary>
        /// 建立 AprilTag 雙圖層物件（不 StartMarking）。tag 中心在 (0,0)。
        /// 前提：呼叫端已 ResetFile + SetDesktopCenter/SetDesktopSize。
        /// 物件依建立順序打標：矩形先建立 → 先打。各物件雷射參數在此設定，呼叫端不可再套用通用參數。
        /// 座標：Y 軸向上為正，圖樣第 0 列（最上排）對應 Y 最大值。
        /// 回傳 true=成功；診斷寫到 stderr（CLI / daemon log 可見）。
        /// </summary>
        private bool BuildAprilTagLayers(int boardIndex, AprilTagParams p)
        {
            if (!p.MarkRect && !p.MarkTag)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] AprilTag 未選擇任何打標目標");
                return false;
            }

            // ---- ① 底部矩形 ----
            if (p.MarkRect)
            {
                double half = p.RectWidth / 2.0;
                long rR = m_MMEdit[boardIndex].AddRect(-half, -half, half, half, 0, "", AT_RECT_NAME);
                Console.Error.WriteLine($"[Board {boardIndex + 1}] AprilTag AddRect rc={rR} size={p.RectWidth:F3} (tag {p.TagTotalWidth:F3} + extra {p.RectExtra})");
                if (rR != 0)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] AprilTag 建立矩形失敗 rc={rR}");
                    return false;
                }
                Application.DoEvents();
                Thread.Sleep(100);

                // 與 QRCODE_白底（BuildWhiteBgQR）的白底矩形同一套填滿設定
                m_MMEdit[boardIndex].SetFillStyle(AT_RECT_NAME, 3);
                m_MMEdit[boardIndex].SetFrameLineType(AT_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillRoundPitch(AT_RECT_NAME, 0.04);
                m_MMEdit[boardIndex].SetFillPitch(AT_RECT_NAME, 0.04);
                m_MMEdit[boardIndex].SetFillTimes(AT_RECT_NAME, 4);
                m_MMEdit[boardIndex].SetFillInsideOut(AT_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillStartAngle(AT_RECT_NAME, 0);
                m_MMEdit[boardIndex].SetFillStepAngle(AT_RECT_NAME, 45);
                m_MMEdit[boardIndex].SetFillAverageDistribution(AT_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFrameSwitch(AT_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillSwitch(AT_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetFillFirstExt(AT_RECT_NAME, 1, 1);
                m_MMMark[boardIndex].SetMarkRepeat(AT_RECT_NAME, 1);
                m_MMEdit[boardIndex].SetBarcodeSpotDelay(AT_RECT_NAME, 100);
                m_MMMark[boardIndex].SetSpeed(AT_RECT_NAME, p.RectSpeed);
                m_MMMark[boardIndex].SetPower(AT_RECT_NAME, p.RectPower);
                m_MMMark[boardIndex].SetFrequency(AT_RECT_NAME, p.RectFreq);
                m_MMMark[boardIndex].SetPulseWidth(AT_RECT_NAME, p.RectPulseWidth);
            }

            // ---- ② AprilTag 層（反相：打白格 + 外圈白邊）----
            if (p.MarkTag)
            {
                int n = AprilTag36h11.TotalWidth;
                double cell = p.Size / AprilTag36h11.WidthAtBorder;
                double halfCells = n / 2.0;

                bool[,] black = AprilTag36h11.GetBlackCells(p.TagId);
                var white = new bool[n, n];
                for (int r = 0; r < n; r++)
                    for (int c = 0; c < n; c++)
                        white[r, c] = !black[r, c];
                var rects = AprilTag36h11.MergeRects(white);

                for (int i = 0; i < rects.Count; i++)
                {
                    var r = rects[i];
                    string name = AT_TAG_PREFIX + i;
                    double left = (r.Col - halfCells) * cell;
                    double right = (r.Col + r.Cols - halfCells) * cell;
                    double top = (halfCells - r.Row) * cell;
                    double bottom = (halfCells - r.Row - r.Rows) * cell;

                    long rc = m_MMEdit[boardIndex].AddRect(left, bottom, right, top, 0, "", name);
                    if (rc != 0)
                    {
                        Console.Error.WriteLine($"[Board {boardIndex + 1}] AprilTag 建立 tag 矩形 #{i} 失敗 rc={rc}");
                        return false;
                    }

                    m_MMEdit[boardIndex].SetFillStyle(name, AT_FILL_STYLE);
                    m_MMEdit[boardIndex].SetFillPitch(name, AT_FILL_PITCH);
                    m_MMEdit[boardIndex].SetFillRoundPitch(name, AT_FILL_ROUND_PITCH);
                    m_MMEdit[boardIndex].SetFillTimes(name, AT_FILL_TIMES);
                    m_MMEdit[boardIndex].SetFillStepAngle(name, AT_FILL_STEP_ANGLE);
                    m_MMEdit[boardIndex].SetFillAverageDistribution(name, 1);
                    m_MMEdit[boardIndex].SetFrameSwitch(name, 1);
                    m_MMEdit[boardIndex].SetFillSwitch(name, 1);
                    m_MMEdit[boardIndex].SetFillFirstExt(name, 0, 1);
                    m_MMMark[boardIndex].SetMarkRepeat(name, 1);
                    m_MMMark[boardIndex].SetSpeed(name, p.TagSpeed);
                    m_MMMark[boardIndex].SetPower(name, p.TagPower);
                    m_MMMark[boardIndex].SetFrequency(name, p.TagFreq);
                    m_MMMark[boardIndex].SetPulseWidth(name, p.TagPulseWidth);
                }
                Console.Error.WriteLine($"[Board {boardIndex + 1}] AprilTag 36h11 id={p.TagId} size={p.Size}mm cell={cell:F4}mm tagRects={rects.Count}（反相：白格）");
            }

            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);
            return true;
        }

        // ============================================================
        // GUI：8. AprilTag 頁
        // ============================================================

        /// <summary>讀取 UI 參數。驗證失敗回 null 並提示。</summary>
        private AprilTagParams ReadAprilTagParamsFromUI()
        {
            var p = new AprilTagParams { TagId = (int)numATId.Value };

            if (!double.TryParse(txtATSize.Text.Trim(), out double size) || size <= 0)
            {
                MessageBox.Show("請輸入有效的大小（mm，需大於 0）！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            p.Size = size;
            if (!double.TryParse(txtATRectExtra.Text.Trim(), out double extra) || extra < 0) extra = 0;
            p.RectExtra = extra;

            p.MarkRect = rdoATMarkRect.Checked || rdoATMarkAll.Checked;
            p.MarkTag = rdoATMarkTag.Checked || rdoATMarkAll.Checked;

            p.TagPower = PDx(txtATTagPower.Text, p.TagPower);
            p.TagSpeed = PD(txtATTagSpeed.Text, p.TagSpeed);
            p.TagFreq = PD(txtATTagFreq.Text, p.TagFreq);
            p.TagPulseWidth = PD(txtATTagPW.Text, p.TagPulseWidth);
            p.RectPower = PDx(txtATRectPower.Text, p.RectPower);
            p.RectSpeed = PD(txtATRectSpeed.Text, p.RectSpeed);
            p.RectFreq = PD(txtATRectFreq.Text, p.RectFreq);
            p.RectPulseWidth = PD(txtATRectPW.Text, p.RectPulseWidth);

            ReadWorkspaceSettings();
            if (!AprilTagFitsWorkspace(p, m_WorkspaceSize, m_WorkspaceHeight, out string err))
            {
                MessageBox.Show(err + "，請縮小大小或加大量。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return p;
        }

        private static string AprilTagTargetText(AprilTagParams p) =>
            p.MarkRect && p.MarkTag ? "矩形+AprilTag" : p.MarkRect ? "矩形" : "AprilTag";

        /// <summary>檢查 AprilTag 頁選擇的板可用性。回傳 -1 = 無效。</summary>
        private int GetAprilTagBoard()
        {
            if (!m_bInit) { MessageBox.Show("請先初始化！", "錯誤"); return -1; }
            int b = comboBoardAT.SelectedIndex;
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

        /// <summary>GUI：清板後建立 AprilTag 物件。</summary>
        private bool BuildAprilTagForGui(int boardIndex, AprilTagParams p)
        {
            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            if (!BuildAprilTagLayers(boardIndex, p))
            {
                MessageBox.Show("AprilTag 建立失敗！詳細原因請看 log。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnATPreview_Click(object sender, EventArgs e)
        {
            int b = GetAprilTagBoard(); if (b < 0) return;
            var p = ReadAprilTagParamsFromUI(); if (p == null) return;
            try
            {
                if (!BuildAprilTagForGui(b, p)) return;
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
                btnATPreview.Enabled = false;
                btnATMark.Enabled = false;
                txtATStatus.Text = $"晶片板 {b + 1} AprilTag #{p.TagId}（{AprilTagTargetText(p)}）紅光預覽中（15 秒後停止）";
            }
            catch (Exception ex) { MessageBox.Show($"AprilTag 預覽失敗：{ex.Message}", "錯誤"); }
        }

        private void btnATMark_Click(object sender, EventArgs e)
        {
            int b = GetAprilTagBoard(); if (b < 0) return;
            var p = ReadAprilTagParamsFromUI(); if (p == null) return;
            try
            {
                if (!BuildAprilTagForGui(b, p)) return;
                m_MMMark[b].MarkStandBy();
                if (m_MMMark[b].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {b + 1} 打標啟動失敗！", "錯誤"); return;
                }
                timerMark.Tag = b;
                timerMark.Start();
                btnATPreview.Enabled = false;
                btnATMark.Enabled = false;
                txtATStatus.Text = $"晶片板 {b + 1} AprilTag #{p.TagId}（{p.Size} mm，{AprilTagTargetText(p)}）打標中";
            }
            catch (Exception ex) { MessageBox.Show($"AprilTag 打標失敗：{ex.Message}", "錯誤"); }
        }

        private void btnATStop_Click(object sender, EventArgs e)
        {
            if (!m_bInit) return;
            try
            {
                timerMark.Stop();
                timerPreview.Stop();
                int b = comboBoardAT.SelectedIndex;
                if (b >= 0 && b < m_bBoardInit.Length && m_bBoardInit[b])
                {
                    try { m_MMMark[b].StopMarking(); } catch { }
                    try { m_MMMark[b].MarkShutdown(); } catch { }
                }
                m_bPreviewing = false;
                m_iPreviewBoard = -1;
                ResetPreviewButtonsAfterStop();
                txtATStatus.Text = "AprilTag 已停止";
            }
            catch (Exception ex) { MessageBox.Show($"停止失敗：{ex.Message}", "錯誤"); }
        }

        // ---- 圖樣預覽與尺寸資訊 ----

        private void AprilTagInputs_Changed(object sender, EventArgs e)
        {
            UpdateAprilTagInfo();
            pnlATPreview.Invalidate();
        }

        private void UpdateAprilTagInfo()
        {
            if (double.TryParse(txtATSize.Text.Trim(), out double size) && size > 0)
            {
                if (!double.TryParse(txtATRectExtra.Text.Trim(), out double extra) || extra < 0) extra = 0;
                var p = new AprilTagParams { Size = size, RectExtra = extra };
                lblATInfo.Text = $"每格 {size / AprilTag36h11.WidthAtBorder:F3} mm，含白邊 {p.TagTotalWidth:F2} mm，矩形 {p.RectWidth:F2} mm";
            }
            else
            {
                lblATInfo.Text = "請輸入有效的大小";
            }
        }

        /// <summary>預覽成品外觀（標準 AprilTag 黑白圖樣）。</summary>
        private void pnlATPreview_Paint(object sender, PaintEventArgs e)
        {
            int n = AprilTag36h11.TotalWidth;
            int cellPx = Math.Min(pnlATPreview.Width, pnlATPreview.Height) / n;
            int ox = (pnlATPreview.Width - cellPx * n) / 2;
            int oy = (pnlATPreview.Height - cellPx * n) / 2;

            bool[,] black = AprilTag36h11.GetBlackCells((int)numATId.Value);
            e.Graphics.Clear(Color.White);
            for (int r = 0; r < n; r++)
                for (int c = 0; c < n; c++)
                    if (black[r, c])
                        e.Graphics.FillRectangle(Brushes.Black, ox + c * cellPx, oy + r * cellPx, cellPx, cellPx);
            // 白邊外框輔助線
            e.Graphics.DrawRectangle(Pens.LightGray, ox, oy, cellPx * n - 1, cellPx * n - 1);
        }
    }
}
