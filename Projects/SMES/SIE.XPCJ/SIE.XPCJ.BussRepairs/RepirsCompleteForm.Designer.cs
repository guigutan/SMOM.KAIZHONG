
namespace SIE.XPCJ.BussRepairs
{
    partial class RepirsCompleteForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.xpComboBox1 = new SIE.XPCJ.Common.Controls.XPComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.xpTextBoxNextProcess = new SIE.XPCJ.Common.Controls.XPTextBox();
            this.panel1.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(451, 56);
            this.panel1.TabIndex = 3;
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
            this.button2.Location = new System.Drawing.Point(209, 0);
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
            this.button1.Location = new System.Drawing.Point(330, 0);
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
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 56);
            this.label1.TabIndex = 18;
            this.label1.Text = "上线工序";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "上线工序";
            // 
            // xpComboBox1
            // 
            this.xpComboBox1.DisplayMember = "PathDescription";
            this.xpComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.xpComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xpComboBox1.FormattingEnabled = true;
            this.xpComboBox1.ItemHeight = 24;
            this.xpComboBox1.Location = new System.Drawing.Point(15, 89);
            this.xpComboBox1.Name = "xpComboBox1";
            this.xpComboBox1.Size = new System.Drawing.Size(424, 30);
            this.xpComboBox1.TabIndex = 5;
            this.xpComboBox1.SelectedIndexChanged += new System.EventHandler(this.xpComboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.label2.Location = new System.Drawing.Point(12, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "后工序";
            // 
            // xpTextBoxNextProcess
            // 
            this.xpTextBoxNextProcess.AText = "";
            this.xpTextBoxNextProcess.BackColor = System.Drawing.SystemColors.Window;
            this.xpTextBoxNextProcess.Location = new System.Drawing.Point(15, 153);
            this.xpTextBoxNextProcess.Name = "xpTextBoxNextProcess";
            this.xpTextBoxNextProcess.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.xpTextBoxNextProcess.ReadOnly = false;
            this.xpTextBoxNextProcess.Size = new System.Drawing.Size(424, 37);
            this.xpTextBoxNextProcess.TabIndex = 7;
            // 
            // RepirsCompleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(451, 249);
            this.ControlBox = false;
            this.Controls.Add(this.xpTextBoxNextProcess);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.xpComboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RepirsCompleteForm";
            this.Text = "上线工序";
            this.Load += new System.EventHandler(this.RepirsCompleteForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Common.Controls.XPButton button2;
        private Common.Controls.XPButton button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Common.Controls.XPComboBox xpComboBox1;
        private System.Windows.Forms.Label label2;
        private Common.Controls.XPTextBox xpTextBoxNextProcess;
    }
}