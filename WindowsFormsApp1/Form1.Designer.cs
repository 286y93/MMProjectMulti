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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timerMark = new System.Windows.Forms.Timer(this.components);
            this.timerPreview = new System.Windows.Forms.Timer(this.components);
            this.timerParallelTest = new System.Windows.Forms.Timer(this.components);
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
            this.btnExit = new System.Windows.Forms.Button();
            this.tabPageParams = new System.Windows.Forms.TabPage();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.txtWorkspace = new System.Windows.Forms.TextBox();
            this.lblWorkspaceHeight = new System.Windows.Forms.Label();
            this.txtWorkspaceHeight = new System.Windows.Forms.TextBox();
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
            this.btnPreviewManual = new System.Windows.Forms.Button();
            this.btnStopPreviewManual = new System.Windows.Forms.Button();
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
            this.groupBoxQRWhiteBg = new System.Windows.Forms.GroupBox();
            this.lblWBRectSpeed = new System.Windows.Forms.Label();
            this.txtWBRectSpeed = new System.Windows.Forms.TextBox();
            this.lblWBRectPower = new System.Windows.Forms.Label();
            this.txtWBRectPower = new System.Windows.Forms.TextBox();
            this.lblWBQRSpeed = new System.Windows.Forms.Label();
            this.txtWBQRSpeed = new System.Windows.Forms.TextBox();
            this.lblWBQRPower = new System.Windows.Forms.Label();
            this.txtWBQRPower = new System.Windows.Forms.TextBox();
            this.lblWBQRWidth = new System.Windows.Forms.Label();
            this.txtWBQRWidth = new System.Windows.Forms.TextBox();
            this.lblWBQRHeight = new System.Windows.Forms.Label();
            this.txtWBQRHeight = new System.Windows.Forms.TextBox();
            this.lblWBQuietZone = new System.Windows.Forms.Label();
            this.txtWBQuietZone = new System.Windows.Forms.TextBox();
            this.lblWBRectExtra = new System.Windows.Forms.Label();
            this.txtWBRectExtra = new System.Windows.Forms.TextBox();
            this.lblWBMarkTarget = new System.Windows.Forms.Label();
            this.rdoWBMarkQR = new System.Windows.Forms.RadioButton();
            this.rdoWBMarkRect = new System.Windows.Forms.RadioButton();
            this.rdoWBMarkAll = new System.Windows.Forms.RadioButton();
            this.btnQRWhiteBgCreate = new System.Windows.Forms.Button();
            this.btnQRWhiteBgPreview = new System.Windows.Forms.Button();
            this.btnQRWhiteBgStopPreview = new System.Windows.Forms.Button();
            this.btnQRWhiteBgMark = new System.Windows.Forms.Button();
            this.lblWBSerial = new System.Windows.Forms.Label();
            this.txtWBSerial = new System.Windows.Forms.TextBox();
            this.groupBoxQRSteel = new System.Windows.Forms.GroupBox();
            this.lblSteelQrWidth = new System.Windows.Forms.Label();
            this.txtSteelQrWidth = new System.Windows.Forms.TextBox();
            this.lblSteelQrHeight = new System.Windows.Forms.Label();
            this.txtSteelQrHeight = new System.Windows.Forms.TextBox();
            this.lblSteelBorder = new System.Windows.Forms.Label();
            this.txtSteelBorder = new System.Windows.Forms.TextBox();
            this.lblSteelRectExtra = new System.Windows.Forms.Label();
            this.txtSteelRectExtra = new System.Windows.Forms.TextBox();
            this.lblSteelECLevel = new System.Windows.Forms.Label();
            this.txtSteelECLevel = new System.Windows.Forms.TextBox();
            this.lblSteelMarkStyle = new System.Windows.Forms.Label();
            this.txtSteelMarkStyle = new System.Windows.Forms.TextBox();
            this.lblSteelSpotSize = new System.Windows.Forms.Label();
            this.txtSteelSpotSize = new System.Windows.Forms.TextBox();
            this.lblSteelQrRepeat = new System.Windows.Forms.Label();
            this.txtSteelQrRepeat = new System.Windows.Forms.TextBox();
            this.lblSteelRectPower = new System.Windows.Forms.Label();
            this.txtSteelRectPower = new System.Windows.Forms.TextBox();
            this.lblSteelRectSpeed = new System.Windows.Forms.Label();
            this.txtSteelRectSpeed = new System.Windows.Forms.TextBox();
            this.lblSteelRectFreq = new System.Windows.Forms.Label();
            this.txtSteelRectFreq = new System.Windows.Forms.TextBox();
            this.lblSteelRectRepeat = new System.Windows.Forms.Label();
            this.txtSteelRectRepeat = new System.Windows.Forms.TextBox();
            this.lblSteelQrPower = new System.Windows.Forms.Label();
            this.txtSteelQrPower = new System.Windows.Forms.TextBox();
            this.lblSteelQrSpeed = new System.Windows.Forms.Label();
            this.txtSteelQrSpeed = new System.Windows.Forms.TextBox();
            this.lblSteelQrFreq = new System.Windows.Forms.Label();
            this.txtSteelQrFreq = new System.Windows.Forms.TextBox();
            this.lblSteelQrPulseWidth = new System.Windows.Forms.Label();
            this.txtSteelQrPulseWidth = new System.Windows.Forms.TextBox();
            this.lblSteelSerial = new System.Windows.Forms.Label();
            this.txtSteelSerial = new System.Windows.Forms.TextBox();
            this.btnQRSteelPreview = new System.Windows.Forms.Button();
            this.btnQRSteelMark = new System.Windows.Forms.Button();
            this.btnQRSteelStopPreview = new System.Windows.Forms.Button();
            this.groupBoxSteelTime = new System.Windows.Forms.GroupBox();
            this.txtSteelTimeInfo = new System.Windows.Forms.TextBox();
            this.groupBoxRectAlone = new System.Windows.Forms.GroupBox();
            this.lblRAWidth = new System.Windows.Forms.Label();
            this.txtRAWidth = new System.Windows.Forms.TextBox();
            this.lblRAHeight = new System.Windows.Forms.Label();
            this.txtRAHeight = new System.Windows.Forms.TextBox();
            this.lblRAX = new System.Windows.Forms.Label();
            this.txtRAX = new System.Windows.Forms.TextBox();
            this.lblRAY = new System.Windows.Forms.Label();
            this.txtRAY = new System.Windows.Forms.TextBox();
            this.lblRASpeed = new System.Windows.Forms.Label();
            this.txtRASpeed = new System.Windows.Forms.TextBox();
            this.lblRAPower = new System.Windows.Forms.Label();
            this.txtRAPower = new System.Windows.Forms.TextBox();
            this.lblRAFreq = new System.Windows.Forms.Label();
            this.txtRAFreq = new System.Windows.Forms.TextBox();
            this.lblRARepeat = new System.Windows.Forms.Label();
            this.txtRARepeat = new System.Windows.Forms.TextBox();
            this.lblRAPulseWidth = new System.Windows.Forms.Label();
            this.txtRAPulseWidth = new System.Windows.Forms.TextBox();
            this.lblRAFillStyle = new System.Windows.Forms.Label();
            this.txtRAFillStyle = new System.Windows.Forms.TextBox();
            this.lblRAFrameLineType = new System.Windows.Forms.Label();
            this.txtRAFrameLineType = new System.Windows.Forms.TextBox();
            this.btnRAPreview = new System.Windows.Forms.Button();
            this.btnRAStopPreview = new System.Windows.Forms.Button();
            this.btnRAMark = new System.Windows.Forms.Button();
            this.groupBoxQRAlone = new System.Windows.Forms.GroupBox();
            this.lblQAContent = new System.Windows.Forms.Label();
            this.txtQAContent = new System.Windows.Forms.TextBox();
            this.lblQAWidth = new System.Windows.Forms.Label();
            this.txtQAWidth = new System.Windows.Forms.TextBox();
            this.lblQAHeight = new System.Windows.Forms.Label();
            this.txtQAHeight = new System.Windows.Forms.TextBox();
            this.lblQAX = new System.Windows.Forms.Label();
            this.txtQAX = new System.Windows.Forms.TextBox();
            this.lblQAY = new System.Windows.Forms.Label();
            this.txtQAY = new System.Windows.Forms.TextBox();
            this.lblQABorder = new System.Windows.Forms.Label();
            this.txtQABorder = new System.Windows.Forms.TextBox();
            this.lblQAECLevel = new System.Windows.Forms.Label();
            this.txtQAECLevel = new System.Windows.Forms.TextBox();
            this.lblQAMarkStyle = new System.Windows.Forms.Label();
            this.txtQAMarkStyle = new System.Windows.Forms.TextBox();
            this.chkQAInvert = new System.Windows.Forms.CheckBox();
            this.lblQASpeed = new System.Windows.Forms.Label();
            this.txtQASpeed = new System.Windows.Forms.TextBox();
            this.lblQAPower = new System.Windows.Forms.Label();
            this.txtQAPower = new System.Windows.Forms.TextBox();
            this.lblQAFreq = new System.Windows.Forms.Label();
            this.txtQAFreq = new System.Windows.Forms.TextBox();
            this.lblQARepeat = new System.Windows.Forms.Label();
            this.txtQARepeat = new System.Windows.Forms.TextBox();
            this.lblQAPulseWidth = new System.Windows.Forms.Label();
            this.txtQAPulseWidth = new System.Windows.Forms.TextBox();
            this.btnQAPreview = new System.Windows.Forms.Button();
            this.btnQAStopPreview = new System.Windows.Forms.Button();
            this.btnQAMark = new System.Windows.Forms.Button();
            this.tabPageQRCode2 = new System.Windows.Forms.TabPage();
            this.lblBoardQR2 = new System.Windows.Forms.Label();
            this.comboBoardQR2 = new System.Windows.Forms.ComboBox();
            this.groupBoxBlackRect = new System.Windows.Forms.GroupBox();
            this.lblBRWidth = new System.Windows.Forms.Label();
            this.txtBRWidth = new System.Windows.Forms.TextBox();
            this.lblBRHeight = new System.Windows.Forms.Label();
            this.txtBRHeight = new System.Windows.Forms.TextBox();
            this.lblBRSpeed = new System.Windows.Forms.Label();
            this.txtBRSpeed = new System.Windows.Forms.TextBox();
            this.lblBRPower = new System.Windows.Forms.Label();
            this.txtBRPower = new System.Windows.Forms.TextBox();
            this.lblBRFreq = new System.Windows.Forms.Label();
            this.txtBRFreq = new System.Windows.Forms.TextBox();
            this.lblBRRepeat = new System.Windows.Forms.Label();
            this.txtBRRepeat = new System.Windows.Forms.TextBox();
            this.lblBRSpotDelay = new System.Windows.Forms.Label();
            this.txtBRSpotDelay = new System.Windows.Forms.TextBox();
            this.lblBRPulseWidth = new System.Windows.Forms.Label();
            this.txtBRPulseWidth = new System.Windows.Forms.TextBox();
            this.lblBRFillPitch = new System.Windows.Forms.Label();
            this.txtBRFillPitch = new System.Windows.Forms.TextBox();
            this.lblBRFillRoundPitch = new System.Windows.Forms.Label();
            this.txtBRFillRoundPitch = new System.Windows.Forms.TextBox();
            this.lblBRFillTimes = new System.Windows.Forms.Label();
            this.txtBRFillTimes = new System.Windows.Forms.TextBox();
            this.lblBRFillStepAngle = new System.Windows.Forms.Label();
            this.txtBRFillStepAngle = new System.Windows.Forms.TextBox();
            this.btnBRPreview = new System.Windows.Forms.Button();
            this.btnBRStopPreview = new System.Windows.Forms.Button();
            this.btnBRMark = new System.Windows.Forms.Button();
            this.groupBoxWhiteRect = new System.Windows.Forms.GroupBox();
            this.lblWRWidth = new System.Windows.Forms.Label();
            this.txtWRWidth = new System.Windows.Forms.TextBox();
            this.lblWRHeight = new System.Windows.Forms.Label();
            this.txtWRHeight = new System.Windows.Forms.TextBox();
            this.lblWRSpeed = new System.Windows.Forms.Label();
            this.txtWRSpeed = new System.Windows.Forms.TextBox();
            this.lblWRPower = new System.Windows.Forms.Label();
            this.txtWRPower = new System.Windows.Forms.TextBox();
            this.lblWRFreq = new System.Windows.Forms.Label();
            this.txtWRFreq = new System.Windows.Forms.TextBox();
            this.lblWRRepeat = new System.Windows.Forms.Label();
            this.txtWRRepeat = new System.Windows.Forms.TextBox();
            this.lblWRSpotDelay = new System.Windows.Forms.Label();
            this.txtWRSpotDelay = new System.Windows.Forms.TextBox();
            this.lblWRPulseWidth = new System.Windows.Forms.Label();
            this.txtWRPulseWidth = new System.Windows.Forms.TextBox();
            this.lblWRFillPitch = new System.Windows.Forms.Label();
            this.txtWRFillPitch = new System.Windows.Forms.TextBox();
            this.lblWRFillRoundPitch = new System.Windows.Forms.Label();
            this.txtWRFillRoundPitch = new System.Windows.Forms.TextBox();
            this.lblWRFillTimes = new System.Windows.Forms.Label();
            this.txtWRFillTimes = new System.Windows.Forms.TextBox();
            this.lblWRFillStepAngle = new System.Windows.Forms.Label();
            this.txtWRFillStepAngle = new System.Windows.Forms.TextBox();
            this.btnWRPreview = new System.Windows.Forms.Button();
            this.btnWRStopPreview = new System.Windows.Forms.Button();
            this.btnWRMark = new System.Windows.Forms.Button();
            this.groupBoxQROnly = new System.Windows.Forms.GroupBox();
            this.lblQOContent = new System.Windows.Forms.Label();
            this.txtQOContent = new System.Windows.Forms.TextBox();
            this.lblQOWidth = new System.Windows.Forms.Label();
            this.txtQOWidth = new System.Windows.Forms.TextBox();
            this.lblQOHeight = new System.Windows.Forms.Label();
            this.txtQOHeight = new System.Windows.Forms.TextBox();
            this.lblQOBorder = new System.Windows.Forms.Label();
            this.txtQOBorder = new System.Windows.Forms.TextBox();
            this.chkQOInvert = new System.Windows.Forms.CheckBox();
            this.lblQOMarkStyle = new System.Windows.Forms.Label();
            this.txtQOMarkStyle = new System.Windows.Forms.TextBox();
            this.lblQORepeat = new System.Windows.Forms.Label();
            this.txtQORepeat = new System.Windows.Forms.TextBox();
            this.lblQOStepAngle = new System.Windows.Forms.Label();
            this.txtQOStepAngle = new System.Windows.Forms.TextBox();
            this.lblQOPower = new System.Windows.Forms.Label();
            this.txtQOPower = new System.Windows.Forms.TextBox();
            this.lblQOSpeed = new System.Windows.Forms.Label();
            this.txtQOSpeed = new System.Windows.Forms.TextBox();
            this.lblQOFreq = new System.Windows.Forms.Label();
            this.txtQOFreq = new System.Windows.Forms.TextBox();
            this.lblQOPulseWidth = new System.Windows.Forms.Label();
            this.txtQOPulseWidth = new System.Windows.Forms.TextBox();
            this.btnQOPreview = new System.Windows.Forms.Button();
            this.btnQOStopPreview = new System.Windows.Forms.Button();
            this.btnQOMark = new System.Windows.Forms.Button();
            this.btnAllPreview = new System.Windows.Forms.Button();
            this.btnAllStopPreview = new System.Windows.Forms.Button();
            this.btnAllMark = new System.Windows.Forms.Button();
            this.tabPageCLIBuilder = new System.Windows.Forms.TabPage();
            this.grpCLIBuilder = new System.Windows.Forms.GroupBox();
            this.lblCLIBoard = new System.Windows.Forms.Label();
            this.txtCLIBoard = new System.Windows.Forms.TextBox();
            this.lblCLIConfig = new System.Windows.Forms.Label();
            this.txtCLIConfig = new System.Windows.Forms.TextBox();
            this.lblCLIWsW = new System.Windows.Forms.Label();
            this.txtCLIWsW = new System.Windows.Forms.TextBox();
            this.lblCLIWsH = new System.Windows.Forms.Label();
            this.txtCLIWsH = new System.Windows.Forms.TextBox();
            this.lblCLIDxf = new System.Windows.Forms.Label();
            this.txtCLIDxf = new System.Windows.Forms.TextBox();
            this.lblCLILines = new System.Windows.Forms.Label();
            this.txtCLILines = new System.Windows.Forms.TextBox();
            this.lblCLIPower = new System.Windows.Forms.Label();
            this.txtCLIPower = new System.Windows.Forms.TextBox();
            this.lblCLISpeed = new System.Windows.Forms.Label();
            this.txtCLISpeed = new System.Windows.Forms.TextBox();
            this.lblCLIFreq = new System.Windows.Forms.Label();
            this.txtCLIFreq = new System.Windows.Forms.TextBox();
            this.lblCLIPulseWidth = new System.Windows.Forms.Label();
            this.txtCLIPulseWidth = new System.Windows.Forms.TextBox();
            this.lblCLIRepeat = new System.Windows.Forms.Label();
            this.txtCLIRepeat = new System.Windows.Forms.TextBox();
            this.lblCLIWobbleWidth = new System.Windows.Forms.Label();
            this.txtCLIWobbleWidth = new System.Windows.Forms.TextBox();
            this.lblCLIWobbleOverlap = new System.Windows.Forms.Label();
            this.txtCLIWobbleOverlap = new System.Windows.Forms.TextBox();
            this.lblCLIWobbleSpeed = new System.Windows.Forms.Label();
            this.txtCLIWobbleSpeed = new System.Windows.Forms.TextBox();
            this.lblCLIPreview = new System.Windows.Forms.Label();
            this.txtCLIPreview = new System.Windows.Forms.TextBox();
            this.lblCLIPreviewSpeed = new System.Windows.Forms.Label();
            this.txtCLIPreviewSpeed = new System.Windows.Forms.TextBox();
            this.lblCLIPreviewTime = new System.Windows.Forms.Label();
            this.txtCLIPreviewTime = new System.Windows.Forms.TextBox();
            this.chkCLIMark = new System.Windows.Forms.CheckBox();
            this.grpCLIQRCode = new System.Windows.Forms.GroupBox();
            this.lblCLIQRContent = new System.Windows.Forms.Label();
            this.txtCLIQRContent = new System.Windows.Forms.TextBox();
            this.lblCLIQRFixedHeader = new System.Windows.Forms.Label();
            this.lblCLIQRFixedParams = new System.Windows.Forms.Label();
            this.lblCLIQROutput = new System.Windows.Forms.Label();
            this.txtCLIQROutput = new System.Windows.Forms.TextBox();
            this.btnCLIQRRefresh = new System.Windows.Forms.Button();
            this.btnCLIQRExecuteMark = new System.Windows.Forms.Button();
            this.btnCLIQRStopPreview = new System.Windows.Forms.Button();
            this.lblCLIOutput = new System.Windows.Forms.Label();
            this.txtCLIOutput = new System.Windows.Forms.TextBox();
            this.btnCLIRefresh = new System.Windows.Forms.Button();
            this.btnCLIExecuteMark = new System.Windows.Forms.Button();
            this.btnCLIStopPreview = new System.Windows.Forms.Button();
            this.tabPageCmd = new System.Windows.Forms.TabPage();
            this.lblCmdHeader = new System.Windows.Forms.Label();
            this.lblBoardCmd = new System.Windows.Forms.Label();
            this.comboBoardCmd = new System.Windows.Forms.ComboBox();
            this.btnCmdRegen = new System.Windows.Forms.Button();
            this.lblCmd1 = new System.Windows.Forms.Label();
            this.txtCmd1 = new System.Windows.Forms.TextBox();
            this.btnCmd1 = new System.Windows.Forms.Button();
            this.lblCmd2 = new System.Windows.Forms.Label();
            this.txtCmd2 = new System.Windows.Forms.TextBox();
            this.btnCmd2 = new System.Windows.Forms.Button();
            this.lblCmd3 = new System.Windows.Forms.Label();
            this.txtCmd3 = new System.Windows.Forms.TextBox();
            this.btnCmd3 = new System.Windows.Forms.Button();
            this.lblCmd4 = new System.Windows.Forms.Label();
            this.txtCmd4 = new System.Windows.Forms.TextBox();
            this.btnCmd4 = new System.Windows.Forms.Button();
            this.lblCmd5 = new System.Windows.Forms.Label();
            this.txtCmd5 = new System.Windows.Forms.TextBox();
            this.btnCmd5 = new System.Windows.Forms.Button();
            this.lblCmdHint = new System.Windows.Forms.Label();
            this.btnParallelTest = new System.Windows.Forms.Button();
            this.txtParallelResult = new System.Windows.Forms.TextBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.trkPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarkRepeat)).BeginInit();
            this.tabPageQRCode.SuspendLayout();
            this.groupBoxQRBasic.SuspendLayout();
            this.groupBoxQRWhiteBg.SuspendLayout();
            this.groupBoxQRSteel.SuspendLayout();
            this.groupBoxSteelTime.SuspendLayout();
            this.groupBoxRectAlone.SuspendLayout();
            this.groupBoxQRAlone.SuspendLayout();
            this.tabPageQRCode2.SuspendLayout();
            this.groupBoxBlackRect.SuspendLayout();
            this.groupBoxWhiteRect.SuspendLayout();
            this.groupBoxQROnly.SuspendLayout();
            this.tabPageCLIBuilder.SuspendLayout();
            this.grpCLIBuilder.SuspendLayout();
            this.grpCLIQRCode.SuspendLayout();
            this.tabPageCmd.SuspendLayout();
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
            // timerParallelTest
            // 
            this.timerParallelTest.Interval = 5000;
            this.timerParallelTest.Tick += new System.EventHandler(this.timerParallelTest_Tick);
            // 
            // panelBoard1
            // 
            this.panelBoard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard1.Location = new System.Drawing.Point(6, 20);
            this.panelBoard1.Name = "panelBoard1";
            this.panelBoard1.Size = new System.Drawing.Size(124, 148);
            this.panelBoard1.TabIndex = 0;
            // 
            // panelBoard2
            // 
            this.panelBoard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard2.Location = new System.Drawing.Point(6, 20);
            this.panelBoard2.Name = "panelBoard2";
            this.panelBoard2.Size = new System.Drawing.Size(124, 148);
            this.panelBoard2.TabIndex = 0;
            // 
            // panelBoard3
            // 
            this.panelBoard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard3.Location = new System.Drawing.Point(6, 20);
            this.panelBoard3.Name = "panelBoard3";
            this.panelBoard3.Size = new System.Drawing.Size(124, 148);
            this.panelBoard3.TabIndex = 0;
            // 
            // panelBoard4
            // 
            this.panelBoard4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard4.Location = new System.Drawing.Point(6, 20);
            this.panelBoard4.Name = "panelBoard4";
            this.panelBoard4.Size = new System.Drawing.Size(124, 148);
            this.panelBoard4.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelBoard1);
            this.groupBox1.Location = new System.Drawing.Point(6, 605);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 173);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "晶片板 1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelBoard2);
            this.groupBox2.Location = new System.Drawing.Point(144, 605);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(135, 173);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "晶片板 2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panelBoard3);
            this.groupBox3.Location = new System.Drawing.Point(282, 605);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(135, 173);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "晶片板 3";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panelBoard4);
            this.groupBox4.Location = new System.Drawing.Point(420, 605);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(135, 173);
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
            this.tabControl1.Controls.Add(this.tabPageQRCode2);
            this.tabControl1.Controls.Add(this.tabPageCLIBuilder);
            this.tabControl1.Controls.Add(this.tabPageCmd);
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1205, 592);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPageConnect
            // 
            this.tabPageConnect.Controls.Add(this.lblBoardCount);
            this.tabPageConnect.Controls.Add(this.numBoardCount);
            this.tabPageConnect.Controls.Add(this.btnInit);
            this.tabPageConnect.Controls.Add(this.btnTestConnect);
            this.tabPageConnect.Controls.Add(this.btnExit);
            this.tabPageConnect.Location = new System.Drawing.Point(4, 22);
            this.tabPageConnect.Name = "tabPageConnect";
            this.tabPageConnect.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageConnect.Size = new System.Drawing.Size(1197, 566);
            this.tabPageConnect.TabIndex = 0;
            this.tabPageConnect.Text = "1. 連接設定";
            this.tabPageConnect.UseVisualStyleBackColor = true;
            // 
            // lblBoardCount
            // 
            this.lblBoardCount.AutoSize = true;
            this.lblBoardCount.Location = new System.Drawing.Point(6, 12);
            this.lblBoardCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoardCount.Name = "lblBoardCount";
            this.lblBoardCount.Size = new System.Drawing.Size(56, 12);
            this.lblBoardCount.TabIndex = 10;
            this.lblBoardCount.Text = "系統數量:";
            // 
            // numBoardCount
            // 
            this.numBoardCount.Location = new System.Drawing.Point(64, 10);
            this.numBoardCount.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.numBoardCount.Size = new System.Drawing.Size(38, 22);
            this.numBoardCount.TabIndex = 11;
            this.numBoardCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(109, 6);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(60, 28);
            this.btnInit.TabIndex = 0;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnTestConnect
            // 
            this.btnTestConnect.Location = new System.Drawing.Point(19, 48);
            this.btnTestConnect.Name = "btnTestConnect";
            this.btnTestConnect.Size = new System.Drawing.Size(142, 40);
            this.btnTestConnect.TabIndex = 2;
            this.btnTestConnect.Text = "測試連接";
            this.btnTestConnect.UseVisualStyleBackColor = true;
            this.btnTestConnect.Click += new System.EventHandler(this.btnTestConnect_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(464, 536);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 28);
            this.btnExit.TabIndex = 21;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tabPageParams
            // 
            this.tabPageParams.Controls.Add(this.lblWorkspace);
            this.tabPageParams.Controls.Add(this.txtWorkspace);
            this.tabPageParams.Controls.Add(this.lblWorkspaceHeight);
            this.tabPageParams.Controls.Add(this.txtWorkspaceHeight);
            this.tabPageParams.Controls.Add(this.lblMargin);
            this.tabPageParams.Controls.Add(this.txtMargin);
            this.tabPageParams.Controls.Add(this.groupBoxIP);
            this.tabPageParams.Location = new System.Drawing.Point(4, 22);
            this.tabPageParams.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageParams.Name = "tabPageParams";
            this.tabPageParams.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageParams.Size = new System.Drawing.Size(1197, 566);
            this.tabPageParams.TabIndex = 3;
            this.tabPageParams.Text = "2. 雷射參數";
            this.tabPageParams.UseVisualStyleBackColor = true;
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.AutoSize = true;
            this.lblWorkspace.Location = new System.Drawing.Point(15, 16);
            this.lblWorkspace.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(96, 12);
            this.lblWorkspace.TabIndex = 0;
            this.lblWorkspace.Text = "工作區寬 W(mm):";
            // 
            // txtWorkspace
            // 
            this.txtWorkspace.Location = new System.Drawing.Point(105, 14);
            this.txtWorkspace.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWorkspace.Name = "txtWorkspace";
            this.txtWorkspace.Size = new System.Drawing.Size(61, 22);
            this.txtWorkspace.TabIndex = 1;
            this.txtWorkspace.Text = "150";
            // 
            // lblWorkspaceHeight
            // 
            this.lblWorkspaceHeight.AutoSize = true;
            this.lblWorkspaceHeight.Location = new System.Drawing.Point(15, 44);
            this.lblWorkspaceHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWorkspaceHeight.Name = "lblWorkspaceHeight";
            this.lblWorkspaceHeight.Size = new System.Drawing.Size(93, 12);
            this.lblWorkspaceHeight.TabIndex = 2;
            this.lblWorkspaceHeight.Text = "工作區高 H(mm):";
            // 
            // txtWorkspaceHeight
            // 
            this.txtWorkspaceHeight.Location = new System.Drawing.Point(105, 42);
            this.txtWorkspaceHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWorkspaceHeight.Name = "txtWorkspaceHeight";
            this.txtWorkspaceHeight.Size = new System.Drawing.Size(61, 22);
            this.txtWorkspaceHeight.TabIndex = 3;
            this.txtWorkspaceHeight.Text = "150";
            // 
            // lblMargin
            // 
            this.lblMargin.AutoSize = true;
            this.lblMargin.Location = new System.Drawing.Point(15, 72);
            this.lblMargin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMargin.Name = "lblMargin";
            this.lblMargin.Size = new System.Drawing.Size(49, 12);
            this.lblMargin.TabIndex = 4;
            this.lblMargin.Text = "邊距(%):";
            // 
            // txtMargin
            // 
            this.txtMargin.Location = new System.Drawing.Point(105, 70);
            this.txtMargin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtMargin.Name = "txtMargin";
            this.txtMargin.Size = new System.Drawing.Size(61, 22);
            this.txtMargin.TabIndex = 5;
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
            this.groupBoxIP.Location = new System.Drawing.Point(6, 104);
            this.groupBoxIP.Name = "groupBoxIP";
            this.groupBoxIP.Size = new System.Drawing.Size(160, 136);
            this.groupBoxIP.TabIndex = 1;
            this.groupBoxIP.TabStop = false;
            this.groupBoxIP.Text = "EMC6 IP 設定";
            // 
            // lblIP1
            // 
            this.lblIP1.AutoSize = true;
            this.lblIP1.Location = new System.Drawing.Point(6, 20);
            this.lblIP1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIP1.Name = "lblIP1";
            this.lblIP1.Size = new System.Drawing.Size(34, 12);
            this.lblIP1.TabIndex = 0;
            this.lblIP1.Text = "MM1:";
            // 
            // txtIP1
            // 
            this.txtIP1.Location = new System.Drawing.Point(41, 18);
            this.txtIP1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIP1.Name = "txtIP1";
            this.txtIP1.Size = new System.Drawing.Size(110, 22);
            this.txtIP1.TabIndex = 1;
            // 
            // lblIP2
            // 
            this.lblIP2.AutoSize = true;
            this.lblIP2.Location = new System.Drawing.Point(6, 44);
            this.lblIP2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIP2.Name = "lblIP2";
            this.lblIP2.Size = new System.Drawing.Size(34, 12);
            this.lblIP2.TabIndex = 2;
            this.lblIP2.Text = "MM2:";
            // 
            // txtIP2
            // 
            this.txtIP2.Location = new System.Drawing.Point(41, 42);
            this.txtIP2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIP2.Name = "txtIP2";
            this.txtIP2.Size = new System.Drawing.Size(110, 22);
            this.txtIP2.TabIndex = 3;
            // 
            // lblIP3
            // 
            this.lblIP3.AutoSize = true;
            this.lblIP3.Location = new System.Drawing.Point(6, 68);
            this.lblIP3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIP3.Name = "lblIP3";
            this.lblIP3.Size = new System.Drawing.Size(34, 12);
            this.lblIP3.TabIndex = 4;
            this.lblIP3.Text = "MM3:";
            // 
            // txtIP3
            // 
            this.txtIP3.Location = new System.Drawing.Point(41, 66);
            this.txtIP3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIP3.Name = "txtIP3";
            this.txtIP3.Size = new System.Drawing.Size(110, 22);
            this.txtIP3.TabIndex = 5;
            // 
            // lblIP4
            // 
            this.lblIP4.AutoSize = true;
            this.lblIP4.Location = new System.Drawing.Point(6, 92);
            this.lblIP4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIP4.Name = "lblIP4";
            this.lblIP4.Size = new System.Drawing.Size(34, 12);
            this.lblIP4.TabIndex = 6;
            this.lblIP4.Text = "MM4:";
            // 
            // txtIP4
            // 
            this.txtIP4.Location = new System.Drawing.Point(41, 90);
            this.txtIP4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIP4.Name = "txtIP4";
            this.txtIP4.Size = new System.Drawing.Size(110, 22);
            this.txtIP4.TabIndex = 7;
            // 
            // btnReadIP
            // 
            this.btnReadIP.Location = new System.Drawing.Point(8, 114);
            this.btnReadIP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnReadIP.Name = "btnReadIP";
            this.btnReadIP.Size = new System.Drawing.Size(68, 18);
            this.btnReadIP.TabIndex = 8;
            this.btnReadIP.Text = "讀取IP";
            this.btnReadIP.UseVisualStyleBackColor = true;
            this.btnReadIP.Click += new System.EventHandler(this.btnReadIP_Click);
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
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(1197, 566);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "3. DXF 操作";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblDXFInfo
            // 
            this.lblDXFInfo.AutoSize = true;
            this.lblDXFInfo.Location = new System.Drawing.Point(15, 316);
            this.lblDXFInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDXFInfo.Name = "lblDXFInfo";
            this.lblDXFInfo.Size = new System.Drawing.Size(65, 12);
            this.lblDXFInfo.TabIndex = 8;
            this.lblDXFInfo.Text = "線段資訊：";
            // 
            // txtDXFInfo
            // 
            this.txtDXFInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDXFInfo.Location = new System.Drawing.Point(15, 330);
            this.txtDXFInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDXFInfo.Multiline = true;
            this.txtDXFInfo.Name = "txtDXFInfo";
            this.txtDXFInfo.ReadOnly = true;
            this.txtDXFInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDXFInfo.Size = new System.Drawing.Size(144, 233);
            this.txtDXFInfo.TabIndex = 7;
            this.txtDXFInfo.WordWrap = false;
            // 
            // btnBrowseDXF
            // 
            this.btnBrowseDXF.Location = new System.Drawing.Point(15, 97);
            this.btnBrowseDXF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBrowseDXF.Name = "btnBrowseDXF";
            this.btnBrowseDXF.Size = new System.Drawing.Size(56, 18);
            this.btnBrowseDXF.TabIndex = 6;
            this.btnBrowseDXF.Text = "瀏覽...";
            this.btnBrowseDXF.UseVisualStyleBackColor = true;
            this.btnBrowseDXF.Click += new System.EventHandler(this.btnBrowseDXF_Click);
            // 
            // lblBoardDXF
            // 
            this.lblBoardDXF.AutoSize = true;
            this.lblBoardDXF.Location = new System.Drawing.Point(15, 120);
            this.lblBoardDXF.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoardDXF.Name = "lblBoardDXF";
            this.lblBoardDXF.Size = new System.Drawing.Size(53, 12);
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
            this.comboBoardDXF.Location = new System.Drawing.Point(68, 118);
            this.comboBoardDXF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoardDXF.Name = "comboBoardDXF";
            this.comboBoardDXF.Size = new System.Drawing.Size(92, 20);
            this.comboBoardDXF.TabIndex = 4;
            // 
            // lblDXFPath
            // 
            this.lblDXFPath.AutoSize = true;
            this.lblDXFPath.Location = new System.Drawing.Point(15, 24);
            this.lblDXFPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDXFPath.Name = "lblDXFPath";
            this.lblDXFPath.Size = new System.Drawing.Size(66, 12);
            this.lblDXFPath.TabIndex = 3;
            this.lblDXFPath.Text = "DXF 路徑：";
            // 
            // txtDXFPath
            // 
            this.txtDXFPath.Location = new System.Drawing.Point(15, 44);
            this.txtDXFPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDXFPath.Multiline = true;
            this.txtDXFPath.Name = "txtDXFPath";
            this.txtDXFPath.Size = new System.Drawing.Size(144, 49);
            this.txtDXFPath.TabIndex = 2;
            this.txtDXFPath.Text = "File\\上翼板-2.dxf";
            // 
            // btnMarkDXF
            // 
            this.btnMarkDXF.Enabled = false;
            this.btnMarkDXF.Location = new System.Drawing.Point(17, 222);
            this.btnMarkDXF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnMarkDXF.Name = "btnMarkDXF";
            this.btnMarkDXF.Size = new System.Drawing.Size(68, 28);
            this.btnMarkDXF.TabIndex = 1;
            this.btnMarkDXF.Text = "打標";
            this.btnMarkDXF.UseVisualStyleBackColor = true;
            this.btnMarkDXF.Click += new System.EventHandler(this.btnMarkDXF_Click);
            // 
            // btnStopMarkDXF
            // 
            this.btnStopMarkDXF.Enabled = false;
            this.btnStopMarkDXF.Location = new System.Drawing.Point(88, 222);
            this.btnStopMarkDXF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStopMarkDXF.Name = "btnStopMarkDXF";
            this.btnStopMarkDXF.Size = new System.Drawing.Size(70, 28);
            this.btnStopMarkDXF.TabIndex = 13;
            this.btnStopMarkDXF.Text = "停止打標";
            this.btnStopMarkDXF.UseVisualStyleBackColor = true;
            this.btnStopMarkDXF.Click += new System.EventHandler(this.btnStopMarkDXF_Click);
            // 
            // btnPreviewDXF
            // 
            this.btnPreviewDXF.Location = new System.Drawing.Point(15, 256);
            this.btnPreviewDXF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnPreviewDXF.Name = "btnPreviewDXF";
            this.btnPreviewDXF.Size = new System.Drawing.Size(71, 24);
            this.btnPreviewDXF.TabIndex = 10;
            this.btnPreviewDXF.Text = "紅光預覽";
            this.btnPreviewDXF.UseVisualStyleBackColor = true;
            this.btnPreviewDXF.Click += new System.EventHandler(this.btnPreviewDXF_Click);
            // 
            // btnStopPreview
            // 
            this.btnStopPreview.Enabled = false;
            this.btnStopPreview.Location = new System.Drawing.Point(15, 284);
            this.btnStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStopPreview.Name = "btnStopPreview";
            this.btnStopPreview.Size = new System.Drawing.Size(71, 24);
            this.btnStopPreview.TabIndex = 12;
            this.btnStopPreview.Text = "停止預覽";
            this.btnStopPreview.UseVisualStyleBackColor = true;
            this.btnStopPreview.Click += new System.EventHandler(this.btnStopPreview_Click);
            // 
            // btnClearDXF
            // 
            this.btnClearDXF.Location = new System.Drawing.Point(90, 256);
            this.btnClearDXF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClearDXF.Name = "btnClearDXF";
            this.btnClearDXF.Size = new System.Drawing.Size(68, 24);
            this.btnClearDXF.TabIndex = 11;
            this.btnClearDXF.Text = "清除畫面";
            this.btnClearDXF.UseVisualStyleBackColor = true;
            this.btnClearDXF.Click += new System.EventHandler(this.btnClearDXF_Click);
            // 
            // btnLoadDXFFile
            // 
            this.btnLoadDXFFile.Location = new System.Drawing.Point(15, 186);
            this.btnLoadDXFFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadDXFFile.Name = "btnLoadDXFFile";
            this.btnLoadDXFFile.Size = new System.Drawing.Size(143, 28);
            this.btnLoadDXFFile.TabIndex = 9;
            this.btnLoadDXFFile.Text = "載入 DXF";
            this.btnLoadDXFFile.UseVisualStyleBackColor = true;
            this.btnLoadDXFFile.Click += new System.EventHandler(this.btnLoadDXFFile_Click);
            // 
            // btnLoadDXF
            // 
            this.btnLoadDXF.Location = new System.Drawing.Point(15, 152);
            this.btnLoadDXF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadDXF.Name = "btnLoadDXF";
            this.btnLoadDXF.Size = new System.Drawing.Size(143, 28);
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
            this.tabPageDraw.Controls.Add(this.btnPreviewManual);
            this.tabPageDraw.Controls.Add(this.btnStopPreviewManual);
            this.tabPageDraw.Location = new System.Drawing.Point(4, 22);
            this.tabPageDraw.Name = "tabPageDraw";
            this.tabPageDraw.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageDraw.Size = new System.Drawing.Size(1197, 566);
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
            this.comboBoard.Location = new System.Drawing.Point(6, 26);
            this.comboBoard.Name = "comboBoard";
            this.comboBoard.Size = new System.Drawing.Size(118, 20);
            this.comboBoard.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
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
            this.groupBox5.Location = new System.Drawing.Point(3, 50);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(160, 200);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "線段參數";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "X1:";
            // 
            // txtX1
            // 
            this.txtX1.Location = new System.Drawing.Point(60, 22);
            this.txtX1.Name = "txtX1";
            this.txtX1.Size = new System.Drawing.Size(80, 22);
            this.txtX1.TabIndex = 1;
            this.txtX1.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Y2:";
            // 
            // txtY1
            // 
            this.txtY1.Location = new System.Drawing.Point(60, 50);
            this.txtY1.Name = "txtY1";
            this.txtY1.Size = new System.Drawing.Size(80, 22);
            this.txtY1.TabIndex = 3;
            this.txtY1.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "X2:";
            // 
            // txtX2
            // 
            this.txtX2.Location = new System.Drawing.Point(60, 78);
            this.txtX2.Name = "txtX2";
            this.txtX2.Size = new System.Drawing.Size(80, 22);
            this.txtX2.TabIndex = 5;
            this.txtX2.Text = "50";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y1:";
            // 
            // txtY2
            // 
            this.txtY2.Location = new System.Drawing.Point(60, 106);
            this.txtY2.Name = "txtY2";
            this.txtY2.Size = new System.Drawing.Size(80, 22);
            this.txtY2.TabIndex = 7;
            this.txtY2.Text = "50";
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.Location = new System.Drawing.Point(8, 257);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(153, 40);
            this.btnDrawLine.TabIndex = 1;
            this.btnDrawLine.Text = "繪製線段";
            this.btnDrawLine.UseVisualStyleBackColor = true;
            this.btnDrawLine.Click += new System.EventHandler(this.btnDrawLine_Click);
            // 
            // btnMark
            // 
            this.btnMark.Location = new System.Drawing.Point(8, 303);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(153, 40);
            this.btnMark.TabIndex = 2;
            this.btnMark.Text = "開始雷射";
            this.btnMark.UseVisualStyleBackColor = true;
            this.btnMark.Click += new System.EventHandler(this.btnMark_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(6, 350);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(155, 40);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPreviewManual
            // 
            this.btnPreviewManual.Location = new System.Drawing.Point(6, 396);
            this.btnPreviewManual.Name = "btnPreviewManual";
            this.btnPreviewManual.Size = new System.Drawing.Size(75, 32);
            this.btnPreviewManual.TabIndex = 14;
            this.btnPreviewManual.Text = "紅光預覽";
            this.btnPreviewManual.UseVisualStyleBackColor = true;
            this.btnPreviewManual.Click += new System.EventHandler(this.btnPreviewManual_Click);
            // 
            // btnStopPreviewManual
            // 
            this.btnStopPreviewManual.Enabled = false;
            this.btnStopPreviewManual.Location = new System.Drawing.Point(86, 396);
            this.btnStopPreviewManual.Name = "btnStopPreviewManual";
            this.btnStopPreviewManual.Size = new System.Drawing.Size(75, 32);
            this.btnStopPreviewManual.TabIndex = 15;
            this.btnStopPreviewManual.Text = "停止預覽";
            this.btnStopPreviewManual.UseVisualStyleBackColor = true;
            this.btnStopPreviewManual.Click += new System.EventHandler(this.btnStopPreviewManual_Click);
            // 
            // tabPageLaserPower
            // 
            this.tabPageLaserPower.Controls.Add(this.lblBoardLaser);
            this.tabPageLaserPower.Controls.Add(this.comboBoardLaser);
            this.tabPageLaserPower.Controls.Add(this.groupBoxLaserParams);
            this.tabPageLaserPower.Controls.Add(this.btnApplyLaser);
            this.tabPageLaserPower.Controls.Add(this.btnReadLaser);
            this.tabPageLaserPower.Controls.Add(this.txtLaserStatus);
            this.tabPageLaserPower.Location = new System.Drawing.Point(4, 22);
            this.tabPageLaserPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageLaserPower.Name = "tabPageLaserPower";
            this.tabPageLaserPower.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageLaserPower.Size = new System.Drawing.Size(1197, 566);
            this.tabPageLaserPower.TabIndex = 4;
            this.tabPageLaserPower.Text = "5. 雷射功率";
            this.tabPageLaserPower.UseVisualStyleBackColor = true;
            // 
            // lblBoardLaser
            // 
            this.lblBoardLaser.AutoSize = true;
            this.lblBoardLaser.Location = new System.Drawing.Point(11, 12);
            this.lblBoardLaser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoardLaser.Name = "lblBoardLaser";
            this.lblBoardLaser.Size = new System.Drawing.Size(53, 12);
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
            this.comboBoardLaser.Location = new System.Drawing.Point(64, 10);
            this.comboBoardLaser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoardLaser.Name = "comboBoardLaser";
            this.comboBoardLaser.Size = new System.Drawing.Size(92, 20);
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
            this.groupBoxLaserParams.Location = new System.Drawing.Point(6, 36);
            this.groupBoxLaserParams.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxLaserParams.Name = "groupBoxLaserParams";
            this.groupBoxLaserParams.Size = new System.Drawing.Size(160, 320);
            this.groupBoxLaserParams.TabIndex = 2;
            this.groupBoxLaserParams.TabStop = false;
            this.groupBoxLaserParams.Text = "雷射參數設定";
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(8, 20);
            this.lblPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(52, 12);
            this.lblPower.TabIndex = 0;
            this.lblPower.Text = "功率 (%):";
            // 
            // trkPower
            // 
            this.trkPower.Location = new System.Drawing.Point(8, 38);
            this.trkPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trkPower.Maximum = 100;
            this.trkPower.Name = "trkPower";
            this.trkPower.Size = new System.Drawing.Size(90, 45);
            this.trkPower.TabIndex = 1;
            this.trkPower.TickFrequency = 10;
            this.trkPower.Value = 50;
            this.trkPower.Scroll += new System.EventHandler(this.trkPower_Scroll);
            // 
            // numPower
            // 
            this.numPower.DecimalPlaces = 1;
            this.numPower.Location = new System.Drawing.Point(105, 38);
            this.numPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numPower.Name = "numPower";
            this.numPower.Size = new System.Drawing.Size(49, 22);
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
            this.lblSpeed.Location = new System.Drawing.Point(8, 84);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(68, 12);
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "速度 (mm/s):";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(82, 82);
            this.txtSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(72, 22);
            this.txtSpeed.TabIndex = 4;
            this.txtSpeed.Text = "1000";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(8, 116);
            this.lblFrequency.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(62, 12);
            this.lblFrequency.TabIndex = 5;
            this.lblFrequency.Text = "頻率 (kHz):";
            // 
            // txtFrequency
            // 
            this.txtFrequency.Location = new System.Drawing.Point(82, 114);
            this.txtFrequency.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.Size = new System.Drawing.Size(72, 22);
            this.txtFrequency.TabIndex = 6;
            this.txtFrequency.Text = "30";
            // 
            // lblPulseWidth
            // 
            this.lblPulseWidth.AutoSize = true;
            this.lblPulseWidth.Location = new System.Drawing.Point(8, 148);
            this.lblPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPulseWidth.Name = "lblPulseWidth";
            this.lblPulseWidth.Size = new System.Drawing.Size(56, 12);
            this.lblPulseWidth.TabIndex = 7;
            this.lblPulseWidth.Text = "脈波寬度:";
            // 
            // txtPulseWidth
            // 
            this.txtPulseWidth.Location = new System.Drawing.Point(82, 146);
            this.txtPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPulseWidth.Name = "txtPulseWidth";
            this.txtPulseWidth.Size = new System.Drawing.Size(72, 22);
            this.txtPulseWidth.TabIndex = 8;
            this.txtPulseWidth.Text = "5";
            // 
            // lblMarkRepeat
            // 
            this.lblMarkRepeat.AutoSize = true;
            this.lblMarkRepeat.Location = new System.Drawing.Point(8, 180);
            this.lblMarkRepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMarkRepeat.Name = "lblMarkRepeat";
            this.lblMarkRepeat.Size = new System.Drawing.Size(56, 12);
            this.lblMarkRepeat.TabIndex = 9;
            this.lblMarkRepeat.Text = "雷射次數:";
            // 
            // numMarkRepeat
            // 
            this.numMarkRepeat.Location = new System.Drawing.Point(82, 178);
            this.numMarkRepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.numMarkRepeat.Size = new System.Drawing.Size(71, 22);
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
            this.chkWobble.Location = new System.Drawing.Point(8, 212);
            this.chkWobble.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkWobble.Name = "chkWobble";
            this.chkWobble.Size = new System.Drawing.Size(72, 16);
            this.chkWobble.TabIndex = 11;
            this.chkWobble.Text = "擺動啟動";
            this.chkWobble.UseVisualStyleBackColor = true;
            this.chkWobble.CheckedChanged += new System.EventHandler(this.chkWobble_CheckedChanged);
            // 
            // lblWobbleWidth
            // 
            this.lblWobbleWidth.AutoSize = true;
            this.lblWobbleWidth.Enabled = false;
            this.lblWobbleWidth.Location = new System.Drawing.Point(8, 238);
            this.lblWobbleWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWobbleWidth.Name = "lblWobbleWidth";
            this.lblWobbleWidth.Size = new System.Drawing.Size(56, 12);
            this.lblWobbleWidth.TabIndex = 12;
            this.lblWobbleWidth.Text = "擺動寬度:";
            // 
            // txtWobbleWidth
            // 
            this.txtWobbleWidth.Enabled = false;
            this.txtWobbleWidth.Location = new System.Drawing.Point(82, 236);
            this.txtWobbleWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWobbleWidth.Name = "txtWobbleWidth";
            this.txtWobbleWidth.Size = new System.Drawing.Size(72, 22);
            this.txtWobbleWidth.TabIndex = 13;
            this.txtWobbleWidth.Text = "0.1";
            // 
            // lblWobbleOverlap
            // 
            this.lblWobbleOverlap.AutoSize = true;
            this.lblWobbleOverlap.Enabled = false;
            this.lblWobbleOverlap.Location = new System.Drawing.Point(8, 262);
            this.lblWobbleOverlap.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWobbleOverlap.Name = "lblWobbleOverlap";
            this.lblWobbleOverlap.Size = new System.Drawing.Size(44, 12);
            this.lblWobbleOverlap.TabIndex = 14;
            this.lblWobbleOverlap.Text = "重疊率:";
            // 
            // txtWobbleOverlap
            // 
            this.txtWobbleOverlap.Enabled = false;
            this.txtWobbleOverlap.Location = new System.Drawing.Point(82, 260);
            this.txtWobbleOverlap.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWobbleOverlap.Name = "txtWobbleOverlap";
            this.txtWobbleOverlap.Size = new System.Drawing.Size(72, 22);
            this.txtWobbleOverlap.TabIndex = 15;
            this.txtWobbleOverlap.Text = "50.000";
            // 
            // lblWobbleSpeed
            // 
            this.lblWobbleSpeed.AutoSize = true;
            this.lblWobbleSpeed.Enabled = false;
            this.lblWobbleSpeed.Location = new System.Drawing.Point(8, 286);
            this.lblWobbleSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWobbleSpeed.Name = "lblWobbleSpeed";
            this.lblWobbleSpeed.Size = new System.Drawing.Size(56, 12);
            this.lblWobbleSpeed.TabIndex = 16;
            this.lblWobbleSpeed.Text = "擺動速度:";
            // 
            // txtWobbleSpeed
            // 
            this.txtWobbleSpeed.Enabled = false;
            this.txtWobbleSpeed.Location = new System.Drawing.Point(82, 284);
            this.txtWobbleSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWobbleSpeed.Name = "txtWobbleSpeed";
            this.txtWobbleSpeed.Size = new System.Drawing.Size(72, 22);
            this.txtWobbleSpeed.TabIndex = 17;
            this.txtWobbleSpeed.Text = "5026.55";
            // 
            // btnApplyLaser
            // 
            this.btnApplyLaser.Location = new System.Drawing.Point(11, 364);
            this.btnApplyLaser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnApplyLaser.Name = "btnApplyLaser";
            this.btnApplyLaser.Size = new System.Drawing.Size(75, 28);
            this.btnApplyLaser.TabIndex = 3;
            this.btnApplyLaser.Text = "套用參數";
            this.btnApplyLaser.UseVisualStyleBackColor = true;
            this.btnApplyLaser.Click += new System.EventHandler(this.btnApplyLaser_Click);
            // 
            // btnReadLaser
            // 
            this.btnReadLaser.Location = new System.Drawing.Point(90, 364);
            this.btnReadLaser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnReadLaser.Name = "btnReadLaser";
            this.btnReadLaser.Size = new System.Drawing.Size(75, 28);
            this.btnReadLaser.TabIndex = 4;
            this.btnReadLaser.Text = "讀取參數";
            this.btnReadLaser.UseVisualStyleBackColor = true;
            this.btnReadLaser.Click += new System.EventHandler(this.btnReadLaser_Click);
            // 
            // txtLaserStatus
            // 
            this.txtLaserStatus.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLaserStatus.Location = new System.Drawing.Point(11, 400);
            this.txtLaserStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLaserStatus.Multiline = true;
            this.txtLaserStatus.Name = "txtLaserStatus";
            this.txtLaserStatus.ReadOnly = true;
            this.txtLaserStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLaserStatus.Size = new System.Drawing.Size(155, 169);
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
            this.tabPageQRCode.Controls.Add(this.groupBoxQRWhiteBg);
            this.tabPageQRCode.Controls.Add(this.groupBoxQRSteel);
            this.tabPageQRCode.Controls.Add(this.groupBoxSteelTime);
            this.tabPageQRCode.Controls.Add(this.groupBoxRectAlone);
            this.tabPageQRCode.Controls.Add(this.groupBoxQRAlone);
            this.tabPageQRCode.Location = new System.Drawing.Point(4, 22);
            this.tabPageQRCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageQRCode.Name = "tabPageQRCode";
            this.tabPageQRCode.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageQRCode.Size = new System.Drawing.Size(1197, 566);
            this.tabPageQRCode.TabIndex = 5;
            this.tabPageQRCode.Text = "6. QR Code";
            this.tabPageQRCode.UseVisualStyleBackColor = true;
            // 
            // lblBoardQR
            // 
            this.lblBoardQR.AutoSize = true;
            this.lblBoardQR.Location = new System.Drawing.Point(11, 12);
            this.lblBoardQR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoardQR.Name = "lblBoardQR";
            this.lblBoardQR.Size = new System.Drawing.Size(53, 12);
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
            this.comboBoardQR.Location = new System.Drawing.Point(64, 10);
            this.comboBoardQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoardQR.Name = "comboBoardQR";
            this.comboBoardQR.Size = new System.Drawing.Size(92, 20);
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
            this.groupBoxQRBasic.Location = new System.Drawing.Point(6, 34);
            this.groupBoxQRBasic.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxQRBasic.Name = "groupBoxQRBasic";
            this.groupBoxQRBasic.Size = new System.Drawing.Size(322, 196);
            this.groupBoxQRBasic.TabIndex = 2;
            this.groupBoxQRBasic.TabStop = false;
            this.groupBoxQRBasic.Text = "QR Code 基本設定";
            // 
            // lblQRContent
            // 
            this.lblQRContent.AutoSize = true;
            this.lblQRContent.Location = new System.Drawing.Point(8, 20);
            this.lblQRContent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQRContent.Name = "lblQRContent";
            this.lblQRContent.Size = new System.Drawing.Size(60, 12);
            this.lblQRContent.TabIndex = 0;
            this.lblQRContent.Text = "QR 內容：";
            // 
            // txtQRContent
            // 
            this.txtQRContent.Location = new System.Drawing.Point(8, 36);
            this.txtQRContent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQRContent.Multiline = true;
            this.txtQRContent.Name = "txtQRContent";
            this.txtQRContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQRContent.Size = new System.Drawing.Size(308, 49);
            this.txtQRContent.TabIndex = 1;
            this.txtQRContent.Text = "Hello World";
            // 
            // lblQRPosX
            // 
            this.lblQRPosX.AutoSize = true;
            this.lblQRPosX.Location = new System.Drawing.Point(8, 94);
            this.lblQRPosX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQRPosX.Name = "lblQRPosX";
            this.lblQRPosX.Size = new System.Drawing.Size(69, 12);
            this.lblQRPosX.TabIndex = 2;
            this.lblQRPosX.Text = "X 位置(mm):";
            // 
            // txtQRPosX
            // 
            this.txtQRPosX.Location = new System.Drawing.Point(75, 92);
            this.txtQRPosX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQRPosX.Name = "txtQRPosX";
            this.txtQRPosX.Size = new System.Drawing.Size(61, 22);
            this.txtQRPosX.TabIndex = 3;
            this.txtQRPosX.Text = "0";
            // 
            // lblQRPosY
            // 
            this.lblQRPosY.AutoSize = true;
            this.lblQRPosY.Location = new System.Drawing.Point(150, 94);
            this.lblQRPosY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQRPosY.Name = "lblQRPosY";
            this.lblQRPosY.Size = new System.Drawing.Size(69, 12);
            this.lblQRPosY.TabIndex = 4;
            this.lblQRPosY.Text = "Y 位置(mm):";
            // 
            // txtQRPosY
            // 
            this.txtQRPosY.Location = new System.Drawing.Point(218, 92);
            this.txtQRPosY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQRPosY.Name = "txtQRPosY";
            this.txtQRPosY.Size = new System.Drawing.Size(61, 22);
            this.txtQRPosY.TabIndex = 5;
            this.txtQRPosY.Text = "0";
            // 
            // lblQRWidth
            // 
            this.lblQRWidth.AutoSize = true;
            this.lblQRWidth.Location = new System.Drawing.Point(8, 122);
            this.lblQRWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQRWidth.Name = "lblQRWidth";
            this.lblQRWidth.Size = new System.Drawing.Size(58, 12);
            this.lblQRWidth.TabIndex = 6;
            this.lblQRWidth.Text = "寬度(mm):";
            // 
            // txtQRWidth
            // 
            this.txtQRWidth.Location = new System.Drawing.Point(75, 120);
            this.txtQRWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQRWidth.Name = "txtQRWidth";
            this.txtQRWidth.Size = new System.Drawing.Size(61, 22);
            this.txtQRWidth.TabIndex = 7;
            this.txtQRWidth.Text = "10";
            // 
            // lblQRHeight
            // 
            this.lblQRHeight.AutoSize = true;
            this.lblQRHeight.Location = new System.Drawing.Point(150, 122);
            this.lblQRHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQRHeight.Name = "lblQRHeight";
            this.lblQRHeight.Size = new System.Drawing.Size(58, 12);
            this.lblQRHeight.TabIndex = 8;
            this.lblQRHeight.Text = "高度(mm):";
            // 
            // txtQRHeight
            // 
            this.txtQRHeight.Location = new System.Drawing.Point(218, 120);
            this.txtQRHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQRHeight.Name = "txtQRHeight";
            this.txtQRHeight.Size = new System.Drawing.Size(61, 22);
            this.txtQRHeight.TabIndex = 9;
            this.txtQRHeight.Text = "10";
            // 
            // chkQRInvert
            // 
            this.chkQRInvert.AutoSize = true;
            this.chkQRInvert.Location = new System.Drawing.Point(11, 236);
            this.chkQRInvert.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkQRInvert.Name = "chkQRInvert";
            this.chkQRInvert.Size = new System.Drawing.Size(72, 16);
            this.chkQRInvert.TabIndex = 3;
            this.chkQRInvert.Text = "反轉黑白";
            this.chkQRInvert.UseVisualStyleBackColor = true;
            // 
            // btnLoadQR
            // 
            this.btnLoadQR.Location = new System.Drawing.Point(11, 260);
            this.btnLoadQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadQR.Name = "btnLoadQR";
            this.btnLoadQR.Size = new System.Drawing.Size(150, 28);
            this.btnLoadQR.TabIndex = 4;
            this.btnLoadQR.Text = "載入 QR Code";
            this.btnLoadQR.UseVisualStyleBackColor = true;
            this.btnLoadQR.Click += new System.EventHandler(this.btnLoadQR_Click);
            // 
            // btnMarkQR
            // 
            this.btnMarkQR.Enabled = false;
            this.btnMarkQR.Location = new System.Drawing.Point(11, 296);
            this.btnMarkQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnMarkQR.Name = "btnMarkQR";
            this.btnMarkQR.Size = new System.Drawing.Size(71, 28);
            this.btnMarkQR.TabIndex = 5;
            this.btnMarkQR.Text = "打標";
            this.btnMarkQR.UseVisualStyleBackColor = true;
            this.btnMarkQR.Click += new System.EventHandler(this.btnMarkQR_Click);
            // 
            // btnStopMarkQR
            // 
            this.btnStopMarkQR.Enabled = false;
            this.btnStopMarkQR.Location = new System.Drawing.Point(90, 296);
            this.btnStopMarkQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStopMarkQR.Name = "btnStopMarkQR";
            this.btnStopMarkQR.Size = new System.Drawing.Size(71, 28);
            this.btnStopMarkQR.TabIndex = 6;
            this.btnStopMarkQR.Text = "停止打標";
            this.btnStopMarkQR.UseVisualStyleBackColor = true;
            this.btnStopMarkQR.Click += new System.EventHandler(this.btnStopMarkQR_Click);
            // 
            // btnPreviewQR
            // 
            this.btnPreviewQR.Location = new System.Drawing.Point(11, 330);
            this.btnPreviewQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnPreviewQR.Name = "btnPreviewQR";
            this.btnPreviewQR.Size = new System.Drawing.Size(71, 24);
            this.btnPreviewQR.TabIndex = 7;
            this.btnPreviewQR.Text = "紅光預覽";
            this.btnPreviewQR.UseVisualStyleBackColor = true;
            this.btnPreviewQR.Click += new System.EventHandler(this.btnPreviewQR_Click);
            // 
            // btnStopPreviewQR
            // 
            this.btnStopPreviewQR.Enabled = false;
            this.btnStopPreviewQR.Location = new System.Drawing.Point(90, 330);
            this.btnStopPreviewQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStopPreviewQR.Name = "btnStopPreviewQR";
            this.btnStopPreviewQR.Size = new System.Drawing.Size(71, 24);
            this.btnStopPreviewQR.TabIndex = 8;
            this.btnStopPreviewQR.Text = "停止預覽";
            this.btnStopPreviewQR.UseVisualStyleBackColor = true;
            this.btnStopPreviewQR.Click += new System.EventHandler(this.btnStopPreviewQR_Click);
            // 
            // btnClearQR
            // 
            this.btnClearQR.Location = new System.Drawing.Point(169, 296);
            this.btnClearQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClearQR.Name = "btnClearQR";
            this.btnClearQR.Size = new System.Drawing.Size(71, 28);
            this.btnClearQR.TabIndex = 9;
            this.btnClearQR.Text = "清除畫面";
            this.btnClearQR.UseVisualStyleBackColor = true;
            this.btnClearQR.Click += new System.EventHandler(this.btnClearQR_Click);
            // 
            // txtQRStatus
            // 
            this.txtQRStatus.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtQRStatus.Location = new System.Drawing.Point(11, 364);
            this.txtQRStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQRStatus.Multiline = true;
            this.txtQRStatus.Name = "txtQRStatus";
            this.txtQRStatus.ReadOnly = true;
            this.txtQRStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQRStatus.Size = new System.Drawing.Size(316, 81);
            this.txtQRStatus.TabIndex = 10;
            // 
            // groupBoxQRWhiteBg
            // 
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBRectSpeed);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBRectSpeed);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBRectPower);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBRectPower);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBQRSpeed);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBQRSpeed);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBQRPower);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBQRPower);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBQRWidth);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBQRWidth);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBQRHeight);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBQRHeight);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBQuietZone);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBQuietZone);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBRectExtra);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBRectExtra);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBMarkTarget);
            this.groupBoxQRWhiteBg.Controls.Add(this.rdoWBMarkQR);
            this.groupBoxQRWhiteBg.Controls.Add(this.rdoWBMarkRect);
            this.groupBoxQRWhiteBg.Controls.Add(this.rdoWBMarkAll);
            this.groupBoxQRWhiteBg.Controls.Add(this.btnQRWhiteBgCreate);
            this.groupBoxQRWhiteBg.Controls.Add(this.btnQRWhiteBgPreview);
            this.groupBoxQRWhiteBg.Controls.Add(this.btnQRWhiteBgStopPreview);
            this.groupBoxQRWhiteBg.Controls.Add(this.btnQRWhiteBgMark);
            this.groupBoxQRWhiteBg.Controls.Add(this.lblWBSerial);
            this.groupBoxQRWhiteBg.Controls.Add(this.txtWBSerial);
            this.groupBoxQRWhiteBg.Location = new System.Drawing.Point(338, 34);
            this.groupBoxQRWhiteBg.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxQRWhiteBg.Name = "groupBoxQRWhiteBg";
            this.groupBoxQRWhiteBg.Size = new System.Drawing.Size(202, 440);
            this.groupBoxQRWhiteBg.TabIndex = 11;
            this.groupBoxQRWhiteBg.TabStop = false;
            this.groupBoxQRWhiteBg.Text = "QRCODE_白底";
            // 
            // lblWBRectSpeed
            // 
            this.lblWBRectSpeed.AutoSize = true;
            this.lblWBRectSpeed.Location = new System.Drawing.Point(8, 22);
            this.lblWBRectSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBRectSpeed.Name = "lblWBRectSpeed";
            this.lblWBRectSpeed.Size = new System.Drawing.Size(56, 12);
            this.lblWBRectSpeed.TabIndex = 1;
            this.lblWBRectSpeed.Text = "矩形速度:";
            // 
            // txtWBRectSpeed
            // 
            this.txtWBRectSpeed.Location = new System.Drawing.Point(64, 20);
            this.txtWBRectSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBRectSpeed.Name = "txtWBRectSpeed";
            this.txtWBRectSpeed.Size = new System.Drawing.Size(38, 22);
            this.txtWBRectSpeed.TabIndex = 2;
            this.txtWBRectSpeed.Text = "800";
            // 
            // lblWBRectPower
            // 
            this.lblWBRectPower.AutoSize = true;
            this.lblWBRectPower.Location = new System.Drawing.Point(112, 22);
            this.lblWBRectPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBRectPower.Name = "lblWBRectPower";
            this.lblWBRectPower.Size = new System.Drawing.Size(56, 12);
            this.lblWBRectPower.TabIndex = 3;
            this.lblWBRectPower.Text = "矩形功率:";
            // 
            // txtWBRectPower
            // 
            this.txtWBRectPower.Location = new System.Drawing.Point(165, 20);
            this.txtWBRectPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBRectPower.Name = "txtWBRectPower";
            this.txtWBRectPower.Size = new System.Drawing.Size(31, 22);
            this.txtWBRectPower.TabIndex = 4;
            this.txtWBRectPower.Text = "100";
            // 
            // lblWBQRSpeed
            // 
            this.lblWBQRSpeed.AutoSize = true;
            this.lblWBQRSpeed.Location = new System.Drawing.Point(8, 48);
            this.lblWBQRSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBQRSpeed.Name = "lblWBQRSpeed";
            this.lblWBQRSpeed.Size = new System.Drawing.Size(48, 12);
            this.lblWBQRSpeed.TabIndex = 5;
            this.lblWBQRSpeed.Text = "QR速度:";
            // 
            // txtWBQRSpeed
            // 
            this.txtWBQRSpeed.Location = new System.Drawing.Point(64, 46);
            this.txtWBQRSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBQRSpeed.Name = "txtWBQRSpeed";
            this.txtWBQRSpeed.Size = new System.Drawing.Size(38, 22);
            this.txtWBQRSpeed.TabIndex = 6;
            this.txtWBQRSpeed.Text = "1000";
            // 
            // lblWBQRPower
            // 
            this.lblWBQRPower.AutoSize = true;
            this.lblWBQRPower.Location = new System.Drawing.Point(112, 48);
            this.lblWBQRPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBQRPower.Name = "lblWBQRPower";
            this.lblWBQRPower.Size = new System.Drawing.Size(48, 12);
            this.lblWBQRPower.TabIndex = 7;
            this.lblWBQRPower.Text = "QR功率:";
            // 
            // txtWBQRPower
            // 
            this.txtWBQRPower.Location = new System.Drawing.Point(165, 46);
            this.txtWBQRPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBQRPower.Name = "txtWBQRPower";
            this.txtWBQRPower.Size = new System.Drawing.Size(31, 22);
            this.txtWBQRPower.TabIndex = 8;
            this.txtWBQRPower.Text = "80";
            // 
            // lblWBQRWidth
            // 
            this.lblWBQRWidth.AutoSize = true;
            this.lblWBQRWidth.Location = new System.Drawing.Point(8, 74);
            this.lblWBQRWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBQRWidth.Name = "lblWBQRWidth";
            this.lblWBQRWidth.Size = new System.Drawing.Size(36, 12);
            this.lblWBQRWidth.TabIndex = 13;
            this.lblWBQRWidth.Text = "QR長:";
            // 
            // txtWBQRWidth
            // 
            this.txtWBQRWidth.Location = new System.Drawing.Point(64, 71);
            this.txtWBQRWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBQRWidth.Name = "txtWBQRWidth";
            this.txtWBQRWidth.Size = new System.Drawing.Size(38, 22);
            this.txtWBQRWidth.TabIndex = 14;
            this.txtWBQRWidth.Text = "15";
            // 
            // lblWBQRHeight
            // 
            this.lblWBQRHeight.AutoSize = true;
            this.lblWBQRHeight.Location = new System.Drawing.Point(112, 74);
            this.lblWBQRHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBQRHeight.Name = "lblWBQRHeight";
            this.lblWBQRHeight.Size = new System.Drawing.Size(36, 12);
            this.lblWBQRHeight.TabIndex = 15;
            this.lblWBQRHeight.Text = "QR寬:";
            // 
            // txtWBQRHeight
            // 
            this.txtWBQRHeight.Location = new System.Drawing.Point(165, 71);
            this.txtWBQRHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBQRHeight.Name = "txtWBQRHeight";
            this.txtWBQRHeight.Size = new System.Drawing.Size(31, 22);
            this.txtWBQRHeight.TabIndex = 16;
            this.txtWBQRHeight.Text = "15";
            // 
            // lblWBQuietZone
            // 
            this.lblWBQuietZone.AutoSize = true;
            this.lblWBQuietZone.Location = new System.Drawing.Point(8, 99);
            this.lblWBQuietZone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBQuietZone.Name = "lblWBQuietZone";
            this.lblWBQuietZone.Size = new System.Drawing.Size(56, 12);
            this.lblWBQuietZone.TabIndex = 17;
            this.lblWBQuietZone.Text = "外框單元:";
            // 
            // txtWBQuietZone
            // 
            this.txtWBQuietZone.Location = new System.Drawing.Point(64, 97);
            this.txtWBQuietZone.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBQuietZone.Name = "txtWBQuietZone";
            this.txtWBQuietZone.Size = new System.Drawing.Size(38, 22);
            this.txtWBQuietZone.TabIndex = 18;
            this.txtWBQuietZone.Text = "2";
            // 
            // lblWBRectExtra
            // 
            this.lblWBRectExtra.AutoSize = true;
            this.lblWBRectExtra.Location = new System.Drawing.Point(112, 99);
            this.lblWBRectExtra.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBRectExtra.Name = "lblWBRectExtra";
            this.lblWBRectExtra.Size = new System.Drawing.Size(16, 12);
            this.lblWBRectExtra.TabIndex = 19;
            this.lblWBRectExtra.Text = "X:";
            // 
            // txtWBRectExtra
            // 
            this.txtWBRectExtra.Location = new System.Drawing.Point(165, 97);
            this.txtWBRectExtra.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBRectExtra.Name = "txtWBRectExtra";
            this.txtWBRectExtra.Size = new System.Drawing.Size(31, 22);
            this.txtWBRectExtra.TabIndex = 20;
            this.txtWBRectExtra.Text = "0";
            // 
            // lblWBMarkTarget
            // 
            this.lblWBMarkTarget.AutoSize = true;
            this.lblWBMarkTarget.Location = new System.Drawing.Point(8, 126);
            this.lblWBMarkTarget.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBMarkTarget.Name = "lblWBMarkTarget";
            this.lblWBMarkTarget.Size = new System.Drawing.Size(32, 12);
            this.lblWBMarkTarget.TabIndex = 22;
            this.lblWBMarkTarget.Text = "打標:";
            // 
            // rdoWBMarkQR
            // 
            this.rdoWBMarkQR.AutoSize = true;
            this.rdoWBMarkQR.Location = new System.Drawing.Point(41, 125);
            this.rdoWBMarkQR.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoWBMarkQR.Name = "rdoWBMarkQR";
            this.rdoWBMarkQR.Size = new System.Drawing.Size(39, 16);
            this.rdoWBMarkQR.TabIndex = 23;
            this.rdoWBMarkQR.Text = "QR";
            this.rdoWBMarkQR.UseVisualStyleBackColor = true;
            // 
            // rdoWBMarkRect
            // 
            this.rdoWBMarkRect.AutoSize = true;
            this.rdoWBMarkRect.Location = new System.Drawing.Point(82, 125);
            this.rdoWBMarkRect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoWBMarkRect.Name = "rdoWBMarkRect";
            this.rdoWBMarkRect.Size = new System.Drawing.Size(47, 16);
            this.rdoWBMarkRect.TabIndex = 24;
            this.rdoWBMarkRect.Text = "矩形";
            this.rdoWBMarkRect.UseVisualStyleBackColor = true;
            // 
            // rdoWBMarkAll
            // 
            this.rdoWBMarkAll.AutoSize = true;
            this.rdoWBMarkAll.Checked = true;
            this.rdoWBMarkAll.Location = new System.Drawing.Point(128, 125);
            this.rdoWBMarkAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdoWBMarkAll.Name = "rdoWBMarkAll";
            this.rdoWBMarkAll.Size = new System.Drawing.Size(47, 16);
            this.rdoWBMarkAll.TabIndex = 25;
            this.rdoWBMarkAll.TabStop = true;
            this.rdoWBMarkAll.Text = "全部";
            this.rdoWBMarkAll.UseVisualStyleBackColor = true;
            // 
            // btnQRWhiteBgCreate
            // 
            this.btnQRWhiteBgCreate.Location = new System.Drawing.Point(6, 174);
            this.btnQRWhiteBgCreate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQRWhiteBgCreate.Name = "btnQRWhiteBgCreate";
            this.btnQRWhiteBgCreate.Size = new System.Drawing.Size(172, 36);
            this.btnQRWhiteBgCreate.TabIndex = 21;
            this.btnQRWhiteBgCreate.Text = "依設定建立 QR Code";
            this.btnQRWhiteBgCreate.UseVisualStyleBackColor = true;
            this.btnQRWhiteBgCreate.Click += new System.EventHandler(this.btnQRWhiteBgCreate_Click);
            // 
            // btnQRWhiteBgPreview
            // 
            this.btnQRWhiteBgPreview.Location = new System.Drawing.Point(6, 288);
            this.btnQRWhiteBgPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQRWhiteBgPreview.Name = "btnQRWhiteBgPreview";
            this.btnQRWhiteBgPreview.Size = new System.Drawing.Size(82, 40);
            this.btnQRWhiteBgPreview.TabIndex = 28;
            this.btnQRWhiteBgPreview.Text = "紅光預覽";
            this.btnQRWhiteBgPreview.UseVisualStyleBackColor = true;
            this.btnQRWhiteBgPreview.Click += new System.EventHandler(this.btnQRWhiteBgPreview_Click);
            // 
            // btnQRWhiteBgStopPreview
            // 
            this.btnQRWhiteBgStopPreview.Location = new System.Drawing.Point(96, 288);
            this.btnQRWhiteBgStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQRWhiteBgStopPreview.Name = "btnQRWhiteBgStopPreview";
            this.btnQRWhiteBgStopPreview.Size = new System.Drawing.Size(82, 40);
            this.btnQRWhiteBgStopPreview.TabIndex = 29;
            this.btnQRWhiteBgStopPreview.Text = "預覽停止";
            this.btnQRWhiteBgStopPreview.UseVisualStyleBackColor = true;
            this.btnQRWhiteBgStopPreview.Click += new System.EventHandler(this.btnQRWhiteBgStopPreview_Click);
            // 
            // btnQRWhiteBgMark
            // 
            this.btnQRWhiteBgMark.Location = new System.Drawing.Point(6, 218);
            this.btnQRWhiteBgMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQRWhiteBgMark.Name = "btnQRWhiteBgMark";
            this.btnQRWhiteBgMark.Size = new System.Drawing.Size(172, 64);
            this.btnQRWhiteBgMark.TabIndex = 0;
            this.btnQRWhiteBgMark.Text = "白底 QR 雙圖層打標";
            this.btnQRWhiteBgMark.UseVisualStyleBackColor = true;
            this.btnQRWhiteBgMark.Click += new System.EventHandler(this.btnQRWhiteBgMark_Click);
            // 
            // lblWBSerial
            // 
            this.lblWBSerial.AutoSize = true;
            this.lblWBSerial.Location = new System.Drawing.Point(4, 149);
            this.lblWBSerial.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWBSerial.Name = "lblWBSerial";
            this.lblWBSerial.Size = new System.Drawing.Size(32, 12);
            this.lblWBSerial.TabIndex = 26;
            this.lblWBSerial.Text = "序號:";
            // 
            // txtWBSerial
            // 
            this.txtWBSerial.Location = new System.Drawing.Point(41, 146);
            this.txtWBSerial.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWBSerial.Name = "txtWBSerial";
            this.txtWBSerial.ReadOnly = true;
            this.txtWBSerial.Size = new System.Drawing.Size(151, 22);
            this.txtWBSerial.TabIndex = 27;
            this.txtWBSerial.Text = "(雙圖層打標後自動填入)";
            // 
            // groupBoxQRSteel
            // 
            this.groupBoxQRSteel.Controls.Add(this.lblSteelQrWidth);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelQrWidth);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelQrHeight);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelQrHeight);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelBorder);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelBorder);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelRectExtra);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelRectExtra);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelECLevel);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelECLevel);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelMarkStyle);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelMarkStyle);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelSpotSize);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelSpotSize);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelQrRepeat);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelQrRepeat);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelRectPower);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelRectPower);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelRectSpeed);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelRectSpeed);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelRectFreq);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelRectFreq);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelRectRepeat);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelRectRepeat);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelQrPower);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelQrPower);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelQrSpeed);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelQrSpeed);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelQrFreq);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelQrFreq);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelQrPulseWidth);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelQrPulseWidth);
            this.groupBoxQRSteel.Controls.Add(this.lblSteelSerial);
            this.groupBoxQRSteel.Controls.Add(this.txtSteelSerial);
            this.groupBoxQRSteel.Controls.Add(this.btnQRSteelPreview);
            this.groupBoxQRSteel.Controls.Add(this.btnQRSteelMark);
            this.groupBoxQRSteel.Controls.Add(this.btnQRSteelStopPreview);
            this.groupBoxQRSteel.Location = new System.Drawing.Point(544, 34);
            this.groupBoxQRSteel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxQRSteel.Name = "groupBoxQRSteel";
            this.groupBoxQRSteel.Size = new System.Drawing.Size(202, 280);
            this.groupBoxQRSteel.TabIndex = 30;
            this.groupBoxQRSteel.TabStop = false;
            this.groupBoxQRSteel.Text = "QRCODE_鋼鐵Quest3";
            // 
            // lblSteelQrWidth
            // 
            this.lblSteelQrWidth.AutoSize = true;
            this.lblSteelQrWidth.Location = new System.Drawing.Point(6, 20);
            this.lblSteelQrWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelQrWidth.Name = "lblSteelQrWidth";
            this.lblSteelQrWidth.Size = new System.Drawing.Size(36, 12);
            this.lblSteelQrWidth.TabIndex = 0;
            this.lblSteelQrWidth.Text = "QR寬:";
            // 
            // txtSteelQrWidth
            // 
            this.txtSteelQrWidth.Location = new System.Drawing.Point(56, 18);
            this.txtSteelQrWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelQrWidth.Name = "txtSteelQrWidth";
            this.txtSteelQrWidth.Size = new System.Drawing.Size(38, 22);
            this.txtSteelQrWidth.TabIndex = 1;
            this.txtSteelQrWidth.Text = "20";
            // 
            // lblSteelQrHeight
            // 
            this.lblSteelQrHeight.AutoSize = true;
            this.lblSteelQrHeight.Location = new System.Drawing.Point(101, 20);
            this.lblSteelQrHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelQrHeight.Name = "lblSteelQrHeight";
            this.lblSteelQrHeight.Size = new System.Drawing.Size(36, 12);
            this.lblSteelQrHeight.TabIndex = 2;
            this.lblSteelQrHeight.Text = "QR高:";
            // 
            // txtSteelQrHeight
            // 
            this.txtSteelQrHeight.Location = new System.Drawing.Point(154, 18);
            this.txtSteelQrHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelQrHeight.Name = "txtSteelQrHeight";
            this.txtSteelQrHeight.Size = new System.Drawing.Size(38, 22);
            this.txtSteelQrHeight.TabIndex = 3;
            this.txtSteelQrHeight.Text = "20";
            // 
            // lblSteelBorder
            // 
            this.lblSteelBorder.AutoSize = true;
            this.lblSteelBorder.Location = new System.Drawing.Point(6, 42);
            this.lblSteelBorder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelBorder.Name = "lblSteelBorder";
            this.lblSteelBorder.Size = new System.Drawing.Size(41, 12);
            this.lblSteelBorder.TabIndex = 4;
            this.lblSteelBorder.Text = "Border:";
            // 
            // txtSteelBorder
            // 
            this.txtSteelBorder.Location = new System.Drawing.Point(56, 40);
            this.txtSteelBorder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelBorder.Name = "txtSteelBorder";
            this.txtSteelBorder.Size = new System.Drawing.Size(38, 22);
            this.txtSteelBorder.TabIndex = 5;
            this.txtSteelBorder.Text = "4";
            // 
            // lblSteelRectExtra
            // 
            this.lblSteelRectExtra.AutoSize = true;
            this.lblSteelRectExtra.Location = new System.Drawing.Point(101, 42);
            this.lblSteelRectExtra.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelRectExtra.Name = "lblSteelRectExtra";
            this.lblSteelRectExtra.Size = new System.Drawing.Size(46, 12);
            this.lblSteelRectExtra.TabIndex = 6;
            this.lblSteelRectExtra.Text = "矩形+X:";
            // 
            // txtSteelRectExtra
            // 
            this.txtSteelRectExtra.Location = new System.Drawing.Point(154, 40);
            this.txtSteelRectExtra.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelRectExtra.Name = "txtSteelRectExtra";
            this.txtSteelRectExtra.Size = new System.Drawing.Size(38, 22);
            this.txtSteelRectExtra.TabIndex = 7;
            this.txtSteelRectExtra.Text = "0";
            // 
            // lblSteelECLevel
            // 
            this.lblSteelECLevel.AutoSize = true;
            this.lblSteelECLevel.Location = new System.Drawing.Point(6, 65);
            this.lblSteelECLevel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelECLevel.Name = "lblSteelECLevel";
            this.lblSteelECLevel.Size = new System.Drawing.Size(47, 12);
            this.lblSteelECLevel.TabIndex = 8;
            this.lblSteelECLevel.Text = "EC等級:";
            // 
            // txtSteelECLevel
            // 
            this.txtSteelECLevel.Location = new System.Drawing.Point(56, 62);
            this.txtSteelECLevel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelECLevel.Name = "txtSteelECLevel";
            this.txtSteelECLevel.Size = new System.Drawing.Size(38, 22);
            this.txtSteelECLevel.TabIndex = 9;
            this.txtSteelECLevel.Text = "1";
            // 
            // lblSteelMarkStyle
            // 
            this.lblSteelMarkStyle.AutoSize = true;
            this.lblSteelMarkStyle.Location = new System.Drawing.Point(101, 65);
            this.lblSteelMarkStyle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelMarkStyle.Name = "lblSteelMarkStyle";
            this.lblSteelMarkStyle.Size = new System.Drawing.Size(56, 12);
            this.lblSteelMarkStyle.TabIndex = 10;
            this.lblSteelMarkStyle.Text = "MarkStyle:";
            // 
            // txtSteelMarkStyle
            // 
            this.txtSteelMarkStyle.Location = new System.Drawing.Point(154, 62);
            this.txtSteelMarkStyle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelMarkStyle.Name = "txtSteelMarkStyle";
            this.txtSteelMarkStyle.Size = new System.Drawing.Size(38, 22);
            this.txtSteelMarkStyle.TabIndex = 11;
            this.txtSteelMarkStyle.Text = "1";
            // 
            // lblSteelSpotSize
            // 
            this.lblSteelSpotSize.AutoSize = true;
            this.lblSteelSpotSize.Location = new System.Drawing.Point(6, 87);
            this.lblSteelSpotSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelSpotSize.Name = "lblSteelSpotSize";
            this.lblSteelSpotSize.Size = new System.Drawing.Size(29, 12);
            this.lblSteelSpotSize.TabIndex = 12;
            this.lblSteelSpotSize.Text = "Spot:";
            // 
            // txtSteelSpotSize
            // 
            this.txtSteelSpotSize.Location = new System.Drawing.Point(56, 85);
            this.txtSteelSpotSize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelSpotSize.Name = "txtSteelSpotSize";
            this.txtSteelSpotSize.Size = new System.Drawing.Size(38, 22);
            this.txtSteelSpotSize.TabIndex = 13;
            this.txtSteelSpotSize.Text = "0.05";
            // 
            // lblSteelQrRepeat
            // 
            this.lblSteelQrRepeat.AutoSize = true;
            this.lblSteelQrRepeat.Location = new System.Drawing.Point(101, 87);
            this.lblSteelQrRepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelQrRepeat.Name = "lblSteelQrRepeat";
            this.lblSteelQrRepeat.Size = new System.Drawing.Size(48, 12);
            this.lblSteelQrRepeat.TabIndex = 14;
            this.lblSteelQrRepeat.Text = "QR重複:";
            // 
            // txtSteelQrRepeat
            // 
            this.txtSteelQrRepeat.Location = new System.Drawing.Point(154, 85);
            this.txtSteelQrRepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelQrRepeat.Name = "txtSteelQrRepeat";
            this.txtSteelQrRepeat.Size = new System.Drawing.Size(38, 22);
            this.txtSteelQrRepeat.TabIndex = 15;
            this.txtSteelQrRepeat.Text = "2";
            // 
            // lblSteelRectPower
            // 
            this.lblSteelRectPower.AutoSize = true;
            this.lblSteelRectPower.Location = new System.Drawing.Point(6, 110);
            this.lblSteelRectPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelRectPower.Name = "lblSteelRectPower";
            this.lblSteelRectPower.Size = new System.Drawing.Size(44, 12);
            this.lblSteelRectPower.TabIndex = 16;
            this.lblSteelRectPower.Text = "底功率:";
            // 
            // txtSteelRectPower
            // 
            this.txtSteelRectPower.Location = new System.Drawing.Point(56, 107);
            this.txtSteelRectPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelRectPower.Name = "txtSteelRectPower";
            this.txtSteelRectPower.Size = new System.Drawing.Size(38, 22);
            this.txtSteelRectPower.TabIndex = 17;
            this.txtSteelRectPower.Text = "45";
            // 
            // lblSteelRectSpeed
            // 
            this.lblSteelRectSpeed.AutoSize = true;
            this.lblSteelRectSpeed.Location = new System.Drawing.Point(101, 110);
            this.lblSteelRectSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelRectSpeed.Name = "lblSteelRectSpeed";
            this.lblSteelRectSpeed.Size = new System.Drawing.Size(44, 12);
            this.lblSteelRectSpeed.TabIndex = 18;
            this.lblSteelRectSpeed.Text = "底速度:";
            // 
            // txtSteelRectSpeed
            // 
            this.txtSteelRectSpeed.Location = new System.Drawing.Point(154, 107);
            this.txtSteelRectSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelRectSpeed.Name = "txtSteelRectSpeed";
            this.txtSteelRectSpeed.Size = new System.Drawing.Size(38, 22);
            this.txtSteelRectSpeed.TabIndex = 19;
            this.txtSteelRectSpeed.Text = "3000";
            // 
            // lblSteelRectFreq
            // 
            this.lblSteelRectFreq.AutoSize = true;
            this.lblSteelRectFreq.Location = new System.Drawing.Point(6, 132);
            this.lblSteelRectFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelRectFreq.Name = "lblSteelRectFreq";
            this.lblSteelRectFreq.Size = new System.Drawing.Size(44, 12);
            this.lblSteelRectFreq.TabIndex = 20;
            this.lblSteelRectFreq.Text = "底頻率:";
            // 
            // txtSteelRectFreq
            // 
            this.txtSteelRectFreq.Location = new System.Drawing.Point(56, 130);
            this.txtSteelRectFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelRectFreq.Name = "txtSteelRectFreq";
            this.txtSteelRectFreq.Size = new System.Drawing.Size(38, 22);
            this.txtSteelRectFreq.TabIndex = 21;
            this.txtSteelRectFreq.Text = "80";
            // 
            // lblSteelRectRepeat
            // 
            this.lblSteelRectRepeat.AutoSize = true;
            this.lblSteelRectRepeat.Location = new System.Drawing.Point(101, 132);
            this.lblSteelRectRepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelRectRepeat.Name = "lblSteelRectRepeat";
            this.lblSteelRectRepeat.Size = new System.Drawing.Size(44, 12);
            this.lblSteelRectRepeat.TabIndex = 22;
            this.lblSteelRectRepeat.Text = "底重複:";
            // 
            // txtSteelRectRepeat
            // 
            this.txtSteelRectRepeat.Location = new System.Drawing.Point(154, 130);
            this.txtSteelRectRepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelRectRepeat.Name = "txtSteelRectRepeat";
            this.txtSteelRectRepeat.Size = new System.Drawing.Size(38, 22);
            this.txtSteelRectRepeat.TabIndex = 23;
            this.txtSteelRectRepeat.Text = "2";
            // 
            // lblSteelQrPower
            // 
            this.lblSteelQrPower.AutoSize = true;
            this.lblSteelQrPower.Location = new System.Drawing.Point(6, 154);
            this.lblSteelQrPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelQrPower.Name = "lblSteelQrPower";
            this.lblSteelQrPower.Size = new System.Drawing.Size(48, 12);
            this.lblSteelQrPower.TabIndex = 24;
            this.lblSteelQrPower.Text = "QR功率:";
            // 
            // txtSteelQrPower
            // 
            this.txtSteelQrPower.Location = new System.Drawing.Point(56, 152);
            this.txtSteelQrPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelQrPower.Name = "txtSteelQrPower";
            this.txtSteelQrPower.Size = new System.Drawing.Size(38, 22);
            this.txtSteelQrPower.TabIndex = 25;
            this.txtSteelQrPower.Text = "85";
            // 
            // lblSteelQrSpeed
            // 
            this.lblSteelQrSpeed.AutoSize = true;
            this.lblSteelQrSpeed.Location = new System.Drawing.Point(101, 154);
            this.lblSteelQrSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelQrSpeed.Name = "lblSteelQrSpeed";
            this.lblSteelQrSpeed.Size = new System.Drawing.Size(48, 12);
            this.lblSteelQrSpeed.TabIndex = 26;
            this.lblSteelQrSpeed.Text = "QR速度:";
            // 
            // txtSteelQrSpeed
            // 
            this.txtSteelQrSpeed.Location = new System.Drawing.Point(154, 152);
            this.txtSteelQrSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelQrSpeed.Name = "txtSteelQrSpeed";
            this.txtSteelQrSpeed.Size = new System.Drawing.Size(38, 22);
            this.txtSteelQrSpeed.TabIndex = 27;
            this.txtSteelQrSpeed.Text = "500";
            // 
            // lblSteelQrFreq
            // 
            this.lblSteelQrFreq.AutoSize = true;
            this.lblSteelQrFreq.Location = new System.Drawing.Point(6, 177);
            this.lblSteelQrFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelQrFreq.Name = "lblSteelQrFreq";
            this.lblSteelQrFreq.Size = new System.Drawing.Size(48, 12);
            this.lblSteelQrFreq.TabIndex = 28;
            this.lblSteelQrFreq.Text = "QR頻率:";
            // 
            // txtSteelQrFreq
            // 
            this.txtSteelQrFreq.Location = new System.Drawing.Point(56, 174);
            this.txtSteelQrFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelQrFreq.Name = "txtSteelQrFreq";
            this.txtSteelQrFreq.Size = new System.Drawing.Size(38, 22);
            this.txtSteelQrFreq.TabIndex = 29;
            this.txtSteelQrFreq.Text = "25";
            // 
            // lblSteelQrPulseWidth
            // 
            this.lblSteelQrPulseWidth.AutoSize = true;
            this.lblSteelQrPulseWidth.Location = new System.Drawing.Point(101, 177);
            this.lblSteelQrPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelQrPulseWidth.Name = "lblSteelQrPulseWidth";
            this.lblSteelQrPulseWidth.Size = new System.Drawing.Size(44, 12);
            this.lblSteelQrPulseWidth.TabIndex = 30;
            this.lblSteelQrPulseWidth.Text = "QR PW:";
            // 
            // txtSteelQrPulseWidth
            // 
            this.txtSteelQrPulseWidth.Location = new System.Drawing.Point(154, 174);
            this.txtSteelQrPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelQrPulseWidth.Name = "txtSteelQrPulseWidth";
            this.txtSteelQrPulseWidth.Size = new System.Drawing.Size(38, 22);
            this.txtSteelQrPulseWidth.TabIndex = 31;
            this.txtSteelQrPulseWidth.Text = "400";
            // 
            // lblSteelSerial
            // 
            this.lblSteelSerial.AutoSize = true;
            this.lblSteelSerial.Location = new System.Drawing.Point(6, 202);
            this.lblSteelSerial.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSteelSerial.Name = "lblSteelSerial";
            this.lblSteelSerial.Size = new System.Drawing.Size(32, 12);
            this.lblSteelSerial.TabIndex = 32;
            this.lblSteelSerial.Text = "序號:";
            // 
            // txtSteelSerial
            // 
            this.txtSteelSerial.Location = new System.Drawing.Point(56, 200);
            this.txtSteelSerial.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelSerial.Name = "txtSteelSerial";
            this.txtSteelSerial.ReadOnly = true;
            this.txtSteelSerial.Size = new System.Drawing.Size(136, 22);
            this.txtSteelSerial.TabIndex = 33;
            this.txtSteelSerial.Text = "(建立後自動填入)";
            // 
            // btnQRSteelPreview
            // 
            this.btnQRSteelPreview.Location = new System.Drawing.Point(5, 242);
            this.btnQRSteelPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQRSteelPreview.Name = "btnQRSteelPreview";
            this.btnQRSteelPreview.Size = new System.Drawing.Size(62, 32);
            this.btnQRSteelPreview.TabIndex = 99;
            this.btnQRSteelPreview.Text = "紅光預覽";
            this.btnQRSteelPreview.UseVisualStyleBackColor = true;
            this.btnQRSteelPreview.Click += new System.EventHandler(this.btnQRSteelPreview_Click);
            // 
            // btnQRSteelMark
            // 
            this.btnQRSteelMark.Location = new System.Drawing.Point(70, 242);
            this.btnQRSteelMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQRSteelMark.Name = "btnQRSteelMark";
            this.btnQRSteelMark.Size = new System.Drawing.Size(62, 32);
            this.btnQRSteelMark.TabIndex = 100;
            this.btnQRSteelMark.Text = "雕刻 QR";
            this.btnQRSteelMark.UseVisualStyleBackColor = true;
            this.btnQRSteelMark.Click += new System.EventHandler(this.btnQRSteelMark_Click);
            // 
            // btnQRSteelStopPreview
            // 
            this.btnQRSteelStopPreview.Location = new System.Drawing.Point(134, 242);
            this.btnQRSteelStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQRSteelStopPreview.Name = "btnQRSteelStopPreview";
            this.btnQRSteelStopPreview.Size = new System.Drawing.Size(62, 32);
            this.btnQRSteelStopPreview.TabIndex = 101;
            this.btnQRSteelStopPreview.Text = "預覽停止";
            this.btnQRSteelStopPreview.UseVisualStyleBackColor = true;
            this.btnQRSteelStopPreview.Click += new System.EventHandler(this.btnQRSteelStopPreview_Click);
            // 
            // groupBoxSteelTime
            // 
            this.groupBoxSteelTime.Controls.Add(this.txtSteelTimeInfo);
            this.groupBoxSteelTime.Location = new System.Drawing.Point(338, 330);
            this.groupBoxSteelTime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxSteelTime.Name = "groupBoxSteelTime";
            this.groupBoxSteelTime.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxSteelTime.Size = new System.Drawing.Size(410, 114);
            this.groupBoxSteelTime.TabIndex = 40;
            this.groupBoxSteelTime.TabStop = false;
            this.groupBoxSteelTime.Text = "預估時間";
            // 
            // txtSteelTimeInfo
            // 
            this.txtSteelTimeInfo.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtSteelTimeInfo.Location = new System.Drawing.Point(207, 19);
            this.txtSteelTimeInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSteelTimeInfo.Multiline = true;
            this.txtSteelTimeInfo.Name = "txtSteelTimeInfo";
            this.txtSteelTimeInfo.ReadOnly = true;
            this.txtSteelTimeInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSteelTimeInfo.Size = new System.Drawing.Size(199, 91);
            this.txtSteelTimeInfo.TabIndex = 0;
            this.txtSteelTimeInfo.Text = "(按下「紅光預覽」或「雕刻 QR」後自動填入)";
            // 
            // groupBoxRectAlone
            // 
            this.groupBoxRectAlone.Controls.Add(this.lblRAWidth);
            this.groupBoxRectAlone.Controls.Add(this.txtRAWidth);
            this.groupBoxRectAlone.Controls.Add(this.lblRAHeight);
            this.groupBoxRectAlone.Controls.Add(this.txtRAHeight);
            this.groupBoxRectAlone.Controls.Add(this.lblRAX);
            this.groupBoxRectAlone.Controls.Add(this.txtRAX);
            this.groupBoxRectAlone.Controls.Add(this.lblRAY);
            this.groupBoxRectAlone.Controls.Add(this.txtRAY);
            this.groupBoxRectAlone.Controls.Add(this.lblRASpeed);
            this.groupBoxRectAlone.Controls.Add(this.txtRASpeed);
            this.groupBoxRectAlone.Controls.Add(this.lblRAPower);
            this.groupBoxRectAlone.Controls.Add(this.txtRAPower);
            this.groupBoxRectAlone.Controls.Add(this.lblRAFreq);
            this.groupBoxRectAlone.Controls.Add(this.txtRAFreq);
            this.groupBoxRectAlone.Controls.Add(this.lblRARepeat);
            this.groupBoxRectAlone.Controls.Add(this.txtRARepeat);
            this.groupBoxRectAlone.Controls.Add(this.lblRAPulseWidth);
            this.groupBoxRectAlone.Controls.Add(this.txtRAPulseWidth);
            this.groupBoxRectAlone.Controls.Add(this.lblRAFillStyle);
            this.groupBoxRectAlone.Controls.Add(this.txtRAFillStyle);
            this.groupBoxRectAlone.Controls.Add(this.lblRAFrameLineType);
            this.groupBoxRectAlone.Controls.Add(this.txtRAFrameLineType);
            this.groupBoxRectAlone.Controls.Add(this.btnRAPreview);
            this.groupBoxRectAlone.Controls.Add(this.btnRAStopPreview);
            this.groupBoxRectAlone.Controls.Add(this.btnRAMark);
            this.groupBoxRectAlone.Location = new System.Drawing.Point(752, 34);
            this.groupBoxRectAlone.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxRectAlone.Name = "groupBoxRectAlone";
            this.groupBoxRectAlone.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxRectAlone.Size = new System.Drawing.Size(315, 200);
            this.groupBoxRectAlone.TabIndex = 60;
            this.groupBoxRectAlone.TabStop = false;
            this.groupBoxRectAlone.Text = "矩形（獨立）";
            // 
            // lblRAWidth
            // 
            this.lblRAWidth.AutoSize = true;
            this.lblRAWidth.Location = new System.Drawing.Point(8, 20);
            this.lblRAWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAWidth.Name = "lblRAWidth";
            this.lblRAWidth.Size = new System.Drawing.Size(46, 12);
            this.lblRAWidth.TabIndex = 0;
            this.lblRAWidth.Text = "寬(mm):";
            // 
            // txtRAWidth
            // 
            this.txtRAWidth.Location = new System.Drawing.Point(56, 18);
            this.txtRAWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAWidth.Name = "txtRAWidth";
            this.txtRAWidth.Size = new System.Drawing.Size(46, 22);
            this.txtRAWidth.TabIndex = 1;
            this.txtRAWidth.Text = "40";
            // 
            // lblRAHeight
            // 
            this.lblRAHeight.AutoSize = true;
            this.lblRAHeight.Location = new System.Drawing.Point(150, 20);
            this.lblRAHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAHeight.Name = "lblRAHeight";
            this.lblRAHeight.Size = new System.Drawing.Size(46, 12);
            this.lblRAHeight.TabIndex = 2;
            this.lblRAHeight.Text = "高(mm):";
            // 
            // txtRAHeight
            // 
            this.txtRAHeight.Location = new System.Drawing.Point(199, 18);
            this.txtRAHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAHeight.Name = "txtRAHeight";
            this.txtRAHeight.Size = new System.Drawing.Size(46, 22);
            this.txtRAHeight.TabIndex = 3;
            this.txtRAHeight.Text = "40";
            // 
            // lblRAX
            // 
            this.lblRAX.AutoSize = true;
            this.lblRAX.Location = new System.Drawing.Point(8, 44);
            this.lblRAX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAX.Name = "lblRAX";
            this.lblRAX.Size = new System.Drawing.Size(42, 12);
            this.lblRAX.TabIndex = 4;
            this.lblRAX.Text = "X(mm):";
            // 
            // txtRAX
            // 
            this.txtRAX.Location = new System.Drawing.Point(56, 42);
            this.txtRAX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAX.Name = "txtRAX";
            this.txtRAX.Size = new System.Drawing.Size(46, 22);
            this.txtRAX.TabIndex = 5;
            this.txtRAX.Text = "0";
            // 
            // lblRAY
            // 
            this.lblRAY.AutoSize = true;
            this.lblRAY.Location = new System.Drawing.Point(150, 44);
            this.lblRAY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAY.Name = "lblRAY";
            this.lblRAY.Size = new System.Drawing.Size(42, 12);
            this.lblRAY.TabIndex = 6;
            this.lblRAY.Text = "Y(mm):";
            // 
            // txtRAY
            // 
            this.txtRAY.Location = new System.Drawing.Point(199, 42);
            this.txtRAY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAY.Name = "txtRAY";
            this.txtRAY.Size = new System.Drawing.Size(46, 22);
            this.txtRAY.TabIndex = 7;
            this.txtRAY.Text = "0";
            // 
            // lblRASpeed
            // 
            this.lblRASpeed.AutoSize = true;
            this.lblRASpeed.Location = new System.Drawing.Point(8, 68);
            this.lblRASpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRASpeed.Name = "lblRASpeed";
            this.lblRASpeed.Size = new System.Drawing.Size(32, 12);
            this.lblRASpeed.TabIndex = 8;
            this.lblRASpeed.Text = "速度:";
            // 
            // txtRASpeed
            // 
            this.txtRASpeed.Location = new System.Drawing.Point(56, 66);
            this.txtRASpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRASpeed.Name = "txtRASpeed";
            this.txtRASpeed.Size = new System.Drawing.Size(46, 22);
            this.txtRASpeed.TabIndex = 9;
            this.txtRASpeed.Text = "1000";
            // 
            // lblRAPower
            // 
            this.lblRAPower.AutoSize = true;
            this.lblRAPower.Location = new System.Drawing.Point(150, 68);
            this.lblRAPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAPower.Name = "lblRAPower";
            this.lblRAPower.Size = new System.Drawing.Size(49, 12);
            this.lblRAPower.TabIndex = 10;
            this.lblRAPower.Text = "功率(%):";
            // 
            // txtRAPower
            // 
            this.txtRAPower.Location = new System.Drawing.Point(199, 66);
            this.txtRAPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAPower.Name = "txtRAPower";
            this.txtRAPower.Size = new System.Drawing.Size(46, 22);
            this.txtRAPower.TabIndex = 11;
            this.txtRAPower.Text = "50";
            // 
            // lblRAFreq
            // 
            this.lblRAFreq.AutoSize = true;
            this.lblRAFreq.Location = new System.Drawing.Point(8, 92);
            this.lblRAFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAFreq.Name = "lblRAFreq";
            this.lblRAFreq.Size = new System.Drawing.Size(59, 12);
            this.lblRAFreq.TabIndex = 12;
            this.lblRAFreq.Text = "頻率(kHz):";
            // 
            // txtRAFreq
            // 
            this.txtRAFreq.Location = new System.Drawing.Point(64, 90);
            this.txtRAFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAFreq.Name = "txtRAFreq";
            this.txtRAFreq.Size = new System.Drawing.Size(38, 22);
            this.txtRAFreq.TabIndex = 13;
            this.txtRAFreq.Text = "20";
            // 
            // lblRARepeat
            // 
            this.lblRARepeat.AutoSize = true;
            this.lblRARepeat.Location = new System.Drawing.Point(150, 92);
            this.lblRARepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRARepeat.Name = "lblRARepeat";
            this.lblRARepeat.Size = new System.Drawing.Size(32, 12);
            this.lblRARepeat.TabIndex = 14;
            this.lblRARepeat.Text = "次數:";
            // 
            // txtRARepeat
            // 
            this.txtRARepeat.Location = new System.Drawing.Point(199, 90);
            this.txtRARepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRARepeat.Name = "txtRARepeat";
            this.txtRARepeat.Size = new System.Drawing.Size(46, 22);
            this.txtRARepeat.TabIndex = 15;
            this.txtRARepeat.Text = "1";
            // 
            // lblRAPulseWidth
            // 
            this.lblRAPulseWidth.AutoSize = true;
            this.lblRAPulseWidth.Location = new System.Drawing.Point(8, 116);
            this.lblRAPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAPulseWidth.Name = "lblRAPulseWidth";
            this.lblRAPulseWidth.Size = new System.Drawing.Size(56, 12);
            this.lblRAPulseWidth.TabIndex = 16;
            this.lblRAPulseWidth.Text = "脈衝寬度:";
            // 
            // txtRAPulseWidth
            // 
            this.txtRAPulseWidth.Location = new System.Drawing.Point(64, 114);
            this.txtRAPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAPulseWidth.Name = "txtRAPulseWidth";
            this.txtRAPulseWidth.Size = new System.Drawing.Size(38, 22);
            this.txtRAPulseWidth.TabIndex = 17;
            this.txtRAPulseWidth.Text = "100";
            // 
            // lblRAFillStyle
            // 
            this.lblRAFillStyle.AutoSize = true;
            this.lblRAFillStyle.Location = new System.Drawing.Point(8, 140);
            this.lblRAFillStyle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAFillStyle.Name = "lblRAFillStyle";
            this.lblRAFillStyle.Size = new System.Drawing.Size(46, 12);
            this.lblRAFillStyle.TabIndex = 18;
            this.lblRAFillStyle.Text = "FillStyle:";
            // 
            // txtRAFillStyle
            // 
            this.txtRAFillStyle.Location = new System.Drawing.Point(64, 138);
            this.txtRAFillStyle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAFillStyle.Name = "txtRAFillStyle";
            this.txtRAFillStyle.Size = new System.Drawing.Size(38, 22);
            this.txtRAFillStyle.TabIndex = 19;
            this.txtRAFillStyle.Text = "0";
            // 
            // lblRAFrameLineType
            // 
            this.lblRAFrameLineType.AutoSize = true;
            this.lblRAFrameLineType.Location = new System.Drawing.Point(120, 140);
            this.lblRAFrameLineType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRAFrameLineType.Name = "lblRAFrameLineType";
            this.lblRAFrameLineType.Size = new System.Drawing.Size(77, 12);
            this.lblRAFrameLineType.TabIndex = 20;
            this.lblRAFrameLineType.Text = "外框LineType:";
            // 
            // txtRAFrameLineType
            // 
            this.txtRAFrameLineType.Location = new System.Drawing.Point(199, 138);
            this.txtRAFrameLineType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRAFrameLineType.Name = "txtRAFrameLineType";
            this.txtRAFrameLineType.Size = new System.Drawing.Size(46, 22);
            this.txtRAFrameLineType.TabIndex = 21;
            this.txtRAFrameLineType.Text = "0";
            // 
            // btnRAPreview
            // 
            this.btnRAPreview.Location = new System.Drawing.Point(8, 161);
            this.btnRAPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRAPreview.Name = "btnRAPreview";
            this.btnRAPreview.Size = new System.Drawing.Size(86, 36);
            this.btnRAPreview.TabIndex = 22;
            this.btnRAPreview.Text = "紅光預覽";
            this.btnRAPreview.UseVisualStyleBackColor = true;
            this.btnRAPreview.Click += new System.EventHandler(this.btnRAPreview_Click);
            // 
            // btnRAStopPreview
            // 
            this.btnRAStopPreview.Location = new System.Drawing.Point(101, 161);
            this.btnRAStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRAStopPreview.Name = "btnRAStopPreview";
            this.btnRAStopPreview.Size = new System.Drawing.Size(86, 36);
            this.btnRAStopPreview.TabIndex = 23;
            this.btnRAStopPreview.Text = "預覽停止";
            this.btnRAStopPreview.UseVisualStyleBackColor = true;
            this.btnRAStopPreview.Click += new System.EventHandler(this.btnRAStopPreview_Click);
            // 
            // btnRAMark
            // 
            this.btnRAMark.Location = new System.Drawing.Point(195, 161);
            this.btnRAMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRAMark.Name = "btnRAMark";
            this.btnRAMark.Size = new System.Drawing.Size(86, 36);
            this.btnRAMark.TabIndex = 24;
            this.btnRAMark.Text = "雕刻";
            this.btnRAMark.UseVisualStyleBackColor = true;
            this.btnRAMark.Click += new System.EventHandler(this.btnRAMark_Click);
            // 
            // groupBoxQRAlone
            // 
            this.groupBoxQRAlone.Controls.Add(this.lblQAContent);
            this.groupBoxQRAlone.Controls.Add(this.txtQAContent);
            this.groupBoxQRAlone.Controls.Add(this.lblQAWidth);
            this.groupBoxQRAlone.Controls.Add(this.txtQAWidth);
            this.groupBoxQRAlone.Controls.Add(this.lblQAHeight);
            this.groupBoxQRAlone.Controls.Add(this.txtQAHeight);
            this.groupBoxQRAlone.Controls.Add(this.lblQAX);
            this.groupBoxQRAlone.Controls.Add(this.txtQAX);
            this.groupBoxQRAlone.Controls.Add(this.lblQAY);
            this.groupBoxQRAlone.Controls.Add(this.txtQAY);
            this.groupBoxQRAlone.Controls.Add(this.lblQABorder);
            this.groupBoxQRAlone.Controls.Add(this.txtQABorder);
            this.groupBoxQRAlone.Controls.Add(this.lblQAECLevel);
            this.groupBoxQRAlone.Controls.Add(this.txtQAECLevel);
            this.groupBoxQRAlone.Controls.Add(this.lblQAMarkStyle);
            this.groupBoxQRAlone.Controls.Add(this.txtQAMarkStyle);
            this.groupBoxQRAlone.Controls.Add(this.chkQAInvert);
            this.groupBoxQRAlone.Controls.Add(this.lblQASpeed);
            this.groupBoxQRAlone.Controls.Add(this.txtQASpeed);
            this.groupBoxQRAlone.Controls.Add(this.lblQAPower);
            this.groupBoxQRAlone.Controls.Add(this.txtQAPower);
            this.groupBoxQRAlone.Controls.Add(this.lblQAFreq);
            this.groupBoxQRAlone.Controls.Add(this.txtQAFreq);
            this.groupBoxQRAlone.Controls.Add(this.lblQARepeat);
            this.groupBoxQRAlone.Controls.Add(this.txtQARepeat);
            this.groupBoxQRAlone.Controls.Add(this.lblQAPulseWidth);
            this.groupBoxQRAlone.Controls.Add(this.txtQAPulseWidth);
            this.groupBoxQRAlone.Controls.Add(this.btnQAPreview);
            this.groupBoxQRAlone.Controls.Add(this.btnQAStopPreview);
            this.groupBoxQRAlone.Controls.Add(this.btnQAMark);
            this.groupBoxQRAlone.Location = new System.Drawing.Point(752, 238);
            this.groupBoxQRAlone.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxQRAlone.Name = "groupBoxQRAlone";
            this.groupBoxQRAlone.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxQRAlone.Size = new System.Drawing.Size(315, 258);
            this.groupBoxQRAlone.TabIndex = 61;
            this.groupBoxQRAlone.TabStop = false;
            this.groupBoxQRAlone.Text = "QR Code（獨立）";
            // 
            // lblQAContent
            // 
            this.lblQAContent.AutoSize = true;
            this.lblQAContent.Location = new System.Drawing.Point(8, 20);
            this.lblQAContent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAContent.Name = "lblQAContent";
            this.lblQAContent.Size = new System.Drawing.Size(32, 12);
            this.lblQAContent.TabIndex = 0;
            this.lblQAContent.Text = "內容:";
            // 
            // txtQAContent
            // 
            this.txtQAContent.Location = new System.Drawing.Point(56, 18);
            this.txtQAContent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAContent.Name = "txtQAContent";
            this.txtQAContent.Size = new System.Drawing.Size(248, 22);
            this.txtQAContent.TabIndex = 1;
            this.txtQAContent.Text = "AAA";
            // 
            // lblQAWidth
            // 
            this.lblQAWidth.AutoSize = true;
            this.lblQAWidth.Location = new System.Drawing.Point(8, 44);
            this.lblQAWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAWidth.Name = "lblQAWidth";
            this.lblQAWidth.Size = new System.Drawing.Size(46, 12);
            this.lblQAWidth.TabIndex = 2;
            this.lblQAWidth.Text = "寬(mm):";
            // 
            // txtQAWidth
            // 
            this.txtQAWidth.Location = new System.Drawing.Point(56, 42);
            this.txtQAWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAWidth.Name = "txtQAWidth";
            this.txtQAWidth.Size = new System.Drawing.Size(46, 22);
            this.txtQAWidth.TabIndex = 3;
            this.txtQAWidth.Text = "20";
            // 
            // lblQAHeight
            // 
            this.lblQAHeight.AutoSize = true;
            this.lblQAHeight.Location = new System.Drawing.Point(158, 44);
            this.lblQAHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAHeight.Name = "lblQAHeight";
            this.lblQAHeight.Size = new System.Drawing.Size(46, 12);
            this.lblQAHeight.TabIndex = 4;
            this.lblQAHeight.Text = "高(mm):";
            // 
            // txtQAHeight
            // 
            this.txtQAHeight.Location = new System.Drawing.Point(206, 42);
            this.txtQAHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAHeight.Name = "txtQAHeight";
            this.txtQAHeight.Size = new System.Drawing.Size(46, 22);
            this.txtQAHeight.TabIndex = 5;
            this.txtQAHeight.Text = "20";
            // 
            // lblQAX
            // 
            this.lblQAX.AutoSize = true;
            this.lblQAX.Location = new System.Drawing.Point(8, 68);
            this.lblQAX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAX.Name = "lblQAX";
            this.lblQAX.Size = new System.Drawing.Size(42, 12);
            this.lblQAX.TabIndex = 6;
            this.lblQAX.Text = "X(mm):";
            // 
            // txtQAX
            // 
            this.txtQAX.Location = new System.Drawing.Point(56, 66);
            this.txtQAX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAX.Name = "txtQAX";
            this.txtQAX.Size = new System.Drawing.Size(46, 22);
            this.txtQAX.TabIndex = 7;
            this.txtQAX.Text = "0";
            // 
            // lblQAY
            // 
            this.lblQAY.AutoSize = true;
            this.lblQAY.Location = new System.Drawing.Point(158, 68);
            this.lblQAY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAY.Name = "lblQAY";
            this.lblQAY.Size = new System.Drawing.Size(42, 12);
            this.lblQAY.TabIndex = 8;
            this.lblQAY.Text = "Y(mm):";
            // 
            // txtQAY
            // 
            this.txtQAY.Location = new System.Drawing.Point(206, 66);
            this.txtQAY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAY.Name = "txtQAY";
            this.txtQAY.Size = new System.Drawing.Size(46, 22);
            this.txtQAY.TabIndex = 9;
            this.txtQAY.Text = "0";
            // 
            // lblQABorder
            // 
            this.lblQABorder.AutoSize = true;
            this.lblQABorder.Location = new System.Drawing.Point(8, 92);
            this.lblQABorder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQABorder.Name = "lblQABorder";
            this.lblQABorder.Size = new System.Drawing.Size(41, 12);
            this.lblQABorder.TabIndex = 10;
            this.lblQABorder.Text = "Border:";
            // 
            // txtQABorder
            // 
            this.txtQABorder.Location = new System.Drawing.Point(56, 90);
            this.txtQABorder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQABorder.Name = "txtQABorder";
            this.txtQABorder.Size = new System.Drawing.Size(46, 22);
            this.txtQABorder.TabIndex = 11;
            this.txtQABorder.Text = "4";
            // 
            // lblQAECLevel
            // 
            this.lblQAECLevel.AutoSize = true;
            this.lblQAECLevel.Location = new System.Drawing.Point(158, 92);
            this.lblQAECLevel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAECLevel.Name = "lblQAECLevel";
            this.lblQAECLevel.Size = new System.Drawing.Size(47, 12);
            this.lblQAECLevel.TabIndex = 12;
            this.lblQAECLevel.Text = "EC等級:";
            // 
            // txtQAECLevel
            // 
            this.txtQAECLevel.Location = new System.Drawing.Point(206, 90);
            this.txtQAECLevel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAECLevel.Name = "txtQAECLevel";
            this.txtQAECLevel.Size = new System.Drawing.Size(46, 22);
            this.txtQAECLevel.TabIndex = 13;
            this.txtQAECLevel.Text = "1";
            // 
            // lblQAMarkStyle
            // 
            this.lblQAMarkStyle.AutoSize = true;
            this.lblQAMarkStyle.Location = new System.Drawing.Point(8, 116);
            this.lblQAMarkStyle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAMarkStyle.Name = "lblQAMarkStyle";
            this.lblQAMarkStyle.Size = new System.Drawing.Size(56, 12);
            this.lblQAMarkStyle.TabIndex = 14;
            this.lblQAMarkStyle.Text = "MarkStyle:";
            // 
            // txtQAMarkStyle
            // 
            this.txtQAMarkStyle.Location = new System.Drawing.Point(64, 114);
            this.txtQAMarkStyle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAMarkStyle.Name = "txtQAMarkStyle";
            this.txtQAMarkStyle.Size = new System.Drawing.Size(38, 22);
            this.txtQAMarkStyle.TabIndex = 15;
            this.txtQAMarkStyle.Text = "1";
            // 
            // chkQAInvert
            // 
            this.chkQAInvert.AutoSize = true;
            this.chkQAInvert.Location = new System.Drawing.Point(158, 115);
            this.chkQAInvert.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkQAInvert.Name = "chkQAInvert";
            this.chkQAInvert.Size = new System.Drawing.Size(48, 16);
            this.chkQAInvert.TabIndex = 16;
            this.chkQAInvert.Text = "反相";
            this.chkQAInvert.UseVisualStyleBackColor = true;
            // 
            // lblQASpeed
            // 
            this.lblQASpeed.AutoSize = true;
            this.lblQASpeed.Location = new System.Drawing.Point(8, 140);
            this.lblQASpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQASpeed.Name = "lblQASpeed";
            this.lblQASpeed.Size = new System.Drawing.Size(32, 12);
            this.lblQASpeed.TabIndex = 17;
            this.lblQASpeed.Text = "速度:";
            // 
            // txtQASpeed
            // 
            this.txtQASpeed.Location = new System.Drawing.Point(56, 138);
            this.txtQASpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQASpeed.Name = "txtQASpeed";
            this.txtQASpeed.Size = new System.Drawing.Size(46, 22);
            this.txtQASpeed.TabIndex = 18;
            this.txtQASpeed.Text = "500";
            // 
            // lblQAPower
            // 
            this.lblQAPower.AutoSize = true;
            this.lblQAPower.Location = new System.Drawing.Point(158, 140);
            this.lblQAPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAPower.Name = "lblQAPower";
            this.lblQAPower.Size = new System.Drawing.Size(49, 12);
            this.lblQAPower.TabIndex = 19;
            this.lblQAPower.Text = "功率(%):";
            // 
            // txtQAPower
            // 
            this.txtQAPower.Location = new System.Drawing.Point(206, 138);
            this.txtQAPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAPower.Name = "txtQAPower";
            this.txtQAPower.Size = new System.Drawing.Size(46, 22);
            this.txtQAPower.TabIndex = 20;
            this.txtQAPower.Text = "80";
            // 
            // lblQAFreq
            // 
            this.lblQAFreq.AutoSize = true;
            this.lblQAFreq.Location = new System.Drawing.Point(8, 164);
            this.lblQAFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAFreq.Name = "lblQAFreq";
            this.lblQAFreq.Size = new System.Drawing.Size(59, 12);
            this.lblQAFreq.TabIndex = 21;
            this.lblQAFreq.Text = "頻率(kHz):";
            // 
            // txtQAFreq
            // 
            this.txtQAFreq.Location = new System.Drawing.Point(64, 162);
            this.txtQAFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAFreq.Name = "txtQAFreq";
            this.txtQAFreq.Size = new System.Drawing.Size(38, 22);
            this.txtQAFreq.TabIndex = 22;
            this.txtQAFreq.Text = "25";
            // 
            // lblQARepeat
            // 
            this.lblQARepeat.AutoSize = true;
            this.lblQARepeat.Location = new System.Drawing.Point(158, 164);
            this.lblQARepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQARepeat.Name = "lblQARepeat";
            this.lblQARepeat.Size = new System.Drawing.Size(32, 12);
            this.lblQARepeat.TabIndex = 23;
            this.lblQARepeat.Text = "次數:";
            // 
            // txtQARepeat
            // 
            this.txtQARepeat.Location = new System.Drawing.Point(206, 162);
            this.txtQARepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQARepeat.Name = "txtQARepeat";
            this.txtQARepeat.Size = new System.Drawing.Size(46, 22);
            this.txtQARepeat.TabIndex = 24;
            this.txtQARepeat.Text = "2";
            // 
            // lblQAPulseWidth
            // 
            this.lblQAPulseWidth.AutoSize = true;
            this.lblQAPulseWidth.Location = new System.Drawing.Point(8, 188);
            this.lblQAPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQAPulseWidth.Name = "lblQAPulseWidth";
            this.lblQAPulseWidth.Size = new System.Drawing.Size(56, 12);
            this.lblQAPulseWidth.TabIndex = 25;
            this.lblQAPulseWidth.Text = "脈衝寬度:";
            // 
            // txtQAPulseWidth
            // 
            this.txtQAPulseWidth.Location = new System.Drawing.Point(64, 186);
            this.txtQAPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQAPulseWidth.Name = "txtQAPulseWidth";
            this.txtQAPulseWidth.Size = new System.Drawing.Size(38, 22);
            this.txtQAPulseWidth.TabIndex = 26;
            this.txtQAPulseWidth.Text = "300";
            // 
            // btnQAPreview
            // 
            this.btnQAPreview.Location = new System.Drawing.Point(8, 212);
            this.btnQAPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQAPreview.Name = "btnQAPreview";
            this.btnQAPreview.Size = new System.Drawing.Size(94, 36);
            this.btnQAPreview.TabIndex = 27;
            this.btnQAPreview.Text = "紅光預覽";
            this.btnQAPreview.UseVisualStyleBackColor = true;
            this.btnQAPreview.Click += new System.EventHandler(this.btnQAPreview_Click);
            // 
            // btnQAStopPreview
            // 
            this.btnQAStopPreview.Location = new System.Drawing.Point(109, 212);
            this.btnQAStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQAStopPreview.Name = "btnQAStopPreview";
            this.btnQAStopPreview.Size = new System.Drawing.Size(94, 36);
            this.btnQAStopPreview.TabIndex = 28;
            this.btnQAStopPreview.Text = "預覽停止";
            this.btnQAStopPreview.UseVisualStyleBackColor = true;
            this.btnQAStopPreview.Click += new System.EventHandler(this.btnQAStopPreview_Click);
            // 
            // btnQAMark
            // 
            this.btnQAMark.Location = new System.Drawing.Point(210, 212);
            this.btnQAMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQAMark.Name = "btnQAMark";
            this.btnQAMark.Size = new System.Drawing.Size(94, 36);
            this.btnQAMark.TabIndex = 29;
            this.btnQAMark.Text = "雕刻";
            this.btnQAMark.UseVisualStyleBackColor = true;
            this.btnQAMark.Click += new System.EventHandler(this.btnQAMark_Click);
            // 
            // tabPageQRCode2
            // 
            this.tabPageQRCode2.Controls.Add(this.lblBoardQR2);
            this.tabPageQRCode2.Controls.Add(this.comboBoardQR2);
            this.tabPageQRCode2.Controls.Add(this.groupBoxBlackRect);
            this.tabPageQRCode2.Controls.Add(this.groupBoxWhiteRect);
            this.tabPageQRCode2.Controls.Add(this.groupBoxQROnly);
            this.tabPageQRCode2.Controls.Add(this.btnAllPreview);
            this.tabPageQRCode2.Controls.Add(this.btnAllStopPreview);
            this.tabPageQRCode2.Controls.Add(this.btnAllMark);
            this.tabPageQRCode2.Location = new System.Drawing.Point(4, 22);
            this.tabPageQRCode2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageQRCode2.Name = "tabPageQRCode2";
            this.tabPageQRCode2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageQRCode2.Size = new System.Drawing.Size(1197, 566);
            this.tabPageQRCode2.TabIndex = 8;
            this.tabPageQRCode2.Text = "6-1. QR Code";
            this.tabPageQRCode2.UseVisualStyleBackColor = true;
            // 
            // lblBoardQR2
            // 
            this.lblBoardQR2.AutoSize = true;
            this.lblBoardQR2.Location = new System.Drawing.Point(15, 12);
            this.lblBoardQR2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoardQR2.Name = "lblBoardQR2";
            this.lblBoardQR2.Size = new System.Drawing.Size(53, 12);
            this.lblBoardQR2.TabIndex = 0;
            this.lblBoardQR2.Text = "選擇板：";
            // 
            // comboBoardQR2
            // 
            this.comboBoardQR2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoardQR2.FormattingEnabled = true;
            this.comboBoardQR2.Items.AddRange(new object[] {
            "板 1",
            "板 2",
            "板 3",
            "板 4"});
            this.comboBoardQR2.Location = new System.Drawing.Point(68, 10);
            this.comboBoardQR2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoardQR2.Name = "comboBoardQR2";
            this.comboBoardQR2.Size = new System.Drawing.Size(92, 20);
            this.comboBoardQR2.TabIndex = 1;
            // 
            // groupBoxBlackRect
            // 
            this.groupBoxBlackRect.Controls.Add(this.lblBRWidth);
            this.groupBoxBlackRect.Controls.Add(this.txtBRWidth);
            this.groupBoxBlackRect.Controls.Add(this.lblBRHeight);
            this.groupBoxBlackRect.Controls.Add(this.txtBRHeight);
            this.groupBoxBlackRect.Controls.Add(this.lblBRSpeed);
            this.groupBoxBlackRect.Controls.Add(this.txtBRSpeed);
            this.groupBoxBlackRect.Controls.Add(this.lblBRPower);
            this.groupBoxBlackRect.Controls.Add(this.txtBRPower);
            this.groupBoxBlackRect.Controls.Add(this.lblBRFreq);
            this.groupBoxBlackRect.Controls.Add(this.txtBRFreq);
            this.groupBoxBlackRect.Controls.Add(this.lblBRRepeat);
            this.groupBoxBlackRect.Controls.Add(this.txtBRRepeat);
            this.groupBoxBlackRect.Controls.Add(this.lblBRSpotDelay);
            this.groupBoxBlackRect.Controls.Add(this.txtBRSpotDelay);
            this.groupBoxBlackRect.Controls.Add(this.lblBRPulseWidth);
            this.groupBoxBlackRect.Controls.Add(this.txtBRPulseWidth);
            this.groupBoxBlackRect.Controls.Add(this.lblBRFillPitch);
            this.groupBoxBlackRect.Controls.Add(this.txtBRFillPitch);
            this.groupBoxBlackRect.Controls.Add(this.lblBRFillRoundPitch);
            this.groupBoxBlackRect.Controls.Add(this.txtBRFillRoundPitch);
            this.groupBoxBlackRect.Controls.Add(this.lblBRFillTimes);
            this.groupBoxBlackRect.Controls.Add(this.txtBRFillTimes);
            this.groupBoxBlackRect.Controls.Add(this.lblBRFillStepAngle);
            this.groupBoxBlackRect.Controls.Add(this.txtBRFillStepAngle);
            this.groupBoxBlackRect.Controls.Add(this.btnBRPreview);
            this.groupBoxBlackRect.Controls.Add(this.btnBRStopPreview);
            this.groupBoxBlackRect.Controls.Add(this.btnBRMark);
            this.groupBoxBlackRect.Location = new System.Drawing.Point(15, 36);
            this.groupBoxBlackRect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxBlackRect.Name = "groupBoxBlackRect";
            this.groupBoxBlackRect.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxBlackRect.Size = new System.Drawing.Size(345, 492);
            this.groupBoxBlackRect.TabIndex = 2;
            this.groupBoxBlackRect.TabStop = false;
            this.groupBoxBlackRect.Text = "① 打黑矩形（凹刻深黑）";
            // 
            // lblBRWidth
            // 
            this.lblBRWidth.AutoSize = true;
            this.lblBRWidth.Location = new System.Drawing.Point(11, 26);
            this.lblBRWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRWidth.Name = "lblBRWidth";
            this.lblBRWidth.Size = new System.Drawing.Size(49, 12);
            this.lblBRWidth.TabIndex = 0;
            this.lblBRWidth.Text = "寬 (mm):";
            // 
            // txtBRWidth
            // 
            this.txtBRWidth.Location = new System.Drawing.Point(105, 24);
            this.txtBRWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRWidth.Name = "txtBRWidth";
            this.txtBRWidth.Size = new System.Drawing.Size(61, 22);
            this.txtBRWidth.TabIndex = 1;
            this.txtBRWidth.Text = "50";
            // 
            // lblBRHeight
            // 
            this.lblBRHeight.AutoSize = true;
            this.lblBRHeight.Location = new System.Drawing.Point(184, 26);
            this.lblBRHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRHeight.Name = "lblBRHeight";
            this.lblBRHeight.Size = new System.Drawing.Size(49, 12);
            this.lblBRHeight.TabIndex = 2;
            this.lblBRHeight.Text = "高 (mm):";
            // 
            // txtBRHeight
            // 
            this.txtBRHeight.Location = new System.Drawing.Point(278, 24);
            this.txtBRHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRHeight.Name = "txtBRHeight";
            this.txtBRHeight.Size = new System.Drawing.Size(61, 22);
            this.txtBRHeight.TabIndex = 3;
            this.txtBRHeight.Text = "50";
            // 
            // lblBRSpeed
            // 
            this.lblBRSpeed.AutoSize = true;
            this.lblBRSpeed.Location = new System.Drawing.Point(11, 54);
            this.lblBRSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRSpeed.Name = "lblBRSpeed";
            this.lblBRSpeed.Size = new System.Drawing.Size(68, 12);
            this.lblBRSpeed.TabIndex = 4;
            this.lblBRSpeed.Text = "速度 (mm/s):";
            // 
            // txtBRSpeed
            // 
            this.txtBRSpeed.Location = new System.Drawing.Point(105, 52);
            this.txtBRSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRSpeed.Name = "txtBRSpeed";
            this.txtBRSpeed.Size = new System.Drawing.Size(61, 22);
            this.txtBRSpeed.TabIndex = 5;
            this.txtBRSpeed.Text = "2000";
            // 
            // lblBRPower
            // 
            this.lblBRPower.AutoSize = true;
            this.lblBRPower.Location = new System.Drawing.Point(184, 54);
            this.lblBRPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRPower.Name = "lblBRPower";
            this.lblBRPower.Size = new System.Drawing.Size(52, 12);
            this.lblBRPower.TabIndex = 6;
            this.lblBRPower.Text = "功率 (%):";
            // 
            // txtBRPower
            // 
            this.txtBRPower.Location = new System.Drawing.Point(278, 52);
            this.txtBRPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRPower.Name = "txtBRPower";
            this.txtBRPower.Size = new System.Drawing.Size(61, 22);
            this.txtBRPower.TabIndex = 7;
            this.txtBRPower.Text = "70";
            // 
            // lblBRFreq
            // 
            this.lblBRFreq.AutoSize = true;
            this.lblBRFreq.Location = new System.Drawing.Point(11, 82);
            this.lblBRFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRFreq.Name = "lblBRFreq";
            this.lblBRFreq.Size = new System.Drawing.Size(62, 12);
            this.lblBRFreq.TabIndex = 8;
            this.lblBRFreq.Text = "頻率 (kHz):";
            // 
            // txtBRFreq
            // 
            this.txtBRFreq.Location = new System.Drawing.Point(105, 80);
            this.txtBRFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRFreq.Name = "txtBRFreq";
            this.txtBRFreq.Size = new System.Drawing.Size(61, 22);
            this.txtBRFreq.TabIndex = 9;
            this.txtBRFreq.Text = "30";
            // 
            // lblBRRepeat
            // 
            this.lblBRRepeat.AutoSize = true;
            this.lblBRRepeat.Location = new System.Drawing.Point(184, 82);
            this.lblBRRepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRRepeat.Name = "lblBRRepeat";
            this.lblBRRepeat.Size = new System.Drawing.Size(56, 12);
            this.lblBRRepeat.TabIndex = 10;
            this.lblBRRepeat.Text = "雕刻次數:";
            // 
            // txtBRRepeat
            // 
            this.txtBRRepeat.Location = new System.Drawing.Point(278, 80);
            this.txtBRRepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRRepeat.Name = "txtBRRepeat";
            this.txtBRRepeat.Size = new System.Drawing.Size(61, 22);
            this.txtBRRepeat.TabIndex = 11;
            this.txtBRRepeat.Text = "1";
            // 
            // lblBRSpotDelay
            // 
            this.lblBRSpotDelay.AutoSize = true;
            this.lblBRSpotDelay.Location = new System.Drawing.Point(11, 110);
            this.lblBRSpotDelay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRSpotDelay.Name = "lblBRSpotDelay";
            this.lblBRSpotDelay.Size = new System.Drawing.Size(68, 12);
            this.lblBRSpotDelay.TabIndex = 12;
            this.lblBRSpotDelay.Text = "點時間 (ms):";
            // 
            // txtBRSpotDelay
            // 
            this.txtBRSpotDelay.Location = new System.Drawing.Point(105, 108);
            this.txtBRSpotDelay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRSpotDelay.Name = "txtBRSpotDelay";
            this.txtBRSpotDelay.Size = new System.Drawing.Size(61, 22);
            this.txtBRSpotDelay.TabIndex = 13;
            this.txtBRSpotDelay.Text = "2";
            // 
            // lblBRPulseWidth
            // 
            this.lblBRPulseWidth.AutoSize = true;
            this.lblBRPulseWidth.Location = new System.Drawing.Point(184, 110);
            this.lblBRPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRPulseWidth.Name = "lblBRPulseWidth";
            this.lblBRPulseWidth.Size = new System.Drawing.Size(53, 12);
            this.lblBRPulseWidth.TabIndex = 14;
            this.lblBRPulseWidth.Text = "脈寬 (ns):";
            // 
            // txtBRPulseWidth
            // 
            this.txtBRPulseWidth.Location = new System.Drawing.Point(278, 108);
            this.txtBRPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRPulseWidth.Name = "txtBRPulseWidth";
            this.txtBRPulseWidth.Size = new System.Drawing.Size(61, 22);
            this.txtBRPulseWidth.TabIndex = 15;
            this.txtBRPulseWidth.Text = "150";
            // 
            // lblBRFillPitch
            // 
            this.lblBRFillPitch.AutoSize = true;
            this.lblBRFillPitch.Location = new System.Drawing.Point(11, 138);
            this.lblBRFillPitch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRFillPitch.Name = "lblBRFillPitch";
            this.lblBRFillPitch.Size = new System.Drawing.Size(85, 12);
            this.lblBRFillPitch.TabIndex = 16;
            this.lblBRFillPitch.Text = "填滿間距 (mm):";
            // 
            // txtBRFillPitch
            // 
            this.txtBRFillPitch.Location = new System.Drawing.Point(105, 136);
            this.txtBRFillPitch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRFillPitch.Name = "txtBRFillPitch";
            this.txtBRFillPitch.Size = new System.Drawing.Size(61, 22);
            this.txtBRFillPitch.TabIndex = 17;
            this.txtBRFillPitch.Text = "0.04";
            // 
            // lblBRFillRoundPitch
            // 
            this.lblBRFillRoundPitch.AutoSize = true;
            this.lblBRFillRoundPitch.Location = new System.Drawing.Point(184, 138);
            this.lblBRFillRoundPitch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRFillRoundPitch.Name = "lblBRFillRoundPitch";
            this.lblBRFillRoundPitch.Size = new System.Drawing.Size(61, 12);
            this.lblBRFillRoundPitch.TabIndex = 18;
            this.lblBRFillRoundPitch.Text = "圈距 (mm):";
            // 
            // txtBRFillRoundPitch
            // 
            this.txtBRFillRoundPitch.Location = new System.Drawing.Point(278, 136);
            this.txtBRFillRoundPitch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRFillRoundPitch.Name = "txtBRFillRoundPitch";
            this.txtBRFillRoundPitch.Size = new System.Drawing.Size(61, 22);
            this.txtBRFillRoundPitch.TabIndex = 19;
            this.txtBRFillRoundPitch.Text = "0.04";
            // 
            // lblBRFillTimes
            // 
            this.lblBRFillTimes.AutoSize = true;
            this.lblBRFillTimes.Location = new System.Drawing.Point(11, 166);
            this.lblBRFillTimes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRFillTimes.Name = "lblBRFillTimes";
            this.lblBRFillTimes.Size = new System.Drawing.Size(56, 12);
            this.lblBRFillTimes.TabIndex = 20;
            this.lblBRFillTimes.Text = "填滿次數:";
            // 
            // txtBRFillTimes
            // 
            this.txtBRFillTimes.Location = new System.Drawing.Point(105, 164);
            this.txtBRFillTimes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRFillTimes.Name = "txtBRFillTimes";
            this.txtBRFillTimes.Size = new System.Drawing.Size(61, 22);
            this.txtBRFillTimes.TabIndex = 21;
            this.txtBRFillTimes.Text = "3";
            // 
            // lblBRFillStepAngle
            // 
            this.lblBRFillStepAngle.AutoSize = true;
            this.lblBRFillStepAngle.Location = new System.Drawing.Point(184, 166);
            this.lblBRFillStepAngle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBRFillStepAngle.Name = "lblBRFillStepAngle";
            this.lblBRFillStepAngle.Size = new System.Drawing.Size(56, 12);
            this.lblBRFillStepAngle.TabIndex = 22;
            this.lblBRFillStepAngle.Text = "累進角度:";
            // 
            // txtBRFillStepAngle
            // 
            this.txtBRFillStepAngle.Location = new System.Drawing.Point(278, 164);
            this.txtBRFillStepAngle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBRFillStepAngle.Name = "txtBRFillStepAngle";
            this.txtBRFillStepAngle.Size = new System.Drawing.Size(61, 22);
            this.txtBRFillStepAngle.TabIndex = 23;
            this.txtBRFillStepAngle.Text = "45";
            // 
            // btnBRPreview
            // 
            this.btnBRPreview.Location = new System.Drawing.Point(15, 208);
            this.btnBRPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBRPreview.Name = "btnBRPreview";
            this.btnBRPreview.Size = new System.Drawing.Size(98, 36);
            this.btnBRPreview.TabIndex = 24;
            this.btnBRPreview.Text = "紅光預覽";
            this.btnBRPreview.UseVisualStyleBackColor = true;
            this.btnBRPreview.Click += new System.EventHandler(this.btnBRPreview_Click);
            // 
            // btnBRStopPreview
            // 
            this.btnBRStopPreview.Location = new System.Drawing.Point(120, 208);
            this.btnBRStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBRStopPreview.Name = "btnBRStopPreview";
            this.btnBRStopPreview.Size = new System.Drawing.Size(98, 36);
            this.btnBRStopPreview.TabIndex = 25;
            this.btnBRStopPreview.Text = "取消預覽";
            this.btnBRStopPreview.UseVisualStyleBackColor = true;
            this.btnBRStopPreview.Click += new System.EventHandler(this.btnBRStopPreview_Click);
            // 
            // btnBRMark
            // 
            this.btnBRMark.Location = new System.Drawing.Point(225, 208);
            this.btnBRMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBRMark.Name = "btnBRMark";
            this.btnBRMark.Size = new System.Drawing.Size(105, 36);
            this.btnBRMark.TabIndex = 26;
            this.btnBRMark.Text = "雷射打標（黑）";
            this.btnBRMark.UseVisualStyleBackColor = true;
            this.btnBRMark.Click += new System.EventHandler(this.btnBRMark_Click);
            // 
            // groupBoxWhiteRect
            // 
            this.groupBoxWhiteRect.Controls.Add(this.lblWRWidth);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRWidth);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRHeight);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRHeight);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRSpeed);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRSpeed);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRPower);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRPower);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRFreq);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRFreq);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRRepeat);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRRepeat);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRSpotDelay);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRSpotDelay);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRPulseWidth);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRPulseWidth);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRFillPitch);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRFillPitch);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRFillRoundPitch);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRFillRoundPitch);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRFillTimes);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRFillTimes);
            this.groupBoxWhiteRect.Controls.Add(this.lblWRFillStepAngle);
            this.groupBoxWhiteRect.Controls.Add(this.txtWRFillStepAngle);
            this.groupBoxWhiteRect.Controls.Add(this.btnWRPreview);
            this.groupBoxWhiteRect.Controls.Add(this.btnWRStopPreview);
            this.groupBoxWhiteRect.Controls.Add(this.btnWRMark);
            this.groupBoxWhiteRect.Location = new System.Drawing.Point(375, 36);
            this.groupBoxWhiteRect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxWhiteRect.Name = "groupBoxWhiteRect";
            this.groupBoxWhiteRect.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxWhiteRect.Size = new System.Drawing.Size(345, 492);
            this.groupBoxWhiteRect.TabIndex = 3;
            this.groupBoxWhiteRect.TabStop = false;
            this.groupBoxWhiteRect.Text = "② 打白矩形（霧化白化）";
            // 
            // lblWRWidth
            // 
            this.lblWRWidth.AutoSize = true;
            this.lblWRWidth.Location = new System.Drawing.Point(11, 26);
            this.lblWRWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRWidth.Name = "lblWRWidth";
            this.lblWRWidth.Size = new System.Drawing.Size(49, 12);
            this.lblWRWidth.TabIndex = 0;
            this.lblWRWidth.Text = "寬 (mm):";
            // 
            // txtWRWidth
            // 
            this.txtWRWidth.Location = new System.Drawing.Point(105, 24);
            this.txtWRWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRWidth.Name = "txtWRWidth";
            this.txtWRWidth.Size = new System.Drawing.Size(61, 22);
            this.txtWRWidth.TabIndex = 1;
            this.txtWRWidth.Text = "50";
            // 
            // lblWRHeight
            // 
            this.lblWRHeight.AutoSize = true;
            this.lblWRHeight.Location = new System.Drawing.Point(184, 26);
            this.lblWRHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRHeight.Name = "lblWRHeight";
            this.lblWRHeight.Size = new System.Drawing.Size(49, 12);
            this.lblWRHeight.TabIndex = 2;
            this.lblWRHeight.Text = "高 (mm):";
            // 
            // txtWRHeight
            // 
            this.txtWRHeight.Location = new System.Drawing.Point(278, 24);
            this.txtWRHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRHeight.Name = "txtWRHeight";
            this.txtWRHeight.Size = new System.Drawing.Size(61, 22);
            this.txtWRHeight.TabIndex = 3;
            this.txtWRHeight.Text = "50";
            // 
            // lblWRSpeed
            // 
            this.lblWRSpeed.AutoSize = true;
            this.lblWRSpeed.Location = new System.Drawing.Point(11, 54);
            this.lblWRSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRSpeed.Name = "lblWRSpeed";
            this.lblWRSpeed.Size = new System.Drawing.Size(68, 12);
            this.lblWRSpeed.TabIndex = 4;
            this.lblWRSpeed.Text = "速度 (mm/s):";
            // 
            // txtWRSpeed
            // 
            this.txtWRSpeed.Location = new System.Drawing.Point(105, 52);
            this.txtWRSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRSpeed.Name = "txtWRSpeed";
            this.txtWRSpeed.Size = new System.Drawing.Size(61, 22);
            this.txtWRSpeed.TabIndex = 5;
            this.txtWRSpeed.Text = "3000";
            // 
            // lblWRPower
            // 
            this.lblWRPower.AutoSize = true;
            this.lblWRPower.Location = new System.Drawing.Point(184, 54);
            this.lblWRPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRPower.Name = "lblWRPower";
            this.lblWRPower.Size = new System.Drawing.Size(52, 12);
            this.lblWRPower.TabIndex = 6;
            this.lblWRPower.Text = "功率 (%):";
            // 
            // txtWRPower
            // 
            this.txtWRPower.Location = new System.Drawing.Point(278, 52);
            this.txtWRPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRPower.Name = "txtWRPower";
            this.txtWRPower.Size = new System.Drawing.Size(61, 22);
            this.txtWRPower.TabIndex = 7;
            this.txtWRPower.Text = "40";
            // 
            // lblWRFreq
            // 
            this.lblWRFreq.AutoSize = true;
            this.lblWRFreq.Location = new System.Drawing.Point(11, 82);
            this.lblWRFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRFreq.Name = "lblWRFreq";
            this.lblWRFreq.Size = new System.Drawing.Size(62, 12);
            this.lblWRFreq.TabIndex = 8;
            this.lblWRFreq.Text = "頻率 (kHz):";
            // 
            // txtWRFreq
            // 
            this.txtWRFreq.Location = new System.Drawing.Point(105, 80);
            this.txtWRFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRFreq.Name = "txtWRFreq";
            this.txtWRFreq.Size = new System.Drawing.Size(61, 22);
            this.txtWRFreq.TabIndex = 9;
            this.txtWRFreq.Text = "500";
            // 
            // lblWRRepeat
            // 
            this.lblWRRepeat.AutoSize = true;
            this.lblWRRepeat.Location = new System.Drawing.Point(184, 82);
            this.lblWRRepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRRepeat.Name = "lblWRRepeat";
            this.lblWRRepeat.Size = new System.Drawing.Size(56, 12);
            this.lblWRRepeat.TabIndex = 10;
            this.lblWRRepeat.Text = "雕刻次數:";
            // 
            // txtWRRepeat
            // 
            this.txtWRRepeat.Location = new System.Drawing.Point(278, 80);
            this.txtWRRepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRRepeat.Name = "txtWRRepeat";
            this.txtWRRepeat.Size = new System.Drawing.Size(61, 22);
            this.txtWRRepeat.TabIndex = 11;
            this.txtWRRepeat.Text = "1";
            // 
            // lblWRSpotDelay
            // 
            this.lblWRSpotDelay.AutoSize = true;
            this.lblWRSpotDelay.Location = new System.Drawing.Point(11, 110);
            this.lblWRSpotDelay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRSpotDelay.Name = "lblWRSpotDelay";
            this.lblWRSpotDelay.Size = new System.Drawing.Size(68, 12);
            this.lblWRSpotDelay.TabIndex = 12;
            this.lblWRSpotDelay.Text = "點時間 (ms):";
            // 
            // txtWRSpotDelay
            // 
            this.txtWRSpotDelay.Location = new System.Drawing.Point(105, 108);
            this.txtWRSpotDelay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRSpotDelay.Name = "txtWRSpotDelay";
            this.txtWRSpotDelay.Size = new System.Drawing.Size(61, 22);
            this.txtWRSpotDelay.TabIndex = 13;
            this.txtWRSpotDelay.Text = "2";
            // 
            // lblWRPulseWidth
            // 
            this.lblWRPulseWidth.AutoSize = true;
            this.lblWRPulseWidth.Location = new System.Drawing.Point(184, 110);
            this.lblWRPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRPulseWidth.Name = "lblWRPulseWidth";
            this.lblWRPulseWidth.Size = new System.Drawing.Size(53, 12);
            this.lblWRPulseWidth.TabIndex = 14;
            this.lblWRPulseWidth.Text = "脈寬 (ns):";
            // 
            // txtWRPulseWidth
            // 
            this.txtWRPulseWidth.Location = new System.Drawing.Point(278, 108);
            this.txtWRPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRPulseWidth.Name = "txtWRPulseWidth";
            this.txtWRPulseWidth.Size = new System.Drawing.Size(61, 22);
            this.txtWRPulseWidth.TabIndex = 15;
            this.txtWRPulseWidth.Text = "200";
            // 
            // lblWRFillPitch
            // 
            this.lblWRFillPitch.AutoSize = true;
            this.lblWRFillPitch.Location = new System.Drawing.Point(11, 138);
            this.lblWRFillPitch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRFillPitch.Name = "lblWRFillPitch";
            this.lblWRFillPitch.Size = new System.Drawing.Size(85, 12);
            this.lblWRFillPitch.TabIndex = 16;
            this.lblWRFillPitch.Text = "填滿間距 (mm):";
            // 
            // txtWRFillPitch
            // 
            this.txtWRFillPitch.Location = new System.Drawing.Point(105, 136);
            this.txtWRFillPitch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRFillPitch.Name = "txtWRFillPitch";
            this.txtWRFillPitch.Size = new System.Drawing.Size(61, 22);
            this.txtWRFillPitch.TabIndex = 17;
            this.txtWRFillPitch.Text = "0.04";
            // 
            // lblWRFillRoundPitch
            // 
            this.lblWRFillRoundPitch.AutoSize = true;
            this.lblWRFillRoundPitch.Location = new System.Drawing.Point(184, 138);
            this.lblWRFillRoundPitch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRFillRoundPitch.Name = "lblWRFillRoundPitch";
            this.lblWRFillRoundPitch.Size = new System.Drawing.Size(61, 12);
            this.lblWRFillRoundPitch.TabIndex = 18;
            this.lblWRFillRoundPitch.Text = "圈距 (mm):";
            // 
            // txtWRFillRoundPitch
            // 
            this.txtWRFillRoundPitch.Location = new System.Drawing.Point(278, 136);
            this.txtWRFillRoundPitch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRFillRoundPitch.Name = "txtWRFillRoundPitch";
            this.txtWRFillRoundPitch.Size = new System.Drawing.Size(61, 22);
            this.txtWRFillRoundPitch.TabIndex = 19;
            this.txtWRFillRoundPitch.Text = "0.04";
            // 
            // lblWRFillTimes
            // 
            this.lblWRFillTimes.AutoSize = true;
            this.lblWRFillTimes.Location = new System.Drawing.Point(11, 166);
            this.lblWRFillTimes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRFillTimes.Name = "lblWRFillTimes";
            this.lblWRFillTimes.Size = new System.Drawing.Size(56, 12);
            this.lblWRFillTimes.TabIndex = 20;
            this.lblWRFillTimes.Text = "填滿次數:";
            // 
            // txtWRFillTimes
            // 
            this.txtWRFillTimes.Location = new System.Drawing.Point(105, 164);
            this.txtWRFillTimes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRFillTimes.Name = "txtWRFillTimes";
            this.txtWRFillTimes.Size = new System.Drawing.Size(61, 22);
            this.txtWRFillTimes.TabIndex = 21;
            this.txtWRFillTimes.Text = "2";
            // 
            // lblWRFillStepAngle
            // 
            this.lblWRFillStepAngle.AutoSize = true;
            this.lblWRFillStepAngle.Location = new System.Drawing.Point(184, 166);
            this.lblWRFillStepAngle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWRFillStepAngle.Name = "lblWRFillStepAngle";
            this.lblWRFillStepAngle.Size = new System.Drawing.Size(56, 12);
            this.lblWRFillStepAngle.TabIndex = 22;
            this.lblWRFillStepAngle.Text = "累進角度:";
            // 
            // txtWRFillStepAngle
            // 
            this.txtWRFillStepAngle.Location = new System.Drawing.Point(278, 164);
            this.txtWRFillStepAngle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWRFillStepAngle.Name = "txtWRFillStepAngle";
            this.txtWRFillStepAngle.Size = new System.Drawing.Size(61, 22);
            this.txtWRFillStepAngle.TabIndex = 23;
            this.txtWRFillStepAngle.Text = "45";
            // 
            // btnWRPreview
            // 
            this.btnWRPreview.Location = new System.Drawing.Point(15, 208);
            this.btnWRPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnWRPreview.Name = "btnWRPreview";
            this.btnWRPreview.Size = new System.Drawing.Size(98, 36);
            this.btnWRPreview.TabIndex = 24;
            this.btnWRPreview.Text = "紅光預覽";
            this.btnWRPreview.UseVisualStyleBackColor = true;
            this.btnWRPreview.Click += new System.EventHandler(this.btnWRPreview_Click);
            // 
            // btnWRStopPreview
            // 
            this.btnWRStopPreview.Location = new System.Drawing.Point(120, 208);
            this.btnWRStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnWRStopPreview.Name = "btnWRStopPreview";
            this.btnWRStopPreview.Size = new System.Drawing.Size(98, 36);
            this.btnWRStopPreview.TabIndex = 25;
            this.btnWRStopPreview.Text = "取消預覽";
            this.btnWRStopPreview.UseVisualStyleBackColor = true;
            this.btnWRStopPreview.Click += new System.EventHandler(this.btnWRStopPreview_Click);
            // 
            // btnWRMark
            // 
            this.btnWRMark.Location = new System.Drawing.Point(225, 208);
            this.btnWRMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnWRMark.Name = "btnWRMark";
            this.btnWRMark.Size = new System.Drawing.Size(105, 36);
            this.btnWRMark.TabIndex = 26;
            this.btnWRMark.Text = "雷射打標（白）";
            this.btnWRMark.UseVisualStyleBackColor = true;
            this.btnWRMark.Click += new System.EventHandler(this.btnWRMark_Click);
            // 
            // groupBoxQROnly
            // 
            this.groupBoxQROnly.Controls.Add(this.lblQOContent);
            this.groupBoxQROnly.Controls.Add(this.txtQOContent);
            this.groupBoxQROnly.Controls.Add(this.lblQOWidth);
            this.groupBoxQROnly.Controls.Add(this.txtQOWidth);
            this.groupBoxQROnly.Controls.Add(this.lblQOHeight);
            this.groupBoxQROnly.Controls.Add(this.txtQOHeight);
            this.groupBoxQROnly.Controls.Add(this.lblQOBorder);
            this.groupBoxQROnly.Controls.Add(this.txtQOBorder);
            this.groupBoxQROnly.Controls.Add(this.chkQOInvert);
            this.groupBoxQROnly.Controls.Add(this.lblQOMarkStyle);
            this.groupBoxQROnly.Controls.Add(this.txtQOMarkStyle);
            this.groupBoxQROnly.Controls.Add(this.lblQORepeat);
            this.groupBoxQROnly.Controls.Add(this.txtQORepeat);
            this.groupBoxQROnly.Controls.Add(this.lblQOStepAngle);
            this.groupBoxQROnly.Controls.Add(this.txtQOStepAngle);
            this.groupBoxQROnly.Controls.Add(this.lblQOPower);
            this.groupBoxQROnly.Controls.Add(this.txtQOPower);
            this.groupBoxQROnly.Controls.Add(this.lblQOSpeed);
            this.groupBoxQROnly.Controls.Add(this.txtQOSpeed);
            this.groupBoxQROnly.Controls.Add(this.lblQOFreq);
            this.groupBoxQROnly.Controls.Add(this.txtQOFreq);
            this.groupBoxQROnly.Controls.Add(this.lblQOPulseWidth);
            this.groupBoxQROnly.Controls.Add(this.txtQOPulseWidth);
            this.groupBoxQROnly.Controls.Add(this.btnQOPreview);
            this.groupBoxQROnly.Controls.Add(this.btnQOStopPreview);
            this.groupBoxQROnly.Controls.Add(this.btnQOMark);
            this.groupBoxQROnly.Location = new System.Drawing.Point(735, 36);
            this.groupBoxQROnly.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxQROnly.Name = "groupBoxQROnly";
            this.groupBoxQROnly.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxQROnly.Size = new System.Drawing.Size(345, 492);
            this.groupBoxQROnly.TabIndex = 4;
            this.groupBoxQROnly.TabStop = false;
            this.groupBoxQROnly.Text = "③ 打 QR Code（反相／連續線段）";
            // 
            // lblQOContent
            // 
            this.lblQOContent.AutoSize = true;
            this.lblQOContent.Location = new System.Drawing.Point(11, 26);
            this.lblQOContent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOContent.Name = "lblQOContent";
            this.lblQOContent.Size = new System.Drawing.Size(32, 12);
            this.lblQOContent.TabIndex = 0;
            this.lblQOContent.Text = "內容:";
            // 
            // txtQOContent
            // 
            this.txtQOContent.Location = new System.Drawing.Point(105, 24);
            this.txtQOContent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOContent.Name = "txtQOContent";
            this.txtQOContent.Size = new System.Drawing.Size(234, 22);
            this.txtQOContent.TabIndex = 1;
            this.txtQOContent.Text = "Hello World";
            // 
            // lblQOWidth
            // 
            this.lblQOWidth.AutoSize = true;
            this.lblQOWidth.Location = new System.Drawing.Point(11, 54);
            this.lblQOWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOWidth.Name = "lblQOWidth";
            this.lblQOWidth.Size = new System.Drawing.Size(49, 12);
            this.lblQOWidth.TabIndex = 2;
            this.lblQOWidth.Text = "寬 (mm):";
            // 
            // txtQOWidth
            // 
            this.txtQOWidth.Location = new System.Drawing.Point(105, 52);
            this.txtQOWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOWidth.Name = "txtQOWidth";
            this.txtQOWidth.Size = new System.Drawing.Size(61, 22);
            this.txtQOWidth.TabIndex = 3;
            this.txtQOWidth.Text = "30";
            // 
            // lblQOHeight
            // 
            this.lblQOHeight.AutoSize = true;
            this.lblQOHeight.Location = new System.Drawing.Point(184, 54);
            this.lblQOHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOHeight.Name = "lblQOHeight";
            this.lblQOHeight.Size = new System.Drawing.Size(49, 12);
            this.lblQOHeight.TabIndex = 4;
            this.lblQOHeight.Text = "高 (mm):";
            // 
            // txtQOHeight
            // 
            this.txtQOHeight.Location = new System.Drawing.Point(278, 52);
            this.txtQOHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOHeight.Name = "txtQOHeight";
            this.txtQOHeight.Size = new System.Drawing.Size(61, 22);
            this.txtQOHeight.TabIndex = 5;
            this.txtQOHeight.Text = "30";
            // 
            // lblQOBorder
            // 
            this.lblQOBorder.AutoSize = true;
            this.lblQOBorder.Location = new System.Drawing.Point(11, 82);
            this.lblQOBorder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOBorder.Name = "lblQOBorder";
            this.lblQOBorder.Size = new System.Drawing.Size(59, 12);
            this.lblQOBorder.TabIndex = 6;
            this.lblQOBorder.Text = "外框 (cell):";
            // 
            // txtQOBorder
            // 
            this.txtQOBorder.Location = new System.Drawing.Point(105, 80);
            this.txtQOBorder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOBorder.Name = "txtQOBorder";
            this.txtQOBorder.Size = new System.Drawing.Size(61, 22);
            this.txtQOBorder.TabIndex = 7;
            this.txtQOBorder.Text = "2";
            // 
            // chkQOInvert
            // 
            this.chkQOInvert.AutoSize = true;
            this.chkQOInvert.Checked = true;
            this.chkQOInvert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQOInvert.Location = new System.Drawing.Point(184, 82);
            this.chkQOInvert.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkQOInvert.Name = "chkQOInvert";
            this.chkQOInvert.Size = new System.Drawing.Size(120, 16);
            this.chkQOInvert.TabIndex = 8;
            this.chkQOInvert.Text = "反相（黑白互換）";
            this.chkQOInvert.UseVisualStyleBackColor = true;
            // 
            // lblQOMarkStyle
            // 
            this.lblQOMarkStyle.AutoSize = true;
            this.lblQOMarkStyle.Location = new System.Drawing.Point(11, 110);
            this.lblQOMarkStyle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOMarkStyle.Name = "lblQOMarkStyle";
            this.lblQOMarkStyle.Size = new System.Drawing.Size(56, 12);
            this.lblQOMarkStyle.TabIndex = 9;
            this.lblQOMarkStyle.Text = "雕刻形式:";
            // 
            // txtQOMarkStyle
            // 
            this.txtQOMarkStyle.Location = new System.Drawing.Point(105, 108);
            this.txtQOMarkStyle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOMarkStyle.Name = "txtQOMarkStyle";
            this.txtQOMarkStyle.Size = new System.Drawing.Size(61, 22);
            this.txtQOMarkStyle.TabIndex = 10;
            this.txtQOMarkStyle.Text = "3";
            // 
            // lblQORepeat
            // 
            this.lblQORepeat.AutoSize = true;
            this.lblQORepeat.Location = new System.Drawing.Point(184, 110);
            this.lblQORepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQORepeat.Name = "lblQORepeat";
            this.lblQORepeat.Size = new System.Drawing.Size(32, 12);
            this.lblQORepeat.TabIndex = 11;
            this.lblQORepeat.Text = "次數:";
            // 
            // txtQORepeat
            // 
            this.txtQORepeat.Location = new System.Drawing.Point(278, 108);
            this.txtQORepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQORepeat.Name = "txtQORepeat";
            this.txtQORepeat.Size = new System.Drawing.Size(61, 22);
            this.txtQORepeat.TabIndex = 12;
            this.txtQORepeat.Text = "2";
            // 
            // lblQOStepAngle
            // 
            this.lblQOStepAngle.AutoSize = true;
            this.lblQOStepAngle.Location = new System.Drawing.Point(11, 138);
            this.lblQOStepAngle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOStepAngle.Name = "lblQOStepAngle";
            this.lblQOStepAngle.Size = new System.Drawing.Size(56, 12);
            this.lblQOStepAngle.TabIndex = 13;
            this.lblQOStepAngle.Text = "累進角度:";
            // 
            // txtQOStepAngle
            // 
            this.txtQOStepAngle.Location = new System.Drawing.Point(105, 136);
            this.txtQOStepAngle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOStepAngle.Name = "txtQOStepAngle";
            this.txtQOStepAngle.Size = new System.Drawing.Size(61, 22);
            this.txtQOStepAngle.TabIndex = 14;
            this.txtQOStepAngle.Text = "90";
            // 
            // lblQOPower
            // 
            this.lblQOPower.AutoSize = true;
            this.lblQOPower.Location = new System.Drawing.Point(184, 138);
            this.lblQOPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOPower.Name = "lblQOPower";
            this.lblQOPower.Size = new System.Drawing.Size(52, 12);
            this.lblQOPower.TabIndex = 15;
            this.lblQOPower.Text = "功率 (%):";
            // 
            // txtQOPower
            // 
            this.txtQOPower.Location = new System.Drawing.Point(278, 136);
            this.txtQOPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOPower.Name = "txtQOPower";
            this.txtQOPower.Size = new System.Drawing.Size(61, 22);
            this.txtQOPower.TabIndex = 16;
            this.txtQOPower.Text = "70";
            // 
            // lblQOSpeed
            // 
            this.lblQOSpeed.AutoSize = true;
            this.lblQOSpeed.Location = new System.Drawing.Point(11, 166);
            this.lblQOSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOSpeed.Name = "lblQOSpeed";
            this.lblQOSpeed.Size = new System.Drawing.Size(68, 12);
            this.lblQOSpeed.TabIndex = 17;
            this.lblQOSpeed.Text = "速度 (mm/s):";
            // 
            // txtQOSpeed
            // 
            this.txtQOSpeed.Location = new System.Drawing.Point(105, 164);
            this.txtQOSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOSpeed.Name = "txtQOSpeed";
            this.txtQOSpeed.Size = new System.Drawing.Size(61, 22);
            this.txtQOSpeed.TabIndex = 18;
            this.txtQOSpeed.Text = "500";
            // 
            // lblQOFreq
            // 
            this.lblQOFreq.AutoSize = true;
            this.lblQOFreq.Location = new System.Drawing.Point(184, 166);
            this.lblQOFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOFreq.Name = "lblQOFreq";
            this.lblQOFreq.Size = new System.Drawing.Size(62, 12);
            this.lblQOFreq.TabIndex = 19;
            this.lblQOFreq.Text = "頻率 (kHz):";
            // 
            // txtQOFreq
            // 
            this.txtQOFreq.Location = new System.Drawing.Point(278, 164);
            this.txtQOFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOFreq.Name = "txtQOFreq";
            this.txtQOFreq.Size = new System.Drawing.Size(61, 22);
            this.txtQOFreq.TabIndex = 20;
            this.txtQOFreq.Text = "25";
            // 
            // lblQOPulseWidth
            // 
            this.lblQOPulseWidth.AutoSize = true;
            this.lblQOPulseWidth.Location = new System.Drawing.Point(11, 194);
            this.lblQOPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQOPulseWidth.Name = "lblQOPulseWidth";
            this.lblQOPulseWidth.Size = new System.Drawing.Size(53, 12);
            this.lblQOPulseWidth.TabIndex = 21;
            this.lblQOPulseWidth.Text = "脈寬 (ns):";
            // 
            // txtQOPulseWidth
            // 
            this.txtQOPulseWidth.Location = new System.Drawing.Point(105, 192);
            this.txtQOPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQOPulseWidth.Name = "txtQOPulseWidth";
            this.txtQOPulseWidth.Size = new System.Drawing.Size(61, 22);
            this.txtQOPulseWidth.TabIndex = 22;
            this.txtQOPulseWidth.Text = "200";
            // 
            // btnQOPreview
            // 
            this.btnQOPreview.Location = new System.Drawing.Point(15, 236);
            this.btnQOPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQOPreview.Name = "btnQOPreview";
            this.btnQOPreview.Size = new System.Drawing.Size(98, 36);
            this.btnQOPreview.TabIndex = 23;
            this.btnQOPreview.Text = "紅光預覽";
            this.btnQOPreview.UseVisualStyleBackColor = true;
            this.btnQOPreview.Click += new System.EventHandler(this.btnQOPreview_Click);
            // 
            // btnQOStopPreview
            // 
            this.btnQOStopPreview.Location = new System.Drawing.Point(120, 236);
            this.btnQOStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQOStopPreview.Name = "btnQOStopPreview";
            this.btnQOStopPreview.Size = new System.Drawing.Size(98, 36);
            this.btnQOStopPreview.TabIndex = 24;
            this.btnQOStopPreview.Text = "取消預覽";
            this.btnQOStopPreview.UseVisualStyleBackColor = true;
            this.btnQOStopPreview.Click += new System.EventHandler(this.btnQOStopPreview_Click);
            // 
            // btnQOMark
            // 
            this.btnQOMark.Location = new System.Drawing.Point(225, 236);
            this.btnQOMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQOMark.Name = "btnQOMark";
            this.btnQOMark.Size = new System.Drawing.Size(105, 36);
            this.btnQOMark.TabIndex = 25;
            this.btnQOMark.Text = "雷射打標 QR";
            this.btnQOMark.UseVisualStyleBackColor = true;
            this.btnQOMark.Click += new System.EventHandler(this.btnQOMark_Click);
            // 
            // btnAllPreview
            // 
            this.btnAllPreview.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAllPreview.Location = new System.Drawing.Point(15, 532);
            this.btnAllPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAllPreview.Name = "btnAllPreview";
            this.btnAllPreview.Size = new System.Drawing.Size(225, 32);
            this.btnAllPreview.TabIndex = 5;
            this.btnAllPreview.Text = "紅光預覽（黑→白→QR 三合一）";
            this.btnAllPreview.UseVisualStyleBackColor = true;
            this.btnAllPreview.Click += new System.EventHandler(this.btnAllPreview_Click);
            // 
            // btnAllStopPreview
            // 
            this.btnAllStopPreview.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAllStopPreview.Location = new System.Drawing.Point(255, 532);
            this.btnAllStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAllStopPreview.Name = "btnAllStopPreview";
            this.btnAllStopPreview.Size = new System.Drawing.Size(225, 32);
            this.btnAllStopPreview.TabIndex = 6;
            this.btnAllStopPreview.Text = "取消預覽";
            this.btnAllStopPreview.UseVisualStyleBackColor = true;
            this.btnAllStopPreview.Click += new System.EventHandler(this.btnAllStopPreview_Click);
            // 
            // btnAllMark
            // 
            this.btnAllMark.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAllMark.Location = new System.Drawing.Point(495, 532);
            this.btnAllMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAllMark.Name = "btnAllMark";
            this.btnAllMark.Size = new System.Drawing.Size(300, 32);
            this.btnAllMark.TabIndex = 7;
            this.btnAllMark.Text = "雷射打標（依序：① 黑 → ② 白 → ③ QR）";
            this.btnAllMark.UseVisualStyleBackColor = true;
            this.btnAllMark.Click += new System.EventHandler(this.btnAllMark_Click);
            // 
            // tabPageCLIBuilder
            // 
            this.tabPageCLIBuilder.Controls.Add(this.grpCLIBuilder);
            this.tabPageCLIBuilder.Controls.Add(this.grpCLIQRCode);
            this.tabPageCLIBuilder.Controls.Add(this.lblCLIQROutput);
            this.tabPageCLIBuilder.Controls.Add(this.txtCLIQROutput);
            this.tabPageCLIBuilder.Controls.Add(this.btnCLIQRRefresh);
            this.tabPageCLIBuilder.Controls.Add(this.btnCLIQRExecuteMark);
            this.tabPageCLIBuilder.Controls.Add(this.btnCLIQRStopPreview);
            this.tabPageCLIBuilder.Controls.Add(this.lblCLIOutput);
            this.tabPageCLIBuilder.Controls.Add(this.txtCLIOutput);
            this.tabPageCLIBuilder.Controls.Add(this.btnCLIRefresh);
            this.tabPageCLIBuilder.Controls.Add(this.btnCLIExecuteMark);
            this.tabPageCLIBuilder.Controls.Add(this.btnCLIStopPreview);
            this.tabPageCLIBuilder.Location = new System.Drawing.Point(4, 22);
            this.tabPageCLIBuilder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageCLIBuilder.Name = "tabPageCLIBuilder";
            this.tabPageCLIBuilder.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageCLIBuilder.Size = new System.Drawing.Size(1197, 566);
            this.tabPageCLIBuilder.TabIndex = 7;
            this.tabPageCLIBuilder.Text = "7. CLI 編輯器";
            this.tabPageCLIBuilder.UseVisualStyleBackColor = true;
            // 
            // grpCLIBuilder
            // 
            this.grpCLIBuilder.Controls.Add(this.lblCLIBoard);
            this.grpCLIBuilder.Controls.Add(this.txtCLIBoard);
            this.grpCLIBuilder.Controls.Add(this.lblCLIConfig);
            this.grpCLIBuilder.Controls.Add(this.txtCLIConfig);
            this.grpCLIBuilder.Controls.Add(this.lblCLIWsW);
            this.grpCLIBuilder.Controls.Add(this.txtCLIWsW);
            this.grpCLIBuilder.Controls.Add(this.lblCLIWsH);
            this.grpCLIBuilder.Controls.Add(this.txtCLIWsH);
            this.grpCLIBuilder.Controls.Add(this.lblCLIDxf);
            this.grpCLIBuilder.Controls.Add(this.txtCLIDxf);
            this.grpCLIBuilder.Controls.Add(this.lblCLILines);
            this.grpCLIBuilder.Controls.Add(this.txtCLILines);
            this.grpCLIBuilder.Controls.Add(this.lblCLIPower);
            this.grpCLIBuilder.Controls.Add(this.txtCLIPower);
            this.grpCLIBuilder.Controls.Add(this.lblCLISpeed);
            this.grpCLIBuilder.Controls.Add(this.txtCLISpeed);
            this.grpCLIBuilder.Controls.Add(this.lblCLIFreq);
            this.grpCLIBuilder.Controls.Add(this.txtCLIFreq);
            this.grpCLIBuilder.Controls.Add(this.lblCLIPulseWidth);
            this.grpCLIBuilder.Controls.Add(this.txtCLIPulseWidth);
            this.grpCLIBuilder.Controls.Add(this.lblCLIRepeat);
            this.grpCLIBuilder.Controls.Add(this.txtCLIRepeat);
            this.grpCLIBuilder.Controls.Add(this.lblCLIWobbleWidth);
            this.grpCLIBuilder.Controls.Add(this.txtCLIWobbleWidth);
            this.grpCLIBuilder.Controls.Add(this.lblCLIWobbleOverlap);
            this.grpCLIBuilder.Controls.Add(this.txtCLIWobbleOverlap);
            this.grpCLIBuilder.Controls.Add(this.lblCLIWobbleSpeed);
            this.grpCLIBuilder.Controls.Add(this.txtCLIWobbleSpeed);
            this.grpCLIBuilder.Controls.Add(this.lblCLIPreview);
            this.grpCLIBuilder.Controls.Add(this.txtCLIPreview);
            this.grpCLIBuilder.Controls.Add(this.lblCLIPreviewSpeed);
            this.grpCLIBuilder.Controls.Add(this.txtCLIPreviewSpeed);
            this.grpCLIBuilder.Controls.Add(this.lblCLIPreviewTime);
            this.grpCLIBuilder.Controls.Add(this.txtCLIPreviewTime);
            this.grpCLIBuilder.Controls.Add(this.chkCLIMark);
            this.grpCLIBuilder.Location = new System.Drawing.Point(6, 6);
            this.grpCLIBuilder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpCLIBuilder.Name = "grpCLIBuilder";
            this.grpCLIBuilder.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpCLIBuilder.Size = new System.Drawing.Size(534, 328);
            this.grpCLIBuilder.TabIndex = 0;
            this.grpCLIBuilder.TabStop = false;
            this.grpCLIBuilder.Text = "命令參數編輯";
            // 
            // lblCLIBoard
            // 
            this.lblCLIBoard.AutoSize = true;
            this.lblCLIBoard.Location = new System.Drawing.Point(8, 22);
            this.lblCLIBoard.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIBoard.Name = "lblCLIBoard";
            this.lblCLIBoard.Size = new System.Drawing.Size(59, 12);
            this.lblCLIBoard.TabIndex = 0;
            this.lblCLIBoard.Text = "板號 (0-3):";
            // 
            // txtCLIBoard
            // 
            this.txtCLIBoard.Location = new System.Drawing.Point(68, 20);
            this.txtCLIBoard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIBoard.Name = "txtCLIBoard";
            this.txtCLIBoard.Size = new System.Drawing.Size(31, 22);
            this.txtCLIBoard.TabIndex = 1;
            this.txtCLIBoard.Text = "0";
            this.txtCLIBoard.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIConfig
            // 
            this.lblCLIConfig.AutoSize = true;
            this.lblCLIConfig.Location = new System.Drawing.Point(112, 22);
            this.lblCLIConfig.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIConfig.Name = "lblCLIConfig";
            this.lblCLIConfig.Size = new System.Drawing.Size(56, 12);
            this.lblCLIConfig.TabIndex = 2;
            this.lblCLIConfig.Text = "配置路徑:";
            // 
            // txtCLIConfig
            // 
            this.txtCLIConfig.Location = new System.Drawing.Point(165, 20);
            this.txtCLIConfig.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIConfig.Name = "txtCLIConfig";
            this.txtCLIConfig.Size = new System.Drawing.Size(136, 22);
            this.txtCLIConfig.TabIndex = 3;
            this.txtCLIConfig.Text = "/cfg_config_MM1";
            this.txtCLIConfig.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIWsW
            // 
            this.lblCLIWsW.AutoSize = true;
            this.lblCLIWsW.Location = new System.Drawing.Point(8, 48);
            this.lblCLIWsW.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIWsW.Name = "lblCLIWsW";
            this.lblCLIWsW.Size = new System.Drawing.Size(85, 12);
            this.lblCLIWsW.TabIndex = 4;
            this.lblCLIWsW.Text = "工作區寬 (mm):";
            // 
            // txtCLIWsW
            // 
            this.txtCLIWsW.Location = new System.Drawing.Point(82, 46);
            this.txtCLIWsW.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIWsW.Name = "txtCLIWsW";
            this.txtCLIWsW.Size = new System.Drawing.Size(46, 22);
            this.txtCLIWsW.TabIndex = 5;
            this.txtCLIWsW.Text = "150";
            this.txtCLIWsW.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIWsH
            // 
            this.lblCLIWsH.AutoSize = true;
            this.lblCLIWsH.Location = new System.Drawing.Point(142, 48);
            this.lblCLIWsH.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIWsH.Name = "lblCLIWsH";
            this.lblCLIWsH.Size = new System.Drawing.Size(85, 12);
            this.lblCLIWsH.TabIndex = 6;
            this.lblCLIWsH.Text = "工作區高 (mm):";
            // 
            // txtCLIWsH
            // 
            this.txtCLIWsH.Location = new System.Drawing.Point(218, 46);
            this.txtCLIWsH.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIWsH.Name = "txtCLIWsH";
            this.txtCLIWsH.Size = new System.Drawing.Size(46, 22);
            this.txtCLIWsH.TabIndex = 7;
            this.txtCLIWsH.Text = "150";
            this.txtCLIWsH.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIDxf
            // 
            this.lblCLIDxf.AutoSize = true;
            this.lblCLIDxf.Location = new System.Drawing.Point(8, 74);
            this.lblCLIDxf.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIDxf.Name = "lblCLIDxf";
            this.lblCLIDxf.Size = new System.Drawing.Size(57, 12);
            this.lblCLIDxf.TabIndex = 8;
            this.lblCLIDxf.Text = "DXF 路徑:";
            // 
            // txtCLIDxf
            // 
            this.txtCLIDxf.Location = new System.Drawing.Point(68, 71);
            this.txtCLIDxf.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIDxf.Name = "txtCLIDxf";
            this.txtCLIDxf.Size = new System.Drawing.Size(458, 22);
            this.txtCLIDxf.TabIndex = 9;
            this.txtCLIDxf.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLILines
            // 
            this.lblCLILines.AutoSize = true;
            this.lblCLILines.Location = new System.Drawing.Point(8, 99);
            this.lblCLILines.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLILines.Name = "lblCLILines";
            this.lblCLILines.Size = new System.Drawing.Size(91, 12);
            this.lblCLILines.TabIndex = 10;
            this.lblCLILines.Text = "線段 (分號分隔):";
            // 
            // txtCLILines
            // 
            this.txtCLILines.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtCLILines.Location = new System.Drawing.Point(8, 116);
            this.txtCLILines.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLILines.Multiline = true;
            this.txtCLILines.Name = "txtCLILines";
            this.txtCLILines.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCLILines.Size = new System.Drawing.Size(518, 49);
            this.txtCLILines.TabIndex = 11;
            this.txtCLILines.Text = "1,8,-25,8;23,44,-5,-14;32,-48,9,3";
            this.txtCLILines.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIPower
            // 
            this.lblCLIPower.AutoSize = true;
            this.lblCLIPower.Location = new System.Drawing.Point(8, 174);
            this.lblCLIPower.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIPower.Name = "lblCLIPower";
            this.lblCLIPower.Size = new System.Drawing.Size(52, 12);
            this.lblCLIPower.TabIndex = 12;
            this.lblCLIPower.Text = "功率 (%):";
            // 
            // txtCLIPower
            // 
            this.txtCLIPower.Location = new System.Drawing.Point(60, 172);
            this.txtCLIPower.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIPower.Name = "txtCLIPower";
            this.txtCLIPower.Size = new System.Drawing.Size(46, 22);
            this.txtCLIPower.TabIndex = 13;
            this.txtCLIPower.Text = "80";
            this.txtCLIPower.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLISpeed
            // 
            this.lblCLISpeed.AutoSize = true;
            this.lblCLISpeed.Location = new System.Drawing.Point(120, 174);
            this.lblCLISpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLISpeed.Name = "lblCLISpeed";
            this.lblCLISpeed.Size = new System.Drawing.Size(68, 12);
            this.lblCLISpeed.TabIndex = 14;
            this.lblCLISpeed.Text = "速度 (mm/s):";
            // 
            // txtCLISpeed
            // 
            this.txtCLISpeed.Location = new System.Drawing.Point(191, 172);
            this.txtCLISpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLISpeed.Name = "txtCLISpeed";
            this.txtCLISpeed.Size = new System.Drawing.Size(54, 22);
            this.txtCLISpeed.TabIndex = 15;
            this.txtCLISpeed.Text = "1800";
            this.txtCLISpeed.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIFreq
            // 
            this.lblCLIFreq.AutoSize = true;
            this.lblCLIFreq.Location = new System.Drawing.Point(259, 174);
            this.lblCLIFreq.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIFreq.Name = "lblCLIFreq";
            this.lblCLIFreq.Size = new System.Drawing.Size(62, 12);
            this.lblCLIFreq.TabIndex = 16;
            this.lblCLIFreq.Text = "頻率 (kHz):";
            // 
            // txtCLIFreq
            // 
            this.txtCLIFreq.Location = new System.Drawing.Point(326, 172);
            this.txtCLIFreq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIFreq.Name = "txtCLIFreq";
            this.txtCLIFreq.Size = new System.Drawing.Size(46, 22);
            this.txtCLIFreq.TabIndex = 17;
            this.txtCLIFreq.Text = "20";
            this.txtCLIFreq.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIPulseWidth
            // 
            this.lblCLIPulseWidth.AutoSize = true;
            this.lblCLIPulseWidth.Location = new System.Drawing.Point(8, 200);
            this.lblCLIPulseWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIPulseWidth.Name = "lblCLIPulseWidth";
            this.lblCLIPulseWidth.Size = new System.Drawing.Size(56, 12);
            this.lblCLIPulseWidth.TabIndex = 18;
            this.lblCLIPulseWidth.Text = "脈衝寬度:";
            // 
            // txtCLIPulseWidth
            // 
            this.txtCLIPulseWidth.Location = new System.Drawing.Point(60, 198);
            this.txtCLIPulseWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIPulseWidth.Name = "txtCLIPulseWidth";
            this.txtCLIPulseWidth.Size = new System.Drawing.Size(46, 22);
            this.txtCLIPulseWidth.TabIndex = 19;
            this.txtCLIPulseWidth.Text = "600";
            this.txtCLIPulseWidth.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIRepeat
            // 
            this.lblCLIRepeat.AutoSize = true;
            this.lblCLIRepeat.Location = new System.Drawing.Point(120, 200);
            this.lblCLIRepeat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIRepeat.Name = "lblCLIRepeat";
            this.lblCLIRepeat.Size = new System.Drawing.Size(56, 12);
            this.lblCLIRepeat.TabIndex = 20;
            this.lblCLIRepeat.Text = "雷射次數:";
            // 
            // txtCLIRepeat
            // 
            this.txtCLIRepeat.Location = new System.Drawing.Point(191, 198);
            this.txtCLIRepeat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIRepeat.Name = "txtCLIRepeat";
            this.txtCLIRepeat.Size = new System.Drawing.Size(38, 22);
            this.txtCLIRepeat.TabIndex = 21;
            this.txtCLIRepeat.Text = "3";
            this.txtCLIRepeat.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIWobbleWidth
            // 
            this.lblCLIWobbleWidth.AutoSize = true;
            this.lblCLIWobbleWidth.Location = new System.Drawing.Point(8, 226);
            this.lblCLIWobbleWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIWobbleWidth.Name = "lblCLIWobbleWidth";
            this.lblCLIWobbleWidth.Size = new System.Drawing.Size(85, 12);
            this.lblCLIWobbleWidth.TabIndex = 22;
            this.lblCLIWobbleWidth.Text = "擺動寬度 (mm):";
            // 
            // txtCLIWobbleWidth
            // 
            this.txtCLIWobbleWidth.Location = new System.Drawing.Point(82, 223);
            this.txtCLIWobbleWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIWobbleWidth.Name = "txtCLIWobbleWidth";
            this.txtCLIWobbleWidth.Size = new System.Drawing.Size(46, 22);
            this.txtCLIWobbleWidth.TabIndex = 23;
            this.txtCLIWobbleWidth.Text = "0.5";
            this.txtCLIWobbleWidth.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIWobbleOverlap
            // 
            this.lblCLIWobbleOverlap.AutoSize = true;
            this.lblCLIWobbleOverlap.Location = new System.Drawing.Point(142, 226);
            this.lblCLIWobbleOverlap.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIWobbleOverlap.Name = "lblCLIWobbleOverlap";
            this.lblCLIWobbleOverlap.Size = new System.Drawing.Size(52, 12);
            this.lblCLIWobbleOverlap.TabIndex = 24;
            this.lblCLIWobbleOverlap.Text = "重疊 (%):";
            // 
            // txtCLIWobbleOverlap
            // 
            this.txtCLIWobbleOverlap.Location = new System.Drawing.Point(191, 223);
            this.txtCLIWobbleOverlap.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIWobbleOverlap.Name = "txtCLIWobbleOverlap";
            this.txtCLIWobbleOverlap.Size = new System.Drawing.Size(38, 22);
            this.txtCLIWobbleOverlap.TabIndex = 25;
            this.txtCLIWobbleOverlap.Text = "50";
            this.txtCLIWobbleOverlap.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIWobbleSpeed
            // 
            this.lblCLIWobbleSpeed.AutoSize = true;
            this.lblCLIWobbleSpeed.Location = new System.Drawing.Point(244, 226);
            this.lblCLIWobbleSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIWobbleSpeed.Name = "lblCLIWobbleSpeed";
            this.lblCLIWobbleSpeed.Size = new System.Drawing.Size(56, 12);
            this.lblCLIWobbleSpeed.TabIndex = 26;
            this.lblCLIWobbleSpeed.Text = "擺動速度:";
            // 
            // txtCLIWobbleSpeed
            // 
            this.txtCLIWobbleSpeed.Location = new System.Drawing.Point(296, 223);
            this.txtCLIWobbleSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIWobbleSpeed.Name = "txtCLIWobbleSpeed";
            this.txtCLIWobbleSpeed.Size = new System.Drawing.Size(54, 22);
            this.txtCLIWobbleSpeed.TabIndex = 27;
            this.txtCLIWobbleSpeed.Text = "5026.55";
            this.txtCLIWobbleSpeed.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIPreview
            // 
            this.lblCLIPreview.AutoSize = true;
            this.lblCLIPreview.Location = new System.Drawing.Point(8, 251);
            this.lblCLIPreview.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIPreview.Name = "lblCLIPreview";
            this.lblCLIPreview.Size = new System.Drawing.Size(94, 12);
            this.lblCLIPreview.TabIndex = 28;
            this.lblCLIPreview.Text = "預覽 (outline/full):";
            // 
            // txtCLIPreview
            // 
            this.txtCLIPreview.Location = new System.Drawing.Point(94, 249);
            this.txtCLIPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIPreview.Name = "txtCLIPreview";
            this.txtCLIPreview.Size = new System.Drawing.Size(54, 22);
            this.txtCLIPreview.TabIndex = 29;
            this.txtCLIPreview.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIPreviewSpeed
            // 
            this.lblCLIPreviewSpeed.AutoSize = true;
            this.lblCLIPreviewSpeed.Location = new System.Drawing.Point(158, 251);
            this.lblCLIPreviewSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIPreviewSpeed.Name = "lblCLIPreviewSpeed";
            this.lblCLIPreviewSpeed.Size = new System.Drawing.Size(56, 12);
            this.lblCLIPreviewSpeed.TabIndex = 30;
            this.lblCLIPreviewSpeed.Text = "預覽速度:";
            // 
            // txtCLIPreviewSpeed
            // 
            this.txtCLIPreviewSpeed.Location = new System.Drawing.Point(210, 249);
            this.txtCLIPreviewSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIPreviewSpeed.Name = "txtCLIPreviewSpeed";
            this.txtCLIPreviewSpeed.Size = new System.Drawing.Size(46, 22);
            this.txtCLIPreviewSpeed.TabIndex = 31;
            this.txtCLIPreviewSpeed.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIPreviewTime
            // 
            this.lblCLIPreviewTime.AutoSize = true;
            this.lblCLIPreviewTime.Location = new System.Drawing.Point(266, 251);
            this.lblCLIPreviewTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIPreviewTime.Name = "lblCLIPreviewTime";
            this.lblCLIPreviewTime.Size = new System.Drawing.Size(79, 12);
            this.lblCLIPreviewTime.TabIndex = 32;
            this.lblCLIPreviewTime.Text = "預覽時間 (秒):";
            // 
            // txtCLIPreviewTime
            // 
            this.txtCLIPreviewTime.Location = new System.Drawing.Point(334, 249);
            this.txtCLIPreviewTime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIPreviewTime.Name = "txtCLIPreviewTime";
            this.txtCLIPreviewTime.Size = new System.Drawing.Size(38, 22);
            this.txtCLIPreviewTime.TabIndex = 33;
            this.txtCLIPreviewTime.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // chkCLIMark
            // 
            this.chkCLIMark.AutoSize = true;
            this.chkCLIMark.Checked = true;
            this.chkCLIMark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCLIMark.Location = new System.Drawing.Point(8, 280);
            this.chkCLIMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkCLIMark.Name = "chkCLIMark";
            this.chkCLIMark.Size = new System.Drawing.Size(235, 16);
            this.chkCLIMark.TabIndex = 34;
            this.chkCLIMark.Text = "--mark (在命令字串中加入自動打標旗標)";
            this.chkCLIMark.UseVisualStyleBackColor = true;
            this.chkCLIMark.CheckedChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // grpCLIQRCode
            // 
            this.grpCLIQRCode.Controls.Add(this.lblCLIQRContent);
            this.grpCLIQRCode.Controls.Add(this.txtCLIQRContent);
            this.grpCLIQRCode.Controls.Add(this.lblCLIQRFixedHeader);
            this.grpCLIQRCode.Controls.Add(this.lblCLIQRFixedParams);
            this.grpCLIQRCode.Location = new System.Drawing.Point(548, 6);
            this.grpCLIQRCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpCLIQRCode.Name = "grpCLIQRCode";
            this.grpCLIQRCode.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpCLIQRCode.Size = new System.Drawing.Size(300, 208);
            this.grpCLIQRCode.TabIndex = 5;
            this.grpCLIQRCode.TabStop = false;
            this.grpCLIQRCode.Text = "QRCODE (白底反相，僅可異動 Content)";
            // 
            // lblCLIQRContent
            // 
            this.lblCLIQRContent.AutoSize = true;
            this.lblCLIQRContent.Location = new System.Drawing.Point(8, 26);
            this.lblCLIQRContent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIQRContent.Name = "lblCLIQRContent";
            this.lblCLIQRContent.Size = new System.Drawing.Size(64, 12);
            this.lblCLIQRContent.TabIndex = 0;
            this.lblCLIQRContent.Text = "QR Content:";
            // 
            // txtCLIQRContent
            // 
            this.txtCLIQRContent.Location = new System.Drawing.Point(75, 23);
            this.txtCLIQRContent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIQRContent.Name = "txtCLIQRContent";
            this.txtCLIQRContent.Size = new System.Drawing.Size(211, 22);
            this.txtCLIQRContent.TabIndex = 1;
            this.txtCLIQRContent.Text = "1234567";
            this.txtCLIQRContent.TextChanged += new System.EventHandler(this.OnCLIBuilderInputChanged);
            // 
            // lblCLIQRFixedHeader
            // 
            this.lblCLIQRFixedHeader.AutoSize = true;
            this.lblCLIQRFixedHeader.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCLIQRFixedHeader.Location = new System.Drawing.Point(8, 52);
            this.lblCLIQRFixedHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIQRFixedHeader.Name = "lblCLIQRFixedHeader";
            this.lblCLIQRFixedHeader.Size = new System.Drawing.Size(130, 15);
            this.lblCLIQRFixedHeader.TabIndex = 2;
            this.lblCLIQRFixedHeader.Text = "固定參數（不可修改）:";
            // 
            // lblCLIQRFixedParams
            // 
            this.lblCLIQRFixedParams.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblCLIQRFixedParams.Location = new System.Drawing.Point(11, 72);
            this.lblCLIQRFixedParams.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIQRFixedParams.Name = "lblCLIQRFixedParams";
            this.lblCLIQRFixedParams.Size = new System.Drawing.Size(285, 128);
            this.lblCLIQRFixedParams.TabIndex = 3;
            this.lblCLIQRFixedParams.Text = "模式: 白底反相 QR (--qr-whitebg)\r\nQR 長 x 寬: 15 x 15 mm\r\n外框單元: 2 cell\r\nQR 速度: 1000 mm/s\r" +
    "\nQR 功率: 80 %\r\n矩形速度: 800 mm/s (寫死於 SDK 呼叫)\r\n矩形功率: 100 % (寫死於 SDK 呼叫)\r\n打標: 全部 (矩形+" +
    "QR 雙圖層)";
            // 
            // lblCLIQROutput
            // 
            this.lblCLIQROutput.AutoSize = true;
            this.lblCLIQROutput.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCLIQROutput.Location = new System.Drawing.Point(548, 224);
            this.lblCLIQROutput.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIQROutput.Name = "lblCLIQROutput";
            this.lblCLIQROutput.Size = new System.Drawing.Size(70, 15);
            this.lblCLIQROutput.TabIndex = 6;
            this.lblCLIQROutput.Text = "組合後命令:";
            // 
            // txtCLIQROutput
            // 
            this.txtCLIQROutput.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCLIQROutput.Location = new System.Drawing.Point(548, 244);
            this.txtCLIQROutput.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIQROutput.Multiline = true;
            this.txtCLIQROutput.Name = "txtCLIQROutput";
            this.txtCLIQROutput.ReadOnly = true;
            this.txtCLIQROutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCLIQROutput.Size = new System.Drawing.Size(301, 105);
            this.txtCLIQROutput.TabIndex = 7;
            // 
            // btnCLIQRRefresh
            // 
            this.btnCLIQRRefresh.Location = new System.Drawing.Point(548, 356);
            this.btnCLIQRRefresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCLIQRRefresh.Name = "btnCLIQRRefresh";
            this.btnCLIQRRefresh.Size = new System.Drawing.Size(90, 32);
            this.btnCLIQRRefresh.TabIndex = 8;
            this.btnCLIQRRefresh.Text = "重新組合命令";
            this.btnCLIQRRefresh.UseVisualStyleBackColor = true;
            this.btnCLIQRRefresh.Click += new System.EventHandler(this.btnCLIQRRefresh_Click);
            // 
            // btnCLIQRExecuteMark
            // 
            this.btnCLIQRExecuteMark.Location = new System.Drawing.Point(645, 356);
            this.btnCLIQRExecuteMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCLIQRExecuteMark.Name = "btnCLIQRExecuteMark";
            this.btnCLIQRExecuteMark.Size = new System.Drawing.Size(98, 32);
            this.btnCLIQRExecuteMark.TabIndex = 9;
            this.btnCLIQRExecuteMark.Text = "依此命令打標";
            this.btnCLIQRExecuteMark.UseVisualStyleBackColor = true;
            this.btnCLIQRExecuteMark.Click += new System.EventHandler(this.btnCLIQRExecuteMark_Click);
            // 
            // btnCLIQRStopPreview
            // 
            this.btnCLIQRStopPreview.Location = new System.Drawing.Point(750, 356);
            this.btnCLIQRStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCLIQRStopPreview.Name = "btnCLIQRStopPreview";
            this.btnCLIQRStopPreview.Size = new System.Drawing.Size(90, 32);
            this.btnCLIQRStopPreview.TabIndex = 10;
            this.btnCLIQRStopPreview.Text = "停止預覽/打標";
            this.btnCLIQRStopPreview.UseVisualStyleBackColor = true;
            this.btnCLIQRStopPreview.Click += new System.EventHandler(this.btnCLIStopPreview_Click);
            // 
            // lblCLIOutput
            // 
            this.lblCLIOutput.AutoSize = true;
            this.lblCLIOutput.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCLIOutput.Location = new System.Drawing.Point(11, 344);
            this.lblCLIOutput.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCLIOutput.Name = "lblCLIOutput";
            this.lblCLIOutput.Size = new System.Drawing.Size(70, 15);
            this.lblCLIOutput.TabIndex = 1;
            this.lblCLIOutput.Text = "組合後命令:";
            // 
            // txtCLIOutput
            // 
            this.txtCLIOutput.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCLIOutput.Location = new System.Drawing.Point(11, 364);
            this.txtCLIOutput.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCLIOutput.Multiline = true;
            this.txtCLIOutput.Name = "txtCLIOutput";
            this.txtCLIOutput.ReadOnly = true;
            this.txtCLIOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCLIOutput.Size = new System.Drawing.Size(526, 137);
            this.txtCLIOutput.TabIndex = 2;
            // 
            // btnCLIRefresh
            // 
            this.btnCLIRefresh.Location = new System.Drawing.Point(11, 512);
            this.btnCLIRefresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCLIRefresh.Name = "btnCLIRefresh";
            this.btnCLIRefresh.Size = new System.Drawing.Size(112, 32);
            this.btnCLIRefresh.TabIndex = 3;
            this.btnCLIRefresh.Text = "重新組合命令";
            this.btnCLIRefresh.UseVisualStyleBackColor = true;
            this.btnCLIRefresh.Click += new System.EventHandler(this.btnCLIRefresh_Click);
            // 
            // btnCLIExecuteMark
            // 
            this.btnCLIExecuteMark.Location = new System.Drawing.Point(135, 512);
            this.btnCLIExecuteMark.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCLIExecuteMark.Name = "btnCLIExecuteMark";
            this.btnCLIExecuteMark.Size = new System.Drawing.Size(150, 32);
            this.btnCLIExecuteMark.TabIndex = 4;
            this.btnCLIExecuteMark.Text = "依此命令打標";
            this.btnCLIExecuteMark.UseVisualStyleBackColor = true;
            this.btnCLIExecuteMark.Click += new System.EventHandler(this.btnCLIExecuteMark_Click);
            // 
            // btnCLIStopPreview
            // 
            this.btnCLIStopPreview.Location = new System.Drawing.Point(296, 512);
            this.btnCLIStopPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCLIStopPreview.Name = "btnCLIStopPreview";
            this.btnCLIStopPreview.Size = new System.Drawing.Size(112, 32);
            this.btnCLIStopPreview.TabIndex = 5;
            this.btnCLIStopPreview.Text = "停止預覽/打標";
            this.btnCLIStopPreview.UseVisualStyleBackColor = true;
            this.btnCLIStopPreview.Click += new System.EventHandler(this.btnCLIStopPreview_Click);
            // 
            // tabPageCmd
            // 
            this.tabPageCmd.Controls.Add(this.lblCmdHeader);
            this.tabPageCmd.Controls.Add(this.lblBoardCmd);
            this.tabPageCmd.Controls.Add(this.comboBoardCmd);
            this.tabPageCmd.Controls.Add(this.btnCmdRegen);
            this.tabPageCmd.Controls.Add(this.lblCmd1);
            this.tabPageCmd.Controls.Add(this.txtCmd1);
            this.tabPageCmd.Controls.Add(this.btnCmd1);
            this.tabPageCmd.Controls.Add(this.lblCmd2);
            this.tabPageCmd.Controls.Add(this.txtCmd2);
            this.tabPageCmd.Controls.Add(this.btnCmd2);
            this.tabPageCmd.Controls.Add(this.lblCmd3);
            this.tabPageCmd.Controls.Add(this.txtCmd3);
            this.tabPageCmd.Controls.Add(this.btnCmd3);
            this.tabPageCmd.Controls.Add(this.lblCmd4);
            this.tabPageCmd.Controls.Add(this.txtCmd4);
            this.tabPageCmd.Controls.Add(this.btnCmd4);
            this.tabPageCmd.Controls.Add(this.lblCmd5);
            this.tabPageCmd.Controls.Add(this.txtCmd5);
            this.tabPageCmd.Controls.Add(this.btnCmd5);
            this.tabPageCmd.Controls.Add(this.lblCmdHint);
            this.tabPageCmd.Controls.Add(this.btnParallelTest);
            this.tabPageCmd.Controls.Add(this.txtParallelResult);
            this.tabPageCmd.Location = new System.Drawing.Point(4, 22);
            this.tabPageCmd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageCmd.Name = "tabPageCmd";
            this.tabPageCmd.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPageCmd.Size = new System.Drawing.Size(1197, 566);
            this.tabPageCmd.TabIndex = 6;
            this.tabPageCmd.Text = "7. 命令提示";
            this.tabPageCmd.UseVisualStyleBackColor = true;
            // 
            // lblCmdHeader
            // 
            this.lblCmdHeader.AutoSize = true;
            this.lblCmdHeader.Font = new System.Drawing.Font("Microsoft JhengHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCmdHeader.Location = new System.Drawing.Point(11, 14);
            this.lblCmdHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmdHeader.Name = "lblCmdHeader";
            this.lblCmdHeader.Size = new System.Drawing.Size(220, 18);
            this.lblCmdHeader.TabIndex = 0;
            this.lblCmdHeader.Text = "命令提示：隨機 5 組紅光預覽指令";
            // 
            // lblBoardCmd
            // 
            this.lblBoardCmd.AutoSize = true;
            this.lblBoardCmd.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblBoardCmd.Location = new System.Drawing.Point(248, 14);
            this.lblBoardCmd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoardCmd.Name = "lblBoardCmd";
            this.lblBoardCmd.Size = new System.Drawing.Size(55, 15);
            this.lblBoardCmd.TabIndex = 1;
            this.lblBoardCmd.Text = "晶片板：";
            // 
            // comboBoardCmd
            // 
            this.comboBoardCmd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoardCmd.FormattingEnabled = true;
            this.comboBoardCmd.Items.AddRange(new object[] {
            "晶片板 1",
            "晶片板 2",
            "晶片板 3",
            "晶片板 4"});
            this.comboBoardCmd.Location = new System.Drawing.Point(296, 11);
            this.comboBoardCmd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoardCmd.Name = "comboBoardCmd";
            this.comboBoardCmd.Size = new System.Drawing.Size(106, 20);
            this.comboBoardCmd.TabIndex = 2;
            this.comboBoardCmd.SelectedIndexChanged += new System.EventHandler(this.comboBoardCmd_SelectedIndexChanged);
            // 
            // btnCmdRegen
            // 
            this.btnCmdRegen.Location = new System.Drawing.Point(431, 10);
            this.btnCmdRegen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCmdRegen.Name = "btnCmdRegen";
            this.btnCmdRegen.Size = new System.Drawing.Size(101, 26);
            this.btnCmdRegen.TabIndex = 3;
            this.btnCmdRegen.Text = "重新產生";
            this.btnCmdRegen.UseVisualStyleBackColor = true;
            this.btnCmdRegen.Click += new System.EventHandler(this.btnCmdRegen_Click);
            // 
            // lblCmd1
            // 
            this.lblCmd1.AutoSize = true;
            this.lblCmd1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCmd1.Location = new System.Drawing.Point(11, 56);
            this.lblCmd1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmd1.Name = "lblCmd1";
            this.lblCmd1.Size = new System.Drawing.Size(22, 15);
            this.lblCmd1.TabIndex = 2;
            this.lblCmd1.Text = "#1";
            // 
            // txtCmd1
            // 
            this.txtCmd1.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtCmd1.Location = new System.Drawing.Point(38, 52);
            this.txtCmd1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCmd1.Name = "txtCmd1";
            this.txtCmd1.Size = new System.Drawing.Size(391, 22);
            this.txtCmd1.TabIndex = 3;
            // 
            // btnCmd1
            // 
            this.btnCmd1.Location = new System.Drawing.Point(435, 50);
            this.btnCmd1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCmd1.Name = "btnCmd1";
            this.btnCmd1.Size = new System.Drawing.Size(98, 24);
            this.btnCmd1.TabIndex = 4;
            this.btnCmd1.Text = "執行預覽";
            this.btnCmd1.UseVisualStyleBackColor = true;
            this.btnCmd1.Click += new System.EventHandler(this.btnCmd1_Click);
            // 
            // lblCmd2
            // 
            this.lblCmd2.AutoSize = true;
            this.lblCmd2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCmd2.Location = new System.Drawing.Point(11, 104);
            this.lblCmd2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmd2.Name = "lblCmd2";
            this.lblCmd2.Size = new System.Drawing.Size(22, 15);
            this.lblCmd2.TabIndex = 5;
            this.lblCmd2.Text = "#2";
            // 
            // txtCmd2
            // 
            this.txtCmd2.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtCmd2.Location = new System.Drawing.Point(38, 100);
            this.txtCmd2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCmd2.Name = "txtCmd2";
            this.txtCmd2.Size = new System.Drawing.Size(391, 22);
            this.txtCmd2.TabIndex = 6;
            // 
            // btnCmd2
            // 
            this.btnCmd2.Location = new System.Drawing.Point(435, 98);
            this.btnCmd2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCmd2.Name = "btnCmd2";
            this.btnCmd2.Size = new System.Drawing.Size(98, 24);
            this.btnCmd2.TabIndex = 7;
            this.btnCmd2.Text = "執行預覽";
            this.btnCmd2.UseVisualStyleBackColor = true;
            this.btnCmd2.Click += new System.EventHandler(this.btnCmd2_Click);
            // 
            // lblCmd3
            // 
            this.lblCmd3.AutoSize = true;
            this.lblCmd3.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCmd3.Location = new System.Drawing.Point(11, 152);
            this.lblCmd3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmd3.Name = "lblCmd3";
            this.lblCmd3.Size = new System.Drawing.Size(22, 15);
            this.lblCmd3.TabIndex = 8;
            this.lblCmd3.Text = "#3";
            // 
            // txtCmd3
            // 
            this.txtCmd3.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtCmd3.Location = new System.Drawing.Point(38, 148);
            this.txtCmd3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCmd3.Name = "txtCmd3";
            this.txtCmd3.Size = new System.Drawing.Size(391, 22);
            this.txtCmd3.TabIndex = 9;
            // 
            // btnCmd3
            // 
            this.btnCmd3.Location = new System.Drawing.Point(435, 146);
            this.btnCmd3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCmd3.Name = "btnCmd3";
            this.btnCmd3.Size = new System.Drawing.Size(98, 24);
            this.btnCmd3.TabIndex = 10;
            this.btnCmd3.Text = "執行預覽";
            this.btnCmd3.UseVisualStyleBackColor = true;
            this.btnCmd3.Click += new System.EventHandler(this.btnCmd3_Click);
            // 
            // lblCmd4
            // 
            this.lblCmd4.AutoSize = true;
            this.lblCmd4.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCmd4.Location = new System.Drawing.Point(11, 200);
            this.lblCmd4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmd4.Name = "lblCmd4";
            this.lblCmd4.Size = new System.Drawing.Size(22, 15);
            this.lblCmd4.TabIndex = 11;
            this.lblCmd4.Text = "#4";
            // 
            // txtCmd4
            // 
            this.txtCmd4.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtCmd4.Location = new System.Drawing.Point(38, 196);
            this.txtCmd4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCmd4.Name = "txtCmd4";
            this.txtCmd4.Size = new System.Drawing.Size(391, 22);
            this.txtCmd4.TabIndex = 12;
            // 
            // btnCmd4
            // 
            this.btnCmd4.Location = new System.Drawing.Point(435, 194);
            this.btnCmd4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCmd4.Name = "btnCmd4";
            this.btnCmd4.Size = new System.Drawing.Size(98, 24);
            this.btnCmd4.TabIndex = 13;
            this.btnCmd4.Text = "執行預覽";
            this.btnCmd4.UseVisualStyleBackColor = true;
            this.btnCmd4.Click += new System.EventHandler(this.btnCmd4_Click);
            // 
            // lblCmd5
            // 
            this.lblCmd5.AutoSize = true;
            this.lblCmd5.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCmd5.Location = new System.Drawing.Point(11, 248);
            this.lblCmd5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmd5.Name = "lblCmd5";
            this.lblCmd5.Size = new System.Drawing.Size(22, 15);
            this.lblCmd5.TabIndex = 14;
            this.lblCmd5.Text = "#5";
            // 
            // txtCmd5
            // 
            this.txtCmd5.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtCmd5.Location = new System.Drawing.Point(38, 244);
            this.txtCmd5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCmd5.Name = "txtCmd5";
            this.txtCmd5.Size = new System.Drawing.Size(391, 22);
            this.txtCmd5.TabIndex = 15;
            // 
            // btnCmd5
            // 
            this.btnCmd5.Location = new System.Drawing.Point(435, 242);
            this.btnCmd5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCmd5.Name = "btnCmd5";
            this.btnCmd5.Size = new System.Drawing.Size(98, 24);
            this.btnCmd5.TabIndex = 16;
            this.btnCmd5.Text = "執行預覽";
            this.btnCmd5.UseVisualStyleBackColor = true;
            this.btnCmd5.Click += new System.EventHandler(this.btnCmd5_Click);
            // 
            // lblCmdHint
            // 
            this.lblCmdHint.Location = new System.Drawing.Point(11, 296);
            this.lblCmdHint.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCmdHint.Name = "lblCmdHint";
            this.lblCmdHint.Size = new System.Drawing.Size(521, 64);
            this.lblCmdHint.TabIndex = 17;
            this.lblCmdHint.Text = resources.GetString("lblCmdHint.Text");
            // 
            // btnParallelTest
            // 
            this.btnParallelTest.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnParallelTest.Location = new System.Drawing.Point(11, 376);
            this.btnParallelTest.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnParallelTest.Name = "btnParallelTest";
            this.btnParallelTest.Size = new System.Drawing.Size(195, 28);
            this.btnParallelTest.TabIndex = 18;
            this.btnParallelTest.Text = "並行驗證（所有已初始化板，5 秒）";
            this.btnParallelTest.UseVisualStyleBackColor = true;
            this.btnParallelTest.Click += new System.EventHandler(this.btnParallelTest_Click);
            // 
            // txtParallelResult
            // 
            this.txtParallelResult.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtParallelResult.Location = new System.Drawing.Point(11, 412);
            this.txtParallelResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtParallelResult.Multiline = true;
            this.txtParallelResult.Name = "txtParallelResult";
            this.txtParallelResult.ReadOnly = true;
            this.txtParallelResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtParallelResult.Size = new System.Drawing.Size(522, 117);
            this.txtParallelResult.TabIndex = 19;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 784);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
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
            this.groupBoxQRWhiteBg.ResumeLayout(false);
            this.groupBoxQRWhiteBg.PerformLayout();
            this.groupBoxQRSteel.ResumeLayout(false);
            this.groupBoxQRSteel.PerformLayout();
            this.groupBoxSteelTime.ResumeLayout(false);
            this.groupBoxSteelTime.PerformLayout();
            this.groupBoxRectAlone.ResumeLayout(false);
            this.groupBoxRectAlone.PerformLayout();
            this.groupBoxQRAlone.ResumeLayout(false);
            this.groupBoxQRAlone.PerformLayout();
            this.tabPageQRCode2.ResumeLayout(false);
            this.tabPageQRCode2.PerformLayout();
            this.groupBoxBlackRect.ResumeLayout(false);
            this.groupBoxBlackRect.PerformLayout();
            this.groupBoxWhiteRect.ResumeLayout(false);
            this.groupBoxWhiteRect.PerformLayout();
            this.groupBoxQROnly.ResumeLayout(false);
            this.groupBoxQROnly.PerformLayout();
            this.tabPageCLIBuilder.ResumeLayout(false);
            this.tabPageCLIBuilder.PerformLayout();
            this.grpCLIBuilder.ResumeLayout(false);
            this.grpCLIBuilder.PerformLayout();
            this.grpCLIQRCode.ResumeLayout(false);
            this.grpCLIQRCode.PerformLayout();
            this.tabPageCmd.ResumeLayout(false);
            this.tabPageCmd.PerformLayout();
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
        private System.Windows.Forms.Button btnPreviewManual;
        private System.Windows.Forms.Button btnStopPreviewManual;
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
        private System.Windows.Forms.Label lblWorkspaceHeight;
        private System.Windows.Forms.TextBox txtWorkspaceHeight;
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
        private System.Windows.Forms.TabPage tabPageQRCode2;
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
        private System.Windows.Forms.GroupBox groupBoxQRWhiteBg;
        private System.Windows.Forms.Button btnQRWhiteBgMark;
        private System.Windows.Forms.Button btnQRWhiteBgCreate;
        private System.Windows.Forms.Button btnQRWhiteBgPreview;
        private System.Windows.Forms.Button btnQRWhiteBgStopPreview;
        private System.Windows.Forms.Label lblWBSerial;
        private System.Windows.Forms.TextBox txtWBSerial;
        private System.Windows.Forms.Label lblWBRectSpeed;
        private System.Windows.Forms.TextBox txtWBRectSpeed;
        private System.Windows.Forms.Label lblWBRectPower;
        private System.Windows.Forms.TextBox txtWBRectPower;
        private System.Windows.Forms.Label lblWBQRSpeed;
        private System.Windows.Forms.TextBox txtWBQRSpeed;
        private System.Windows.Forms.Label lblWBQRPower;
        private System.Windows.Forms.TextBox txtWBQRPower;
        private System.Windows.Forms.Label lblWBQRWidth;
        private System.Windows.Forms.TextBox txtWBQRWidth;
        private System.Windows.Forms.Label lblWBQRHeight;
        private System.Windows.Forms.TextBox txtWBQRHeight;
        private System.Windows.Forms.Label lblWBQuietZone;
        private System.Windows.Forms.TextBox txtWBQuietZone;
        private System.Windows.Forms.Label lblWBRectExtra;
        private System.Windows.Forms.TextBox txtWBRectExtra;
        private System.Windows.Forms.Label lblWBMarkTarget;
        private System.Windows.Forms.RadioButton rdoWBMarkQR;
        private System.Windows.Forms.RadioButton rdoWBMarkRect;
        private System.Windows.Forms.RadioButton rdoWBMarkAll;
        // === 鋼鐵 + Quest 3 QR ===
        private System.Windows.Forms.GroupBox groupBoxQRSteel;
        private System.Windows.Forms.Label lblSteelQrWidth;
        private System.Windows.Forms.TextBox txtSteelQrWidth;
        private System.Windows.Forms.Label lblSteelQrHeight;
        private System.Windows.Forms.TextBox txtSteelQrHeight;
        private System.Windows.Forms.Label lblSteelBorder;
        private System.Windows.Forms.TextBox txtSteelBorder;
        private System.Windows.Forms.Label lblSteelRectExtra;
        private System.Windows.Forms.TextBox txtSteelRectExtra;
        private System.Windows.Forms.Label lblSteelECLevel;
        private System.Windows.Forms.TextBox txtSteelECLevel;
        private System.Windows.Forms.Label lblSteelMarkStyle;
        private System.Windows.Forms.TextBox txtSteelMarkStyle;
        private System.Windows.Forms.Label lblSteelSpotSize;
        private System.Windows.Forms.TextBox txtSteelSpotSize;
        private System.Windows.Forms.Label lblSteelQrRepeat;
        private System.Windows.Forms.TextBox txtSteelQrRepeat;
        private System.Windows.Forms.Label lblSteelRectPower;
        private System.Windows.Forms.TextBox txtSteelRectPower;
        private System.Windows.Forms.Label lblSteelRectSpeed;
        private System.Windows.Forms.TextBox txtSteelRectSpeed;
        private System.Windows.Forms.Label lblSteelRectFreq;
        private System.Windows.Forms.TextBox txtSteelRectFreq;
        private System.Windows.Forms.Label lblSteelRectRepeat;
        private System.Windows.Forms.TextBox txtSteelRectRepeat;
        private System.Windows.Forms.Label lblSteelQrPower;
        private System.Windows.Forms.TextBox txtSteelQrPower;
        private System.Windows.Forms.Label lblSteelQrSpeed;
        private System.Windows.Forms.TextBox txtSteelQrSpeed;
        private System.Windows.Forms.Label lblSteelQrFreq;
        private System.Windows.Forms.TextBox txtSteelQrFreq;
        private System.Windows.Forms.Label lblSteelQrPulseWidth;
        private System.Windows.Forms.TextBox txtSteelQrPulseWidth;
        private System.Windows.Forms.Button btnQRSteelMark;
        private System.Windows.Forms.Button btnQRSteelPreview;
        private System.Windows.Forms.Button btnQRSteelStopPreview;
        private System.Windows.Forms.Label lblSteelSerial;
        private System.Windows.Forms.TextBox txtSteelSerial;
        // === 預估時間 GroupBox ===
        private System.Windows.Forms.GroupBox groupBoxSteelTime;
        private System.Windows.Forms.TextBox txtSteelTimeInfo;
        // === 矩形獨立 GroupBox 控件 (Rect Alone) ===
        private System.Windows.Forms.GroupBox groupBoxRectAlone;
        private System.Windows.Forms.Label lblRAWidth;
        private System.Windows.Forms.TextBox txtRAWidth;
        private System.Windows.Forms.Label lblRAHeight;
        private System.Windows.Forms.TextBox txtRAHeight;
        private System.Windows.Forms.Label lblRAX;
        private System.Windows.Forms.TextBox txtRAX;
        private System.Windows.Forms.Label lblRAY;
        private System.Windows.Forms.TextBox txtRAY;
        private System.Windows.Forms.Label lblRASpeed;
        private System.Windows.Forms.TextBox txtRASpeed;
        private System.Windows.Forms.Label lblRAPower;
        private System.Windows.Forms.TextBox txtRAPower;
        private System.Windows.Forms.Label lblRAFreq;
        private System.Windows.Forms.TextBox txtRAFreq;
        private System.Windows.Forms.Label lblRARepeat;
        private System.Windows.Forms.TextBox txtRARepeat;
        private System.Windows.Forms.Label lblRAPulseWidth;
        private System.Windows.Forms.TextBox txtRAPulseWidth;
        private System.Windows.Forms.Label lblRAFillStyle;
        private System.Windows.Forms.TextBox txtRAFillStyle;
        private System.Windows.Forms.Label lblRAFrameLineType;
        private System.Windows.Forms.TextBox txtRAFrameLineType;
        private System.Windows.Forms.Button btnRAPreview;
        private System.Windows.Forms.Button btnRAStopPreview;
        private System.Windows.Forms.Button btnRAMark;
        // === QR 獨立 GroupBox 控件 (QR Alone) ===
        private System.Windows.Forms.GroupBox groupBoxQRAlone;
        private System.Windows.Forms.Label lblQAContent;
        private System.Windows.Forms.TextBox txtQAContent;
        private System.Windows.Forms.Label lblQAWidth;
        private System.Windows.Forms.TextBox txtQAWidth;
        private System.Windows.Forms.Label lblQAHeight;
        private System.Windows.Forms.TextBox txtQAHeight;
        private System.Windows.Forms.Label lblQAX;
        private System.Windows.Forms.TextBox txtQAX;
        private System.Windows.Forms.Label lblQAY;
        private System.Windows.Forms.TextBox txtQAY;
        private System.Windows.Forms.Label lblQABorder;
        private System.Windows.Forms.TextBox txtQABorder;
        private System.Windows.Forms.Label lblQAECLevel;
        private System.Windows.Forms.TextBox txtQAECLevel;
        private System.Windows.Forms.Label lblQAMarkStyle;
        private System.Windows.Forms.TextBox txtQAMarkStyle;
        private System.Windows.Forms.CheckBox chkQAInvert;
        private System.Windows.Forms.Label lblQASpeed;
        private System.Windows.Forms.TextBox txtQASpeed;
        private System.Windows.Forms.Label lblQAPower;
        private System.Windows.Forms.TextBox txtQAPower;
        private System.Windows.Forms.Label lblQAFreq;
        private System.Windows.Forms.TextBox txtQAFreq;
        private System.Windows.Forms.Label lblQARepeat;
        private System.Windows.Forms.TextBox txtQARepeat;
        private System.Windows.Forms.Label lblQAPulseWidth;
        private System.Windows.Forms.TextBox txtQAPulseWidth;
        private System.Windows.Forms.Button btnQAPreview;
        private System.Windows.Forms.Button btnQAStopPreview;
        private System.Windows.Forms.Button btnQAMark;
        // === CLI Builder 頁籤控件 ===
        private System.Windows.Forms.TabPage tabPageCLIBuilder;
        private System.Windows.Forms.GroupBox grpCLIBuilder;
        private System.Windows.Forms.Label lblCLIBoard;
        private System.Windows.Forms.TextBox txtCLIBoard;
        private System.Windows.Forms.Label lblCLIConfig;
        private System.Windows.Forms.TextBox txtCLIConfig;
        private System.Windows.Forms.Label lblCLIWsW;
        private System.Windows.Forms.TextBox txtCLIWsW;
        private System.Windows.Forms.Label lblCLIWsH;
        private System.Windows.Forms.TextBox txtCLIWsH;
        private System.Windows.Forms.Label lblCLIDxf;
        private System.Windows.Forms.TextBox txtCLIDxf;
        private System.Windows.Forms.Label lblCLILines;
        private System.Windows.Forms.TextBox txtCLILines;
        private System.Windows.Forms.Label lblCLIPower;
        private System.Windows.Forms.TextBox txtCLIPower;
        private System.Windows.Forms.Label lblCLISpeed;
        private System.Windows.Forms.TextBox txtCLISpeed;
        private System.Windows.Forms.Label lblCLIFreq;
        private System.Windows.Forms.TextBox txtCLIFreq;
        private System.Windows.Forms.Label lblCLIPulseWidth;
        private System.Windows.Forms.TextBox txtCLIPulseWidth;
        private System.Windows.Forms.Label lblCLIRepeat;
        private System.Windows.Forms.TextBox txtCLIRepeat;
        private System.Windows.Forms.Label lblCLIWobbleWidth;
        private System.Windows.Forms.TextBox txtCLIWobbleWidth;
        private System.Windows.Forms.Label lblCLIWobbleOverlap;
        private System.Windows.Forms.TextBox txtCLIWobbleOverlap;
        private System.Windows.Forms.Label lblCLIWobbleSpeed;
        private System.Windows.Forms.TextBox txtCLIWobbleSpeed;
        private System.Windows.Forms.Label lblCLIPreview;
        private System.Windows.Forms.TextBox txtCLIPreview;
        private System.Windows.Forms.Label lblCLIPreviewSpeed;
        private System.Windows.Forms.TextBox txtCLIPreviewSpeed;
        private System.Windows.Forms.Label lblCLIPreviewTime;
        private System.Windows.Forms.TextBox txtCLIPreviewTime;
        private System.Windows.Forms.CheckBox chkCLIMark;
        private System.Windows.Forms.GroupBox grpCLIQRCode;
        private System.Windows.Forms.Label lblCLIQRContent;
        private System.Windows.Forms.TextBox txtCLIQRContent;
        private System.Windows.Forms.Label lblCLIQRFixedHeader;
        private System.Windows.Forms.Label lblCLIQRFixedParams;
        private System.Windows.Forms.Label lblCLIQROutput;
        private System.Windows.Forms.TextBox txtCLIQROutput;
        private System.Windows.Forms.Button btnCLIQRRefresh;
        private System.Windows.Forms.Button btnCLIQRExecuteMark;
        private System.Windows.Forms.Button btnCLIQRStopPreview;
        private System.Windows.Forms.Label lblCLIOutput;
        private System.Windows.Forms.TextBox txtCLIOutput;
        private System.Windows.Forms.Button btnCLIRefresh;
        private System.Windows.Forms.Button btnCLIExecuteMark;
        private System.Windows.Forms.Button btnCLIStopPreview;
        // === 命令提示 tab 控件 ===
        private System.Windows.Forms.TabPage tabPageCmd;
        private System.Windows.Forms.Label lblCmdHeader;
        private System.Windows.Forms.Label lblBoardCmd;
        private System.Windows.Forms.ComboBox comboBoardCmd;
        private System.Windows.Forms.Button btnCmdRegen;
        private System.Windows.Forms.Label lblCmd1;
        private System.Windows.Forms.TextBox txtCmd1;
        private System.Windows.Forms.Button btnCmd1;
        private System.Windows.Forms.Label lblCmd2;
        private System.Windows.Forms.TextBox txtCmd2;
        private System.Windows.Forms.Button btnCmd2;
        private System.Windows.Forms.Label lblCmd3;
        private System.Windows.Forms.TextBox txtCmd3;
        private System.Windows.Forms.Button btnCmd3;
        private System.Windows.Forms.Label lblCmd4;
        private System.Windows.Forms.TextBox txtCmd4;
        private System.Windows.Forms.Button btnCmd4;
        private System.Windows.Forms.Label lblCmd5;
        private System.Windows.Forms.TextBox txtCmd5;
        private System.Windows.Forms.Button btnCmd5;
        private System.Windows.Forms.Label lblCmdHint;
        // === 並行驗證控件 ===
        private System.Windows.Forms.Timer timerParallelTest;
        private System.Windows.Forms.Button btnParallelTest;
        private System.Windows.Forms.TextBox txtParallelResult;

        // === 6-1 QR Code 頁：黑矩形 GroupBox ===
        private System.Windows.Forms.GroupBox groupBoxBlackRect;
        private System.Windows.Forms.Label lblBRWidth;
        private System.Windows.Forms.TextBox txtBRWidth;
        private System.Windows.Forms.Label lblBRHeight;
        private System.Windows.Forms.TextBox txtBRHeight;
        private System.Windows.Forms.Label lblBRSpeed;
        private System.Windows.Forms.TextBox txtBRSpeed;
        private System.Windows.Forms.Label lblBRPower;
        private System.Windows.Forms.TextBox txtBRPower;
        private System.Windows.Forms.Label lblBRFreq;
        private System.Windows.Forms.TextBox txtBRFreq;
        private System.Windows.Forms.Label lblBRRepeat;
        private System.Windows.Forms.TextBox txtBRRepeat;
        private System.Windows.Forms.Label lblBRSpotDelay;
        private System.Windows.Forms.TextBox txtBRSpotDelay;
        private System.Windows.Forms.Label lblBRPulseWidth;
        private System.Windows.Forms.TextBox txtBRPulseWidth;
        private System.Windows.Forms.Label lblBRFillPitch;
        private System.Windows.Forms.TextBox txtBRFillPitch;
        private System.Windows.Forms.Label lblBRFillRoundPitch;
        private System.Windows.Forms.TextBox txtBRFillRoundPitch;
        private System.Windows.Forms.Label lblBRFillTimes;
        private System.Windows.Forms.TextBox txtBRFillTimes;
        private System.Windows.Forms.Label lblBRFillStepAngle;
        private System.Windows.Forms.TextBox txtBRFillStepAngle;
        private System.Windows.Forms.Button btnBRPreview;
        private System.Windows.Forms.Button btnBRStopPreview;
        private System.Windows.Forms.Button btnBRMark;

        // === 6-1 QR Code 頁：白矩形 GroupBox ===
        private System.Windows.Forms.GroupBox groupBoxWhiteRect;
        private System.Windows.Forms.Label lblWRWidth;
        private System.Windows.Forms.TextBox txtWRWidth;
        private System.Windows.Forms.Label lblWRHeight;
        private System.Windows.Forms.TextBox txtWRHeight;
        private System.Windows.Forms.Label lblWRSpeed;
        private System.Windows.Forms.TextBox txtWRSpeed;
        private System.Windows.Forms.Label lblWRPower;
        private System.Windows.Forms.TextBox txtWRPower;
        private System.Windows.Forms.Label lblWRFreq;
        private System.Windows.Forms.TextBox txtWRFreq;
        private System.Windows.Forms.Label lblWRRepeat;
        private System.Windows.Forms.TextBox txtWRRepeat;
        private System.Windows.Forms.Label lblWRSpotDelay;
        private System.Windows.Forms.TextBox txtWRSpotDelay;
        private System.Windows.Forms.Label lblWRPulseWidth;
        private System.Windows.Forms.TextBox txtWRPulseWidth;
        private System.Windows.Forms.Label lblWRFillPitch;
        private System.Windows.Forms.TextBox txtWRFillPitch;
        private System.Windows.Forms.Label lblWRFillRoundPitch;
        private System.Windows.Forms.TextBox txtWRFillRoundPitch;
        private System.Windows.Forms.Label lblWRFillTimes;
        private System.Windows.Forms.TextBox txtWRFillTimes;
        private System.Windows.Forms.Label lblWRFillStepAngle;
        private System.Windows.Forms.TextBox txtWRFillStepAngle;
        private System.Windows.Forms.Button btnWRPreview;
        private System.Windows.Forms.Button btnWRStopPreview;
        private System.Windows.Forms.Button btnWRMark;

        // === 6-1 QR Code 頁：單打 QR GroupBox ===
        private System.Windows.Forms.GroupBox groupBoxQROnly;
        private System.Windows.Forms.Label lblQOContent;
        private System.Windows.Forms.TextBox txtQOContent;
        private System.Windows.Forms.Label lblQOWidth;
        private System.Windows.Forms.TextBox txtQOWidth;
        private System.Windows.Forms.Label lblQOHeight;
        private System.Windows.Forms.TextBox txtQOHeight;
        private System.Windows.Forms.Label lblQOBorder;
        private System.Windows.Forms.TextBox txtQOBorder;
        private System.Windows.Forms.CheckBox chkQOInvert;
        private System.Windows.Forms.Label lblQOMarkStyle;
        private System.Windows.Forms.TextBox txtQOMarkStyle;
        private System.Windows.Forms.Label lblQORepeat;
        private System.Windows.Forms.TextBox txtQORepeat;
        private System.Windows.Forms.Label lblQOStepAngle;
        private System.Windows.Forms.TextBox txtQOStepAngle;
        private System.Windows.Forms.Label lblQOPower;
        private System.Windows.Forms.TextBox txtQOPower;
        private System.Windows.Forms.Label lblQOSpeed;
        private System.Windows.Forms.TextBox txtQOSpeed;
        private System.Windows.Forms.Label lblQOFreq;
        private System.Windows.Forms.TextBox txtQOFreq;
        private System.Windows.Forms.Label lblQOPulseWidth;
        private System.Windows.Forms.TextBox txtQOPulseWidth;
        private System.Windows.Forms.Button btnQOPreview;
        private System.Windows.Forms.Button btnQOStopPreview;
        private System.Windows.Forms.Button btnQOMark;

        // === 6-1 QR Code 頁：同時執行三個 GroupBox（順序 1→2→3） ===
        private System.Windows.Forms.Button btnAllPreview;
        private System.Windows.Forms.Button btnAllStopPreview;
        private System.Windows.Forms.Button btnAllMark;

        // === 6-1 QR Code 頁：晶片板選擇（獨立於 6. QR Code）===
        private System.Windows.Forms.Label lblBoardQR2;
        private System.Windows.Forms.ComboBox comboBoardQR2;
    }
}
