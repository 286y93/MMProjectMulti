namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerMark = new System.Windows.Forms.Timer(this.components);
            this.panelBoard1 = new System.Windows.Forms.Panel();
            this.panelBoard2 = new System.Windows.Forms.Panel();
            this.panelBoard3 = new System.Windows.Forms.Panel();
            this.panelBoard4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageConnect = new System.Windows.Forms.TabPage();
            this.lblBoardCount = new System.Windows.Forms.Label();
            this.numBoardCount = new System.Windows.Forms.NumericUpDown();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnTestConnect = new System.Windows.Forms.Button();
            this.tabPageDraw = new System.Windows.Forms.TabPage();
            this.comboBoard = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtX1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtY1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtX2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtY2 = new System.Windows.Forms.TextBox();
            this.btnDrawLine = new System.Windows.Forms.Button();
            this.btnMark = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblDXFInfo = new System.Windows.Forms.Label();
            this.txtDXFInfo = new System.Windows.Forms.TextBox();
            this.btnBrowseDXF = new System.Windows.Forms.Button();
            this.lblBoardDXF = new System.Windows.Forms.Label();
            this.comboBoardDXF = new System.Windows.Forms.ComboBox();
            this.lblDXFPath = new System.Windows.Forms.Label();
            this.txtDXFPath = new System.Windows.Forms.TextBox();
            this.btnMarkDXF = new System.Windows.Forms.Button();
            this.btnPreviewDXF = new System.Windows.Forms.Button();
            this.btnClearDXF = new System.Windows.Forms.Button();
            this.btnLoadDXFFile = new System.Windows.Forms.Button();
            this.btnLoadDXF = new System.Windows.Forms.Button();
            this.tabPageParams = new System.Windows.Forms.TabPage();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.txtWorkspace = new System.Windows.Forms.TextBox();
            this.lblMargin = new System.Windows.Forms.Label();
            this.txtMargin = new System.Windows.Forms.TextBox();
            this.groupBoxIP = new System.Windows.Forms.GroupBox();
            this.lblIP1 = new System.Windows.Forms.Label();
            this.txtIP1 = new System.Windows.Forms.TextBox();
            this.lblIP2 = new System.Windows.Forms.Label();
            this.txtIP2 = new System.Windows.Forms.TextBox();
            this.lblIP3 = new System.Windows.Forms.Label();
            this.txtIP3 = new System.Windows.Forms.TextBox();
            this.lblIP4 = new System.Windows.Forms.Label();
            this.txtIP4 = new System.Windows.Forms.TextBox();
            this.btnReadIP = new System.Windows.Forms.Button();
            this.btnSaveIP = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBoardCount)).BeginInit();
            this.tabPageDraw.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPageParams.SuspendLayout();
            this.groupBoxIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerMark
            // 
            this.timerMark.Interval = 200;
            this.timerMark.Tick += new System.EventHandler(this.timerMark_Tick);
            // 
            // panelBoard1
            // 
            this.panelBoard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard1.Location = new System.Drawing.Point(20, 38);
            this.panelBoard1.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard1.Name = "panelBoard1";
            this.panelBoard1.Size = new System.Drawing.Size(533, 374);
            this.panelBoard1.TabIndex = 0;
            // 
            // panelBoard2
            // 
            this.panelBoard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard2.Location = new System.Drawing.Point(20, 38);
            this.panelBoard2.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard2.Name = "panelBoard2";
            this.panelBoard2.Size = new System.Drawing.Size(533, 374);
            this.panelBoard2.TabIndex = 0;
            // 
            // panelBoard3
            // 
            this.panelBoard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard3.Location = new System.Drawing.Point(20, 38);
            this.panelBoard3.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard3.Name = "panelBoard3";
            this.panelBoard3.Size = new System.Drawing.Size(533, 374);
            this.panelBoard3.TabIndex = 0;
            // 
            // panelBoard4
            // 
            this.panelBoard4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard4.Location = new System.Drawing.Point(20, 38);
            this.panelBoard4.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard4.Name = "panelBoard4";
            this.panelBoard4.Size = new System.Drawing.Size(533, 374);
            this.panelBoard4.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelBoard1);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(573, 431);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "晶片板 1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelBoard2);
            this.groupBox2.Location = new System.Drawing.Point(597, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(573, 431);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "晶片板 2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panelBoard3);
            this.groupBox3.Location = new System.Drawing.Point(16, 454);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(573, 431);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "晶片板 3";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panelBoard4);
            this.groupBox4.Location = new System.Drawing.Point(597, 454);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(573, 431);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "晶片板 4";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageConnect);
            this.tabControl1.Controls.Add(this.tabPageParams);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPageDraw);
            this.tabControl1.Location = new System.Drawing.Point(1185, 15);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(245, 790);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPageConnect
            // 
            this.tabPageConnect.Controls.Add(this.lblBoardCount);
            this.tabPageConnect.Controls.Add(this.numBoardCount);
            this.tabPageConnect.Controls.Add(this.btnInit);
            this.tabPageConnect.Controls.Add(this.btnTestConnect);
            this.tabPageConnect.Location = new System.Drawing.Point(4, 25);
            this.tabPageConnect.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageConnect.Name = "tabPageConnect";
            this.tabPageConnect.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageConnect.Size = new System.Drawing.Size(237, 761);
            this.tabPageConnect.TabIndex = 0;
            this.tabPageConnect.Text = "1. 連接設定";
            this.tabPageConnect.UseVisualStyleBackColor = true;
            // 
            // lblBoardCount
            // 
            this.lblBoardCount.AutoSize = true;
            this.lblBoardCount.Location = new System.Drawing.Point(8, 15);
            this.lblBoardCount.Name = "lblBoardCount";
            this.lblBoardCount.Size = new System.Drawing.Size(71, 15);
            this.lblBoardCount.TabIndex = 10;
            this.lblBoardCount.Text = "系統數量:";
            // 
            // numBoardCount
            // 
            this.numBoardCount.Location = new System.Drawing.Point(85, 13);
            this.numBoardCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numBoardCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBoardCount.Name = "numBoardCount";
            this.numBoardCount.Size = new System.Drawing.Size(50, 25);
            this.numBoardCount.TabIndex = 11;
            this.numBoardCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(145, 8);
            this.btnInit.Margin = new System.Windows.Forms.Padding(4);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(80, 35);
            this.btnInit.TabIndex = 0;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnTestConnect
            // 
            this.btnTestConnect.Location = new System.Drawing.Point(25, 60);
            this.btnTestConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestConnect.Name = "btnTestConnect";
            this.btnTestConnect.Size = new System.Drawing.Size(190, 50);
            this.btnTestConnect.TabIndex = 2;
            this.btnTestConnect.Text = "測試連接";
            this.btnTestConnect.UseVisualStyleBackColor = true;
            this.btnTestConnect.Click += new System.EventHandler(this.btnTestConnect_Click);
            // 
            // tabPageDraw
            // 
            this.tabPageDraw.Controls.Add(this.comboBoard);
            this.tabPageDraw.Controls.Add(this.label5);
            this.tabPageDraw.Controls.Add(this.groupBox5);
            this.tabPageDraw.Controls.Add(this.btnDrawLine);
            this.tabPageDraw.Controls.Add(this.btnMark);
            this.tabPageDraw.Controls.Add(this.btnStop);
            this.tabPageDraw.Location = new System.Drawing.Point(4, 25);
            this.tabPageDraw.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageDraw.Name = "tabPageDraw";
            this.tabPageDraw.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageDraw.Size = new System.Drawing.Size(237, 761);
            this.tabPageDraw.TabIndex = 1;
            this.tabPageDraw.Text = "4. 手動繪圖";
            this.tabPageDraw.UseVisualStyleBackColor = true;
            // 
            // comboBoard
            // 
            this.comboBoard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoard.FormattingEnabled = true;
            this.comboBoard.Items.AddRange(new object[] {
            "晶片板 1",
            "晶片板 2",
            "晶片板 3",
            "晶片板 4"});
            this.comboBoard.Location = new System.Drawing.Point(8, 32);
            this.comboBoard.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoard.Name = "comboBoard";
            this.comboBoard.Size = new System.Drawing.Size(156, 23);
            this.comboBoard.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 13);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "選擇板:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.txtX1);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtY1);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.txtX2);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.txtY2);
            this.groupBox5.Location = new System.Drawing.Point(4, 63);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(213, 250);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "線段參數";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "X1:";
            // 
            // txtX1
            // 
            this.txtX1.Location = new System.Drawing.Point(80, 27);
            this.txtX1.Margin = new System.Windows.Forms.Padding(4);
            this.txtX1.Name = "txtX1";
            this.txtX1.Size = new System.Drawing.Size(105, 25);
            this.txtX1.TabIndex = 1;
            this.txtX1.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 135);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Y2:";
            // 
            // txtY1
            // 
            this.txtY1.Location = new System.Drawing.Point(80, 62);
            this.txtY1.Margin = new System.Windows.Forms.Padding(4);
            this.txtY1.Name = "txtY1";
            this.txtY1.Size = new System.Drawing.Size(105, 25);
            this.txtY1.TabIndex = 3;
            this.txtY1.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 100);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "X2:";
            // 
            // txtX2
            // 
            this.txtX2.Location = new System.Drawing.Point(80, 97);
            this.txtX2.Margin = new System.Windows.Forms.Padding(4);
            this.txtX2.Name = "txtX2";
            this.txtX2.Size = new System.Drawing.Size(105, 25);
            this.txtX2.TabIndex = 5;
            this.txtX2.Text = "50";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y1:";
            // 
            // txtY2
            // 
            this.txtY2.Location = new System.Drawing.Point(80, 132);
            this.txtY2.Margin = new System.Windows.Forms.Padding(4);
            this.txtY2.Name = "txtY2";
            this.txtY2.Size = new System.Drawing.Size(105, 25);
            this.txtY2.TabIndex = 7;
            this.txtY2.Text = "50";
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.Location = new System.Drawing.Point(11, 321);
            this.btnDrawLine.Margin = new System.Windows.Forms.Padding(4);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(204, 50);
            this.btnDrawLine.TabIndex = 1;
            this.btnDrawLine.Text = "繪製線段";
            this.btnDrawLine.UseVisualStyleBackColor = true;
            this.btnDrawLine.Click += new System.EventHandler(this.btnDrawLine_Click);
            // 
            // btnMark
            // 
            this.btnMark.Location = new System.Drawing.Point(11, 379);
            this.btnMark.Margin = new System.Windows.Forms.Padding(4);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(204, 50);
            this.btnMark.TabIndex = 2;
            this.btnMark.Text = "開始雷射";
            this.btnMark.UseVisualStyleBackColor = true;
            this.btnMark.Click += new System.EventHandler(this.btnMark_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(8, 437);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(207, 50);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblDXFInfo);
            this.tabPage1.Controls.Add(this.txtDXFInfo);
            this.tabPage1.Controls.Add(this.btnBrowseDXF);
            this.tabPage1.Controls.Add(this.lblBoardDXF);
            this.tabPage1.Controls.Add(this.comboBoardDXF);
            this.tabPage1.Controls.Add(this.lblDXFPath);
            this.tabPage1.Controls.Add(this.txtDXFPath);
            this.tabPage1.Controls.Add(this.btnMarkDXF);
            this.tabPage1.Controls.Add(this.btnPreviewDXF);
            this.tabPage1.Controls.Add(this.btnClearDXF);
            this.tabPage1.Controls.Add(this.btnLoadDXFFile);
            this.tabPage1.Controls.Add(this.btnLoadDXF);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(237, 761);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "3. DXF 操作";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblDXFInfo
            // 
            this.lblDXFInfo.AutoSize = true;
            this.lblDXFInfo.Location = new System.Drawing.Point(20, 395);
            this.lblDXFInfo.Name = "lblDXFInfo";
            this.lblDXFInfo.Size = new System.Drawing.Size(82, 15);
            this.lblDXFInfo.TabIndex = 8;
            this.lblDXFInfo.Text = "線段資訊：";
            //
            // txtDXFInfo
            //
            this.txtDXFInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDXFInfo.Location = new System.Drawing.Point(20, 413);
            this.txtDXFInfo.Multiline = true;
            this.txtDXFInfo.Name = "txtDXFInfo";
            this.txtDXFInfo.ReadOnly = true;
            this.txtDXFInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDXFInfo.Size = new System.Drawing.Size(191, 327);
            this.txtDXFInfo.TabIndex = 7;
            this.txtDXFInfo.WordWrap = false;
            // 
            // btnBrowseDXF
            // 
            this.btnBrowseDXF.Location = new System.Drawing.Point(20, 121);
            this.btnBrowseDXF.Name = "btnBrowseDXF";
            this.btnBrowseDXF.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDXF.TabIndex = 6;
            this.btnBrowseDXF.Text = "瀏覽...";
            this.btnBrowseDXF.UseVisualStyleBackColor = true;
            this.btnBrowseDXF.Click += new System.EventHandler(this.btnBrowseDXF_Click);
            // 
            // lblBoardDXF
            // 
            this.lblBoardDXF.AutoSize = true;
            this.lblBoardDXF.Location = new System.Drawing.Point(20, 150);
            this.lblBoardDXF.Name = "lblBoardDXF";
            this.lblBoardDXF.Size = new System.Drawing.Size(67, 15);
            this.lblBoardDXF.TabIndex = 5;
            this.lblBoardDXF.Text = "選擇板：";
            // 
            // comboBoardDXF
            // 
            this.comboBoardDXF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoardDXF.FormattingEnabled = true;
            this.comboBoardDXF.Items.AddRange(new object[] {
            "板 1",
            "板 2",
            "板 3",
            "板 4"});
            this.comboBoardDXF.Location = new System.Drawing.Point(90, 147);
            this.comboBoardDXF.Name = "comboBoardDXF";
            this.comboBoardDXF.Size = new System.Drawing.Size(121, 23);
            this.comboBoardDXF.TabIndex = 4;
            // 
            // lblDXFPath
            // 
            this.lblDXFPath.AutoSize = true;
            this.lblDXFPath.Location = new System.Drawing.Point(20, 30);
            this.lblDXFPath.Name = "lblDXFPath";
            this.lblDXFPath.Size = new System.Drawing.Size(84, 15);
            this.lblDXFPath.TabIndex = 3;
            this.lblDXFPath.Text = "DXF 路徑：";
            // 
            // txtDXFPath
            // 
            this.txtDXFPath.Location = new System.Drawing.Point(20, 55);
            this.txtDXFPath.Multiline = true;
            this.txtDXFPath.Name = "txtDXFPath";
            this.txtDXFPath.Size = new System.Drawing.Size(191, 60);
            this.txtDXFPath.TabIndex = 2;
            this.txtDXFPath.Text = "File\\上翼板-2.dxf";
            // 
            // btnMarkDXF
            // 
            this.btnMarkDXF.Enabled = false;
            this.btnMarkDXF.Location = new System.Drawing.Point(23, 278);
            this.btnMarkDXF.Name = "btnMarkDXF";
            this.btnMarkDXF.Size = new System.Drawing.Size(188, 35);
            this.btnMarkDXF.TabIndex = 1;
            this.btnMarkDXF.Text = "打標";
            this.btnMarkDXF.UseVisualStyleBackColor = true;
            this.btnMarkDXF.Click += new System.EventHandler(this.btnMarkDXF_Click);
            // 
            // btnLoadDXFFile
            // 
            this.btnLoadDXFFile.Location = new System.Drawing.Point(20, 233);
            this.btnLoadDXFFile.Name = "btnLoadDXFFile";
            this.btnLoadDXFFile.Size = new System.Drawing.Size(191, 35);
            this.btnLoadDXFFile.TabIndex = 9;
            this.btnLoadDXFFile.Text = "載入 DXF";
            this.btnLoadDXFFile.UseVisualStyleBackColor = true;
            this.btnLoadDXFFile.Click += new System.EventHandler(this.btnLoadDXFFile_Click);
            // 
            // btnLoadDXF
            // 
            this.btnLoadDXF.Location = new System.Drawing.Point(20, 190);
            this.btnLoadDXF.Name = "btnLoadDXF";
            this.btnLoadDXF.Size = new System.Drawing.Size(191, 35);
            this.btnLoadDXF.TabIndex = 0;
            this.btnLoadDXF.Text = "載入 DXF 線段";
            this.btnLoadDXF.UseVisualStyleBackColor = true;
            this.btnLoadDXF.Click += new System.EventHandler(this.btnLoadDXF_Click);
            //
            // btnPreviewDXF
            //
            this.btnPreviewDXF.Location = new System.Drawing.Point(20, 320);
            this.btnPreviewDXF.Name = "btnPreviewDXF";
            this.btnPreviewDXF.Size = new System.Drawing.Size(95, 30);
            this.btnPreviewDXF.TabIndex = 10;
            this.btnPreviewDXF.Text = "紅光預覽";
            this.btnPreviewDXF.UseVisualStyleBackColor = true;
            this.btnPreviewDXF.Click += new System.EventHandler(this.btnPreviewDXF_Click);
            //
            // btnClearDXF
            //
            this.btnClearDXF.Location = new System.Drawing.Point(120, 320);
            this.btnClearDXF.Name = "btnClearDXF";
            this.btnClearDXF.Size = new System.Drawing.Size(91, 30);
            this.btnClearDXF.TabIndex = 11;
            this.btnClearDXF.Text = "清除畫面";
            this.btnClearDXF.UseVisualStyleBackColor = true;
            this.btnClearDXF.Click += new System.EventHandler(this.btnClearDXF_Click);
            //
            // tabPageParams
            // 
            this.tabPageParams.Controls.Add(this.lblWorkspace);
            this.tabPageParams.Controls.Add(this.txtWorkspace);
            this.tabPageParams.Controls.Add(this.lblMargin);
            this.tabPageParams.Controls.Add(this.txtMargin);
            this.tabPageParams.Controls.Add(this.groupBoxIP);
            this.tabPageParams.Location = new System.Drawing.Point(4, 25);
            this.tabPageParams.Name = "tabPageParams";
            this.tabPageParams.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageParams.Size = new System.Drawing.Size(237, 761);
            this.tabPageParams.TabIndex = 3;
            this.tabPageParams.Text = "2. 雷射參數";
            this.tabPageParams.UseVisualStyleBackColor = true;
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.AutoSize = true;
            this.lblWorkspace.Location = new System.Drawing.Point(20, 20);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(88, 15);
            this.lblWorkspace.TabIndex = 0;
            this.lblWorkspace.Text = "工作區(mm):";
            // 
            // txtWorkspace
            // 
            this.txtWorkspace.Location = new System.Drawing.Point(120, 17);
            this.txtWorkspace.Name = "txtWorkspace";
            this.txtWorkspace.Size = new System.Drawing.Size(80, 25);
            this.txtWorkspace.TabIndex = 1;
            this.txtWorkspace.Text = "150";
            // 
            // lblMargin
            // 
            this.lblMargin.AutoSize = true;
            this.lblMargin.Location = new System.Drawing.Point(20, 55);
            this.lblMargin.Name = "lblMargin";
            this.lblMargin.Size = new System.Drawing.Size(63, 15);
            this.lblMargin.TabIndex = 2;
            this.lblMargin.Text = "邊距(%):";
            // 
            // txtMargin
            // 
            this.txtMargin.Location = new System.Drawing.Point(120, 52);
            this.txtMargin.Name = "txtMargin";
            this.txtMargin.Size = new System.Drawing.Size(80, 25);
            this.txtMargin.TabIndex = 3;
            this.txtMargin.Text = "90";
            // 
            // groupBoxIP
            // 
            this.groupBoxIP.Controls.Add(this.lblIP1);
            this.groupBoxIP.Controls.Add(this.txtIP1);
            this.groupBoxIP.Controls.Add(this.lblIP2);
            this.groupBoxIP.Controls.Add(this.txtIP2);
            this.groupBoxIP.Controls.Add(this.lblIP3);
            this.groupBoxIP.Controls.Add(this.txtIP3);
            this.groupBoxIP.Controls.Add(this.lblIP4);
            this.groupBoxIP.Controls.Add(this.txtIP4);
            this.groupBoxIP.Controls.Add(this.btnReadIP);
            this.groupBoxIP.Controls.Add(this.btnSaveIP);
            this.groupBoxIP.Location = new System.Drawing.Point(8, 90);
            this.groupBoxIP.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxIP.Name = "groupBoxIP";
            this.groupBoxIP.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxIP.Size = new System.Drawing.Size(213, 170);
            this.groupBoxIP.TabIndex = 1;
            this.groupBoxIP.TabStop = false;
            this.groupBoxIP.Text = "EMC6 IP 設定";
            // 
            // lblIP1
            // 
            this.lblIP1.AutoSize = true;
            this.lblIP1.Location = new System.Drawing.Point(8, 25);
            this.lblIP1.Name = "lblIP1";
            this.lblIP1.Size = new System.Drawing.Size(44, 15);
            this.lblIP1.TabIndex = 0;
            this.lblIP1.Text = "MM1:";
            // 
            // txtIP1
            // 
            this.txtIP1.Location = new System.Drawing.Point(55, 22);
            this.txtIP1.Name = "txtIP1";
            this.txtIP1.Size = new System.Drawing.Size(145, 25);
            this.txtIP1.TabIndex = 1;
            // 
            // lblIP2
            // 
            this.lblIP2.AutoSize = true;
            this.lblIP2.Location = new System.Drawing.Point(8, 55);
            this.lblIP2.Name = "lblIP2";
            this.lblIP2.Size = new System.Drawing.Size(44, 15);
            this.lblIP2.TabIndex = 2;
            this.lblIP2.Text = "MM2:";
            // 
            // txtIP2
            // 
            this.txtIP2.Location = new System.Drawing.Point(55, 52);
            this.txtIP2.Name = "txtIP2";
            this.txtIP2.Size = new System.Drawing.Size(145, 25);
            this.txtIP2.TabIndex = 3;
            // 
            // lblIP3
            // 
            this.lblIP3.AutoSize = true;
            this.lblIP3.Location = new System.Drawing.Point(8, 85);
            this.lblIP3.Name = "lblIP3";
            this.lblIP3.Size = new System.Drawing.Size(44, 15);
            this.lblIP3.TabIndex = 4;
            this.lblIP3.Text = "MM3:";
            // 
            // txtIP3
            // 
            this.txtIP3.Location = new System.Drawing.Point(55, 82);
            this.txtIP3.Name = "txtIP3";
            this.txtIP3.Size = new System.Drawing.Size(145, 25);
            this.txtIP3.TabIndex = 5;
            // 
            // lblIP4
            // 
            this.lblIP4.AutoSize = true;
            this.lblIP4.Location = new System.Drawing.Point(8, 115);
            this.lblIP4.Name = "lblIP4";
            this.lblIP4.Size = new System.Drawing.Size(44, 15);
            this.lblIP4.TabIndex = 6;
            this.lblIP4.Text = "MM4:";
            // 
            // txtIP4
            // 
            this.txtIP4.Location = new System.Drawing.Point(55, 112);
            this.txtIP4.Name = "txtIP4";
            this.txtIP4.Size = new System.Drawing.Size(145, 25);
            this.txtIP4.TabIndex = 7;
            // 
            // btnReadIP
            // 
            this.btnReadIP.Location = new System.Drawing.Point(10, 142);
            this.btnReadIP.Name = "btnReadIP";
            this.btnReadIP.Size = new System.Drawing.Size(90, 23);
            this.btnReadIP.TabIndex = 8;
            this.btnReadIP.Text = "讀取IP";
            this.btnReadIP.UseVisualStyleBackColor = true;
            this.btnReadIP.Click += new System.EventHandler(this.btnReadIP_Click);
            // 
            // btnSaveIP
            // 
            this.btnSaveIP.Location = new System.Drawing.Point(110, 142);
            this.btnSaveIP.Name = "btnSaveIP";
            this.btnSaveIP.Size = new System.Drawing.Size(90, 23);
            this.btnSaveIP.TabIndex = 9;
            this.btnSaveIP.Text = "儲存IP";
            this.btnSaveIP.UseVisualStyleBackColor = true;
            this.btnSaveIP.Click += new System.EventHandler(this.btnSaveIP_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(1200, 812);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(160, 50);
            this.btnExit.TabIndex = 21;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 820);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExit);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MarkingMate 多晶片板控制系統 v1.1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageConnect.ResumeLayout(false);
            this.tabPageConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBoardCount)).EndInit();
            this.tabPageDraw.ResumeLayout(false);
            this.tabPageDraw.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPageParams.ResumeLayout(false);
            this.tabPageParams.PerformLayout();
            this.groupBoxIP.ResumeLayout(false);
            this.groupBoxIP.PerformLayout();
            this.ResumeLayout(false);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

        }

        #endregion

        private System.Windows.Forms.Panel panelBoard1;
        private System.Windows.Forms.Panel panelBoard2;
        private System.Windows.Forms.Panel panelBoard3;
        private System.Windows.Forms.Panel panelBoard4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageConnect;
        private System.Windows.Forms.TabPage tabPageDraw;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnTestConnect;
        private System.Windows.Forms.GroupBox groupBoxIP;
        private System.Windows.Forms.Label lblIP1;
        private System.Windows.Forms.Label lblIP2;
        private System.Windows.Forms.Label lblIP3;
        private System.Windows.Forms.Label lblIP4;
        private System.Windows.Forms.TextBox txtIP1;
        private System.Windows.Forms.TextBox txtIP2;
        private System.Windows.Forms.TextBox txtIP3;
        private System.Windows.Forms.TextBox txtIP4;
        private System.Windows.Forms.Button btnReadIP;
        private System.Windows.Forms.Button btnSaveIP;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtX1;
        private System.Windows.Forms.TextBox txtY1;
        private System.Windows.Forms.TextBox txtX2;
        private System.Windows.Forms.TextBox txtY2;
        private System.Windows.Forms.ComboBox comboBoard;
        private System.Windows.Forms.Button btnDrawLine;
        private System.Windows.Forms.Button btnMark;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblBoardCount;
        private System.Windows.Forms.NumericUpDown numBoardCount;
        private System.Windows.Forms.Timer timerMark;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnLoadDXF;
        private System.Windows.Forms.Button btnMarkDXF;
        private System.Windows.Forms.TextBox txtDXFPath;
        private System.Windows.Forms.Label lblDXFPath;
        private System.Windows.Forms.ComboBox comboBoardDXF;
        private System.Windows.Forms.Label lblBoardDXF;
        private System.Windows.Forms.Button btnBrowseDXF;
        private System.Windows.Forms.TextBox txtDXFInfo;
        private System.Windows.Forms.Label lblDXFInfo;
        private System.Windows.Forms.Button btnLoadDXFFile;
        private System.Windows.Forms.Button btnPreviewDXF;
        private System.Windows.Forms.Button btnClearDXF;
        private System.Windows.Forms.TabPage tabPageParams;
        private System.Windows.Forms.Label lblWorkspace;
        private System.Windows.Forms.TextBox txtWorkspace;
        private System.Windows.Forms.Label lblMargin;
        private System.Windows.Forms.TextBox txtMargin;
    }
}
