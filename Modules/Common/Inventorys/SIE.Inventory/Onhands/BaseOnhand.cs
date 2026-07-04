using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库存基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("库存基类")]
    [DisplayMember(nameof(Id))]
    ////[SIE.DataAuth.EntityDataAuth(typeof(WarehouseEmployee), nameof(WarehouseId))]
    public partial class BaseOnhand : DataEntity
    {
        #region 现有量 Qty
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
        public static readonly Property<decimal> QtyProperty = P<BaseOnhand>.Register(e => e.Qty);

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 可用量 AvailableQty
        /// <summary>
        /// 可用量
        /// </summary>
        [Label("可用量")]
        public static readonly Property<decimal> AvailableQtyProperty = P<BaseOnhand>.Register(e => e.AvailableQty);

        /// <summary>
        /// 可用量
        /// </summary>
        public decimal AvailableQty
        {
            get { return GetProperty(AvailableQtyProperty); }
            set { SetProperty(AvailableQtyProperty, value); }
        }
        #endregion

        #region 冻结量 FreezingQty
        /// <summary>
        /// 冻结量
        /// </summary>
        [Label("冻结量")]
        public static readonly Property<decimal> FreezingQtyProperty = P<BaseOnhand>.Register(e => e.FreezingQty);

        /// <summary>
        /// 冻结量
        /// </summary>
        public decimal FreezingQty
        {
            get { return GetProperty(FreezingQtyProperty); }
            set { SetProperty(FreezingQtyProperty, value); }
        }
        #endregion

        #region 分配量 AllottedQty
        /// <summary>
        /// 分配量
        /// </summary>
        [Label("分配量")]
        public static readonly Property<decimal> AllottedQtyProperty = P<BaseOnhand>.Register(e => e.AllottedQty);

        /// <summary>
        /// 分配量
        /// </summary>
        public decimal AllottedQty
        {
            get { return GetProperty(AllottedQtyProperty); }
            set { SetProperty(AllottedQtyProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<BaseOnhand>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<BaseOnhand>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty =
            P<BaseOnhand>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区
        /// </summary>
        public double StorageAreaId
        {
            get { return (double)this.GetRefId(StorageAreaIdProperty); }
            set { this.SetRefId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty =
            P<BaseOnhand>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return this.GetRefEntity(StorageAreaProperty); }
            set { this.SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<BaseOnhand>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<BaseOnhand>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<BaseOnhand>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<BaseOnhand>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BaseOnhand>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<BaseOnhand>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region ABC分类 ABCCategory
        /// <summary>
        /// ABC分类
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<AbcType?> ABCCategoryProperty = P<BaseOnhand>.RegisterView(e => e.ABCCategory, p => p.Item.ABCCategory);

        /// <summary>
        /// ABC分类
        /// </summary>
        public AbcType? ABCCategory
        {
            get { return this.GetProperty(ABCCategoryProperty); }
        }
        #endregion

        #region 单位 ItemUnit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitProperty = P<BaseOnhand>.RegisterView(e => e.ItemUnit, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnit
        {
            get { return this.GetProperty(ItemUnitProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<BaseOnhand>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 物料扩展属性显示名 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<BaseOnhand>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 启用物料扩展属性 EnableExtPro
        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        [Label("启用物料扩展属性")]
        public static readonly Property<bool> EnableExtProProperty = P<BaseOnhand>.RegisterView(e => e.EnableExtPro, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        public bool EnableExtPro
        {
            get { return this.GetProperty(EnableExtProProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<BaseOnhand>.RegisterView(e => e.UnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<BaseOnhand>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<BaseOnhand>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 库区编码 AreaCode
        /// <summary>
        /// 库区编码
        /// </summary>
        [Label("库区编码")]
        public static readonly Property<string> AreaCodeProperty = P<BaseOnhand>.RegisterView(e => e.AreaCode, p => p.StorageArea.Code);

        /// <summary>
        /// 库区编码
        /// </summary>
        public string AreaCode
        {
            get { return this.GetProperty(AreaCodeProperty); }
        }
        #endregion

        #region 库区名称 AreaName
        /// <summary>
        /// 库区名称
        /// </summary>
        [Label("库区名称")]
        public static readonly Property<string> AreaNameProperty = P<BaseOnhand>.RegisterView(e => e.AreaName, p => p.StorageArea.Name);

        /// <summary>
        /// 库区名称
        /// </summary>
        public string AreaName
        {
            get { return this.GetProperty(AreaNameProperty); }
        }
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<BaseOnhand>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<BaseOnhand>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 允许人工上架 IsAllowManualGrounding
        /// <summary>
        /// 允许人工上架
        /// </summary>
        [Label("允许人工上架")]
        public static readonly Property<bool> IsAllowManualGroundingProperty = P<BaseOnhand>.RegisterView(e => e.IsAllowManualGrounding, p => p.StorageArea.IsAllowManualGrounding);

        /// <summary>
        /// 允许人工上架
        /// </summary>
        public bool IsAllowManualGrounding
        {
            get { return this.GetProperty(IsAllowManualGroundingProperty); }
        }
        #endregion

        #region 立库库位 IsAutomatedStorage
        /// <summary>
        /// 立库库位
        /// </summary>
        [Label("立库库位")]
        public static readonly Property<bool> IsAutomatedStorageProperty = P<BaseOnhand>.RegisterView(e => e.IsAutomatedStorage, p => p.StorageLocation.IsAutomatedStorage);

        /// <summary>
        /// 立库库位
        /// </summary>
        public bool IsAutomatedStorage
        {
            get { return this.GetProperty(IsAutomatedStorageProperty); }
        }
        #endregion

        #region 是否立库库区 IsAutomatedArea
        /// <summary>
        /// 是否立库
        /// </summary>
        [Label("是否立库")]
        public static readonly Property<bool> IsAutomatedAreaProperty = P<BaseOnhand>.RegisterView(e => e.IsAutomatedArea, p => p.StorageArea.IsAutomatedArea);

        /// <summary>
        /// 是否立库
        /// </summary>
        public bool IsAutomatedArea
        {
            get { return this.GetProperty(IsAutomatedAreaProperty); }
        }
        #endregion

        #region 排 RowNo
        /// <summary>
        /// 排
        /// </summary>
        [Label("排")]
        public static readonly Property<int> RowNoProperty = P<BaseOnhand>.RegisterView(e => e.RowNo, p => p.StorageLocation.RowNo);

        /// <summary>
        /// 排
        /// </summary>
        public int RowNo
        {
            get { return this.GetProperty(RowNoProperty); }
        }
        #endregion

        #region 层 LayerNo
        /// <summary>
        /// 层
        /// </summary>
        [Label("层")]
        public static readonly Property<int> LayerNoProperty = P<BaseOnhand>.RegisterView(e => e.LayerNo, p => p.StorageLocation.LayerNo);

        /// <summary>
        /// 层
        /// </summary>
        public int LayerNo
        {
            get { return this.GetProperty(LayerNoProperty); }
        }
        #endregion

        #region 列 ColumnNo
        /// <summary>
        /// 列
        /// </summary>
        [Label("列")]
        public static readonly Property<int> ColumnNoProperty = P<BaseOnhand>.RegisterView(e => e.ColumnNo, p => p.StorageLocation.ColumnNo);

        /// <summary>
        /// 列
        /// </summary>
        public int ColumnNo
        {
            get { return this.GetProperty(ColumnNoProperty); }
        }
        #endregion

        #region 深度 Depth
        /// <summary>
        /// 深度
        /// </summary>
        [Label("深度")]
        public static readonly Property<int> DepthProperty = P<BaseOnhand>.RegisterView(e => e.Depth, p => p.StorageLocation.Depth);

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get { return this.GetProperty(DepthProperty); }
        }
        #endregion

        #region 最深库位 IsMaxDepth
        /// <summary>
        /// 最深库位
        /// </summary>
        [Label("最深库位")]
        public static readonly Property<bool?> IsMaxDepthProperty = P<BaseOnhand>.RegisterView(e => e.IsMaxDepth, p => p.StorageLocation.IsMaxDepth);

        /// <summary>
        /// 最深库位
        /// </summary>
        public bool? IsMaxDepth
        {
            get { return this.GetProperty(IsMaxDepthProperty); }
        }
        #endregion

        #region 巷道 RouteNo
        /// <summary>
        /// 巷道
        /// </summary>
        [Label("巷道")]
        public static readonly Property<int> RouteNoProperty = P<BaseOnhand>.RegisterView(e => e.RouteNo, p => p.StorageLocation.Routeway.RoutewayNumber);

        /// <summary>
        /// 巷道
        /// </summary>
        public int RouteNo
        {
            get { return this.GetProperty(RouteNoProperty); }
        }
        #endregion

        #region 巷道 RoutewayId
        /// <summary>
        /// 巷道
        /// </summary>
        [Label("巷道ID")]
        public static readonly Property<double?> RoutewayIdProperty = P<BaseOnhand>.RegisterView(e => e.RoutewayId, p => p.StorageLocation.RoutewayId);

        /// <summary>
        /// 巷道
        /// </summary>
        public double? RoutewayId
        {
            get { return this.GetProperty(RoutewayIdProperty); }
        }
        #endregion

        #region 物料单位Id ItemUnitId
        /// <summary>
        /// 物料单位Id
        /// </summary>
        [Label("物料单位Id")]
        public static readonly Property<double?> ItemUnitIdProperty = P<BaseOnhand>.RegisterView(e => e.ItemUnitId, p => p.Item.UnitId);

        /// <summary>
        /// 物料单位Id
        /// </summary>
        public double? ItemUnitId
        {
            get { return this.GetProperty(ItemUnitIdProperty); }
        }
        #endregion

        #region 辅助单位 SecondUnitId
        /// <summary>
        /// 辅助单位
        /// </summary>
        [Label("辅助单位")]
        public static readonly Property<double?> SecondUnitIdProperty = P<BaseOnhand>.RegisterView(e => e.SecondUnitId, p => p.Item.SecondUnitId);

        /// <summary>
        /// 辅助单位
        /// </summary>
        public double? SecondUnitId
        {
            get { return this.GetProperty(SecondUnitIdProperty); }
        }
        #endregion

        #region 辅助单位 SecondUnitName
        /// <summary>
        /// 辅助单位
        /// </summary>
        [Label("辅助单位")]
        public static readonly Property<string> SecondUnitNameProperty = P<BaseOnhand>.RegisterView(e => e.SecondUnitName, p => p.Item.SecondUnit.Name);

        /// <summary>
        /// 辅助单位
        /// </summary>
        public string SecondUnitName
        {
            get { return this.GetProperty(SecondUnitNameProperty); }
        }
        #endregion

        #region 主单位精度 UnitPrecision
        /// <summary>
        /// 主单位精度
        /// </summary>
        [Label("主单位精度")]
        public static readonly Property<int?> UnitPrecisionProperty = P<BaseOnhand>.RegisterView(e => e.UnitPrecision, p => p.Item.Unit.Precision);

        /// <summary>
        /// 主单位精度
        /// </summary>
        public int? UnitPrecision
        {
            get { return this.GetProperty(UnitPrecisionProperty); }
        }
        #endregion

        #region 主单位取舍类型 MainTrade
        /// <summary>
        /// 主单位取舍类型
        /// </summary>
        [Label("主单位取舍类型")]
        public static readonly Property<TradeType> MainTradeProperty = P<BaseOnhand>.RegisterView(e => e.MainTrade, p => p.Item.Unit.TradeType);

        /// <summary>
        /// 主单位取舍类型
        /// </summary>
        public TradeType MainTrade
        {
            get { return this.GetProperty(MainTradeProperty); }
        }
        #endregion

        #region 辅单位精度 SecondUnitPrecision
        /// <summary>
        /// 辅单位精度
        /// </summary>
        [Label("辅单位精度")]
        public static readonly Property<int?> SecondUnitPrecisionProperty = P<BaseOnhand>.RegisterView(e => e.SecondUnitPrecision, p => p.Item.SecondUnit.Precision);

        /// <summary>
        /// 辅单位精度
        /// </summary>
        public int? SecondUnitPrecision
        {
            get { return this.GetProperty(SecondUnitPrecisionProperty); }
        }
        #endregion

        #region 辅助单位取舍 SecondTrade
        /// <summary>
        /// 辅助单位取舍
        /// </summary>
        [Label("辅助单位取舍")]
        public static readonly Property<TradeType> SecondTradeProperty = P<BaseOnhand>.RegisterView(e => e.SecondTrade, p => p.Item.SecondUnit.TradeType);

        /// <summary>
        /// 辅助单位取舍
        /// </summary>
        public TradeType SecondTrade
        {
            get { return this.GetProperty(SecondTradeProperty); }
        }
        #endregion

    }
}