using SIE.Common;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Inventory.Interfaces;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 单据大类
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FunctionCriteria))]
    [Label("单据大类")]
    [DisplayMember(nameof(Name))]
    public partial class Function : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public Function()
        {
            State = State.Enable;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Function>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Function>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<Function>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 数据来源类型 SourceType
        /// <summary>
        /// 数据来源类型
        /// </summary>
        [Label("数据来源类型")]
        public static readonly Property<SourceType> SourceTypeProperty = P<Function>.Register(e => e.SourceType);

        /// <summary>
        /// 数据来源类型
        /// </summary>
        public SourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Function>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否IQC报检 IsQc
        /// <summary>
        /// 是否IQC报检报检
        /// </summary>
        [Label("是否IQC报检")]
        public static readonly Property<bool> IsQcProperty = P<Function>.Register(e => e.IsQc);

        /// <summary>
        /// 是否IQC报检
        /// </summary>
        public bool IsQc
        {
            get { return this.GetProperty(IsQcProperty); }
            set { this.SetProperty(IsQcProperty, value); }
        }
        #endregion

        #region 是否OQC报检 IsOqc
        /// <summary>
        /// 是否OQC报检
        /// </summary>
        [Label("是否OQC报检")]
        public static readonly Property<bool> IsOqcProperty = P<Function>.Register(e => e.IsOqc);

        /// <summary>
        /// 是否OQC报检
        /// </summary>
        public bool IsOqc
        {
            get { return GetProperty(IsOqcProperty); }
            set { SetProperty(IsOqcProperty, value); }
        }
        #endregion

        #region 允许超发 IsAllowOut
        /// <summary>
        /// 允许超发
        /// </summary>
        [Label("允许超发")]
        public static readonly Property<bool> IsAllowOutProperty = P<Function>.Register(e => e.IsAllowOut);

        /// <summary>
        /// 允许超发
        /// </summary>
        public bool IsAllowOut
        {
            get { return GetProperty(IsAllowOutProperty); }
            set { SetProperty(IsAllowOutProperty, value); }
        }
        #endregion

        #region 拣货后即发货 IsAutoDelivery
        /// <summary>
        /// 拣货后即发货
        /// </summary>
        [Label("拣货后即发货")]
        public static readonly Property<bool> IsAutoDeliveryProperty = P<Function>.Register(e => e.IsAutoDelivery);

        /// <summary>
        /// 拣货后即发货
        /// </summary>
        public bool IsAutoDelivery
        {
            get { return GetProperty(IsAutoDeliveryProperty); }
            set { SetProperty(IsAutoDeliveryProperty, value); }
        }
        #endregion

        #region 越库自动拣货 IsCrossPick
        /// <summary>
        /// 越库自动拣货
        /// </summary>
        [Label("越库自动拣货")]
        public static readonly Property<bool> IsCrossPickProperty = P<Function>.Register(e => e.IsCrossPick);

        /// <summary>
        /// 越库自动拣货
        /// </summary>
        public bool IsCrossPick
        {
            get { return this.GetProperty(IsCrossPickProperty); }
            set { this.SetProperty(IsCrossPickProperty, value); }
        }
        #endregion

        #region 收货到不合格 IsReceiveNg
        /// <summary>
        /// 收货到不合格
        /// </summary>
        [Label("收货到不合格")]
        public static readonly Property<bool> IsReceiveNgProperty = P<Function>.Register(e => e.IsReceiveNg);

        /// <summary>
        /// 收货到不合格
        /// </summary>
        public bool IsReceiveNg
        {
            get { return this.GetProperty(IsReceiveNgProperty); }
            set { this.SetProperty(IsReceiveNgProperty, value); }
        }
        #endregion

        #region 部分发货 IsPartDelivery
        /// <summary>
        /// 部分发货
        /// </summary>
        [Label("部分发货")]
        public static readonly Property<bool> IsPartDeliveryProperty = P<Function>.Register(e => e.IsPartDelivery);

        /// <summary>
        /// 部分发货
        /// </summary>
        public bool IsPartDelivery
        {
            get { return this.GetProperty(IsPartDeliveryProperty); }
            set { this.SetProperty(IsPartDeliveryProperty, value); }
        }
        #endregion

        #region 单据大类 OrderType
        /// <summary>
        /// 单据大类
        /// </summary>
        [Label("单据大类")]
        public static readonly Property<OrderType> OrderTypeProperty = P<Function>.Register(e => e.OrderType);

        /// <summary>
        /// 单据大类
        /// </summary>
        public OrderType OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<Function>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<Function>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 按包装分配 IsPickByPackage
        /// <summary>
        /// 按包装分配
        /// </summary>
        [Label("按包装分配")]
        public static readonly Property<bool> IsPickByPackageProperty = P<Function>.Register(e => e.IsPickByPackage);

        /// <summary>
        /// 按包装分配
        /// </summary>
        public bool IsPickByPackage
        {
            get { return this.GetProperty(IsPickByPackageProperty); }
            set { this.SetProperty(IsPickByPackageProperty, value); }
        }
        #endregion

        #region 打印模板 BillTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty BillTemplateIdProperty =
            P<Function>.RegisterRefId(e => e.BillTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? BillTemplateId
        {
            get { return (double?)this.GetRefNullableId(BillTemplateIdProperty); }
            set { this.SetRefNullableId(BillTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> BillTemplateProperty =
            P<Function>.RegisterRef(e => e.BillTemplate, BillTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate BillTemplate
        {
            get { return this.GetRefEntity(BillTemplateProperty); }
            set { this.SetRefEntity(BillTemplateProperty, value); }
        }
        #endregion

        #region 超发上限% OutUpLimitMultiple
        /// <summary>
        /// 超发上限%
        /// </summary>
        [Label("超发上限%")]
        [MinValue(0)]
        public static readonly Property<decimal?> OutUpLimitMultipleProperty = P<Function>.Register(e => e.OutUpLimitMultiple);

        /// <summary>
        /// 超发上限%
        /// </summary>
        public decimal? OutUpLimitMultiple
        {
            get { return GetProperty(OutUpLimitMultipleProperty); }
            set { SetProperty(OutUpLimitMultipleProperty, value); }
        }
        #endregion

        #region 超发数量上限 MaxOutUpLimit
        /// <summary>
        /// 超发数量上限
        /// </summary>
        [Label("超发数量上限")]
        [MinValue(0)]
        public static readonly Property<decimal?> MaxOutUpLimitProperty = P<Function>.Register(e => e.MaxOutUpLimit);

        /// <summary>
        /// 超发数量上限
        /// </summary>
        public decimal? MaxOutUpLimit
        {
            get { return GetProperty(MaxOutUpLimitProperty); }
            set { SetProperty(MaxOutUpLimitProperty, value); }
        }
        #endregion
         
        #region 送货明细收货 IsCollectByDelivery
        /// <summary>
        /// 送货明细收货
        /// </summary>
        [Label("按送货明细收货")]
        public static readonly Property<bool> IsCollectByDeliveryProperty = P<Function>.Register(e => e.IsCollectByDelivery);

        /// <summary>
        /// 送货明细收货
        /// </summary>
        public bool IsCollectByDelivery
        {
            get { return this.GetProperty(IsCollectByDeliveryProperty); }
            set { this.SetProperty(IsCollectByDeliveryProperty, value); }
        }
        #endregion

        #region 单据大类与员工关系 EmployeeList
        /// <summary>
        /// 单据大类与员工关系
        /// </summary>
        public static readonly ListProperty<EntityList<FunctionEmployee>> EmployeeListProperty = P<Function>.RegisterList(e => e.EmployeeList);

        /// <summary>
        /// 单据大类与员工关系
        /// </summary>
        public EntityList<FunctionEmployee> EmployeeList
        {
            get { return this.GetLazyList(EmployeeListProperty); }
        }
        #endregion

        #region 类别 FunctionType
        /// <summary>
        /// 类别
        /// </summary>
        [Label("类别")]
        public static readonly Property<FunctionType?> FunctionTypeProperty = P<Function>.Register(e => e.FunctionType);

        /// <summary>
        /// 类别
        /// </summary>
        public FunctionType? FunctionType
        {
            get { return this.GetProperty(FunctionTypeProperty); }
            set { this.SetProperty(FunctionTypeProperty, value); }
        }
        #endregion
       
        #region 直接调拨 IsDirectAllocate
        /// <summary>
        /// 直接调拨
        /// </summary>
        [Label("直接调拨")]
        public static readonly Property<bool> IsDirectAllocateProperty = P<Function>.Register(e => e.IsDirectAllocate);

        /// <summary>
        /// 直接调拨
        /// </summary>
        public bool IsDirectAllocate
        {
            get { return this.GetProperty(IsDirectAllocateProperty); }
            set { this.SetProperty(IsDirectAllocateProperty, value); }
        }
        #endregion

        #region 两步调拨 IsTwoAllocate
        /// <summary>
        /// 两步调拨
        /// </summary>
        [Label("两步调拨")]
        public static readonly Property<bool> IsTwoAllocateProperty = P<Function>.Register(e => e.IsTwoAllocate);

        /// <summary>
        /// 两步调拨
        /// </summary>
        public bool IsTwoAllocate
        {
            get { return this.GetProperty(IsTwoAllocateProperty); }
            set { this.SetProperty(IsTwoAllocateProperty, value); }
        }
        #endregion

        #region 跨组织调拨 IsAcrossAllocate
        /// <summary>
        /// 跨组织调拨
        /// </summary>
        [Label("跨组织调拨")]
        public static readonly Property<bool> IsAcrossAllocateProperty = P<Function>.Register(e => e.IsAcrossAllocate);

        /// <summary>
        /// 跨组织调拨
        /// </summary>
        public bool IsAcrossAllocate
        {
            get { return this.GetProperty(IsAcrossAllocateProperty); }
            set { this.SetProperty(IsAcrossAllocateProperty, value); }
        }
        #endregion

        #region 调拨导入序列号 AllocateSn
        /// <summary>
        /// 调拨导入序列号
        /// </summary>
        [Label("调拨导入序列号")]
        public static readonly Property<bool> AllocateSnProperty = P<Function>.Register(e => e.AllocateSn);

        /// <summary>
        /// 调拨导入序列号
        /// </summary>
        public bool AllocateSn
        {
            get { return this.GetProperty(AllocateSnProperty); }
            set { this.SetProperty(AllocateSnProperty, value); }
        }
        #endregion

        #region 不启用送货明细,实收数<预期数时 ConfigCollectType
        /// <summary>
        /// 不启用送货明细,实收数<预期数时
        /// </summary>
        [Label("收货明细实收数<预期数时")]
        public static readonly Property<CollectAutoGenerateType> ConfigCollectTypeProperty = P<Function>.Register(e => e.ConfigCollectType);

        /// <summary>
        /// 不启用送货明细,实收数<预期数时
        /// </summary>
        public CollectAutoGenerateType ConfigCollectType
        {
            get { return this.GetProperty(ConfigCollectTypeProperty); }
            set { this.SetProperty(ConfigCollectTypeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 单据大类 实体配置
    /// </summary>
    internal class FunctionConfig : EntityConfig<Function>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TRANS_FUNCTION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}