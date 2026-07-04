using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 设备清单
    /// </summary>
    [ChildEntity, Serializable]    
    [Label("设备清单")]
    public partial class FixedAssetDeviceBill : DataEntity
    {
        #region 设备台账  EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<FixedAssetDeviceBill>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<FixedAssetDeviceBill>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 固定资产台账 FixedAssetsAccount
        /// <summary>
        /// 固定资产台账Id
        /// </summary>
        public static readonly IRefIdProperty FixedAssetsAccountIdProperty = P<FixedAssetDeviceBill>.RegisterRefId(e => e.FixedAssetsAccountId, ReferenceType.Parent);

        /// <summary>
        /// 固定资产台账Id
        /// </summary>
        public double FixedAssetsAccountId
        {
            get { return (double)GetRefId(FixedAssetsAccountIdProperty); }
            set { SetRefId(FixedAssetsAccountIdProperty, value); }
        }

        /// <summary>
        /// 固定资产台账
        /// </summary>
        public static readonly RefEntityProperty<FixedAssetsAccount> FixedAssetsAccountProperty = P<FixedAssetDeviceBill>.RegisterRef(e => e.FixedAssetsAccount, FixedAssetsAccountIdProperty);

        /// <summary>
        /// 固定资产台账
        /// </summary>
        public FixedAssetsAccount FixedAssetsAccount
        {
            get { return GetRefEntity(FixedAssetsAccountProperty); }
            set { SetRefEntity(FixedAssetsAccountProperty, value); }
        }
        #endregion

        #region 主设备 IsMajor
        /// <summary>
        /// 主设备
        /// </summary>
        [Label("主设备")]
        public static readonly Property<bool> IsMajorProperty = P<FixedAssetDeviceBill>.Register(e => e.IsMajor);

        /// <summary>
        /// 主设备
        /// </summary>
        public bool IsMajor
        {
            get { return GetProperty(IsMajorProperty); }
            set { SetProperty(IsMajorProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备编码 Code
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> CodeProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.Code, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 设备名称 Name
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> NameProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.Name, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }

        #endregion

        #region 设备型号编码 ModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.ModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { SetProperty(ModelCodeProperty, value); }
        }
        #endregion 

        #region 设备型号名称 ModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.ModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 管理状态 UseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState> UseStateProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.UseState, p => p.EquipAccount.UseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState UseState
        {
            get { return this.GetProperty(UseStateProperty); }
        }
        #endregion

        #region 冻结 Frozen
        /// <summary>
        /// 冻结
        /// </summary>
        [Label("冻结")]
        public static readonly Property<YesNo> FrozenProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.Frozen, p => p.EquipAccount.Frozen);

        /// <summary>
        /// 冻结
        /// </summary>
        public YesNo Frozen
        {
            get { return this.GetProperty(FrozenProperty); }
        }
        #endregion

        #region 设备类型 EquipTypeCode
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeCodeProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.EquipTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #region 设备类型名称 EquipTypeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.EquipTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion

        #region 设备类别 EquipTypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> EquipTypeCategoryProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.EquipTypeCategory, p => p.EquipAccount.EquipModel.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipTypeCategory
        {
            get { return this.GetProperty(EquipTypeCategoryProperty); }
        }
        #endregion

        #region 海关监管设备 IsCustomsSupervision
        /// <summary>
        /// 海关监管设备
        /// </summary>
        [Label("海关监管设备")]
        public static readonly Property<bool> IsCustomsSupervisionProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.IsCustomsSupervision, p => p.EquipAccount.IsCustomsSupervision);

        /// <summary>
        /// 海关监管设备
        /// </summary>
        public bool IsCustomsSupervision
        {
            get { return this.GetProperty(IsCustomsSupervisionProperty); }
        }
        #endregion

        #region ABC分类 UseLevel
        /// <summary>
        /// ABC分类
        /// </summary>
        [Label("ABC分类")]
        public static readonly Property<string> UseLevelProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.UseLevel, p => p.EquipAccount.UseLevel);

        /// <summary>
        /// ABC分类
        /// </summary>
        public string UseLevel
        {
            get { return this.GetProperty(UseLevelProperty); }
        }
        #endregion

        #region 使用部门 UseDepartmentName
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentNameProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.UseDepartmentName, p => p.EquipAccount.UseDepartment.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartmentName
        {
            get { return this.GetProperty(UseDepartmentNameProperty); }
        }
        #endregion

        #region 生产厂家 Manufacturer
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Label("生产厂家")]
        public static readonly Property<string> ManufacturerProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.Manufacturer, p => p.EquipAccount.Manufacturer);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.SupplierCode, p => p.EquipAccount.Supplier.Code);

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
        public static readonly Property<string> SupplierNameProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.SupplierName, p => p.EquipAccount.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.PurchaseOrderNo, p => p.EquipAccount.PurchaseOrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
        }
        #endregion

        #region 入厂日期 EnterDate
        /// <summary>
        /// 入厂日期
        /// </summary>
        [Label("入厂日期")]
        public static readonly Property<DateTime?> EnterDateProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.EnterDate, p => p.EquipAccount.EnterDate);

        /// <summary>
        /// 入厂日期
        /// </summary>
        public DateTime? EnterDate
        {
            get { return this.GetProperty(EnterDateProperty); }
        }
        #endregion

        #region 使用年限 UsefulLife
        /// <summary>
        /// 使用年限
        /// </summary>
        [Label("使用年限")]
        public static readonly Property<string> UsefulLifeProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.UsefulLife, p => p.EquipAccount.UsefulLife);

        /// <summary>
        /// 使用年限
        /// </summary>
        public string UsefulLife
        {
            get { return this.GetProperty(UsefulLifeProperty); }
        }
        #endregion

        #region 保修期 WarrantyPeriod
        /// <summary>
        /// 保修期
        /// </summary>
        [Label("保修期")]
        public static readonly Property<DateTime?> WarrantyPeriodProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.WarrantyPeriod, p => p.EquipAccount.WarrantyPeriod);

        /// <summary>
        /// 保修期
        /// </summary>
        public DateTime? WarrantyPeriod
        {
            get { return this.GetProperty(WarrantyPeriodProperty); }
        }
        #endregion

        #region 位置 InstallationLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> InstallationLocationProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.InstallationLocation, p => p.EquipAccount.InstallationLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.Resource, p => p.EquipAccount.Resource.Name);

        /// <summary>
        /// 产线
        /// </summary>
        public string Resource
        {
            get { return this.GetProperty(ResourceProperty); }
            set { SetProperty(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<FixedAssetDeviceBill>.RegisterView(e => e.Process, p => p.EquipAccount.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { SetProperty(ProcessProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备清单 实体配置
    /// </summary>
    internal class FixedAssetDeviceBillConfig : EntityConfig<FixedAssetDeviceBill>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASSETSACCOUNT_DEVICE_BILL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}