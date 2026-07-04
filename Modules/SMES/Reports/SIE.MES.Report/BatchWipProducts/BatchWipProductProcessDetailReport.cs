using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次采集工序明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次采集工序明细")]
    public partial class BatchWipProductProcessDetailReport : BatchWipProductProcessDetail
    {
        #region 工序记录 Process
        /// <summary>
        /// 工序记录Id
        /// </summary>
        [Label("工序记录")]
        public static new readonly IRefIdProperty ProductProcessIdProperty =
            P<BatchWipProductProcessDetailReport>.RegisterRefId(e => e.ProductProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序记录Id
        /// </summary>
        public new double ProductProcessId
        {
            get { return (double)this.GetRefId(ProductProcessIdProperty); }
            set { this.SetRefId(ProductProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序记录
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductReportProcess> ProductProcessProperty =
            P<BatchWipProductProcessDetailReport>.RegisterRef(e => e.ProductProcess, ProductProcessIdProperty);

        /// <summary>
        /// 工序记录
        /// </summary>
        public new BatchWipProductReportProcess ProductProcess
        {
            get { return this.GetRefEntity(ProductProcessProperty); }
            set { this.SetRefEntity(ProductProcessProperty, value); }
        }
        #endregion

        #region 关键件列表 KeyItemList
        /// <summary>
        /// 关键件列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchWipProductProcessKeyItemReport>> KeyItemListProperty = P<BatchWipProductProcessDetailReport>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 关键件列表
        /// </summary>
        public new EntityList<BatchWipProductProcessKeyItemReport> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion
    }
}
