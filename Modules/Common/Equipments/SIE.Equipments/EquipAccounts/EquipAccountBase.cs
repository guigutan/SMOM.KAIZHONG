using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.Core.Equipments.FixedAssets;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Equipments.Configs;
using SIE.Equipments.DataAuth;
using SIE.Equipments.DeviceControls.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using SIE.Warehouses;
using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账基础数据
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账基础数据")]
    public class EquipAccountBase : SIE.Core.Equipments.EquipAccount
    {
        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public static new readonly IRefIdProperty EquipModelIdProperty = P<EquipAccountBase>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public new double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static new readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipAccountBase>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public new EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<EquipAccountBase>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<EquipAccountBase>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<EquipAccountBase>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<EquipAccountBase>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<EquipAccountBase>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty = P<EquipAccountBase>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<EquipAccountBase>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<EquipAccountBase>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return GetProperty(PhotoProperty); }
            set { SetProperty(PhotoProperty, value); }
        }
        #endregion

        #region 位置 InstallationLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> InstallationLocationProperty
            = P<EquipAccountBase>.Register(e => e.InstallationLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
            set { this.SetProperty(InstallationLocationProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary> 
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty
            = P<EquipAccountBase>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty
            = P<EquipAccountBase>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<EquipAccountBase>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
            set { this.SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<EquipAccountBase>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)this.GetRefId(FactoryIdProperty); }
            set { this.SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Resources.Enterprises.Enterprise> FactoryProperty =
            P<EquipAccountBase>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 是否海关监管 IsCustomsSupervision
        /// <summary>
        /// 是否海关监管
        /// </summary>
        [Label("海关监管")]
        public static readonly Property<bool> IsCustomsSupervisionProperty = P<EquipAccountBase>.Register(e => e.IsCustomsSupervision);

        /// <summary>
        /// 是否海关监管
        /// </summary>
        public bool IsCustomsSupervision
        {
            get { return this.GetProperty(IsCustomsSupervisionProperty); }
            set { this.SetProperty(IsCustomsSupervisionProperty, value); }
        }
        #endregion

        #region 虚拟设备 IsVirtual
        /// <summary>
        /// 虚拟设备
        /// </summary>
        [Label("虚拟设备")]
        public static readonly Property<YesNo> IsVirtualProperty
            = P<EquipAccountBase>.Register(e => e.IsVirtual);

        /// <summary>
        /// 虚拟设备
        /// </summary>
        public YesNo IsVirtual
        {
            get { return GetProperty(IsVirtualProperty); }
            set { SetProperty(IsVirtualProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty
            = P<EquipAccountBase>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty
            = P<EquipAccountBase>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 使用部门 UseDepartment
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDepartmentIdProperty =
            P<EquipAccountBase>.RegisterRefId(e => e.UseDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDepartmentId
        {
            get { return (double?)this.GetRefNullableId(UseDepartmentIdProperty); }
            set { this.SetRefNullableId(UseDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDepartmentProperty =
            P<EquipAccountBase>.RegisterRef(e => e.UseDepartment, UseDepartmentIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDepartment
        {
            get { return this.GetRefEntity(UseDepartmentProperty); }
            set { this.SetRefEntity(UseDepartmentProperty, value); }
        }
        #endregion

        #region 管理部门 ManageDepartment
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty ManageDepartmentIdProperty =
            P<EquipAccountBase>.RegisterRefId(e => e.ManageDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double? ManageDepartmentId
        {
            get { return (double?)this.GetRefNullableId(ManageDepartmentIdProperty); }
            set { this.SetRefNullableId(ManageDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ManageDepartmentProperty =
            P<EquipAccountBase>.RegisterRef(e => e.ManageDepartment, ManageDepartmentIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise ManageDepartment
        {
            get { return this.GetRefEntity(ManageDepartmentProperty); }
            set { this.SetRefEntity(ManageDepartmentProperty, value); }
        }
        #endregion

        #region 生产厂家 Manufacturer
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Label("生产厂家")]
        public static readonly Property<string> ManufacturerProperty
            = P<EquipAccountBase>.Register(e => e.Manufacturer);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
            set { this.SetProperty(ManufacturerProperty, value); }
        }
        #endregion

        #region 立卡日期 CardDate
        /// <summary>
        /// 立卡日期
        /// </summary>
        [Label("立卡日期")]
        public static readonly Property<DateTime?> CardDateProperty = P<EquipAccountBase>.Register(e => e.CardDate);

        /// <summary>
        /// 立卡日期
        /// </summary>
        public DateTime? CardDate
        {
            get { return this.GetProperty(CardDateProperty); }
            set { this.SetProperty(CardDateProperty, value); }
        }
        #endregion

        #region 租赁/客供单位 PurchaseUnit
        /// <summary>
        /// 租赁/客供单位
        /// </summary>
        [Label("租赁/客供单位")]
        public static readonly Property<string> PurchaseUnitProperty
            = P<EquipAccountBase>.Register(e => e.PurchaseUnit);

        /// <summary>
        /// 租赁/客供单位
        /// </summary>
        public string PurchaseUnit
        {
            get { return this.GetProperty(PurchaseUnitProperty); }
            set { this.SetProperty(PurchaseUnitProperty, value); }
        }
        #endregion

        #region 入厂日期 EnterDate
        /// <summary>
        /// 入厂日期
        /// </summary>
        [Label("入厂日期")]
        public static readonly Property<DateTime?> EnterDateProperty
            = P<EquipAccountBase>.Register(e => e.EnterDate);

        /// <summary>
        /// 入厂日期
        /// </summary>
        public DateTime? EnterDate
        {
            get { return this.GetProperty(EnterDateProperty); }
            set { this.SetProperty(EnterDateProperty, value); }
        }
        #endregion

        #region 使用年限 UsefulLife
        /// <summary>
        /// 使用年限
        /// </summary>
        [Label("使用年限")]
        public static readonly Property<decimal?> UsefulLifeProperty
            = P<EquipAccountBase>.Register(e => e.UsefulLife);

        /// <summary>
        /// 使用年限
        /// </summary>
        public decimal? UsefulLife
        {
            get { return this.GetProperty(UsefulLifeProperty); }
            set { this.SetProperty(UsefulLifeProperty, value); }
        }
        #endregion

        #region 保修期 WarrantyPeriod
        /// <summary>
        /// 保修期
        /// </summary>
        [Label("保修期")]
        public static readonly Property<DateTime?> WarrantyPeriodProperty
            = P<EquipAccountBase>.Register(e => e.WarrantyPeriod);

        /// <summary>
        /// 保修期
        /// </summary>
        public DateTime? WarrantyPeriod
        {
            get { return this.GetProperty(WarrantyPeriodProperty); }
            set { this.SetProperty(WarrantyPeriodProperty, value); }
        }
        #endregion

        #region 资产来源 Proprietorship
        /// <summary>
        /// 资产来源
        /// </summary>
        [Label("资产来源")]
        public static readonly Property<Proprietorship> ProprietorshipProperty
            = P<EquipAccountBase>.Register(e => e.Proprietorship);

        /// <summary>
        /// 资产来源
        /// </summary>
        public Proprietorship Proprietorship
        {
            get { return GetProperty(ProprietorshipProperty); }
            set { SetProperty(ProprietorshipProperty, value); }
        }
        #endregion

        #region 资产责任人 ResPerson
        /// <summary>
        /// 资产责任人Id
        /// </summary>
        [Label("资产责任人")]
        public static readonly IRefIdProperty ResPersonIdProperty =
            P<EquipAccountBase>.RegisterRefId(e => e.ResPersonId, ReferenceType.Normal);

        /// <summary>
        /// 资产责任人Id
        /// </summary>
        public double? ResPersonId
        {
            get { return (double?)this.GetRefNullableId(ResPersonIdProperty); }
            set { this.SetRefNullableId(ResPersonIdProperty, value); }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ResPersonProperty =
            P<EquipAccountBase>.RegisterRef(e => e.ResPerson, ResPersonIdProperty);

        /// <summary>
        /// 责任人
        /// </summary>
        public Employee ResPerson
        {
            get { return this.GetRefEntity(ResPersonProperty); }
            set { this.SetRefEntity(ResPersonProperty, value); }
        }
        #endregion

        #region 使用责任人 UserId
        /// <summary>
        /// 使用责任人Id
        /// </summary>
        [Label("使用责任人")]
        public static readonly IRefIdProperty UserIdProperty =
            P<EquipAccountBase>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 使用责任人Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)this.GetRefNullableId(UserIdProperty); }
            set { this.SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 使用责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> UserProperty =
            P<EquipAccountBase>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 使用责任人
        /// </summary>
        public Employee User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 设备别名 Alias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> AliasProperty = P<EquipAccountBase>.Register(e => e.Alias);

        /// <summary>
        /// 设备别名
        /// </summary>
        public string Alias
        {
            get { return this.GetProperty(AliasProperty); }
            set { this.SetProperty(AliasProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSerialNumber
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSerialNumberProperty = P<EquipAccountBase>.Register(e => e.OriginalSerialNumber);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSerialNumber
        {
            get { return this.GetProperty(OriginalSerialNumberProperty); }
            set { this.SetProperty(OriginalSerialNumberProperty, value); }
        }
        #endregion

        #region RFID RFID
        /// <summary>
        /// RFID
        /// </summary>
        [Label("RFID")]
        public static readonly Property<string> RFIDProperty = P<EquipAccountBase>.Register(e => e.RFID);

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID
        {
            get { return this.GetProperty(RFIDProperty); }
            set { this.SetProperty(RFIDProperty, value); }
        }
        #endregion

        #region 冻结 Frozen
        /// <summary>
        /// 冻结
        /// </summary>
        [Label("冻结")]
        public static readonly Property<YesNo> FrozenProperty = P<EquipAccountBase>.Register(e => e.Frozen);

        /// <summary>
        /// 冻结
        /// </summary>
        public YesNo Frozen
        {
            get { return this.GetProperty(FrozenProperty); }
            set { this.SetProperty(FrozenProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<EquipAccountBase>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<EquipAccountBase>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly IRefIdProperty StorageLocationIdProperty = P<EquipAccountBase>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<EquipAccountBase>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 保管人 Administrator
        /// <summary>
        /// 保管人Id
        /// </summary>
        public static readonly IRefIdProperty AdministratorIdProperty = P<EquipAccountBase>.RegisterRefId(e => e.AdministratorId, ReferenceType.Normal);

        /// <summary>
        /// 保管人Id
        /// </summary>
        public double? AdministratorId
        {
            get { return (double?)GetRefNullableId(AdministratorIdProperty); }
            set { SetRefNullableId(AdministratorIdProperty, value); }
        }

        /// <summary>
        /// 保管人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AdministratorProperty = P<EquipAccountBase>.RegisterRef(e => e.Administrator, AdministratorIdProperty);

        /// <summary>
        /// 保管人
        /// </summary>
        public Employee Administrator
        {
            get { return GetRefEntity(AdministratorProperty); }
            set { SetRefEntity(AdministratorProperty, value); }
        }
        #endregion

        #region 设备评级 EquipmentGrading
        /// <summary>
        /// 设备评级
        /// </summary>
        [Label("设备评级")]
        public static readonly Property<string> EquipmentGradingProperty = P<EquipAccountBase>.Register(e => e.EquipmentGrading);

        /// <summary>
        /// 设备评级
        /// </summary>
        public string EquipmentGrading
        {
            get { return this.GetProperty(EquipmentGradingProperty); }
            set { this.SetProperty(EquipmentGradingProperty, value); }
        }
        #endregion

        #region 固定资产 FixAsset
        /// <summary>
        /// 固定资产Id
        /// </summary>
        [Label("固定资产")]
        public static readonly IRefIdProperty FixedAssetsAccountIdProperty =
            P<EquipAccountBase>.RegisterRefId(e => e.FixedAssetsAccountId, ReferenceType.Normal);

        /// <summary>
        /// 固定资产Id
        /// </summary>
        public double? FixedAssetsAccountId
        {
            get { return (double?)this.GetRefNullableId(FixedAssetsAccountIdProperty); }
            set { this.SetRefNullableId(FixedAssetsAccountIdProperty, value); }
        }

        /// <summary>
        /// 固定资产
        /// </summary>
        public static readonly RefEntityProperty<FixedAssetsAccount> FixedAssetsAccountProperty =
            P<EquipAccountBase>.RegisterRef(e => e.FixedAssetsAccount, FixedAssetsAccountIdProperty);

        /// <summary>
        /// 固定资产
        /// </summary>
        public FixedAssetsAccount FixedAssetsAccount
        {
            get { return this.GetRefEntity(FixedAssetsAccountProperty); }
            set { this.SetRefEntity(FixedAssetsAccountProperty, value); }
        }
        #endregion

        #region 在线状态 DeviceOnLineState
        /// <summary>
        /// 在线状态
        /// </summary>
        [Label("在线状态")]
        public static readonly Property<EquipOnLineState?> EquipOnLineStateProperty = P<EquipAccountBase>.Register(e => e.EquipOnLineState);

        /// <summary>
        /// 在线状态
        /// </summary>
        public EquipOnLineState? EquipOnLineState
        {
            get { return this.GetProperty(EquipOnLineStateProperty); }
            set { this.SetProperty(EquipOnLineStateProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 行业属性 IndustryCategory
        /// <summary>
        /// 行业属性
        /// </summary>
        [Label("行业属性")]
        public static readonly Property<IndustryCategory> IndustryCategoryProperty
            = P<EquipAccountBase>.RegisterView(e => e.IndustryCategory, p => p.EquipModel.IndustryCategory);

        /// <summary>
        /// 行业属性
        /// </summary>
        public IndustryCategory IndustryCategory
        {
            get { return this.GetProperty(IndustryCategoryProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty
            = P<EquipAccountBase>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

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
        [Label("供应商全称")]
        public static readonly Property<string> SupplierNameProperty
            = P<EquipAccountBase>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion 

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty
            = P<EquipAccountBase>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return GetProperty(WorkShopCodeProperty); }
            set { SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion 

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty
            = P<EquipAccountBase>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty
            = P<EquipAccountBase>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 使用部门 UseDepartmentName
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentNameProperty
            = P<EquipAccountBase>.RegisterView(e => e.UseDepartmentName, p => p.UseDepartment.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartmentName
        {
            get { return this.GetProperty(UseDepartmentNameProperty); }
        }
        #endregion

        #region 管理部门名称 ManageDepartmentName
        /// <summary>
        /// 管理部门名称
        /// </summary>
        [Label("管理部门名称")]
        public static readonly Property<string> ManageDepartmentNameProperty = P<EquipAccountBase>.RegisterView(e => e.ManageDepartmentName, p => p.ManageDepartment.Name);

        /// <summary>
        /// 管理部门名称
        /// </summary>
        public string ManageDepartmentName
        {
            get { return this.GetProperty(ManageDepartmentNameProperty); }
        }
        #endregion

        #region 责任人 ResPersonName
        /// <summary>
        /// 责任人
        /// </summary>
        [Label("责任人")]
        public static readonly Property<string> ResPersonNameProperty
            = P<EquipAccountBase>.RegisterView(e => e.ResPersonName, p => p.ResPerson.Name);

        /// <summary>
        /// 责任人
        /// </summary>
        public string ResPersonName
        {
            get { return this.GetProperty(ResPersonNameProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipAccountBase>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 设备型号类型名称 EquipModelTypeName
        /// <summary>
        /// 设备型号类型名称
        /// </summary>
        [Label("设备型号类型名称")]
        public static readonly Property<string> EquipModelTypeNameProperty = P<EquipAccountBase>.RegisterView(e => e.EquipModelTypeName, p => p.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备型号类型名称
        /// </summary>
        public string EquipModelTypeName
        {
            get { return this.GetProperty(EquipModelTypeNameProperty); }
        }
        #endregion

        #region 电子行业基础数据

        #region 注释 RailType
        /// <summary>
        /// 注释
        /// </summary>
        [Label("轨道类型")]
        public static readonly Property<OrbitType?> RailTypeProperty
            = P<EquipAccountBase>.RegisterView(e => e.RailType, p => p.EquipModel.RailType);

        /// <summary>
        /// 注释
        /// </summary>
        public OrbitType? RailType
        {
            get { return this.GetProperty(RailTypeProperty); }
        }
        #endregion

        #region 是否Feeder绑定 FeederBinding
        /// <summary>
        /// 是否Feeder绑定
        /// </summary>
        [Label("是否Feeder绑定")]
        public static readonly Property<YesNo> FeederBindingProperty
            = P<EquipAccountBase>.RegisterView(e => e.FeederBinding, p => p.EquipModel.FeederBinding);

        /// <summary>
        /// 是否Feeder绑定
        /// </summary>
        public YesNo FeederBinding
        {
            get { return this.GetProperty(FeederBindingProperty); }
        }
        #endregion

        #region 启用站位防错 FeederLocFailSafe
        /// <summary>
        /// 启用站位防错
        /// </summary>
        [Label("启用站位防错")]
        public static readonly Property<State> FeederLocFailSafeProperty
            = P<EquipAccountBase>.RegisterView(e => e.FeederLocFailSafe, p => p.EquipModel.FeederLocFailSafe);

        /// <summary>
        /// 启用站位防错
        /// </summary>
        public State FeederLocFailSafe
        {
            get { return this.GetProperty(FeederLocFailSafeProperty); }
        }
        #endregion

        #region 启用Feeder防错 FeederBarcodeFailSafe
        /// <summary>
        /// 启用Feeder防错
        /// </summary>
        [Label("启用Feeder防错")]
        public static readonly Property<State> FeederBarcodeFailSafeProperty
            = P<EquipAccountBase>.RegisterView(e => e.FeederBarcodeFailSafe, p => p.EquipModel.FeederBarcodeFailSafe);

        /// <summary>
        /// 启用Feeder防错
        /// </summary>
        public State FeederBarcodeFailSafe
        {
            get { return this.GetProperty(FeederBarcodeFailSafeProperty); }
        }
        #endregion

        #region 虚拟设备 VirtualDevice
        /// <summary>
        /// 虚拟设备
        /// </summary>
        [Label("虚拟设备")]
        public static readonly Property<YesNo> VirtualDeviceProperty
            = P<EquipAccountBase>.RegisterView(e => e.VirtualDevice, p => p.EquipModel.VirtualDevice);

        /// <summary>
        /// 虚拟设备
        /// </summary>
        public YesNo VirtualDevice
        {
            get { return this.GetProperty(VirtualDeviceProperty); }
        }
        #endregion

        #region 禁用 IsDisabled
        /// <summary>
        /// 禁用
        /// </summary>
        [Label("禁用")]
        public static readonly Property<YesNo> IsDisabledProperty
            = P<EquipAccountBase>.RegisterView(e => e.IsDisabled, p => p.EquipModel.IsDisabled);

        /// <summary>
        /// 禁用
        /// </summary>
        public YesNo IsDisabled
        {
            get { return this.GetProperty(IsDisabledProperty); }
        }
        #endregion

        #region 老化方式 AgingType
        /// <summary>
        /// 老化方式
        /// </summary>
        [Label("老化方式")]
        public static readonly Property<AgingMode?> AgingTypeProperty
            = P<EquipAccountBase>.RegisterView(e => e.AgingType, p => p.EquipModel.AgingType);

        /// <summary>
        /// 老化方式
        /// </summary>
        public AgingMode? AgingType
        {
            get { return this.GetProperty(AgingTypeProperty); }
        }
        #endregion

        #region 产品生产模式 ProductionType
        /// <summary>
        /// 产品生产模式
        /// </summary>
        [Label("产品生产模式")]
        public static readonly Property<ProductionMode?> ProductionTypeProperty
            = P<EquipAccountBase>.RegisterView(e => e.ProductionType, p => p.EquipModel.ProductionType);

        /// <summary>
        /// 产品生产模式
        /// </summary>
        public ProductionMode? ProductionType
        {
            get { return this.GetProperty(ProductionTypeProperty); }

        }
        #endregion

        #endregion

        #region 固定资产编码 AssetCode
        /// <summary>
        /// 固定资产编码
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> AssetCodeProperty = P<EquipAccountBase>.RegisterView(e => e.AssetCode, p => p.FixedAssetsAccount.Code);

        /// <summary>
        /// 固定资产编码
        /// </summary>
        public string AssetCode
        {
            get { return GetProperty(AssetCodeProperty); }
            set { SetProperty(AssetCodeProperty, value); }

        }
        #endregion

        #region 设备类型编码 EquipModelTypeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> EquipModelTypeCodeProperty = P<EquipAccountBase>.RegisterView(e => e.EquipModelTypeCode, p => p.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipModelTypeCode
        {
            get { return this.GetProperty(EquipModelTypeCodeProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty
            = P<EquipAccountBase>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return GetProperty(FactoryNameProperty); }
            set { SetProperty(FactoryNameProperty, value); }
        }
        #endregion 


        #endregion

        #region
        #region 原值 OriginalValue
        /// <summary>
        /// 原值
        /// </summary>
        [Label("原值")]
        public static readonly Property<decimal> OriginalValueProperty = P<EquipAccountBase>.Register(e => e.OriginalValue);

        /// <summary>
        /// 原值
        /// </summary>
        public decimal OriginalValue
        {
            get { return GetProperty(OriginalValueProperty); }
            set { SetProperty(OriginalValueProperty, value); }
        }
        #endregion

        #region 是否固定资产 IssAsset
        /// <summary>
        /// 是否固定资产
        /// </summary>
        [Label("是否固定资产")]
        public static readonly Property<bool?> IssAssetProperty = P<EquipAccountBase>.Register(e => e.IssAsset);

        /// <summary>
        /// 是否固定资产
        /// </summary>
        public bool? IssAsset
        {
            get { return GetProperty(IssAssetProperty); }
            set { SetProperty(IssAssetProperty, value); }
        }
        #endregion

        #region 资产名称 AssetName
        /// <summary>
        /// 资产名称
        /// </summary>
        [Label("资产名称")]
        public static readonly Property<string> AssetNameProperty = P<EquipAccountBase>.Register(e => e.AssetName);

        /// <summary>
        /// 资产名称
        /// </summary>
        public string AssetName
        {
            get { return GetProperty(AssetNameProperty); }
            set { SetProperty(AssetNameProperty, value); }
        }
        #endregion

        #region 固定资产编码(视图属性) FixedAssetsAccountCode
        /// <summary>
        /// 固定资产编码(视图属性)
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetsAccountCodeProperty
            = P<EquipAccountBase>.RegisterView(e => e.FixedAssetsAccountCode, p => p.FixedAssetsAccount.Code);

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
            = P<EquipAccountBase>.RegisterView(e => e.FixedAssetsAccountName, p => p.FixedAssetsAccount.Name);

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
            = P<EquipAccountBase>.RegisterView(e => e.OriginalAssetsValue, p => p.FixedAssetsAccount.OriginalAssetsValue);

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
            = P<EquipAccountBase>.RegisterView(e => e.NetAssetValue, p => p.FixedAssetsAccount.NetAssetValue);

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
            = P<EquipAccountBase>.RegisterView(e => e.DepreciationResidualValue, p => p.FixedAssetsAccount.DepreciationResidualValue);

        /// <summary>
        /// 残值(视图属性)
        /// </summary>
        public decimal DepreciationResidualValue
        {
            get { return this.GetProperty(DepreciationResidualValueProperty); }
            set { SetProperty(DepreciationResidualValueProperty, value); }
        }
        #endregion

        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<EquipAccountBase>.RegisterView(e => e.Specifications, p => p.EquipModel.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
        }
        #endregion

        #region 仓库 WarehouseName
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<EquipAccountBase>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 库位 StorageLocationName
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationNameProperty = P<EquipAccountBase>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion


        #region 保管人 AdministratorName
        /// <summary>
        /// 保管人
        /// </summary>
        [Label("保管人")]
        public static readonly Property<string> AdministratorNameProperty = P<EquipAccountBase>.RegisterView(e => e.AdministratorName, p => p.Administrator.Name);

        /// <summary>
        /// 保管人
        /// </summary>
        public string AdministratorName
        {
            get { return this.GetProperty(AdministratorNameProperty); }
        }
        #endregion


        #endregion

        #region 下次检验日期 NextInspectionDate
        /// <summary>
        /// 下次检验日期
        /// </summary>
        [Label("下次检验日期")]
        public static readonly Property<DateTime?> NextInspectionDateProperty = P<EquipAccountBase>.Register(e => e.NextInspectionDate);

        /// <summary>
        /// 下次检验日期
        /// </summary>
        public DateTime? NextInspectionDate
        {
            get { return GetProperty(NextInspectionDateProperty); }
            set { SetProperty(NextInspectionDateProperty, value); }
        }
        #endregion

        #region 定检状态 RegularInspectionStatus
        /// <summary>
        /// 定检状态
        /// </summary>
        [Label("定检状态")]
        public static readonly Property<RegularInspectionStatus?> RegularInspectionStatusProperty = P<EquipAccountBase>.Register(e => e.RegularInspectionStatus);

        /// <summary>
        /// 定检状态
        /// </summary>
        public RegularInspectionStatus? RegularInspectionStatus
        {
            get { return GetProperty(RegularInspectionStatusProperty); }
            set { SetProperty(RegularInspectionStatusProperty, value); }
        }
        #endregion

        #region  计量检验规程字段
        #region 降级使用 Downgrade
        /// <summary>
        /// 降级使用
        /// </summary>
        [Label("降级使用")]
        public static readonly Property<bool?> DowngradeProperty = P<EquipAccountBase>.Register(e => e.Downgrade);

        /// <summary>
        /// 降级使用
        /// </summary>
        public bool? Downgrade
        {
            get { return GetProperty(DowngradeProperty); }
            set { SetProperty(DowngradeProperty, value); }
        }
        #endregion

        #region 精度等级 PrecisionClass
        /// <summary>
        /// 精度等级
        /// </summary>
        [Label("精度等级")]
        public static readonly Property<string> PrecisionClassProperty = P<EquipAccountBase>.Register(e => e.PrecisionClass);

        /// <summary>
        /// 精度等级
        /// </summary>
        public string PrecisionClass
        {
            get { return GetProperty(PrecisionClassProperty); }
            set { SetProperty(PrecisionClassProperty, value); }
        }
        #endregion

        #region 检验日期 InspectionDate
        /// <summary>
        /// 检验日期
        /// </summary>
        [Label("检验日期")]
        public static readonly Property<DateTime?> InspectionDateProperty = P<EquipAccountBase>.Register(e => e.InspectionDate);

        /// <summary>
        /// 检验日期
        /// </summary>
        public DateTime? InspectionDate
        {
            get { return GetProperty(InspectionDateProperty); }
            set { SetProperty(InspectionDateProperty, value); }
        }
        #endregion

        #endregion

        #region 设备借还字段
        #region 原管理状态 OldUseState
        /// <summary>
        /// 原管理状态
        /// </summary>
        [Label("原管理状态")]
        public static readonly Property<AccountUseState?> OldUseStateProperty = P<EquipAccountBase>.Register(e => e.OldUseState);

        /// <summary>
        /// 原管理状态
        /// </summary>
        public AccountUseState? OldUseState
        {
            get { return this.GetProperty(OldUseStateProperty); }
            set { this.SetProperty(OldUseStateProperty, value); }
        }
        #endregion
        #endregion

        #region 不映射数据库的属性

        #region 报废类型 ScrapType
        /// <summary>
        /// 报废类型
        /// </summary>
        [Label("报废类型")]
        public static readonly Property<string> ScrapTypeProperty = P<EquipAccountBase>.Register(e => e.ScrapType);

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
        public static readonly Property<string> ReasonProperty = P<EquipAccountBase>.Register(e => e.Reason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string Reason
        {
            get { return GetProperty(ReasonProperty); }
            set { SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 是否计量类型 IsCalibration
        /// <summary>
        /// 是否计量类型
        /// </summary>
        [Label("是否计量类型")]
        public static readonly Property<bool> IsCalibrationProperty = P<EquipAccountBase>.Register(e => e.IsCalibration);

        /// <summary>
        /// 是否计量类型
        /// </summary>
        public bool IsCalibration
        {
            get { return GetProperty(IsCalibrationProperty); }
            set { SetProperty(IsCalibrationProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备台账 实体配置
    /// </summary>
    internal class EquipAccountBaseConfig : EntityConfig<EquipAccountBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();
            Meta.SupportTree();
            Meta.Property(EquipAccountBase.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccountBase.FactoryIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccountBase.ManageDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccountBase.UseDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccountBase.EquipModelIdProperty).ColumnMeta.HasIndex();


            Meta.Property(EquipAccountBase.ScrapTypeProperty).DontMapColumn();
            Meta.Property(EquipAccountBase.ReasonProperty).DontMapColumn();
            Meta.Property(EquipAccountBase.IsCalibrationProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
