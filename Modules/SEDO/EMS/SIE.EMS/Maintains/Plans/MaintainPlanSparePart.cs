using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养计划备件更换
    /// </summary>
    [ChildEntity, Serializable]
    [Label("保养计划备件更换")]
    public partial class MaintainPlanSparePart : DataEntity
    {
        #region 保养计划 MaintainPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty MaintainPlanIdProperty =
            P<MaintainPlanSparePart>.RegisterRefId(e => e.MaintainPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double MaintainPlanId
        {
            get { return (double)this.GetRefId(MaintainPlanIdProperty); }
            set { this.SetRefId(MaintainPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainPlan> MaintainPlanProperty =
            P<MaintainPlanSparePart>.RegisterRef(e => e.MaintainPlan, MaintainPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainPlan MaintainPlan
        {
            get { return this.GetRefEntity(MaintainPlanProperty); }
            set { this.SetRefEntity(MaintainPlanProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<MaintainPlanSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<MaintainPlanSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ChangeSparePartState> StateProperty = P<MaintainPlanSparePart>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ChangeSparePartState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 备件出库单明细 PartOutDepotDetail
        /// <summary>
        /// 备件出库单明细Id
        /// </summary>
        [Label("备件出库单明细")]
        public static readonly IRefIdProperty PartOutDepotDetailIdProperty =
            P<MaintainPlanSparePart>.RegisterRefId(e => e.PartOutDepotDetailId, ReferenceType.Normal);

        /// <summary>
        /// 备件出库单明细Id
        /// </summary>
        public double? PartOutDepotDetailId
        {
            get { return (double?)this.GetRefNullableId(PartOutDepotDetailIdProperty); }
            set { this.SetRefNullableId(PartOutDepotDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件出库单明细
        /// </summary>
        public static readonly RefEntityProperty<PartOutDepotDetail> PartOutDepotDetailProperty =
            P<MaintainPlanSparePart>.RegisterRef(e => e.PartOutDepotDetail, PartOutDepotDetailIdProperty);

        /// <summary>
        /// 备件出库单明细
        /// </summary>
        public PartOutDepotDetail PartOutDepotDetail
        {
            get { return this.GetRefEntity(PartOutDepotDetailProperty); }
            set { this.SetRefEntity(PartOutDepotDetailProperty, value); }
        }
        #endregion

        #region 更换数量 ChangeQty
        /// <summary>
        /// 更换数量
        /// </summary>
        [Label("更换数量")]
        public static readonly Property<int> ChangeQtyProperty = P<MaintainPlanSparePart>.Register(e => e.ChangeQty);

        /// <summary>
        /// 更换数量
        /// </summary>
        public int ChangeQty
        {
            get { return this.GetProperty(ChangeQtyProperty); }
            set { this.SetProperty(ChangeQtyProperty, value); }
        }
        #endregion

        #region 新序列号 NewSequence
        ///// <summary>
        ///// 新序列号Id
        ///// </summary>
        //[Label("新序列号")]
        //public static readonly IRefIdProperty NewSequenceIdProperty =
        //    P<MaintainPlanSparePart>.RegisterRefId(e => e.NewSequenceId, ReferenceType.Normal);

        ///// <summary>
        ///// 新序列号Id
        ///// </summary>
        //public double? NewSequenceId
        //{
        //    get { return (double?)this.GetRefNullableId(NewSequenceIdProperty); }
        //    set { this.SetRefNullableId(NewSequenceIdProperty, value); }
        //}

        ///// <summary>
        ///// 新序列号    
        ///// </summary>
        //public static readonly RefEntityProperty<SerializeNo> NewSequenceProperty =
        //    P<MaintainPlanSparePart>.RegisterRef(e => e.NewSequence, NewSequenceIdProperty);

        ///// <summary>
        ///// 新序列号
        ///// </summary>
        //public SerializeNo NewSequence
        //{
        //    get { return this.GetRefEntity(NewSequenceProperty); }
        //    set { this.SetRefEntity(NewSequenceProperty, value); }
        //}
        #endregion

        #region 旧序列号 OldSequence
        /// <summary>
        /// 旧序列号
        /// </summary>
        [Label("旧序列号")]
        public static readonly Property<string> OldSequenceProperty = P<MaintainPlanSparePart>.Register(e => e.OldSequence);

        /// <summary>
        /// 旧序列号
        /// </summary>
        public string OldSequence
        {
            get { return this.GetProperty(OldSequenceProperty); }
            set { this.SetProperty(OldSequenceProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<MaintainPlanSparePart>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 非映射字段

        #region 剩余数量 RemainingQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<int?> RemainingQtyProperty = P<MaintainPlanSparePart>.Register(e => e.RemainingQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int? RemainingQty
        {
            get { return this.GetProperty(RemainingQtyProperty); }
            set { this.SetProperty(RemainingQtyProperty, value); }
        }
        #endregion

        #endregion

        #region 视图属性

        #region 备件编码 SparePartCodeView
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeViewProperty = P<MaintainPlanSparePart>.RegisterView(e => e.SparePartCodeView, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCodeView
        {
            get { return this.GetProperty(SparePartCodeViewProperty); }
        }
        #endregion

        #region 备件名称 SparePartNameView
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameViewProperty = P<MaintainPlanSparePart>.RegisterView(e => e.SparePartNameView, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartNameView
        {
            get { return this.GetProperty(SparePartNameViewProperty); }
        }
        #endregion

        #region 规格型号 SpecificationView
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationViewProperty = P<MaintainPlanSparePart>.RegisterView(e => e.SpecificationView, p => p.SparePart.Specification);

        /// <summary>
        /// 是否以旧换新
        /// </summary>
        public string SpecificationView
        {
            get { return this.GetProperty(SpecificationViewProperty); }
        }
        #endregion

        #region 单位 UnitView
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitViewProperty = P<MaintainPlanSparePart>.RegisterView(e => e.UnitView, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitView
        {
            get { return this.GetProperty(UnitViewProperty); }
        }
        #endregion

        #region 出库单号 OutDepotNoView
        /// <summary>
        /// 出库单号
        /// </summary>
        [Label("出库单号")]
        public static readonly Property<string> OutDepotNoViewProperty = P<MaintainPlanSparePart>.RegisterView(e => e.OutDepotNoView, p => p.PartOutDepotDetail.OutDepot.No);

        /// <summary>
        /// 出库单号
        /// </summary>
        public string OutDepotNoView
        {
            get { return this.GetProperty(OutDepotNoViewProperty); }
            set { this.SetProperty(OutDepotNoViewProperty, value); }
        }
        #endregion
        
        #region 序列号 SeriaNoView
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SeriaNoViewProperty = P<MaintainPlanSparePart>.RegisterView(e => e.SeriaNoView, p => p.PartOutDepotDetail.SeriaNoRef.OrderNumberCode);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNoView
        {
            get { return this.GetProperty(SeriaNoViewProperty); }
            set { this.SetProperty(SeriaNoViewProperty, value); }
        }
        #endregion

        #region 批次号 BatchNoView
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoViewProperty = P<MaintainPlanSparePart>.RegisterView(e => e.BatchNoView, p => p.PartOutDepotDetail.BatchNoRef.BatchNumber);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNoView
        {
            get { return this.GetProperty(BatchNoViewProperty); }
            set { this.SetProperty(BatchNoViewProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<MaintainPlanSparePart>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 出库数量 OutDepotCount
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> OutDepotCountProperty = P<MaintainPlanSparePart>.RegisterView(e => e.OutDepotCount, p => p.PartOutDepotDetail.OutDepotCount);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutDepotCount
        {
            get { return this.GetProperty(OutDepotCountProperty); }
        }
        #endregion

        #region 使用数量 UseCount
        /// <summary>
        /// 使用数量
        /// </summary>
        [Label("使用数量")]
        public static readonly Property<int> UseCountProperty = P<MaintainPlanSparePart>.RegisterView(e => e.UseCount, p => p.PartOutDepotDetail.UseCount);

        /// <summary>
        /// 使用数量
        /// </summary>
        public int UseCount
        {
            get { return this.GetProperty(UseCountProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 保养计划备件更换 实体配置
    /// </summary>
    internal class MaintainPlanSparePartConfig : EntityConfig<MaintainPlanSparePart>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(MaintainPlanSparePart.RemarkProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(MaintainPlanSparePart.ChangeQtyProperty, new PositiveNumberRule());
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_PLAN_SP_PART").MapAllProperties();
            Meta.Property(MaintainPlanSparePart.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(MaintainPlanSparePart.RemainingQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}