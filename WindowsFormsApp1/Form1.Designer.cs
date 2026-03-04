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
            this.panelBoard1 = new System.Windows.Forms.Panel();
            this.panelBoard2 = new System.Windows.Forms.Panel();
            this.panelBoard3 = new System.Windows.Forms.Panel();
            this.panelBoard4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnDrawLine = new System.Windows.Forms.Button();
            this.btnMark = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtX1 = new System.Windows.Forms.TextBox();
            this.txtY1 = new System.Windows.Forms.TextBox();
            this.txtX2 = new System.Windows.Forms.TextBox();
            this.txtY2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.comboBoard = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            //
            // panelBoard1
            //
            this.panelBoard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard1.Location = new System.Drawing.Point(15, 30);
            this.panelBoard1.Name = "panelBoard1";
            this.panelBoard1.Size = new System.Drawing.Size(400, 300);
            this.panelBoard1.TabIndex = 0;
            //
            // panelBoard2
            //
            this.panelBoard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard2.Location = new System.Drawing.Point(15, 30);
            this.panelBoard2.Name = "panelBoard2";
            this.panelBoard2.Size = new System.Drawing.Size(400, 300);
            this.panelBoard2.TabIndex = 0;
            //
            // panelBoard3
            //
            this.panelBoard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard3.Location = new System.Drawing.Point(15, 30);
            this.panelBoard3.Name = "panelBoard3";
            this.panelBoard3.Size = new System.Drawing.Size(400, 300);
            this.panelBoard3.TabIndex = 0;
            //
            // panelBoard4
            //
            this.panelBoard4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard4.Location = new System.Drawing.Point(15, 30);
            this.panelBoard4.Name = "panelBoard4";
            this.panelBoard4.Size = new System.Drawing.Size(400, 300);
            this.panelBoard4.TabIndex = 0;
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.panelBoard1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 345);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "晶片板 1";
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.panelBoard2);
            this.groupBox2.Location = new System.Drawing.Point(448, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 345);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "晶片板 2";
            //
            // groupBox3
            //
            this.groupBox3.Controls.Add(this.panelBoard3);
            this.groupBox3.Location = new System.Drawing.Point(12, 363);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(430, 345);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "晶片板 3";
            //
            // groupBox4
            //
            this.groupBox4.Controls.Add(this.panelBoard4);
            this.groupBox4.Location = new System.Drawing.Point(448, 363);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(430, 345);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "晶片板 4";
            //
            // btnInit
            //
            this.btnInit.Location = new System.Drawing.Point(900, 30);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(120, 40);
            this.btnInit.TabIndex = 4;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            //
            // btnDrawLine
            //
            this.btnDrawLine.Location = new System.Drawing.Point(900, 250);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(120, 40);
            this.btnDrawLine.TabIndex = 5;
            this.btnDrawLine.Text = "繪製線段";
            this.btnDrawLine.UseVisualStyleBackColor = true;
            this.btnDrawLine.Click += new System.EventHandler(this.btnDrawLine_Click);
            //
            // btnMark
            //
            this.btnMark.Location = new System.Drawing.Point(900, 300);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(120, 40);
            this.btnMark.TabIndex = 6;
            this.btnMark.Text = "開始雷射";
            this.btnMark.UseVisualStyleBackColor = true;
            this.btnMark.Click += new System.EventHandler(this.btnMark_Click);
            //
            // btnStop
            //
            this.btnStop.Location = new System.Drawing.Point(900, 350);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 40);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            //
            // btnExit
            //
            this.btnExit.Location = new System.Drawing.Point(900, 650);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 40);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            //
            // txtX1
            //
            this.txtX1.Location = new System.Drawing.Point(60, 30);
            this.txtX1.Name = "txtX1";
            this.txtX1.Size = new System.Drawing.Size(80, 22);
            this.txtX1.TabIndex = 9;
            this.txtX1.Text = "0";
            //
            // txtY1
            //
            this.txtY1.Location = new System.Drawing.Point(60, 60);
            this.txtY1.Name = "txtY1";
            this.txtY1.Size = new System.Drawing.Size(80, 22);
            this.txtY1.TabIndex = 10;
            this.txtY1.Text = "0";
            //
            // txtX2
            //
            this.txtX2.Location = new System.Drawing.Point(60, 90);
            this.txtX2.Name = "txtX2";
            this.txtX2.Size = new System.Drawing.Size(80, 22);
            this.txtX2.TabIndex = 11;
            this.txtX2.Text = "50";
            //
            // txtY2
            //
            this.txtY2.Location = new System.Drawing.Point(60, 120);
            this.txtY2.Name = "txtY2";
            this.txtY2.Size = new System.Drawing.Size(80, 22);
            this.txtY2.TabIndex = 12;
            this.txtY2.Text = "50";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "X1:";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "Y1:";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "X2:";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "Y2:";
            //
            // groupBox5
            //
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.comboBoard);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.txtX1);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtY1);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.txtX2);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.txtY2);
            this.groupBox5.Location = new System.Drawing.Point(900, 80);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(160, 160);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "線段參數";
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
            this.comboBoard.Location = new System.Drawing.Point(22, 150);
            this.comboBoard.Name = "comboBoard";
            this.comboBoard.Size = new System.Drawing.Size(118, 20);
            this.comboBoard.TabIndex = 17;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "選擇板:";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 720);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnMark);
            this.Controls.Add(this.btnDrawLine);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "四晶片板雷射打標系統";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
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
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnDrawLine;
        private System.Windows.Forms.Button btnMark;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtX1;
        private System.Windows.Forms.TextBox txtY1;
        private System.Windows.Forms.TextBox txtX2;
        private System.Windows.Forms.TextBox txtY2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox comboBoard;
        private System.Windows.Forms.Label label5;
    }
}

