
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    partial class XPFormNumberInput
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.xpButton13 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton12 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton11 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton9 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton8 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton7 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton6 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton5 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton4 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton3 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton2 = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButton1 = new SIE.XPCJ.Common.Controls.XPButton();
            this.watermarkTextBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(612, 58);
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
            this.button2.Location = new System.Drawing.Point(430, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.PrivilegeName = null;
            this.button2.Size = new System.Drawing.Size(91, 58);
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
            this.button1.Location = new System.Drawing.Point(521, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.PrivilegeName = null;
            this.button1.Size = new System.Drawing.Size(91, 58);
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
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(433, 62);
            this.label1.TabIndex = 18;
            this.label1.Text = "输入数量";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.watermarkTextBox1);
            this.panel2.Location = new System.Drawing.Point(11, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(597, 416);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.xpButton13);
            this.panel3.Controls.Add(this.xpButton12);
            this.panel3.Controls.Add(this.xpButton11);
            this.panel3.Controls.Add(this.xpButton9);
            this.panel3.Controls.Add(this.xpButton8);
            this.panel3.Controls.Add(this.xpButton7);
            this.panel3.Controls.Add(this.xpButton6);
            this.panel3.Controls.Add(this.xpButton5);
            this.panel3.Controls.Add(this.xpButton4);
            this.panel3.Controls.Add(this.xpButton3);
            this.panel3.Controls.Add(this.xpButton2);
            this.panel3.Controls.Add(this.xpButton1);
            this.panel3.Location = new System.Drawing.Point(7, 73);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(583, 331);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.pictureBox2);
            this.panel5.Location = new System.Drawing.Point(446, 179);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(127, 122);
            this.panel5.TabIndex = 15;
            this.panel5.Click += new System.EventHandler(this.xpButton14_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(41, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 50);
            this.label3.TabIndex = 1;
            this.label3.Text = "清空";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Click += new System.EventHandler(this.xpButton14_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SIE.XPCJ.Common.Properties.Resources.清空;
            this.pictureBox2.Location = new System.Drawing.Point(15, 50);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.xpButton14_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Location = new System.Drawing.Point(446, 33);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(127, 122);
            this.panel4.TabIndex = 14;
            this.panel4.Click += new System.EventHandler(this.xpButton10_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(41, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "退格";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Click += new System.EventHandler(this.xpButton10_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SIE.XPCJ.Common.Properties.Resources.退格;
            this.pictureBox1.Location = new System.Drawing.Point(15, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.xpButton10_Click);
            // 
            // xpButton13
            // 
            this.xpButton13.BackColor = System.Drawing.Color.White;
            this.xpButton13.FlatAppearance.BorderSize = 0;
            this.xpButton13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton13.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton13.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton13.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_876;
            this.xpButton13.IsPrivilegeAllow = true;
            this.xpButton13.Location = new System.Drawing.Point(305, 252);
            this.xpButton13.Name = "xpButton13";
            this.xpButton13.PrivilegeName = null;
            this.xpButton13.Size = new System.Drawing.Size(127, 49);
            this.xpButton13.TabIndex = 12;
            this.xpButton13.UseVisualStyleBackColor = false;
            this.xpButton13.Click += new System.EventHandler(this.xpButton13_Click);
            // 
            // xpButton12
            // 
            this.xpButton12.BackColor = System.Drawing.Color.White;
            this.xpButton12.FlatAppearance.BorderSize = 0;
            this.xpButton12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton12.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton12.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton12.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_870;
            this.xpButton12.IsPrivilegeAllow = true;
            this.xpButton12.Location = new System.Drawing.Point(156, 252);
            this.xpButton12.Name = "xpButton12";
            this.xpButton12.PrivilegeName = null;
            this.xpButton12.Size = new System.Drawing.Size(127, 49);
            this.xpButton12.TabIndex = 11;
            this.xpButton12.UseVisualStyleBackColor = false;
            this.xpButton12.Click += new System.EventHandler(this.xpButton12_Click);
            // 
            // xpButton11
            // 
            this.xpButton11.BackColor = System.Drawing.Color.White;
            this.xpButton11.FlatAppearance.BorderSize = 0;
            this.xpButton11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton11.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton11.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton11.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_876;
            this.xpButton11.IsPrivilegeAllow = true;
            this.xpButton11.Location = new System.Drawing.Point(11, 252);
            this.xpButton11.Name = "xpButton11";
            this.xpButton11.PrivilegeName = null;
            this.xpButton11.Size = new System.Drawing.Size(127, 49);
            this.xpButton11.TabIndex = 10;
            this.xpButton11.UseVisualStyleBackColor = false;
            this.xpButton11.Click += new System.EventHandler(this.xpButton13_Click);
            // 
            // xpButton9
            // 
            this.xpButton9.BackColor = System.Drawing.Color.White;
            this.xpButton9.FlatAppearance.BorderSize = 0;
            this.xpButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton9.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton9.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton9.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_869;
            this.xpButton9.IsPrivilegeAllow = true;
            this.xpButton9.Location = new System.Drawing.Point(11, 179);
            this.xpButton9.Name = "xpButton9";
            this.xpButton9.PrivilegeName = null;
            this.xpButton9.Size = new System.Drawing.Size(127, 49);
            this.xpButton9.TabIndex = 8;
            this.xpButton9.UseVisualStyleBackColor = false;
            this.xpButton9.Click += new System.EventHandler(this.xpButton9_Click);
            // 
            // xpButton8
            // 
            this.xpButton8.BackColor = System.Drawing.Color.White;
            this.xpButton8.FlatAppearance.BorderSize = 0;
            this.xpButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton8.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton8.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton8.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_875;
            this.xpButton8.IsPrivilegeAllow = true;
            this.xpButton8.Location = new System.Drawing.Point(156, 179);
            this.xpButton8.Name = "xpButton8";
            this.xpButton8.PrivilegeName = null;
            this.xpButton8.Size = new System.Drawing.Size(127, 49);
            this.xpButton8.TabIndex = 7;
            this.xpButton8.UseVisualStyleBackColor = false;
            this.xpButton8.Click += new System.EventHandler(this.xpButton8_Click);
            // 
            // xpButton7
            // 
            this.xpButton7.BackColor = System.Drawing.Color.White;
            this.xpButton7.FlatAppearance.BorderSize = 0;
            this.xpButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton7.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton7.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton7.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_863;
            this.xpButton7.IsPrivilegeAllow = true;
            this.xpButton7.Location = new System.Drawing.Point(305, 106);
            this.xpButton7.Name = "xpButton7";
            this.xpButton7.PrivilegeName = null;
            this.xpButton7.Size = new System.Drawing.Size(127, 49);
            this.xpButton7.TabIndex = 6;
            this.xpButton7.UseVisualStyleBackColor = false;
            this.xpButton7.Click += new System.EventHandler(this.xpButton7_Click);
            // 
            // xpButton6
            // 
            this.xpButton6.BackColor = System.Drawing.Color.White;
            this.xpButton6.FlatAppearance.BorderSize = 0;
            this.xpButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton6.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton6.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton6.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_865;
            this.xpButton6.IsPrivilegeAllow = true;
            this.xpButton6.Location = new System.Drawing.Point(156, 106);
            this.xpButton6.Name = "xpButton6";
            this.xpButton6.PrivilegeName = null;
            this.xpButton6.Size = new System.Drawing.Size(127, 49);
            this.xpButton6.TabIndex = 5;
            this.xpButton6.UseVisualStyleBackColor = false;
            this.xpButton6.Click += new System.EventHandler(this.xpButton6_Click);
            // 
            // xpButton5
            // 
            this.xpButton5.BackColor = System.Drawing.Color.White;
            this.xpButton5.FlatAppearance.BorderSize = 0;
            this.xpButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton5.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton5.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton5.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_871;
            this.xpButton5.IsPrivilegeAllow = true;
            this.xpButton5.Location = new System.Drawing.Point(305, 179);
            this.xpButton5.Name = "xpButton5";
            this.xpButton5.PrivilegeName = null;
            this.xpButton5.Size = new System.Drawing.Size(127, 49);
            this.xpButton5.TabIndex = 4;
            this.xpButton5.UseVisualStyleBackColor = false;
            this.xpButton5.Click += new System.EventHandler(this.xpButton5_Click);
            // 
            // xpButton4
            // 
            this.xpButton4.BackColor = System.Drawing.Color.White;
            this.xpButton4.FlatAppearance.BorderSize = 0;
            this.xpButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton4.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton4.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton4.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_862;
            this.xpButton4.IsPrivilegeAllow = true;
            this.xpButton4.Location = new System.Drawing.Point(11, 106);
            this.xpButton4.Name = "xpButton4";
            this.xpButton4.PrivilegeName = null;
            this.xpButton4.Size = new System.Drawing.Size(127, 49);
            this.xpButton4.TabIndex = 3;
            this.xpButton4.UseVisualStyleBackColor = false;
            this.xpButton4.Click += new System.EventHandler(this.xpButton4_Click);
            // 
            // xpButton3
            // 
            this.xpButton3.BackColor = System.Drawing.Color.White;
            this.xpButton3.FlatAppearance.BorderSize = 0;
            this.xpButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton3.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton3.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton3.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_857;
            this.xpButton3.IsPrivilegeAllow = true;
            this.xpButton3.Location = new System.Drawing.Point(305, 33);
            this.xpButton3.Name = "xpButton3";
            this.xpButton3.PrivilegeName = null;
            this.xpButton3.Size = new System.Drawing.Size(127, 49);
            this.xpButton3.TabIndex = 2;
            this.xpButton3.UseVisualStyleBackColor = false;
            this.xpButton3.Click += new System.EventHandler(this.xpButton3_Click);
            // 
            // xpButton2
            // 
            this.xpButton2.BackColor = System.Drawing.Color.White;
            this.xpButton2.FlatAppearance.BorderSize = 0;
            this.xpButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton2.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton2.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton2.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_856;
            this.xpButton2.IsPrivilegeAllow = true;
            this.xpButton2.Location = new System.Drawing.Point(156, 33);
            this.xpButton2.Name = "xpButton2";
            this.xpButton2.PrivilegeName = null;
            this.xpButton2.Size = new System.Drawing.Size(127, 49);
            this.xpButton2.TabIndex = 1;
            this.xpButton2.UseVisualStyleBackColor = false;
            this.xpButton2.Click += new System.EventHandler(this.xpButton2_Click);
            // 
            // xpButton1
            // 
            this.xpButton1.BackColor = System.Drawing.Color.White;
            this.xpButton1.FlatAppearance.BorderSize = 0;
            this.xpButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton1.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButton1.ForeColorEnable = System.Drawing.Color.White;
            this.xpButton1.Image = global::SIE.XPCJ.Common.Properties.Resources.Frame_855;
            this.xpButton1.IsPrivilegeAllow = true;
            this.xpButton1.Location = new System.Drawing.Point(11, 33);
            this.xpButton1.Name = "xpButton1";
            this.xpButton1.PrivilegeName = null;
            this.xpButton1.Size = new System.Drawing.Size(127, 49);
            this.xpButton1.TabIndex = 0;
            this.xpButton1.UseVisualStyleBackColor = false;
            this.xpButton1.Click += new System.EventHandler(this.xpButton1_Click);
            // 
            // watermarkTextBox1
            // 
            this.watermarkTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.watermarkTextBox1.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.watermarkTextBox1.Location = new System.Drawing.Point(11, 7);
            this.watermarkTextBox1.Multiline = true;
            this.watermarkTextBox1.Name = "watermarkTextBox1";
            this.watermarkTextBox1.Size = new System.Drawing.Size(579, 50);
            this.watermarkTextBox1.TabIndex = 0;
            this.watermarkTextBox1.WordWrap = false;
            this.watermarkTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.watermarkTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.watermarkTextBox1_KeyUp);
            // 
            // XPFormNumberInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 474);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "XPFormNumberInput";
            this.Text = "输入数量";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private SIE.XPCJ.Common.Controls.XPButton button2;
        private SIE.XPCJ.Common.Controls.XPButton button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private TextBox watermarkTextBox1;
        private System.Windows.Forms.Panel panel3;
        private SIE.XPCJ.Common.Controls.XPButton xpButton1;
        private SIE.XPCJ.Common.Controls.XPButton xpButton13;
        private SIE.XPCJ.Common.Controls.XPButton xpButton12;
        private SIE.XPCJ.Common.Controls.XPButton xpButton11;
        private SIE.XPCJ.Common.Controls.XPButton xpButton9;
        private SIE.XPCJ.Common.Controls.XPButton xpButton8;
        private SIE.XPCJ.Common.Controls.XPButton xpButton7;
        private SIE.XPCJ.Common.Controls.XPButton xpButton6;
        private SIE.XPCJ.Common.Controls.XPButton xpButton5;
        private SIE.XPCJ.Common.Controls.XPButton xpButton4;
        private SIE.XPCJ.Common.Controls.XPButton xpButton3;
        private SIE.XPCJ.Common.Controls.XPButton xpButton2;
        private Panel panel4;
        private PictureBox pictureBox1;
        private Label label2;
        private Panel panel5;
        private Label label3;
        private PictureBox pictureBox2;
    }
}