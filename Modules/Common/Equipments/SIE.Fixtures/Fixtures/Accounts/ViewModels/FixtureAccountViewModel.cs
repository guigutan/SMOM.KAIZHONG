using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Models;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.Fixtures.Accounts.ViewModels
{
    /// <summary>
    /// 工治具台账ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具台账信息")]
    public class FixtureAccountViewModel : ViewModel
    {
        #region 工治具ID Code
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> CodeProperty = P<FixtureAccountViewModel>.Register(e => e.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> QtyProperty = P<FixtureAccountViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 状态 AccountState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<FixtureAccountState?> AccountStateProperty = P<FixtureAccountViewModel>.Register(e => e.AccountState);

        /// <summary>
        /// 状态
        /// </summary>
        public FixtureAccountState? AccountState
        {
            get { return GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSN
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSNProperty = P<FixtureAccountViewModel>.Register(e => e.OriginalSN);

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
        public static readonly Property<string> AssetCodeProperty = P<FixtureAccountViewModel>.Register(e => e.AssetCode);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string AssetCode
        {
            get { return GetProperty(AssetCodeProperty); }
            set { SetProperty(AssetCodeProperty, value); }
        }
        #endregion

        #region 产权归属 Proprietorship
        /// <summary>
        /// 产权归属
        /// </summary>
        [Label("产权归属")]
        public static readonly Property<Proprietorship?> ProprietorshipProperty
            = P<FixtureAccountViewModel>.Register(e => e.Proprietorship);

        /// <summary>
        /// 产权归属
        /// </summary>
        public Proprietorship? Proprietorship
        {
            get { return GetProperty(ProprietorshipProperty); }
            set { SetProperty(ProprietorshipProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<FixtureAccountViewModel>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<FixtureAccountViewModel>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<FixtureAccountViewModel>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<FixtureAccountViewModel>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 生产日期 ProductDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductDateProperty = P<FixtureAccountViewModel>.Register(e => e.ProductDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductDate
        {
            get { return GetProperty(ProductDateProperty); }
            set { SetProperty(ProductDateProperty, value); }
        }
        #endregion

        #region 厂商名称 Manufacturer
        /// <summary>
        /// 厂商名称
        /// </summary>
        [Label("厂商名称")]
        public static readonly Property<string> ManufacturerProperty = P<FixtureAccountViewModel>.Register(e => e.Manufacturer);

        /// <summary>
        /// 厂商名称
        /// </summary>
        public string Manufacturer
        {
            get { return GetProperty(ManufacturerProperty); }
            set { SetProperty(ManufacturerProperty, value); }
        }
        #endregion

        #region 单价 UnitPrice
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal> UnitPriceProperty = P<FixtureAccountViewModel>.Register(e => e.UnitPrice);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice
        {
            get { return GetProperty(UnitPriceProperty); }
            set { SetProperty(UnitPriceProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureAccountViewModel>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureAccountViewModel>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 工治具仓库 Warehouse
        /// <summary>
        /// 工治具仓库Id
        /// </summary>
        public static readonly IRefIdProperty FixtureWarehouseIdProperty = P<FixtureAccountViewModel>.RegisterRefId(e => e.FixtureWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 工治具仓库Id
        /// </summary>
        public double? FixtureWarehouseId
        {
            get { return (double?)GetRefNullableId(FixtureWarehouseIdProperty); }
            set { SetRefNullableId(FixtureWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 工治具仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> FixtureWarehouseProperty = P<FixtureAccountViewModel>.RegisterRef(e => e.Warehouse, FixtureWarehouseIdProperty);

        /// <summary>
        /// 工治具仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(FixtureWarehouseProperty); }
            set { SetRefEntity(FixtureWarehouseProperty, value); }
        }
        #endregion

        #region 工治具库位 StorageLocation
        /// <summary>
        /// 工治具库位Id
        /// </summary>
        public static readonly IRefIdProperty FixtureStorageLocationIdProperty = P<FixtureAccountViewModel>.RegisterRefId(e => e.FixtureStorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 工治具库位Id
        /// </summary>
        public double? FixtureStorageLocationId
        {
            get { return (double?)GetRefNullableId(FixtureStorageLocationIdProperty); }
            set { SetRefNullableId(FixtureStorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 工治具库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> FixtureStorageLocationProperty = P<FixtureAccountViewModel>.RegisterRef(e => e.StorageLocation, FixtureStorageLocationIdProperty);

        /// <summary>
        /// 工治具库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(FixtureStorageLocationProperty); }
            set { SetRefEntity(FixtureStorageLocationProperty, value); }
        }
        #endregion

        #region RFID Rfid
        /// <summary>
        /// RFID
        /// </summary>
        [Label("RFID")]
        public static readonly Property<string> RfidProperty = P<FixtureAccountViewModel>.Register(e => e.Rfid);

        /// <summary>
        /// RFID
        /// </summary>
        public string Rfid
        {
            get { return this.GetProperty(RfidProperty); }
            set { this.SetProperty(RfidProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<FixtureAccountViewModel>.RegisterView(e => e.EncodeCode, p => p.FixtureEncode.Code);

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
        public static readonly Property<string> ModelCodeProperty = P<FixtureAccountViewModel>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

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
        public static readonly Property<string> ModelNameProperty = P<FixtureAccountViewModel>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 槽位类型 SlotType
        /// <summary>
        /// 槽位类型
        /// </summary>
        [Label("槽位类型")]
        public static readonly Property<SlotType> SlotTypeProperty = P<FixtureAccountViewModel>.RegisterView(e => e.SlotType, p => p.FixtureEncode.FixtureModel.SlotType);

        /// <summary>
        /// 槽位类型
        /// </summary>
        public SlotType SlotType
        {
            get { return this.GetProperty(SlotTypeProperty); }
        }
        #endregion


        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureAccountViewModel>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

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
            P<FixtureAccountViewModel>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion


        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureAccountViewModel>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }
        #endregion

        #region 固定储位 FixedStorage
        /// <summary>
        /// 固定储位
        /// </summary>
        [Label("固定储位")]
        public static readonly Property<YesNo> FixedStorageProperty = P<FixtureAccountViewModel>.RegisterView(e => e.FixedStorage, p => p.FixtureEncode.FixtureModel.FixedStorage);

        /// <summary>
        /// 固定储位
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
        public static readonly Property<YesNo> LoadingManageProperty = P<FixtureAccountViewModel>.RegisterView(e => e.LoadingManage, p => p.FixtureEncode.FixtureModel.LoadingManage);

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
        public static readonly Property<YesNo> BindProductProperty = P<FixtureAccountViewModel>.RegisterView(e => e.BindProduct, p => p.FixtureEncode.FixtureModel.BindProduct);

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
        public static readonly Property<YesNo> BindEquipProperty = P<FixtureAccountViewModel>.RegisterView(e => e.BindEquip, p => p.FixtureEncode.FixtureModel.BindEquip);

        /// <summary>
        /// 绑定设备
        /// </summary>
        public YesNo BindEquip
        {
            get { return this.GetProperty(BindEquipProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<FixtureAccountViewModel>.RegisterView(e => e.UnitName, p => p.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<FixtureAccountViewModel>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<FixtureAccountViewModel>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 最大使用次数 MaxUseNum
        /// <summary>
        /// 最大使用次数
        /// </summary>
        [Label("最大使用次数")]
        public static readonly Property<int> MaxUseNumProperty = P<FixtureAccountViewModel>.RegisterView(e => e.MaxUseNum, p => p.FixtureEncode.FixtureModel.MaxUseNum);

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
        public static readonly Property<int> MaxUseHourProperty = P<FixtureAccountViewModel>.RegisterView(e => e.MaxUseHour, p => p.FixtureEncode.FixtureModel.MaxUseHour);

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
        public static readonly Property<int> MaintainNumProperty = P<FixtureAccountViewModel>.RegisterView(e => e.MaintainNum, p => p.FixtureEncode.FixtureModel.MaintainNum);

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
        public static readonly Property<decimal> MaintainHourProperty = P<FixtureAccountViewModel>.RegisterView(e => e.MaintainHour, p => p.FixtureEncode.FixtureModel.MaintainHour);

        /// <summary>
        /// 保养标准（小时）
        /// </summary>
        public decimal MaintainHour
        {
            get { return this.GetProperty(MaintainHourProperty); }
        }
        #endregion

        #region 上线定期保养标准(小时) OnlineHour
        /// <summary>
        /// 上线定期保养标准(小时)
        /// </summary>
        [Label("上线定期保养标准(小时)")]
        public static readonly Property<decimal> OnlineHourProperty = P<FixtureAccountViewModel>.RegisterView(e => e.OnlineHour, p => p.FixtureEncode.FixtureModel.OnlineHour);

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
        public static readonly Property<bool> MaintainEnforceProperty = P<FixtureAccountViewModel>.RegisterView(e => e.MaintainEnforce, p => p.FixtureEncode.FixtureModel.MaintainEnforce);

        /// <summary>
        /// 保养强制执行
        /// </summary>
        public bool MaintainEnforce
        {
            get { return this.GetProperty(MaintainEnforceProperty); }
        }
        #endregion

        #region 预警值（次数） WarnNum
        /// <summary>
        /// 预警值（次数）
        /// </summary>
        [Label("预警值（次数）")]
        public static readonly Property<int> WarnNumProperty = P<FixtureAccountViewModel>.RegisterView(e => e.WarnNum, p => p.FixtureEncode.FixtureModel.WarnNum);

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
        public static readonly Property<decimal> WarnHourProperty = P<FixtureAccountViewModel>.RegisterView(e => e.WarnHour, p => p.FixtureEncode.FixtureModel.WarnHour);

        /// <summary>
        /// 预警值（小时）
        /// </summary>
        public decimal WarnHour
        {
            get { return this.GetProperty(WarnHourProperty); }
        }
        #endregion

        #region 仓库名称 FixtureWarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> FixtureWarehouseNameProperty = P<FixtureAccountViewModel>.RegisterView(e => e.FixtureWarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string FixtureWarehouseName
        {
            get { return this.GetProperty(FixtureWarehouseNameProperty); }
        }
        #endregion

        #region 库位名称 FixtureStorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> FixtureStorageLocationNameProperty = P<FixtureAccountViewModel>.RegisterView(e => e.FixtureStorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string FixtureStorageLocationName
        {
            get { return this.GetProperty(FixtureStorageLocationNameProperty); }
        }
        #endregion
        #endregion
    }
}
