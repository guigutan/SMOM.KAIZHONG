using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.WIP;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 生产批次版本
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产批次版本")]
    [CriteriaQuery(typeof(BatchReportCriteriaProvider))]
    public partial class BatchWipProductVersionReport : BatchWipProductVersion
    {
        #region 不良记录 DefectList
        /// <summary>
        /// 不良记录
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchWipProductReportDefect>> DefectListProperty = P<BatchWipProductVersionReport>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 不良记录
        /// </summary>
        public new EntityList<BatchWipProductReportDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 采集记录列表 ProcessList
        /// <summary>
        /// 采集记录列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchWipProductReportProcess>> ProcessListProperty = P<BatchWipProductVersionReport>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 采集记录列表
        /// </summary>
        public new EntityList<BatchWipProductReportProcess> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 采集记录(新) BatchWipRecordList
        /// <summary>
        /// 采集记录(新)
        /// </summary>
        [Label("采集记录")]
        public static new readonly ListProperty<EntityList<BatchWipReportRecord>> BatchWipRecordListProperty = P<BatchWipProductVersionReport>.RegisterList(e => e.BatchWipRecordList);

        /// <summary>
        /// 采集记录(新)
        /// </summary>
        public new EntityList<BatchWipReportRecord> BatchWipRecordList
        {
            get { return this.GetLazyList(BatchWipRecordListProperty); }
        }
        #endregion

        #region 维修记录列表 RepaireList
        /// <summary>
        /// 维修记录列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchWipProductReportRepaire>> RepaireListProperty = P<BatchWipProductVersionReport>.RegisterList(e => e.RepaireList);

        /// <summary>
        /// 维修记录列表
        /// </summary>
        public new EntityList<BatchWipProductReportRepaire> RepaireList
        {
            get { return this.GetLazyList(RepaireListProperty); }
        }
        #endregion

        #region 批次合并拆分记录 BatchSplitMergeList
        /// <summary>
        /// 批次合并拆分记录
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchWipProductReportSplitMerge>> BatchSplitMergeListProperty = P<BatchWipProductVersionReport>.RegisterList(e => e.BatchSplitMergeList);

        /// <summary>
        /// 批次合并拆分记录
        /// </summary>
        public new EntityList<BatchWipProductReportSplitMerge> BatchSplitMergeList
        {
            get { return this.GetLazyList(BatchSplitMergeListProperty); }
        }
        #endregion
    }
}
