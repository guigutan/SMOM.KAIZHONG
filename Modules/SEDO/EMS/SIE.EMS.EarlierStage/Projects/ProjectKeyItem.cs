using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目关键事项
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目关键事项")]
    [ConditionQueryType(typeof(ProjectKeyItemCriteria))]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class ProjectKeyItem : EMS.Projects.ProjectKeyItem
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<ProjectKeyItem>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<ProjectKeyItem>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<ProjectKeyItem>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<ProjectKeyItem>.RegisterRef(e => e.Department, DepartmentIdProperty);

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
        public static new readonly IRefIdProperty ProjectIdProperty = P<ProjectKeyItem>.RegisterRefId(e => e.ProjectId, ReferenceType.Parent);

        /// <summary>
        /// 项目Id
        /// </summary>
        public new double ProjectId
        {
            get { return (double)GetRefId(ProjectIdProperty); }
            set { SetRefId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static new readonly RefEntityProperty<Project> ProjectProperty = P<ProjectKeyItem>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public new Project Project
        {
            get { return GetRefEntity(ProjectProperty); }
            set { SetRefEntity(ProjectProperty, value); }
        }
        #endregion


        #region 事项状态 WorkStatus
        /// <summary>
        /// 事项状态
        /// </summary>
        [Label("事项状态")]
        public static readonly Property<WorkStatus> WorkStatusProperty = P<ProjectKeyItem>.Register(e => e.WorkStatus);

        /// <summary>
        /// 事项状态
        /// </summary>
        public WorkStatus WorkStatus
        {
            get { return GetProperty(WorkStatusProperty); }
            set { SetProperty(WorkStatusProperty, value); }
        }
        #endregion

        #region 项目计划 ProjectWorkItem
        /// <summary>
        /// 项目计划Id
        /// </summary>
        [Label("项目计划")]
        public static readonly IRefIdProperty ProjectWorkItemIdProperty = P<ProjectKeyItem>.RegisterRefId(e => e.ProjectWorkItemId, ReferenceType.Normal);

        /// <summary>
        /// 项目计划Id
        /// </summary>
        public double? ProjectWorkItemId
        {
            get { return (double?)GetRefNullableId(ProjectWorkItemIdProperty); }
            set { SetRefNullableId(ProjectWorkItemIdProperty, value); }
        }

        /// <summary>
        /// 项目计划
        /// </summary>
        public static readonly RefEntityProperty<ProjectWorkItem> ProjectWorkItemProperty = P<ProjectKeyItem>.RegisterRef(e => e.ProjectWorkItem, ProjectWorkItemIdProperty);

        /// <summary>
        /// 项目计划
        /// </summary>
        public ProjectWorkItem ProjectWorkItem
        {
            get { return GetRefEntity(ProjectWorkItemProperty); }
            set { SetRefEntity(ProjectWorkItemProperty, value); }
        }
        #endregion
        
        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProjectKeyItem>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 工时成本 LaborCost
        /// <summary>
        /// 工时成本
        /// </summary>
        [Label("工时成本")]
        [MinValue(0)]
        public static readonly Property<decimal?> LaborCostProperty = P<ProjectKeyItem>.Register(e => e.LaborCost);

        /// <summary>
        /// 工时成本
        /// </summary>
        public decimal? LaborCost
        {
            get { return GetProperty(LaborCostProperty); }
            set { SetProperty(LaborCostProperty, value); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        [Label("附件列表")]
        public static readonly ListProperty<EntityList<ProjectKeyItemAttachment>> AttachmentListProperty = P<ProjectKeyItem>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<ProjectKeyItemAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 事项成员列表 MemberList
        /// <summary>
        /// 事项成员列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProjectKeyItemMember>> MemberListProperty = P<ProjectKeyItem>.RegisterList(e => e.MemberList);
        /// <summary>
        /// 事项成员列表
        /// </summary>
        public EntityList<ProjectKeyItemMember> MemberList
        {
            get { return this.GetLazyList(MemberListProperty); }
        }
        #endregion

        #region 事项计划列表 PlanList
        /// <summary>
        /// 事项计划列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProjectKeyItemPlan>> PlanListProperty = P<ProjectKeyItem>.RegisterList(e => e.PlanList);
        /// <summary>
        /// 事项计划列表
        /// </summary>
        public EntityList<ProjectKeyItemPlan> PlanList
        {
            get { return this.GetLazyList(PlanListProperty); }
        }
        #endregion

        #region 视图属性

        #region 工作计划开始时间 PlanStart
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime?> PlanStartProperty = P<ProjectKeyItem>.RegisterView(e => e.PlanStart,e=>e.ProjectWorkItem.PlanStart);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStart
        {
            get { return GetProperty(PlanStartProperty); }
            set { SetProperty(PlanStartProperty, value); }
        }
        #endregion

        #region 工作计划结束时间 PlantEnd
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime?> PlantEndProperty = P<ProjectKeyItem>.RegisterView(e => e.PlantEnd, e => e.ProjectWorkItem.PlantEnd);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlantEnd
        {
            get { return GetProperty(PlantEndProperty); }
            set { SetProperty(PlantEndProperty, value); }
        }
        #endregion

        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<ProjectKeyItem>.RegisterView(e => e.ProjectCode, p => p.Project.Code);

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
        public static readonly Property<string> ProjectNameProperty = P<ProjectKeyItem>.RegisterView(e => e.ProjectName, p => p.Project.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 项目状态 ProjectStatus
        /// <summary>
        /// 项目状态
        /// </summary>
        [Label("项目状态")]
        public static readonly Property<ProjectStatus> ProjectStatusProperty = P<ProjectKeyItem>.RegisterView(e => e.ProjectStatus, p => p.Project.ProjectStatus);

        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus ProjectStatus
        {
            get { return this.GetProperty(ProjectStatusProperty); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<ProjectKeyItem>.RegisterView(e => e.ApprovalStatus, p => p.Project.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 计划类型（界面属性） PlanType
        /// <summary>
        /// 计划类型
        /// </summary>
        [Label("计划类型")]
        public static readonly Property<PlanType> PlanTypeProperty = P<ProjectKeyItem>.Register(e => e.PlanType);

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType
        {
            get { return this.GetProperty(PlanTypeProperty); }
            set { this.SetProperty(PlanTypeProperty, value); }
        }
        #endregion

        #region 导入事项预算（界面属性） NullBudgetAmount
        /// <summary>
        /// 导入事项预算
        /// </summary>
        [Label("事项预算")]
        public static readonly Property<decimal?> NullBudgetAmountProperty = P<ProjectKeyItem>.Register(e => e.NullBudgetAmount);

        /// <summary>
        /// 导入事项预算
        /// </summary>
        public decimal? NullBudgetAmount
        {
            get { return this.GetProperty(NullBudgetAmountProperty); }
            set { this.SetProperty(NullBudgetAmountProperty, value); }
        }
        #endregion

        #region 项目计划 ProjectWorkItemName
        /// <summary>
        /// 项目计划
        /// </summary>
        [Label("项目计划")]
        public static readonly Property<string> ProjectWorkItemNameProperty = P<ProjectKeyItem>.Register(e => e.ProjectWorkItemName);

        /// <summary>
        /// 项目计划
        /// </summary>
        public string ProjectWorkItemName
        {
            get { return this.GetProperty(ProjectWorkItemNameProperty); }
            set { this.SetProperty(ProjectWorkItemNameProperty, value); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 项目关键事项 实体配置
    /// </summary>
    internal class ProjectKeyItemConfig : EntityConfig<ProjectKeyItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PROJ_KEY").MapAllProperties();
            Meta.Property(ProjectKeyItem.PlanTypeProperty).DontMapColumn();
            Meta.Property(ProjectKeyItem.NullBudgetAmountProperty).DontMapColumn();
            Meta.Property(ProjectKeyItem.ProjectWorkItemNameProperty).DontMapColumn();
            Meta.Property(ProjectKeyItem.DescriptionProperty).ColumnMeta.HasLength(160);
            Meta.Property(ProjectKeyItem.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}