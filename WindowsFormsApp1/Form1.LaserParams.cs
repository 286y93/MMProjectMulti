using System;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // 雷射功率頁籤 UI：功率滑桿 / 擺動勾選 / 套用 / 讀取（自 Form1.cs 抽出，partial 拆檔，行為不變）。
    // 註：auto 模式的 ApplyLaserParamsAuto（CLI / daemon / 命令頁籤共用）仍保留在 Form1.cs。
    public partial class Form1
    {
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
    }
}
