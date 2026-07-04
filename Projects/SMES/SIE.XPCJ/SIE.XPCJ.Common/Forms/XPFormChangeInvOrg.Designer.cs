using SIE.XPCJ.Common.Controls;

namespace SIE.XPCJ.Common.Forms
{
    partial class XPFormChangeInvOrg
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.xpListBox1 = new SIE.XPCJ.Common.Controls.XPListBoxCtr();
            this.xpDialogTitle1 = new SIE.XPCJ.Common.Controls.XPDialogTitle();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(633, 1);
            this.panel3.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(633, 2);
            this.panel2.TabIndex = 17;
            // 
            // xpListBox1
            // 
            this.xpListBox1.BackColor = System.Drawing.Color.White;
            this.xpListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.xpListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.xpListBox1.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpListBox1.FormattingEnabled = true;
            this.xpListBox1.ItemHeight = 80;
            this.xpListBox1.Location = new System.Drawing.Point(2, 63);
            this.xpListBox1.Name = "xpListBox1";
            this.xpListBox1.Size = new System.Drawing.Size(627, 422);
            this.xpListBox1.TabIndex = 22;
            // 
            // xpDialogTitle1
            // 
            this.xpDialogTitle1.ATileFont = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpDialogTitle1.ATitle = "切换库存组织";
            this.xpDialogTitle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.xpDialogTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xpDialogTitle1.Location = new System.Drawing.Point(0, 3);
            this.xpDialogTitle1.Margin = new System.Windows.Forms.Padding(0);
            this.xpDialogTitle1.Name = "xpDialogTitle1";
            this.xpDialogTitle1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.xpDialogTitle1.Size = new System.Drawing.Size(633, 58);
            this.xpDialogTitle1.TabIndex = 20;
            this.xpDialogTitle1.AOkClick += new System.EventHandler(this.xpDialogTitle1_AOkClick);
            // 
            // XPFormChangeInvOrg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(633, 493);
            this.ControlBox = false;
            this.Controls.Add(this.xpListBox1);
            this.Controls.Add(this.xpDialogTitle1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "XPFormChangeInvOrg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "切换库存组织";
            this.Load += new System.EventHandler(this.FormChangeInvOrg_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private XPDialogTitle xpDialogTitle1;
        private XPListBoxCtr xpListBox1;
    }
}