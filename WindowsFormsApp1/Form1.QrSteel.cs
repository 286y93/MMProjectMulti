using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // 序號產生 / 參數記錄 helper + 鋼鐵 Quest3 QR 建立 / 打標（自 Form1.cs 抽出，partial 拆檔，行為不變）。
    // 註：序號 helper（NextSerial / GetQRCodeParaDir / consts）與 WriteWhiteBgParaLog 也被
    //     Form1.QrWhiteBg.cs 呼叫 — partial class 同型別，跨檔呼叫無礙。
    public partial class Form1
    {
        // === 鋼鐵 Quest3 QR 序號 & 參數記錄 =====================================
        // 序號格式 YYYYMMDD-NNN。同日 NNN 累加（001,002,003...），換日自動重置為 001。
        // 狀態存在 QRCodePara\counter.txt，內容一行 "YYYYMMDD NNN"。

        private const string QRCodeCounterFileName = "counter.txt";
        private const string WhiteBgCounterFileName = "counter_whitebg.txt";
        private const string WhiteBgFilePrefix = "QRCODE_白底";

        /// <summary>
        /// 取得下一組序號（YYYYMMDD-NNN）。counterFile 決定使用哪個計數器檔（Steel/白底 分別維護）。
        /// </summary>
        private static string NextSerial(string counterFile)
        {
            string dir = GetQRCodeParaDir();
            Directory.CreateDirectory(dir);
            string counterPath = Path.Combine(dir, counterFile);

            string today = DateTime.Now.ToString("yyyyMMdd");
            int nextSeq = 1;
            if (File.Exists(counterPath))
            {
                try
                {
                    string line = File.ReadAllText(counterPath, Encoding.UTF8).Trim();
                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && parts[0] == today && int.TryParse(parts[1], out int lastSeq))
                        nextSeq = lastSeq + 1;
                }
                catch { /* 檔案壞掉就從 001 重來 */ }
            }
            File.WriteAllText(counterPath, $"{today} {nextSeq}", Encoding.UTF8);
            return $"{today}-{nextSeq:D3}";
        }

        /// <summary>取得 QRCodePara 資料夾絕對路徑（跟 exe 同層，跨機器都能用）。</summary>
        private static string GetQRCodeParaDir()
        {
            string exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.Combine(exeDir, "QRCodePara");
        }

        /// <summary>
        /// 取得下一組鋼鐵 Quest3 QR 序號（YYYYMMDD-NNN）。
        /// </summary>
        private static string NextSteelSerial() => NextSerial(QRCodeCounterFileName);

        /// <summary>
        /// 將本次鋼鐵 Quest3 QR 使用的所有參數寫成 txt，檔名為序號.txt。
        /// action = "PREVIEW" 或 "MARK"，便於事後回溯。
        /// </summary>
        private void WriteSteelQRParaLog(string serial, string action, string content,
            int boardIndex,
            double qrWidth, double qrHeight, int border, double rectExtra,
            int ecLevel, int markStyle, double spotSize, int qrRepeat,
            double rectPower, double rectSpeed, double rectFreq, int rectRepeat,
            double qrPower, double qrSpeed, double qrFreq, double qrPulseWidth,
            double estTotalSec = 0, double estTotalLen = 0,
            double estRectSec = 0, double estQrSec = 0, double estSerialSec = 0)
        {
            try
            {
                string dir = GetQRCodeParaDir();
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, $"{serial}.txt");

                var sb = new StringBuilder();
                sb.AppendLine($"# MarkingMate 鋼鐵Quest3 QR 參數記錄");
                sb.AppendLine($"Serial       : {serial}");
                sb.AppendLine($"Action       : {action}");
                sb.AppendLine($"Timestamp    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Board        : {boardIndex + 1}");
                sb.AppendLine($"QR Content   : {content}");
                sb.AppendLine();
                sb.AppendLine("[QR 幾何]");
                sb.AppendLine($"QR Width     : {qrWidth} mm");
                sb.AppendLine($"QR Height    : {qrHeight} mm");
                sb.AppendLine($"Border       : {border} cells");
                sb.AppendLine($"Rect Extra   : {rectExtra} mm");
                sb.AppendLine();
                sb.AppendLine("[QR 屬性]");
                sb.AppendLine($"EC Level     : {ecLevel} (0=L 1=M 2=Q 3=H)");
                sb.AppendLine($"Mark Style   : {markStyle} (1=dot 2=fill)");
                sb.AppendLine($"Spot Size    : {spotSize} mm");
                sb.AppendLine($"QR Repeat    : {qrRepeat}");
                sb.AppendLine();
                sb.AppendLine("[白底矩形 雷射參數]");
                sb.AppendLine($"Rect Power   : {rectPower} %");
                sb.AppendLine($"Rect Speed   : {rectSpeed} mm/s");
                sb.AppendLine($"Rect Freq    : {rectFreq} kHz");
                sb.AppendLine($"Rect Repeat  : {rectRepeat}");
                sb.AppendLine();
                sb.AppendLine("[QR 雷射參數]");
                sb.AppendLine($"QR Power     : {qrPower} %");
                sb.AppendLine($"QR Speed     : {qrSpeed} mm/s");
                sb.AppendLine($"QR Freq      : {qrFreq} kHz");
                sb.AppendLine($"QR PulseWidth: {qrPulseWidth}");
                sb.AppendLine();
                sb.AppendLine("[工作區]");
                sb.AppendLine($"Workspace W  : {m_WorkspaceSize} mm");
                sb.AppendLine($"Workspace H  : {m_WorkspaceHeight} mm");
                sb.AppendLine();
                sb.AppendLine("[SDK 預估打標時間 & 路徑長度]");
                sb.AppendLine($"Total Time   : {estTotalSec:F3} sec");
                sb.AppendLine($"Total Length : {estTotalLen:F3} mm");
                sb.AppendLine($"  ├ Rect Time  : {estRectSec:F3} sec");
                sb.AppendLine($"  ├ QR Time    : {estQrSec:F3} sec");
                sb.AppendLine($"  └ Serial Time: {estSerialSec:F3} sec");

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel QR para log → {path}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WriteSteelQRParaLog failed: {ex.Message}");
            }
        }

        /// <summary>
        /// 將 QRCODE_白底 雙圖層打標的參數 + 預估時間寫入 QRCODE_白底&lt;serial&gt;.txt。
        /// </summary>
        private void WriteWhiteBgParaLog(string serial, int boardIndex,
            bool markQR, bool markRect,
            double qrWidth, double qrHeight, double quietZone, double rectExtra,
            double qrSpeed, double qrPower,
            double rectSpeed, double rectPower,
            double estTotalSec, double estTotalLen,
            double estRectSec, double estQrSec, double estSerialSec = 0)
        {
            try
            {
                string dir = GetQRCodeParaDir();
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, $"{WhiteBgFilePrefix}{serial}.txt");

                var sb = new StringBuilder();
                sb.AppendLine("# MarkingMate QRCODE_白底 參數記錄");
                sb.AppendLine($"Serial       : {serial}");
                sb.AppendLine($"Timestamp    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Board        : {boardIndex + 1}");
                sb.AppendLine($"Mark Target  : QR={markQR} Rect={markRect}");
                sb.AppendLine();
                sb.AppendLine("[QR 幾何]");
                sb.AppendLine($"QR Width     : {qrWidth} mm");
                sb.AppendLine($"QR Height    : {qrHeight} mm");
                sb.AppendLine($"Border/QuietZone : {quietZone} units");
                sb.AppendLine($"Rect Extra X : {rectExtra} mm");
                sb.AppendLine();
                sb.AppendLine("[QR 雷射參數]");
                sb.AppendLine($"QR Speed     : {qrSpeed} mm/s");
                sb.AppendLine($"QR Power     : {qrPower} %");
                sb.AppendLine();
                sb.AppendLine("[矩形雷射參數]");
                sb.AppendLine($"Rect Speed   : {rectSpeed} mm/s");
                sb.AppendLine($"Rect Power   : {rectPower} %");
                sb.AppendLine();
                sb.AppendLine("[工作區]");
                sb.AppendLine($"Workspace W  : {m_WorkspaceSize} mm");
                sb.AppendLine($"Workspace H  : {m_WorkspaceHeight} mm");
                sb.AppendLine();
                sb.AppendLine("[SDK 預估打標時間 & 路徑長度]");
                sb.AppendLine($"Total Time   : {estTotalSec:F3} sec");
                sb.AppendLine($"Total Length : {estTotalLen:F3} mm");
                sb.AppendLine($"  ├ Rect Time  : {estRectSec:F3} sec");
                sb.AppendLine($"  ├ QR Time    : {estQrSec:F3} sec");
                sb.AppendLine($"  └ Serial Time: {estSerialSec:F3} sec");

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WriteWhiteBgParaLog → {path}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] WriteWhiteBgParaLog failed: {ex.Message}");
            }
        }

        /// <summary>
        /// 依 groupBoxQRSteel 內 TextBox 的參數，在指定板建立「白底矩形 + 反相 QR」兩個物件。
        /// 不呼叫 StartMarking — 交給呼叫端決定要打標（mode 4）或紅光預覽（mode 3）。
        /// 內容取自 txtQRContent，若空則用「STEEL」佔位。
        /// 白底策略：低功率高速多次疊層 → 表面退火霧化（灰白）
        /// QR 策略：高功率低速長脈衝 + dot mode + repeat → 深黑碳化點
        /// 回傳 true=兩個物件都建立成功；失敗會自行 MessageBox 提示。
        /// </summary>
        private bool BuildSteelQRLayers(int boardIndex, string action, out string serial)
        {
            serial = null;
            // 從 TextBox 讀取參數（解析失敗回落至鋼鐵建議預設）
            if (!double.TryParse(txtSteelQrWidth.Text.Trim(), out double qrWidth) || qrWidth <= 0) qrWidth = 20;
            if (!double.TryParse(txtSteelQrHeight.Text.Trim(), out double qrHeight) || qrHeight <= 0) qrHeight = 20;
            if (!int.TryParse(txtSteelBorder.Text.Trim(), out int border) || border < 0) border = 4;
            if (!double.TryParse(txtSteelRectExtra.Text.Trim(), out double rectExtra)) rectExtra = 0;
            if (!int.TryParse(txtSteelECLevel.Text.Trim(), out int ecLevel) || ecLevel < 0 || ecLevel > 3) ecLevel = 1;
            if (!int.TryParse(txtSteelMarkStyle.Text.Trim(), out int markStyle)) markStyle = 1;
            if (!double.TryParse(txtSteelSpotSize.Text.Trim(), out double spotSize) || spotSize <= 0) spotSize = 0.05;
            if (!int.TryParse(txtSteelQrRepeat.Text.Trim(), out int qrRepeat) || qrRepeat < 1) qrRepeat = 2;
            if (!double.TryParse(txtSteelRectPower.Text.Trim(), out double rectPower)) rectPower = 45;
            if (!double.TryParse(txtSteelRectSpeed.Text.Trim(), out double rectSpeed) || rectSpeed <= 0) rectSpeed = 3000;
            if (!double.TryParse(txtSteelRectFreq.Text.Trim(), out double rectFreq) || rectFreq <= 0) rectFreq = 80;
            if (!int.TryParse(txtSteelRectRepeat.Text.Trim(), out int rectRepeat) || rectRepeat < 1) rectRepeat = 2;
            if (!double.TryParse(txtSteelQrPower.Text.Trim(), out double qrPower)) qrPower = 85;
            if (!double.TryParse(txtSteelQrSpeed.Text.Trim(), out double qrSpeed) || qrSpeed <= 0) qrSpeed = 500;
            if (!double.TryParse(txtSteelQrFreq.Text.Trim(), out double qrFreq) || qrFreq <= 0) qrFreq = 25;
            if (!double.TryParse(txtSteelQrPulseWidth.Text.Trim(), out double qrPulseWidth) || qrPulseWidth <= 0) qrPulseWidth = 400;

            // QR 內容：與其他 QR 按鈕共用 txtQRContent
            string content = txtQRContent.Text?.Trim();
            if (string.IsNullOrEmpty(content)) content = "STEEL";

            // 取得本次序號（YYYYMMDD-NNN，同日累加）
            serial = NextSteelSerial();
            if (txtSteelSerial != null) txtSteelSerial.Text = serial;

            string qrName = "QRSteel_QR";
            string rectName = "QRSteel_Rect";
            string serialName = "QRSteel_Serial";

            // 0) 清板 + 重設工作區
            m_MMMark[boardIndex].ResetFile();
            m_MMMark[boardIndex].SetDesktopCenter(0, 0);
            m_MMMark[boardIndex].SetDesktopSize(m_WorkspaceSize, m_WorkspaceHeight);
            Application.DoEvents();
            Thread.Sleep(100);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);

            // === 為了決定 QR 實際渲染尺寸，先建立一個「暫存 QR」查詢 Version → 算完 cellSize 後刪除 → 依正確順序重建 ===

            // Step A) 建立暫存 QR，查 Version + 算 cellSize
            string tempQrName = "QRSteel_TempQR";
            long rTemp = m_MMMark[boardIndex].AddBarcode(
                BARCODE_TYPE_QRCODE, content, 0, 0, qrWidth, qrHeight, "", tempQrName);
            if (rTemp != 0)
            {
                MessageBox.Show($"建立暫存 QR 失敗！回傳碼: {rTemp}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(tempQrName, ecLevel);
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(tempQrName, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(tempQrName, 1, 1);
            ApplyQRBorder(boardIndex, tempQrName, border);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(300);

            long qrVersion = m_MMEdit[boardIndex].Get2DBarcodeQRVersion(tempQrName);
            if (qrVersion < 1) qrVersion = 1;
            int modules = 17 + 4 * (int)qrVersion;
            double cellW = qrWidth / modules;
            double cellH = qrHeight / modules;

            // 讀取暫存 QR 實際渲染尺寸（含 border）→ 給矩形用
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(tempQrName, cellW, cellH);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);
            double actualW = m_MMEdit[boardIndex].GetWidth(tempQrName);
            double actualH = m_MMEdit[boardIndex].GetHeight(tempQrName);
            double rectBaseW = actualW > 0 ? actualW : qrWidth;
            double rectBaseH = actualH > 0 ? actualH : qrHeight;
            Console.Error.WriteLine(
                $"[Board {boardIndex + 1}] Steel probe QR ver={qrVersion} modules={modules} EC={ecLevel} " +
                $"Border={border} cell={cellW:F4}x{cellH:F4} rendered={actualW:F2}x{actualH:F2}");

            // 刪除暫存 QR
            try { m_MMEdit[boardIndex].DeleteObject("", tempQrName); } catch { /* 忽略 */ }
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);

            // Step B) 依「打標順序」重建 3 個物件：矩形 → QR → 文字
            //   AddXxx 順序 = SDK 打標順序，不依賴 ChangeObjectOrder。

            // 1) AddRect（Layer 1，最先打）
            double rectW = rectBaseW + rectExtra;
            double rectH = rectBaseH + rectExtra;
            double rectHalfW = rectW / 2.0, rectHalfH = rectH / 2.0;
            long rR = m_MMEdit[boardIndex].AddRect(-rectHalfW, -rectHalfH, rectHalfW, rectHalfH, 0, "", rectName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel AddRect rc={rR} size={rectW}x{rectH}");
            if (rR != 0)
            {
                MessageBox.Show($"建立矩形失敗！回傳碼: {rR}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Application.DoEvents();
            Thread.Sleep(200);

            m_MMEdit[boardIndex].SetFillStyle(rectName, 1);
            m_MMEdit[boardIndex].SetFrameLineType(rectName, 1);
            m_MMEdit[boardIndex].SetFillRoundPitch(rectName, 0.05);   // 0.05 = 白度與時間折衷
            m_MMEdit[boardIndex].SetFillPitch(rectName, 0.05);        // 同上
            m_MMEdit[boardIndex].SetFillTimes(rectName, 1);
            m_MMEdit[boardIndex].SetFillAverageDistribution(rectName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(rectName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(rectName, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(rectName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(rectName, rectSpeed);
            m_MMMark[boardIndex].SetPower(rectName, rectPower);
            m_MMMark[boardIndex].SetFrequency(rectName, rectFreq);
            m_MMMark[boardIndex].SetMarkRepeat(rectName, rectRepeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(rectName, 100);
            m_MMMark[boardIndex].SetPulseWidth(rectName, 200);

            // 2) AddBarcode QR（Layer 2，中間打）
            long rQR = m_MMMark[boardIndex].AddBarcode(
                BARCODE_TYPE_QRCODE, content, 0, 0, qrWidth, qrHeight, "", qrName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel AddBarcode rc={rQR} content=\"{content}\"");
            if (rQR != 0)
            {
                MessageBox.Show($"建立 QR Code 失敗！回傳碼: {rQR}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Application.DoEvents();
            Thread.Sleep(200);

            m_MMEdit[boardIndex].SetBarcodeInvert(qrName, 1);
            m_MMEdit[boardIndex].Set2DBarcodeQRECLevel(qrName, ecLevel);
            m_MMEdit[boardIndex].Set2DBarcodeFixedType(qrName, 0);
            m_MMEdit[boardIndex].Set2DBarcodeFixedCellSize(qrName, cellW, cellH);
            ApplyQRBorder(boardIndex, qrName, border);
            m_MMEdit[boardIndex].SetBarcodeMarkStyle(qrName, markStyle);
            m_MMEdit[boardIndex].SetBarcodeLineType(qrName, 0);
            m_MMEdit[boardIndex].SetBarcodeSpotSize(qrName, spotSize);
            m_MMEdit[boardIndex].SetBarcodeLineTimes(qrName, 1);
            m_MMEdit[boardIndex].SetFillStartAngle(qrName, 0);
            m_MMEdit[boardIndex].SetFillStepAngle(qrName, 90);
            m_MMEdit[boardIndex].SetBarcodeLineTwoway(qrName, 1);
            m_MMEdit[boardIndex].SetFrameSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillSwitch(qrName, 1);
            m_MMEdit[boardIndex].SetFillFirstExt(qrName, 0, 1);
            m_MMMark[boardIndex].SetSpeed(qrName, qrSpeed);
            m_MMMark[boardIndex].SetPower(qrName, qrPower);
            m_MMMark[boardIndex].SetFrequency(qrName, qrFreq);
            m_MMMark[boardIndex].SetMarkRepeat(qrName, qrRepeat);
            m_MMEdit[boardIndex].SetBarcodeSpotDelay(qrName, 1000);
            m_MMMark[boardIndex].SetPulseWidth(qrName, qrPulseWidth);
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(200);

            // 6) 序號文字物件：放在 QR 左上角外側上方（不會擋到資料區）
            //    字高採 QR 高的 1/8，最小 1.5 mm；y 座標在矩形上方 1 mm。
            // 序號文字位置：改用 border × cellW 顯式計算 border 白邊寬度（不依賴 GetWidth 是否含 border）
            //   border 最高 y = QR 模組頂邊 + border 白邊寬度 = qrHeight/2 + border*cellW
            //   文字最下緣 > 該 y，因此把文字中心 Y 設在 (border最高y + margin + fontSize/2)
            double qrModuleTopY = qrHeight / 2.0;
            double borderZoneWidth = border * cellW;                   // 顯式：border 白邊寬度 (mm)
            double borderTopY = qrModuleTopY + borderZoneWidth;        // border 最高 y
            double serialFontSize = Math.Max(qrHeight / 8.0, 1.5);
            double serialCx = -qrWidth / 2.0;                          // 左對齊到 QR 模組左邊
            double serialCy;
            if (border > 0 && borderZoneWidth > 0)
            {
                // 文字最下緣 = serialCy - fontSize/2 > borderTopY
                const double marginAboveBorder = 0.5;    // 0.5 mm 安全間距
                serialCy = borderTopY + serialFontSize / 2.0 + marginAboveBorder;
            }
            else
            {
                // border = 0 fallback：QR 模組頂邊 + 半字高 + 1mm 間距
                serialCy = qrModuleTopY + serialFontSize / 2.0 + 1.0;
            }
            Console.Error.WriteLine(
                $"[Board {boardIndex + 1}] Steel Text pos: border={border} cellW={cellW:F4} " +
                $"borderZone={borderZoneWidth:F3}mm borderTopY={borderTopY:F3} → serialCy={serialCy:F3}");
            long rT = m_MMMark[boardIndex].AddText(serial, serialCx, serialCy, "", serialName);
            Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel AddText serial=\"{serial}\" rc={rT} @({serialCx:F2},{serialCy:F2}) h={serialFontSize:F2}");
            if (rT == 0)
            {
                try
                {
                    m_MMEdit[boardIndex].SetFontSize(serialName, serialFontSize);
                    m_MMMark[boardIndex].SetSpeed(serialName, qrSpeed);
                    m_MMMark[boardIndex].SetPower(serialName, qrPower);
                    m_MMMark[boardIndex].SetFrequency(serialName, qrFreq);
                    m_MMMark[boardIndex].SetMarkRepeat(serialName, 2);
                    m_MMMark[boardIndex].SetPulseWidth(serialName, qrPulseWidth);
                }
                catch (Exception exSerial)
                {
                    Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel serial text 屬性設定失敗（略過）: {exSerial.Message}");
                }
            }

            // 7) 圖層順序由 AddXxx 呼叫順序保證：矩形（先） → QR（中） → 序號文字（後）
            //    不再依賴 ChangeObjectOrder。

            // 8) 最終 Redraw（呼叫端決定要 StartMarking(4) 打標或 StartMarking(3) 紅光預覽）
            m_MMMark[boardIndex].Redraw();
            Application.DoEvents();
            Thread.Sleep(300);

            // 8.5) 估算打標時間（SDK GetEstimatedTotalTime + 各物件 GetSKWObjTime）
            double estTotalSec = 0;
            double estRectSec = 0, estQrSec = 0, estSerialSec = 0, estTotalLen = 0;
            try
            {
                estTotalSec = m_MMMark[boardIndex].GetEstimatedTotalTime();
                estTotalLen = m_MMMark[boardIndex].GetEstimatedTotalLength();
                estRectSec = m_MMMark[boardIndex].GetSKWObjTime(rectName);
                estQrSec = m_MMMark[boardIndex].GetSKWObjTime(qrName);
                if (rT == 0)
                    estSerialSec = m_MMMark[boardIndex].GetSKWObjTime(serialName);
                Console.Error.WriteLine(
                    $"[Board {boardIndex + 1}] Steel time est: total={estTotalSec:F2}s len={estTotalLen:F2}mm | " +
                    $"rect={estRectSec:F2}s qr={estQrSec:F2}s serial={estSerialSec:F2}s");
            }
            catch (Exception exTime)
            {
                Console.Error.WriteLine($"[Board {boardIndex + 1}] Steel time est 失敗: {exTime.Message}");
            }

            // 序號欄還原為只顯示序號
            if (txtSteelSerial != null)
            {
                txtSteelSerial.Text = serial;
            }

            // 預估時間 GroupBox 內的 TextBox 更新
            if (txtSteelTimeInfo != null)
            {
                var timeSb = new StringBuilder();
                timeSb.AppendLine($"Serial       : {serial}");
                timeSb.AppendLine($"Action       : {action}");
                timeSb.AppendLine($"Total Time   : {estTotalSec:F2} 秒");
                timeSb.AppendLine($"Total Length : {estTotalLen:F2} mm");
                timeSb.AppendLine($"──────────────────────────");
                timeSb.AppendLine($"矩形 (Rect)  : {estRectSec:F2} 秒");
                timeSb.AppendLine($"QR Code      : {estQrSec:F2} 秒");
                timeSb.AppendLine($"序號文字     : {estSerialSec:F2} 秒");
                txtSteelTimeInfo.Text = timeSb.ToString();
            }

            // 9) 寫參數 log（PREVIEW / MARK 兩種呼叫都會落檔，方便對照）
            WriteSteelQRParaLog(serial, action, content, boardIndex,
                qrWidth, qrHeight, border, rectExtra,
                ecLevel, markStyle, spotSize, qrRepeat,
                rectPower, rectSpeed, rectFreq, rectRepeat,
                qrPower, qrSpeed, qrFreq, qrPulseWidth,
                estTotalSec, estTotalLen, estRectSec, estQrSec, estSerialSec);

            return true;
        }

        /// <summary>
        /// 鋼鐵 + Quest 3 QR 專用打標：建立雙圖層後直接以 mode 4 打標。
        /// </summary>
        private void btnQRSteelMark_Click(object sender, EventArgs e)
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

            try
            {
                if (!BuildSteelQRLayers(boardIndex, "MARK", out string serial)) return;

                m_MMMark[boardIndex].MarkStandBy();
                if (m_MMMark[boardIndex].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {boardIndex + 1} 打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 啟動 Timer 監控 + 停用相關按鈕
                timerMark.Tag = boardIndex;
                timerMark.Start();

                btnQRSteelMark.Enabled = false;
                btnQRSteelPreview.Enabled = false;
                btnQRWhiteBgMark.Enabled = false;
                btnMarkQR.Enabled = false;
                btnLoadQR.Enabled = false;
                btnPreviewQR.Enabled = false;
                btnClearQR.Enabled = false;
                btnStopMarkQR.Enabled = true;
                btnMarkDXF.Enabled = false;
                btnMark.Enabled = false;
                btnStop.Enabled = true;

                string ct = string.IsNullOrEmpty(txtQRContent.Text?.Trim()) ? "STEEL" : txtQRContent.Text.Trim();
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 鋼鐵Quest3 QR 打標中（{serial} / {ct}）";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"鋼鐵 QR 打標失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 鋼鐵 + Quest 3 QR 紅光預覽：與打標按鈕使用相同參數建立物件，
        /// 但改為紅光全路徑預覽（SetPreviewMode(2) + StartMarking(3)），15 秒自動停止。
        /// 讓使用者實際畫在晶片板上校準位置、確認 QR 尺寸與白底範圍。
        /// </summary>
        private void btnQRSteelPreview_Click(object sender, EventArgs e)
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

            try
            {
                if (!BuildSteelQRLayers(boardIndex, "PREVIEW", out string serial)) return;

                // 紅光全路徑預覽
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

                // 15 秒自動停止
                timerPreview.Stop();
                timerPreview.Start();

                // 停用相關按鈕
                btnQRSteelMark.Enabled = false;
                btnQRSteelPreview.Enabled = false;
                btnQRWhiteBgMark.Enabled = false;
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

                string ct = string.IsNullOrEmpty(txtQRContent.Text?.Trim()) ? "STEEL" : txtQRContent.Text.Trim();
                txtQRStatus.Text = $"晶片板 {boardIndex + 1} 鋼鐵Quest3 QR 紅光預覽中（{serial} / {ct}，15秒後停止）";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"鋼鐵 QR 預覽失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
