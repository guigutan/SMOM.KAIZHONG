using SIE.Core.Equipments.FixedAssets;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures.Enums;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台账
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(FixtureAccount.Code))]
    //[CriteriaQuery]
    [Label("工治具台账")]
    public partial class FixtureAccount : DataEntity
    {
        #region 工治具ID Code
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("工治具ID")]
        public static readonly Property<string> CodeProperty = P<FixtureAccount>.Register(e => e.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 总数量 TotalQty
        /// <summary>
        /// 总数量
        /// </summary>
        [Label("总数量")]
        public static readonly Property<int> TotalQtyProperty = P<FixtureAccount>.Register(e => e.TotalQty);

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalQty
        {
            get { return GetProperty(TotalQtyProperty); }
            set { SetProperty(TotalQtyProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSN
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSNProperty = P<FixtureAccount>.Register(e => e.OriginalSN);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSN
        {
            get { return GetProperty(OriginalSNProperty); }
            set { SetProperty(OriginalSNProperty, value); }
        }
        #endregion

        #region 资产编码 AssetCode
        /// <summary>
        /// 资产编码
        /// </summary>
        [Label("资产编码")]
        public static readonly Property<string> AssetCodeProperty = P<FixtureAccount>.Register(e => e.AssetCode);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string AssetCode
        {
            get { return GetProperty(AssetCodeProperty); }
            set { SetProperty(AssetCodeProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<FixtureAccount>.Register(e => e.ProductionDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return GetProperty(ProductionDateProperty); }
            set { SetProperty(ProductionDateProperty, value); }
        }
        #endregion

        #region 入库日期 InStorageDate
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("入库日期")]
        public static readonly Property<DateTime?> InStorageDateProperty = P<FixtureAccount>.Register(e => e.InStorageDate);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? InStorageDate
        {
            get { return GetProperty(InStorageDateProperty); }
            set { SetProperty(InStorageDateProperty, value); }
        }
        #endregion

        #region 厂商名称 Manufacturer
        /// <summary>
        /// 厂商名称
        /// </summary>
        [Label("厂商名称")]
        public static readonly Property<string> ManufacturerProperty = P<FixtureAccount>.Register(e => e.Manufacturer);

        /// <summary>
        /// 厂商名称
        /// </summary>
        public string Manufacturer
        {
            get { return GetProperty(ManufacturerProperty); }
            set { SetProperty(ManufacturerProperty, value); }
        }
        #endregion

        #region 总使用次数 TotalUseNum
        /// <summary>
        /// 总使用次数
        /// </summary>
        [Label("总使用次数")]
        public static readonly Property<decimal> TotalUseNumProperty = P<FixtureAccount>.Register(e => e.TotalUseNum);

        /// <summary>
        /// 总使用次数
        /// </summary>
        public decimal TotalUseNum
        {
            get { return GetProperty(TotalUseNumProperty); }
            set { SetProperty(TotalUseNumProperty, value); }
        }
        #endregion

        #region 总使用时数 TotalUseHour
        /// <summary>
        /// 总使用时数
        /// </summary>
        [Label("总使用时数")]
        public static readonly Property<decimal> TotalUseHourProperty = P<FixtureAccount>.Register(e => e.TotalUseHour);

        /// <summary>
        /// 总使用时数
        /// </summary>
        public decimal TotalUseHour
        {
            get { return GetProperty(TotalUseHourProperty); }
            set { SetProperty(TotalUseHourProperty, value); }
        }
        #endregion

        #region 保养后使用次数 MaintainedNum
        /// <summary>
        /// 保养后使用次数
        /// </summary>
        [Label("保养后使用次数")]
        public static readonly Property<decimal> MaintainedNumProperty = P<FixtureAccount>.Register(e => e.MaintainedNum);

        /// <summary>
        /// 保养后使用次数
        /// </summary>
        public decimal MaintainedNum
        {
            get { return GetProperty(MaintainedNumProperty); }
            set { SetProperty(MaintainedNumProperty, value); }
        }
        #endregion

        #region 保养后使用时长 MaintainedHour
        /// <summary>
        /// 保养后使用时长
        /// </summary>
        [Label("保养后使用时长")]
        public static readonly Property<decimal> MaintainedHourProperty = P<FixtureAccount>.Register(e => e.MaintainedHour);

        /// <summary>
        /// 保养后使用时长
        /// </summary>
        public decimal MaintainedHour
        {
            get { return GetProperty(MaintainedHourProperty); }
            set { SetProperty(MaintainedHourProperty, value); }
        }
        #endregion

        #region 待维修 WaitRepair
        /// <summary>
        /// 待维修
        /// </summary>
        [Label("待维修")]
        public static readonly Property<int> WaitRepairProperty = P<FixtureAccount>.Register(e => e.WaitRepair);

        /// <summary>
        /// 待维修
        /// </summary>
        public int WaitRepair
        {
            get { return GetProperty(WaitRepairProperty); }
            set { SetProperty(WaitRepairProperty, value); }
        }
        #endregion

        #region 待保养 WaitMaintain
        /// <summary>
        /// 待保养
        /// </summary>
        [Label("待保养")]
        public static readonly Property<int> WaitMaintainProperty = P<FixtureAccount>.Register(e => e.WaitMaintain);

        /// <summary>
        /// 待保养
        /// </summary>
        public int WaitMaintain
        {
            get { return GetProperty(WaitMaintainProperty); }
            set { SetProperty(WaitMaintainProperty, value); }
        }
        #endregion

        #region 待领用 WaitReceive
        /// <summary>
        /// 待领用
        /// </summary>
        [Label("待领用")]
        public static readonly Property<int> WaitReceiveProperty = P<FixtureAccount>.Register(e => e.WaitReceive);

        /// <summary>
        /// 待领用
        /// </summary>
        public int WaitReceive
        {
            get { return GetProperty(WaitReceiveProperty); }
            set { SetProperty(WaitReceiveProperty, value); }
        }
        #endregion

        #region 在库 InStockQty
        /// <summary>
        /// 在库
        /// </summary>
        [Label("在库")]
        public static readonly Property<int> InStockQtyProperty = P<FixtureAccount>.Register(e => e.InStockQty);

        /// <summary>
        /// 在库
        /// </summary>
        public int InStockQty
        {
            get { return GetProperty(InStockQtyProperty); }
            set { SetProperty(InStockQtyProperty, value); }
        }
        #endregion

        #region 待入库 WaitShelfQty
        /// <summary>
        /// 待入库
        /// </summary>
        [Label("待入库")]
        public static readonly Property<int> WaitShelfQtyProperty = P<FixtureAccount>.Register(e => e.WaitShelfQty);

        /// <summary>
        /// 待入库
        /// </summary>
        public int WaitShelfQty
        {
            get { return GetProperty(WaitShelfQtyProperty); }
            set { SetProperty(WaitShelfQtyProperty, value); }
        }
        #endregion

        #region 在线 OnlineQty
        /// <summary>
        /// 在线
        /// </summary>
        [Label("在线")]
        public static readonly Property<int> OnlineQtyProperty = P<FixtureAccount>.Register(e => e.OnlineQty);

        /// <summary>
        /// 在线
        /// </summary>
        public int OnlineQty
        {
            get { return GetProperty(OnlineQtyProperty); }
            set { SetProperty(OnlineQtyProperty, value); }
        }
        #endregion

        #region 合格 PassQty
        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        public static readonly Property<int> PassQtyProperty = P<FixtureAccount>.Register(e => e.PassQty);

        /// <summary>
        /// 合格
        /// </summary>
        public int PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格 NgQty
        /// <summary>
        /// 不合格
        /// </summary>
        [Label("不合格")]
        public static readonly Property<int> NgQtyProperty = P<FixtureAccount>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格
        /// </summary>
        public int NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 报废 ScrapQty
        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        public static readonly Property<int> ScrapQtyProperty = P<FixtureAccount>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废
        /// </summary>
        public int ScrapQty
        {
            get { return GetProperty(ScrapQtyProperty); }
            set { SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 待验收 ToAccepted
        /// <summary>
        /// 待验收
        /// </summary>
        [Label("待验收")]
        public static readonly Property<int> ToAcceptedProperty = P<FixtureAccount>.Register(e => e.ToAccepted);

        /// <summary>
        /// 待验收
        /// </summary>
        public int ToAccepted
        {
            get { return GetProperty(ToAcceptedProperty); }
            set { SetProperty(ToAcceptedProperty, value); }
        }
        #endregion

        #region 产权归属 Proprietorship
        /// <summary>
        /// 产权归属
        /// </summary>
        [Label("产权归属")]
        public static readonly Property<Proprietorship> ProprietorshipProperty
            = P<FixtureAccount>.Register(e => e.Proprietorship);

        /// <summary>
        /// 产权归属
        /// </summary>
        public Proprietorship Proprietorship
        {
            get { return GetProperty(ProprietorshipProperty); }
            set { SetProperty(ProprietorshipProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureAccount>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureAccount>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<FixtureAccount>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<FixtureAccount>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 状态 AccountState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<FixtureAccountState?> AccountStateProperty = P<FixtureAccount>.Register(e => e.AccountState);

        /// <summary>
        /// 状态
        /// </summary>
        public FixtureAccountState? AccountState
        {
            get { return GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<FixtureAccount>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<FixtureAccount>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 质量状态 QualityState
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<FixtureQualityState?> QualityStateProperty = P<FixtureAccount>.Register(e => e.QualityState);

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState? QualityState
        {
            get { return GetProperty(QualityStateProperty); }
            set { SetProperty(QualityStateProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse（用于ID类台账固定储位）
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<FixtureAccount>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<FixtureAccount>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 Location（用于ID类台账固定储位）
        /// <summary>
        /// 库位Id
        /// </summary>
        public static readonly IRefIdProperty LocationIdProperty = P<FixtureAccount>.RegisterRefId(e => e.LocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? LocationId
        {
            get { return (double?)GetRefNullableId(LocationIdProperty); }
            set { SetRefNullableId(LocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> LocationProperty = P<FixtureAccount>.RegisterRef(e => e.Location, LocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation Location
        {
            get { return GetRefEntity(LocationProperty); }
            set { SetRefEntity(LocationProperty, value); }
        }
        #endregion

        #region RFID Rfid
        /// <summary>
        /// RFID
        /// </summary>
        [Label("RFID")]
        public static readonly Property<string> RfidProperty = P<FixtureAccount>.Register(e => e.Rfid);

        /// <summary>
        /// RFID
        /// </summary>
        public string Rfid
        {
            get { return this.GetProperty(RfidProperty); }
            set { this.SetProperty(RfidProperty, value); }
        }
        #endregion

        #region 固定资产 FixedAssetsAccount
        /// <summary>
        /// 固定资产Id
        /// </summary>
        [Label("固定资产")]
        public static readonly IRefIdProperty FixedAssetsAccountIdProperty = P<FixtureAccount>.RegisterRefId(e => e.FixedAssetsAccountId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixedAssetsAccount> FixedAssetsAccountProperty = P<FixtureAccount>.RegisterRef(e => e.FixedAssetsAccount, FixedAssetsAccountIdProperty);

        /// <summary>
        /// 固定资产
        /// </summary>
        public FixedAssetsAccount FixedAssetsAccount
        {
            get { return GetRefEntity(FixedAssetsAccountProperty); }
            set { SetRefEntity(FixedAssetsAccountProperty, value); }
        }
        #endregion

        #region 库存详情 StockList
        /// <summary>
        /// 库存详情
        /// </summary>
        [Label("库存详情")]
        public static readonly ListProperty<EntityList<FixtureAccountStock>> StockListProperty = P<FixtureAccount>.RegisterList(e => e.StockList);
        /// <summary>
        /// 库存详情
        /// </summary>
        public EntityList<FixtureAccountStock> StockList
        {
            get { return this.GetLazyList(StockListProperty); }
            
        }
        #endregion

        #region feeder详情 ToolList
        /// <summary>
        /// feeder详情
        /// </summary>
        [Label("feeder详情")]
        public static readonly ListProperty<EntityList<FixtureAccountTool>> ToolListProperty = P<FixtureAccount>.RegisterList(e => e.ToolList);
        /// <summary>
        /// feeder详情
        /// </summary>
        public EntityList<FixtureAccountTool> ToolList
        {
            get { return this.GetLazyList(ToolListProperty); }
        }
        #endregion

        #region 使用履历 UseResumeList
        /// <summary>
        /// 使用履历
        /// </summary>
        [Label("使用履历")]
        public static readonly ListProperty<EntityList<FixtureAccountUseResume>> UseResumeListProperty = P<FixtureAccount>.RegisterList(e => e.UseResumeList);
        /// <summary>
        /// 使用履历
        /// </summary>
        public EntityList<FixtureAccountUseResume> UseResumeList
        {
            get { return this.GetLazyList(UseResumeListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<FixtureAccount>.RegisterView(e => e.EncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureAccount>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureAccount>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 工治具类型编码 FixtureTypeCode
        /// <summary>
        /// 工治具类型编码
        /// </summary>
        [Label("工治具类型编码")]
        public static readonly Property<string> FixtureTypeCodeProperty = P<FixtureAccount>.RegisterView(e => e.FixtureTypeCode, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型编码
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureAccount>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double? FixtureTypeId
        {
            get { return (double?)this.GetRefNullableId(FixtureTypeIdProperty); }
            set { this.SetRefNullableId(FixtureTypeIdProperty, value); }
        }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public static readonly RefEntityProperty<FixtureType> FixtureTypeProperty =
            P<FixtureAccount>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 槽位类型 SlotType
        /// <summary>
        /// 槽位类型
        /// </summary>
        [Label("槽位类型")]
        public static readonly Property<SlotType> SlotTypeProperty = P<FixtureAccount>.RegisterView(e => e.SlotType, p => p.FixtureEncode.FixtureModel.SlotType);

        /// <summary>
        /// 槽位类型
        /// </summary>
        public SlotType SlotType
        {
            get { return this.GetProperty(SlotTypeProperty); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureAccount>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }
        #endregion

        #region 固定货位 FixedStorage
        /// <summary>
        /// 固定货位
        /// </summary>
        [Label("固定货位")]
        public static readonly Property<YesNo> FixedStorageProperty = P<FixtureAccount>.RegisterView(e => e.FixedStorage, p => p.FixtureEncode.FixtureModel.FixedStorage);

        /// <summary>
        /// 固定货位
        /// </summary>
        public YesNo FixedStorage
        {
            get { return this.GetProperty(FixedStorageProperty); }
        }
        #endregion

        #region 上料管理 LoadingManage
        /// <summary>
        /// 上料管理
        /// </summary>
        [Label("上料管理")]
        public static readonly Property<YesNo> LoadingManageProperty = P<FixtureAccount>.RegisterView(e => e.LoadingManage, p => p.FixtureEncode.FixtureModel.LoadingManage);

        /// <summary>
        /// 上料管理
        /// </summary>
        public YesNo LoadingManage
        {
            get { return this.GetProperty(LoadingManageProperty); }
        }
        #endregion

        #region 绑定产品 BindProduct
        /// <summary>
        /// 绑定产品
        /// </summary>
        [Label("绑定产品")]
        public static readonly Property<YesNo> BindProductProperty = P<FixtureAccount>.RegisterView(e => e.BindProduct, p => p.FixtureEncode.FixtureModel.BindProduct);

        /// <summary>
        /// 绑定产品
        /// </summary>
        public YesNo BindProduct
        {
            get { return this.GetProperty(BindProductProperty); }
        }
        #endregion

        #region 绑定设备 BindEquip
        /// <summary>
        /// 绑定设备
        /// </summary>
        [Label("绑定设备")]
        public static readonly Property<YesNo> BindEquipProperty = P<FixtureAccount>.RegisterView(e => e.BindEquip, p => p.FixtureEncode.FixtureModel.BindEquip);

        /// <summary>
        /// 绑定设备
        /// </summary>
        public YesNo BindEquip
        {
            get { return this.GetProperty(BindEquipProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<FixtureAccount>.RegisterView(e => e.UnitName, p => p.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 最大使用次数 MaxUseNum
        /// <summary>
        /// 最大使用次数
        /// </summary>
        [Label("最大使用次数")]
        public static readonly Property<int> MaxUseNumProperty = P<FixtureAccount>.RegisterView(e => e.MaxUseNum, p => p.FixtureEncode.FixtureModel.MaxUseNum);

        /// <summary>
        /// 最大使用次数
        /// </summary>
        public int MaxUseNum
        {
            get { return this.GetProperty(MaxUseNumProperty); }
        }
        #endregion

        #region 最大使用小时数 MaxUseHour
        /// <summary>
        /// 最大使用小时数
        /// </summary>
        [Label("最大使用小时数")]
        public static readonly Property<int> MaxUseHourProperty = P<FixtureAccount>.RegisterView(e => e.MaxUseHour, p => p.FixtureEncode.FixtureModel.MaxUseHour);

        /// <summary>
        /// 最大使用小时数
        /// </summary>
        public int MaxUseHour
        {
            get { return this.GetProperty(MaxUseHourProperty); }
        }
        #endregion

        #region 保养标准（次数） MaintainNum
        /// <summary>
        /// 保养标准（次数）
        /// </summary>
        [Label("保养标准（次数）")]
        public static readonly Property<int> MaintainNumProperty = P<FixtureAccount>.RegisterView(e => e.MaintainNum, p => p.FixtureEncode.FixtureModel.MaintainNum);

        /// <summary>
        /// 保养标准（次数）
        /// </summary>
        public int MaintainNum
        {
            get { return this.GetProperty(MaintainNumProperty); }
        }
        #endregion

        #region 保养标准（小时） MaintainHour
        /// <summary>
        /// 保养标准（小时）
        /// </summary>
        [Label("保养标准（小时）")]
        public static readonly Property<decimal> MaintainHourProperty = P<FixtureAccount>.RegisterView(e => e.MaintainHour, p => p.FixtureEncode.FixtureModel.MaintainHour);

        /// <summary>
        /// 保养标准（小时）
        /// </summary>
        public decimal MaintainHour
        {
            get { return this.GetProperty(MaintainHourProperty); }
        }
        #endregion

        #region 预警值（次数） WarnNum
        /// <summary>
        /// 预警值（次数）
        /// </summary>
        [Label("预警值（次数）")]
        public static readonly Property<int> WarnNumProperty = P<FixtureAccount>.RegisterView(e => e.WarnNum, p => p.FixtureEncode.FixtureModel.WarnNum);

        /// <summary>
        /// 预警值（次数）
        /// </summary>
        public int WarnNum
        {
            get { return this.GetProperty(WarnNumProperty); }
        }
        #endregion

        #region 预警值（小时） WarnHour
        /// <summary>
        /// 预警值（小时）
        /// </summary>
        [Label("预警值（小时）")]
        public static readonly Property<decimal> WarnHourProperty = P<FixtureAccount>.RegisterView(e => e.WarnHour, p => p.FixtureEncode.FixtureModel.WarnHour);

        /// <summary>
        /// 预警值（小时）
        /// </summary>
        public decimal WarnHour
        {
            get { return this.GetProperty(WarnHourProperty); }
        }
        #endregion

        #region 上线定期保养标准(小时) OnlineHour
        /// <summary>
        /// 上线定期保养标准(小时)
        /// </summary>
        [Label("上线定期保养标准(小时)")]
        public static readonly Property<decimal> OnlineHourProperty = P<FixtureAccount>.RegisterView(e => e.OnlineHour, p => p.FixtureEncode.FixtureModel.OnlineHour);

        /// <summary>
        /// 上线定期保养标准(小时)
        /// </summary>
        public decimal OnlineHour
        {
            get { return this.GetProperty(OnlineHourProperty); }
        }
        #endregion

        #region 保养强制执行 MaintainEnforce
        /// <summary>
        /// 保养强制执行
        /// </summary>
        [Label("保养强制执行")]
        public static readonly Property<bool> MaintainEnforceProperty = P<FixtureAccount>.RegisterView(e => e.MaintainEnforce, p => p.FixtureEncode.FixtureModel.MaintainEnforce);

        /// <summary>
        /// 保养强制执行
        /// </summary>
        public bool MaintainEnforce
        {
            get { return this.GetProperty(MaintainEnforceProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<FixtureAccount>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
            set { this.SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<FixtureAccount>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
            set { this.SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<FixtureAccount>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
            set { this.SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<FixtureAccount>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { this.SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<FixtureAccount>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<FixtureAccount>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 库位编码 LocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> LocationCodeProperty = P<FixtureAccount>.RegisterView(e => e.LocationCode, p => p.Location.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode
        {
            get { return this.GetProperty(LocationCodeProperty); }
            set { this.SetProperty(LocationCodeProperty, value); }
        }
        #endregion

        #region 库位名称 LocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> LocationNameProperty = P<FixtureAccount>.RegisterView(e => e.LocationName, p => p.Location.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { this.SetProperty(LocationNameProperty, value); }
        }
        #endregion

        #region 行业属性 IndustryProperties
        /// <summary>
        /// 行业属性
        /// </summary>
        [Label("行业属性")]
        public static readonly Property<IndustryProperties> IndustryPropertiesProperty = P<FixtureAccount>.RegisterView(e => e.IndustryProperties, p => p.FixtureEncode.FixtureModel.IndustryProperties);

        /// <summary>
        /// 行业属性
        /// </summary>
        public IndustryProperties IndustryProperties
        {
            get { return this.GetProperty(IndustryPropertiesProperty); }
        }
        #endregion

        #region 固定资产编码 FixedAssetsAccountCode
        /// <summary>
        /// 固定资产编码(视图属性)
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetsAccountCodeProperty
            = P<FixtureAccount>.RegisterView(e => e.FixedAssetsAccountCode, p => p.FixedAssetsAccount.Code);

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
            = P<FixtureAccount>.RegisterView(e => e.FixedAssetsAccountName, p => p.FixedAssetsAccount.Name);

        /// <summary>
        /// 固定资产名称(视图属性)
        /// </summary>
        public string FixedAssetsAccountName
        {
            get { return this.GetProperty(FixedAssetsAccountNameProperty); }
        }
        #endregion

        #region 原值(视图属性) OriginalAssetsValue
        /// <summary>
        /// 原值(视图属性)
        /// </summary>
        [Label("原值(视图属性)")]
        public static readonly Property<decimal> OriginalAssetsValueProperty
            = P<FixtureAccount>.RegisterView(e => e.OriginalAssetsValue, p => p.FixedAssetsAccount.OriginalAssetsValue);

        /// <summary>
        /// 原值(视图属性)
        /// </summary>
        public decimal OriginalAssetsValue
        {
            get { return this.GetProperty(OriginalAssetsValueProperty); }
            set { SetProperty(OriginalAssetsValueProperty, value); }
        }
        #endregion

        #region 净值(视图属性) NetAssetValue
        /// <summary>
        /// 净值(视图属性)
        /// </summary>
        [Label("净值(视图属性)")]
        public static readonly Property<decimal> NetAssetValueProperty
            = P<FixtureAccount>.RegisterView(e => e.NetAssetValue, p => p.FixedAssetsAccount.NetAssetValue);

        /// <summary>
        /// 净值(视图属性)
        /// </summary>
        public decimal NetAssetValue
        {
            get { return this.GetProperty(NetAssetValueProperty); }
            set { SetProperty(NetAssetValueProperty, value); }
        }
        #endregion

        #region 残值(视图属性) DepreciationResidualValue
        /// <summary>
        /// 残值(视图属性)
        /// </summary>
        [Label("残值(视图属性)")]
        public static readonly Property<decimal> DepreciationResidualValueProperty
            = P<FixtureAccount>.RegisterView(e => e.DepreciationResidualValue, p => p.FixedAssetsAccount.DepreciationResidualValue);

        /// <summary>
        /// 残值(视图属性)
        /// </summary>
        public decimal DepreciationResidualValue
        {
            get { return this.GetProperty(DepreciationResidualValueProperty); }
            set { SetProperty(DepreciationResidualValueProperty, value); }
        }
        #endregion
        #endregion

        #region 不映射数据库的属性

        #region 报废类型 ScrapType
        /// <summary>
        /// 报废类型
        /// </summary>
        [Label("报废类型")]
        public static readonly Property<string> ScrapTypeProperty = P<FixtureAccount>.Register(e => e.ScrapType);

        /// <summary>
        /// 报废类型
        /// </summary>
        public string ScrapType
        {
            get { return GetProperty(ScrapTypeProperty); }
            set { SetProperty(ScrapTypeProperty, value); }
        }
        #endregion

        #region 报废原因 Reason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ReasonProperty = P<FixtureAccount>.Register(e => e.Reason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string Reason
        {
            get { return GetProperty(ReasonProperty); }
            set { SetProperty(ReasonProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工治具台账 实体配置
    /// </summary>
    internal class FixtureAccountConfig : EntityConfig<FixtureAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXTURE_ACC").MapAllProperties();
            Meta.Property(FixtureAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(FixtureAccount.ReasonProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
