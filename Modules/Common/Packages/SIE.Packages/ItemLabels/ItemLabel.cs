using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels.Configs;
using SIE.Warehouses;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ItemLabelCriteria))]
    [EntityWithConfig(typeof(ItemLabelNoConfig))]
    [EntityWithConfig(typeof(Configs.ItemLabelConfig))]
    [Label("物料标签")]
    [DisplayMember(nameof(Label))]
    //[DataAuth.EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class ItemLabel : DataEntity
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ItemLabel()
        {
            NgQty = 0;
            ProjectNo = "*";
        }

        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        [Required]
        public static readonly Property<string> LabelProperty = P<ItemLabel>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
            set { this.SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 可用数量 Qty
        /// <summary>
        /// 可用数量
        /// </summary>
        [Label("可用数量")]
        public static readonly Property<decimal> QtyProperty = P<ItemLabel>.Register(e => e.Qty);

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty = P<ItemLabel>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<ItemLabel>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 来源工单 WorkOrder
        /// <summary>
        /// 来源工单Id
        /// </summary>
        [Label("来源工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ItemLabel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 来源工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 来源工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<ItemLabel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 来源工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
               
        #region 包装关系 Relation
        /// <summary>
        /// 包装关系Id
        /// </summary>
        [Label("包装关系")]
        public static readonly IRefIdProperty RelationIdProperty = P<ItemLabel>.RegisterRefId(e => e.RelationId, ReferenceType.Normal);

        /// <summary>
        /// 包装关系Id
        /// </summary>
        public double? RelationId
        {
            get { return (double?)GetRefNullableId(RelationIdProperty); }
            set { SetRefNullableId(RelationIdProperty, value); }
        }

        /// <summary>
        /// 包装关系
        /// </summary>
        public static readonly RefEntityProperty<PackingRelation> RelationProperty = P<ItemLabel>.RegisterRef(e => e.Relation, RelationIdProperty);

        /// <summary>
        /// 包装关系
        /// </summary>
        public PackingRelation Relation
        {
            get { return GetRefEntity(RelationProperty); }
            set { SetRefEntity(RelationProperty, value); }
        }
        #endregion

        #region 条码信息来源 SourceType
        /// <summary>
        /// 条码信息来源
        /// </summary>
        [Label("条码信息来源")]
        public static readonly Property<LabelSource> SourceTypeProperty = P<ItemLabel>.Register(e => e.SourceType);

        /// <summary>
        /// 条码信息来源
        /// </summary>
        public LabelSource SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [Label("重量")]
        public static readonly Property<decimal> WeightProperty = P<ItemLabel>.Register(e => e.Weight);

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 原始标签 OriginalLabel
        /// <summary>
        /// 原始标签
        /// </summary>
        [Label("原始标签")]
        public static readonly Property<string> OriginalLabelProperty = P<ItemLabel>.Register(e => e.OriginalLabel);

        /// <summary>
        /// 原始标签
        /// </summary>
        public string OriginalLabel
        {
            get { return this.GetProperty(OriginalLabelProperty); }
            set { this.SetProperty(OriginalLabelProperty, value); }
        }
        #endregion 

        #region 标签属性 LabelPropertyValueList
        /// <summary>
        /// 标签属性
        /// </summary>
        [Label("标签属性")]
        public static readonly ListProperty<EntityList<LabelPropertyValue>> PropertyValueListProperty = P<ItemLabel>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 标签属性
        /// </summary>
        public EntityList<LabelPropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemLabel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<ItemLabel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ItemLabel>.RegisterView(e => e.ShortDescription,p=>p.Item.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<ItemLabel>.RegisterView(e => e.MrpController,p=>p.Item.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<ItemLabel>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ItemLabel>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotProperty = P<ItemLabel>.Register(e => e.Lot);

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot
        {
            get { return GetProperty(LotProperty); }
            set { SetProperty(LotProperty, value); }
        }
        #endregion

        #region 生产批次 ProductBatch
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> ProductBatchProperty = P<ItemLabel>.Register(e => e.ProductBatch);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductBatch
        {
            get { return GetProperty(ProductBatchProperty); }
            set { SetProperty(ProductBatchProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<ItemLabel>.Register(e => e.ProductionDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return GetProperty(ProductionDateProperty); }
            set { SetProperty(ProductionDateProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<ItemLabel>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<ItemLabel>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region ASN单号 AsnNo
        /// <summary>
        /// ASN单号
        /// </summary>
        [Label("ASN单号")]
        public static readonly Property<string> AsnNoProperty = P<ItemLabel>.Register(e => e.AsnNo);

        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo
        {
            get { return GetProperty(AsnNoProperty); }
            set { SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateTime?> DeliveryDateProperty = P<ItemLabel>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region WPNL尺寸长 PnlHeight
        /// <summary>
        /// WPNL尺寸长
        /// </summary>
        [Label("WPNL尺寸长")]
        public static readonly Property<decimal?> PnlHeightProperty = P<ItemLabel>.Register(e => e.PnlHeight);

        /// <summary>
        /// WPNL尺寸长
        /// </summary>
        public decimal? PnlHeight
        {
            get { return this.GetProperty(PnlHeightProperty); }
            set { this.SetProperty(PnlHeightProperty, value); }
        }
        #endregion

        #region WPNL尺寸宽 PnlWidth
        /// <summary>
        /// WPNL尺寸宽
        /// </summary>
        [Label("WPNL尺寸宽")]
        public static readonly Property<decimal?> PnlWidthProperty = P<ItemLabel>.Register(e => e.PnlWidth);

        /// <summary>
        /// WPNL尺寸宽
        /// </summary>
        public decimal? PnlWidth
        {
            get { return this.GetProperty(PnlWidthProperty); }
            set { this.SetProperty(PnlWidthProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<ItemLabel>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<ItemLabel>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Resources.Enterprises.Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<ItemLabel>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 正常退料在途数 ReturnQtyInTransit
        /// <summary>
        /// 正常退料在途数
        /// </summary>
        [Label("正常退料在途数")]
        public static readonly Property<decimal?> ReturnQtyInTransitProperty = P<ItemLabel>.Register(e => e.ReturnQtyInTransit);

        /// <summary>
        /// 正常退料在途数
        /// </summary>
        public decimal? ReturnQtyInTransit
        {
            get { return this.GetProperty(ReturnQtyInTransitProperty); }
            set { this.SetProperty(ReturnQtyInTransitProperty, value); }
        }
        #endregion

        #region 不良退料在途数 NgReturnQtyInTransit
        /// <summary>
        /// 不良退料在途数
        /// </summary>
        [Label("不良退料在途数")]
        public static readonly Property<decimal?> NgReturnQtyInTransitProperty = P<ItemLabel>.Register(e => e.NgReturnQtyInTransit);

        /// <summary>
        /// 不良退料在途数
        /// </summary>
        public decimal? NgReturnQtyInTransit
        {
            get { return this.GetProperty(NgReturnQtyInTransitProperty); }
            set { this.SetProperty(NgReturnQtyInTransitProperty, value); }
        }
        #endregion

        #region 序列号管理 IsSerialNumber
        /// <summary>
        /// 序列号管理
        /// </summary>
        [Label("序列号管理")]
        public static readonly Property<bool?> IsSerialNumberProperty = P<ItemLabel>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 序列号管理
        /// </summary>
        public bool? IsSerialNumber
        {
            get { return GetProperty(IsSerialNumberProperty); }
            set { SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<ItemLabel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<ItemLabel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<ItemLabel>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<ItemLabel>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示名 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<ItemLabel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<ItemLabel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<ItemLabel>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 物料投入工单 WorkOrderList
        /// <summary>
        /// 物料投入工单
        /// </summary>
        [Label("物料投入工单")]
        public static readonly ListProperty<EntityList<ItemLabelWorkOrder>> WorkOrderListProperty
            = P<ItemLabel>.RegisterList(e => e.WorkOrderList);

        /// <summary>
        /// 物料投入工单
        /// </summary>
        public EntityList<ItemLabelWorkOrder> WorkOrderList
        {
            get { return this.GetLazyList(WorkOrderListProperty); }
        }
        #endregion

        #region 视图属性

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<ItemLabel>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

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
        public static readonly Property<string> StorageLocationNameProperty = P<ItemLabel>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion


        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<ItemLabel>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType> ItemTypeProperty = P<ItemLabel>.RegisterView(e => e.ItemType, p => p.Item.Type);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
        }
        #endregion

        #region 物料来源类型 ItemSourceType
        /// <summary>
        /// 物料来源类型
        /// </summary>
        [Label("物料来源类型")]
        public static readonly Property<ItemSourceType?> ItemSourceTypeProperty = P<ItemLabel>.RegisterView(e => e.ItemSourceType, p => p.Item.ItemSourceType);

        /// <summary>
        /// 物料来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType
        {
            get { return this.GetProperty(ItemSourceTypeProperty); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<ItemLabel>.RegisterView(e => e.Specification, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<ItemLabel>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<ItemLabel>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

        /// <summary>
        /// 
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
        }
        #endregion

        #region 供应商 SupplierName
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly Property<string> SupplierNameProperty = P<ItemLabel>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #endregion

        #region 有效期管理
        #region 剩余可用时长 RemainLongLived
        /// <summary>
        /// 剩余可用时长
        /// </summary>
        [Label("剩余可用时长")]
        public static readonly Property<decimal?> RemainLongLivedProperty = P<ItemLabel>.Register(e => e.RemainLongLived);

        /// <summary>
        /// 剩余可用时长
        /// </summary>
        public decimal? RemainLongLived
        {
            get { return this.GetProperty(RemainLongLivedProperty); }
            set { this.SetProperty(RemainLongLivedProperty, value); }
        }
        #endregion

        #region 可用时长寿命 LongLived
        /// <summary>
        /// 可用时长寿命
        /// </summary>
        [Label("可用时长寿命")]
        public static readonly Property<decimal?> LongLivedProperty = P<ItemLabel>.Register(e => e.LongLived);

        /// <summary>
        /// 可用时长寿命
        /// </summary>
        public decimal? LongLived
        {
            get { return this.GetProperty(LongLivedProperty); }
            set { this.SetProperty(LongLivedProperty, value); }
        }
        #endregion


        #region 有效期开始时间 ValidityStart
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        [Label("有效期开始时间")]
        public static readonly Property<DateTime?> ValidityStartProperty = P<ItemLabel>.Register(e => e.ValidityStart);

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? ValidityStart
        {
            get { return this.GetProperty(ValidityStartProperty); }
            set { this.SetProperty(ValidityStartProperty, value); }
        }
        #endregion

        #region 有效期结束时间 ValidityEnd
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        [Label("有效期结束时间")]
        public static readonly Property<DateTime?> ValidityEndProperty = P<ItemLabel>.Register(e => e.ValidityEnd);

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime? ValidityEnd
        {
            get { return this.GetProperty(ValidityEndProperty); }
            set { this.SetProperty(ValidityEndProperty, value); }
        }
        #endregion

        #endregion

        #region 状态 ItemLabelState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ItemLabelState?> ItemLabelStateProperty = P<ItemLabel>.Register(e => e.ItemLabelState);

        /// <summary>
        /// 状态
        /// </summary>
        public ItemLabelState? ItemLabelState
        {
            get { return this.GetProperty(ItemLabelStateProperty); }
            set { this.SetProperty(ItemLabelStateProperty, value); }
        }
        #endregion

        #region 库存地 Lgort
        /// <summary>
        /// 库存地
        /// </summary>
        [Label("库存地")]
        public static readonly Property<string> LgortProperty = P<ItemLabel>.Register(e => e.Lgort);

        /// <summary>
        /// 库存地
        /// </summary>
        public string Lgort
        {
            get { return this.GetProperty(LgortProperty); }
            set { this.SetProperty(LgortProperty, value); }
        }
        #endregion

        #region 外部处理单位标识 Exidv
        /// <summary>
        /// 外部处理单位标识
        /// </summary>
        [Label("外部处理单位标识")]
        public static readonly Property<string> ExidvProperty = P<ItemLabel>.Register(e => e.Exidv);

        /// <summary>
        /// 外部处理单位标识
        /// </summary>
        public string Exidv
        {
            get { return this.GetProperty(ExidvProperty); }
            set { this.SetProperty(ExidvProperty, value); }
        }
        #endregion

        #region 绿标标签 Exidv2
        /// <summary>
        /// 绿标标签
        /// </summary>
        [Label("绿标标签")]
        public static readonly Property<string> Exidv2Property = P<ItemLabel>.Register(e => e.Exidv2);

        /// <summary>
        /// 绿标标签
        /// </summary>
        public string Exidv2
        {
            get { return this.GetProperty(Exidv2Property); }
            set { this.SetProperty(Exidv2Property, value); }
        }
        #endregion

        #region 是否已使用 Isuse
        /// <summary>
        /// 是否已使用
        /// </summary>
        [Label("是否已使用")]
        public static readonly Property<bool> IsuseProperty = P<ItemLabel>.Register(e => e.Isuse);

        /// <summary>
        /// 是否已使用
        /// </summary>
        public bool Isuse
        {
            get { return this.GetProperty(IsuseProperty); }
            set { this.SetProperty(IsuseProperty, value); }
        }
        #endregion

        #region 初始数量 InitialQty
        /// <summary>
        /// 初始数量
        /// </summary>
        [Label("初始数量")]
        public static readonly Property<decimal?> InitialQtyProperty = P<ItemLabel>.Register(e => e.InitialQty);

        /// <summary>
        /// 初始数量
        /// </summary>
        public decimal? InitialQty
        {
            get { return this.GetProperty(InitialQtyProperty); }
            set { this.SetProperty(InitialQtyProperty, value); }
        }
        #endregion

        #region 跨库存组织同步工厂 ToFactory
        /// <summary>
        /// 跨库存组织同步工厂
        /// </summary>
        [Label("跨库存组织同步工厂")]
        public static readonly Property<string> ToFactoryProperty = P<ItemLabel>.Register(e => e.ToFactory);

        /// <summary>
        /// 跨库存组织同步工厂
        /// </summary>
        public string ToFactory
        {
            get { return this.GetProperty(ToFactoryProperty); }
            set { this.SetProperty(ToFactoryProperty, value); }
        }
        #endregion

        #region 供应商批次 Licha
        /// <summary>
        /// 供应商批次
        /// </summary>
        [Label("供应商批次")]
        public static readonly Property<string> LichaProperty = P<ItemLabel>.Register(e => e.Licha);

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string Licha
        {
            get { return this.GetProperty(LichaProperty); }
            set { this.SetProperty(LichaProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 物料标签 实体配置
    /// </summary>
    internal class ItemLabelConfig : EntityConfig<ItemLabel>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(ItemLabel.LabelProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }

        /// <summary>
        /// Meta属性配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_LABEL").MapAllProperties();
            Meta.Property(ItemLabel.RelationIdProperty).MapColumn().IgnoreFK().IsNullable();
            Meta.Property(ItemLabel.SpecificationProperty).ColumnMeta.HasLength(200);
            Meta.Property(ItemLabel.LabelProperty).ColumnMeta.HasIndex();
            Meta.Property(ItemLabel.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(ItemLabel.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}