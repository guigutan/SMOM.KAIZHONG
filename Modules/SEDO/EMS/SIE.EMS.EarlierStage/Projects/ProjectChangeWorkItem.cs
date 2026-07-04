using SIE.Domain;
using SIE.EMS.EarlierStage.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更计划
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(WorkItem))]
    [Label("项目变更计划")]
    public partial class ProjectChangeWorkItem : DataEntity
    {
        #region 项目变更 ProjectChange
        /// <summary>
        /// 项目变更Id
        /// </summary>
        [Label("项目变更")]
        public static readonly IRefIdProperty ProjectChangeIdProperty =
            P<ProjectChangeWorkItem>.RegisterRefId(e => e.ProjectChangeId, ReferenceType.Parent);

        /// <summary>
        /// 项目变更Id
        /// </summary>
        public double ProjectChangeId
        {
            get { return (double)this.GetRefId(ProjectChangeIdProperty); }
            set { this.SetRefId(ProjectChangeIdProperty, value); }
        }

        /// <summary>
        /// 项目变更
        /// </summary>
        public static readonly RefEntityProperty<ProjectChange> ProjectChangeProperty =
            P<ProjectChangeWorkItem>.RegisterRef(e => e.ProjectChange, ProjectChangeIdProperty);

        /// <summary>
        /// 项目变更
        /// </summary>
        public ProjectChange ProjectChange
        {
            get { return this.GetRefEntity(ProjectChangeProperty); }
            set { this.SetRefEntity(ProjectChangeProperty, value); }
        }
        #endregion

        #region 计划节点 WorkItem
        /// <summary>
        /// 计划节点
        /// </summary>
        [MaxLength(80)]
        [Required]
        [Label("计划节点")]
        public static readonly Property<string> WorkItemProperty = P<ProjectChangeWorkItem>.Register(e => e.WorkItem);

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
        public static readonly Property<DateTime> PlanStartProperty = P<ProjectChangeWorkItem>.Register(e => e.PlanStart);

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
        public static readonly Property<DateTime> PlantEndProperty = P<ProjectChangeWorkItem>.Register(e => e.PlantEnd);

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
        public static readonly Property<DateTime?> ActualStartProperty = P<ProjectChangeWorkItem>.Register(e => e.ActualStart);

        /// <summary>
        /// 实际开始
        /// </summary>
        public DateTime? ActualStart
        {
            get { return GetProperty(ActualStartProperty); }
            set { SetProperty(ActualStartProperty, value); }
        }
        #endregion

        #region 实际结束时间 ActaulEnd
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [Label("实际结束时间")]
        public static readonly Property<DateTime?> ActaulEndProperty = P<ProjectChangeWorkItem>.Register(e => e.ActaulEnd);

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? ActaulEnd
        {
            get { return GetProperty(ActaulEndProperty); }
            set { SetProperty(ActaulEndProperty, value); }
        }
        #endregion

        #region 责任人 Principal
        /// <summary>
        /// 责任人Id
        /// </summary>
        [Label("责任人")]
        public static readonly IRefIdProperty PrincipalIdProperty = P<ProjectChangeWorkItem>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<ProjectChangeWorkItem>.RegisterRef(e => e.Principal, PrincipalIdProperty);

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
        public static readonly Property<WorkStatus> WorkStatusProperty = P<ProjectChangeWorkItem>.Register(e => e.WorkStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public WorkStatus WorkStatus
        {
            get { return GetProperty(WorkStatusProperty); }
            set { SetProperty(WorkStatusProperty, value); }
        }
        #endregion

        #region 原工作计划id ProjectWorkItemId
        /// <summary>
        /// 原工作计划id
        /// </summary>
        [Label("原工作计划id")]
        public static readonly Property<double?> ProjectWorkItemIdProperty = P<ProjectChangeWorkItem>.Register(e => e.ProjectWorkItemId);

        /// <summary>
        /// 原工作计划id
        /// </summary>
        public double? ProjectWorkItemId
        {
            get { return this.GetProperty(ProjectWorkItemIdProperty); }
            set { this.SetProperty(ProjectWorkItemIdProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 责任人名称 PrincipalName
        /// <summary>
        /// 责任人名称
        /// </summary>
        [Label("责任人名称")]
        public static readonly Property<string> PrincipalNameProperty = P<ProjectChangeWorkItem>.RegisterView(e => e.PrincipalName, p => p.Principal.Name);

        /// <summary>
        /// 责任人名称
        /// </summary>
        public string PrincipalName
        {
            get { return this.GetProperty(PrincipalNameProperty); }
            set { SetProperty(PrincipalNameProperty, value); }

        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 项目变更计划 实体配置
    /// </summary>
    internal class ProjectChangeWorkItemConfig : EntityConfig<ProjectChangeWorkItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PRO_CHA_WORK").MapAllProperties();
            Meta.Property(ProjectChangeWorkItem.WorkItemProperty).ColumnMeta.HasLength(160);
            Meta.EnablePhantoms();
        }
    }
}
