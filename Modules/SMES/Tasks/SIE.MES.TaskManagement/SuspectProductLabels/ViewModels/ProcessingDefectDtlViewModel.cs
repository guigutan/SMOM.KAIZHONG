using SIE.Defects;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SuspectProductLabels.ViewModels
{
    /// <summary>
    /// 可疑品处理不良处理明细
    /// </summary>
    [RootEntity, Serializable]
    public class ProcessingDefectDtlViewModel : ViewModel
    {
        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<ProcessingDefectDtlViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<ProcessingDefectDtlViewModel>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double? DefectId
        {
            get { return (double?)this.GetRefNullableId(DefectIdProperty); }
            set { this.SetRefNullableId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty =
            P<ProcessingDefectDtlViewModel>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 判定结果 SuspectJudgeResult
        /// <summary>
        /// 判定结果
        /// </summary>
        [Label("判定结果")]
        public static readonly Property<ProcessingDefectDtlViewModelType?> SuspectJudgeResultProperty = P<ProcessingDefectDtlViewModel>.Register(e => e.SuspectJudgeResult);

        /// <summary>
        /// 判定结果
        /// </summary>
        public ProcessingDefectDtlViewModelType? SuspectJudgeResult
        {
            get { return this.GetProperty(SuspectJudgeResultProperty); }
            set { this.SetProperty(SuspectJudgeResultProperty, value); }
        }
        #endregion

    }

    public enum ProcessingDefectDtlViewModelType
    {
        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap,
        /// <summary>
        /// 返工
        /// </summary>
        [Label("返工")]
        Repair,
    }
}
