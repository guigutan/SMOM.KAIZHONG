using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 产品缺陷责任
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷责任")]
    public partial class BatchDefectReportResponsibility : BatchWipDefectResponsibility
    {
        #region 责任列表 Defect
        /// <summary>
        /// 责任列表Id
        /// </summary>
        public static new readonly IRefIdProperty DefectIdProperty = P<BatchDefectReportResponsibility>.RegisterRefId(e => e.DefectId, ReferenceType.Parent);

        /// <summary>
        /// 责任列表Id
        /// </summary>
        public new double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 责任列表
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductReportDefect> DefectProperty = P<BatchDefectReportResponsibility>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 责任列表
        /// </summary>
        public new BatchWipProductReportDefect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion
    }
}
