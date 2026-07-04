using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品维修记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品维修换料记录")]
    public partial  class WipProductReportRepairReplacceRecord: WipProductRepairReplaceRecord
    {
        #region 产品维修记录 WipProductRepair
        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        [Label("产品维修记录")]
        public static new readonly IRefIdProperty WipProductRepairIdProperty =
            P<WipProductReportRepairReplacceRecord>.RegisterRefId(e => e.WipProductRepairId, ReferenceType.Parent);

        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        public new double WipProductRepairId
        {
            get { return (double)this.GetRefId(WipProductRepairIdProperty); }
            set { this.SetRefId(WipProductRepairIdProperty, value); }
        }

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public static new readonly RefEntityProperty<WipProductReportRepair> WipProductRepairProperty =
            P<WipProductReportRepairReplacceRecord>.RegisterRef(e => e.WipProductRepair, WipProductRepairIdProperty);

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public new WipProductReportRepair WipProductRepair
        {
            get { return this.GetRefEntity(WipProductRepairProperty); }
            set { this.SetRefEntity(WipProductRepairProperty, value); }
        }
        #endregion
    }
}
