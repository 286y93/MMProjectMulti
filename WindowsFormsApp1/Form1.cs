using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxMMMarkx641Lib;
using AxMMEditx641Lib;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // 四個晶片板的多系統控件陣列
        private AxMMMarkx641[] m_MMMark = new AxMMMarkx641[4];
        private AxMMEditx641[] m_MMEdit = new AxMMEditx641[4];

        private Panel[] m_Panels;
        private bool m_bInit = false;
        private int m_iCurrentBoard = 0;

        // 每個晶片板的配置路徑
        // 注意：MarkingMate MultiMM SDK 預設支援 MM1 和 MM2
        // 若需要 MM3、MM4，需在 MarkingMate 中建立對應的配置
        private string[] m_ConfigPaths = new string[]
        {
            "/cfg_config_MM1",
            "/cfg_config_MM2",
            "/cfg_config_MM3",
            "/cfg_config_MM4"
        };

        // 記錄每個板是否成功初始化
        private bool[] m_bBoardInit = new bool[4];

        // EMC6 IP 設定檔路徑
        private string[] m_IPConfigPaths = new string[]
        {
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM1\DevIPAddress.ini",
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM2\DevIPAddress.ini",
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM3\DevIPAddress.ini",
            @"C:\Program Files (x86)\MarkingMate\Drivers\EMC6_MM4\DevIPAddress.ini"
        };

        private TextBox[] m_txtIPs;

        // 自動模式相關
        private CommandLineArgs m_AutoModeArgs = null;
        private bool m_IsAutoMode = false;
        public int ExitCode { get; private set; } = 0;

        // GUI 模式建構子
        public Form1()
        {
            InitializeComponent();

            m_Panels = new Panel[] { panelBoard1, panelBoard2, panelBoard3, panelBoard4 };
            m_txtIPs = new TextBox[] { txtIP1, txtIP2, txtIP3, txtIP4 };
            comboBoard.SelectedIndex = 0;
            comboBoardDXF.SelectedIndex = 0;
            m_IsAutoMode = false;
        }

        // 自動模式建構子
        public Form1(CommandLineArgs args) : this()
        {
            m_AutoModeArgs = args;
            m_IsAutoMode = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadIPSettings();

            // 如果是自動模式，啟動自動執行
            if (m_IsAutoMode && m_AutoModeArgs != null)
            {
                // 使用 Timer 延遲執行，確保 Form 完全載入
                System.Windows.Forms.Timer startTimer = new System.Windows.Forms.Timer();
                startTimer.Interval = 100;
                startTimer.Tick += (s, ev) =>
                {
                    startTimer.Stop();
                    ExecuteAutoMode();
                };
                startTimer.Start();
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if (m_bInit)
            {
                MessageBox.Show("已經初始化過了！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 重要：清理可能存在的舊控件（防止殘留）
            CleanupOldControls();

            int successCount = 0;
            string failInfo = "";
            int boardCount = (int)numBoardCount.Value;

            // 重要：MultiMM SDK 要求逐個板建立、初始化，而非批次處理
            // 必須：建立板 0 → 初始化板 0 → 建立板 1 → 初始化板 1 → ...

            for (int i = 0; i < boardCount; i++)
            {
                try
                {
                    // 步驟 1：建立 MMMark 控件
                    m_MMMark[i] = new AxMMMarkx641();
                    m_MMMark[i].Left = 0;
                    m_MMMark[i].Top = 0;
                    m_MMMark[i].Width = m_Panels[i].Width;
                    m_MMMark[i].Height = m_Panels[i].Height;
                    m_Panels[i].Controls.Add(m_MMMark[i]);

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);

                    // 步驟 2：立即初始化 MMMark
                    m_MMMark[i].InitialExt(m_ConfigPaths[i]);
                    m_MMMark[i].SetDesktopCenter(0, 0);
                    m_MMMark[i].SetDesktopSize(100, 100);
                    m_MMMark[i].SetActiveDB(0);
                    m_MMMark[i].MarkStandBy();
                    m_MMMark[i].SetCurEditFun(2);
                    m_MMMark[i].Redraw();

                    m_bBoardInit[i] = true;

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(200);

                    // 步驟 3：建立並初始化 MMEdit
                    m_MMEdit[i] = new AxMMEditx641();
                    this.Controls.Add(m_MMEdit[i]);
                    m_MMEdit[i].Visible = false;

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);

                    m_MMEdit[i].InitialExt(m_ConfigPaths[i]);

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);

                    successCount++;

                    System.Diagnostics.Debug.WriteLine($"晶片板 {i + 1} 初始化成功");
                }
                catch (Exception ex)
                {
                    m_bBoardInit[i] = false;
                    failInfo += $"晶片板 {i + 1}：{ex.Message}\n";

                    System.Diagnostics.Debug.WriteLine($"晶片板 {i + 1} 初始化失敗：{ex.Message}");

                    // 如果第一個板失敗，後續的板也會失敗，直接中斷
                    if (i == 0)
                    {
                        MessageBox.Show($"第一個晶片板初始化失敗，無法繼續！\n\n錯誤：{ex.Message}\n\n" +
                            "提示：\n" +
                            "1. 確認 MarkingMate SDK 已安裝\n" +
                            "2. 確認硬體已連接\n" +
                            "3. 嘗試將板數量設為 1\n" +
                            "4. 先用 MarkingMate 軟體測試連接",
                            "初始化失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 如果不是第一個板失敗，記錄錯誤但繼續
                    // 這樣至少可以使用已成功的板
                }
            }

            m_bInit = successCount > 0;

            if (successCount == boardCount)
            {
                MessageBox.Show($"{successCount} 個晶片板全部初始化完成！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (successCount > 0)
            {
                MessageBox.Show($"已成功初始化 {successCount}/{boardCount} 個晶片板。\n\n以下晶片板初始化失敗：\n{failInfo}",
                    "部分初始化", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show($"所有晶片板初始化失敗！\n\n{failInfo}", "初始化失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_iCurrentBoard = comboBoard.SelectedIndex;

                if (!m_bBoardInit[m_iCurrentBoard])
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double x1 = double.Parse(txtX1.Text);
                double y1 = double.Parse(txtY1.Text);
                double x2 = double.Parse(txtX2.Text);
                double y2 = double.Parse(txtY2.Text);

                // 使用多系統 MMEdit 的 AddLine 方法
                int result = m_MMEdit[m_iCurrentBoard].AddLine(x1, y1, x2, y2, "", "");

                if (result == 0)
                {
                    m_MMMark[m_iCurrentBoard].Redraw();
                    MessageBox.Show($"已在晶片板 {m_iCurrentBoard + 1} 繪製線段！\n起點({x1}, {y1}) -> 終點({x2}, {y2})",
                        "繪製成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"繪製線段失敗！錯誤碼：{result}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"繪製線段失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_iCurrentBoard = comboBoard.SelectedIndex;

                if (!m_bBoardInit[m_iCurrentBoard])
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_MMMark[m_iCurrentBoard].MarkStandBy();

                if (m_MMMark[m_iCurrentBoard].StartMarking(4) != 0)
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 打標啟動失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 啟動 Timer 來監控打標狀態
                timerMark.Tag = m_iCurrentBoard;
                timerMark.Start();

                btnMark.Enabled = false;
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                m_iCurrentBoard = comboBoard.SelectedIndex;

                if (!m_bBoardInit[m_iCurrentBoard])
                {
                    MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 未成功初始化，無法操作！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_MMMark[m_iCurrentBoard].StopMarking();
                timerMark.Stop();

                btnMark.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerMark_Tick(object sender, EventArgs e)
        {
            int boardIndex = (int)timerMark.Tag;

            if (m_MMMark[boardIndex].IsMarking() == 0)
            {
                timerMark.Stop();
                m_MMMark[boardIndex].MarkShutdown();

                btnMark.Enabled = true;
                btnMarkDXF.Enabled = true;
                btnLoadDXF.Enabled = true;
                btnLoadDXFFile.Enabled = true;
                btnStop.Enabled = false;

                MessageBox.Show($"晶片板 {boardIndex + 1} 打標完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                MessageBox.Show("請先初始化！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string statusInfo = "";

            for (int i = 0; i < 4; i++)
            {
                string ip = m_txtIPs[i].Text.Trim();
                string ipStr = string.IsNullOrEmpty(ip) ? "未設定" : ip;

                if (!m_bBoardInit[i])
                {
                    statusInfo += $"晶片板 {i + 1} (IP: {ipStr})：未初始化\n";
                    continue;
                }

                try
                {
                    long cardConnect = m_MMMark[i].IsCardConnect();
                    long headStatus = m_MMMark[i].GetHeadStatus(0);

                    string connectStr = cardConnect != 0 ? "已連接" : "未連接";
                    string headStr = headStatus != 0 ? "正常" : "異常";

                    statusInfo += $"晶片板 {i + 1} (IP: {ipStr})：控制卡 {connectStr}，掃描頭 {headStr}\n";
                }
                catch (Exception ex)
                {
                    statusInfo += $"晶片板 {i + 1} (IP: {ipStr})：查詢失敗 - {ex.Message}\n";
                }
            }

            MessageBox.Show(statusInfo, "連接狀態", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadIPSettings()
        {
            for (int i = 0; i < 4; i++)
            {
                m_txtIPs[i].Text = ReadIPFromIni(m_IPConfigPaths[i]);
            }
        }

        private string ReadIPFromIni(string iniPath)
        {
            if (!File.Exists(iniPath))
                return "";

            foreach (string line in File.ReadAllLines(iniPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("DEV0=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed.Substring(5).Trim();
                }
            }
            return "";
        }

        private void btnReadIP_Click(object sender, EventArgs e)
        {
            LoadIPSettings();
            MessageBox.Show("已從 DevIPAddress.ini 讀取 IP 設定。", "讀取IP", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSaveIP_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            string errorInfo = "";

            for (int i = 0; i < 4; i++)
            {
                string ip = m_txtIPs[i].Text.Trim();
                try
                {
                    SaveIPToIni(m_IPConfigPaths[i], ip);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errorInfo += $"MM{i + 1}：{ex.Message}\n";
                }
            }

            if (successCount == 4)
            {
                MessageBox.Show("四組 IP 設定已儲存！\n\n注意：IP 變更需重新啟動程式並重新初始化才會生效。",
                    "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"已儲存 {successCount}/4 組。\n\n儲存失敗：\n{errorInfo}\n" +
                    "提示：可能需要以管理員身分執行程式才能寫入 Program Files 目錄。",
                    "儲存IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveIPToIni(string iniPath, string ip)
        {
            if (!File.Exists(iniPath))
                throw new FileNotFoundException($"找不到設定檔：{iniPath}");

            string[] lines = File.ReadAllLines(iniPath);
            for (int j = 0; j < lines.Length; j++)
            {
                if (lines[j].Trim().StartsWith("DEV0=", StringComparison.OrdinalIgnoreCase))
                {
                    lines[j] = $"DEV0={ip}";
                    break;
                }
            }
            File.WriteAllLines(iniPath, lines);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 執行自動模式
        /// </summary>
        private void ExecuteAutoMode()
        {
            try
            {
                // 步驟 1: 初始化指定的板
                if (!InitializeBoardAuto(m_AutoModeArgs.BoardIndex, m_AutoModeArgs.ConfigPath))
                {
                    ExitCode = 1; // 初始化失敗
                    this.Close();
                    return;
                }

                // 步驟 2: 載入 DXF 檔案或繪製手動線段
                if (!string.IsNullOrEmpty(m_AutoModeArgs.DxfPath))
                {
                    if (!LoadDxfAuto(m_AutoModeArgs.BoardIndex, m_AutoModeArgs.DxfPath))
                    {
                        ExitCode = 2; // DXF 載入失敗
                        this.Close();
                        return;
                    }
                }
                else if (m_AutoModeArgs.Lines.Count > 0)
                {
                    foreach (var line in m_AutoModeArgs.Lines)
                    {
                        if (!DrawLineAuto(m_AutoModeArgs.BoardIndex, line))
                        {
                            ExitCode = 2; // 繪圖失敗
                            this.Close();
                            return;
                        }
                    }

                    // 所有線段繪製完成後，確保全部載入
                    m_MMMark[m_AutoModeArgs.BoardIndex].Redraw();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(300);
                    System.Diagnostics.Debug.WriteLine($"已繪製 {m_AutoModeArgs.Lines.Count} 條線段");
                }

                // 步驟 3: 如果需要自動打標
                if (m_AutoModeArgs.AutoMark)
                {
                    if (!ExecuteMarkingAuto(m_AutoModeArgs.BoardIndex))
                    {
                        ExitCode = 3; // 打標失敗
                        this.Close();
                        return;
                    }
                }

                // 成功
                ExitCode = 0;
                this.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"自動模式執行失敗：{ex.Message}");
                ExitCode = 1;
                this.Close();
            }
        }

        /// <summary>
        /// 自動模式：初始化指定的板
        /// </summary>
        private bool InitializeBoardAuto(int boardIndex, string configPath)
        {
            try
            {
                // 建立控件
                m_MMMark[boardIndex] = new AxMMMarkx641();
                m_MMEdit[boardIndex] = new AxMMEditx641();

                m_MMMark[boardIndex].Left = 0;
                m_MMMark[boardIndex].Top = 0;
                m_MMMark[boardIndex].Width = m_Panels[boardIndex].Width;
                m_MMMark[boardIndex].Height = m_Panels[boardIndex].Height;

                m_Panels[boardIndex].Controls.Add(m_MMMark[boardIndex]);

                this.Controls.Add(m_MMEdit[boardIndex]);
                m_MMEdit[boardIndex].Visible = false;

                Application.DoEvents();

                // 初始化 MMMark
                m_MMMark[boardIndex].InitialExt(configPath);
                m_MMMark[boardIndex].SetDesktopCenter(0, 0);
                m_MMMark[boardIndex].SetDesktopSize(100, 100);
                m_MMMark[boardIndex].SetActiveDB(0);
                m_MMMark[boardIndex].MarkStandBy();
                m_MMMark[boardIndex].SetCurEditFun(2);
                m_MMMark[boardIndex].Redraw();

                m_bBoardInit[boardIndex] = true;

                Application.DoEvents();

                // 初始化 MMEdit
                m_MMEdit[boardIndex].InitialExt(configPath);

                m_bInit = true;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化板 {boardIndex} 失敗：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自動模式：繪製線段
        /// </summary>
        private bool DrawLineAuto(int boardIndex, LineSegment line)
        {
            try
            {
                // 使用 MMEdit 的 AddLine 方法新增線段
                int result = m_MMEdit[boardIndex].AddLine(line.X1, line.Y1, line.X2, line.Y2, "", "");

                if (result == 0)
                {
                    // 只新增到資料庫，不立即 Redraw（批次繪製時效能更好）
                    // Redraw 會在所有線段繪製完成後統一執行
                    System.Diagnostics.Debug.WriteLine($"已繪製線段：{line}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"繪製線段失敗，錯誤碼：{result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"繪製線段失敗：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自動模式：載入 DXF 檔案（使用 ParseDXFFile 解析線段）
        /// </summary>
        private bool LoadDxfAuto(int boardIndex, string dxfPath)
        {
            try
            {
                // 如果是相對路徑，轉換為絕對路徑
                if (!Path.IsPathRooted(dxfPath))
                {
                    string exeDir = Path.GetDirectoryName(Application.ExecutablePath);
                    dxfPath = Path.Combine(exeDir, dxfPath);
                }

                if (!File.Exists(dxfPath))
                {
                    System.Diagnostics.Debug.WriteLine($"找不到 DXF 檔案：{dxfPath}");
                    return false;
                }

                var lines = ParseDXFFile(dxfPath);

                if (lines.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"DXF 檔案中沒有找到線段：{dxfPath}");
                    return false;
                }

                // 計算座標範圍並自動縮放
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

                const double WORKSPACE_SIZE = 150.0;
                const double MARGIN_PERCENT = 0.9;
                double maxSpan = Math.Max(origWidth, origHeight);
                double scaleFactor = (WORKSPACE_SIZE * MARGIN_PERCENT) / maxSpan;

                System.Diagnostics.Debug.WriteLine($"DXF 解析完成：{lines.Count} 條線段，縮放比例：{scaleFactor:F4}");

                // 轉換座標並加入到 MMEdit
                foreach (var line in lines)
                {
                    double tx1 = (line.X1 - origCenterX) * scaleFactor;
                    double ty1 = (line.Y1 - origCenterY) * scaleFactor;
                    double tx2 = (line.X2 - origCenterX) * scaleFactor;
                    double ty2 = (line.Y2 - origCenterY) * scaleFactor;

                    m_MMEdit[boardIndex].AddLine(tx1, ty1, tx2, ty2, "", "");
                }

                Application.DoEvents();
                Thread.Sleep(100);
                m_MMMark[boardIndex].Redraw();
                Thread.Sleep(300);

                System.Diagnostics.Debug.WriteLine($"DXF 載入完成：{lines.Count} 條線段已加入晶片板 {boardIndex + 1}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DXF 載入失敗：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 自動模式：執行打標（同步等待完成）
        /// </summary>
        private bool ExecuteMarkingAuto(int boardIndex)
        {
            try
            {
                // 確保圖形已重繪並載入
                m_MMMark[boardIndex].Redraw();
                Application.DoEvents();
                System.Threading.Thread.Sleep(200); // 給予時間讓圖形載入

                // 進入待命狀態
                m_MMMark[boardIndex].MarkStandBy();
                Application.DoEvents();

                // 啟動打標（模式 4 = 非阻塞）
                int startResult = m_MMMark[boardIndex].StartMarking(4);
                if (startResult != 0)
                {
                    System.Diagnostics.Debug.WriteLine($"打標啟動失敗，錯誤碼：{startResult}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine("打標已啟動，等待完成...");

                // 同步等待打標完成
                int loopCount = 0;
                while (m_MMMark[boardIndex].IsMarking() != 0)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    loopCount++;

                    // 每 10 次迴圈輸出一次狀態（每秒）
                    if (loopCount % 10 == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"打標進行中... ({loopCount * 100}ms)");
                    }

                    // 超時保護（60 秒）
                    if (loopCount > 600)
                    {
                        System.Diagnostics.Debug.WriteLine("打標超時，強制停止");
                        m_MMMark[boardIndex].StopMarking();
                        return false;
                    }
                }

                m_MMMark[boardIndex].MarkShutdown();
                System.Diagnostics.Debug.WriteLine($"打標完成（耗時 {loopCount * 100}ms）");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打標失敗：{ex.Message}");
                return false;
            }
        }

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

                // 自動縮放和平移到工作區 (-75 到 +75)
                const double WORKSPACE_SIZE = 150.0; // -75 到 +75
                const double MARGIN_PERCENT = 0.9;   // 使用 90% 空間，留 10% 邊距

                double maxSpan = Math.Max(origWidth, origHeight);
                double scaleFactor = (WORKSPACE_SIZE * MARGIN_PERCENT) / maxSpan;

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
                sb.AppendLine($"共找到 {lines.Count} 條線段\n");
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
                    sb.AppendLine($"... 還有 {transformedLines.Count - displayCount} 條線段");
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
                    $"共解析出 {lines.Count} 條線段\n" +
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
                btnLoadDXF.Enabled = false;
                btnLoadDXFFile.Enabled = false;
                btnMark.Enabled = false;
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"啟動雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                System.Diagnostics.Debug.WriteLine($"解析 DXF 完成，共找到 {lines.Count} 條線段");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解析 DXF 失敗：{ex.Message}");
            }

            return lines;
        }

        /// <summary>
        /// 清理舊的控件（防止殘留導致初始化失敗）
        /// </summary>
        private void CleanupOldControls()
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    // 清理 MMMark
                    if (m_MMMark[i] != null)
                    {
                        try
                        {
                            if (m_bBoardInit[i])
                            {
                                m_MMMark[i].Finish();
                            }

                            if (m_Panels[i].Controls.Contains(m_MMMark[i]))
                            {
                                m_Panels[i].Controls.Remove(m_MMMark[i]);
                            }

                            m_MMMark[i].Dispose();
                            m_MMMark[i] = null;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"清理 MMMark[{i}] 失敗：{ex.Message}");
                        }
                    }

                    // 清理 MMEdit
                    if (m_MMEdit[i] != null)
                    {
                        try
                        {
                            if (m_bBoardInit[i])
                            {
                                m_MMEdit[i].Finish();
                            }

                            if (this.Controls.Contains(m_MMEdit[i]))
                            {
                                this.Controls.Remove(m_MMEdit[i]);
                            }

                            m_MMEdit[i].Dispose();
                            m_MMEdit[i] = null;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"清理 MMEdit[{i}] 失敗：{ex.Message}");
                        }
                    }

                    m_bBoardInit[i] = false;
                }

                m_bInit = false;
                Application.DoEvents();
                System.Threading.Thread.Sleep(500); // 等待清理完成

                System.Diagnostics.Debug.WriteLine("舊控件清理完成");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清理過程發生錯誤：{ex.Message}");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CleanupOldControls();
        }
    }

    /// <summary>
    /// DXF 線段資料結構
    /// </summary>
    public class DXFLine
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public double Length
        {
            get
            {
                double dx = X2 - X1;
                double dy = Y2 - Y1;
                return Math.Sqrt(dx * dx + dy * dy);
            }
        }

        public DXFLine(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override string ToString()
        {
            return $"({X1:F2}, {Y1:F2}) -> ({X2:F2}, {Y2:F2}), 長度: {Length:F2}";
        }
    }
}
