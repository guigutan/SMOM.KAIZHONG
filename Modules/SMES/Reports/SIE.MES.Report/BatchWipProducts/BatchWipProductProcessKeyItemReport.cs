using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 产品生产关键件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品生产关键件")]
    public partial class BatchWipProductProcessKeyItemReport : BatchWipProductProcessKeyItem
    {
        #region 过站采集记录明细 Detail
        /// <summary>
        /// 过站采集记录明细Id
        /// </summary>
        [Label("过站采集记录明细")]
        public static new readonly IRefIdProperty DetailIdProperty = P<BatchWipProductProcessKeyItemReport>.RegisterRefId(e => e.DetailId, ReferenceType.Parent);

        /// <summary>
        /// 过站采集记录明细Id
        /// </summary>
        public new double DetailId
        {
            get { return (double)GetRefId(DetailIdProperty); }
            set { SetRefId(DetailIdProperty, value); }
        }

        /// <summary>
        /// 过站采集记录明细
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipReportRecord> DetailProperty = P<BatchWipProductProcessKeyItemReport>.RegisterRef(e => e.Detail, DetailIdProperty);

        /// <summary>
        /// 过站采集记录明细
        /// </summary>
        public new BatchWipReportRecord Detail
        {
            get { return GetRefEntity(DetailProperty); }
            set { SetRefEntity(DetailProperty, value); }
        }
        #endregion
    }
}
