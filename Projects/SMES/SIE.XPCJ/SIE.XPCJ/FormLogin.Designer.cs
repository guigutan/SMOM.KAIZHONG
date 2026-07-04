
namespace SIE.XPCJ
{
    partial class FormLogin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCopyRight = new System.Windows.Forms.Label();
            this.comboBoxAccount = new System.Windows.Forms.ComboBox();
            this.labelCampany = new System.Windows.Forms.Label();
            this.buttonClose = new SIE.XPCJ.Common.Controls.XPButton();
            this.buttonLogin = new SIE.XPCJ.Common.Controls.XPButton();
            this.textBoxPwd = new System.Windows.Forms.TextBox();
            this.labelPwd = new System.Windows.Forms.Label();
            this.labelAccount = new System.Windows.Forms.Label();
            this.buttonSetting = new SIE.XPCJ.Common.Controls.XPButton();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.labelMsg = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.checkBoxRemberPwd = new System.Windows.Forms.CheckBox();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCopyRight
            // 
            this.labelCopyRight.AutoSize = true;
            this.labelCopyRight.BackColor = System.Drawing.Color.Transparent;
            this.labelCopyRight.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCopyRight.ForeColor = System.Drawing.SystemColors.Window;
            this.labelCopyRight.Location = new System.Drawing.Point(330, 628);
            this.labelCopyRight.Name = "labelCopyRight";
            this.labelCopyRight.Size = new System.Drawing.Size(351, 22);
            this.labelCopyRight.TabIndex = 7;
            this.labelCopyRight.Text = "© 2024 SIE ALL Rights Reserved.";
            this.labelCopyRight.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxAccount
            // 
            this.comboBoxAccount.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxAccount.FormattingEnabled = true;
            this.comboBoxAccount.Location = new System.Drawing.Point(316, 239);
            this.comboBoxAccount.Name = "comboBoxAccount";
            this.comboBoxAccount.Size = new System.Drawing.Size(393, 37);
            this.comboBoxAccount.TabIndex = 5;
            this.comboBoxAccount.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccount_SelectedIndexChanged);
            // 
            // labelCampany
            // 
            this.labelCampany.AutoSize = true;
            this.labelCampany.BackColor = System.Drawing.Color.Transparent;
            this.labelCampany.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCampany.ForeColor = System.Drawing.SystemColors.Window;
            this.labelCampany.Location = new System.Drawing.Point(350, 582);
            this.labelCampany.Name = "labelCampany";
            this.labelCampany.Size = new System.Drawing.Size(318, 22);
            this.labelCampany.TabIndex = 6;
            this.labelCampany.Text = "广州赛意信息科技股份有限公司";
            this.labelCampany.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.Transparent;
            this.buttonClose.BackgroundImage = global::SIE.XPCJ.Properties.Resources.关闭;
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.ForeColorDisable = System.Drawing.Color.Silver;
            this.buttonClose.ForeColorEnable = System.Drawing.Color.White;
            this.buttonClose.IsPrivilegeAllow = true;
            this.buttonClose.Location = new System.Drawing.Point(920, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.PrivilegeName = null;
            this.buttonClose.Size = new System.Drawing.Size(80, 60);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(108)))), ((int)(((byte)(204)))));
            this.buttonLogin.FlatAppearance.BorderSize = 0;
            this.buttonLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogin.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLogin.ForeColor = System.Drawing.SystemColors.Window;
            this.buttonLogin.ForeColorDisable = System.Drawing.Color.Silver;
            this.buttonLogin.ForeColorEnable = System.Drawing.Color.White;
            this.buttonLogin.IsPrivilegeAllow = true;
            this.buttonLogin.Location = new System.Drawing.Point(316, 471);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.PrivilegeName = null;
            this.buttonLogin.Size = new System.Drawing.Size(393, 44);
            this.buttonLogin.TabIndex = 4;
            this.buttonLogin.Text = "登  录";
            this.buttonLogin.UseVisualStyleBackColor = false;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxPwd
            // 
            this.textBoxPwd.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxPwd.Location = new System.Drawing.Point(316, 339);
            this.textBoxPwd.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPwd.Name = "textBoxPwd";
            this.textBoxPwd.PasswordChar = '*';
            this.textBoxPwd.Size = new System.Drawing.Size(393, 38);
            this.textBoxPwd.TabIndex = 3;
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.BackColor = System.Drawing.Color.Transparent;
            this.labelPwd.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPwd.ForeColor = System.Drawing.SystemColors.Window;
            this.labelPwd.Location = new System.Drawing.Point(312, 313);
            this.labelPwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(120, 22);
            this.labelPwd.TabIndex = 2;
            this.labelPwd.Text = "登录密码：";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.BackColor = System.Drawing.Color.Transparent;
            this.labelAccount.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelAccount.ForeColor = System.Drawing.SystemColors.Window;
            this.labelAccount.Location = new System.Drawing.Point(312, 211);
            this.labelAccount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(120, 22);
            this.labelAccount.TabIndex = 0;
            this.labelAccount.Text = "登录账号：";
            // 
            // buttonSetting
            // 
            this.buttonSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetting.BackColor = System.Drawing.Color.Transparent;
            this.buttonSetting.BackgroundImage = global::SIE.XPCJ.Properties.Resources.设置;
            this.buttonSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonSetting.FlatAppearance.BorderSize = 0;
            this.buttonSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetting.ForeColorDisable = System.Drawing.Color.Silver;
            this.buttonSetting.ForeColorEnable = System.Drawing.Color.White;
            this.buttonSetting.IsPrivilegeAllow = true;
            this.buttonSetting.Location = new System.Drawing.Point(836, 0);
            this.buttonSetting.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.PrivilegeName = null;
            this.buttonSetting.Size = new System.Drawing.Size(80, 60);
            this.buttonSetting.TabIndex = 5;
            this.buttonSetting.UseVisualStyleBackColor = false;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Image = global::SIE.XPCJ.Properties.Resources.SMOM_LOGO;
            this.pictureBoxLogo.Location = new System.Drawing.Point(354, 73);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(339, 117);
            this.pictureBoxLogo.TabIndex = 8;
            this.pictureBoxLogo.TabStop = false;
            // 
            // labelMsg
            // 
            this.labelMsg.BackColor = System.Drawing.Color.Transparent;
            this.labelMsg.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMsg.ForeColor = System.Drawing.Color.Red;
            this.labelMsg.Location = new System.Drawing.Point(147, 535);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(747, 30);
            this.labelMsg.TabIndex = 9;
            this.labelMsg.Text = "密码或账号错误";
            this.labelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.BackColor = System.Drawing.Color.Transparent;
            this.labelVersion.ForeColor = System.Drawing.SystemColors.Window;
            this.labelVersion.Location = new System.Drawing.Point(462, 668);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(80, 16);
            this.labelVersion.TabIndex = 10;
            this.labelVersion.Text = "V 1.0.0.0";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkBoxRemberPwd
            // 
            this.checkBoxRemberPwd.AutoSize = true;
            this.checkBoxRemberPwd.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRemberPwd.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxRemberPwd.ForeColor = System.Drawing.SystemColors.Window;
            this.checkBoxRemberPwd.Location = new System.Drawing.Point(315, 414);
            this.checkBoxRemberPwd.Name = "checkBoxRemberPwd";
            this.checkBoxRemberPwd.Size = new System.Drawing.Size(117, 26);
            this.checkBoxRemberPwd.TabIndex = 11;
            this.checkBoxRemberPwd.Text = "记住密码";
            this.checkBoxRemberPwd.UseVisualStyleBackColor = false;
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.Font = new System.Drawing.Font("宋体", 18F);
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(561, 410);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(149, 32);
            this.comboBoxLanguage.TabIndex = 12;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguage_SelectedIndexChanged);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SIE.XPCJ.Properties.Resources.登录页_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.ControlBox = false;
            this.Controls.Add(this.comboBoxLanguage);
            this.Controls.Add(this.checkBoxRemberPwd);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.labelCampany);
            this.Controls.Add(this.labelCopyRight);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSetting);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.comboBoxAccount);
            this.Controls.Add(this.textBoxPwd);
            this.Controls.Add(this.labelPwd);
            this.Controls.Add(this.labelAccount);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogin";
            this.Text = "赛意采集-登录";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxPwd;
        private System.Windows.Forms.Label labelPwd;
        private System.Windows.Forms.Label labelAccount;
        private SIE.XPCJ.Common.Controls.XPButton buttonLogin;
        private SIE.XPCJ.Common.Controls.XPButton buttonSetting;
        private SIE.XPCJ.Common.Controls.XPButton buttonClose;
        private System.Windows.Forms.ComboBox comboBoxAccount;
        private System.Windows.Forms.Label labelCampany;
        private System.Windows.Forms.Label labelCopyRight;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.CheckBox checkBoxRemberPwd;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
    }
}

