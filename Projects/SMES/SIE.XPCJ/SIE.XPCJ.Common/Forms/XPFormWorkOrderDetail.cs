using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Models.WIP;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormWorkOrderDetail : XPFormBaseNoBorder
    {
        private WorkOrder CurWorkOrder { get; set; }

        /// <summary>
        /// 显示工作单元完全信息窗口
        /// </summary>
        /// <param name="resourceInfo">资源信息</param>
        /// <param name="processInfo">工序信息</param>
        /// <param name="stationInfo">工位信息</param>
        public static void ShowInfo(Control senderParentControl, Control senderControl, WorkOrder workOrder)
        {
            XPFormWorkOrderDetail frm = new XPFormWorkOrderDetail();

            frm.Width = senderControl.Width;

            var newScreenLocation = senderParentControl.PointToScreen(new System.Drawing.Point(senderControl.Location.X, senderControl.Location.Y + senderControl.Height - senderControl.Padding.Bottom));
            frm.CurWorkOrder = workOrder;
            frm.Location = newScreenLocation;
            frm.Show();
        }


        public XPFormWorkOrderDetail()
        {
            InitializeComponent();
            this.Load += XPFormWorkOrderDetail_Load;
        }

        private void XPFormWorkOrderDetail_Load(object sender, System.EventArgs e)
        {
            if (CurWorkOrder != null)
            {
                this.lbProduceModelName.Text = CurWorkOrder?.ProductModelName;
                this.lbProduceName.Text = CurWorkOrder?.ProductName;
                this.labelItemExtPropName.Text = CurWorkOrder?.ItemExtPropName;
            }
        }

        private void XPFormWorkOrderDetail_Deactivate(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
