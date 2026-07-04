using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.Core.Equipments.FixedAssets;
using SIE.CSM.Suppliers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.Equipments.EquipmentCards
{
    /// <summary>
    /// 设备立卡
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备立卡")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [ConditionQueryType(typeof(EquipmentCardCriteria))]
    [EntityDataAuthAttribute(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class EquipmentCard : DataEntity
    {
        #region 设备编码 Code
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        [Required]
        public static readonly Property<string> CodeProperty = P<EquipmentCard>.Register(e => e.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 设备名称 Name
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> NameProperty = P<EquipmentCard>.Register(e => e.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 设备别名 Alias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> AliasProperty = P<EquipmentCard>.Register(e => e.Alias);

        /// <summary>
        /// 设备别名
        /// </summary>
        public string Alias
        {
            get { return GetProperty(AliasProperty); }
            set { SetProperty(AliasProperty, value); }
        }
        #endregion

        #region 已验收 IsAccepted
        /// <summary>
        /// 已验收
        /// </summary>
        [Label("已验收")]
        public static readonly Property<bool> IsAcceptedProperty = P<EquipmentCard>.Register(e => e.IsAccepted);

        /// <summary>
        /// 已验收
        /// </summary>
        public bool IsAccepted
        {
            get { return GetProperty(IsAcceptedProperty); }
            set { SetProperty(IsAcceptedProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSerialNumber
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSerialNumberProperty = P<EquipmentCard>.Register(e => e.OriginalSerialNumber);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSerialNumber
        {
            get { return GetProperty(OriginalSerialNumberProperty); }
            set { SetProperty(OriginalSerialNumberProperty, value); }
        }
        #endregion

        #region RFID Rfid
        /// <summary>
        /// RFID
        /// </summary>
        [Label("RFID")]
        public static readonly Property<string> RfidProperty = P<EquipmentCard>.Register(e => e.Rfid);

        /// <summary>
        /// RFID
        /// </summary>
        public string Rfid
        {
            get { return GetProperty(RfidProperty); }
            set { SetProperty(RfidProperty, value); }
        }
        #endregion

        #region ABC分类 UseLevel
        /// <summary>
        /// ABC分类
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<string> UseLevelProperty = P<EquipmentCard>.Register(e => e.UseLevel);

        /// <summary>
        /// ABC分类
        /// </summary>
        public string UseLevel
        {
            get { return GetProperty(UseLevelProperty); }
            set { SetProperty(UseLevelProperty, value); }
        }
        #endregion

        #region 设备评级 EquipModelGrade
        /// <summary>
        /// 设备评级
        /// </summary>
        [Label("设备评级")]
        public static readonly Property<string> EquipModelGradeProperty = P<EquipmentCard>.Register(e => e.EquipModelGrade);

        /// <summary>
        /// 设备评级
        /// </summary>
        public string EquipModelGrade
        {
            get { return GetProperty(EquipModelGradeProperty); }
            set { SetProperty(EquipModelGradeProperty, value); }
        }
        #endregion

        #region 租赁/客供单位 PurchaseUnit
        /// <summary>
        /// 租赁/客供单位
        /// </summary>
        [Label("租赁/客供单位")]
        public static readonly Property<string> PurchaseUnitProperty = P<EquipmentCard>.Register(e => e.PurchaseUnit);

        /// <summary>
        /// 租赁/客供单位
        /// </summary>
        public string PurchaseUnit
        {
            get { return GetProperty(PurchaseUnitProperty); }
            set { SetProperty(PurchaseUnitProperty, value); }
        }
        #endregion

        #region 生产厂家 Manufacturer
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Label("生产厂家")]
        public static readonly Property<string> ManufacturerProperty = P<EquipmentCard>.Register(e => e.Manufacturer);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
        {
            get { return GetProperty(ManufacturerProperty); }
            set { SetProperty(ManufacturerProperty, value); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<EquipmentCard>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return GetProperty(PurchaseOrderNoProperty); }
            set { SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 入厂日期 EnterDate
        /// <summary>
        /// 入厂日期
        /// </summary>
        [Label("入厂日期")]
        public static readonly Property<DateTime> EnterDateProperty = P<EquipmentCard>.Register(e => e.EnterDate);

        /// <summary>
        /// 入厂日期
        /// </summary>
        public DateTime EnterDate
        {
            get { return GetProperty(EnterDateProperty); }
            set { SetProperty(EnterDateProperty, value); }
        }
        #endregion

        #region 海关监管设备 IsCustomsSupervision
        /// <summary>
        /// 海关监管设备
        /// </summary>
        [Label("海关监管设备")]
        public static readonly Property<bool> IsCustomsSupervisionProperty = P<EquipmentCard>.Register(e => e.IsCustomsSupervision);

        /// <summary>
        /// 海关监管设备
        /// </summary>
        public bool IsCustomsSupervision
        {
            get { return GetProperty(IsCustomsSupervisionProperty); }
            set { SetProperty(IsCustomsSupervisionProperty, value); }
        }
        #endregion

        #region 使用年限 UsefulLife
        /// <summary>
        /// 使用年限
        /// </summary>
        [Label("使用年限")]
        public static readonly Property<decimal> UsefulLifeProperty = P<EquipmentCard>.Register(e => e.UsefulLife);

        /// <summary>
        /// 使用年限
        /// </summary>
        public decimal UsefulLife
        {
            get { return GetProperty(UsefulLifeProperty); }
            set { SetProperty(UsefulLifeProperty, value); }
        }
        #endregion

        #region 保修期 WarrantyPeriod
        /// <summary>
        /// 保修期
        /// </summary>
        [Label("保修期")]
        public static readonly Property<DateTime?> WarrantyPeriodProperty = P<EquipmentCard>.Register(e => e.WarrantyPeriod);

        /// <summary>
        /// 保修期
        /// </summary>
        public DateTime? WarrantyPeriod
        {
            get { return GetProperty(WarrantyPeriodProperty); }
            set { SetProperty(WarrantyPeriodProperty, value); }
        }
        #endregion

        #region 位置 InstallationLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> InstallationLocationProperty = P<EquipmentCard>.Register(e => e.InstallationLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string InstallationLocation
        {
            get { return GetProperty(InstallationLocationProperty); }
            set { SetProperty(InstallationLocationProperty, value); }
        }
        #endregion

        #region 立卡日期 CreateCardDateTime
        /// <summary>
        /// 立卡日期
        /// </summary>
        [Label("立卡日期")]
        public static readonly Property<DateTime?> CreateCardDateTimeProperty = P<EquipmentCard>.Register(e => e.CreateCardDateTime);

        /// <summary>
        /// 立卡日期
        /// </summary>
        public DateTime? CreateCardDateTime
        {
            get { return GetProperty(CreateCardDateTimeProperty); }
            set { SetProperty(CreateCardDateTimeProperty, value); }
        }
        #endregion

        #region 是否固定资产 IssAsset
        /// <summary>
        /// 是否固定资产
        /// </summary>
        [Label("是否固定资产")]
        public static readonly Property<bool?> IssAssetProperty = P<EquipmentCard>.Register(e => e.IssAsset);

        /// <summary>
        /// 是否固定资产
        /// </summary>
        public bool? IssAsset
        {
            get { return GetProperty(IssAssetProperty); }
            set { SetProperty(IssAssetProperty, value); }
        }
        #endregion

        #region 固定资产编码 AssetCode
        /// <summary>
        /// 固定资产编码
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> AssetCodeProperty = P<EquipmentCard>.Register(e => e.AssetCode);

        /// <summary>
        /// 固定资产编码
        /// </summary>
        public string AssetCode
        {
            get { return GetProperty(AssetCodeProperty); }
            set { SetProperty(AssetCodeProperty, value); }
        }
        #endregion

        #region 资产名称 AssetName
        /// <summary>
        /// 资产名称
        /// </summary>
        [Label("资产名称")]
        public static readonly Property<string> AssetNameProperty = P<EquipmentCard>.Register(e => e.AssetName);

        /// <summary>
        /// 资产名称
        /// </summary>
        public string AssetName
        {
            get { return GetProperty(AssetNameProperty); }
            set { SetProperty(AssetNameProperty, value); }
        }
        #endregion

        #region 资产责任人 AssetUser
        /// <summary>
        /// 资产责任人Id
        /// </summary>
        [Label("资产责任人")]
        public static readonly IRefIdProperty AssetUserIdProperty = P<EquipmentCard>.RegisterRefId(e => e.AssetUserId, ReferenceType.Normal);

        /// <summary>
        /// 资产责任人Id
        /// </summary>
        public double? AssetUserId
        {
            get { return (double?)GetRefNullableId(AssetUserIdProperty); }
            set { SetRefNullableId(AssetUserIdProperty, value); }
        }

        /// <summary>
        /// 资产责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AssetUserProperty = P<EquipmentCard>.RegisterRef(e => e.AssetUser, AssetUserIdProperty);

        /// <summary>
        /// 资产责任人
        /// </summary>
        public Employee AssetUser
        {
            get { return GetRefEntity(AssetUserProperty); }
            set { SetRefEntity(AssetUserProperty, value); }
        }
        #endregion

        #region 原值 OriginalValue
        /// <summary>
        /// 原值
        /// </summary>
        [Label("原值")]
        public static readonly Property<decimal> OriginalValueProperty = P<EquipmentCard>.Register(e => e.OriginalValue);

        /// <summary>
        /// 原值
        /// </summary>
        public decimal OriginalValue
        {
            get { return GetProperty(OriginalValueProperty); }
            set { SetProperty(OriginalValueProperty, value); }
        }
        #endregion

        #region 开箱验收 NeedAcceptance
        /// <summary>
        /// 开箱验收
        /// </summary>
        [Label("开箱验收")]
        public static readonly Property<bool> NeedAcceptanceProperty = P<EquipmentCard>.Register(e => e.NeedAcceptance);

        /// <summary>
        /// 开箱验收
        /// </summary>
        public bool NeedAcceptance
        {
            get { return GetProperty(NeedAcceptanceProperty); }
            set { SetProperty(NeedAcceptanceProperty, value); }
        }
        #endregion

        #region 已修改 IsChange
        /// <summary>
        /// 已修改
        /// </summary>
        [Label("已修改")]
        public static readonly Property<bool> IsChangeProperty = P<EquipmentCard>.Register(e => e.IsChange);

        /// <summary>
        /// 已修改
        /// </summary>
        public bool IsChange
        {
            get { return GetProperty(IsChangeProperty); }
            set { SetProperty(IsChangeProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<EquipmentCard>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<EquipmentCard>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationCodeProperty = P<EquipmentCard>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationCode
        {
            get { return GetProperty(StorageLocationCodeProperty); }
            set { SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion

        #region 资产来源 Proprietorship
        /// <summary>
        /// 资产来源
        /// </summary>
        [Label("资产来源")]
        public static readonly Property<Proprietorship> ProprietorshipProperty = P<EquipmentCard>.Register(e => e.Proprietorship);

        /// <summary>
        /// 资产来源
        /// </summary>
        public Proprietorship Proprietorship
        {
            get { return GetProperty(ProprietorshipProperty); }
            set { SetProperty(ProprietorshipProperty, value); }
        }
        #endregion

        #region 使用责任人 User
        /// <summary>
        /// 使用责任人Id
        /// </summary>
        [Label("使用责任人")]
        public static readonly IRefIdProperty UserIdProperty = P<EquipmentCard>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 使用责任人Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)GetRefNullableId(UserIdProperty); }
            set { SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 使用责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> UserProperty = P<EquipmentCard>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 使用责任人
        /// </summary>
        public Employee User
        {
            get { return GetRefEntity(UserProperty); }
            set { SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 设备状态 AccountState
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("台账状态")]
        public static readonly Property<AccountState> AccountStateProperty = P<EquipmentCard>.Register(e => e.AccountState);

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState AccountState
        {
            get { return GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<EquipmentCard>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<EquipmentCard>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 管理部门 Management
        /// <summary>
        /// 管理部门Id
        /// </summary>
        [Label("管理部门")]
        public static readonly IRefIdProperty ManagementIdProperty = P<EquipmentCard>.RegisterRefId(e => e.ManagementId, ReferenceType.Normal);

        /// <summary>
        /// 管理部门Id
        /// </summary>
        public double? ManagementId
        {
            get { return (double?)GetRefNullableId(ManagementIdProperty); }
            set { SetRefNullableId(ManagementIdProperty, value); }
        }

        /// <summary>
        /// 管理部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ManagementProperty = P<EquipmentCard>.RegisterRef(e => e.Management, ManagementIdProperty);

        /// <summary>
        /// 管理部门
        /// </summary>
        public Enterprise Management
        {
            get { return GetRefEntity(ManagementProperty); }
            set { SetRefEntity(ManagementProperty, value); }
        }
        #endregion

        #region 管理状态 AccountUseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState> AccountUseStateProperty = P<EquipmentCard>.Register(e => e.AccountUseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState AccountUseState
        {
            get { return GetProperty(AccountUseStateProperty); }
            set { SetProperty(AccountUseStateProperty, value); }
        }
        #endregion

        #region 使用部门 UseDepartment
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")] 
        public static readonly IRefIdProperty UseDepartmentIdProperty = P<EquipmentCard>.RegisterRefId(e => e.UseDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDepartmentId
        {
            get { return (double?)GetRefNullableId(UseDepartmentIdProperty); }
            set { SetRefNullableId(UseDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDepartmentProperty = P<EquipmentCard>.RegisterRef(e => e.UseDepartment, UseDepartmentIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDepartment
        {
            get { return GetRefEntity(UseDepartmentProperty); }
            set { SetRefEntity(UseDepartmentProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<EquipmentCard>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<EquipmentCard>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 卡片来源 EquipmentCardSource
        /// <summary>
        /// 卡片来源
        /// </summary>
        [Label("卡片来源")]
        public static readonly Property<EquipmentCardSource> EquipmentCardSourceProperty = P<EquipmentCard>.Register(e => e.EquipmentCardSource);

        /// <summary>
        /// 卡片来源
        /// </summary>
        public EquipmentCardSource EquipmentCardSource
        {
            get { return GetProperty(EquipmentCardSourceProperty); }
            set { SetProperty(EquipmentCardSourceProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<EquipmentCard>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<EquipmentCard>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<EquipmentCard>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<EquipmentCard>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<EquipmentCard>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty = P<EquipmentCard>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<EquipmentCard>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 保管人 Administrator
        /// <summary>
        /// 保管人Id
        /// </summary>
        [Label("保管人")]
        public static readonly IRefIdProperty AdministratorIdProperty = P<EquipmentCard>.RegisterRefId(e => e.AdministratorId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> AdministratorProperty = P<EquipmentCard>.RegisterRef(e => e.Administrator, AdministratorIdProperty);

        /// <summary>
        /// 保管人
        /// </summary>
        public Employee Administrator
        {
            get { return GetRefEntity(AdministratorProperty); }
            set { SetRefEntity(AdministratorProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipmentCard>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipmentCard>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 固定资产 FixedAssetsAccount
        /// <summary>
        /// 固定资产Id
        /// </summary>
        [Label("固定资产")]
        public static readonly IRefIdProperty FixedAssetsAccountIdProperty =
            P<EquipmentCard>.RegisterRefId(e => e.FixedAssetsAccountId, ReferenceType.Normal);

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
            P<EquipmentCard>.RegisterRef(e => e.FixedAssetsAccount, FixedAssetsAccountIdProperty);

        /// <summary>
        /// 固定资产
        /// </summary>
        public FixedAssetsAccount FixedAssetsAccount
        {
            get { return this.GetRefEntity(FixedAssetsAccountProperty); }
            set { this.SetRefEntity(FixedAssetsAccountProperty, value); }
        }
        #endregion

        #region  引用属性

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipmentCard>.RegisterView(e => e.EquipModelName, e => e.EquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return GetProperty(EquipModelNameProperty); }
            set { SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 设备类型 EquipModelType
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipModelTypeProperty = P<EquipmentCard>.RegisterView(e => e.EquipModelType, e => e.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipModelType
        {
            get { return GetProperty(EquipModelTypeProperty); }
            set { SetProperty(EquipModelTypeProperty, value); }
        }
        #endregion

        #region 设备规格 EquipModelSpecifications
        /// <summary>
        /// 设备规格
        /// </summary>
        [Label("设备规格")]
        public static readonly Property<string> EquipModelSpecificationsProperty = P<EquipmentCard>.RegisterView(e => e.EquipModelSpecifications, e => e.EquipModel.Specifications);

        /// <summary>
        /// 设备规格
        /// </summary>
        public string EquipModelSpecifications
        {
            get { return GetProperty(EquipModelSpecificationsProperty); }
            set { SetProperty(EquipModelSpecificationsProperty, value); }
        }
        #endregion

        #region 设备类别 EquipTypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> EquipTypeCategoryProperty = P<EquipmentCard>.RegisterView(e => e.EquipTypeCategory, e => e.EquipModel.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipTypeCategory
        {
            get { return GetProperty(EquipTypeCategoryProperty); }
            set { SetProperty(EquipTypeCategoryProperty, value); }
        }
        #endregion

        #region 行业属性 IndustryCategory
        /// <summary>
        /// 行业属性
        /// </summary>
        [Label("行业属性")]
        public static readonly Property<IndustryCategory> IndustryCategoryProperty = P<EquipmentCard>.RegisterView(e => e.IndustryCategory, e => e.EquipModel.IndustryCategory);

        /// <summary>
        /// 行业属性
        /// </summary>
        public IndustryCategory IndustryCategory
        {
            get { return GetProperty(IndustryCategoryProperty); }
            set { SetProperty(IndustryCategoryProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<EquipmentCard>.RegisterView(e => e.SupplierName, e => e.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return GetProperty(SupplierNameProperty); }
            set { SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 固定资产编码(视图属性) FixedAssetsAccountCode
        /// <summary>
        /// 固定资产编码(视图属性)
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetsAccountCodeProperty
            = P<EquipmentCard>.RegisterView(e => e.FixedAssetsAccountCode, p => p.FixedAssetsAccount.Code);

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
            = P<EquipmentCard>.RegisterView(e => e.FixedAssetsAccountName, p => p.FixedAssetsAccount.Name);

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
            = P<EquipmentCard>.RegisterView(e => e.OriginalAssetsValue, p => p.FixedAssetsAccount.OriginalAssetsValue);

        /// <summary>
        /// 原值(视图属性)
        /// </summary>
        public decimal OriginalAssetsValue
        {
            get { return this.GetProperty(OriginalAssetsValueProperty); }
        }
        #endregion

        #region 净值(视图属性) NetAssetValue
        /// <summary>
        /// 净值(视图属性)
        /// </summary>
        [Label("净值(视图属性)")]
        public static readonly Property<decimal> NetAssetValueProperty
            = P<EquipmentCard>.RegisterView(e => e.NetAssetValue, p => p.FixedAssetsAccount.NetAssetValue);

        /// <summary>
        /// 净值(视图属性)
        /// </summary>
        public decimal NetAssetValue
        {
            get { return this.GetProperty(NetAssetValueProperty); }
        }
        #endregion

        #region 审核意见 ApprovalInfo
        /// <summary>
        /// 审核意见
        /// </summary>
        [Label("审核意见")]
        public static readonly Property<string> ApprovalInfoProperty = P<EquipmentCard>.Register(e => e.ApprovalInfo);

        /// <summary>
        /// 审核意见
        /// </summary>
        public string ApprovalInfo
        {
            get { return this.GetProperty(ApprovalInfoProperty); }
            set { this.SetProperty(ApprovalInfoProperty, value); }
        }
        #endregion

        #region 是否启用固定资产 IsEnableAsset
        /// <summary>
        /// 是否启用固定资产
        /// </summary>
        [Label("是否启用固定资产")]
        public static readonly Property<bool> IsEnableAssetProperty = P<EquipmentCard>.Register(e => e.IsEnableAsset);

        /// <summary>
        /// 是否启用固定资产
        /// </summary>
        public bool IsEnableAsset
        {
            get { return this.GetProperty(IsEnableAssetProperty); }
            set { this.SetProperty(IsEnableAssetProperty, value); }
        }
        #endregion
        #endregion

        #region 不映射数据库字段
        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码(不映射数据库）
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<EquipmentCard>.Register(e => e.FactoryCode);

        /// <summary>
        /// 工厂编码(不映射数据库）
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
            set { this.SetProperty(FactoryCodeProperty, value); }
        }
        #endregion

        #region 使用部门编码 UseDepartmentCode
        /// <summary>
        /// 使用部门编码(不映射数据库）
        /// </summary>
        [Label("使用部门编码")]
        public static readonly Property<string> UseDepartmentCodeProperty = P<EquipmentCard>.Register(e => e.UseDepartmentCode);

        /// <summary>
        /// 使用部门编码(不映射数据库）
        /// </summary>
        public string UseDepartmentCode
        {
            get { return this.GetProperty(UseDepartmentCodeProperty); }
            set { this.SetProperty(UseDepartmentCodeProperty, value); }
        }
        #endregion

        #region 管理部门编码 ManagementCode
        /// <summary>
        /// 管理部门编码(不映射数据库）
        /// </summary>
        [Label("ManagementCode")]
        public static readonly Property<string> ManagementCodeProperty = P<EquipmentCard>.Register(e => e.ManagementCode);

        /// <summary>
        /// 管理部门编码(不映射数据库）
        /// </summary>
        public string ManagementCode
        {
            get { return this.GetProperty(ManagementCodeProperty); }
            set { this.SetProperty(ManagementCodeProperty, value); }
        }
        #endregion

        #region 车间编码(不映射数据库） WorkShopCode
        /// <summary>
        /// 车间编码(不映射数据库）
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<EquipmentCard>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间编码(不映射数据库）
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        #region 产线编码 ResourceCode
        /// <summary>
        /// 产线编码(不映射数据库）
        /// </summary>
        [Label("产线编码")]
        public static readonly Property<string> ResourceCodeProperty = P<EquipmentCard>.Register(e => e.ResourceCode);

        /// <summary>
        /// 产线编码(不映射数据库）
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备卡片 实体配置
    /// </summary>
    internal class EquipmentCardConfig : EntityConfig<EquipmentCard>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_CARD").MapAllProperties();
            Meta.Property(EquipmentCard.ApprovalInfoProperty).DontMapColumn();
            Meta.Property(EquipmentCard.IsEnableAssetProperty).DontMapColumn();
            Meta.Property(EquipmentCard.StorageLocationCodeProperty).DontMapColumn();            
            Meta.Property(EquipmentCard.FactoryCodeProperty).DontMapColumn();
            Meta.Property(EquipmentCard.UseDepartmentCodeProperty).DontMapColumn();
            Meta.Property(EquipmentCard.ManagementCodeProperty).DontMapColumn();
            Meta.Property(EquipmentCard.WorkShopCodeProperty).DontMapColumn();
            Meta.Property(EquipmentCard.ResourceCodeProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}