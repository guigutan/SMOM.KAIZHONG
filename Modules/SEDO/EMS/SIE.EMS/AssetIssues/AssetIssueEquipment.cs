using SIE;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.AssetIssues
{
    /// <summary>
    /// 发放设备清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("发放设备清单")]
    public partial class AssetIssueEquipment : DataEntity
    {
        #region 资产发放 AssetIssue
        /// <summary>
        /// 资产发放Id
        /// </summary>
        [Label("资产发放")]
        public static readonly IRefIdProperty AssetIssueIdProperty = P<AssetIssueEquipment>.RegisterRefId(e => e.AssetIssueId, ReferenceType.Parent);

        /// <summary>
        /// 资产发放Id
        /// </summary>
        public double AssetIssueId
        {
            get { return (double)GetRefId(AssetIssueIdProperty); }
            set { SetRefId(AssetIssueIdProperty, value); }
        }

        /// <summary>
        /// 资产发放
        /// </summary>
        public static readonly RefEntityProperty<AssetIssue> AssetIssueProperty = P<AssetIssueEquipment>.RegisterRef(e => e.AssetIssue, AssetIssueIdProperty);

        /// <summary>
        /// 资产发放
        /// </summary>
        public AssetIssue AssetIssue
        {
            get { return GetRefEntity(AssetIssueProperty); }
            set { SetRefEntity(AssetIssueProperty, value); }
        }
        #endregion

        #region 领用申请设备清单 AssetRequisitionEquipment
        /// <summary>
        /// 领用申请设备清单Id
        /// </summary>
        [Label("领用申请设备清单")]
        public static readonly IRefIdProperty AssetRequisitionEquipmentIdProperty = P<AssetIssueEquipment>.RegisterRefId(e => e.AssetRequisitionEquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 领用申请设备清单Id
        /// </summary>
        public double AssetRequisitionEquipmentId
        {
            get { return (double)GetRefId(AssetRequisitionEquipmentIdProperty); }
            set { SetRefId(AssetRequisitionEquipmentIdProperty, value); }
        }

        /// <summary>
        /// 领用申请设备清单
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisitionEquipment> AssetRequisitionEquipmentProperty = P<AssetIssueEquipment>.RegisterRef(e => e.AssetRequisitionEquipment, AssetRequisitionEquipmentIdProperty);

        /// <summary>
        /// 领用申请设备清单
        /// </summary>
        public AssetRequisitionEquipment AssetRequisitionEquipment
        {
            get { return GetRefEntity(AssetRequisitionEquipmentProperty); }
            set { SetRefEntity(AssetRequisitionEquipmentProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<AssetIssueEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<AssetIssueEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 归还状态 ReturnStatus
        /// <summary>
        /// 归还状态
        /// </summary>
        [Label("归还状态")]
        public static readonly Property<ReturnStatus> ReturnStatusProperty = P<AssetIssueEquipment>.Register(e => e.ReturnStatus);

        /// <summary>
        /// 归还状态
        /// </summary>
        public ReturnStatus ReturnStatus
        {
            get { return GetProperty(ReturnStatusProperty); }
            set { SetProperty(ReturnStatusProperty, value); }
        }
        #endregion

        #region 归还单号 ReturnNo
        /// <summary>
        /// 归还单号
        /// </summary>
        [Label("归还单号")]
        public static readonly Property<string> ReturnNoProperty = P<AssetIssueEquipment>.Register(e => e.ReturnNo);

        /// <summary>
        /// 归还单号
        /// </summary>
        public string ReturnNo
        {
            get { return GetProperty(ReturnNoProperty); }
            set { SetProperty(ReturnNoProperty, value); }
        }
        #endregion

        #region 实际归还日期 ReturnDate
        /// <summary>
        /// 实际归还日期
        /// </summary>
        [Label("实际归还日期")]
        public static readonly Property<DateTime?> ReturnDateProperty = P<AssetIssueEquipment>.Register(e => e.ReturnDate);

        /// <summary>
        /// 实际归还日期
        /// </summary>
        public DateTime? ReturnDate
        {
            get { return GetProperty(ReturnDateProperty); }
            set { SetProperty(ReturnDateProperty, value); }
        }
        #endregion

        #region 领用单是否已选设备编码 IsSelectEquipAccount
        /// <summary>
        /// 领用单是否已选设备编码
        /// </summary>
        [Label("是否已选设备编码")]
        public static readonly Property<bool> IsSelectEquipAccountProperty = P<AssetIssueEquipment>.Register(e => e.IsSelectEquipAccount);

        /// <summary>
        /// 领用单是否已选设备编码
        /// </summary>
        public bool IsSelectEquipAccount
        {
            get { return GetProperty(IsSelectEquipAccountProperty); }
            set { SetProperty(IsSelectEquipAccountProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 申请行号 LineNo
        /// <summary>
        /// 申请行号
        /// </summary>
        [Label("申请行号")]
        public static readonly Property<string> LineNoProperty = P<AssetIssueEquipment>.RegisterView(e => e.LineNo, p => p.AssetRequisitionEquipment.LineNo);

        /// <summary>
        /// 申请行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<AssetIssueEquipment>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { this.SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<AssetIssueEquipment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 管理状态 UseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> UseStateProperty = P<AssetIssueEquipment>.RegisterView(e => e.UseState, p => p.EquipAccount.UseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState? UseState
        {
            get { return this.GetProperty(UseStateProperty); }
            set { this.SetProperty(UseStateProperty, value); }
        }
        #endregion

        #region 设备别名 Alias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> AliasProperty = P<AssetIssueEquipment>.RegisterView(e => e.Alias, p => p.EquipAccount.Alias);

        /// <summary>
        /// 设备别名
        /// </summary>
        public string Alias
        {
            get { return this.GetProperty(AliasProperty); }
            set { this.SetProperty(AliasProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelCodeProperty = P<AssetIssueEquipment>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
            set { this.SetProperty(EquipModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<AssetIssueEquipment>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<AssetIssueEquipment>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
            set { this.SetProperty(SpecificationsProperty, value); }
        }
        #endregion

        #region 设备类型 EquipTypeCode
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeCodeProperty = P<AssetIssueEquipment>.RegisterView(e => e.EquipTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
            set { this.SetProperty(EquipTypeCodeProperty, value); }
        }
        #endregion

        #region 类型名称 EquipTypeName
        /// <summary>
        /// 类型名称
        /// </summary>
        [Label("类型名称")]
        public static readonly Property<string> EquipTypeNameProperty = P<AssetIssueEquipment>.RegisterView(e => e.EquipTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
            set { this.SetProperty(EquipTypeNameProperty, value); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AssetIssueEquipment>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AssetIssueEquipment>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 借出部门 LendingDepartment
        /// <summary>
        /// 借出部门Id
        /// </summary>
        [Label("借出部门")]
        public static readonly IRefIdProperty LendingDepartmentIdProperty =
            P<AssetIssueEquipment>.RegisterRefId(e => e.LendingDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 借出部门Id
        /// </summary>
        public double? LendingDepartmentId
        {
            get { return (double?)this.GetRefNullableId(LendingDepartmentIdProperty); }
            set { this.SetRefNullableId(LendingDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 借出部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LendingDepartmentProperty =
            P<AssetIssueEquipment>.RegisterRef(e => e.LendingDepartment, LendingDepartmentIdProperty);

        /// <summary>
        /// 借出部门
        /// </summary>
        public Enterprise LendingDepartment
        {
            get { return this.GetRefEntity(LendingDepartmentProperty); }
            set { this.SetRefEntity(LendingDepartmentProperty, value); }
        }
        #endregion

        #region 是否已选明细行 IsSelected
        /// <summary>
        /// 是否已选明细行
        /// </summary>
        [Label("是否已选明细行")]
        public static readonly Property<bool> IsSelectedProperty = P<AssetIssueEquipment>.Register(e => e.IsSelected);

        /// <summary>
        /// 是否已选明细行
        /// </summary>
        public bool IsSelected
        {
            get { return this.GetProperty(IsSelectedProperty); }
            set { this.SetProperty(IsSelectedProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 发放设备清单 实体配置
    /// </summary>
    internal class AssetIssueEquipmentConfig : EntityConfig<AssetIssueEquipment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_ISSUE_EQP").MapAllProperties();
            Meta.Property(AssetIssueEquipment.FactoryIdProperty).DontMapColumn();
            Meta.Property(AssetIssueEquipment.FactoryProperty).DontMapColumn();
            Meta.Property(AssetIssueEquipment.LendingDepartmentIdProperty).DontMapColumn();
            Meta.Property(AssetIssueEquipment.LendingDepartmentProperty).DontMapColumn();
            Meta.Property(AssetIssueEquipment.IsSelectedProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}