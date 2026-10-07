using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // 8. AprilTag 頁：依 tag ID 與大小產生 tag36h11 圖樣（黑格 → 填滿矩形），紅光預覽 / 雷射打標。
    // 雷射參數（功率 / 速度 / 頻率 / 脈寬 / 次數）沿用「5. 雷射功率」頁。
    public partial class Form1
    {
        private const string AT_NAME_PREFIX = "AT_";

        // 填滿參數（與 6-1 黑矩形同一套做法：外框 + 填滿）
        private const int AT_FILL_STYLE = 1;
        private const double AT_FILL_PITCH = 0.04;
        private const double AT_FILL_ROUND_PITCH = 0.04;
        private const int AT_FILL_TIMES = 2;
        private const double AT_FILL_STEP_ANGLE = 90;

        /// <summary>讀取並驗證 tag 大小（黑框外緣邊長 mm）。失敗回 false 並提示。</summary>
        private bool TryGetAprilTagSize(out double size)
        {
            if (!double.TryParse(txtATSize.Text.Trim(), out size) || size <= 0)
            {
                MessageBox.Show("請輸入有效的大小（mm，需大於 0）！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // 含白邊的總寬 = 大小 × 10/8，必須放得進工作區
            double total = size * AprilTag36h11.TotalWidth / AprilTag36h11.WidthAtBorder;
            double minWorkspace = Math.Min(m_WorkspaceSize, m_WorkspaceHeight);
            if (total > minWorkspace)
            {
                MessageBox.Show($"含白邊總寬 {total:F2} mm 超過工作區 {minWorkspace} mm，請縮小大小。",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 建立 AprilTag 物件（不 StartMarking）。tag 中心在 (0,0)。
        /// 座標：Y 軸向上為正，圖樣第 0 列（最上排）對應 Y 最大值。
        /// </summary>
        private bool BuildAprilTag(int boardIndex, int tagId, double size)
        {
            double cell = size / AprilTag36h11.WidthAtBorder;
            double half = AprilTag36h11.TotalWidth / 2.0;   // 以格為單位的半寬（5 格）

            bool[,] black = AprilTag36h11.GetBlackCells(tagId);
            var rects = AprilTag36h11.MergeBlackRects(black);

            ReadWorkspaceSettings();
            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents(); Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            for (int i = 0; i < rects.Count; i++)
            {
                var r = rects[i];
                string name = AT_NAME_PREFIX + i;
                double left = (r.Col - half) * cell;
                double right = (r.Col + r.Cols - half) * cell;
                double top = (half - r.Row) * cell;
                double bottom = (half - r.Row - r.Rows) * cell;

                long rc = m_MMEdit[boardIndex].AddRect(left, bottom, right, top, 0, "", name);
                if (rc != 0)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] AprilTag AddRect #{i} rc={rc}");
                    MessageBox.Show($"AprilTag 建立失敗（第 {i + 1} 個矩形）rc={rc}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                m_MMEdit[boardIndex].SetFillStyle(name, AT_FILL_STYLE);
                m_MMEdit[boardIndex].SetFillPitch(name, AT_FILL_PITCH);
                m_MMEdit[boardIndex].SetFillRoundPitch(name, AT_FILL_ROUND_PITCH);
                m_MMEdit[boardIndex].SetFillTimes(name, AT_FILL_TIMES);
                m_MMEdit[boardIndex].SetFillStepAngle(name, AT_FILL_STEP_ANGLE);
                m_MMEdit[boardIndex].SetFillAverageDistribution(name, 1);
                m_MMEdit[boardIndex].SetFrameSwitch(name, 1);        // 外框，讓格子邊緣銳利
                m_MMEdit[boardIndex].SetFillSwitch(name, 1);
                m_MMEdit[boardIndex].SetFillFirstExt(name, 0, 1);    // 填滿優先
            }
            Console.Error.WriteLine($"[Board {boardIndex + 1}] AprilTag 36h11 id={tagId} size={size}mm cell={cell:F4}mm rects={rects.Count}");

            m_MMMark[boardIndex].Redraw();
            Application.DoEvents(); Thread.Sleep(200);

            // 雷射參數沿用「5. 雷射功率」頁
            return ApplyLaserParamsFromUI(boardIndex);
        }

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

        private void btnATPreview_Click(object sender, EventArgs e)
        {
            int b = GetAprilTagBoard(); if (b < 0) return;
            if (!TryGetAprilTagSize(out double size)) return;
            int id = (int)numATId.Value;
            try
            {
                if (!BuildAprilTag(b, id, size)) return;
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
                txtATStatus.Text = $"晶片板 {b + 1} AprilTag #{id} 紅光預覽中（15 秒後停止）";
            }
            catch (Exception ex) { MessageBox.Show($"AprilTag 預覽失敗：{ex.Message}", "錯誤"); }
        }

        private void btnATMark_Click(object sender, EventArgs e)
        {
            int b = GetAprilTagBoard(); if (b < 0) return;
            if (!TryGetAprilTagSize(out double size)) return;
            int id = (int)numATId.Value;
            try
            {
                if (!BuildAprilTag(b, id, size)) return;
                m_MMMark[b].MarkStandBy();
                if (m_MMMark[b].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {b + 1} 打標啟動失敗！", "錯誤"); return;
                }
                timerMark.Tag = b;
                timerMark.Start();
                btnATPreview.Enabled = false;
                btnATMark.Enabled = false;
                txtATStatus.Text = $"晶片板 {b + 1} AprilTag #{id}（{size} mm）打標中";
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
                double cell = size / AprilTag36h11.WidthAtBorder;
                double total = size * AprilTag36h11.TotalWidth / AprilTag36h11.WidthAtBorder;
                lblATInfo.Text = $"每格 {cell:F3} mm，含白邊總寬 {total:F2} mm";
            }
            else
            {
                lblATInfo.Text = "請輸入有效的大小";
            }
        }

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
