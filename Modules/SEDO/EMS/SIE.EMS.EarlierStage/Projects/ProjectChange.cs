using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProjectChangeCriteria))]
    [Label("项目变更")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [DisplayMember(nameof(No))]
    [EntityWithConfig(typeof(NoConfig), "项目变更编号配置项", "项目变更编号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class ProjectChange : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<ProjectChange>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<ProjectChange>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<ProjectChange>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<ProjectChange>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 变更单号 No
        /// <summary>
        /// 变更单号
        /// </summary>
        [Label("变更单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NoProperty = P<ProjectChange>.Register(e => e.No);

        /// <summary>
        /// 变更单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<ProjectChange>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 审核时间 ApprovalTime
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime?> ApprovalTimeProperty = P<ProjectChange>.Register(e => e.ApprovalTime);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovalTime
        {
            get { return this.GetProperty(ApprovalTimeProperty); }
            set { this.SetProperty(ApprovalTimeProperty, value); }
        }
        #endregion

        #region 项目 Project
        /// <summary>
        /// 项目Id
        /// </summary>
        [Label("项目")]
        public static readonly IRefIdProperty ProjectIdProperty =
            P<ProjectChange>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

        /// <summary>
        /// 项目Id
        /// </summary>
        public double ProjectId
        {
            get { return (double)this.GetRefId(ProjectIdProperty); }
            set { this.SetRefId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static readonly RefEntityProperty<Project> ProjectProperty =
            P<ProjectChange>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return this.GetRefEntity(ProjectProperty); }
            set { this.SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 预算金额 Amount
        /// <summary>
        /// 预算金额
        /// </summary>
        [Label("预算金额")]
        [MinValue(0)]
        public static readonly Property<decimal> AmountProperty = P<ProjectChange>.Register(e => e.Amount);

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 变更内容 ChangeContentList
        /// <summary>
        /// 变更内容
        /// </summary>
        [Label("变更内容")]
        public static readonly ListProperty<EntityList<ProjectChangeContent>> ChangeContentListProperty = P<ProjectChange>.RegisterList(e => e.ChangeContentList);

        /// <summary>
        /// 变更内容
        /// </summary>
        public EntityList<ProjectChangeContent> ChangeContentList
        {
            get { return this.GetLazyList(ChangeContentListProperty); }
        }
        #endregion

        #region 项目暂停 IsSuspend
        /// <summary>
        /// 项目暂停
        /// </summary>
        [Label("项目暂停")]
        public static readonly Property<bool?> IsSuspendProperty = P<ProjectChange>.Register(e => e.IsSuspend);

        /// <summary>
        /// 项目暂停
        /// </summary>
        public bool? IsSuspend
        {
            get { return this.GetProperty(IsSuspendProperty); }
            set { this.SetProperty(IsSuspendProperty, value); }
        }
        #endregion

        #region 暂停原因 SuspendReason
        /// <summary>
        /// 暂停原因
        /// </summary>
        [Label("暂停原因")]
        public static readonly Property<string> SuspendReasonProperty = P<ProjectChange>.Register(e => e.SuspendReason);

        /// <summary>
        /// 暂停原因
        /// </summary>
        public string SuspendReason
        {
            get { return this.GetProperty(SuspendReasonProperty); }
            set { this.SetProperty(SuspendReasonProperty, value); }
        }
        #endregion

        #region 项目恢复 IsRecovery
        /// <summary>
        /// 项目恢复
        /// </summary>
        [Label("项目恢复")]
        public static readonly Property<bool?> IsRecoveryProperty = P<ProjectChange>.Register(e => e.IsRecovery);

        /// <summary>
        /// 项目恢复
        /// </summary>
        public bool? IsRecovery
        {
            get { return this.GetProperty(IsRecoveryProperty); }
            set { this.SetProperty(IsRecoveryProperty, value); }
        }
        #endregion

        #region 恢复说明 RecoveryExplain
        /// <summary>
        /// 恢复说明
        /// </summary>
        [Label("恢复说明")]
        public static readonly Property<string> RecoveryExplainProperty = P<ProjectChange>.Register(e => e.RecoveryExplain);

        /// <summary>
        /// 恢复说明
        /// </summary>
        public string RecoveryExplain
        {
            get { return this.GetProperty(RecoveryExplainProperty); }
            set { this.SetProperty(RecoveryExplainProperty, value); }
        }
        #endregion

        #region 项目成员列表 ProjectMemberList
        /// <summary>
        /// 项目成员列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProjectChangeMember>> ProjectMemberListProperty = P<ProjectChange>.RegisterList(e => e.ProjectMemberList);
        /// <summary>
        /// 项目成员列表
        /// </summary>
        public EntityList<ProjectChangeMember> ProjectMemberList
        {
            get { return this.GetLazyList(ProjectMemberListProperty); }
        }
        #endregion

        #region 工作计划列表 ProjectWorkItemList
        /// <summary>
        /// 工作计划列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProjectChangeWorkItem>> ProjectWorkItemListProperty = P<ProjectChange>.RegisterList(e => e.ProjectWorkItemList);
        /// <summary>
        /// 工作计划列表
        /// </summary>
        public EntityList<ProjectChangeWorkItem> ProjectWorkItemList
        {
            get { return this.GetLazyList(ProjectWorkItemListProperty); }
        }
        #endregion

        #region 关键事项 KeyItemList
        /// <summary>
        /// 关键事项
        /// </summary>
        [Label("关键事项")]
        public static readonly ListProperty<EntityList<ProjectChangeKeyItem>> KeyItemListProperty = P<ProjectChange>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 关键事项
        /// </summary>
        public EntityList<ProjectChangeKeyItem> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<ProjectChange>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 部门 DepartmentName
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentNameProperty = P<ProjectChange>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<ProjectChange>.RegisterView(e => e.ProjectName, p => p.Project.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 计划类型 PlanType
        /// <summary>
        /// 计划类型
        /// </summary>
        [Label("计划类型")]
        public static readonly Property<PlanType> PlanTypeProperty = P<ProjectChange>.RegisterView(e => e.PlanType, p => p.Project.PlanType);

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType
        {
            get { return this.GetProperty(PlanTypeProperty); }
        }
        #endregion

        #region 年度 Year
        /// <summary>
        /// 年度
        /// </summary>
        [Label("年度")]
        public static readonly Property<DateTime?> YearProperty = P<ProjectChange>.RegisterView(e => e.Year, p => p.Project.Year);

        /// <summary>
        /// 年度
        /// </summary>
        public DateTime? Year
        {
            get { return this.GetProperty(YearProperty); }
        }
        #endregion

        #region 项目类别 ProjectType
        /// <summary>
        /// 项目类别
        /// </summary>
        [Label("项目类别")]
        public static readonly Property<string> ProjectTypeProperty = P<ProjectChange>.RegisterView(e => e.ProjectType, p => p.Project.ProjectType);

        /// <summary>
        /// 项目类别
        /// </summary>
        public string ProjectType
        {
            get { return this.GetProperty(ProjectTypeProperty); }
        }
        #endregion

        #region 项目负责人 PrincipalName
        /// <summary>
        /// 项目负责人
        /// </summary>
        [Label("项目负责人")]
        public static readonly Property<string> PrincipalNameProperty = P<ProjectChange>.RegisterView(e => e.PrincipalName, p => p.Project.Principal.Name);

        /// <summary>
        /// 项目负责人
        /// </summary>
        public string PrincipalName
        {
            get { return this.GetProperty(PrincipalNameProperty); }
        }
        #endregion

        #region 中标金额 ActualAmount
        /// <summary>
        /// 中标金额
        /// </summary>
        [Label("中标金额")]
        public static readonly Property<decimal?> ActualAmountProperty = P<ProjectChange>.RegisterView(e => e.ActualAmount, p => p.Project.ActualAmount);

        /// <summary>
        /// 中标金额
        /// </summary>
        public decimal? ActualAmount
        {
            get { return this.GetProperty(ActualAmountProperty); }
        }
        #endregion

        #region 立项日期 ProjectDate
        /// <summary>
        /// 立项日期
        /// </summary>
        [Label("立项日期")]
        public static readonly Property<DateTime?> ProjectDateProperty = P<ProjectChange>.RegisterView(e => e.ProjectDate, p => p.Project.ProjectDate);

        /// <summary>
        /// 立项日期
        /// </summary>
        public DateTime? ProjectDate
        {
            get { return this.GetProperty(ProjectDateProperty); }
        }
        #endregion

        #region 父项目编码 ParentProjectCode
        /// <summary>
        /// 父项目编码
        /// </summary>
        [Label("父项目编码")]
        public static readonly Property<string> ParentProjectCodeProperty = P<ProjectChange>.RegisterView(e => e.ParentProjectCode, p => p.Project.ParentProject.Code);

        /// <summary>
        /// 父项目编码
        /// </summary>
        public string ParentProjectCode
        {
            get { return this.GetProperty(ParentProjectCodeProperty); }
        }
        #endregion

        #region 父项目名称 ParentProjectName
        /// <summary>
        /// 父项目名称
        /// </summary>
        [Label("父项目名称")]
        public static readonly Property<string> ParentProjectNameProperty = P<ProjectChange>.RegisterView(e => e.ParentProjectName, p => p.Project.ParentProject.Name);

        /// <summary>
        /// 父项目名称
        /// </summary>
        public string ParentProjectName
        {
            get { return this.GetProperty(ParentProjectNameProperty); }
        }
        #endregion

        #region 项目状态 ProjectStatus
        /// <summary>
        /// 项目状态
        /// </summary>
        [Label("项目状态")]
        public static readonly Property<ProjectStatus> ProjectStatusProperty = P<ProjectChange>.RegisterView(e => e.ProjectStatus, p => p.Project.ProjectStatus);

        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus ProjectStatus
        {
            get { return this.GetProperty(ProjectStatusProperty); }
        }
        #endregion

        #region 项目内容及立项依据 ContentAndBasis
        /// <summary>
        /// 项目内容及立项依据
        /// </summary>
        [Label("项目内容及立项依据")]
        public static readonly Property<string> ContentAndBasisProperty = P<ProjectChange>.RegisterView(e => e.ContentAndBasis, p => p.Project.ContentAndBasis);

        /// <summary>
        /// 项目内容及立项依据
        /// </summary>
        public string ContentAndBasis
        {
            get { return this.GetProperty(ContentAndBasisProperty); }
        }
        #endregion

        #region 预期目标及综合经济效益 GoalAndBenefit
        /// <summary>
        /// 预期目标及综合经济效益
        /// </summary>
        [Label("预期目标及综合经济效益")]
        public static readonly Property<string> GoalAndBenefitProperty = P<ProjectChange>.RegisterView(e => e.GoalAndBenefit, p => p.Project.GoalAndBenefit);

        /// <summary>
        /// 预期目标及综合经济效益
        /// </summary>
        public string GoalAndBenefit
        {
            get { return this.GetProperty(GoalAndBenefitProperty); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProjectChange>.RegisterView(e => e.Remark, p => p.Project.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 项目变更 实体配置
    /// </summary>
    internal class ProjectChangeConfig : EntityConfig<ProjectChange>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PROJ_CHA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
