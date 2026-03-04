using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

        public Form1()
        {
            InitializeComponent();

            m_Panels = new Panel[] { panelBoard1, panelBoard2, panelBoard3, panelBoard4 };
            m_txtIPs = new TextBox[] { txtIP1, txtIP2, txtIP3, txtIP4 };
            comboBoard.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadIPSettings();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if (m_bInit)
            {
                MessageBox.Show("已經初始化過了！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int successCount = 0;
            string failInfo = "";

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    // 創建多系統控件實例
                    m_MMMark[i] = new AxMMMarkx641();
                    m_MMEdit[i] = new AxMMEditx641();

                    // 設置 MMMark 控件位置和大小
                    m_MMMark[i].Left = 0;
                    m_MMMark[i].Top = 0;
                    m_MMMark[i].Width = m_Panels[i].Width;
                    m_MMMark[i].Height = m_Panels[i].Height;

                    // 添加到對應的面板
                    m_Panels[i].Controls.Add(m_MMMark[i]);

                    // 添加編輯控件（隱藏）
                    this.Controls.Add(m_MMEdit[i]);
                    m_MMEdit[i].Visible = false;

                    // 使用多系統初始化方法，每個板有獨立的配置
                    m_MMMark[i].InitialExt(m_ConfigPaths[i]);
                    m_MMEdit[i].InitialExt(m_ConfigPaths[i]);

                    // 設置工作區域
                    m_MMMark[i].SetDesktopCenter(0, 0);
                    m_MMMark[i].SetDesktopSize(100, 100);
                    m_MMMark[i].SetActiveDB(0);
                    m_MMMark[i].MarkStandBy();
                    m_MMMark[i].SetCurEditFun(2);

                    m_bBoardInit[i] = true;
                    successCount++;
                }
                catch (Exception ex)
                {
                    m_bBoardInit[i] = false;
                    failInfo += $"晶片板 {i + 1}：{ex.Message}\n";
                }
            }

            m_bInit = successCount > 0;

            if (successCount == 4)
            {
                MessageBox.Show("四個晶片板全部初始化完成！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (successCount > 0)
            {
                MessageBox.Show($"已成功初始化 {successCount}/4 個晶片板。\n\n以下晶片板初始化失敗：\n{failInfo}\n" +
                    "提示：MarkingMate MultiMM 預設只支援 MM1、MM2。\n" +
                    "若需要 MM3、MM4，請在 MarkingMate 中建立對應的配置。",
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_bInit)
            {
                timerMark.Stop();

                for (int i = 0; i < 4; i++)
                {
                    if (m_bBoardInit[i])
                    {
                        if (m_MMMark[i] != null)
                            m_MMMark[i].Finish();

                        if (m_MMEdit[i] != null)
                            m_MMEdit[i].Finish();
                    }
                }
            }
        }
    }
}
