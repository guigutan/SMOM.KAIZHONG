using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Core.Enums;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using SIE.Warehouses;
using SIE.Warehouses.Stations;
using System;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 上架规则明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("上架规则明细")]
    public partial class OnShelvesRuleDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Required]
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<OnShelvesRuleDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 待上架物料批次属性1值 FromLotAtt01Value
        /// <summary>
        /// 待上架物料批次属性1值
        /// </summary>
        [Label("待上架物料批次属性1值")]
        public static readonly Property<string> FromLotAtt01ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt01Value);

        /// <summary>
        /// 待上架物料批次属性1值
        /// </summary>
        public string FromLotAtt01Value
        {
            get { return GetProperty(FromLotAtt01ValueProperty); }
            set { SetProperty(FromLotAtt01ValueProperty, value); }
        }
        #endregion

        #region 待上架物料批次属性2值 FromLotAtt02Value
        /// <summary>
        /// 待上架物料批次属性2值
        /// </summary>
        [Label("待上架物料批次属性2值")]
        public static readonly Property<string> FromLotAtt02ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt02Value);

        /// <summary>
        /// 待上架物料批次属性2值
        /// </summary>
        public string FromLotAtt02Value
        {
            get { return GetProperty(FromLotAtt02ValueProperty); }
            set { SetProperty(FromLotAtt02ValueProperty, value); }
        }
        #endregion

        #region 待上架物料批次属性3值 FromLotAtt03Value
        /// <summary>
        /// 待上架物料批次属性3值
        /// </summary>
        [Label("待上架物料批次属性3值")]
        public static readonly Property<string> FromLotAtt03ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt03Value);

        /// <summary>
        /// 待上架物料批次属性3值
        /// </summary>
        public string FromLotAtt03Value
        {
            get { return GetProperty(FromLotAtt03ValueProperty); }
            set { SetProperty(FromLotAtt03ValueProperty, value); }
        }
        #endregion

        #region 待上架物料批次属性4值 FromLotAtt04Value
        /// <summary>
        /// 待上架物料批次属性4值
        /// </summary>
        [Label("待上架物料批次属性4值")]
        public static readonly Property<string> FromLotAtt04ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt04Value);

        /// <summary>
        /// 待上架物料批次属性4值
        /// </summary>
        public string FromLotAtt04Value
        {
            get { return GetProperty(FromLotAtt04ValueProperty); }
            set { SetProperty(FromLotAtt04ValueProperty, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制1值 ToLotAtt01Value
        /// <summary>
        /// 目标库位库存批次属性限制1值
        /// </summary>
        [Label("目标库位库存批次属性限制1值")]
        public static readonly Property<string> ToLotAtt01ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt01Value);

        /// <summary>
        /// 目标库位库存批次属性限制1值
        /// </summary>
        public string ToLotAtt01Value
        {
            get { return GetProperty(ToLotAtt01ValueProperty); }
            set { SetProperty(ToLotAtt01ValueProperty, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制2值 ToLotAtt02Value
        /// <summary>
        /// 目标库位库存批次属性限制2值
        /// </summary>
        [Label("目标库位库存批次属性限制2值")]
        public static readonly Property<string> ToLotAtt02ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt02Value);

        /// <summary>
        /// 目标库位库存批次属性限制2值
        /// </summary>
        public string ToLotAtt02Value
        {
            get { return GetProperty(ToLotAtt02ValueProperty); }
            set { SetProperty(ToLotAtt02ValueProperty, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制3值 ToLotAtt03Value
        /// <summary>
        /// 目标库位库存批次属性限制3值
        /// </summary>
        [Label("目标库位库存批次属性限制3值")]
        public static readonly Property<string> ToLotAtt03ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt03Value);

        /// <summary>
        /// 目标库位库存批次属性限制3值
        /// </summary>
        public string ToLotAtt03Value
        {
            get { return GetProperty(ToLotAtt03ValueProperty); }
            set { SetProperty(ToLotAtt03ValueProperty, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制4值 ToLotAtt04Value
        /// <summary>
        /// 目标库位库存批次属性限制4值
        /// </summary>
        [Label("目标库位库存批次属性限制4值")]
        public static readonly Property<string> ToLotAtt04ValueProperty = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt04Value);

        /// <summary>
        /// 目标库位库存批次属性限制4值
        /// </summary>
        public string ToLotAtt04Value
        {
            get { return GetProperty(ToLotAtt04ValueProperty); }
            set { SetProperty(ToLotAtt04ValueProperty, value); }
        }
        #endregion

        #region 最大混放品总数 MaxItemNum
        /// <summary>
        /// 最大混放品总数
        /// </summary>
        [MinValue(1)]
        [Label("最大混放品总数")]
        public static readonly Property<int?> MaxItemNumProperty = P<OnShelvesRuleDetail>.Register(e => e.MaxItemNum);

        /// <summary>
        /// 最大混放品总数
        /// </summary>
        public int? MaxItemNum
        {
            get { return GetProperty(MaxItemNumProperty); }
            set { SetProperty(MaxItemNumProperty, value); }
        }
        #endregion

        #region 最大混放批次数 MaxLotNum
        /// <summary>
        /// 最大混放批次数
        /// </summary>
        [MinValue(1)]
        [Label("最大混放批次数")]
        public static readonly Property<int?> MaxLotNumProperty = P<OnShelvesRuleDetail>.Register(e => e.MaxLotNum);

        /// <summary>
        /// 最大混放批次数
        /// </summary>
        public int? MaxLotNum
        {
            get { return GetProperty(MaxLotNumProperty); }
            set { SetProperty(MaxLotNumProperty, value); }
        }
        #endregion

        #region 占库区最大库位数 MaxLocNum
        /// <summary>
        /// 占库区最大库位数
        /// </summary>
        [MinValue(1)]
        [Label("占库区最大库位数")]
        public static readonly Property<int?> MaxLocNumProperty = P<OnShelvesRuleDetail>.Register(e => e.MaxLocNum);

        /// <summary>
        /// 占库区最大库位数
        /// </summary>
        public int? MaxLocNum
        {
            get { return GetProperty(MaxLocNumProperty); }
            set { SetProperty(MaxLocNumProperty, value); }
        }
        #endregion

        #region 具有的物料库存 ExistsItem
        /// <summary>
        /// 具有的物料库存Id
        /// </summary>
        public static readonly IRefIdProperty ExistsItemIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.ExistsItemId, ReferenceType.Normal);

        /// <summary>
        /// 具有的物料库存Id
        /// </summary>
        public double? ExistsItemId
        {
            get { return (double?)GetRefNullableId(ExistsItemIdProperty); }
            set { SetRefNullableId(ExistsItemIdProperty, value); }
        }

        /// <summary>
        /// 具有的物料库存
        /// </summary>
        public static readonly RefEntityProperty<Item> ExistsItemProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.ExistsItem, ExistsItemIdProperty);

        /// <summary>
        /// 具有的物料库存
        /// </summary>
        public Item ExistsItem
        {
            get { return GetRefEntity(ExistsItemProperty); }
            set { SetRefEntity(ExistsItemProperty, value); }
        }
        #endregion

        #region 同物料库存  IsSameItemOnhand
        /// <summary>
        /// 同物料库存 
        /// </summary>
        [Label("同物料库存")]
        public static readonly Property<bool?> IsSameItemOnhandProperty = P<OnShelvesRuleDetail>.Register(e => e.IsSameItemOnhand);

        /// <summary>
        /// 同物料库存
        /// </summary>
        public bool? IsSameItemOnhand
        {
            get { return GetProperty(IsSameItemOnhandProperty); }
            set { SetProperty(IsSameItemOnhandProperty, value); }
        }
        #endregion

        #region 待上架物料批次属性1 FromLotAtt01
        /// <summary>
        /// 待上架物料批次属性1
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> FromLotAtt01Property = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt01);

        /// <summary>
        /// 待上架物料批次属性1
        /// </summary>
        public LotAttribute? FromLotAtt01
        {
            get { return GetProperty(FromLotAtt01Property); }
            set { SetProperty(FromLotAtt01Property, value); }
        }
        #endregion

        #region 待上架物料批次属性2 FromLotAtt02
        /// <summary>
        /// 待上架物料批次属性2
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> FromLotAtt02Property = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt02);

        /// <summary>
        /// 待上架物料批次属性2
        /// </summary>
        public LotAttribute? FromLotAtt02
        {
            get { return GetProperty(FromLotAtt02Property); }
            set { SetProperty(FromLotAtt02Property, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 储存限制条件1 StorageLimit1
        /// <summary>
        /// 储存限制条件1
        /// </summary>
        [Label("储存限制")]
        public static readonly Property<StorageLimit?> StorageLimit1Property = P<OnShelvesRuleDetail>.Register(e => e.StorageLimit1);

        /// <summary>
        /// 储存限制条件1
        /// </summary>
        public StorageLimit? StorageLimit1
        {
            get { return GetProperty(StorageLimit1Property); }
            set { SetProperty(StorageLimit1Property, value); }
        }
        #endregion

        #region 来源库位 FromLocation
        /// <summary>
        /// 来源库位Id
        /// </summary>
        [Label("来源库位")]
        public static readonly IRefIdProperty FromLocationIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.FromLocationId, ReferenceType.Normal);

        /// <summary>
        /// 来源库位Id
        /// </summary>
        public double? FromLocationId
        {
            get { return (double?)GetRefNullableId(FromLocationIdProperty); }
            set { SetRefNullableId(FromLocationIdProperty, value); }
        }

        /// <summary>
        /// 来源库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> FromLocationProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.FromLocation, FromLocationIdProperty);

        /// <summary>
        /// 来源库位
        /// </summary>
        public StorageLocation FromLocation
        {
            get { return GetRefEntity(FromLocationProperty); }
            set { SetRefEntity(FromLocationProperty, value); }
        }
        #endregion

        #region 目标库区 ToArea
        /// <summary>
        /// 目标库区Id
        /// </summary>
        [Label("目标库区")]
        public static readonly IRefIdProperty ToAreaIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.ToAreaId, ReferenceType.Normal);

        /// <summary>
        /// 目标库区Id
        /// </summary>
        public double? ToAreaId
        {
            get { return (double?)GetRefNullableId(ToAreaIdProperty); }
            set { SetRefNullableId(ToAreaIdProperty, value); }
        }

        /// <summary>
        /// 目标库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> ToAreaProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.ToArea, ToAreaIdProperty);

        /// <summary>
        /// 目标库区
        /// </summary>
        public StorageArea ToArea
        {
            get { return GetRefEntity(ToAreaProperty); }
            set { SetRefEntity(ToAreaProperty, value); }
        }
        #endregion

        #region 储存限制条件2 StorageLimit2
        /// <summary>
        /// 储存限制条件2
        /// </summary>
        [Label("储存限制")]
        public static readonly Property<StorageLimit?> StorageLimit2Property = P<OnShelvesRuleDetail>.Register(e => e.StorageLimit2);

        /// <summary>
        /// 储存限制条件2
        /// </summary>
        public StorageLimit? StorageLimit2
        {
            get { return GetProperty(StorageLimit2Property); }
            set { SetProperty(StorageLimit2Property, value); }
        }
        #endregion

        #region ABC分类 AbcType
        /// <summary>
        /// ABC分类
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<AbcType?> AbcTypeProperty = P<OnShelvesRuleDetail>.Register(e => e.AbcType);

        /// <summary>
        /// ABC分类
        /// </summary>
        public AbcType? AbcType
        {
            get { return GetProperty(AbcTypeProperty); }
            set { SetProperty(AbcTypeProperty, value); }
        }
        #endregion

        #region 储存限制条件3 StorageLimit3
        /// <summary>
        /// 储存限制条件3
        /// </summary>
        [Label("储存限制")]
        public static readonly Property<StorageLimit?> StorageLimit3Property = P<OnShelvesRuleDetail>.Register(e => e.StorageLimit3);

        /// <summary>
        /// 储存限制条件3
        /// </summary>
        public StorageLimit? StorageLimit3
        {
            get { return GetProperty(StorageLimit3Property); }
            set { SetProperty(StorageLimit3Property, value); }
        }
        #endregion

        #region 拣货处理 PickProcessType
        /// <summary>
        /// 拣货处理
        /// </summary>
        [Label("拣货处理类型")]
        public static readonly Property<PickProcessType?> PickProcessTypeProperty = P<OnShelvesRuleDetail>.Register(e => e.PickProcessType);

        /// <summary>
        /// 拣货处理
        /// </summary>
        public PickProcessType? PickProcessType
        {
            get { return GetProperty(PickProcessTypeProperty); }
            set { SetProperty(PickProcessTypeProperty, value); }
        }
        #endregion

        #region 库位形式 LocationType
        /// <summary>
        /// 库位形式
        /// </summary>
        [Label("库位形式")]
        public static readonly Property<string> LocationTypeProperty = P<OnShelvesRuleDetail>.Register(e => e.LocationType);

        /// <summary>
        /// 库位形式
        /// </summary>
        public string LocationType
        {
            get { return GetProperty(LocationTypeProperty); }
            set { SetProperty(LocationTypeProperty, value); }
        }
        #endregion

        #region 库位库存 LocationState
        /// <summary>
        /// 库位库存
        /// </summary>
        [Label("库位库存")]
        public static readonly Property<LocationState?> LocationStateProperty = P<OnShelvesRuleDetail>.Register(e => e.LocationState);

        /// <summary>
        /// 库位库存
        /// </summary>
        public LocationState? LocationState
        {
            get { return GetProperty(LocationStateProperty); }
            set { SetProperty(LocationStateProperty, value); }
        }
        #endregion

        #region 策略 Strategy
        /// <summary>
        /// 策略
        /// </summary>
        [Label("策略")]
        [Required]
        public static readonly Property<StrategyType?> StrategyProperty = P<OnShelvesRuleDetail>.Register(e => e.Strategy);

        /// <summary>
        /// 策略
        /// </summary>
        public StrategyType? Strategy
        {
            get { return GetProperty(StrategyProperty); }
            set { SetProperty(StrategyProperty, value); }
        }
        #endregion

        #region 上架处理 UpProcessType
        /// <summary>
        /// 上架处理
        /// </summary>
        [Label("上架处理类型")]
        public static readonly Property<UpProcessType?> UpProcessTypeProperty = P<OnShelvesRuleDetail>.Register(e => e.UpProcessType);

        /// <summary>
        /// 上架处理
        /// </summary>
        public UpProcessType? UpProcessType
        {
            get { return GetProperty(UpProcessTypeProperty); }
            set { SetProperty(UpProcessTypeProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<OnShelvesRuleDetail>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType? OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 货主 Shipper
        /// <summary>
        /// 货主Id
        /// </summary>
        [Label("货主")]
        public static readonly IRefIdProperty ShipperIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.ShipperId, ReferenceType.Normal);

        /// <summary>
        /// 货主Id
        /// </summary>
        public double? ShipperId
        {
            get { return (double?)GetRefNullableId(ShipperIdProperty); }
            set { SetRefNullableId(ShipperIdProperty, value); }
        }

        /// <summary>
        /// 货主
        /// </summary>
        public static readonly RefEntityProperty<Customer> ShipperProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.Shipper, ShipperIdProperty);

        /// <summary>
        /// 货主
        /// </summary>
        public Customer Shipper
        {
            get { return GetRefEntity(ShipperProperty); }
            set { SetRefEntity(ShipperProperty, value); }
        }
        #endregion

        #region 待上架物料批次属性3 FromLotAtt03
        /// <summary>
        /// 待上架物料批次属性3
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> FromLotAtt03Property = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt03);

        /// <summary>
        /// 待上架物料批次属性3
        /// </summary>
        public LotAttribute? FromLotAtt03
        {
            get { return GetProperty(FromLotAtt03Property); }
            set { SetProperty(FromLotAtt03Property, value); }
        }
        #endregion

        #region 空间限制条件3 SpaceLimit3
        /// <summary>
        /// 空间限制条件3
        /// </summary>
        [Label("空间限制")]
        public static readonly Property<SpaceLimit?> SpaceLimit3Property = P<OnShelvesRuleDetail>.Register(e => e.SpaceLimit3);

        /// <summary>
        /// 空间限制条件3
        /// </summary>
        public SpaceLimit? SpaceLimit3
        {
            get { return GetProperty(SpaceLimit3Property); }
            set { SetProperty(SpaceLimit3Property, value); }
        }
        #endregion

        #region 储存限制条件4 StorageLimit4
        /// <summary>
        /// 储存限制条件4
        /// </summary>
        [Label("储存限制")]
        public static readonly Property<StorageLimit?> StorageLimit4Property = P<OnShelvesRuleDetail>.Register(e => e.StorageLimit4);

        /// <summary>
        /// 储存限制条件4
        /// </summary>
        public StorageLimit? StorageLimit4
        {
            get { return GetProperty(StorageLimit4Property); }
            set { SetProperty(StorageLimit4Property, value); }
        }
        #endregion

        #region 单据小类 Transaction
        /// <summary>
        /// 单据小类Id
        /// </summary>
        [Label("单据小类")]
        public static readonly IRefIdProperty TransactionIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.TransactionId, ReferenceType.Normal);

        /// <summary>
		/// 单据小类Id
        /// </summary>
        public double? TransactionId
        {
            get { return (double?)GetRefNullableId(TransactionIdProperty); }
            set { SetRefNullableId(TransactionIdProperty, value); }
        }

        /// <summary>
		/// 单据小类
        /// </summary>
        public static readonly RefEntityProperty<Transaction> TransactionProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.Transaction, TransactionIdProperty);

        /// <summary>
		/// 单据小类
        /// </summary>
        public Transaction Transaction
        {
            get { return GetRefEntity(TransactionProperty); }
            set { SetRefEntity(TransactionProperty, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制1 ToLotAtt01
        /// <summary>
        /// 目标库位库存批次属性限制1
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> ToLotAtt01Property = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt01);

        /// <summary>
        /// 目标库位库存批次属性限制1
        /// </summary>
        public LotAttribute? ToLotAtt01
        {
            get { return GetProperty(ToLotAtt01Property); }
            set { SetProperty(ToLotAtt01Property, value); }
        }
        #endregion

        #region 空间限制条件4 SpaceLimit4
        /// <summary>
        /// 空间限制条件4
        /// </summary>
        [Label("空间限制")]
        public static readonly Property<SpaceLimit?> SpaceLimit4Property = P<OnShelvesRuleDetail>.Register(e => e.SpaceLimit4);

        /// <summary>
        /// 空间限制条件4
        /// </summary>
        public SpaceLimit? SpaceLimit4
        {
            get { return GetProperty(SpaceLimit4Property); }
            set { SetProperty(SpaceLimit4Property, value); }
        }
        #endregion

        #region 库存类别 ItemCategory
        /// <summary>
        /// 库存类别Id
        /// </summary>
        [Label("库存类别")]
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 库存类别Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)GetRefNullableId(ItemCategoryIdProperty); }
            set { SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 库存类别
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 库存类别
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制2 ToLotAtt02
        /// <summary>
        /// 目标库位库存批次属性限制2
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> ToLotAtt02Property = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt02);

        /// <summary>
        /// 目标库位库存批次属性限制2
        /// </summary>
        public LotAttribute? ToLotAtt02
        {
            get { return GetProperty(ToLotAtt02Property); }
            set { SetProperty(ToLotAtt02Property, value); }
        }
        #endregion

        #region 目标库位 ToLocation
        /// <summary>
        /// 目标库位Id
        /// </summary>
        [Label("目标库位")]
        public static readonly IRefIdProperty ToLocationIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.ToLocationId, ReferenceType.Normal);

        /// <summary>
        /// 目标库位Id
        /// </summary>
        public double? ToLocationId
        {
            get { return (double?)GetRefNullableId(ToLocationIdProperty); }
            set { SetRefNullableId(ToLocationIdProperty, value); }
        }

        /// <summary>
        /// 目标库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> ToLocationProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.ToLocation, ToLocationIdProperty);

        /// <summary>
        /// 目标库位
        /// </summary>
        public StorageLocation ToLocation
        {
            get { return GetRefEntity(ToLocationProperty); }
            set { SetRefEntity(ToLocationProperty, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制4 ToLotAtt04
        /// <summary>
        /// 目标库位库存批次属性限制4
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> ToLotAtt04Property = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt04);

        /// <summary>
        /// 目标库位库存批次属性限制4
        /// </summary>
        public LotAttribute? ToLotAtt04
        {
            get { return GetProperty(ToLotAtt04Property); }
            set { SetProperty(ToLotAtt04Property, value); }
        }
        #endregion

        #region 空间限制条件2 SpaceLimit2
        /// <summary>
        /// 空间限制条件2
        /// </summary>
        [Label("空间限制")]
        public static readonly Property<SpaceLimit?> SpaceLimit2Property = P<OnShelvesRuleDetail>.Register(e => e.SpaceLimit2);

        /// <summary>
        /// 空间限制条件2
        /// </summary>
        public SpaceLimit? SpaceLimit2
        {
            get { return GetProperty(SpaceLimit2Property); }
            set { SetProperty(SpaceLimit2Property, value); }
        }
        #endregion

        #region 目标库位库存批次属性限制3 ToLotAtt03
        /// <summary>
        /// 目标库位库存批次属性限制3
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> ToLotAtt03Property = P<OnShelvesRuleDetail>.Register(e => e.ToLotAtt03);

        /// <summary>
        /// 目标库位库存批次属性限制3
        /// </summary>
        public LotAttribute? ToLotAtt03
        {
            get { return GetProperty(ToLotAtt03Property); }
            set { SetProperty(ToLotAtt03Property, value); }
        }
        #endregion

        #region 待上架物料批次属性4 FromLotAtt04
        /// <summary>
        /// 待上架物料批次属性4
        /// </summary>
        [Label("")]
        public static readonly Property<LotAttribute?> FromLotAtt04Property = P<OnShelvesRuleDetail>.Register(e => e.FromLotAtt04);

        /// <summary>
        /// 待上架物料批次属性4
        /// </summary>
        public LotAttribute? FromLotAtt04
        {
            get { return GetProperty(FromLotAtt04Property); }
            set { SetProperty(FromLotAtt04Property, value); }
        }
        #endregion

        #region 空间限制条件1 SpaceLimit1
        /// <summary>
        /// 空间限制条件1
        /// </summary>
        [Label("空间限制")]
        public static readonly Property<SpaceLimit?> SpaceLimit1Property = P<OnShelvesRuleDetail>.Register(e => e.SpaceLimit1);

        /// <summary>
        /// 空间限制条件1
        /// </summary>
        public SpaceLimit? SpaceLimit1
        {
            get { return GetProperty(SpaceLimit1Property); }
            set { SetProperty(SpaceLimit1Property, value); }
        }
        #endregion

        #region 上架规则 OnShelvesRule
        /// <summary>
        /// 上架规则Id
        /// </summary>
        [Label("上架规则")]
        public static readonly IRefIdProperty OnShelvesRuleIdProperty = P<OnShelvesRuleDetail>.RegisterRefId(e => e.OnShelvesRuleId, ReferenceType.Parent);

        /// <summary>
        /// 上架规则Id
        /// </summary>
        public double OnShelvesRuleId
        {
            get { return (double)GetRefId(OnShelvesRuleIdProperty); }
            set { SetRefId(OnShelvesRuleIdProperty, value); }
        }

        /// <summary>
        /// 上架规则
        /// </summary>
        public static readonly RefEntityProperty<OnShelvesRule> OnShelvesRuleProperty = P<OnShelvesRuleDetail>.RegisterRef(e => e.OnShelvesRule, OnShelvesRuleIdProperty);

        /// <summary>
        /// 上架规则
        /// </summary>
        public OnShelvesRule OnShelvesRule
        {
            get { return GetRefEntity(OnShelvesRuleProperty); }
            set { SetRefEntity(OnShelvesRuleProperty, value); }
        }
        #endregion

        #region 应用场景 SceneType
        /// <summary>
        /// 应用场景
        /// </summary>
        [Label("应用场景")]
        public static readonly Property<SceneType> SceneTypeProperty = P<OnShelvesRuleDetail>.Register(e => e.SceneType);

        /// <summary>
        /// 应用场景
        /// </summary>
        public SceneType SceneType
        {
            get { return this.GetProperty(SceneTypeProperty); }
            set { this.SetProperty(SceneTypeProperty, value); }
        }
        #endregion

        #region 物料的库存类别 FromItemCategory
        /// <summary>
        /// 物料的库存类别Id
        /// </summary>
        [Label("物料的库存类别")]
        public static readonly IRefIdProperty FromItemCategoryIdProperty =
            P<OnShelvesRuleDetail>.RegisterRefId(e => e.FromItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 物料的库存类别Id
        /// </summary>
        public double? FromItemCategoryId
        {
            get { return (double?)this.GetRefNullableId(FromItemCategoryIdProperty); }
            set { this.SetRefNullableId(FromItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 物料的库存类别
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> FromItemCategoryProperty =
            P<OnShelvesRuleDetail>.RegisterRef(e => e.FromItemCategory, FromItemCategoryIdProperty);

        /// <summary>
        /// 物料的库存类别
        /// </summary>
        public ItemCategory FromItemCategory
        {
            get { return this.GetRefEntity(FromItemCategoryProperty); }
            set { this.SetRefEntity(FromItemCategoryProperty, value); }
        }
        #endregion

        #region 来源逻辑分区 FromLogicArea
        /// <summary>
        /// 来源逻辑分区Id
        /// </summary>
        [Label("来源逻辑分区")]
        public static readonly IRefIdProperty FromLogicAreaIdProperty =
            P<OnShelvesRuleDetail>.RegisterRefId(e => e.FromLogicAreaId, ReferenceType.Normal);

        /// <summary>
        /// 来源逻辑分区Id
        /// </summary>
        public double? FromLogicAreaId
        {
            get { return (double?)this.GetRefNullableId(FromLogicAreaIdProperty); }
            set { this.SetRefNullableId(FromLogicAreaIdProperty, value); }
        }

        /// <summary>
        /// 来源逻辑分区
        /// </summary>
        public static readonly RefEntityProperty<LogicArea> FromLogicAreaProperty =
            P<OnShelvesRuleDetail>.RegisterRef(e => e.FromLogicArea, FromLogicAreaIdProperty);

        /// <summary>
        /// 来源逻辑分区
        /// </summary>
        public LogicArea FromLogicArea
        {
            get { return this.GetRefEntity(FromLogicAreaProperty); }
            set { this.SetRefEntity(FromLogicAreaProperty, value); }
        }
        #endregion

        #region 目标逻辑分区 ToLogicArea
        /// <summary>
        /// 目标逻辑分区Id
        /// </summary>
        [Label("目标逻辑分区")]
        public static readonly IRefIdProperty ToLogicAreaIdProperty =
            P<OnShelvesRuleDetail>.RegisterRefId(e => e.ToLogicAreaId, ReferenceType.Normal);

        /// <summary>
        /// 目标逻辑分区Id
        /// </summary>
        public double? ToLogicAreaId
        {
            get { return (double?)this.GetRefNullableId(ToLogicAreaIdProperty); }
            set { this.SetRefNullableId(ToLogicAreaIdProperty, value); }
        }

        /// <summary>
        /// 目标逻辑分区
        /// </summary>
        public static readonly RefEntityProperty<LogicArea> ToLogicAreaProperty =
            P<OnShelvesRuleDetail>.RegisterRef(e => e.ToLogicArea, ToLogicAreaIdProperty);

        /// <summary>
        /// 目标逻辑分区
        /// </summary>
        public LogicArea ToLogicArea
        {
            get { return this.GetRefEntity(ToLogicAreaProperty); }
            set { this.SetRefEntity(ToLogicAreaProperty, value); }
        }
        #endregion

        #region 来源站台 FromStation
        /// <summary>
        /// 来源站台Id
        /// </summary>
        [Label("来源站台")]
        public static readonly IRefIdProperty FromStationIdProperty =
            P<OnShelvesRuleDetail>.RegisterRefId(e => e.FromStationId, ReferenceType.Normal);

        /// <summary>
        /// 来源站台Id
        /// </summary>
        public double? FromStationId
        {
            get { return (double?)this.GetRefNullableId(FromStationIdProperty); }
            set { this.SetRefNullableId(FromStationIdProperty, value); }
        }

        /// <summary>
        /// 来源站台
        /// </summary>
        public static readonly RefEntityProperty<Station> FromStationProperty =
            P<OnShelvesRuleDetail>.RegisterRef(e => e.FromStation, FromStationIdProperty);

        /// <summary>
        /// 来源站台
        /// </summary>
        public Station FromStation
        {
            get { return this.GetRefEntity(FromStationProperty); }
            set { this.SetRefEntity(FromStationProperty, value); }
        }
        #endregion

        #region 目标站台 ToStation
        /// <summary>
        /// 目标站台Id
        /// </summary>
        [Label("目标站台")]
        public static readonly IRefIdProperty ToStationIdProperty =
            P<OnShelvesRuleDetail>.RegisterRefId(e => e.ToStationId, ReferenceType.Normal);

        /// <summary>
        /// 目标站台Id
        /// </summary>
        public double? ToStationId
        {
            get { return (double?)this.GetRefNullableId(ToStationIdProperty); }
            set { this.SetRefNullableId(ToStationIdProperty, value); }
        }

        /// <summary>
        /// 目标站台
        /// </summary>
        public static readonly RefEntityProperty<Station> ToStationProperty =
            P<OnShelvesRuleDetail>.RegisterRef(e => e.ToStation, ToStationIdProperty);

        /// <summary>
        /// 目标站台
        /// </summary>
        public Station ToStation
        {
            get { return this.GetRefEntity(ToStationProperty); }
            set { this.SetRefEntity(ToStationProperty, value); }
        }
        #endregion

        #region 目标站台组 ToStationGroup
        /// <summary>
        /// 目标站台组Id
        /// </summary>
        [Label("目标站台组")]
        public static readonly IRefIdProperty ToStationGroupIdProperty =
            P<OnShelvesRuleDetail>.RegisterRefId(e => e.ToStationGroupId, ReferenceType.Normal);

        /// <summary>
        /// 目标站台组Id
        /// </summary>
        public double? ToStationGroupId
        {
            get { return (double?)this.GetRefNullableId(ToStationGroupIdProperty); }
            set { this.SetRefNullableId(ToStationGroupIdProperty, value); }
        }

        /// <summary>
        /// 目标站台组
        /// </summary>
        public static readonly RefEntityProperty<StationGroup> ToStationGroupProperty =
            P<OnShelvesRuleDetail>.RegisterRef(e => e.ToStationGroup, ToStationGroupIdProperty);

        /// <summary>
        /// 目标站台组
        /// </summary>
        public StationGroup ToStationGroup
        {
            get { return this.GetRefEntity(ToStationGroupProperty); }
            set { this.SetRefEntity(ToStationGroupProperty, value); }
        }
        #endregion

        #region 托盘型号 TrunoverBoxModel
        /// <summary>
        /// 托盘型号Id
        /// </summary>
        [Label("托盘型号")]
        public static readonly IRefIdProperty TrunoverBoxModelIdProperty =
            P<OnShelvesRuleDetail>.RegisterRefId(e => e.TrunoverBoxModelId, ReferenceType.Normal);

        /// <summary>
        /// 托盘型号Id
        /// </summary>
        public double? TrunoverBoxModelId
        {
            get { return (double?)this.GetRefNullableId(TrunoverBoxModelIdProperty); }
            set { this.SetRefNullableId(TrunoverBoxModelIdProperty, value); }
        }

        /// <summary>
        /// 托盘型号
        /// </summary>
        public static readonly RefEntityProperty<TurnoverBoxModel> TrunoverBoxModelProperty =
            P<OnShelvesRuleDetail>.RegisterRef(e => e.TrunoverBoxModel, TrunoverBoxModelIdProperty);

        /// <summary>
        /// 托盘型号
        /// </summary>
        public TurnoverBoxModel TrunoverBoxModel
        {
            get { return this.GetRefEntity(TrunoverBoxModelProperty); }
            set { this.SetRefEntity(TrunoverBoxModelProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 货主名称 ShipperName
        /// <summary>
        /// 货主名称
        /// </summary>
        [Label("货主名称")]
        public static readonly Property<string> ShipperNameProperty = P<OnShelvesRuleDetail>.RegisterView(e => e.ShipperName, e => e.Shipper.Name);

        /// <summary>
        /// 货主名称
        /// </summary>
        public string ShipperName
        {
            get { return this.GetProperty(ShipperNameProperty); }
        }
        #endregion 
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.NewValue != e.OldValue)
            {
                if (e.Property == OnShelvesRuleDetail.StrategyProperty)
                {
                    if (Strategy != null && Strategy.HasValue)
                    {
                        if (Strategy.Value != StrategyType.Strategy01 && Strategy.Value != StrategyType.Strategy02)
                        {
                            FromLocationId = null;
                            FromLocation = null;
                        }

                        if (Strategy.Value != StrategyType.Strategy02 && Strategy.Value != StrategyType.Strategy03)
                        {
                            ToAreaId = null;
                            ToArea = null;
                        }

                        if (Strategy.Value != StrategyType.Strategy01 && Strategy.Value != StrategyType.Strategy04)
                        {
                            ToLocationId = null;
                            ToLocation = null;
                        }
                    }
                    else
                    {
                        FromLocationId = null;
                        FromLocation = null;
                        ToAreaId = null;
                        ToArea = null;
                        ToLocationId = null;
                        ToLocation = null;
                    }
                }

                if (e.Property == OnShelvesRuleDetail.OrderTypeProperty)
                {
                    TransactionId = null;
                    Transaction = null;
                }
            }
        }
    }

    /// <summary>
    /// 上架规则明细 实体配置
    /// </summary>
    internal class OnShelvesRuleDetailConfig : EntityConfig<OnShelvesRuleDetail>
    {
        /// <summary>
        /// 增加验证逻辑
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    OnShelvesRuleDetail.OnShelvesRuleIdProperty,
                    OnShelvesRuleDetail.LineNoProperty,
                },
                MessageBuilder = o =>
                {
                    return "已经存在相同的行号".L10N();
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ON_SHELVES_RULE_DTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableSort();
        }
    }
}