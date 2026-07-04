using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次采集记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次采集记录")]
    public partial class BatchWipProductReportProcess : BatchWipProductProcess
    {
        #region 版本 Version
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static new readonly IRefIdProperty VersionIdProperty = P<BatchWipProductReportProcess>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 版本Id
        /// </summary>
        public new double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductVersionReport> VersionProperty = P<BatchWipProductReportProcess>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 版本
        /// </summary>
        public new BatchWipProductVersionReport Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 明细列表 DetailList
        /// <summary>
        /// 批次采集工序明细列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchWipProductProcessDetailReport>> DetailListProperty = P<BatchWipProductReportProcess>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 批次采集工序明细列表
        /// </summary>
        public new EntityList<BatchWipProductProcessDetailReport> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion
    }
}
