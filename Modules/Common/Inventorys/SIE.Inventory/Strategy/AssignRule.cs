using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using SIE.Inventory.Onhands;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配规则
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AssignRuleCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [Label("分配规则")]
    [DisplayMember(nameof(AssignRule.Name))]
    public partial class AssignRule : DataEntity, IStateEntity
    {
        /// <summary>
        /// 默认标准规则
        /// </summary>
        public const string Default = "默认标准规则";

        #region 编号 Code
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("编号")]
        public static readonly Property<string> CodeProperty = P<AssignRule>.Register(e => e.Code);

        /// <summary>
        /// 编号
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
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<AssignRule>.Register(e => e.Name);

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
        [MaxLength(1000)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<AssignRule>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 散料优先 BulkType
        /// <summary>
        /// 散料优先
        /// </summary>
        [Label("散料优先")]
        public static readonly Property<BulkType?> BulkTypeProperty = P<AssignRule>.Register(e => e.BulkType);

        /// <summary>
        /// 散料优先
        /// </summary>
        public BulkType? BulkType
        {
            get { return GetProperty(BulkTypeProperty); }
            set { SetProperty(BulkTypeProperty, value); }
        }
        #endregion

        #region 分配规则明细列表 AssignRuleDetailList
        /// <summary>
        /// 分配规则明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<AssignRuleDetail>> AssignRuleDetailListProperty = P<AssignRule>.RegisterList(e => e.AssignRuleDetailList);

        /// <summary>
        /// 分配规则明细列表
        /// </summary>
        public EntityList<AssignRuleDetail> AssignRuleDetailList
        {
            get { return this.GetLazyList(AssignRuleDetailListProperty); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<AssignRule>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<bool> IsDefaultProperty = P<AssignRule>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<AssignRule>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType? OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<AssignRule>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)GetRefNullableId(CustomerIdProperty); }
            set { SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<AssignRule>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssignRule>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssignRule>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 子规则关系 RelationType
        /// <summary>
        /// 子规则关系
        /// </summary>
        [Label("子规则关系")]
        public static readonly Property<RelationType> RelationTypeProperty = P<AssignRule>.Register(e => e.RelationType);

        /// <summary>
        /// 子规则关系
        /// </summary>
        public RelationType RelationType
        {
            get { return this.GetProperty(RelationTypeProperty); }
            set { this.SetProperty(RelationTypeProperty, value); }
        }
        #endregion

        #region 分配库存状态限制 OnhandState
        /// <summary>
        /// 分配库存状态限制
        /// </summary>
        [Label("分配库存状态限制")]
        public static readonly Property<OnhandState?> OnhandStateProperty = P<AssignRule>.Register(e => e.OnhandState);

        /// <summary>
        /// 分配库存状态限制
        /// </summary>
        public OnhandState? OnhandState
        {
            get { return this.GetProperty(OnhandStateProperty); }
            set { this.SetProperty(OnhandStateProperty, value); }
        }
        #endregion

        #region 批次数上限 LotCountUL
        /// <summary>
        /// 批次数上限
        /// </summary>
        [MinValue(1)]
        [Label("批次数上限")]
        public static readonly Property<int?> LotCountULProperty = P<AssignRule>.Register(e => e.LotCountUL);

        /// <summary>
        /// 批次数上限
        /// </summary>
        public int? LotCountUL
        {
            get { return GetProperty(LotCountULProperty); }
            set { SetProperty(LotCountULProperty, value); }
        }
        #endregion

        #region 生产批次上限 ProductBatchUL
        /// <summary>
        /// 生产批次上限
        /// </summary>
        [MinValue(1)]
        [Label("生产批次上限")]
        public static readonly Property<int?> ProductBatchULProperty = P<AssignRule>.Register(e => e.ProductBatchUL);

        /// <summary>
        /// 生产批次上限
        /// </summary>
        public int? ProductBatchUL
        {
            get { return GetProperty(ProductBatchULProperty); }
            set { SetProperty(ProductBatchULProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 分配规则 实体配置
    /// </summary>
    internal class AssignRuleConfig : EntityConfig<AssignRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ASSIGN_RULE").MapAllProperties();
            Meta.Property(AssignRule.CodeProperty).ColumnMeta.HasLength(40);
            Meta.Property(AssignRule.NameProperty).ColumnMeta.HasLength(40);
            Meta.Property(AssignRule.DescriptionProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}