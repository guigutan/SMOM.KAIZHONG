using SIE.XPCJ.Models.WIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormWokerCellDetai : XPFormBaseDialog
    {
        public XPFormWokerCellDetai()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 显示工作单元完全信息窗口
        /// </summary>
        /// <param name="resourceInfo">资源信息</param>
        /// <param name="processInfo">工序信息</param>
        /// <param name="stationInfo">工位信息</param>
        public static void ShowInfo(Control senderParentControl, Control senderControl, ResourceInfo resourceInfo, ProcessInfo processInfo, StationInfo stationInfo)
        {
            XPFormWokerCellDetai frm = new XPFormWokerCellDetai();
            frm.lbResouce.Text = resourceInfo == null ? string.Empty : resourceInfo.Name;
            frm.lbProcess.Text = processInfo == null ? string.Empty : processInfo.Name;
            frm.lbStation.Text = stationInfo == null ? string.Empty : stationInfo.Name;
            frm.Width = senderControl.Width;

            var newScreenLocation = senderParentControl.PointToScreen(new System.Drawing.Point(senderControl.Location.X, senderControl.Location.Y + senderControl.Height - senderControl.Padding.Bottom));

            frm.Location = newScreenLocation;
            frm.Show();
        }

        private void XPWokerCellDetaiForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
