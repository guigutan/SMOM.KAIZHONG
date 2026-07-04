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
using SIE.Resources.WorkCenters;
using SIE.Tech.Processs;
using SIE.Warehouses;
using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Code))]
    [EntityWithConfig(typeof(AccountNoConfig))]
    [EntityWithConfig(typeof(SmdcUrlConfig))]
    [EntityWithConfig(typeof(EquipAccountAssetConfig))]
    [ConditionQueryType(typeof(EquipAccountCriteria))]
    //[EquipAccountAuth(nameof(EquipModelId), nameof(UseDepartmentId), true)]
    //[EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [Label("设备台账")]
    public partial class EquipAccount : EquipAccountBase
    {
        /// <summary>
		/// 快码类型：精度级别
		/// </summary>
		public const string PrecisionClassType = "PRECISION_CLASS_TYPE";

        #region 缸槽列表 PcbSlotList
        /// <summary>
        /// 缸槽列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipAccountSlot>> PcbSlotListProperty = P<EquipAccount>.RegisterList(e => e.PcbSlotList);
        /// <summary>
        /// 缸槽列表
        /// </summary>
        public EntityList<EquipAccountSlot> PcbSlotList
        {
            get { return this.GetLazyList(PcbSlotListProperty); }
        }
        #endregion

        #region 设备履历列表 ResumeList
        /// <summary>
        /// 设备履历列表
        /// </summary>
        [Label("设备履历列表")]
        public static readonly ListProperty<EntityList<EquipAccountResume>> ResumeListProperty = P<EquipAccount>.RegisterList(e => e.ResumeList);

        /// <summary>
        /// 设备履历列表
        /// </summary>
        public EntityList<EquipAccountResume> ResumeList
        {
            get { return this.GetLazyList(ResumeListProperty); }
        }
        #endregion

        #region 工序列表 ProcessList
        /// <summary>
        /// 工序列表
        /// </summary>
        [Label("工序列表")]
        public static readonly ListProperty<EntityList<EquipAccountProcess>> ProcessListProperty = P<EquipAccount>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 工序列表
        /// </summary>
        public EntityList<EquipAccountProcess> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 设备位置列表 EquipAccountLocationList
        /// <summary>
        /// 设备位置列表
        /// </summary>
        [Label("设备位置列表")]
        public static readonly ListProperty<EntityList<EquipAccountLocation>> EquipAccountLocationListProperty = P<EquipAccount>.RegisterList(e => e.EquipAccountLocationList);

        /// <summary>
        /// 设备位置列表
        /// </summary>
        public EntityList<EquipAccountLocation> EquipAccountLocationList
        {
            get { return this.GetLazyList(EquipAccountLocationListProperty); }
        }
        #endregion

        #region 设备物联列表 EquipAccountPhysicalUnionList
        /// <summary>
        /// 设备物联列表
        /// </summary>
        [Label("设备物联列表")]
        public static readonly ListProperty<EntityList<EquipAccountPhysicalUnion>> EquipAccountPhysicalUnionListProperty = P<EquipAccount>.RegisterList(e => e.EquipAccountPhysicalUnionList);

        /// <summary>
        /// 设备物联列表
        /// </summary>
        public EntityList<EquipAccountPhysicalUnion> EquipAccountPhysicalUnionList
        {
            get { return this.GetLazyList(EquipAccountPhysicalUnionListProperty); }
        }
        #endregion

        #region 附加列表 AttachmentList
        /// <summary>
        /// 附加列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipAccountAttachment>> AttachmentListProperty
            = P<EquipAccount>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 附加列表
        /// </summary>
        public EntityList<EquipAccountAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 在线状态 DeviceOnLineState
        /// <summary>
        /// 在线状态
        /// </summary>
        [Label("在线状态")]
        public static readonly Property<EquipOnLineState?> EquipOnLineStateProperty = P<EquipAccount>.Register(e => e.EquipOnLineState);

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
            = P<EquipAccount>.RegisterView(e => e.IndustryCategory, p => p.EquipModel.IndustryCategory);

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
            = P<EquipAccount>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

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
            = P<EquipAccount>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

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
            = P<EquipAccount>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

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
            = P<EquipAccount>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

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
            = P<EquipAccount>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

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
            = P<EquipAccount>.RegisterView(e => e.UseDepartmentName, p => p.UseDepartment.Name);

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
        public static readonly Property<string> ManageDepartmentNameProperty = P<EquipAccount>.RegisterView(e => e.ManageDepartmentName, p => p.ManageDepartment.Name);

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
            = P<EquipAccount>.RegisterView(e => e.ResPersonName, p => p.ResPerson.Name);

        /// <summary>
        /// 责任人
        /// </summary>
        public string ResPersonName
        {
            get { return this.GetProperty(ResPersonNameProperty); }
        }
        #endregion

        #region 电子行业基础数据

        #region 注释 RailType
        /// <summary>
        /// 注释
        /// </summary>
        [Label("轨道类型")]
        public static readonly Property<OrbitType?> RailTypeProperty
            = P<EquipAccount>.RegisterView(e => e.RailType, p => p.EquipModel.RailType);

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
            = P<EquipAccount>.RegisterView(e => e.FeederBinding, p => p.EquipModel.FeederBinding);

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
            = P<EquipAccount>.RegisterView(e => e.FeederLocFailSafe, p => p.EquipModel.FeederLocFailSafe);

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
            = P<EquipAccount>.RegisterView(e => e.FeederBarcodeFailSafe, p => p.EquipModel.FeederBarcodeFailSafe);

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
            = P<EquipAccount>.RegisterView(e => e.VirtualDevice, p => p.EquipModel.VirtualDevice);

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
            = P<EquipAccount>.RegisterView(e => e.IsDisabled, p => p.EquipModel.IsDisabled);

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
            = P<EquipAccount>.RegisterView(e => e.AgingType, p => p.EquipModel.AgingType);

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
            = P<EquipAccount>.RegisterView(e => e.ProductionType, p => p.EquipModel.ProductionType);

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
        public static readonly Property<string> AssetCodeProperty = P<EquipAccount>.RegisterView(e => e.AssetCode, p => p.FixedAssetsAccount.Code);

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
        public static readonly Property<string> EquipModelTypeCodeProperty = P<EquipAccount>.RegisterView(e => e.EquipModelTypeCode, p => p.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipModelTypeCode
        {
            get { return this.GetProperty(EquipModelTypeCodeProperty); }
        }
        #endregion


        #endregion

        #region
        #region 原值 OriginalValue
        /// <summary>
        /// 原值
        /// </summary>
        [Label("原值")]
        public static readonly Property<decimal> OriginalValueProperty = P<EquipAccount>.Register(e => e.OriginalValue);

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
        public static readonly Property<bool?> IssAssetProperty = P<EquipAccount>.Register(e => e.IssAsset);

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
        public static readonly Property<string> AssetNameProperty = P<EquipAccount>.Register(e => e.AssetName);

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
            = P<EquipAccount>.RegisterView(e => e.FixedAssetsAccountCode, p => p.FixedAssetsAccount.Code);

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
            = P<EquipAccount>.RegisterView(e => e.FixedAssetsAccountName, p => p.FixedAssetsAccount.Name);

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
            = P<EquipAccount>.RegisterView(e => e.OriginalAssetsValue, p => p.FixedAssetsAccount.OriginalAssetsValue);

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
            = P<EquipAccount>.RegisterView(e => e.NetAssetValue, p => p.FixedAssetsAccount.NetAssetValue);

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
            = P<EquipAccount>.RegisterView(e => e.DepreciationResidualValue, p => p.FixedAssetsAccount.DepreciationResidualValue);

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
        public static readonly Property<string> SpecificationsProperty = P<EquipAccount>.RegisterView(e => e.Specifications, p => p.EquipModel.Specifications);

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
        public static readonly Property<string> WarehouseNameProperty = P<EquipAccount>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

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
        public static readonly Property<string> StorageLocationNameProperty = P<EquipAccount>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

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
        public static readonly Property<string> AdministratorNameProperty = P<EquipAccount>.RegisterView(e => e.AdministratorName, p => p.Administrator.Name);

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
        public static readonly Property<DateTime?> NextInspectionDateProperty = P<EquipAccount>.Register(e => e.NextInspectionDate);

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
        public static readonly Property<RegularInspectionStatus?> RegularInspectionStatusProperty = P<EquipAccount>.Register(e => e.RegularInspectionStatus);

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
        public static readonly Property<bool?> DowngradeProperty = P<EquipAccount>.Register(e => e.Downgrade);

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
        public static readonly Property<string> PrecisionClassProperty = P<EquipAccount>.Register(e => e.PrecisionClass);

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
        public static readonly Property<DateTime?> InspectionDateProperty = P<EquipAccount>.Register(e => e.InspectionDate);

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

        #region 不映射数据库的属性

        #region 报废类型 ScrapType
        /// <summary>
        /// 报废类型
        /// </summary>
        [Label("报废类型")]
        public static readonly Property<string> ScrapTypeProperty = P<EquipAccount>.Register(e => e.ScrapType);

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
        public static readonly Property<string> ReasonProperty = P<EquipAccount>.Register(e => e.Reason);

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
        public static readonly Property<bool> IsCalibrationProperty = P<EquipAccount>.Register(e => e.IsCalibration);

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

        #region 图号 Drawn
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawnProperty = P<EquipAccount>.Register(e => e.Drawn);

        /// <summary>
        /// 图号
        /// </summary>
        public string Drawn
        {
            get { return this.GetProperty(DrawnProperty); }
            set { this.SetProperty(DrawnProperty, value); }
        }
        #endregion

        #region 购置日期 PurchaseDate
        /// <summary>
        /// 购置日期
        /// </summary>
        [Label("购置日期")]
        public static readonly Property<DateTime?> PurchaseDateProperty = P<EquipAccount>.Register(e => e.PurchaseDate);

        /// <summary>
        /// 购置日期
        /// </summary>
        public DateTime? PurchaseDate
        {
            get { return this.GetProperty(PurchaseDateProperty); }
            set { this.SetProperty(PurchaseDateProperty, value); }
        }
        #endregion

        #region 成本中心代码 CostCenterCode
        /// <summary>
        /// 成本中心代码
        /// </summary>
        [Label("成本中心代码")]
        public static readonly Property<string> CostCenterCodeProperty = P<EquipAccount>.Register(e => e.CostCenterCode);

        /// <summary>
        /// 成本中心代码
        /// </summary>
        public string CostCenterCode
        {
            get { return this.GetProperty(CostCenterCodeProperty); }
            set { this.SetProperty(CostCenterCodeProperty, value); }
        }
        #endregion

        #region 工作中心 WorkCenter
        /// <summary>
        /// 工作中心Id
        /// </summary>
        [Label("工作中心")]
        public static readonly IRefIdProperty WorkCenterIdProperty =
            P<EquipAccount>.RegisterRefId(e => e.WorkCenterId, ReferenceType.Normal);

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public double? WorkCenterId
        {
            get { return (double?)this.GetRefNullableId(WorkCenterIdProperty); }
            set { this.SetRefNullableId(WorkCenterIdProperty, value); }
        }

        /// <summary>
        /// 工作中心
        /// </summary>
        public static readonly RefEntityProperty<WorkCenter> WorkCenterProperty =
            P<EquipAccount>.RegisterRef(e => e.WorkCenter, WorkCenterIdProperty);

        /// <summary>
        /// 工作中心
        /// </summary>
        public WorkCenter WorkCenter
        {
            get { return this.GetRefEntity(WorkCenterProperty); }
            set { this.SetRefEntity(WorkCenterProperty, value); }
        }
        #endregion

        #region 系列号 SerialNumber
        /// <summary>
        /// 系列号
        /// </summary>
        [Label("系列号")]
        public static readonly Property<string> SerialNumberProperty = P<EquipAccount>.Register(e => e.SerialNumber);

        /// <summary>
        /// 系列号
        /// </summary>
        public string SerialNumber
        {
            get { return this.GetProperty(SerialNumberProperty); }
            set { this.SetProperty(SerialNumberProperty, value); }
        }
        #endregion

        #region 穴位 Acupoint
        /// <summary>
        /// 穴位
        /// </summary>
        [Label("穴位")]
        public static readonly Property<string> AcupointProperty = P<EquipAccount>.Register(e => e.Acupoint);

        /// <summary>
        /// 穴位
        /// </summary>
        public string Acupoint
        {
            get { return this.GetProperty(AcupointProperty); }
            set { this.SetProperty(AcupointProperty, value); }
        }
        #endregion

        #region 功能位置 FunctionalLocation
        /// <summary>
        /// 功能位置
        /// </summary>
        [Label("功能位置")]
        public static readonly Property<string> FunctionalLocationProperty = P<EquipAccount>.Register(e => e.FunctionalLocation);

        /// <summary>
        /// 功能位置
        /// </summary>
        public string FunctionalLocation
        {
            get { return this.GetProperty(FunctionalLocationProperty); }
            set { this.SetProperty(FunctionalLocationProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 设备台账 实体配置
    /// </summary>
    internal class EquipAccountConfig : EntityConfig<EquipAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();
            Meta.SupportTree();
            Meta.Property(EquipAccount.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccount.FactoryIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccount.ManageDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccount.UseDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EquipAccount.EquipModelIdProperty).ColumnMeta.HasIndex();


            Meta.Property(EquipAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(EquipAccount.ReasonProperty).DontMapColumn();
            Meta.Property(EquipAccount.IsCalibrationProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}