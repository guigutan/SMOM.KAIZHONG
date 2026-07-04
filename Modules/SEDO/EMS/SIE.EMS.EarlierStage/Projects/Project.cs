using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.EarlierStage.Enums;
using SIE.Equipments.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProjectCriteria))]
    [Label("项目管理")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [DisplayMember(nameof(Code))]
    [EntityWithConfig(typeof(NoConfig), "项目管理编号配置项", "项目管理编号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class Project : EMS.Projects.Project
    {
        /// <summary>
        /// 快码类型：项目类别
        /// </summary>
        public static string ProjectClassify { get { return "PROJECT_CLASSIFY"; } }

        #region 年度 Year
        /// <summary>
        /// 年度
        /// </summary>
        [Label("年度")]
        [Required]
        public static readonly Property<DateTime> YearProperty = P<Project>.Register(e => e.Year);

        /// <summary>
        /// 年度
        /// </summary>
        public DateTime Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        #region 立项日期 ProjectDate
        /// <summary>
        /// 立项日期
        /// </summary>
        [Label("立项日期")]
        [Required]
        public static readonly Property<DateTime> ProjectDateProperty = P<Project>.Register(e => e.ProjectDate);

        /// <summary>
        /// 立项日期
        /// </summary>
        public DateTime ProjectDate
        {
            get { return GetProperty(ProjectDateProperty); }
            set { SetProperty(ProjectDateProperty, value); }
        }
        #endregion

        #region 项目类别 ProjectType
        /// <summary>
        /// 项目类别
        /// </summary>
        [Label("项目类别")]
        [Required]
        public static readonly Property<string> ProjectTypeProperty = P<Project>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类别
        /// </summary>
        public string ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 项目内容及立项依据 ContentAndBasis
        /// <summary>
        /// 项目内容及立项依据
        /// </summary>
        [MaxLength(1000)]
        [Required]
        [Label("项目内容及立项依据")]
        public static readonly Property<string> ContentAndBasisProperty = P<Project>.Register(e => e.ContentAndBasis);

        /// <summary>
        /// 项目内容及立项依据
        /// </summary>
        public string ContentAndBasis
        {
            get { return GetProperty(ContentAndBasisProperty); }
            set { SetProperty(ContentAndBasisProperty, value); }
        }
        #endregion

        #region 预期目标及综合经济效益 GoalAndBenefit
        /// <summary>
        /// 预期目标及综合经济效益
        /// </summary>
        [MaxLength(1000)]
        [Required]
        [Label("预期目标及综合经济效益")]
        public static readonly Property<string> GoalAndBenefitProperty = P<Project>.Register(e => e.GoalAndBenefit);

        /// <summary>
        /// 预期目标及综合经济效益
        /// </summary>
        public string GoalAndBenefit
        {
            get { return GetProperty(GoalAndBenefitProperty); }
            set { SetProperty(GoalAndBenefitProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<Project>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 父项目 ParentProject
        /// <summary>
        /// 父项目Id
        /// </summary>
        public static readonly IRefIdProperty ParentProjectIdProperty = P<Project>.RegisterRefId(e => e.ParentProjectId, ReferenceType.Normal);

        /// <summary>
        /// 父项目Id
        /// </summary>
        public double? ParentProjectId
        {
            get { return (double?)GetRefNullableId(ParentProjectIdProperty); }
            set { SetRefNullableId(ParentProjectIdProperty, value); }
        }

        /// <summary>
        /// 父项目
        /// </summary>
        public static readonly RefEntityProperty<Project> ParentProjectProperty = P<Project>.RegisterRef(e => e.ParentProject, ParentProjectIdProperty);

        /// <summary>
        /// 父项目
        /// </summary>
        public Project ParentProject
        {
            get { return GetRefEntity(ParentProjectProperty); }
            set { SetRefEntity(ParentProjectProperty, value); }
        }
        #endregion

        #region 计划类型 PlanType
        /// <summary>
        /// 计划类型
        /// </summary>
        [Label("计划类型")]
        public static readonly Property<PlanType> PlanTypeProperty = P<Project>.Register(e => e.PlanType);

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType
        {
            get { return GetProperty(PlanTypeProperty); }
            set { SetProperty(PlanTypeProperty, value); }
        }
        #endregion

        #region 工时成本 LaborCost
        /// <summary>
        /// 工时成本
        /// </summary>
        [Label("工时成本")]
        public static readonly Property<decimal?> LaborCostProperty = P<Project>.Register(e => e.LaborCost);

        /// <summary>
        /// 工时成本
        /// </summary>
        public decimal? LaborCost
        {
            get { return GetProperty(LaborCostProperty); }
            set { SetProperty(LaborCostProperty, value); }
        }
        #endregion

        #region 初验日期 InitialAcceptanceDate
        /// <summary>
        /// 初验日期
        /// </summary>
        [Label("初验日期")]
        public static readonly Property<DateTime?> InitialAcceptanceDateProperty = P<Project>.Register(e => e.InitialAcceptanceDate);

        /// <summary>
        /// 初验日期
        /// </summary>
        public DateTime? InitialAcceptanceDate
        {
            get { return this.GetProperty(InitialAcceptanceDateProperty); }
            set { this.SetProperty(InitialAcceptanceDateProperty, value); }
        }
        #endregion

        #region 验收日期 AcceptanceDate
        /// <summary>
        /// 验收日期
        /// </summary>
        [Label("验收日期")]
        public static readonly Property<DateTime?> AcceptanceDateProperty = P<Project>.Register(e => e.AcceptanceDate);

        /// <summary>
        /// 验收日期
        /// </summary>
        public DateTime? AcceptanceDate
        {
            get { return this.GetProperty(AcceptanceDateProperty); }
            set { this.SetProperty(AcceptanceDateProperty, value); }
        }
        #endregion

        #region 质保验收日期 WarrantyAcceptanceDate
        /// <summary>
        /// 质保验收日期
        /// </summary>
        [Label("质保验收日期")]
        public static readonly Property<DateTime?> WarrantyAcceptanceDateProperty = P<Project>.Register(e => e.WarrantyAcceptanceDate);

        /// <summary>
        /// 质保验收日期
        /// </summary>
        public DateTime? WarrantyAcceptanceDate
        {
            get { return this.GetProperty(WarrantyAcceptanceDateProperty); }
            set { this.SetProperty(WarrantyAcceptanceDateProperty, value); }
        }
        #endregion

        #region 项目事项 KeyItemList
        /// <summary>
        /// 项目事项
        /// </summary>
        [Label("项目事项")]
        public static new readonly ListProperty<EntityList<ProjectKeyItem>> KeyItemListProperty = P<Project>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 项目事项
        /// </summary>
        public new EntityList<ProjectKeyItem> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion

        #region 工作计划列表 ProjectWorkItemList
        /// <summary>
        /// 工作计划列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProjectWorkItem>> ProjectWorkItemListProperty = P<Project>.RegisterList(e => e.ProjectWorkItemList);
        /// <summary>
        /// 工作计划列表
        /// </summary>
        public EntityList<ProjectWorkItem> ProjectWorkItemList
        {
            get { return this.GetLazyList(ProjectWorkItemListProperty); }
        }
        #endregion

        #region 项目成员列表 ProjectMemberList
        /// <summary>
        /// 项目成员列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProjectMember>> ProjectMemberListProperty = P<Project>.RegisterList(e => e.ProjectMemberList);
        /// <summary>
        /// 项目成员列表
        /// </summary>
        public EntityList<ProjectMember> ProjectMemberList
        {
            get { return this.GetLazyList(ProjectMemberListProperty); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        [Label("附件列表")]
        public static readonly ListProperty<EntityList<ProjectAttachment>> AttachmentListProperty = P<Project>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<ProjectAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 父项目编码 ParentProjectCode
        /// <summary>
        /// 父项目编码
        /// </summary>
        [Label("父项目编码")]
        public static readonly Property<string> ParentProjectCodeProperty = P<Project>.RegisterView(e => e.ParentProjectCode, p => p.ParentProject.Code);

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
        public static readonly Property<string> ParentProjectNameProperty = P<Project>.RegisterView(e => e.ParentProjectName, p => p.ParentProject.Name);

        /// <summary>
        /// 父项目名称
        /// </summary>
        public string ParentProjectName
        {
            get { return this.GetProperty(ParentProjectNameProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<Project>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

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
        public static readonly Property<string> DepartmentNameProperty = P<Project>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion

        #region 责任人名称 PrincipalName
        /// <summary>
        /// 责任人名称
        /// </summary>
        [Label("责任人名称")]
        public static readonly Property<string> PrincipalNameProperty = P<Project>.RegisterView(e => e.PrincipalName, p => p.Principal.Name);

        /// <summary>
        /// 责任人名称
        /// </summary>
        public string PrincipalName
        {
            get { return this.GetProperty(PrincipalNameProperty); }
        }
        #endregion

        #region 项目年度(不映射数据库，供导入时使用） ProjectYear
        /// <summary>
        /// 项目年度(不映射数据库，供导入时使用）
        /// </summary>
        [Label("年度")]
        public static readonly Property<int> ProjectYearProperty = P<Project>.Register(e => e.ProjectYear);

        /// <summary>
        /// 项目年度(不映射数据库，供导入时使用）
        /// </summary>
        public int ProjectYear
        {
            get { return this.GetProperty(ProjectYearProperty); }
            set { this.SetProperty(ProjectYearProperty, value); }
        }
        #endregion

        #region 导入用工厂名称 FactoryNameImport
        /// <summary>
        /// 导入用工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameImportProperty = P<Project>.Register(e => e.FactoryNameImport);

        /// <summary>
        /// 导入用工厂名称
        /// </summary>
        public string FactoryNameImport
        {
            get { return this.GetProperty(FactoryNameImportProperty); }
            set { this.SetProperty(FactoryNameImportProperty, value); }
        }
        #endregion


        #region 导入用部门 DepartmentNameImport
        /// <summary>
        /// 导入用部门
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DepartmentNameImportProperty = P<Project>.Register(e => e.DepartmentNameImport);

        /// <summary>
        /// 导入用部门
        /// </summary>
        public string DepartmentNameImport
        {
            get { return this.GetProperty(DepartmentNameImportProperty); }
            set { this.SetProperty(DepartmentNameImportProperty, value); }
        }
        #endregion


        #region 导入用项目负责人 PrincipalCodeImport
        /// <summary>
        /// 导入用项目负责人
        /// </summary>
        [Label("项目负责人")]
        public static readonly Property<string> PrincipalCodeImportProperty = P<Project>.Register(e => e.PrincipalCodeImport);

        /// <summary>
        /// 导入用项目负责人
        /// </summary>
        public string PrincipalCodeImport
        {
            get { return this.GetProperty(PrincipalCodeImportProperty); }
            set { this.SetProperty(PrincipalCodeImportProperty, value); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 项目 实体配置
    /// </summary>
    internal class ProjectConfig : EntityConfig<Project>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PROJ").MapAllPropertiesExcept(Project.DepartmentNameImportProperty,
                Project.FactoryNameImportProperty,Project.PrincipalCodeImportProperty);
            Meta.Property(Project.ProjectYearProperty).DontMapColumn();

            Meta.Property(Project.DepartmentNameImportProperty).DontMapColumn();
            Meta.Property(Project.FactoryNameImportProperty).DontMapColumn();
            Meta.Property(Project.PrincipalCodeImportProperty).DontMapColumn();
            Meta.Property(Project.ContentAndBasisProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Project.GoalAndBenefitProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Project.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}