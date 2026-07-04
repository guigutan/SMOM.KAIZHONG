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
    [Label("产品维修记录")]
    public partial class WipProductReportRepair : WipProductRepair
    {
        #region 维修记录 Version
        /// <summary>
        /// 维修记录Id
        /// </summary>
        [Label("维修记录")]
        public static new readonly IRefIdProperty VersionIdProperty = P<WipProductReportRepair>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 维修记录Id
        /// </summary>
        public new double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 维修记录
        /// </summary>
        public static new readonly RefEntityProperty<WipProductVersionReport> VersionProperty = P<WipProductReportRepair>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 维修记录
        /// </summary>
        public new WipProductVersionReport Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 产品维修记录缺陷列表 WipProductRepairDefectList
        /// <summary>
        /// 产品维修记录缺陷列表
        /// </summary>
        public static new  readonly ListProperty<EntityList<WipProductReportRepairDefect>> WipProductRepairDefectListProperty
            = P<WipProductReportRepair>.RegisterList(e => e.WipProductRepairDefectList);
        /// <summary>
        /// 产品维修记录缺陷列表
        /// </summary>
        public new EntityList<WipProductReportRepairDefect> WipProductRepairDefectList
        {
            get { return this.GetLazyList(WipProductRepairDefectListProperty); }
        }
        #endregion


        #region 产品维修记录换料列表 WipProductRepairDefectList
        /// <summary>
        /// 产品维修记录换料列表
        /// </summary>
        public static new readonly ListProperty<EntityList<WipProductReportRepairReplacceRecord>> WipProductRepairReplaceRecordListProperty
            = P<WipProductReportRepair>.RegisterList(e => e.WipProductRepairReplaceRecordList);
        /// <summary>
        /// 产品维修记录换料列表
        /// </summary>
        public new EntityList<WipProductReportRepairReplacceRecord> WipProductRepairReplaceRecordList
        {
            get { return this.GetLazyList(WipProductRepairReplaceRecordListProperty); }
        }
        #endregion
    }
}
