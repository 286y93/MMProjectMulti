using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // 6-1 QR Code 頁：打黑矩形 / 打白矩形 / 單打 QR / 三合一 / 矩形獨立 / QR 獨立
    // + 鋼鐵Quest3 停止（自 Form1.cs 抽出，partial 拆檔，行為不變）。
    public partial class Form1
    {
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
    }
}
