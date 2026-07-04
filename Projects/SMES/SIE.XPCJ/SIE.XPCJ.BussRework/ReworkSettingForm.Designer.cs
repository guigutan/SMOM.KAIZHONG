
namespace SIE.XPCJ.BussRework
{
    partial class ReworkSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new SIE.XPCJ.Common.Controls.XPButton();
            this.button1 = new SIE.XPCJ.Common.Controls.XPButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.xpReplaceItemHandleMethod = new SIE.XPCJ.Common.Controls.XPSwitch();
            this.label14 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.SerialPortPanel = new System.Windows.Forms.Panel();
            this.xpButton1 = new SIE.XPCJ.Common.Controls.XPButton();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.swichBtnPort = new SIE.XPCJ.Common.Controls.XPSwitch();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 56);
            this.panel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(41)))), ((int)(((byte)(60)))));
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.ForeColorDisable = System.Drawing.Color.Silver;
            this.button2.ForeColorEnable = System.Drawing.Color.White;
            this.button2.IsPrivilegeAllow = true;
            this.button2.Location = new System.Drawing.Point(560, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.PrivilegeName = null;
            this.button2.Size = new System.Drawing.Size(121, 56);
            this.button2.TabIndex = 20;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.ForeColorDisable = System.Drawing.Color.Silver;
            this.button1.ForeColorEnable = System.Drawing.Color.White;
            this.button1.IsPrivilegeAllow = true;
            this.button1.Location = new System.Drawing.Point(681, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.PrivilegeName = null;
            this.button1.Size = new System.Drawing.Size(121, 56);
            this.button1.TabIndex = 19;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(571, 56);
            this.label1.TabIndex = 18;
            this.label1.Text = "返工采集-配置项";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(4, 60);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(791, 457);
            this.panel2.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.xpReplaceItemHandleMethod);
            this.panel5.Location = new System.Drawing.Point(0, 415);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(791, 42);
            this.panel5.TabIndex = 14;
            // 
            // xpReplaceItemHandleMethod
            // 
            this.xpReplaceItemHandleMethod.AChecked = false;
            this.xpReplaceItemHandleMethod.ACheckedIndex = 0;
            this.xpReplaceItemHandleMethod.ALeftText = "置换后作废";
            this.xpReplaceItemHandleMethod.AMiddleText = "";
            this.xpReplaceItemHandleMethod.ARightText = "置换后不良下料";
            this.xpReplaceItemHandleMethod.Location = new System.Drawing.Point(7, 6);
            this.xpReplaceItemHandleMethod.Margin = new System.Windows.Forms.Padding(4);
            this.xpReplaceItemHandleMethod.Name = "xpReplaceItemHandleMethod";
            this.xpReplaceItemHandleMethod.Size = new System.Drawing.Size(437, 30);
            this.xpReplaceItemHandleMethod.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.SystemColors.Control;
            this.label14.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(0, 385);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(791, 30);
            this.label14.TabIndex = 12;
            this.label14.Text = "物料置换后处理方式";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.SerialPortPanel);
            this.panel4.Controls.Add(this.xpButton1);
            this.panel4.Location = new System.Drawing.Point(0, 101);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(791, 284);
            this.panel4.TabIndex = 5;
            // 
            // SerialPortPanel
            // 
            this.SerialPortPanel.AutoScroll = true;
            this.SerialPortPanel.Location = new System.Drawing.Point(7, 38);
            this.SerialPortPanel.Name = "SerialPortPanel";
            this.SerialPortPanel.Size = new System.Drawing.Size(781, 242);
            this.SerialPortPanel.TabIndex = 3;
            // 
            // xpButton1
            // 
            this.xpButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.xpButton1.FlatAppearance.BorderSize = 0;
            this.xpButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton1.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton1.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton1.IsPrivilegeAllow = true;
            this.xpButton1.Location = new System.Drawing.Point(7, 3);
            this.xpButton1.Name = "xpButton1";
            this.xpButton1.PrivilegeName = null;
            this.xpButton1.Size = new System.Drawing.Size(82, 29);
            this.xpButton1.TabIndex = 2;
            this.xpButton1.Text = "新增";
            this.xpButton1.UseVisualStyleBackColor = false;
            this.xpButton1.Click += new System.EventHandler(this.xpButton1_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(0, 70);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(791, 31);
            this.label4.TabIndex = 4;
            this.label4.Text = "串口参数";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.swichBtnPort);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(0, 31);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(791, 39);
            this.panel3.TabIndex = 3;
            // 
            // swichBtnPort
            // 
            this.swichBtnPort.AChecked = false;
            this.swichBtnPort.ACheckedIndex = 0;
            this.swichBtnPort.ALeftText = "串口";
            this.swichBtnPort.AMiddleText = "";
            this.swichBtnPort.ARightText = "USB";
            this.swichBtnPort.Location = new System.Drawing.Point(141, 6);
            this.swichBtnPort.Margin = new System.Windows.Forms.Padding(4);
            this.swichBtnPort.Name = "swichBtnPort";
            this.swichBtnPort.Size = new System.Drawing.Size(227, 29);
            this.swichBtnPort.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "设备端口";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(791, 31);
            this.label2.TabIndex = 0;
            this.label2.Text = "端口类型";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReworkSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 521);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ReworkSettingForm";
            this.Text = "检验采集-配置项";
            this.Load += new System.EventHandler(this.NewPackageSettingForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private SIE.XPCJ.Common.Controls.XPButton button2;
        private SIE.XPCJ.Common.Controls.XPButton button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private SIE.XPCJ.Common.Controls.XPSwitch swichBtnPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel SerialPortPanel;
        private SIE.XPCJ.Common.Controls.XPButton xpButton1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel5;
        private Common.Controls.XPSwitch xpReplaceItemHandleMethod;
    }
}