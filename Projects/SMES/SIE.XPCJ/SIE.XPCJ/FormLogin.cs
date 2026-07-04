using SIE.XPCJ.Common;
using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ
{
    public partial class FormLogin : Common.Forms.FormBase
    {
        public FormLogin()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetDefaultTagText(this);
        }

        private bool isLoaded = false; //是否加载完成

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.comboBoxAccount.Location = new Point((this.ClientSize.Width - comboBoxAccount.Width) / 2, (this.ClientSize.Height - comboBoxAccount.Height) / 2 - 200);
            this.labelAccount.Location = new Point(this.comboBoxAccount.Location.X - 4, this.comboBoxAccount.Location.Y - 30);

            this.pictureBoxLogo.Location = new Point((this.ClientSize.Width - comboBoxAccount.Width) / 2, this.comboBoxAccount.Location.Y - 160);

            this.textBoxPwd.Location = new Point(this.comboBoxAccount.Location.X, this.comboBoxAccount.Location.Y + 100);
            this.labelPwd.Location = new Point(this.textBoxPwd.Location.X - 4, this.textBoxPwd.Location.Y - 30);

            this.checkBoxRemberPwd.Location = new Point(this.textBoxPwd.Location.X, this.textBoxPwd.Location.Y + 100);

            this.comboBoxLanguage.Location = new Point(this.textBoxPwd.Location.X + this.textBoxPwd.Width - this.comboBoxLanguage.Width, this.checkBoxRemberPwd.Location.Y - 6);

            this.buttonLogin.Location = new Point(this.textBoxPwd.Location.X, this.comboBoxLanguage.Location.Y + 80);

            this.labelMsg.Location = new Point((this.ClientSize.Width - labelMsg.Width) / 2, this.buttonLogin.Location.Y + 60);

            this.labelCampany.Location = new Point((this.ClientSize.Width - labelCampany.Width) / 2, this.labelMsg.Location.Y + 60);
            this.labelCopyRight.Location = new Point((this.ClientSize.Width - labelCopyRight.Width) / 2, this.labelCampany.Location.Y + 40);

            this.labelVersion.Text = $"V {Global.VersionName}";
            this.labelVersion.Location = new Point((this.ClientSize.Width - labelVersion.Width) / 2, this.labelCopyRight.Location.Y + 50);

            this.comboBoxLanguage.Items.Clear();
            this.comboBoxLanguage.Items.AddRange(LanguageSettings.All.Select(p => p.Name).ToArray());

            if (this.comboBoxLanguage.Items.Count > 0)
            {
                this.comboBoxLanguage.SelectedIndex = 0;
                Global.Language = LanguageSettings.All[comboBoxLanguage.SelectedIndex];
            }


            this.checkBoxRemberPwd.Checked = LoginInfo.Instance.IsRemberPwd;

            if (LoginInfo.Instance.HistoryAccountList.Count > 0 && LoginInfo.Instance.IsRemberPwd && LoginInfo.Instance.RemberUser == LoginInfo.Instance.HistoryAccountList[0])
                this.textBoxPwd.Text = LoginInfo.Instance.RemberPwd;


            if (!string.IsNullOrEmpty(LoginInfo.Instance.RemberLanguage))
            {
                int index = LanguageSettings.All.FindIndex(p => p.Code == LoginInfo.Instance.RemberLanguage);
                if (index >= 0)
                {
                    this.comboBoxLanguage.SelectedIndex = index;
                    Global.Language = LanguageSettings.All[comboBoxLanguage.SelectedIndex];
                }
            }
            this.comboBoxAccount.Items.Clear();
            this.comboBoxAccount.Items.AddRange(LoginInfo.Instance.HistoryAccountList.ToArray());
            if (this.comboBoxAccount.Items.Count > 0)
            {
                this.comboBoxAccount.SelectedIndex = 0;
                this.comboBoxAccount.Items.Add("清空历史".L10N());
            }

            this.labelMsg.Visible = false;

            this.isLoaded = true;
        }

        private void ResetPosition()
        {
            this.labelCampany.Location = new Point((this.ClientSize.Width - labelCampany.Width) / 2, this.labelMsg.Location.Y + 60);
        }

        private void ShowTips(string tipText)
        {
            this.labelMsg.Visible = true;
            this.labelMsg.Text = tipText;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出赛意SMOM采集系统吗？".L10N(), "赛意SMOM".L10N(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            FormSetting frmSetting = new FormSetting();
            var result = frmSetting.ShowDialog();
            if (result == DialogResult.OK)
            {
                LanguageSettings.UpdateLanguageSettings();
                LanguageSettings.All = null;//重置
                this.comboBoxLanguage.Items.Clear();
                this.comboBoxLanguage.Items.AddRange(LanguageSettings.All.Select(p => p.Name).ToArray());

                if (this.comboBoxLanguage.Items.Count > 0 && Global.Language != null && !string.IsNullOrEmpty(Global.Language.Code))
                {
                    var index = LanguageSettings.All.FindIndex(m => m.Code == Global.Language.Code);//如用户设置过则加载回原来的语言
                    if (index >= 0)
                    {
                        comboBoxLanguage.SelectedIndex = index;
                        Global.Language = LanguageSettings.All[comboBoxLanguage.SelectedIndex];
                    }
                }
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            this.ShowTips("");

            LoginInfo.Instance.UserCode = this.comboBoxAccount.Text.Trim();
            LoginInfo.Instance.Password = this.textBoxPwd.Text.Trim();

            if (string.IsNullOrEmpty(AppSettings.Instance.ApiUrl) || string.IsNullOrEmpty(AppSettings.Instance.AttachUrl))
            {
                buttonSetting_Click(sender, e);
                return;
            }

            if (string.IsNullOrEmpty(LoginInfo.Instance.UserCode))
            {
                ShowTips("请输入账号".L10N());
                return;
            }

            if (string.IsNullOrEmpty(LoginInfo.Instance.Password))
            {
                ShowTips("请输入密码".L10N());
                return;
            }

            try
            {
                new Task(() =>
                {
                    Thread.Sleep(1000);
                    ApiHelper.Login(this, LoginCallback);
                }).Start();

                this.Invoke(new Action(() =>
                {
                    this.ShowLoading();
                }));
            }
            catch (Exception ex)
            {
                ShowTips(ex.Message);
            }
        }

        private void LoginCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            if (LoginInfo.Instance.UserId <= 0) //登录失败
            {
                this.CloseLoading();
                ShowTips(LoginInfo.Instance.UserName);
                return;
            }
            LoginInfo.Instance.SaveHistoryAccount(); //登录成功后记住账号
            LoginInfo.Instance.RemberLanguage = Global.Language == null ? "" : Global.Language.Code;
            LoginInfo.Instance.SavePwd(this.checkBoxRemberPwd.Checked);
            //获取库存组织
            ApiHelper.PostAsync<List<InvOrg>>(this, "InvOrgController", "getinvorguser", OnGetInvorgUserCallback, LoginInfo.Instance.UserId.ToString());
            LanguageSettings.UpdateLanguageSettings();//更新语言配置
        }

        private void OnGetInvorgUserCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            if (!result.Success)
            {
                this.CloseLoading();
                ShowTips(result.Message);
                return;
            }
            LoginInfo.Instance.AllInvOrgs = result.Result as List<InvOrg>;
            //获取权限项目
            ApiHelper.PostAsync<List<MenuPermission>>(this, "RoleController", "GetWpfFirstPermissionByUserId", OnGetWpfFirstPermissionByUserIdCallback, LoginInfo.Instance.UserId.ToString());

        }

        private void OnGetWpfFirstPermissionByUserIdCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            this.CloseLoading();
            if (!result.Success)
            {
                ShowTips(result.Message);
                return;
            }
            LoginInfo.Instance.AllPermissions = result.Result as List<MenuPermission>;
            this.DialogResult = DialogResult.OK;
        }

        private void comboBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;

            if (this.comboBoxAccount.Items.Count > 0 && this.comboBoxAccount.SelectedIndex == this.comboBoxAccount.Items.Count - 1)
            {
                this.comboBoxAccount.Items.Clear();
                LoginInfo.Instance.CleanHistoryAccount();
            }

            this.textBoxPwd.Text = "";
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Language = LanguageSettings.All[comboBoxLanguage.SelectedIndex];
            base.SetLanguage(this, this.Name);
            ResetPosition();
        }
        /// <summary>
        /// 对登录页设置默认的Tag 用于语言切换保留默认Key
        /// </summary>
        private void SetDefaultTagText(Control ctrl)
        {

            ctrl.Tag = ctrl.Text;
            if (ctrl is TextBox || ctrl is ComboBox)
            {
                return;
            }
            foreach (Control subControl in ctrl.Controls)
            {
                SetDefaultTagText(subControl); // 递归调用
            }
        }
    }
}
