using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxMMMarkx64Lib;
using AxMMEditx64Lib;
using AxMMStatusx64Lib;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // 四個晶片板的控件陣列
        protected AxMMMarkx64[] m_MMMark = new AxMMMarkx64[4];
        protected AxMMEditx64[] m_MMEdit = new AxMMEditx64[4];
        protected AxMMStatusx64[] m_MMStatus = new AxMMStatusx64[4];

        private Panel[] m_Panels;
        private bool m_bInit = false;
        private int m_iCurrentBoard = 0;

        public Form1()
        {
            InitializeComponent();

            // 初始化面板陣列
            m_Panels = new Panel[] { panelBoard1, panelBoard2, panelBoard3, panelBoard4 };
            comboBoard.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form load event
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if (!m_bInit)
            {
                // 初始化四個晶片板
                for (int i = 0; i < 4; i++)
                {
                    // 創建控件實例
                    m_MMMark[i] = new AxMMMarkx64();
                    m_MMEdit[i] = new AxMMEditx64();
                    m_MMStatus[i] = new AxMMStatusx64();

                    // 設置 MMMark 控件位置和大小
                    m_MMMark[i].Left = 0;
                    m_MMMark[i].Top = 0;
                    m_MMMark[i].Width = m_Panels[i].Width;
                    m_MMMark[i].Height = m_Panels[i].Height;

                    // 添加到對應的面板
                    m_Panels[i].Controls.Add(m_MMMark[i]);

                    // 添加編輯和狀態控件（隱藏）
                    this.Controls.Add(m_MMEdit[i]);
                    this.Controls.Add(m_MMStatus[i]);
                    m_MMEdit[i].Visible = false;
                    m_MMStatus[i].Visible = false;

                    // 初始化控件
                    m_MMMark[i].Initial();
                    m_MMEdit[i].Initial();
                    m_MMStatus[i].Initial();

                    // 設置工作區域
                    m_MMMark[i].SetDesktopCenter(0, 0);
                    m_MMMark[i].SetDesktopSize(100, 100);
                    m_MMMark[i].SetActiveDB(0);
                    m_MMMark[i].MarkStandBy();
                    m_MMMark[i].SetCurEditFun(2); // 設置編輯模式
                }

                m_bInit = true;
                MessageBox.Show("四個晶片板初始化完成！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("已經初始化過了！", "初始化", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                // 獲取選擇的晶片板
                m_iCurrentBoard = comboBoard.SelectedIndex;

                // 獲取線段參數
                double x1 = double.Parse(txtX1.Text);
                double y1 = double.Parse(txtY1.Text);
                double x2 = double.Parse(txtX2.Text);
                double y2 = double.Parse(txtY2.Text);

                // 使用 MMEdit 的 AddLine 方法添加線段
                // 參數: X1, Y1, X2, Y2, ParentName(""), NewName("")
                int result = m_MMEdit[m_iCurrentBoard].AddLine(x1, y1, x2, y2, "", "");

                if (result == 0)
                {
                    // 更新顯示
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

                // 開始雷射打標（非阻塞模式）
                m_MMMark[m_iCurrentBoard].StartMarking(4);

                MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 開始雷射打標！",
                    "開始", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // 停止雷射打標
                m_MMMark[m_iCurrentBoard].StopMarking();

                MessageBox.Show($"晶片板 {m_iCurrentBoard + 1} 停止雷射打標！",
                    "停止", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止雷射失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_bInit)
            {
                // 清理資源
                for (int i = 0; i < 4; i++)
                {
                    if (m_MMMark[i] != null)
                    {
                        m_MMMark[i].MarkShutdown();
                        m_MMMark[i].Finish();
                    }

                    if (m_MMEdit[i] != null)
                    {
                        m_MMEdit[i].Finish();
                    }

                    if (m_MMStatus[i] != null)
                    {
                        m_MMStatus[i].Finish();
                    }
                }
            }
        }
    }
}
