using SIE.Core.Equipments.FixedAssets;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 序列号明细
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(OrderNumberCode))]
    [Label("序列号明细")]
    public partial class StoreSummaryDetail : DataEntity
    {
        #region 备件库存 StoreSummary
        /// <summary>
        /// 备件库存Id
        /// </summary>
        public static readonly IRefIdProperty StoreSummaryIdProperty = P<StoreSummaryDetail>.RegisterRefId(e => e.StoreSummaryId, ReferenceType.Parent);

        /// <summary>
        /// 备件库存Id
        /// </summary>
        public double StoreSummaryId
        {
            get { return (double)GetRefId(StoreSummaryIdProperty); }
            set { SetRefId(StoreSummaryIdProperty, value); }
        }

        /// <summary>
        /// 备件库存
        /// </summary>
        public static readonly RefEntityProperty<StoreSummary> StoreSummaryProperty = P<StoreSummaryDetail>.RegisterRef(e => e.StoreSummary, StoreSummaryIdProperty);

        /// <summary>
        /// 备件库存
        /// </summary>
        public StoreSummary StoreSummary
        {
            get { return GetRefEntity(StoreSummaryProperty); }
            set { SetRefEntity(StoreSummaryProperty, value); }
        }
        #endregion

        #region 序列号编码 OrderNumberCode
        /// <summary>
        /// 序列号编码
        /// </summary>
        [Required]
        [Label("序列号")]
        public static readonly Property<string> OrderNumberCodeProperty = P<StoreSummaryDetail>.Register(e => e.OrderNumberCode);

        /// <summary>
        /// 序列号编码
        /// </summary>
        public string OrderNumberCode
        {
            get { return GetProperty(OrderNumberCodeProperty); }
            set { SetProperty(OrderNumberCodeProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSn
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSnProperty = P<StoreSummaryDetail>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion

        #region 状态 OdNbStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OdNbStatus> OdNbStatusProperty = P<StoreSummaryDetail>.Register(e => e.OdNbStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public OdNbStatus OdNbStatus
        {
            get { return GetProperty(OdNbStatusProperty); }
            set { SetProperty(OdNbStatusProperty, value); }
        }
        #endregion

        #region 库存状态 StoreStatus
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OrdNumStoreStatus> StoreStatusProperty = P<StoreSummaryDetail>.Register(e => e.StoreStatus);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OrdNumStoreStatus StoreStatus
        {
            get { return GetProperty(StoreStatusProperty); }
            set { SetProperty(StoreStatusProperty, value); }
        }
        #endregion

        #region 数量 Number
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> NumberProperty = P<StoreSummaryDetail>.Register(e => e.Number);
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
        public static readonly Property<decimal?> UnitPriceProperty = P<StoreSummaryDetail>.Register(e => e.UnitPrice);
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice
        {
            get { return GetProperty(UnitPriceProperty); }
            set { SetProperty(UnitPriceProperty, value); }
        }
        #endregion

        #region 入库单号 StoreCode
        /// <summary>
        /// 入库单号
        /// </summary>
        [Label("入库单号")]
        public static readonly Property<string> StoreCodeProperty = P<StoreSummaryDetail>.Register(e => e.StoreCode);

        /// <summary>
        /// 入库单号
        /// </summary>
        public string StoreCode
        {
            get { return GetProperty(StoreCodeProperty); }
            set { SetProperty(StoreCodeProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商编码")]
        public static readonly IRefIdProperty SupplierIdProperty = P<StoreSummaryDetail>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefId(SupplierIdProperty); }
            set { SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<StoreSummaryDetail>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 相关单据 LinkCode
        /// <summary>
        /// 相关单据
        /// </summary>
        [Label("相关单据")]
        public static readonly Property<string> LinkCodeProperty = P<StoreSummaryDetail>.Register(e => e.LinkCode);

        /// <summary>
        /// 相关单据
        /// </summary>
        public string LinkCode
        {
            get { return GetProperty(LinkCodeProperty); }
            set { SetProperty(LinkCodeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库编码")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<StoreSummaryDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<StoreSummaryDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        [Label("库位编码")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StoreSummaryDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StoreSummaryDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 入库日期 InboundDate
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("原始入库日期")]
        public static readonly Property<DateTime?> InboundDateProperty = P<StoreSummaryDetail>.Register(e => e.InboundDate);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? InboundDate
        {
            get { return GetProperty(InboundDateProperty); }
            set { SetProperty(InboundDateProperty, value); }
        }
        #endregion

        #region 不良品数 RotNumber
        /// <summary>
        /// 不良品数
        /// </summary>
        [Label("不良品数")]
        public static readonly Property<int> RotNumberProperty = P<StoreSummaryDetail>.Register(e => e.RotNumber);

        /// <summary>
        /// 不良品数
        /// </summary>
        public int RotNumber
        {
            get { return GetProperty(RotNumberProperty); }
            set { SetProperty(RotNumberProperty, value); }
        }
        #endregion

        #region 可用库存 GoodNumber
        /// <summary>
        /// 可用库存
        /// </summary>
        [Label("可用库存")]
        public static readonly Property<int> GoodNumberProperty = P<StoreSummaryDetail>.Register(e => e.GoodNumber);

        /// <summary>
        /// 可用库存
        /// </summary>
        public int GoodNumber
        {
            get { return GetProperty(GoodNumberProperty); }
            set { SetProperty(GoodNumberProperty, value); }
        }
        #endregion

        #region 总库存 SumNumber
        /// <summary>
        /// 总库存
        /// </summary>
        [Label("总库存")]
        public static readonly Property<int> SumNumberProperty = P<StoreSummaryDetail>.Register(e => e.SumNumber);

        /// <summary>
        /// 总库存
        /// </summary>
        public int SumNumber
        {
            get { return GetProperty(SumNumberProperty); }
            set { SetProperty(SumNumberProperty, value); }
        }
        #endregion

        #region 固定资产 FixedAssetsAccount
        /// <summary>
        /// 固定资产Id
        /// </summary>
        [Label("固定资产")]
        public static readonly IRefIdProperty FixedAssetsAccountIdProperty = P<StoreSummaryDetail>.RegisterRefId(e => e.FixedAssetsAccountId, ReferenceType.Normal);

        /// <summary>
        /// 固定资产Id
        /// </summary>
        public double? FixedAssetsAccountId
        {
            get { return (double?)GetRefNullableId(FixedAssetsAccountIdProperty); }
            set { SetRefNullableId(FixedAssetsAccountIdProperty, value); }
        }

        /// <summary>
        /// 固定资产
        /// </summary>
        public static readonly RefEntityProperty<FixedAssetsAccount> FixedAssetsAccountProperty = P<StoreSummaryDetail>.RegisterRef(e => e.FixedAssetsAccount, FixedAssetsAccountIdProperty);

        /// <summary>
        /// 固定资产
        /// </summary>
        public FixedAssetsAccount FixedAssetsAccount
        {
            get { return GetRefEntity(FixedAssetsAccountProperty); }
            set { SetRefEntity(FixedAssetsAccountProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 备件Id SparePartId
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件Id")]
        public static readonly Property<double> SparePartIdProperty = P<StoreSummaryDetail>.RegisterView(e => e.SparePartId, p => p.StoreSummary.SparePartId);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
        {
            get { return this.GetProperty(SparePartIdProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<StoreSummaryDetail>.RegisterView(e => e.SparePartCode, p => p.StoreSummary.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<StoreSummaryDetail>.RegisterView(e => e.SparePartName, p => p.StoreSummary.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion


        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<StoreSummaryDetail>.RegisterView(e => e.ControlMethod, p => p.StoreSummary.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion


        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<StoreSummaryDetail>.RegisterView(e => e.State, p => p.StoreSummary.SparePart.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<StoreSummaryDetail>.RegisterView(e => e.Specification, p => p.StoreSummary.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 备件类型 SpartType
        /// <summary>
        /// 备件类型
        /// </summary>
        [Label("备件类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<StoreSummaryDetail>.RegisterView(e => e.SpartType, p => p.StoreSummary.SparePart.SpartType);

        /// <summary>
        /// 备件类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<StoreSummaryDetail>.RegisterView(e => e.UnitName, p => p.StoreSummary.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<StoreSummaryDetail>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<StoreSummaryDetail>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return GetProperty(SupplierNameProperty); }
            set { SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<StoreSummaryDetail>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 分类 LibraryType
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<LibraryType> LibraryTypeProperty = P<StoreSummaryDetail>.RegisterView(e => e.LibraryType, p => p.Warehouse.LibraryType);

        /// <summary>
        /// 分类
        /// </summary>
        public LibraryType LibraryType
        {
            get { return this.GetProperty(LibraryTypeProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<StoreSummaryDetail>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 固定资产编码 FixedAssetsAccountCode
        /// <summary>
        /// 固定资产编码(视图属性)
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetsAccountCodeProperty
            = P<StoreSummaryDetail>.RegisterView(e => e.FixedAssetsAccountCode, p => p.FixedAssetsAccount.Code);

        /// <summary>
        /// 固定资产编码(视图属性)
        /// </summary>
        public string FixedAssetsAccountCode
        {
            get { return this.GetProperty(FixedAssetsAccountCodeProperty); }
        }
        #endregion

        #region 固定资产名称 FixedAssetsAccountName
        /// <summary>
        /// 固定资产名称(视图属性)
        /// </summary>
        [Label("固定资产名称")]
        public static readonly Property<string> FixedAssetsAccountNameProperty
            = P<StoreSummaryDetail>.RegisterView(e => e.FixedAssetsAccountName, p => p.FixedAssetsAccount.Name);

        /// <summary>
        /// 固定资产名称(视图属性)
        /// </summary>
        public string FixedAssetsAccountName
        {
            get { return this.GetProperty(FixedAssetsAccountNameProperty); }
        }
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<StoreSummaryDetail>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
        }
        #endregion
        
        #endregion
    }

    /// <summary>
    /// 备件库存明细查询 实体配置
    /// </summary>
    internal class StoreSummaryDetailConfig : EntityConfig<StoreSummaryDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_SUMR_DTA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
