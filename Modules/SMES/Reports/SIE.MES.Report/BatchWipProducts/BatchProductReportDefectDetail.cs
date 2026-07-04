using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 产品缺陷记录明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷记录明细")]
    public partial class BatchProductReportDefectDetail : BatchWipProductDefectDetail
    {
        #region 产品缺陷记录 BatchWipProductDefect
        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        [Label("产品缺陷记录")]
        public static new readonly IRefIdProperty BatchWipProductDefectIdProperty =
            P<BatchProductReportDefectDetail>.RegisterRefId(e => e.BatchWipProductDefectId, ReferenceType.Parent);

        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public new double BatchWipProductDefectId
        {
            get { return (double)this.GetRefId(BatchWipProductDefectIdProperty); }
            set { this.SetRefId(BatchWipProductDefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductReportDefect> BatchWipProductDefectProperty =
            P<BatchProductReportDefectDetail>.RegisterRef(e => e.BatchWipProductDefect, BatchWipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public new BatchWipProductReportDefect BatchWipProductDefect
        {
            get { return this.GetRefEntity(BatchWipProductDefectProperty); }
            set { this.SetRefEntity(BatchWipProductDefectProperty, value); }
        }
        #endregion
    }
}
