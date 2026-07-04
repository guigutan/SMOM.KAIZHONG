using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品缺陷责任
    /// </summary>
    [ChildEntity, Serializable]
    [Label("缺陷责任")]
    public class WipReportDefectResponsibility : WipDefectResponsibility
    {
        #region 产品缺陷记录 WipProductDefect
        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        [Label("产品缺陷记录")]
        public static new readonly IRefIdProperty WipProductDefectIdProperty =
            P<WipReportDefectResponsibility>.RegisterRefId(e => e.WipProductDefectId, ReferenceType.Parent);

        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public new double WipProductDefectId
        {
            get { return (double)this.GetRefId(WipProductDefectIdProperty); }
            set { this.SetRefId(WipProductDefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public static new readonly RefEntityProperty<WipProductReportDefect> WipProductDefectProperty =
            P<WipReportDefectResponsibility>.RegisterRef(e => e.WipProductDefect, WipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public new WipProductReportDefect WipProductDefect
        {
            get { return this.GetRefEntity(WipProductDefectProperty); }
            set { this.SetRefEntity(WipProductDefectProperty, value); }
        }
        #endregion
    }
}
