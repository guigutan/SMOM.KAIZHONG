using SIE.XPCJ.Common;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
            LanguageHelper.SetLanguage(this);
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            this.textBoxAttachUrl.Text = AppSettings.Instance.AttachUrl;
            this.textBoxApiUrl.Text = AppSettings.Instance.ApiUrl;
            this.textBoxBarTenderExePath.Text = AppSettings.Instance.BarTenderExePath;
            this.chkCollectionCulture.Checked = AppSettings.Instance.CollectionCulture;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.AttachUrl = this.textBoxAttachUrl.Text.Trim();
            AppSettings.Instance.ApiUrl = this.textBoxApiUrl.Text.Trim();
            AppSettings.Instance.BarTenderExePath = this.textBoxBarTenderExePath.Text.Trim();
            AppSettings.Instance.CollectionCulture = this.chkCollectionCulture.Checked;
            AppSettings.Instance.SaveToFile();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "选择BarTender打印程序".L10N();
            dialog.Filter = "BarTender程序文件|*.exe".L10N();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxBarTenderExePath.Text = dialog.FileName;
            }
        }
    }
}
