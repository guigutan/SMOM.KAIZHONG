using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 收发控制基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("收发控制")]
    public partial class BaseItemIoLimit : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseItemIoLimit()
        {
            //InUpLimitMultiple = 0;
            //MaxInUpLimit = 0;
            //OutUpLimitMultiple = 0;
            //MaxOutUpLimit = 0;
            MaxStockQty = 0;
        }

        #region 超收上限% InUpLimitMultiple
        /// <summary>
        /// 超收上限%
        /// </summary>
        [Label("超收上限%")]
        [MinValue(0)]
        public static readonly Property<decimal?> InUpLimitMultipleProperty = P<BaseItemIoLimit>.Register(e => e.InUpLimitMultiple);

        /// <summary>
        /// 超收上限%
        /// </summary>
        public decimal? InUpLimitMultiple
        {
            get { return GetProperty(InUpLimitMultipleProperty); }
            set { SetProperty(InUpLimitMultipleProperty, value); }
        }
        #endregion

        #region 超收数量上限 MaxInUpLimit
        /// <summary>
        /// 超收数量上限
        /// </summary>
        [Label("超收数量上限")]
        [MinValue(0)]
        public static readonly Property<decimal?> MaxInUpLimitProperty = P<BaseItemIoLimit>.Register(e => e.MaxInUpLimit);

        /// <summary>
        /// 超收数量上限
        /// </summary>
        public decimal? MaxInUpLimit
        {
            get { return GetProperty(MaxInUpLimitProperty); }
            set { SetProperty(MaxInUpLimitProperty, value); }
        }
        #endregion

        #region 超发上限% OutUpLimitMultiple
        /// <summary>
        /// 超发上限%
        /// </summary>
        [Label("超发上限%")]
        [MinValue(0)]
        public static readonly Property<decimal?> OutUpLimitMultipleProperty = P<BaseItemIoLimit>.Register(e => e.OutUpLimitMultiple);

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
        public static readonly Property<decimal?> MaxOutUpLimitProperty = P<BaseItemIoLimit>.Register(e => e.MaxOutUpLimit);

        /// <summary>
        /// 超发数量上限
        /// </summary>
        public decimal? MaxOutUpLimit
        {
            get { return GetProperty(MaxOutUpLimitProperty); }
            set { SetProperty(MaxOutUpLimitProperty, value); }
        }
        #endregion

        #region 采购提前交付天数上限 PoAdvanceUpLimit
        /// <summary>
        /// 采购提前交付天数上限
        /// </summary>
        [Label("采购提前交付天数上限")]
        [MinValue(0)]
        public static readonly Property<decimal?> PoAdvanceUpLimitProperty = P<BaseItemIoLimit>.Register(e => e.PoAdvanceUpLimit);

        /// <summary>
        /// 采购提前交付天数上限
        /// </summary>
        public decimal? PoAdvanceUpLimit
        {
            get { return GetProperty(PoAdvanceUpLimitProperty); }
            set { SetProperty(PoAdvanceUpLimitProperty, value); }
        }
        #endregion

        #region 采购延后交付天数上限 PoPostponeUpLimit
        /// <summary>
        /// 采购延后交付天数上限
        /// </summary>
        [Label("采购延后交付天数上限")]
        [MinValue(0)]
        public static readonly Property<decimal?> PoPostponeUpLimitProperty = P<BaseItemIoLimit>.Register(e => e.PoPostponeUpLimit);

        /// <summary>
        /// 采购延后交付天数上限
        /// </summary>
        public decimal? PoPostponeUpLimit
        {
            get { return GetProperty(PoPostponeUpLimitProperty); }
            set { SetProperty(PoPostponeUpLimitProperty, value); }
        }
        #endregion

        #region 收货剩余保质天数下限 ReSurplusLowerLimit
        /// <summary>
        /// 收货剩余保质天数下限
        /// </summary>
        [Label("收货剩余保质天数下限")]
        public static readonly Property<decimal?> ReSurplusLowerLimitProperty = P<BaseItemIoLimit>.Register(e => e.ReSurplusLowerLimit);

        /// <summary>
        /// 收货剩余保质天数下限
        /// </summary>
        public decimal? ReSurplusLowerLimit
        {
            get { return GetProperty(ReSurplusLowerLimitProperty); }
            set { SetProperty(ReSurplusLowerLimitProperty, value); }
        }
        #endregion

        #region 发货剩余保质天数下限 ShSurplusLowerLimit
        /// <summary>
        /// 发货剩余保质天数下限
        /// </summary>
        [Label("发货剩余保质天数下限")]
        public static readonly Property<decimal?> ShSurplusLowerLimitProperty = P<BaseItemIoLimit>.Register(e => e.ShSurplusLowerLimit);

        /// <summary>
        /// 发货剩余保质天数下限
        /// </summary>
        public decimal? ShSurplusLowerLimit
        {
            get { return GetProperty(ShSurplusLowerLimitProperty); }
            set { SetProperty(ShSurplusLowerLimitProperty, value); }
        }
        #endregion

        #region 最低存量 MinStockQty
        /// <summary>
        /// 最低存量
        /// </summary>
        [Label("最低存量")]
        [MinValue(0)]
        public static readonly Property<decimal?> MinStockQtyProperty = P<BaseItemIoLimit>.Register(e => e.MinStockQty);

        /// <summary>
        /// 最低存量
        /// </summary>
        public decimal? MinStockQty
        {
            get { return GetProperty(MinStockQtyProperty); }
            set { SetProperty(MinStockQtyProperty, value); }
        }
        #endregion

        #region 最高存量 MaxStockQty
        /// <summary>
        /// 最高存量
        /// </summary>
        [Label("最高存量")]
        [MinValue(0)]
        public static readonly Property<decimal?> MaxStockQtyProperty = P<BaseItemIoLimit>.Register(e => e.MaxStockQty);

        /// <summary>
        /// 最高存量
        /// </summary>
        public decimal? MaxStockQty
        {
            get { return GetProperty(MaxStockQtyProperty); }
            set { SetProperty(MaxStockQtyProperty, value); }
        }
        #endregion

        #region 安全库存量 SafetyStockQty
        /// <summary>
        /// 安全库存量
        /// </summary>
        [Label("安全库存量")]
        [MinValue(0)]
        public static readonly Property<decimal?> SafetyStockQtyProperty = P<BaseItemIoLimit>.Register(e => e.SafetyStockQty);

        /// <summary>
        /// 安全库存量
        /// </summary>
        public decimal? SafetyStockQty
        {
            get { return GetProperty(SafetyStockQtyProperty); }
            set { SetProperty(SafetyStockQtyProperty, value); }
        }
        #endregion

        #region 备注 Remarks
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarksProperty = P<BaseItemIoLimit>.Register(e => e.Remarks);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get { return GetProperty(RemarksProperty); }
            set { SetProperty(RemarksProperty, value); }
        }
        #endregion

        #region 默认上架库区 DefaultArea
        /// <summary>
        /// 默认上架库区Id
        /// </summary>
        [Label("默认上架库区")]
        public static readonly IRefIdProperty DefaultAreaIdProperty = P<BaseItemIoLimit>.RegisterRefId(e => e.DefaultAreaId, ReferenceType.Normal);

        /// <summary>
        /// 默认上架库区Id
        /// </summary>
        public double? DefaultAreaId
        {
            get { return (double?)GetRefNullableId(DefaultAreaIdProperty); }
            set { SetRefNullableId(DefaultAreaIdProperty, value); }
        }

        /// <summary>
        /// 默认上架库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> DefaultAreaProperty = P<BaseItemIoLimit>.RegisterRef(e => e.DefaultArea, DefaultAreaIdProperty);

        /// <summary>
        /// 默认上架库区
        /// </summary>
        public StorageArea DefaultArea
        {
            get { return GetRefEntity(DefaultAreaProperty); }
            set { SetRefEntity(DefaultAreaProperty, value); }
        }
        #endregion

        #region 默认上架库位 StorageLocation
        /// <summary>
        /// 默认上架库位Id
        /// </summary>
        [Label("默认上架库位")]
        public static readonly IRefIdProperty DefaultLocationIdProperty = P<BaseItemIoLimit>.RegisterRefId(e => e.DefaultLocationId, ReferenceType.Normal);

        /// <summary>
        /// 默认上架库位Id
        /// </summary>
        public double? DefaultLocationId
        {
            get { return (double?)GetRefNullableId(DefaultLocationIdProperty); }
            set { SetRefNullableId(DefaultLocationIdProperty, value); }
        }

        /// <summary>
        /// 默认上架库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> DefaultLocationProperty = P<BaseItemIoLimit>.RegisterRef(e => e.DefaultLocation, DefaultLocationIdProperty);

        /// <summary>
        /// 默认上架库位
        /// </summary>
        public StorageLocation DefaultLocation
        {
            get { return GetRefEntity(DefaultLocationProperty); }
            set { SetRefEntity(DefaultLocationProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<BaseItemIoLimit>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<BaseItemIoLimit>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<BaseItemIoLimit>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<BaseItemIoLimit>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<BaseItemIoLimit>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 备注
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<BaseItemIoLimit>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 补货

        #region 默认操作人 Employee
        /// <summary>
        /// 默认操作人Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<BaseItemIoLimit>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 默认操作人Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 默认操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<BaseItemIoLimit>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 默认操作人
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 是否补货 IsReplenish
        /// <summary>
        /// 是否补货
        /// </summary>
        [Label("是否补货")]
        public static readonly Property<bool> IsReplenishProperty = P<BaseItemIoLimit>.Register(e => e.IsReplenish);

        /// <summary>
        /// 是否补货
        /// </summary>
        public bool IsReplenish
        {
            get { return GetProperty(IsReplenishProperty); }
            set { SetProperty(IsReplenishProperty, value); }
        }
        #endregion

        #region 分配库存参与运算 IsAlotInvInOperation
        /// <summary>
        /// 分配库存参与运算
        /// </summary>
        [Label("分配库存参与运算")]
        public static readonly Property<bool> IsAlotInvInOperationProperty = P<BaseItemIoLimit>.Register(e => e.IsAlotInvInOperation);

        /// <summary>
        /// 分配库存参与运算
        /// </summary>
        public bool IsAlotInvInOperation
        {
            get { return GetProperty(IsAlotInvInOperationProperty); }
            set { SetProperty(IsAlotInvInOperationProperty, value); }
        }
        #endregion

        #endregion

        #region 注册视图

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BaseItemIoLimit>.RegisterView(e => e.ItemCode, e => e.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<BaseItemIoLimit>.RegisterView(e => e.ItemName, e => e.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpec
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecProperty = P<BaseItemIoLimit>.RegisterView(e => e.ItemSpec, e => e.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpec
        {
            get { return this.GetProperty(ItemSpecProperty); }
        }
        #endregion

        #region 基本分类 ItemType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType> ItemTypeProperty = P<BaseItemIoLimit>.RegisterView(e => e.ItemType, e => e.Item.Type);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
        }
        #endregion

        #region 物料是否扩展 ItemEnableExtendProperty
        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public static readonly Property<bool> ItemEnableExtendPropertyProperty = P<BaseItemIoLimit>.RegisterView(e => e.ItemEnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public bool ItemEnableExtendProperty
        {
            get { return this.GetProperty(ItemEnableExtendPropertyProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<BaseItemIoLimit>.RegisterView(e => e.UnitName, e => e.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 收发控制 实体配置
    /// </summary>
    internal class BaseItemIoLimitConfig : EntityConfig<BaseItemIoLimit>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("Item_IO_Limit").MapAllProperties();
            Meta.Property(BaseItemIoLimit.RemarksProperty).ColumnMeta.HasLength(2000);
            Meta.Property(BaseItemIoLimit.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(BaseItemIoLimit.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.Property(BaseItemIoLimit.ItemIdProperty).ColumnMeta.HasIndex(IndexTypeMeta.Indexed);
            Meta.EnablePhantoms();
        }
    }
}