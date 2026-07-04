using SIE.Domain;
using SIE.EMS.AssetIssues;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.AssetReturns
{
    /// <summary>
    /// 归还设备清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备清单")]
    public partial class AssetReturnEquipment : DataEntity
    {
        #region 资产归还单 AssetReturn
        /// <summary>
        /// 资产归还单Id
        /// </summary>
        [Label("资产归还单")]
        public static readonly IRefIdProperty AssetReturnIdProperty = P<AssetReturnEquipment>.RegisterRefId(e => e.AssetReturnId, ReferenceType.Parent);

        /// <summary>
        /// 资产归还单Id
        /// </summary>
        public double AssetReturnId
        {
            get { return (double)GetRefId(AssetReturnIdProperty); }
            set { SetRefId(AssetReturnIdProperty, value); }
        }

        /// <summary>
        /// 资产归还单
        /// </summary>
        public static readonly RefEntityProperty<AssetReturn> AssetReturnProperty = P<AssetReturnEquipment>.RegisterRef(e => e.AssetReturn, AssetReturnIdProperty);

        /// <summary>
        /// 资产归还单
        /// </summary>
        public AssetReturn AssetReturn
        {
            get { return GetRefEntity(AssetReturnProperty); }
            set { SetRefEntity(AssetReturnProperty, value); }
        }
        #endregion

        #region 归还单号 ReturnNo
        /// <summary>
        /// 归还单号
        /// </summary>
        [Label("归还单号")]
        public static readonly Property<string> ReturnNoProperty = P<AssetReturnEquipment>.Register(e => e.ReturnNo);

        /// <summary>
        /// 归还单号
        /// </summary>
        public string ReturnNo
        {
            get { return GetProperty(ReturnNoProperty); }
            set { SetProperty(ReturnNoProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<AssetReturnEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<AssetReturnEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 归还类型 ReturnType
        /// <summary>
        /// 归还类型
        /// </summary>
        [Label("归还类型")]
        public static readonly Property<ReturnType?> ReturnTypeProperty = P<AssetReturnEquipment>.Register(e => e.ReturnType);

        /// <summary>
        /// 归还类型
        /// </summary>
        public ReturnType? ReturnType
        {
            get { return GetProperty(ReturnTypeProperty); }
            set { SetProperty(ReturnTypeProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<AssetReturnEquipment>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<AssetReturnEquipment>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty = P<AssetReturnEquipment>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<AssetReturnEquipment>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 位置 Location
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> LocationProperty = P<AssetReturnEquipment>.Register(e => e.Location);

        /// <summary>
        /// 位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 保管人 Depositary
        /// <summary>
        /// 保管人Id
        /// </summary>
        [Label("保管人")]
        public static readonly IRefIdProperty DepositaryIdProperty = P<AssetReturnEquipment>.RegisterRefId(e => e.DepositaryId, ReferenceType.Normal);

        /// <summary>
        /// 保管人Id
        /// </summary>
        public double? DepositaryId
        {
            get { return (double?)GetRefNullableId(DepositaryIdProperty); }
            set { SetRefNullableId(DepositaryIdProperty, value); }
        }

        /// <summary>
        /// 保管人
        /// </summary>
        public static readonly RefEntityProperty<Employee> DepositaryProperty = P<AssetReturnEquipment>.RegisterRef(e => e.Depositary, DepositaryIdProperty);

        /// <summary>
        /// 保管人
        /// </summary>
        public Employee Depositary
        {
            get { return GetRefEntity(DepositaryProperty); }
            set { SetRefEntity(DepositaryProperty, value); }
        }
        #endregion

        #region 实际归还日期 ReturnDate
        /// <summary>
        /// 实际归还日期
        /// </summary>
        [Label("实际归还日期")]
        public static readonly Property<DateTime?> ReturnDateProperty = P<AssetReturnEquipment>.Register(e => e.ReturnDate);

        /// <summary>
        /// 实际归还日期
        /// </summary>
        public DateTime? ReturnDate
        {
            get { return GetProperty(ReturnDateProperty); }
            set { SetProperty(ReturnDateProperty, value); }
        }
        #endregion

        #region 领用设备明细 AssetRequisitionEquipment
        /// <summary>
        /// 领用设备明细Id
        /// </summary>
        [Label("领用设备明细")]
        public static readonly IRefIdProperty AssetRequisitionEquipmentIdProperty = P<AssetReturnEquipment>.RegisterRefId(e => e.AssetRequisitionEquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 领用设备明细Id
        /// </summary>
        public double AssetRequisitionEquipmentId
        {
            get { return (double)GetRefId(AssetRequisitionEquipmentIdProperty); }
            set { SetRefId(AssetRequisitionEquipmentIdProperty, value); }
        }

        /// <summary>
        /// 领用设备明细
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisitionEquipment> AssetRequisitionEquipmentProperty = P<AssetReturnEquipment>.RegisterRef(e => e.AssetRequisitionEquipment, AssetRequisitionEquipmentIdProperty);

        /// <summary>
        /// 领用设备明细
        /// </summary>
        public AssetRequisitionEquipment AssetRequisitionEquipment
        {
            get { return GetRefEntity(AssetRequisitionEquipmentProperty); }
            set { SetRefEntity(AssetRequisitionEquipmentProperty, value); }
        }
        #endregion

        #region 发放设备明细 AssetIssueEquipment
        /// <summary>
        /// 发放设备明细Id
        /// </summary>
        [Label("发放设备明细")]
        public static readonly IRefIdProperty AssetIssueEquipmentIdProperty =
            P<AssetReturnEquipment>.RegisterRefId(e => e.AssetIssueEquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 发放设备明细Id
        /// </summary>
        public double? AssetIssueEquipmentId
        {
            get { return (double?)this.GetRefNullableId(AssetIssueEquipmentIdProperty); }
            set { this.SetRefNullableId(AssetIssueEquipmentIdProperty, value); }
        }

        /// <summary>
        /// 发放设备明细
        /// </summary>
        public static readonly RefEntityProperty<AssetIssueEquipment> AssetIssueEquipmentProperty =
            P<AssetReturnEquipment>.RegisterRef(e => e.AssetIssueEquipment, AssetIssueEquipmentIdProperty);

        /// <summary>
        /// 发放设备明细
        /// </summary>
        public AssetIssueEquipment AssetIssueEquipment
        {
            get { return this.GetRefEntity(AssetIssueEquipmentProperty); }
            set { this.SetRefEntity(AssetIssueEquipmentProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 借用行号 LineNo
        /// <summary>
        /// 借用行号
        /// </summary>
        [Label("借用行号")]
        public static readonly Property<string> LineNoProperty = P<AssetReturnEquipment>.RegisterView(e => e.LineNo, p => p.AssetRequisitionEquipment.LineNo);

        /// <summary>
        /// 借用行号
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
        public static readonly Property<string> EquipAccountCodeProperty = P<AssetReturnEquipment>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

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
        public static readonly Property<string> EquipAccountNameProperty = P<AssetReturnEquipment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 设备别名 Alias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> AliasProperty = P<AssetReturnEquipment>.RegisterView(e => e.Alias, p => p.EquipAccount.Alias);

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
        public static readonly Property<string> EquipModelCodeProperty = P<AssetReturnEquipment>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

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
        public static readonly Property<string> EquipModelNameProperty = P<AssetReturnEquipment>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

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
        public static readonly Property<string> SpecificationsProperty = P<AssetReturnEquipment>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
            set { this.SetProperty(SpecificationsProperty, value); }
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
            P<AssetReturnEquipment>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<AssetReturnEquipment>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 是否已选明细行 IsSelected
        /// <summary>
        /// 是否已选明细行
        /// </summary>
        [Label("是否已选明细行")]
        public static readonly Property<bool> IsSelectedProperty = P<AssetReturnEquipment>.Register(e => e.IsSelected);

        /// <summary>
        /// 是否已选明细行
        /// </summary>
        public bool IsSelected
        {
            get { return this.GetProperty(IsSelectedProperty); }
            set { this.SetProperty(IsSelectedProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetReturnEquipment>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 归还设备清单 实体配置
    /// </summary>
    internal class AssetReturnEquipmentConfig : EntityConfig<AssetReturnEquipment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_RETURN_EQP").MapAllProperties();
            Meta.Property(AssetReturnEquipment.FactoryIdProperty).DontMapColumn();
            Meta.Property(AssetReturnEquipment.FactoryProperty).DontMapColumn();
            Meta.Property(AssetReturnEquipment.IsSelectedProperty).DontMapColumn();
            Meta.Property(AssetReturnEquipment.ApprovalStatusProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}