using SIE.XPCJ.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ
{
    public partial class FormQuitDialog : SIE.XPCJ.Common.Forms.XPFormBaseDialog
    {
        public FormQuitDialog()
        {
            InitializeComponent();
            this.xpButtonExit.Text = "退出赛意SMOM采集系统".L10N();
            this.xpButtonGoToLogin.Text = "切换账号".L10N();
            this.xpButtonCancel.Text = "取消".L10N();
        }

        private void xpButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xpButtonExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void xpButtonGoToLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
