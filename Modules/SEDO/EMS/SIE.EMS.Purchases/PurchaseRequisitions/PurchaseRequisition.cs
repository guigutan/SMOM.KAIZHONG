using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Projects;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PurchaseRequisitionCriteria))]
    [Label("采购申请")]
    [DisplayMember(nameof(No))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(NoConfig), "采购申请单号配置项", "采购申请单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class PurchaseRequisition : DataEntity
    {
        #region 申请单号 No
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NoProperty = P<PurchaseRequisition>.Register(e => e.No);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 品种数 VarietyQuantity
        /// <summary>
        /// 品种数
        /// </summary>
        [Label("品种数")]
        public static readonly Property<decimal> VarietyQuantityProperty = P<PurchaseRequisition>.Register(e => e.VarietyQuantity);

        /// <summary>
        /// 品种数
        /// </summary>
        public decimal VarietyQuantity
        {
            get { return GetProperty(VarietyQuantityProperty); }
            set { SetProperty(VarietyQuantityProperty, value); }
        }
        #endregion

        #region 总数量 TotalAmount
        /// <summary>
        /// 总数量
        /// </summary>
        [Label("总数量")]
        public static readonly Property<decimal> TotalAmountProperty = P<PurchaseRequisition>.Register(e => e.TotalAmount);

        /// <summary>
        /// 总数量
        /// </summary>
        public decimal TotalAmount
        {
            get { return GetProperty(TotalAmountProperty); }
            set { SetProperty(TotalAmountProperty, value); }
        }
        #endregion

        #region 采购预算 PurchaseBudget
        /// <summary>
        /// 采购预算
        /// </summary>
        [Label("采购预算")]
        [MinValue(0.01)]
        public static readonly Property<decimal> PurchaseBudgetProperty = P<PurchaseRequisition>.Register(e => e.PurchaseBudget);

        /// <summary>
        /// 采购预算
        /// </summary>
        public decimal PurchaseBudget
        {
            get { return GetProperty(PurchaseBudgetProperty); }
            set { SetProperty(PurchaseBudgetProperty, value); }
        }
        #endregion

        #region 中标金额 BidAmount
        /// <summary>
        /// 中标金额
        /// </summary>
        [Label("中标金额")]
        public static readonly Property<decimal> BidAmountProperty = P<PurchaseRequisition>.Register(e => e.BidAmount);

        /// <summary>
        /// 中标金额
        /// </summary>
        public decimal BidAmount
        {
            get { return GetProperty(BidAmountProperty); }
            set { SetProperty(BidAmountProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<PurchaseRequisition>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 币种 Currency
        /// <summary>
        /// 币种
        /// </summary>
        [Label("币种")]
        public static readonly Property<Currency> CurrencyProperty = P<PurchaseRequisition>.Register(e => e.Currency);

        /// <summary>
        /// 币种
        /// </summary>
        public Currency Currency
        {
            get { return GetProperty(CurrencyProperty); }
            set { SetProperty(CurrencyProperty, value); }
        }
        #endregion

        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType> PurchaseObjectTypeProperty = P<PurchaseRequisition>.Register(e => e.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType PurchaseObjectType
        {
            get { return GetProperty(PurchaseObjectTypeProperty); }
            set { SetProperty(PurchaseObjectTypeProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<PurchaseRequisition>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<PurchaseRequisition>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<PurchaseRequisition>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 审核日期 ApprovalTime
        /// <summary>
        /// 审核日期
        /// </summary>
        [Label("审核日期")]
        public static readonly Property<DateTime?> ApprovalTimeProperty = P<PurchaseRequisition>.Register(e => e.ApprovalTime);

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? ApprovalTime
        {
            get { return this.GetProperty(ApprovalTimeProperty); }
            set { this.SetProperty(ApprovalTimeProperty, value); }
        }
        #endregion

        #region 采购类型 PurchaseType
        /// <summary>
        /// 采购类型
        /// </summary>
        [Label("采购类型")]
        public static readonly Property<PurchaseType> PurchaseTypeProperty = P<PurchaseRequisition>.Register(e => e.PurchaseType);

        /// <summary>
        /// 采购类型
        /// </summary>
        public PurchaseType PurchaseType
        {
            get { return GetProperty(PurchaseTypeProperty); }
            set { SetProperty(PurchaseTypeProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<PurchaseRequisition>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double DepartmentId
        {
            get { return (double)GetRefId(DepartmentIdProperty); }
            set { SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<PurchaseRequisition>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 项目 Project
        /// <summary>
        /// 项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectIdProperty = P<PurchaseRequisition>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

        /// <summary>
        /// 项目Id
        /// </summary>
        public double? ProjectId
        {
            get { return (double?)GetRefNullableId(ProjectIdProperty); }
            set { SetRefNullableId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static readonly RefEntityProperty<Project> ProjectProperty = P<PurchaseRequisition>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return GetRefEntity(ProjectProperty); }
            set { SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 流程实例ID WorkflowInstanceId
        /// <summary>
        /// 流程实例ID(兼容可能要与外部工作流集成，设计成字符型字段）
        /// </summary>
        [Label("流程实例ID")]
        [MaxLength(80)]
        public static readonly Property<string> WorkflowInstanceIdProperty = P<PurchaseRequisition>.Register(e => e.WorkflowInstanceId);

        /// <summary>
        /// 流程实例ID
        /// </summary>
        public string WorkflowInstanceId
        {
            get { return this.GetProperty(WorkflowInstanceIdProperty); }
            set { this.SetProperty(WorkflowInstanceIdProperty, value); }
        }
        #endregion

        #region 流程启动结果 WorkflowStartResult
        /// <summary>
        /// 流程启动结果
        /// </summary>
        [Label("流程启动结果")]
        [MaxLength(1000)]
        public static readonly Property<string> WorkflowStartResultProperty = P<PurchaseRequisition>.Register(e => e.WorkflowStartResult);

        /// <summary>
        /// 流程启动结果
        /// </summary>
        public string WorkflowStartResult
        {
            get { return this.GetProperty(WorkflowStartResultProperty); }
            set { this.SetProperty(WorkflowStartResultProperty, value); }
        }
        #endregion


        #region 采购申请明细列表 DetailList
        /// <summary>
        /// 采购申请明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<PurchaseRequisitionItem>> DetailListProperty = P<PurchaseRequisition>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 采购申请明细列表
        /// </summary>
        public EntityList<PurchaseRequisitionItem> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<PurchaseRequisitionAttachment>> AttachmentListProperty = P<PurchaseRequisition>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<PurchaseRequisitionAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<PurchaseRequisition>.RegisterView(e => e.ProjectCode, p => p.Project.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<PurchaseRequisition>.RegisterView(e => e.ProjectName, p => p.Project.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 金额单位 AmountUnit
        /// <summary>
        /// 金额单位
        /// </summary>
        [Label("金额单位")]
        public static readonly Property<AmountUnit> AmountUnitProperty = P<PurchaseRequisition>.RegisterReadOnly(
            e => e.AmountUnit, e => e.GetAmountUnit(), CurrencyProperty);
        /// <summary>
        /// 金额单位
        /// </summary>
        public AmountUnit AmountUnit
        {
            get { return this.GetProperty(AmountUnitProperty); }
        }
        private AmountUnit GetAmountUnit()
        {
            return (AmountUnit)Currency;
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty
            = P<PurchaseRequisition>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 部门名称 DepartmentName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DepartmentNameProperty
            = P<PurchaseRequisition>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 采购申请 实体配置
    /// </summary>
    internal class PurchaseRequisitionConfig : EntityConfig<PurchaseRequisition>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PR").MapAllProperties();
            Meta.Property(PurchaseRequisition.RemarkProperty).ColumnMeta.HasLength(4000);            
            Meta.Property(PurchaseRequisition.WorkflowInstanceIdProperty).ColumnMeta.HasLength(240);
            Meta.Property(PurchaseRequisition.WorkflowStartResultProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}