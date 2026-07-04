using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 产品缺陷记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷记录")]
    public partial class BatchWipProductReportDefect : BatchWipProductDefect
    {
        #region 不良记录 Version
        /// <summary>
        /// 不良记录Id
        /// </summary>
        public static new readonly IRefIdProperty VersionIdProperty = P<BatchWipProductReportDefect>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 不良记录Id
        /// </summary>
        public new double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 不良记录
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductVersionReport> VersionProperty = P<BatchWipProductReportDefect>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 不良记录
        /// </summary>
        public new BatchWipProductVersionReport Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 责任列表 ResponsibilityList
        /// <summary>
        /// 责任列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchDefectReportResponsibility>> ResponsibilityListProperty = P<BatchWipProductReportDefect>.RegisterList(e => e.ResponsibilityList);

        /// <summary>
        /// 责任列表
        /// </summary>
        public new EntityList<BatchDefectReportResponsibility> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion

        #region 明细列表 DetailList
        /// <summary>
        /// 明细列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchProductReportDefectDetail>> DetailListProperty = P<BatchWipProductReportDefect>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 明细列表
        /// </summary>
        public new EntityList<BatchProductReportDefectDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 维修措施列表 MeasureList
        /// <summary>
        /// 维修措施列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchDefectMeasureReport>> MeasureListProperty = P<BatchWipProductReportDefect>.RegisterList(e => e.MeasureList);

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public new EntityList<BatchDefectMeasureReport> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion
    }
}
