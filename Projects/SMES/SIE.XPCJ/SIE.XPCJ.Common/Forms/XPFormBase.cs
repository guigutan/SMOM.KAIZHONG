using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormBase : Form
    {
        public XPFormBase()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// 隐藏Loading框
        /// </summary>
        public void CloseLoading()
        {
            XPFormLoading.CloseMask();
        }

        /// <summary>
        /// 显示Loading框
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="isLoading"></param>
        /// <param name="tipText"></param>
        public void ShowLoading()
        {
            XPFormLoading.ShowMask("正在执行，请耐心等待......".L10N());
        }



        /// <summary>
        /// 记录其他控件的Enable状态
        /// </summary>
        private Dictionary<Control, bool> dicControlEnableState;
        /// <summary>
        /// 禁用全部控件，并记录控件的Enabled状态
        /// </summary>
        /// <param name="parentControl"></param>
        protected void DisabledAllControl(Control parentControl)
        {
            if (dicControlEnableState == null)
                dicControlEnableState = new Dictionary<Control, bool>();

            foreach (Control ctrl in parentControl.Controls)
            {
                dicControlEnableState[ctrl] = ctrl.Enabled;
                ctrl.Enabled = false;
                DisabledAllControl(ctrl);
            }
        }
        /// <summary>
        /// 还原全部控件的Enabled状态
        /// </summary>
        protected void EnabledAllControl()
        {
            if (dicControlEnableState == null)
                return;

            foreach (Control ctrl in dicControlEnableState.Keys)
            {
                ctrl.Enabled = dicControlEnableState[ctrl];
            }
            dicControlEnableState.Clear();
        }

        /// <summary>
        /// 递归调用
        /// </summary>
        /// <param name="control"></param>
        public void SetLanguage(Control control, string formName = "")
        {
            LanguageHelper.SetLanguage(control, formName);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetLanguage(this, this.Name);
        }

    }

}
