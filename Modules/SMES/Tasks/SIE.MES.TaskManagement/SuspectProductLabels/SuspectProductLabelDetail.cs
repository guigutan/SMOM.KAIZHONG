using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("可疑品标签明细")]
    [DisplayMember(nameof(SubBatchNo))]
    public class SuspectProductLabelDetail : DataEntity
    {
        #region 可疑品子标签 SubBatchNo
        /// <summary>
        /// 可疑品子标签
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("可疑品子标签")]
        public static readonly Property<string> SubBatchNoProperty = P<SuspectProductLabelDetail>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 可疑品子标签
        /// </summary>
        public string SubBatchNo
        {
            get { return this.GetProperty(SubBatchNoProperty); }
            set { this.SetProperty(SubBatchNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SuspectProductLabelDetail>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 判定结果 SuspectJudgeResult
        /// <summary>
        /// 判定结果
        /// </summary>
        [Label("判定结果")]
        public static readonly Property<SuspectJudgeResult> SuspectJudgeResultProperty = P<SuspectProductLabelDetail>.Register(e => e.SuspectJudgeResult);

        /// <summary>
        /// 判定结果
        /// </summary>
        public SuspectJudgeResult SuspectJudgeResult
        {
            get { return this.GetProperty(SuspectJudgeResultProperty); }
            set { this.SetProperty(SuspectJudgeResultProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<SuspectProductLabelDetail>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

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
            P<SuspectProductLabelDetail>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 可疑品标签 SuspectProductLabel
        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        [Label("可疑品标签")]
        public static readonly IRefIdProperty SuspectProductLabelIdProperty =
            P<SuspectProductLabelDetail>.RegisterRefId(e => e.SuspectProductLabelId, ReferenceType.Parent);

        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        public double SuspectProductLabelId
        {
            get { return (double)this.GetRefId(SuspectProductLabelIdProperty); }
            set { this.SetRefId(SuspectProductLabelIdProperty, value); }
        }

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public static readonly RefEntityProperty<SuspectProductLabel> SuspectProductLabelProperty =
            P<SuspectProductLabelDetail>.RegisterRef(e => e.SuspectProductLabel, SuspectProductLabelIdProperty);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public SuspectProductLabel SuspectProductLabel
        {
            get { return this.GetRefEntity(SuspectProductLabelProperty); }
            set { this.SetRefEntity(SuspectProductLabelProperty, value); }
        }
        #endregion

        #region 处理人 HandleBy
        /// <summary>
        /// 处理人Id
        /// </summary>
        [Label("处理人")]
        public static readonly IRefIdProperty HandleByIdProperty =
            P<SuspectProductLabelDetail>.RegisterRefId(e => e.HandleById, ReferenceType.Normal);

        /// <summary>
        /// 处理人Id
        /// </summary>
        public double HandleById
        {
            get { return (double)this.GetRefId(HandleByIdProperty); }
            set { this.SetRefId(HandleByIdProperty, value); }
        }

        /// <summary>
        /// 处理人
        /// </summary>
        public static readonly RefEntityProperty<Employee> HandleByProperty =
            P<SuspectProductLabelDetail>.RegisterRef(e => e.HandleBy, HandleByIdProperty);

        /// <summary>
        /// 处理人
        /// </summary>
        public Employee HandleBy
        {
            get { return this.GetRefEntity(HandleByProperty); }
            set { this.SetRefEntity(HandleByProperty, value); }
        }
        #endregion

        #region 处理时间 HandleDate
        /// <summary>
        /// 处理时间
        /// </summary>
        [Label("处理时间")]
        public static readonly Property<DateTime> HandleDateProperty = P<SuspectProductLabelDetail>.Register(e => e.HandleDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandleDate
        {
            get { return this.GetProperty(HandleDateProperty); }
            set { this.SetProperty(HandleDateProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 可疑品标签明细 实体配置
    /// </summary>
    internal class SuspectProductLabelDetailEntityConfig : EntityConfig<SuspectProductLabelDetail>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_SUSPECT_PROD_LABEL_DTL").MapAllProperties();
            Meta.Property(SuspectProductLabelDetail.SubBatchNoProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
