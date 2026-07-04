using SIE.XPCJ.Common.Extensions;
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
    public partial class XPFormLoading : Form
    {
        public XPFormLoading()
        {
            InitializeComponent();
        }

        private static XPFormLoading _instance = null;

        public static void ShowMask(string text = "正在加载，请等待.......")
        {
            if (_instance == null)
                _instance = new XPFormLoading();

            if (text == "正在加载，请等待.......")
                _instance.labelMsg.Text = text.L10N();
            else
                _instance.labelMsg.Text = text;
            _instance.ShowDialog();
        }

        public static void CloseMask()
        {
            if (_instance == null)
                return;

            _instance.Close();
        }
    }
}
