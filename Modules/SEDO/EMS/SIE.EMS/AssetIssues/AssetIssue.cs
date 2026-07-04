using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.AssetIssues
{
    /// <summary>
    /// 资产发放
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AssetIssueCriteria))]
    [EntityWithConfig(typeof(NoConfig), "资产发放单号生成规则配置项", "资产发放单号生成规则")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [Label("资产发放")]
    public partial class AssetIssue : DataEntity
    {
        #region 发放单号 IssueNo
        /// <summary>
        /// 发放单号
        /// </summary>
        [Label("发放单号")]
        public static readonly Property<string> IssueNoProperty = P<AssetIssue>.Register(e => e.IssueNo);

        /// <summary>
        /// 发放单号
        /// </summary>
        public string IssueNo
        {
            get { return GetProperty(IssueNoProperty); }
            set { SetProperty(IssueNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<AssetIssue>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<AssetIssue>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 资产领用 AssetRequisition
        /// <summary>
        /// 资产领用Id
        /// </summary>
        [Label("资产领用")]
        public static readonly IRefIdProperty AssetRequisitionIdProperty = P<AssetIssue>.RegisterRefId(e => e.AssetRequisitionId, ReferenceType.Normal);

        /// <summary>
        /// 资产领用Id
        /// </summary>
        public double AssetRequisitionId
        {
            get { return (double)GetRefId(AssetRequisitionIdProperty); }
            set { SetRefId(AssetRequisitionIdProperty, value); }
        }

        /// <summary>
        /// 资产领用
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisition> AssetRequisitionProperty = P<AssetIssue>.RegisterRef(e => e.AssetRequisition, AssetRequisitionIdProperty);

        /// <summary>
        /// 资产领用
        /// </summary>
        public AssetRequisition AssetRequisition
        {
            get { return GetRefEntity(AssetRequisitionProperty); }
            set { SetRefEntity(AssetRequisitionProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetIssue>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 申请部门 ApplyDepartment
        /// <summary>
        /// 申请部门Id
        /// </summary>
        [Label("申请部门")]
        public static readonly IRefIdProperty ApplyDepartmentIdProperty = P<AssetIssue>.RegisterRefId(e => e.ApplyDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 申请部门Id
        /// </summary>
        public double ApplyDepartmentId
        {
            get { return (double)GetRefId(ApplyDepartmentIdProperty); }
            set { SetRefId(ApplyDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 申请部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ApplyDepartmentProperty = P<AssetIssue>.RegisterRef(e => e.ApplyDepartment, ApplyDepartmentIdProperty);

        /// <summary>
        /// 申请部门
        /// </summary>
        public Enterprise ApplyDepartment
        {
            get { return GetRefEntity(ApplyDepartmentProperty); }
            set { SetRefEntity(ApplyDepartmentProperty, value); }
        }
        #endregion

        #region 借出部门 LendingDepartment
        /// <summary>
        /// 借出部门Id
        /// </summary>
        [Label("借出部门")]
        public static readonly IRefIdProperty LendingDepartmentIdProperty =
            P<AssetIssue>.RegisterRefId(e => e.LendingDepartmentId, ReferenceType.Normal);

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
            P<AssetIssue>.RegisterRef(e => e.LendingDepartment, LendingDepartmentIdProperty);

        /// <summary>
        /// 借出部门
        /// </summary>
        public Enterprise LendingDepartment
        {
            get { return this.GetRefEntity(LendingDepartmentProperty); }
            set { this.SetRefEntity(LendingDepartmentProperty, value); }
        }
        #endregion

        #region 发放仓库 Warehouse
        /// <summary>
        /// 发放仓库Id
        /// </summary>
        [Label("发放仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetIssue>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发放仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发放仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetIssue>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发放仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 联系人 ContactPerson
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactPersonProperty = P<AssetIssue>.Register(e => e.ContactPerson);

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson
        {
            get { return GetProperty(ContactPersonProperty); }
            set { SetProperty(ContactPersonProperty, value); }
        }
        #endregion

        #region 联系方式 ContactInformation
        /// <summary>
        /// 联系方式
        /// </summary>
        [Label("联系方式")]
        public static readonly Property<string> ContactInformationProperty = P<AssetIssue>.Register(e => e.ContactInformation);

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInformation
        {
            get { return GetProperty(ContactInformationProperty); }
            set { SetProperty(ContactInformationProperty, value); }
        }
        #endregion

        #region 联系地址 Address
        /// <summary>
        /// 联系地址
        /// </summary>
        [Label("联系地址")]
        public static readonly Property<string> AddressProperty = P<AssetIssue>.Register(e => e.Address);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address
        {
            get { return GetProperty(AddressProperty); }
            set { SetProperty(AddressProperty, value); }
        }
        #endregion

        #region 发货方式 DeliveryWay
        /// <summary>
        /// 发货方式
        /// </summary>
        [Label("发货方式")]
        public static readonly Property<DeliveryWay?> DeliveryWayProperty = P<AssetIssue>.Register(e => e.DeliveryWay);

        /// <summary>
        /// 发货方式
        /// </summary>
        public DeliveryWay? DeliveryWay
        {
            get { return GetProperty(DeliveryWayProperty); }
            set { SetProperty(DeliveryWayProperty, value); }
        }
        #endregion

        #region 快递/出厂单号 TrackNo
        /// <summary>
        /// 快递/出厂单号
        /// </summary>
        [Label("快递/出厂单号")]
        public static readonly Property<string> TrackNoProperty = P<AssetIssue>.Register(e => e.TrackNo);

        /// <summary>
        /// 快递/出厂单号
        /// </summary>
        public string TrackNo
        {
            get { return GetProperty(TrackNoProperty); }
            set { SetProperty(TrackNoProperty, value); }
        }
        #endregion

        #region 设备清单 AssetIssueEquipmentList
        /// <summary>
        /// 设备清单
        /// </summary>
        [Label("设备清单")]
        public static readonly ListProperty<EntityList<AssetIssueEquipment>> AssetIssueEquipmentListProperty = P<AssetIssue>.RegisterList(e => e.AssetIssueEquipmentList);
        /// <summary>
        /// 设备清单
        /// </summary>
        public EntityList<AssetIssueEquipment> AssetIssueEquipmentList
        {
            get { return this.GetLazyList(AssetIssueEquipmentListProperty); }
        }
        #endregion

        #region 工治具清单 AssetIssueFixtureList
        /// <summary>
        /// 工治具清单
        /// </summary>
        [Label("工治具清单")]
        public static readonly ListProperty<EntityList<AssetIssueFixture>> AssetIssueFixtureListProperty = P<AssetIssue>.RegisterList(e => e.AssetIssueFixtureList);
        /// <summary>
        /// 工治具清单
        /// </summary>
        public EntityList<AssetIssueFixture> AssetIssueFixtureList
        {
            get { return this.GetLazyList(AssetIssueFixtureListProperty); }
        }
        #endregion

        #region 视图属性

        #region 领用单号 RequisitionNo
        /// <summary>
        /// 领用单号
        /// </summary>
        [Label("领用单号")]
        public static readonly Property<string> RequisitionNoProperty = P<AssetIssue>.RegisterView(e => e.RequisitionNo, p => p.AssetRequisition.RequisitionNo);

        /// <summary>
        /// 领用单号
        /// </summary>
        public string RequisitionNo
        {
            get { return this.GetProperty(RequisitionNoProperty); }
        }
        #endregion

        #region 领用人 EmployeeId
        /// <summary>
        /// 领用人
        /// </summary>
        [Label("领用人")]
        public static readonly Property<double> EmployeeIdProperty = P<AssetIssue>.RegisterView(e => e.EmployeeId, p => p.AssetRequisition.EmployeeId);

        /// <summary>
        /// 领用人
        /// </summary>
        public double EmployeeId
        {
            get { return this.GetProperty(EmployeeIdProperty); }
        }
        #endregion

        #region 业务类型 RequisitionType
        /// <summary>
        /// 业务类型
        /// </summary>
        [Label("业务类型")]
        public static readonly Property<RequisitionType> RequisitionTypeProperty = P<AssetIssue>.RegisterView(e => e.RequisitionType, p => p.AssetRequisition.RequisitionType);

        /// <summary>
        /// 业务类型
        /// </summary>
        public RequisitionType RequisitionType
        {
            get { return this.GetProperty(RequisitionTypeProperty); }
        }
        #endregion

        #region 资产对象 AssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
        [Label("资产对象")]
        public static readonly Property<AssetObject> AssetObjectProperty = P<AssetIssue>.RegisterView(e => e.AssetObject, p => p.AssetRequisition.AssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public AssetObject AssetObject
        {
            get { return this.GetProperty(AssetObjectProperty); }
        }
        #endregion

        #region 外部领用 External
        /// <summary>
        /// 外部领用
        /// </summary>
        [Label("外部领用")]
        public static readonly Property<bool> ExternalProperty = P<AssetIssue>.RegisterView(e => e.External, p => p.AssetRequisition.External);

        /// <summary>
        /// 外部领用
        /// </summary>
        public bool External
        {
            get { return this.GetProperty(ExternalProperty); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<AssetIssue>.RegisterView(e => e.Remark, p => p.AssetRequisition.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
        }
        #endregion

        #region 外部领用类型 ExternalType
        /// <summary>
        /// 外部领用类型
        /// </summary>
        [Label("外部领用类型")]
        public static readonly Property<ExternalType?> ExternalTypeProperty = P<AssetIssue>.RegisterView(e => e.ExternalType, p => p.AssetRequisition.ExternalType);

        /// <summary>
        /// 外部领用类型
        /// </summary>
        public ExternalType? ExternalType
        {
            get { return this.GetProperty(ExternalTypeProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<AssetIssue>.RegisterView(e => e.SupplierCode, p => p.AssetRequisition.Supplier.Code);

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
        public static readonly Property<string> SupplierNameProperty = P<AssetIssue>.RegisterView(e => e.SupplierName, p => p.AssetRequisition.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<AssetIssue>.RegisterView(e => e.CustomerCode, p => p.AssetRequisition.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<AssetIssue>.RegisterView(e => e.CustomerName, p => p.AssetRequisition.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 目的地描述 Destination
        /// <summary>
        /// 目的地描述
        /// </summary>
        [Label("目的地描述")]
        public static readonly Property<string> DestinationProperty = P<AssetIssue>.RegisterView(e => e.Destination, p => p.AssetRequisition.Destination);

        /// <summary>
        /// 目的地描述
        /// </summary>
        public string Destination
        {
            get { return this.GetProperty(DestinationProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 资产发放 实体配置
    /// </summary>
    internal class AssetIssueConfig : EntityConfig<AssetIssue>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_ISSUE").MapAllProperties();
            Meta.EnablePhantoms();
        }

        /// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<AssetIssue>();
                    StringBuilder sb = new StringBuilder();

                    if (para.AssetRequisition.External)
                    {
                        if (para.ContactPerson.IsNullOrEmpty())
                        {
                            sb.AppendLine("外部领用信息的【联系人】不能为空！".L10N());
                        }
                        if (para.ContactInformation.IsNullOrEmpty())
                        {
                            sb.AppendLine("外部领用信息的【联系方式】不能为空！".L10N());
                        }
                        if (para.Address.IsNullOrEmpty())
                        {
                            sb.AppendLine("外部领用信息的【联系地址】不能为空！".L10N());
                        }
                        if (para.DeliveryWay == null)
                        {
                            sb.AppendLine("外部领用信息的【发货方式】不能为空！".L10N());
                        }
                    }

                    e.BrokenDescription = sb.ToString();
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}