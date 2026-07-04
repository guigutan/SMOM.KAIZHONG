using SIE.XPCJ.Common.ApiCall;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormChangeInvOrg : XPFormBaseDialog
    {
        public XPFormChangeInvOrg()
        {
            InitializeComponent();
        }

        private void FormChangeInvOrg_Load(object sender, EventArgs e)
        {
            this.xpListBox1.DataSource = LoginInfo.Instance.AllInvOrgs;
        }

        private void xpDialogTitle1_AOkClick(object sender, EventArgs e)
        {
            InvOrg newOrg = LoginInfo.Instance.AllInvOrgs[xpListBox1.SelectedIndex];
            ApiHelper.PostAsync<string>(this, "UserController", "changeinvorg", OnChangeInvOrgCallback, LoginInfo.Instance.UserId, newOrg.Code);
            this.ShowLoading();
        }
        
        private void OnChangeInvOrgCallback<T>(ApiResult<T> result, string apiType, string method, string postData)
        {
            this.CloseLoading();
            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }

            InvOrg newOrg = LoginInfo.Instance.AllInvOrgs[xpListBox1.SelectedIndex];
            LoginInfo.Instance.InvOrgId = newOrg.Id;

            this.DialogResult = DialogResult.OK;
        }
    }
}
