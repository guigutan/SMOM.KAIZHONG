using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class XPButton : Button
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string PrivilegeName { get; set; }

        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool IsPrivilegeAllow { get; set; } = true;

        public Color ForeColorEnable { get; set; } = Color.White;
        public Color ForeColorDisable { get; set; } = Color.Silver;

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                this.ForeColor = value ? this.ForeColorEnable : this.ForeColorDisable;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public XPButton()
        {
            SetButton(this);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }

        private void SetButton(Button button)
        {
            MethodInfo methodinfo = button.GetType().GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            methodinfo.Invoke(button, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, new object[] { ControlStyles.Selectable, false }, Application.CurrentCulture);
        }

    }
}
