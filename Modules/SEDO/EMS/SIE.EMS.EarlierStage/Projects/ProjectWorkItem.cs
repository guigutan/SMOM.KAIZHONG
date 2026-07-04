using SIE.Domain;
using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 工作计划
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(WorkItem))]
    [Label("工作计划")]
    public partial class ProjectWorkItem : DataEntity
    {
        #region 计划节点 WorkItem
        /// <summary>
        /// 计划节点
        /// </summary>
        [MaxLength(80)]
        [Required]
        [Label("计划节点")]
        public static readonly Property<string> WorkItemProperty = P<ProjectWorkItem>.Register(e => e.WorkItem);

        /// <summary>
        /// 计划节点
        /// </summary>
        public string WorkItem
        {
            get { return GetProperty(WorkItemProperty); }
            set { SetProperty(WorkItemProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanStart
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanStartProperty = P<ProjectWorkItem>.Register(e => e.PlanStart);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStart
        {
            get { return GetProperty(PlanStartProperty); }
            set { SetProperty(PlanStartProperty, value); }
        }
        #endregion

        #region 计划结束时间 PlantEnd
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> PlantEndProperty = P<ProjectWorkItem>.Register(e => e.PlantEnd);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlantEnd
        {
            get { return GetProperty(PlantEndProperty); }
            set { SetProperty(PlantEndProperty, value); }
        }
        #endregion

        #region 实际开始 ActualStart
        /// <summary>
        /// 实际开始
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> ActualStartProperty = P<ProjectWorkItem>.Register(e => e.ActualStart);

        /// <summary>
        /// 实际开始
        /// </summary>
        public DateTime? ActualStart
        {
            get { return GetProperty(ActualStartProperty); }
            set { SetProperty(ActualStartProperty, value); }
        }
        #endregion

        #region 实际开始人 ActualStartPeople
        /// <summary>
        /// 实际开始人Id
        /// </summary>
        [Label("实际开始人")]
        public static readonly IRefIdProperty ActualStartPeopleIdProperty = P<ProjectWorkItem>.RegisterRefId(e => e.ActualStartPeopleId, ReferenceType.Normal);

        /// <summary>
        /// 实际开始人Id
        /// </summary>
        public double? ActualStartPeopleId
        {
            get { return (double?)this.GetRefNullableId(ActualStartPeopleIdProperty); }
            set { this.SetRefNullableId(ActualStartPeopleIdProperty, value); }
        }

        /// <summary>
        /// 实际开始人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ActualStartPeopleProperty = P<ProjectWorkItem>.RegisterRef(e => e.ActualStartPeople, ActualStartPeopleIdProperty);

        /// <summary>
        /// 实际开始人
        /// </summary>
        public Employee ActualStartPeople
        {
            get { return GetRefEntity(ActualStartPeopleProperty); }
            set { SetRefEntity(ActualStartPeopleProperty, value); }
        }
        #endregion

        #region 实际结束时间 ActaulEnd
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [Label("实际结束时间")]
        public static readonly Property<DateTime?> ActaulEndProperty = P<ProjectWorkItem>.Register(e => e.ActaulEnd);

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? ActaulEnd
        {
            get { return GetProperty(ActaulEndProperty); }
            set { SetProperty(ActaulEndProperty, value); }
        }
        #endregion

        #region 实际结束人 ActaulEndPeople
        /// <summary>
        /// 实际结束人Id
        /// </summary>
        [Label("实际结束人")]
        public static readonly IRefIdProperty ActaulEndPeopleIdProperty = P<ProjectWorkItem>.RegisterRefId(e => e.ActaulEndPeopleId, ReferenceType.Normal);

        /// <summary>
        /// 实际结束人Id
        /// </summary>
        public double? ActaulEndPeopleId
        {
            get { return (double?)this.GetRefNullableId(ActaulEndPeopleIdProperty); }
            set { this.SetRefNullableId(ActaulEndPeopleIdProperty, value); }
        }

        /// <summary>
        /// 实际结束人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ActaulEndPeopleProperty = P<ProjectWorkItem>.RegisterRef(e => e.ActaulEndPeople, ActaulEndPeopleIdProperty);

        /// <summary>
        /// 实际结束人
        /// </summary>
        public Employee ActaulEndPeople
        {
            get { return GetRefEntity(ActaulEndPeopleProperty); }
            set { SetRefEntity(ActaulEndPeopleProperty, value); }
        }
        #endregion

        #region 责任人 Principal
        /// <summary>
        /// 责任人Id
        /// </summary>
        [Label("责任人")]
        public static readonly IRefIdProperty PrincipalIdProperty = P<ProjectWorkItem>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

        /// <summary>
        /// 责任人Id
        /// </summary>
        public double PrincipalId
        {
            get { return (double)GetRefId(PrincipalIdProperty); }
            set { SetRefId(PrincipalIdProperty, value); }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<ProjectWorkItem>.RegisterRef(e => e.Principal, PrincipalIdProperty);

        /// <summary>
        /// 责任人
        /// </summary>
        public Employee Principal
        {
            get { return GetRefEntity(PrincipalProperty); }
            set { SetRefEntity(PrincipalProperty, value); }
        }
        #endregion

        #region 状态 WorkStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<WorkStatus> WorkStatusProperty = P<ProjectWorkItem>.Register(e => e.WorkStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public WorkStatus WorkStatus
        {
            get { return GetProperty(WorkStatusProperty); }
            set { SetProperty(WorkStatusProperty, value); }
        }
        #endregion

        #region 项目 Project
        /// <summary>
        /// 项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectIdProperty = P<ProjectWorkItem>.RegisterRefId(e => e.ProjectId, ReferenceType.Parent);

        /// <summary>
        /// 项目Id
        /// </summary>
        public double ProjectId
        {
            get { return (double)GetRefId(ProjectIdProperty); }
            set { SetRefId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static readonly RefEntityProperty<Project> ProjectProperty = P<ProjectWorkItem>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return GetRefEntity(ProjectProperty); }
            set { SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<ProjectWorkItem>.RegisterView(e => e.ProjectCode, p => p.Project.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
        }
        #endregion

        #region 责任人名称 PrincipalName
        /// <summary>
        /// 责任人名称
        /// </summary>
        [Label("责任人名称")]
        public static readonly Property<string> PrincipalNameProperty = P<ProjectWorkItem>.RegisterView(e => e.PrincipalName, p => p.Principal.Name);

        /// <summary>
        /// 责任人名称
        /// </summary>
        public string PrincipalName
        {
            get { return this.GetProperty(PrincipalNameProperty); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<ProjectWorkItem>.RegisterView(e => e.ApprovalStatus, p => p.Project.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion

        #region 项目状态 ProjectStatus
        /// <summary>
        /// 项目状态
        /// </summary>
        [Label("项目状态")]
        public static readonly Property<ProjectStatus> ProjectStatusProperty = P<ProjectWorkItem>.RegisterView(e => e.ProjectStatus, p => p.Project.ProjectStatus);

        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus ProjectStatus
        {
            get { return GetProperty(ProjectStatusProperty); }
            set { SetProperty(ProjectStatusProperty, value); }
        }
        #endregion
        #endregion

    }

    /// <summary>
    /// 工作计划 实体配置
    /// </summary>
    internal class ProjectWorkItemConfig : EntityConfig<ProjectWorkItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PROJ_ITEM").MapAllProperties();
            Meta.Property(ProjectWorkItem.WorkItemProperty).ColumnMeta.HasLength(160);
            Meta.EnablePhantoms();
        }
    }
}