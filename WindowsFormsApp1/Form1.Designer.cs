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
            this.timerPreview = new System.Windows.Forms.Timer(this.components);
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblDXFInfo = new System.Windows.Forms.Label();
            this.txtDXFInfo = new System.Windows.Forms.TextBox();
            this.btnBrowseDXF = new System.Windows.Forms.Button();
            this.lblBoardDXF = new System.Windows.Forms.Label();
            this.comboBoardDXF = new System.Windows.Forms.ComboBox();
            this.lblDXFPath = new System.Windows.Forms.Label();
            this.txtDXFPath = new System.Windows.Forms.TextBox();
            this.btnMarkDXF = new System.Windows.Forms.Button();
            this.btnStopMarkDXF = new System.Windows.Forms.Button();
            this.btnPreviewDXF = new System.Windows.Forms.Button();
            this.btnStopPreview = new System.Windows.Forms.Button();
            this.btnClearDXF = new System.Windows.Forms.Button();
            this.btnLoadDXFFile = new System.Windows.Forms.Button();
            this.btnLoadDXF = new System.Windows.Forms.Button();
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
            this.tabPageLaserPower = new System.Windows.Forms.TabPage();
            this.lblBoardLaser = new System.Windows.Forms.Label();
            this.comboBoardLaser = new System.Windows.Forms.ComboBox();
            this.groupBoxLaserParams = new System.Windows.Forms.GroupBox();
            this.lblPower = new System.Windows.Forms.Label();
            this.trkPower = new System.Windows.Forms.TrackBar();
            this.numPower = new System.Windows.Forms.NumericUpDown();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.txtFrequency = new System.Windows.Forms.TextBox();
            this.lblPulseWidth = new System.Windows.Forms.Label();
            this.txtPulseWidth = new System.Windows.Forms.TextBox();
            this.lblMarkRepeat = new System.Windows.Forms.Label();
            this.numMarkRepeat = new System.Windows.Forms.NumericUpDown();
            this.chkWobble = new System.Windows.Forms.CheckBox();
            this.lblWobbleWidth = new System.Windows.Forms.Label();
            this.txtWobbleWidth = new System.Windows.Forms.TextBox();
            this.lblWobbleOverlap = new System.Windows.Forms.Label();
            this.txtWobbleOverlap = new System.Windows.Forms.TextBox();
            this.lblWobbleSpeed = new System.Windows.Forms.Label();
            this.txtWobbleSpeed = new System.Windows.Forms.TextBox();
            this.btnApplyLaser = new System.Windows.Forms.Button();
            this.btnReadLaser = new System.Windows.Forms.Button();
            this.txtLaserStatus = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.tabPageQRCode = new System.Windows.Forms.TabPage();
            this.lblBoardQR = new System.Windows.Forms.Label();
            this.comboBoardQR = new System.Windows.Forms.ComboBox();
            this.groupBoxQRBasic = new System.Windows.Forms.GroupBox();
            this.lblQRContent = new System.Windows.Forms.Label();
            this.txtQRContent = new System.Windows.Forms.TextBox();
            this.lblQRPosX = new System.Windows.Forms.Label();
            this.txtQRPosX = new System.Windows.Forms.TextBox();
            this.lblQRPosY = new System.Windows.Forms.Label();
            this.txtQRPosY = new System.Windows.Forms.TextBox();
            this.lblQRWidth = new System.Windows.Forms.Label();
            this.txtQRWidth = new System.Windows.Forms.TextBox();
            this.lblQRHeight = new System.Windows.Forms.Label();
            this.txtQRHeight = new System.Windows.Forms.TextBox();
            this.chkQRInvert = new System.Windows.Forms.CheckBox();
            this.btnLoadQR = new System.Windows.Forms.Button();
            this.btnMarkQR = new System.Windows.Forms.Button();
            this.btnStopMarkQR = new System.Windows.Forms.Button();
            this.btnPreviewQR = new System.Windows.Forms.Button();
            this.btnStopPreviewQR = new System.Windows.Forms.Button();
            this.btnClearQR = new System.Windows.Forms.Button();
            this.txtQRStatus = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBoardCount)).BeginInit();
            this.tabPageParams.SuspendLayout();
            this.groupBoxIP.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPageDraw.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPageLaserPower.SuspendLayout();
            this.groupBoxLaserParams.SuspendLayout();
            this.tabPageQRCode.SuspendLayout();
            this.groupBoxQRBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarkRepeat)).BeginInit();
            this.SuspendLayout();
            // 
            // timerMark
            // 
            this.timerMark.Interval = 200;
            this.timerMark.Tick += new System.EventHandler(this.timerMark_Tick);
            //
            // timerPreview
            //
            this.timerPreview.Interval = 15000;
            this.timerPreview.Tick += new System.EventHandler(this.timerPreview_Tick);
            //
            // panelBoard1
            // 
            this.panelBoard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard1.Location = new System.Drawing.Point(20, 38);
            this.panelBoard1.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard1.Name = "panelBoard1";
            this.panelBoard1.Size = new System.Drawing.Size(428, 313);
            this.panelBoard1.TabIndex = 0;
            // 
            // panelBoard2
            // 
            this.panelBoard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard2.Location = new System.Drawing.Point(20, 38);
            this.panelBoard2.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard2.Name = "panelBoard2";
            this.panelBoard2.Size = new System.Drawing.Size(428, 313);
            this.panelBoard2.TabIndex = 0;
            // 
            // panelBoard3
            // 
            this.panelBoard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard3.Location = new System.Drawing.Point(20, 38);
            this.panelBoard3.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard3.Name = "panelBoard3";
            this.panelBoard3.Size = new System.Drawing.Size(428, 313);
            this.panelBoard3.TabIndex = 0;
            // 
            // panelBoard4
            // 
            this.panelBoard4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard4.Location = new System.Drawing.Point(20, 38);
            this.panelBoard4.Margin = new System.Windows.Forms.Padding(4);
            this.panelBoard4.Name = "panelBoard4";
            this.panelBoard4.Size = new System.Drawing.Size(428, 313);
            this.panelBoard4.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelBoard1);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(469, 369);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "晶片板 1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelBoard2);
            this.groupBox2.Location = new System.Drawing.Point(493, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(469, 369);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "晶片板 2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panelBoard3);
            this.groupBox3.Location = new System.Drawing.Point(16, 392);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(469, 369);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "晶片板 3";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panelBoard4);
            this.groupBox4.Location = new System.Drawing.Point(493, 392);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(469, 369);
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
            this.tabControl1.Controls.Add(this.tabPageLaserPower);
            this.tabControl1.Controls.Add(this.tabPageQRCode);
            this.tabControl1.Location = new System.Drawing.Point(970, 15);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(460, 790);
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
            this.tabPageConnect.Size = new System.Drawing.Size(452, 761);
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
            this.tabPageParams.Size = new System.Drawing.Size(452, 761);
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
            this.tabPage1.Controls.Add(this.btnStopMarkDXF);
            this.tabPage1.Controls.Add(this.btnPreviewDXF);
            this.tabPage1.Controls.Add(this.btnStopPreview);
            this.tabPage1.Controls.Add(this.btnClearDXF);
            this.tabPage1.Controls.Add(this.btnLoadDXFFile);
            this.tabPage1.Controls.Add(this.btnLoadDXF);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(452, 761);
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
            this.btnMarkDXF.Size = new System.Drawing.Size(90, 35);
            this.btnMarkDXF.TabIndex = 1;
            this.btnMarkDXF.Text = "打標";
            this.btnMarkDXF.UseVisualStyleBackColor = true;
            this.btnMarkDXF.Click += new System.EventHandler(this.btnMarkDXF_Click);
            //
            // btnStopMarkDXF
            //
            this.btnStopMarkDXF.Enabled = false;
            this.btnStopMarkDXF.Location = new System.Drawing.Point(118, 278);
            this.btnStopMarkDXF.Name = "btnStopMarkDXF";
            this.btnStopMarkDXF.Size = new System.Drawing.Size(93, 35);
            this.btnStopMarkDXF.TabIndex = 13;
            this.btnStopMarkDXF.Text = "停止打標";
            this.btnStopMarkDXF.UseVisualStyleBackColor = true;
            this.btnStopMarkDXF.Click += new System.EventHandler(this.btnStopMarkDXF_Click);
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
            // btnStopPreview
            //
            this.btnStopPreview.Location = new System.Drawing.Point(20, 355);
            this.btnStopPreview.Name = "btnStopPreview";
            this.btnStopPreview.Size = new System.Drawing.Size(95, 30);
            this.btnStopPreview.TabIndex = 12;
            this.btnStopPreview.Text = "停止預覽";
            this.btnStopPreview.UseVisualStyleBackColor = true;
            this.btnStopPreview.Enabled = false;
            this.btnStopPreview.Click += new System.EventHandler(this.btnStopPreview_Click);
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
            this.tabPageDraw.Size = new System.Drawing.Size(452, 761);
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
            // tabPageLaserPower
            // 
            this.tabPageLaserPower.Controls.Add(this.lblBoardLaser);
            this.tabPageLaserPower.Controls.Add(this.comboBoardLaser);
            this.tabPageLaserPower.Controls.Add(this.groupBoxLaserParams);
            this.tabPageLaserPower.Controls.Add(this.btnApplyLaser);
            this.tabPageLaserPower.Controls.Add(this.btnReadLaser);
            this.tabPageLaserPower.Controls.Add(this.txtLaserStatus);
            this.tabPageLaserPower.Location = new System.Drawing.Point(4, 25);
            this.tabPageLaserPower.Name = "tabPageLaserPower";
            this.tabPageLaserPower.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLaserPower.Size = new System.Drawing.Size(452, 761);
            this.tabPageLaserPower.TabIndex = 4;
            this.tabPageLaserPower.Text = "5. 雷射功率";
            this.tabPageLaserPower.UseVisualStyleBackColor = true;
            // 
            // lblBoardLaser
            // 
            this.lblBoardLaser.AutoSize = true;
            this.lblBoardLaser.Location = new System.Drawing.Point(15, 15);
            this.lblBoardLaser.Name = "lblBoardLaser";
            this.lblBoardLaser.Size = new System.Drawing.Size(67, 15);
            this.lblBoardLaser.TabIndex = 0;
            this.lblBoardLaser.Text = "選擇板：";
            // 
            // comboBoardLaser
            // 
            this.comboBoardLaser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoardLaser.FormattingEnabled = true;
            this.comboBoardLaser.Items.AddRange(new object[] {
            "板 1",
            "板 2",
            "板 3",
            "板 4"});
            this.comboBoardLaser.Location = new System.Drawing.Point(85, 12);
            this.comboBoardLaser.Name = "comboBoardLaser";
            this.comboBoardLaser.Size = new System.Drawing.Size(121, 23);
            this.comboBoardLaser.TabIndex = 1;
            // 
            // groupBoxLaserParams
            // 
            this.groupBoxLaserParams.Controls.Add(this.lblPower);
            this.groupBoxLaserParams.Controls.Add(this.trkPower);
            this.groupBoxLaserParams.Controls.Add(this.numPower);
            this.groupBoxLaserParams.Controls.Add(this.lblSpeed);
            this.groupBoxLaserParams.Controls.Add(this.txtSpeed);
            this.groupBoxLaserParams.Controls.Add(this.lblFrequency);
            this.groupBoxLaserParams.Controls.Add(this.txtFrequency);
            this.groupBoxLaserParams.Controls.Add(this.lblPulseWidth);
            this.groupBoxLaserParams.Controls.Add(this.txtPulseWidth);
            this.groupBoxLaserParams.Controls.Add(this.lblMarkRepeat);
            this.groupBoxLaserParams.Controls.Add(this.numMarkRepeat);
            this.groupBoxLaserParams.Controls.Add(this.chkWobble);
            this.groupBoxLaserParams.Controls.Add(this.lblWobbleWidth);
            this.groupBoxLaserParams.Controls.Add(this.txtWobbleWidth);
            this.groupBoxLaserParams.Controls.Add(this.lblWobbleOverlap);
            this.groupBoxLaserParams.Controls.Add(this.txtWobbleOverlap);
            this.groupBoxLaserParams.Controls.Add(this.lblWobbleSpeed);
            this.groupBoxLaserParams.Controls.Add(this.txtWobbleSpeed);
            this.groupBoxLaserParams.Location = new System.Drawing.Point(8, 45);
            this.groupBoxLaserParams.Name = "groupBoxLaserParams";
            this.groupBoxLaserParams.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxLaserParams.Size = new System.Drawing.Size(213, 400);
            this.groupBoxLaserParams.TabIndex = 2;
            this.groupBoxLaserParams.TabStop = false;
            this.groupBoxLaserParams.Text = "雷射參數設定";
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(10, 25);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(67, 15);
            this.lblPower.TabIndex = 0;
            this.lblPower.Text = "功率 (%):";
            // 
            // trkPower
            // 
            this.trkPower.Location = new System.Drawing.Point(10, 48);
            this.trkPower.Maximum = 100;
            this.trkPower.Name = "trkPower";
            this.trkPower.Size = new System.Drawing.Size(120, 56);
            this.trkPower.TabIndex = 1;
            this.trkPower.TickFrequency = 10;
            this.trkPower.Value = 50;
            this.trkPower.Scroll += new System.EventHandler(this.trkPower_Scroll);
            // 
            // numPower
            // 
            this.numPower.DecimalPlaces = 1;
            this.numPower.Location = new System.Drawing.Point(140, 48);
            this.numPower.Name = "numPower";
            this.numPower.Size = new System.Drawing.Size(65, 25);
            this.numPower.TabIndex = 2;
            this.numPower.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numPower.ValueChanged += new System.EventHandler(this.numPower_ValueChanged);
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(10, 105);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(86, 15);
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "速度 (mm/s):";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(110, 102);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(95, 25);
            this.txtSpeed.TabIndex = 4;
            this.txtSpeed.Text = "1000";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(10, 145);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(78, 15);
            this.lblFrequency.TabIndex = 5;
            this.lblFrequency.Text = "頻率 (kHz):";
            // 
            // txtFrequency
            // 
            this.txtFrequency.Location = new System.Drawing.Point(110, 142);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.Size = new System.Drawing.Size(95, 25);
            this.txtFrequency.TabIndex = 6;
            this.txtFrequency.Text = "30";
            // 
            // lblPulseWidth
            // 
            this.lblPulseWidth.AutoSize = true;
            this.lblPulseWidth.Location = new System.Drawing.Point(10, 185);
            this.lblPulseWidth.Name = "lblPulseWidth";
            this.lblPulseWidth.Size = new System.Drawing.Size(71, 15);
            this.lblPulseWidth.TabIndex = 7;
            this.lblPulseWidth.Text = "脈波寬度:";
            // 
            // txtPulseWidth
            // 
            this.txtPulseWidth.Location = new System.Drawing.Point(110, 182);
            this.txtPulseWidth.Name = "txtPulseWidth";
            this.txtPulseWidth.Size = new System.Drawing.Size(95, 25);
            this.txtPulseWidth.TabIndex = 8;
            this.txtPulseWidth.Text = "5";
            // 
            // lblMarkRepeat
            // 
            this.lblMarkRepeat.AutoSize = true;
            this.lblMarkRepeat.Location = new System.Drawing.Point(10, 225);
            this.lblMarkRepeat.Name = "lblMarkRepeat";
            this.lblMarkRepeat.Size = new System.Drawing.Size(71, 15);
            this.lblMarkRepeat.TabIndex = 9;
            this.lblMarkRepeat.Text = "雷射次數:";
            // 
            // numMarkRepeat
            // 
            this.numMarkRepeat.Location = new System.Drawing.Point(110, 222);
            this.numMarkRepeat.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numMarkRepeat.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMarkRepeat.Name = "numMarkRepeat";
            this.numMarkRepeat.Size = new System.Drawing.Size(95, 25);
            this.numMarkRepeat.TabIndex = 10;
            this.numMarkRepeat.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // chkWobble
            //
            this.chkWobble.AutoSize = true;
            this.chkWobble.Location = new System.Drawing.Point(10, 265);
            this.chkWobble.Name = "chkWobble";
            this.chkWobble.Size = new System.Drawing.Size(74, 19);
            this.chkWobble.TabIndex = 11;
            this.chkWobble.Text = "擺動啟動";
            this.chkWobble.UseVisualStyleBackColor = true;
            this.chkWobble.CheckedChanged += new System.EventHandler(this.chkWobble_CheckedChanged);
            //
            // lblWobbleWidth
            //
            this.lblWobbleWidth.AutoSize = true;
            this.lblWobbleWidth.Location = new System.Drawing.Point(10, 298);
            this.lblWobbleWidth.Name = "lblWobbleWidth";
            this.lblWobbleWidth.Size = new System.Drawing.Size(71, 15);
            this.lblWobbleWidth.TabIndex = 12;
            this.lblWobbleWidth.Text = "擺動寬度:";
            this.lblWobbleWidth.Enabled = false;
            //
            // txtWobbleWidth
            //
            this.txtWobbleWidth.Location = new System.Drawing.Point(110, 295);
            this.txtWobbleWidth.Name = "txtWobbleWidth";
            this.txtWobbleWidth.Size = new System.Drawing.Size(95, 25);
            this.txtWobbleWidth.TabIndex = 13;
            this.txtWobbleWidth.Text = "0.1";
            this.txtWobbleWidth.Enabled = false;
            //
            // lblWobbleOverlap
            //
            this.lblWobbleOverlap.AutoSize = true;
            this.lblWobbleOverlap.Location = new System.Drawing.Point(10, 328);
            this.lblWobbleOverlap.Name = "lblWobbleOverlap";
            this.lblWobbleOverlap.Size = new System.Drawing.Size(71, 15);
            this.lblWobbleOverlap.TabIndex = 14;
            this.lblWobbleOverlap.Text = "重疊率:";
            this.lblWobbleOverlap.Enabled = false;
            //
            // txtWobbleOverlap
            //
            this.txtWobbleOverlap.Location = new System.Drawing.Point(110, 325);
            this.txtWobbleOverlap.Name = "txtWobbleOverlap";
            this.txtWobbleOverlap.Size = new System.Drawing.Size(95, 25);
            this.txtWobbleOverlap.TabIndex = 15;
            this.txtWobbleOverlap.Text = "50.000";
            this.txtWobbleOverlap.Enabled = false;
            //
            // lblWobbleSpeed
            //
            this.lblWobbleSpeed.AutoSize = true;
            this.lblWobbleSpeed.Location = new System.Drawing.Point(10, 358);
            this.lblWobbleSpeed.Name = "lblWobbleSpeed";
            this.lblWobbleSpeed.Size = new System.Drawing.Size(71, 15);
            this.lblWobbleSpeed.TabIndex = 16;
            this.lblWobbleSpeed.Text = "擺動速度:";
            this.lblWobbleSpeed.Enabled = false;
            //
            // txtWobbleSpeed
            //
            this.txtWobbleSpeed.Location = new System.Drawing.Point(110, 355);
            this.txtWobbleSpeed.Name = "txtWobbleSpeed";
            this.txtWobbleSpeed.Size = new System.Drawing.Size(95, 25);
            this.txtWobbleSpeed.TabIndex = 17;
            this.txtWobbleSpeed.Text = "5026.55";
            this.txtWobbleSpeed.Enabled = false;
            //
            // btnApplyLaser
            // 
            this.btnApplyLaser.Location = new System.Drawing.Point(15, 455);
            this.btnApplyLaser.Name = "btnApplyLaser";
            this.btnApplyLaser.Size = new System.Drawing.Size(100, 35);
            this.btnApplyLaser.TabIndex = 3;
            this.btnApplyLaser.Text = "套用參數";
            this.btnApplyLaser.UseVisualStyleBackColor = true;
            this.btnApplyLaser.Click += new System.EventHandler(this.btnApplyLaser_Click);
            // 
            // btnReadLaser
            // 
            this.btnReadLaser.Location = new System.Drawing.Point(120, 455);
            this.btnReadLaser.Name = "btnReadLaser";
            this.btnReadLaser.Size = new System.Drawing.Size(100, 35);
            this.btnReadLaser.TabIndex = 4;
            this.btnReadLaser.Text = "讀取參數";
            this.btnReadLaser.UseVisualStyleBackColor = true;
            this.btnReadLaser.Click += new System.EventHandler(this.btnReadLaser_Click);
            // 
            // txtLaserStatus
            // 
            this.txtLaserStatus.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLaserStatus.Location = new System.Drawing.Point(15, 500);
            this.txtLaserStatus.Multiline = true;
            this.txtLaserStatus.Name = "txtLaserStatus";
            this.txtLaserStatus.ReadOnly = true;
            this.txtLaserStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLaserStatus.Size = new System.Drawing.Size(205, 240);
            this.txtLaserStatus.TabIndex = 5;
            //
            // tabPageQRCode
            //
            this.tabPageQRCode.Controls.Add(this.lblBoardQR);
            this.tabPageQRCode.Controls.Add(this.comboBoardQR);
            this.tabPageQRCode.Controls.Add(this.groupBoxQRBasic);
            this.tabPageQRCode.Controls.Add(this.chkQRInvert);
            this.tabPageQRCode.Controls.Add(this.btnLoadQR);
            this.tabPageQRCode.Controls.Add(this.btnMarkQR);
            this.tabPageQRCode.Controls.Add(this.btnStopMarkQR);
            this.tabPageQRCode.Controls.Add(this.btnPreviewQR);
            this.tabPageQRCode.Controls.Add(this.btnStopPreviewQR);
            this.tabPageQRCode.Controls.Add(this.btnClearQR);
            this.tabPageQRCode.Controls.Add(this.txtQRStatus);
            this.tabPageQRCode.Location = new System.Drawing.Point(4, 25);
            this.tabPageQRCode.Name = "tabPageQRCode";
            this.tabPageQRCode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQRCode.Size = new System.Drawing.Size(452, 761);
            this.tabPageQRCode.TabIndex = 5;
            this.tabPageQRCode.Text = "6. QR Code";
            this.tabPageQRCode.UseVisualStyleBackColor = true;
            //
            // lblBoardQR
            //
            this.lblBoardQR.AutoSize = true;
            this.lblBoardQR.Location = new System.Drawing.Point(15, 15);
            this.lblBoardQR.Name = "lblBoardQR";
            this.lblBoardQR.Size = new System.Drawing.Size(67, 15);
            this.lblBoardQR.TabIndex = 0;
            this.lblBoardQR.Text = "選擇板：";
            //
            // comboBoardQR
            //
            this.comboBoardQR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoardQR.FormattingEnabled = true;
            this.comboBoardQR.Items.AddRange(new object[] {
            "板 1",
            "板 2",
            "板 3",
            "板 4"});
            this.comboBoardQR.Location = new System.Drawing.Point(85, 12);
            this.comboBoardQR.Name = "comboBoardQR";
            this.comboBoardQR.Size = new System.Drawing.Size(121, 23);
            this.comboBoardQR.TabIndex = 1;
            //
            // groupBoxQRBasic
            //
            this.groupBoxQRBasic.Controls.Add(this.lblQRContent);
            this.groupBoxQRBasic.Controls.Add(this.txtQRContent);
            this.groupBoxQRBasic.Controls.Add(this.lblQRPosX);
            this.groupBoxQRBasic.Controls.Add(this.txtQRPosX);
            this.groupBoxQRBasic.Controls.Add(this.lblQRPosY);
            this.groupBoxQRBasic.Controls.Add(this.txtQRPosY);
            this.groupBoxQRBasic.Controls.Add(this.lblQRWidth);
            this.groupBoxQRBasic.Controls.Add(this.txtQRWidth);
            this.groupBoxQRBasic.Controls.Add(this.lblQRHeight);
            this.groupBoxQRBasic.Controls.Add(this.txtQRHeight);
            this.groupBoxQRBasic.Location = new System.Drawing.Point(8, 42);
            this.groupBoxQRBasic.Name = "groupBoxQRBasic";
            this.groupBoxQRBasic.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxQRBasic.Size = new System.Drawing.Size(430, 245);
            this.groupBoxQRBasic.TabIndex = 2;
            this.groupBoxQRBasic.TabStop = false;
            this.groupBoxQRBasic.Text = "QR Code 基本設定";
            //
            // lblQRContent
            //
            this.lblQRContent.AutoSize = true;
            this.lblQRContent.Location = new System.Drawing.Point(10, 25);
            this.lblQRContent.Name = "lblQRContent";
            this.lblQRContent.Size = new System.Drawing.Size(71, 15);
            this.lblQRContent.TabIndex = 0;
            this.lblQRContent.Text = "QR 內容：";
            //
            // txtQRContent
            //
            this.txtQRContent.Location = new System.Drawing.Point(10, 45);
            this.txtQRContent.Multiline = true;
            this.txtQRContent.Name = "txtQRContent";
            this.txtQRContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQRContent.Size = new System.Drawing.Size(410, 60);
            this.txtQRContent.TabIndex = 1;
            this.txtQRContent.Text = "Hello World";
            //
            // lblQRPosX
            //
            this.lblQRPosX.AutoSize = true;
            this.lblQRPosX.Location = new System.Drawing.Point(10, 118);
            this.lblQRPosX.Name = "lblQRPosX";
            this.lblQRPosX.Size = new System.Drawing.Size(78, 15);
            this.lblQRPosX.TabIndex = 2;
            this.lblQRPosX.Text = "X 位置(mm):";
            //
            // txtQRPosX
            //
            this.txtQRPosX.Location = new System.Drawing.Point(100, 115);
            this.txtQRPosX.Name = "txtQRPosX";
            this.txtQRPosX.Size = new System.Drawing.Size(80, 25);
            this.txtQRPosX.TabIndex = 3;
            this.txtQRPosX.Text = "0";
            //
            // lblQRPosY
            //
            this.lblQRPosY.AutoSize = true;
            this.lblQRPosY.Location = new System.Drawing.Point(200, 118);
            this.lblQRPosY.Name = "lblQRPosY";
            this.lblQRPosY.Size = new System.Drawing.Size(77, 15);
            this.lblQRPosY.TabIndex = 4;
            this.lblQRPosY.Text = "Y 位置(mm):";
            //
            // txtQRPosY
            //
            this.txtQRPosY.Location = new System.Drawing.Point(290, 115);
            this.txtQRPosY.Name = "txtQRPosY";
            this.txtQRPosY.Size = new System.Drawing.Size(80, 25);
            this.txtQRPosY.TabIndex = 5;
            this.txtQRPosY.Text = "0";
            //
            // lblQRWidth
            //
            this.lblQRWidth.AutoSize = true;
            this.lblQRWidth.Location = new System.Drawing.Point(10, 153);
            this.lblQRWidth.Name = "lblQRWidth";
            this.lblQRWidth.Size = new System.Drawing.Size(66, 15);
            this.lblQRWidth.TabIndex = 6;
            this.lblQRWidth.Text = "寬度(mm):";
            //
            // txtQRWidth
            //
            this.txtQRWidth.Location = new System.Drawing.Point(100, 150);
            this.txtQRWidth.Name = "txtQRWidth";
            this.txtQRWidth.Size = new System.Drawing.Size(80, 25);
            this.txtQRWidth.TabIndex = 7;
            this.txtQRWidth.Text = "10";
            //
            // lblQRHeight
            //
            this.lblQRHeight.AutoSize = true;
            this.lblQRHeight.Location = new System.Drawing.Point(200, 153);
            this.lblQRHeight.Name = "lblQRHeight";
            this.lblQRHeight.Size = new System.Drawing.Size(66, 15);
            this.lblQRHeight.TabIndex = 8;
            this.lblQRHeight.Text = "高度(mm):";
            //
            // txtQRHeight
            //
            this.txtQRHeight.Location = new System.Drawing.Point(290, 150);
            this.txtQRHeight.Name = "txtQRHeight";
            this.txtQRHeight.Size = new System.Drawing.Size(80, 25);
            this.txtQRHeight.TabIndex = 9;
            this.txtQRHeight.Text = "10";
            //
            // chkQRInvert
            //
            this.chkQRInvert.AutoSize = true;
            this.chkQRInvert.Location = new System.Drawing.Point(15, 295);
            this.chkQRInvert.Name = "chkQRInvert";
            this.chkQRInvert.Size = new System.Drawing.Size(100, 19);
            this.chkQRInvert.TabIndex = 3;
            this.chkQRInvert.Text = "反轉黑白";
            this.chkQRInvert.UseVisualStyleBackColor = true;
            //
            // btnLoadQR
            //
            this.btnLoadQR.Location = new System.Drawing.Point(15, 325);
            this.btnLoadQR.Name = "btnLoadQR";
            this.btnLoadQR.Size = new System.Drawing.Size(200, 35);
            this.btnLoadQR.TabIndex = 4;
            this.btnLoadQR.Text = "載入 QR Code";
            this.btnLoadQR.UseVisualStyleBackColor = true;
            this.btnLoadQR.Click += new System.EventHandler(this.btnLoadQR_Click);
            //
            // btnMarkQR
            //
            this.btnMarkQR.Enabled = false;
            this.btnMarkQR.Location = new System.Drawing.Point(15, 370);
            this.btnMarkQR.Name = "btnMarkQR";
            this.btnMarkQR.Size = new System.Drawing.Size(95, 35);
            this.btnMarkQR.TabIndex = 5;
            this.btnMarkQR.Text = "打標";
            this.btnMarkQR.UseVisualStyleBackColor = true;
            this.btnMarkQR.Click += new System.EventHandler(this.btnMarkQR_Click);
            //
            // btnStopMarkQR
            //
            this.btnStopMarkQR.Enabled = false;
            this.btnStopMarkQR.Location = new System.Drawing.Point(120, 370);
            this.btnStopMarkQR.Name = "btnStopMarkQR";
            this.btnStopMarkQR.Size = new System.Drawing.Size(95, 35);
            this.btnStopMarkQR.TabIndex = 6;
            this.btnStopMarkQR.Text = "停止打標";
            this.btnStopMarkQR.UseVisualStyleBackColor = true;
            this.btnStopMarkQR.Click += new System.EventHandler(this.btnStopMarkQR_Click);
            //
            // btnPreviewQR
            //
            this.btnPreviewQR.Location = new System.Drawing.Point(15, 413);
            this.btnPreviewQR.Name = "btnPreviewQR";
            this.btnPreviewQR.Size = new System.Drawing.Size(95, 30);
            this.btnPreviewQR.TabIndex = 7;
            this.btnPreviewQR.Text = "紅光預覽";
            this.btnPreviewQR.UseVisualStyleBackColor = true;
            this.btnPreviewQR.Click += new System.EventHandler(this.btnPreviewQR_Click);
            //
            // btnStopPreviewQR
            //
            this.btnStopPreviewQR.Enabled = false;
            this.btnStopPreviewQR.Location = new System.Drawing.Point(120, 413);
            this.btnStopPreviewQR.Name = "btnStopPreviewQR";
            this.btnStopPreviewQR.Size = new System.Drawing.Size(95, 30);
            this.btnStopPreviewQR.TabIndex = 8;
            this.btnStopPreviewQR.Text = "停止預覽";
            this.btnStopPreviewQR.UseVisualStyleBackColor = true;
            this.btnStopPreviewQR.Click += new System.EventHandler(this.btnStopPreviewQR_Click);
            //
            // btnClearQR
            //
            this.btnClearQR.Location = new System.Drawing.Point(225, 370);
            this.btnClearQR.Name = "btnClearQR";
            this.btnClearQR.Size = new System.Drawing.Size(95, 35);
            this.btnClearQR.TabIndex = 9;
            this.btnClearQR.Text = "清除畫面";
            this.btnClearQR.UseVisualStyleBackColor = true;
            this.btnClearQR.Click += new System.EventHandler(this.btnClearQR_Click);
            //
            // txtQRStatus
            //
            this.txtQRStatus.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtQRStatus.Location = new System.Drawing.Point(15, 455);
            this.txtQRStatus.Multiline = true;
            this.txtQRStatus.Name = "txtQRStatus";
            this.txtQRStatus.ReadOnly = true;
            this.txtQRStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQRStatus.Size = new System.Drawing.Size(420, 100);
            this.txtQRStatus.TabIndex = 10;
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
            this.tabPageParams.ResumeLayout(false);
            this.tabPageParams.PerformLayout();
            this.groupBoxIP.ResumeLayout(false);
            this.groupBoxIP.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPageDraw.ResumeLayout(false);
            this.tabPageDraw.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPageLaserPower.ResumeLayout(false);
            this.tabPageLaserPower.PerformLayout();
            this.groupBoxLaserParams.ResumeLayout(false);
            this.groupBoxLaserParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarkRepeat)).EndInit();
            this.tabPageQRCode.ResumeLayout(false);
            this.tabPageQRCode.PerformLayout();
            this.groupBoxQRBasic.ResumeLayout(false);
            this.groupBoxQRBasic.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Timer timerPreview;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnLoadDXF;
        private System.Windows.Forms.Button btnMarkDXF;
        private System.Windows.Forms.Button btnStopMarkDXF;
        private System.Windows.Forms.TextBox txtDXFPath;
        private System.Windows.Forms.Label lblDXFPath;
        private System.Windows.Forms.ComboBox comboBoardDXF;
        private System.Windows.Forms.Label lblBoardDXF;
        private System.Windows.Forms.Button btnBrowseDXF;
        private System.Windows.Forms.TextBox txtDXFInfo;
        private System.Windows.Forms.Label lblDXFInfo;
        private System.Windows.Forms.Button btnLoadDXFFile;
        private System.Windows.Forms.Button btnPreviewDXF;
        private System.Windows.Forms.Button btnStopPreview;
        private System.Windows.Forms.Button btnClearDXF;
        private System.Windows.Forms.TabPage tabPageParams;
        private System.Windows.Forms.Label lblWorkspace;
        private System.Windows.Forms.TextBox txtWorkspace;
        private System.Windows.Forms.Label lblMargin;
        private System.Windows.Forms.TextBox txtMargin;
        private System.Windows.Forms.TabPage tabPageLaserPower;
        private System.Windows.Forms.Label lblBoardLaser;
        private System.Windows.Forms.ComboBox comboBoardLaser;
        private System.Windows.Forms.GroupBox groupBoxLaserParams;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.TrackBar trkPower;
        private System.Windows.Forms.NumericUpDown numPower;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.TextBox txtFrequency;
        private System.Windows.Forms.Button btnApplyLaser;
        private System.Windows.Forms.Button btnReadLaser;
        private System.Windows.Forms.TextBox txtLaserStatus;
        private System.Windows.Forms.Label lblPulseWidth;
        private System.Windows.Forms.TextBox txtPulseWidth;
        private System.Windows.Forms.Label lblMarkRepeat;
        private System.Windows.Forms.NumericUpDown numMarkRepeat;
        private System.Windows.Forms.CheckBox chkWobble;
        private System.Windows.Forms.Label lblWobbleWidth;
        private System.Windows.Forms.TextBox txtWobbleWidth;
        private System.Windows.Forms.Label lblWobbleOverlap;
        private System.Windows.Forms.TextBox txtWobbleOverlap;
        private System.Windows.Forms.Label lblWobbleSpeed;
        private System.Windows.Forms.TextBox txtWobbleSpeed;
        private System.Windows.Forms.TabPage tabPageQRCode;
        private System.Windows.Forms.Label lblBoardQR;
        private System.Windows.Forms.ComboBox comboBoardQR;
        private System.Windows.Forms.GroupBox groupBoxQRBasic;
        private System.Windows.Forms.Label lblQRContent;
        private System.Windows.Forms.TextBox txtQRContent;
        private System.Windows.Forms.Label lblQRPosX;
        private System.Windows.Forms.TextBox txtQRPosX;
        private System.Windows.Forms.Label lblQRPosY;
        private System.Windows.Forms.TextBox txtQRPosY;
        private System.Windows.Forms.Label lblQRWidth;
        private System.Windows.Forms.TextBox txtQRWidth;
        private System.Windows.Forms.Label lblQRHeight;
        private System.Windows.Forms.TextBox txtQRHeight;
        private System.Windows.Forms.CheckBox chkQRInvert;
        private System.Windows.Forms.Button btnLoadQR;
        private System.Windows.Forms.Button btnMarkQR;
        private System.Windows.Forms.Button btnStopMarkQR;
        private System.Windows.Forms.Button btnPreviewQR;
        private System.Windows.Forms.Button btnStopPreviewQR;
        private System.Windows.Forms.Button btnClearQR;
        private System.Windows.Forms.TextBox txtQRStatus;
    }
}
