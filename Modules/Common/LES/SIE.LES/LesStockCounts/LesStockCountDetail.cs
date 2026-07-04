using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点单明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("线边仓盘点单明细")]
    public partial class LesStockCountDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Required]
        [Label("行号")]
        [MaxLength(80)]
        public static readonly Property<string> LineNoProperty = P<LesStockCountDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion
        
        #region 现有量 Qty
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
        public static readonly Property<decimal?> QtyProperty = P<LesStockCountDetail>.Register(e => e.Qty);

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion
        
        #region 实盘数量 ActualCountQty
        /// <summary>
        /// 实盘数量
        /// </summary>
        [MinValue(0)]
        [Label("实盘数量")]
        public static readonly Property<decimal?> ActualCountQtyProperty = P<LesStockCountDetail>.Register(e => e.ActualCountQty);

        /// <summary>
        /// 实盘数量
        /// </summary>
        public decimal? ActualCountQty
        {
            get { return GetProperty(ActualCountQtyProperty); }
            set { SetProperty(ActualCountQtyProperty, value); }
        }
        #endregion

        #region 差异数量 DiffCountQty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal?> DiffCountQtyProperty = P<LesStockCountDetail>.Register(e => e.DiffCountQty);

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal? DiffCountQty
        {
            get { return GetProperty(DiffCountQtyProperty); }
            set { SetProperty(DiffCountQtyProperty, value); }
        }
        #endregion

        #region 盘点时间 CountDate
        /// <summary>
        /// 盘点时间
        /// </summary>
        [Label("盘点时间")]
        public static readonly Property<DateTime?> CountDateProperty = P<LesStockCountDetail>.Register(e => e.CountDate);

        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime? CountDate
        {
            get { return GetProperty(CountDateProperty); }
            set { SetProperty(CountDateProperty, value); }
        }
        #endregion

        #region 新增盘盈 IsNewInventory
        /// <summary>
        /// 新增盘盈
        /// </summary>
        [Label("新增盘盈")]
        public static readonly Property<bool> IsNewInventoryProperty = P<LesStockCountDetail>.Register(e => e.IsNewInventory);

        /// <summary>
        /// 新增盘盈
        /// </summary>
        public bool IsNewInventory
        {
            get { return GetProperty(IsNewInventoryProperty); }
            set { SetProperty(IsNewInventoryProperty, value); }
        }
        #endregion

        #region 行状态 State
        /// <summary>
        /// 行状态
        /// </summary>
        [Label("行状态")]
        public static readonly Property<LesCountState> StateProperty = P<LesStockCountDetail>.Register(e => e.State);

        /// <summary>
        /// 行状态
        /// </summary>
        public LesCountState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion        

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<LesStockCountDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<LesStockCountDetail>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly IRefIdProperty WarehouseIdProperty = P<LesStockCountDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<LesStockCountDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion
                
        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<LesStockCountDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
            set { SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<LesStockCountDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion
        
        #region 批次 Lot
        /// <summary>
        /// 批次Id
        /// </summary>
        [Label("批次")]
        public static readonly IRefIdProperty LotIdProperty = P<LesStockCountDetail>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次Id
        /// </summary>
        public double? LotId
        {
            get { return (double?)GetRefNullableId(LotIdProperty); }
            set { SetRefNullableId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty = P<LesStockCountDetail>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot
        {
            get { return GetRefEntity(LotProperty); }
            set { SetRefEntity(LotProperty, value); }
        }
        #endregion        

        #region 盘点人 CountBy
        /// <summary>
        /// 盘点人Id
        /// </summary>
        [Label("盘点人")]
        public static readonly IRefIdProperty CountByIdProperty = P<LesStockCountDetail>.RegisterRefId(e => e.CountById, ReferenceType.Normal);

        /// <summary>
        /// 盘点人Id
        /// </summary>
        public double? CountById
        {
            get { return (double?)GetRefNullableId(CountByIdProperty); }
            set { SetRefNullableId(CountByIdProperty, value); }
        }

        /// <summary>
        /// 盘点人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CountByProperty = P<LesStockCountDetail>.RegisterRef(e => e.CountBy, CountByIdProperty);

        /// <summary>
        /// 盘点人
        /// </summary>
        public Employee CountBy
        {
            get { return GetRefEntity(CountByProperty); }
            set { SetRefEntity(CountByProperty, value); }
        }
        #endregion

        #region 明细盘点结果 LesStockCountDetailResult
        /// <summary>
        /// 盘点结果
        /// </summary>
        [Label("盘点结果")]
        public static readonly Property<LesStockCountDetailResult?> LesStockCountDetailResultProperty = P<LesStockCountDetail>.Register(e => e.LesStockCountDetailResult);

        /// <summary>
        /// 盘点结果
        /// </summary>
        public LesStockCountDetailResult? LesStockCountDetailResult
        {
            get { return GetProperty(LesStockCountDetailResultProperty); }
            set { SetProperty(LesStockCountDetailResultProperty, value); }
        }
        #endregion

        #region 盘点单 LesStockCount
        /// <summary>
        /// 盘点单Id
        /// </summary>
        [Label("盘点单")]
        public static readonly IRefIdProperty LesStockCountIdProperty = P<LesStockCountDetail>.RegisterRefId(e => e.LesStockCountId, ReferenceType.Parent);

        /// <summary>
        /// 盘点单Id
        /// </summary>
        public double LesStockCountId
        {
            get { return (double)GetRefId(LesStockCountIdProperty); }
            set { SetRefId(LesStockCountIdProperty, value); }
        }

        /// <summary>
        /// 盘点单
        /// </summary>
        public static readonly RefEntityProperty<LesStockCount> LesStockCountProperty = P<LesStockCountDetail>.RegisterRef(e => e.LesStockCount, LesStockCountIdProperty);

        /// <summary>
        /// 盘点单
        /// </summary>
        public LesStockCount LesStockCount
        {
            get { return GetRefEntity(LesStockCountProperty); }
            set { SetRefEntity(LesStockCountProperty, value); }
        }
        #endregion
       
        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState?> OnhandStateProperty = P<LesStockCountDetail>.Register(e => e.OnhandState);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? OnhandState
        {
            get { return this.GetProperty(OnhandStateProperty); }
            set { this.SetProperty(OnhandStateProperty, value); }
        }
        #endregion
        
        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [MaxLength(120)]
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<LesStockCountDetail>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示名 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<LesStockCountDetail>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 分析结果 AnalysisResult
        /// <summary>
        /// 分析结果
        /// </summary>
        [Label("分析结果")]
        public static readonly Property<AnalysisResult?> AnalysisResultProperty = P<LesStockCountDetail>.Register(e => e.AnalysisResult);

        /// <summary>
        /// 分析结果
        /// </summary>
        public AnalysisResult? AnalysisResult
        {
            get { return this.GetProperty(AnalysisResultProperty); }
            set { this.SetProperty(AnalysisResultProperty, value); }
        }
        #endregion

        #region 结果描述 ResultDesc
        /// <summary>
        /// 结果描述
        /// </summary>
        [Label("结果描述")]
        public static readonly Property<string> ResultDescProperty = P<LesStockCountDetail>.Register(e => e.ResultDesc);

        /// <summary>
        /// 结果描述
        /// </summary>
        public string ResultDesc
        {
            get { return this.GetProperty(ResultDescProperty); }
            set { this.SetProperty(ResultDescProperty, value); }
        }
        #endregion

        #region 是否允许调账 IsAllowAdjust
        /// <summary>
        /// 是否允许调账
        /// </summary>
        [Label("是否允许调账")]
        public static readonly Property<bool?> IsAllowAdjustProperty = P<LesStockCountDetail>.Register(e => e.IsAllowAdjust);

        /// <summary>
        /// 是否允许调账
        /// </summary>
        public bool? IsAllowAdjust
        {
            get { return this.GetProperty(IsAllowAdjustProperty); }
            set { this.SetProperty(IsAllowAdjustProperty, value); }
        }
        #endregion

        #region 标签 LabelNo
        /// <summary>
        /// 标签
        /// </summary>
        [Label("标签")]
        public static readonly Property<string> LabelNoProperty = P<LesStockCountDetail>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 盘点细度 CountDimension
        /// <summary>
        /// 盘点细度
        /// </summary>
        [Label("盘点细度")]
        public static readonly Property<CountDimension> CountDimensionProperty = P<LesStockCountDetail>.Register(e => e.CountDimension);

        /// <summary>
        /// 盘点细度
        /// </summary>
        public CountDimension CountDimension
        {
            get { return GetProperty(CountDimensionProperty); }
            set { SetProperty(CountDimensionProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<LesStockCountDetail>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Resources.Enterprises.Enterprise> FactoryProperty =
            P<LesStockCountDetail>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Resources.Enterprises.Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 盘点单号 StockCountNo
        /// <summary>
        /// 盘点单号
        /// </summary>
        [Label("盘点单号")]
        public static readonly Property<string> StockCountNoProperty = P<LesStockCountDetail>.RegisterView(e => e.StockCountNo, p => p.LesStockCount.No);

        /// <summary>
        /// 盘点单号
        /// </summary>
        public string StockCountNo
        {
            get { return this.GetProperty(StockCountNoProperty); }
        }
        #endregion

        #region 盘点单状态 StockCountState
        /// <summary>
        /// 盘点单状态
        /// </summary>
        [Label("盘点单状态")]
        public static readonly Property<LesCountState> StockCountStateProperty = P<LesStockCountDetail>.RegisterView(e => e.StockCountState, p => p.LesStockCount.State);

        /// <summary>
        /// 盘点单状态
        /// </summary>
        public LesCountState StockCountState
        {
            get { return this.GetProperty(StockCountStateProperty); }
        }
        #endregion

        #region 启用物料扩展属性 EnableExtPro
        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        [Label("启用物料扩展属性")]
        public static readonly Property<bool> EnableExtProProperty = P<LesStockCountDetail>.RegisterView(e => e.EnableExtPro, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        public bool EnableExtPro
        {
            get { return this.GetProperty(EnableExtProProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<LesStockCountDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<LesStockCountDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<LesStockCountDetail>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        ///  单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<LesStockCountDetail>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        ///  单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<LesStockCountDetail>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<LesStockCountDetail>.RegisterView(e => e.LotCode, p => p.Lot.Code);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
        }
        #endregion

        #region 仓库名称 WareHouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WareHouseNameProperty = P<LesStockCountDetail>.RegisterView(e => e.WareHouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName
        {
            get { return this.GetProperty(WareHouseNameProperty); }
        }
        #endregion

        #region 仓库编码 WareHouseCode
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WareHouseCodeProperty = P<LesStockCountDetail>.RegisterView(e => e.WareHouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseCode
        {
            get { return this.GetProperty(WareHouseCodeProperty); }
        }
        #endregion

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<LesStockCountDetail>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryNameProperty = P<LesStockCountDetail>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<LesStockCountDetail>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 盘点单明细 实体配置
    /// </summary>
    internal class StockCountDetailConfig : EntityConfig<LesStockCountDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_STOCK_COUNT_DTL").MapAllProperties();           
            Meta.Property(LesStockCountDetail.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(LesStockCountDetail.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.Property(LesStockCountDetail.ResultDescProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}