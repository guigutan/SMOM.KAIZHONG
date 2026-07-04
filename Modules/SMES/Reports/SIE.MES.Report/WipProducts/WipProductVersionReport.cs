using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 生产产品版本
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产产品版本")]
    [ConditionQueryType(typeof(WipProductReportCriteria))]
    public partial class WipProductVersionReport : WipProductVersion
    {
        #region 检验记录 InspectionItemList
        /// <summary>
        /// 检验记录
        /// </summary>
        public static new readonly ListProperty<EntityList<WipProductReportInspectionItem>> InspectionItemListProperty = P<WipProductVersionReport>.RegisterList(e => e.InspectionItemList);

        /// <summary>
        /// 检验记录
        /// </summary>
        public new EntityList<WipProductReportInspectionItem> InspectionItemList
        {
            get { return this.GetLazyList(InspectionItemListProperty); }
        }
        #endregion

        #region 采集记录 ProcessList
        /// <summary>
        /// 采集记录
        /// </summary>
        public static new readonly ListProperty<EntityList<WipProductReportProcess>> ProcessListProperty = P<WipProductVersionReport>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 采集记录
        /// </summary>
        public new EntityList<WipProductReportProcess> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 维修记录 RepaireList
        /// <summary>
        /// 维修记录
        /// </summary>
        public static new readonly ListProperty<EntityList<WipProductReportRepair>> RepaireListProperty = P<WipProductVersionReport>.RegisterList(e => e.RepaireList);

        /// <summary>
        /// 维修记录
        /// </summary>
        public new EntityList<WipProductReportRepair> RepaireList
        {
            get { return this.GetLazyList(RepaireListProperty); }
        }
        #endregion

        #region 不良记录 DefectList
        /// <summary>
        /// 不良记录
        /// </summary>
        public static new readonly ListProperty<EntityList<WipProductReportDefect>> DefectListProperty = P<WipProductVersionReport>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 不良记录
        /// </summary>
        public new EntityList<WipProductReportDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion
    }
}
