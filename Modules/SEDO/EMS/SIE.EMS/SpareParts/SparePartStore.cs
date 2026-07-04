using SIE.Common.Configs;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.SpareParts.Configs;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件入库
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SparePartStoreCriteria))]
    [EntityWithConfig(typeof(SparePartStoreNoConfig))]
    [EntityWithConfig(typeof(IsComputeAvgCostConfig))]
    [EntityWithConfig(typeof(StoreDetailLabelPrintConfig))]
    [Label("备件入库")]
    public partial class SparePartStore : DataEntity
    {
        #region 备件入库单号 StoreCode
        /// <summary>
        /// 备件入库单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("备件入库单号")]
        public static readonly Property<string> StoreCodeProperty = P<SparePartStore>.Register(e => e.StoreCode);

        /// <summary>
        /// 备件入库单号
        /// </summary>
        public string StoreCode
        {
            get { return GetProperty(StoreCodeProperty); }
            set { SetProperty(StoreCodeProperty, value); }
        }
        #endregion

        #region 入库类型 InboundType
        /// <summary>
        /// 入库类型
        /// </summary>
        [Required]
        [Label("入库类型")]
        public static readonly Property<SparePartInboundType> InboundTypeProperty = P<SparePartStore>.Register(e => e.InboundType);

        /// <summary>
        /// 入库类型
        /// </summary>
        public SparePartInboundType InboundType
        {
            get { return GetProperty(InboundTypeProperty); }
            set { SetProperty(InboundTypeProperty, value); }
        }
        #endregion

        #region 接收单号 ReceiveNo
        /// <summary>
        /// 接收单号
        /// </summary>
        [Label("接收单号")]
        public static readonly Property<string> ReceiveNoProperty = P<SparePartStore>.Register(e => e.ReceiveNo);

        /// <summary>
        /// 接收单号
        /// </summary>
        public string ReceiveNo
        {
            get { return GetProperty(ReceiveNoProperty); }
            set { SetProperty(ReceiveNoProperty, value); }
        }
        #endregion

        #region 验收单号 AcceptanceNo
        /// <summary>
        /// 验收单号
        /// </summary>
        [Label("验收单号")]
        public static readonly Property<string> AcceptanceNoProperty = P<SparePartStore>.Register(e => e.AcceptanceNo);

        /// <summary>
        /// 验收单号
        /// </summary>
        public string AcceptanceNo
        {
            get { return GetProperty(AcceptanceNoProperty); }
            set { SetProperty(AcceptanceNoProperty, value); }
        }
        #endregion

        #region 处置单号 DisposalNo
        /// <summary>
        /// 处置单号
        /// </summary>
        [Label("处置单号")]
        public static readonly Property<string> DisposalNoProperty = P<SparePartStore>.Register(e => e.DisposalNo);

        /// <summary>
        /// 处置单号
        /// </summary>
        public string DisposalNo
        {
            get { return this.GetProperty(DisposalNoProperty); }
            set { this.SetProperty(DisposalNoProperty, value); }
        }
        #endregion

        #region 相关单号 LinkCode
        /// <summary>
        /// 相关单号
        /// </summary>
        [Label("相关单号")]
        public static readonly Property<string> LinkCodeProperty = P<SparePartStore>.Register(e => e.LinkCode);

        /// <summary>
        /// 相关单号
        /// </summary>
        public string LinkCode
        {
            get { return GetProperty(LinkCodeProperty); }
            set { SetProperty(LinkCodeProperty, value); }
        }
        #endregion

        #region 入库状态 InboundStatus
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InboundStatus> InboundStatusProperty
            = P<SparePartStore>.Register(e => e.InboundStatus);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InboundStatus InboundStatus
        {
            get { return GetProperty(InboundStatusProperty); }
            set { SetProperty(InboundStatusProperty, value); }
        }
        #endregion

        #region 入库日期 StoreDateTime
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("入库日期")]
        public static readonly Property<DateTime?> StoreDateTimeProperty = P<SparePartStore>.Register(e => e.StoreDateTime);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? StoreDateTime
        {
            get { return GetProperty(StoreDateTimeProperty); }
            set { SetProperty(StoreDateTimeProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<SparePartStore>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SparePartStore>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 入库人员 WarehouseOperator
        /// <summary>
        /// 入库人员Id
        /// </summary>
        [Label("入库人员")]
        public static readonly IRefIdProperty WarehouseOperatorIdProperty =
            P<SparePartStore>.RegisterRefId(e => e.WarehouseOperatorId, ReferenceType.Normal);

        /// <summary>
        /// 入库人员Id
        /// </summary>
        public double? WarehouseOperatorId
        {
            get { return (double?)this.GetRefNullableId(WarehouseOperatorIdProperty); }
            set { this.SetRefNullableId(WarehouseOperatorIdProperty, value); }
        }

        /// <summary>
        /// 入库人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> WarehouseOperatorProperty =
            P<SparePartStore>.RegisterRef(e => e.WarehouseOperator, WarehouseOperatorIdProperty);

        /// <summary>
        /// 入库人员
        /// </summary>
        public Employee WarehouseOperator
        {
            get { return this.GetRefEntity(WarehouseOperatorProperty); }
            set { this.SetRefEntity(WarehouseOperatorProperty, value); }
        }
        #endregion

        #region 入库仓库 Warehouse
        /// <summary>
        /// 入库仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<SparePartStore>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 入库仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 入库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<SparePartStore>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 入库仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 入库明细列表 StoreDetailList
        /// <summary>
        /// 入库明细列表
        /// </summary>
        [Label("入库明细")]
        public static readonly ListProperty<EntityList<StoreDetail>> StoreDetailListProperty = P<SparePartStore>.RegisterList(e => e.StoreDetailList);

        /// <summary>
        /// 入库明细列表
        /// </summary>
        public EntityList<StoreDetail> StoreDetailList
        {
            get { return this.GetLazyList(StoreDetailListProperty); }
        }
        #endregion

        #region 视图属性
        #region 入库仓库 WarehouseName
        /// <summary>
        /// 入库仓库
        /// </summary>
        [Label("入库仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<SparePartStore>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 入库仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<SparePartStore>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return GetProperty(SupplierCodeProperty); }
            set { SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<SparePartStore>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return GetProperty(SupplierNameProperty); }
            set { SetProperty(SupplierNameProperty, value); }
        }
        #endregion
        #endregion

        #region 不映射数据库的属性

        #region 入库明细查询关键字 StoreDetailKeyWord
        /// <summary>
        /// 入库明细查询关键字
        /// </summary>
        [Label("入库明细查询关键字")]
        public static readonly Property<string> StoreDetailKeyWordProperty = P<SparePartStore>.Register(e => e.StoreDetailKeyWord);

        /// <summary>
        /// 入库明细查询关键字
        /// </summary>
        public string StoreDetailKeyWord
        {
            get { return this.GetProperty(StoreDetailKeyWordProperty); }
            set { this.SetProperty(StoreDetailKeyWordProperty, value); }
        }
        #endregion

        #region 拆机件/原件 StorePartType
        /// <summary>
        /// 拆机件/原件
        /// </summary>
        [Label("拆机件/原件")]
        public static readonly Property<StorePartType?> StorePartTypeProperty = P<SparePartStore>.Register(e => e.StorePartType);

        /// <summary>
        /// 拆机件/原件
        /// </summary>
        public StorePartType? StorePartType
        {
            get { return this.GetProperty(StorePartTypeProperty); }
            set { this.SetProperty(StorePartTypeProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<SparePartStore>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefNullableId(SparePartIdProperty); }
            set { this.SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<SparePartStore>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartStore>.Register(e => e.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartStore>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePartStore>.Register(e => e.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 以旧换新 IsReplacement
        /// <summary>
        /// 以旧换新
        /// </summary>
        [Label("以旧换新")]
        public static readonly Property<bool> IsReplacementProperty = P<SparePartStore>.Register(e => e.IsReplacement);

        /// <summary>
        /// 以旧换新
        /// </summary>
        public bool IsReplacement
        {
            get { return this.GetProperty(IsReplacementProperty); }
            set { this.SetProperty(IsReplacementProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<QualityStatus?> QualityStatusProperty = P<SparePartStore>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QualityStatus? QualityStatus
        {
            get { return GetProperty(QualityStatusProperty); }
            set { SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 数量 Number
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> NumberProperty = P<SparePartStore>.Register(e => e.Number);
        /// <summary>
        /// 数量
        /// </summary>
        public int Number
        {
            get { return GetProperty(NumberProperty); }
            set { SetProperty(NumberProperty, value); }
        }
        #endregion

        #region 单价 UnitPrice
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal?> UnitPriceProperty = P<SparePartStore>.Register(e => e.UnitPrice);
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice
        {
            get { return GetProperty(UnitPriceProperty); }
            set { SetProperty(UnitPriceProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<SparePartStore>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
            P<SparePartStore>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 出库单号-行号 PartOutDepotDetail
        /// <summary>
        /// 出库单号-行号Id
        /// </summary>
        [Label("出库单号-行号")]
        public static readonly IRefIdProperty PartOutDepotDetailIdProperty =
            P<SparePartStore>.RegisterRefId(e => e.PartOutDepotDetailId, ReferenceType.Normal);

        /// <summary>
        /// 出库单号-行号Id
        /// </summary>
        public double? PartOutDepotDetailId
        {
            get { return (double?)this.GetRefNullableId(PartOutDepotDetailIdProperty); }
            set { this.SetRefNullableId(PartOutDepotDetailIdProperty, value); }
        }

        /// <summary>
        /// 出库单号-行号
        /// </summary>
        public static readonly RefEntityProperty<PartOutDepotDetail> PartOutDepotDetailProperty =
            P<SparePartStore>.RegisterRef(e => e.PartOutDepotDetail, PartOutDepotDetailIdProperty);

        /// <summary>
        /// 出库单号-行号
        /// </summary>
        public PartOutDepotDetail PartOutDepotDetail
        {
            get { return this.GetRefEntity(PartOutDepotDetailProperty); }
            set { this.SetRefEntity(PartOutDepotDetailProperty, value); }
        }
        #endregion

        #region 出库可退数量 CanReturnQty
        /// <summary>
        /// 出库可退数量
        /// </summary>
        [Label("出库单可退数量")]
        public static readonly Property<int> CanReturnQtyProperty = P<SparePartStore>.Register(e => e.CanReturnQty);
        /// <summary>
        /// 出库单可退数量
        /// </summary>
        public int CanReturnQty
        {
            get { return GetProperty(CanReturnQtyProperty); }
            set { SetProperty(CanReturnQtyProperty, value); }
        }
        #endregion

        #region 是否生成新标签 IsCreateNewLabel
        /// <summary>
        /// 是否生成新标签
        /// </summary>
        [Label("是否生成新标签")]
        public static readonly Property<bool> IsCreateNewLabelProperty = P<SparePartStore>.Register(e => e.IsCreateNewLabel);

        /// <summary>
        /// 是否生成新标签
        /// </summary>
        public bool IsCreateNewLabel
        {
            get { return this.GetProperty(IsCreateNewLabelProperty); }
            set { this.SetProperty(IsCreateNewLabelProperty, value); }
        }
        #endregion

        #region 扫描值 ScanValue
        /// <summary>
        /// 扫描值
        /// </summary>
        [Label("扫描值")]
        public static readonly Property<string> ScanValueProperty = P<SparePartStore>.Register(e => e.ScanValue);

        /// <summary>
        /// 扫描值
        /// </summary>
        public string ScanValue
        {
            get { return this.GetProperty(ScanValueProperty); }
            set { this.SetProperty(ScanValueProperty, value); }
        }
        #endregion

        #region 提示消息 Message
        /// <summary>
        /// 提示消息
        /// </summary>
        [Label("提示消息")]
        public static readonly Property<string> MessageProperty = P<SparePartStore>.Register(e => e.Message);

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message
        {
            get { return this.GetProperty(MessageProperty); }
            set { this.SetProperty(MessageProperty, value); }
        }
        #endregion

        #region 是否存在明细 IsExistDetail
        /// <summary>
        /// 是否存在明细
        /// </summary>
        [Label("是否存在明细")]
        public static readonly Property<bool> IsExistDetailProperty = P<SparePartStore>.Register(e => e.IsExistDetail);

        /// <summary>
        /// 是否存在明细
        /// </summary>
        public bool IsExistDetail
        {
            get { return this.GetProperty(IsExistDetailProperty); }
            set { this.SetProperty(IsExistDetailProperty, value); }
        }
        #endregion

        #region 是否手动选择备件 IsSelectSparePart
        /// <summary>
        /// 是否手动选择备件
        /// </summary>
        [Label("是否手动选择备件")]
        public static readonly Property<bool> IsSelectSparePartProperty = P<SparePartStore>.Register(e => e.IsSelectSparePart);

        /// <summary>
        /// 是否手动选择备件
        /// </summary>
        public bool IsSelectSparePart
        {
            get { return this.GetProperty(IsSelectSparePartProperty); }
            set { this.SetProperty(IsSelectSparePartProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 备件入库 实体配置
    /// </summary>
    internal class SparePartStoreConfig : EntityConfig<SparePartStore>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_STR").MapAllProperties();
            Meta.Property(SparePartStore.StoreDetailKeyWordProperty).DontMapColumn();
            Meta.Property(SparePartStore.StorePartTypeProperty).DontMapColumn();
            Meta.Property(SparePartStore.SparePartIdProperty).DontMapColumn();
            Meta.Property(SparePartStore.SparePartProperty).DontMapColumn();
            Meta.Property(SparePartStore.SparePartCodeProperty).DontMapColumn();
            Meta.Property(SparePartStore.SparePartNameProperty).DontMapColumn();
            Meta.Property(SparePartStore.ControlMethodProperty).DontMapColumn();
            Meta.Property(SparePartStore.IsReplacementProperty).DontMapColumn();
            Meta.Property(SparePartStore.QualityStatusProperty).DontMapColumn();
            Meta.Property(SparePartStore.NumberProperty).DontMapColumn();
            Meta.Property(SparePartStore.UnitPriceProperty).DontMapColumn();
            Meta.Property(SparePartStore.PartOutDepotDetailIdProperty).DontMapColumn();
            Meta.Property(SparePartStore.PartOutDepotDetailProperty).DontMapColumn();
            Meta.Property(SparePartStore.CanReturnQtyProperty).DontMapColumn();
            Meta.Property(SparePartStore.StorageLocationIdProperty).DontMapColumn();
            Meta.Property(SparePartStore.StorageLocationProperty).DontMapColumn();
            Meta.Property(SparePartStore.IsCreateNewLabelProperty).DontMapColumn();
            Meta.Property(SparePartStore.ScanValueProperty).DontMapColumn();
            Meta.Property(SparePartStore.MessageProperty).DontMapColumn();
            Meta.Property(SparePartStore.IsExistDetailProperty).DontMapColumn();
            Meta.Property(SparePartStore.IsSelectSparePartProperty).DontMapColumn();
            Meta.Property(SparePartStore.LinkCodeProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
