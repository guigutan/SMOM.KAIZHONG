using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls.TreeGrid
{
    public class ControlHelper
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        [DllImport("user32.dll")]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        #region 冻结控件
        /// <summary>
        /// The m LST freeze control
        /// </summary>
        static Dictionary<Control, bool> m_lstFreezeControl = new Dictionary<Control, bool>();
        /// <summary>
        /// 功能描述:停止更新控件
        /// 作　　者:HZH
        /// 创建日期:2019-07-13 11:11:32
        /// 任务编号:POS
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="blnToFreeze">是否停止更新</param>
        public static void FreezeControl(Control control, bool blnToFreeze)
        {
            if (blnToFreeze && control.IsHandleCreated && control.Visible && !control.IsDisposed && (!m_lstFreezeControl.ContainsKey(control) || (m_lstFreezeControl.ContainsKey(control) && m_lstFreezeControl[control] == false)))
            {
                m_lstFreezeControl[control] = true;
                control.Disposed += control_Disposed;
                SendMessage(control.Handle, 11, 0, 0);
            }
            else if (!blnToFreeze && !control.IsDisposed && m_lstFreezeControl.ContainsKey(control) && m_lstFreezeControl[control] == true)
            {
                m_lstFreezeControl.Remove(control);
                SendMessage(control.Handle, 11, 1, 0);
                control.Invalidate(true);
            }
        }

        /// <summary>
        /// Handles the Disposed event of the control control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        static void control_Disposed(object sender, EventArgs e)
        {
            try
            {
                if (m_lstFreezeControl.ContainsKey((Control)sender))
                    m_lstFreezeControl.Remove((Control)sender);
            }
            catch { }
        }
        #endregion
    }
}
