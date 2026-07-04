using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPDialogTitle : UserControl
    {
        public XPDialogTitle()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public string ATitle
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }

        public Font ATileFont
        {
            get { return this.labelTitle.Font; }
            set { this.labelTitle.Font = value; }
        }

        #region 取消按钮事件
        /// <summary>
        /// 取消按钮事件
        /// </summary>
        public event EventHandler ACancelClick;

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (ACancelClick == null)
            {
                //默认事件
                if (this.ParentForm != null)
                    this.ParentForm.Close();
            }
            else
            {
                ACancelClick.Invoke(this, e);
            }
        }
        #endregion

        #region 确定按钮事件
        /// <summary>
        /// 确定按钮事件
        /// </summary>
        public event EventHandler AOkClick;

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (AOkClick == null)
            {
                //默认事件
            }
            else
            {
                AOkClick.Invoke(this, e);
            }
        }
        #endregion
    }
}
