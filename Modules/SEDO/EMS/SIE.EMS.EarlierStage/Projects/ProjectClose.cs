using SIE.Common.Configs;
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
    /// 项目结项
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProjectCloseCriteria))]
    [Label("项目结项")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class ProjectClose : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<ProjectClose>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<ProjectClose>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<ProjectClose>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<ProjectClose>.RegisterRef(e => e.Department, DepartmentIdProperty);

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
        [Label("项目")]
        [NotDuplicate]
        public static readonly IRefIdProperty ProjectIdProperty =
            P<ProjectClose>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

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
            P<ProjectClose>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return this.GetRefEntity(ProjectProperty); }
            set { this.SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<ProjectClose>.Register(e => e.ApprovalStatus);

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
        public static readonly Property<DateTime?> ApprovalTimeProperty = P<ProjectClose>.Register(e => e.ApprovalTime);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovalTime
        {
            get { return this.GetProperty(ApprovalTimeProperty); }
            set { this.SetProperty(ApprovalTimeProperty, value); }
        }
        #endregion

        #region 结项类型 CloseItemType
        /// <summary>
        /// 结项类型
        /// </summary>
        [Label("结项类型")]
        public static readonly Property<CloseItemType> CloseItemTypeProperty = P<ProjectClose>.Register(e => e.CloseItemType);

        /// <summary>
        /// 结项类型
        /// </summary>
        public CloseItemType CloseItemType
        {
            get { return this.GetProperty(CloseItemTypeProperty); }
            set { this.SetProperty(CloseItemTypeProperty, value); }
        }
        #endregion

        #region 项目总结 Summary
        /// <summary>
        /// 项目总结
        /// </summary>
        [Label("项目总结")]
        [MaxLength(1000)]
        [Required]
        public static readonly Property<string> SummaryProperty = P<ProjectClose>.Register(e => e.Summary);

        /// <summary>
        /// 项目总结
        /// </summary>
        public string Summary
        {
            get { return this.GetProperty(SummaryProperty); }
            set { this.SetProperty(SummaryProperty, value); }
        }
        #endregion

        #region 项目经验总结及建议 Experience
        /// <summary>
        /// 项目经验总结及建议
        /// </summary>
        [Label("项目经验总结及建议")]
        [MaxLength(1000)]
        public static readonly Property<string> ExperienceProperty = P<ProjectClose>.Register(e => e.Experience);

        /// <summary>
        /// 项目经验总结及建议
        /// </summary>
        public string Experience
        {
            get { return this.GetProperty(ExperienceProperty); }
            set { this.SetProperty(ExperienceProperty, value); }
        }
        #endregion

        #region 与立项经济效益分析对比 BenefitAnalysis
        /// <summary>
        /// 与立项经济效益分析对比
        /// </summary>
        [Label("与立项经济效益分析对比")]
        [MaxLength(1000)]
        public static readonly Property<string> BenefitAnalysisProperty = P<ProjectClose>.Register(e => e.BenefitAnalysis);

        /// <summary>
        /// 与立项经济效益分析对比
        /// </summary>
        public string BenefitAnalysis
        {
            get { return this.GetProperty(BenefitAnalysisProperty); }
            set { this.SetProperty(BenefitAnalysisProperty, value); }
        }
        #endregion

        #region 投资前后效率对比 InvestmentEfficiency
        /// <summary>
        /// 投资前后效率对比
        /// </summary>
        [Label("投资前后效率对比")]
        [MaxLength(1000)]
        public static readonly Property<string> InvestmentEfficiencyProperty = P<ProjectClose>.Register(e => e.InvestmentEfficiency);

        /// <summary>
        /// 投资前后效率对比
        /// </summary>
        public string InvestmentEfficiency
        {
            get { return this.GetProperty(InvestmentEfficiencyProperty); }
            set { this.SetProperty(InvestmentEfficiencyProperty, value); }
        }
        #endregion

        #region 投资回收期预测 PaybackForecast
        /// <summary>
        /// 投资回收期预测
        /// </summary>
        [Label("投资回收期预测")]
        [MaxLength(1000)]
        public static readonly Property<string> PaybackForecastProperty = P<ProjectClose>.Register(e => e.PaybackForecast);

        /// <summary>
        /// 投资回收期预测
        /// </summary>
        public string PaybackForecast
        {
            get { return this.GetProperty(PaybackForecastProperty); }
            set { this.SetProperty(PaybackForecastProperty, value); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        [Label("附件列表")]
        public static readonly ListProperty<EntityList<ProjectCloseAttachment>> AttachmentListProperty = P<ProjectClose>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<ProjectCloseAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<ProjectClose>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

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
        public static readonly Property<string> DepartmentNameProperty = P<ProjectClose>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

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
        public static readonly Property<string> ProjectNameProperty = P<ProjectClose>.RegisterView(e => e.ProjectName, p => p.Project.Name);

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
        public static readonly Property<PlanType> PlanTypeProperty = P<ProjectClose>.RegisterView(e => e.PlanType, p => p.Project.PlanType);

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
        public static readonly Property<DateTime?> YearProperty = P<ProjectClose>.RegisterView(e => e.Year, p => p.Project.Year);

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
        public static readonly Property<string> ProjectTypeProperty = P<ProjectClose>.RegisterView(e => e.ProjectType, p => p.Project.ProjectType);

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
        public static readonly Property<string> PrincipalNameProperty = P<ProjectClose>.RegisterView(e => e.PrincipalName, p => p.Project.Principal.Name);

        /// <summary>
        /// 项目负责人
        /// </summary>
        public string PrincipalName
        {
            get { return this.GetProperty(PrincipalNameProperty); }
        }
        #endregion

        #region 项目金额 Amount
        /// <summary>
        /// 项目金额
        /// </summary>
        [Label("项目金额")]
        public static readonly Property<decimal> AmountProperty = P<ProjectClose>.RegisterView(e => e.Amount, p => p.Project.Amount);

        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal Amount
        {
            get { return this.GetProperty(AmountProperty); }
        }
        #endregion

        #region 中标金额 ActualAmount
        /// <summary>
        /// 中标金额
        /// </summary>
        [Label("中标金额")]
        public static readonly Property<decimal?> ActualAmountProperty = P<ProjectClose>.RegisterView(e => e.ActualAmount, p => p.Project.ActualAmount);

        /// <summary>
        /// 中标金额
        /// </summary>
        public decimal? ActualAmount
        {
            get { return this.GetProperty(ActualAmountProperty); }
        }
        #endregion

        #region 工时成本 LaborCost
        /// <summary>
        /// 工时成本
        /// </summary>
        [Label("工时成本")]
        public static readonly Property<decimal?> LaborCostProperty = P<ProjectClose>.RegisterView(e => e.LaborCost, p => p.Project.LaborCost);

        /// <summary>
        /// 工时成本
        /// </summary>
        public decimal? LaborCost
        {
            get { return this.GetProperty(LaborCostProperty); }
        }
        #endregion

        #region 立项日期 ProjectDate
        /// <summary>
        /// 立项日期
        /// </summary>
        [Label("立项日期")]
        public static readonly Property<DateTime?> ProjectDateProperty = P<ProjectClose>.RegisterView(e => e.ProjectDate, p => p.Project.ProjectDate);

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
        public static readonly Property<string> ParentProjectCodeProperty = P<ProjectClose>.RegisterView(e => e.ParentProjectCode, p => p.Project.ParentProject.Code);

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
        public static readonly Property<string> ParentProjectNameProperty = P<ProjectClose>.RegisterView(e => e.ParentProjectName, p => p.Project.ParentProject.Name);

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
        public static readonly Property<ProjectStatus> ProjectStatusProperty = P<ProjectClose>.RegisterView(e => e.ProjectStatus, p => p.Project.ProjectStatus);

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
        public static readonly Property<string> ContentAndBasisProperty = P<ProjectClose>.RegisterView(e => e.ContentAndBasis, p => p.Project.ContentAndBasis);

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
        public static readonly Property<string> GoalAndBenefitProperty = P<ProjectClose>.RegisterView(e => e.GoalAndBenefit, p => p.Project.GoalAndBenefit);

        /// <summary>
        /// 预期目标及综合经济效益
        /// </summary>
        public string GoalAndBenefit
        {
            get { return this.GetProperty(GoalAndBenefitProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 项目结项 实体配置
    /// </summary>
    internal class ProjectCloseConfig : EntityConfig<ProjectClose>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PRO_CLOSE").MapAllProperties();
            Meta.Property(ProjectClose.SummaryProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ProjectClose.ExperienceProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ProjectClose.BenefitAnalysisProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ProjectClose.InvestmentEfficiencyProperty).ColumnMeta.HasLength(2000);
            Meta.Property(ProjectClose.PaybackForecastProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
