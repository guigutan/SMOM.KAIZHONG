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

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 周转规则
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [EntityWithConfig(typeof(NoConfig))]
    [DisplayMember(nameof(Name))]
    [Label("周转规则")]
    public partial class TurnOverRule : DataEntity, IStateEntity
    {
        /// <summary>
        /// 默认标准规则
        /// </summary>
        public const string Default = "默认标准规则";

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<TurnOverRule>.Register(e => e.Code);

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
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<TurnOverRule>.Register(e => e.Name);

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
        public static readonly Property<string> DescriptionProperty = P<TurnOverRule>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 周转规则明细列表 DetailList
        /// <summary>
        /// 周转规则明细列表
        /// </summary>
        [Label("明细")]
        public static readonly ListProperty<EntityList<TurnOverRuleDetail>> DetailListProperty = P<TurnOverRule>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 周转规则明细列表
        /// </summary>
        public EntityList<TurnOverRuleDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<TurnOverRule>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<bool> IsDefaultProperty = P<TurnOverRule>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 允许拣货超失效期库存 CanPickOverDue
        /// <summary>
        /// 允许拣货超失效期库存
        /// </summary>
        [Label("允许拣货超失效期库存")]
        public static readonly Property<bool> CanPickOverDueProperty = P<TurnOverRule>.Register(e => e.CanPickOverDue);

        /// <summary>
        /// 允许拣货超失效期库存
        /// </summary>
        public bool CanPickOverDue
        {
            get { return GetProperty(CanPickOverDueProperty); }
            set { SetProperty(CanPickOverDueProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<TurnOverRule>.Register(e => e.OrderType);

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
        public static readonly IRefIdProperty CustomerIdProperty = P<TurnOverRule>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<TurnOverRule>.RegisterRef(e => e.Customer, CustomerIdProperty);

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
        public static readonly IRefIdProperty WarehouseIdProperty = P<TurnOverRule>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<TurnOverRule>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 周转规则 实体配置
    /// </summary>
    internal class TurnOverRuleConfig : EntityConfig<TurnOverRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TURN_OVER_RULE").MapAllProperties();
            Meta.Property(TurnOverRule.CodeProperty).ColumnMeta.HasLength(40);
            Meta.Property(TurnOverRule.NameProperty).ColumnMeta.HasLength(40);
            Meta.Property(TurnOverRule.DescriptionProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}