using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品缺陷记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷记录")]
    public partial class WipProductReportDefect : WipProductDefect
    {
        #region 产品版本 Version
        /// <summary>
        /// 产品版本
        /// </summary>
        [Label("产品版本")]
        public static new readonly IRefIdProperty VersionIdProperty = P<WipProductReportDefect>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 产品版本
        /// </summary>
        public new double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 产品版本
        /// </summary>
        public static new readonly RefEntityProperty<WipProductVersionReport> VersionProperty = P<WipProductReportDefect>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 产品版本
        /// </summary>
        public new WipProductVersionReport Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 缺陷责任 ResponsibilityList
        /// <summary>
        /// 缺陷责任
        /// </summary>
        [Label("缺陷责任")]
        public static new readonly ListProperty<EntityList<WipReportDefectResponsibility>> ResponsibilityListProperty = P<WipProductReportDefect>.RegisterList(e => e.ResponsibilityList);

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public new EntityList<WipReportDefectResponsibility> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion

        #region 维修措施 MeasureList
        /// <summary>
        /// 维修措施
        /// </summary>
        [Label("维修措施")]
        public static new readonly ListProperty<EntityList<WipReportDefectMeasure>> MeasureListProperty = P<WipProductReportDefect>.RegisterList(e => e.MeasureList);

        /// <summary>
        /// 维修措施
        /// </summary>
        public new EntityList<WipReportDefectMeasure> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion
    }
}
