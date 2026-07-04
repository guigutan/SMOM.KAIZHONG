using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试设备明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安装调试设备明细")]
    public partial class EquipmentDetail : DataEntity
    {
        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipmentDetail>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<EquipmentDetail>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备明细 EquipmentSetup
        /// <summary>
        /// 设备明细Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupIdProperty = P<EquipmentDetail>.RegisterRefId(e => e.EquipmentSetupId, ReferenceType.Parent);

        /// <summary>
        /// 设备明细Id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return (double)GetRefId(EquipmentSetupIdProperty); }
            set { SetRefId(EquipmentSetupIdProperty, value); }
        }

        /// <summary>
        /// 设备明细
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetup> EquipmentSetupProperty = P<EquipmentDetail>.RegisterRef(e => e.EquipmentSetup, EquipmentSetupIdProperty);

        /// <summary>
        /// 设备明细
        /// </summary>
        public EquipmentSetup EquipmentSetup
        {
            get { return GetRefEntity(EquipmentSetupProperty); }
            set { SetRefEntity(EquipmentSetupProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipmentDetail>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备别名 EquipAccountAlias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> EquipAccountAliasProperty = P<EquipmentDetail>.RegisterView(e => e.EquipAccountAlias, p => p.EquipAccount.Alias);

        /// <summary>
        /// 设备别名
        /// </summary>
        public string EquipAccountAlias
        {
            get { return this.GetProperty(EquipAccountAliasProperty); }
        }
        #endregion

        #region 设备型号 EquipAccountModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipAccountModelCodeProperty = P<EquipmentDetail>.RegisterView(e => e.EquipAccountModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipAccountModelCode
        {
            get { return this.GetProperty(EquipAccountModelCodeProperty); }
        }
        #endregion

        #region 型号名称 EquipAccountModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipAccountModelNameProperty = P<EquipmentDetail>.RegisterView(e => e.EquipAccountModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipAccountModelName
        {
            get { return this.GetProperty(EquipAccountModelNameProperty); }
        }
        #endregion

        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<EquipmentDetail>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
        }
        #endregion

        #region 管理部门 ManageDepartmentName
        /// <summary>
        /// 管理部门
        /// </summary>
        [Label("管理部门")]
        public static readonly Property<string> ManageDepartmentNameProperty = P<EquipmentDetail>.RegisterView(e => e.ManageDepartmentName, p => p.EquipAccount.ManageDepartment.Name);

        /// <summary>
        /// 管理部门
        /// </summary>
        public string ManageDepartmentName
        {
            get { return this.GetProperty(ManageDepartmentNameProperty); }
        }
        #endregion

        #region 使用部门 UseDepartmentName
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentNameProperty = P<EquipmentDetail>.RegisterView(e => e.UseDepartmentName, p => p.EquipAccount.UseDepartment.Name);

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
        public static readonly Property<string> ManufacturerProperty = P<EquipmentDetail>.RegisterView(e => e.Manufacturer, p => p.EquipAccount.Manufacturer);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
        }
        #endregion

        #region 原厂序列号 OriginalSerialNumber
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSerialNumberProperty = P<EquipmentDetail>.RegisterView(e => e.OriginalSerialNumber, p => p.EquipAccount.OriginalSerialNumber);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSerialNumber
        {
            get { return this.GetProperty(OriginalSerialNumberProperty); }
        }
        #endregion

        #region 保修期 WarrantyPeriod
        /// <summary>
        /// 保修期
        /// </summary>
        [Label("保修期")]
        public static readonly Property<DateTime?> WarrantyPeriodProperty = P<EquipmentDetail>.RegisterView(e => e.WarrantyPeriod, p => p.EquipAccount.WarrantyPeriod);

        /// <summary>
        /// 保修期
        /// </summary>
        public DateTime? WarrantyPeriod
        {
            get { return this.GetProperty(WarrantyPeriodProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安装调试设备明细 实体配置
    /// </summary>
    internal class EquipmentDetailConfig : EntityConfig<EquipmentDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP_EQP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}