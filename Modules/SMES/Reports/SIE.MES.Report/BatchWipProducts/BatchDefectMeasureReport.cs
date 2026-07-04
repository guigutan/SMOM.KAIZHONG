using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 产品缺陷维修措施
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷维修措施")]
    public partial class BatchDefectMeasureReport : BatchWipDefectMeasure
    {
        #region 维修措施列表 Defect
        /// <summary>
        /// 维修措施列表Id
        /// </summary>
        public static new readonly IRefIdProperty DefectIdProperty = P<BatchDefectMeasureReport>.RegisterRefId(e => e.DefectId, ReferenceType.Parent);

        /// <summary>
        /// 维修措施列表Id
        /// </summary>
        public new double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductReportDefect> DefectProperty = P<BatchDefectMeasureReport>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public new BatchWipProductReportDefect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion
    }
}
