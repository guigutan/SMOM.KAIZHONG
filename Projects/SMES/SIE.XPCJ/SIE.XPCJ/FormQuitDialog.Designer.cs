
namespace SIE.XPCJ
{
    partial class FormQuitDialog
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
            this.xpButtonCancel = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButtonGoToLogin = new SIE.XPCJ.Common.Controls.XPButton();
            this.xpButtonExit = new SIE.XPCJ.Common.Controls.XPButton();
            this.SuspendLayout();
            // 
            // xpButtonCancel
            // 
            this.xpButtonCancel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.xpButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpButtonCancel.IsPrivilegeAllow = true;
            this.xpButtonCancel.Location = new System.Drawing.Point(82, 310);
            this.xpButtonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xpButtonCancel.Name = "xpButtonCancel";
            this.xpButtonCancel.PrivilegeName = null;
            this.xpButtonCancel.Size = new System.Drawing.Size(379, 72);
            this.xpButtonCancel.TabIndex = 2;
            this.xpButtonCancel.Text = "取消";
            this.xpButtonCancel.UseVisualStyleBackColor = false;
            this.xpButtonCancel.Click += new System.EventHandler(this.xpButtonCancel_Click);
            // 
            // xpButtonGoToLogin
            // 
            this.xpButtonGoToLogin.BackColor = System.Drawing.Color.BurlyWood;
            this.xpButtonGoToLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonGoToLogin.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpButtonGoToLogin.IsPrivilegeAllow = true;
            this.xpButtonGoToLogin.Location = new System.Drawing.Point(82, 183);
            this.xpButtonGoToLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xpButtonGoToLogin.Name = "xpButtonGoToLogin";
            this.xpButtonGoToLogin.PrivilegeName = null;
            this.xpButtonGoToLogin.Size = new System.Drawing.Size(379, 72);
            this.xpButtonGoToLogin.TabIndex = 1;
            this.xpButtonGoToLogin.Text = "切换账号";
            this.xpButtonGoToLogin.UseVisualStyleBackColor = false;
            this.xpButtonGoToLogin.Click += new System.EventHandler(this.xpButtonGoToLogin_Click);
            // 
            // xpButtonExit
            // 
            this.xpButtonExit.BackColor = System.Drawing.Color.RosyBrown;
            this.xpButtonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonExit.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xpButtonExit.IsPrivilegeAllow = true;
            this.xpButtonExit.Location = new System.Drawing.Point(82, 57);
            this.xpButtonExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xpButtonExit.Name = "xpButtonExit";
            this.xpButtonExit.PrivilegeName = null;
            this.xpButtonExit.Size = new System.Drawing.Size(379, 72);
            this.xpButtonExit.TabIndex = 0;
            this.xpButtonExit.Text = "退出赛意SMOM采集系统";
            this.xpButtonExit.UseVisualStyleBackColor = false;
            this.xpButtonExit.Click += new System.EventHandler(this.xpButtonExit_Click);
            // 
            // FormQuitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 455);
            this.Controls.Add(this.xpButtonCancel);
            this.Controls.Add(this.xpButtonGoToLogin);
            this.Controls.Add(this.xpButtonExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormQuitDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "赛意SMOM采集系统";
            this.ResumeLayout(false);

        }

        #endregion

        private SIE.XPCJ.Common.Controls.XPButton xpButtonExit;
        private SIE.XPCJ.Common.Controls.XPButton xpButtonGoToLogin;
        private SIE.XPCJ.Common.Controls.XPButton xpButtonCancel;
    }
}