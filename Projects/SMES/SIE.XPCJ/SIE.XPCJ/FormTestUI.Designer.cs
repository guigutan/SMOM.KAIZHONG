namespace SIE.XPCJ
{
    partial class FormTestUI
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
            this.xpTitle1 = new SIE.XPCJ.Common.Controls.XPTitle();
            this.SuspendLayout();
            // 
            // xpTitle1
            // 
            this.xpTitle1.AInvOrg = "    库存组织";
            this.xpTitle1.AProcessType = SIE.XPCJ.Models.Enums.ProcessType.Assembly;
            this.xpTitle1.ATileFont = new System.Drawing.Font("宋体", 26F, System.Drawing.FontStyle.Bold);
            this.xpTitle1.ATitle = "过站采集";
            this.xpTitle1.AType = SIE.XPCJ.Common.Controls.EnumXPTitleType.WorkerCell;
            this.xpTitle1.AUserInfo = "当前用户";
            this.xpTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xpTitle1.Location = new System.Drawing.Point(0, 0);
            this.xpTitle1.Margin = new System.Windows.Forms.Padding(4);
            this.xpTitle1.Name = "xpTitle1";
            this.xpTitle1.Size = new System.Drawing.Size(1095, 87);
            this.xpTitle1.TabIndex = 0;
            // 
            // FormTestUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(1095, 450);
            this.Controls.Add(this.xpTitle1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTestUI";
            this.Text = "FormTestUI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.XPTitle xpTitle1;
    }
}