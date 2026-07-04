using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 条码工序指派员工信息
    /// </summary>
    [ChildEntity, Serializable]
    [Label("条码工序指派员工信息")]
    public partial class WipProductProcessReportEmployee : WipProProcessEmployee
    {
        #region 生产采集 WipProductProcess
        /// <summary>
        /// 生产采集Id
        /// </summary>
        [Label("生产采集")]
        public static new readonly IRefIdProperty WipProductProcessIdProperty =
            P<WipProductProcessReportEmployee>.RegisterRefId(e => e.WipProductProcessId, ReferenceType.Parent);

        /// <summary>
        /// 生产采集Id
        /// </summary>
        public new double WipProductProcessId
        {
            get { return (double)this.GetRefId(WipProductProcessIdProperty); }
            set { this.SetRefId(WipProductProcessIdProperty, value); }
        }

        /// <summary>
        /// 生产采集
        /// </summary>
        public static new readonly RefEntityProperty<WipProductReportProcess> WipProductProcessProperty =
            P<WipProductProcessReportEmployee>.RegisterRef(e => e.WipProductProcess, WipProductProcessIdProperty);

        /// <summary>
        /// 生产采集
        /// </summary>
        public new WipProductReportProcess WipProductProcess
        {
            get { return this.GetRefEntity(WipProductProcessProperty); }
            set { this.SetRefEntity(WipProductProcessProperty, value); }
        }
        #endregion

    }
}
