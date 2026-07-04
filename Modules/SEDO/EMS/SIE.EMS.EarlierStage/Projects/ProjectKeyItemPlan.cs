using SIE.Domain;
using SIE.EMS.EarlierStage.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 事项计划
    /// </summary>
    [ChildEntity, Serializable]
    [Label("事项计划")]
    public partial class ProjectKeyItemPlan : DataEntity
    {
        #region 计划节点 PlanNode
        /// <summary>
        /// 计划节点
        /// </summary>
        [MaxLength(80)]
        [Required]
        [Label("计划节点")]
        public static readonly Property<string> PlanNodeProperty = P<ProjectKeyItemPlan>.Register(e => e.PlanNode);

        /// <summary>
        /// 计划节点
        /// </summary>
        public string PlanNode
        {
            get { return GetProperty(PlanNodeProperty); }
            set { SetProperty(PlanNodeProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanStart
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [MaxLength(80)]
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanStartProperty = P<ProjectKeyItemPlan>.Register(e => e.PlanStart);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStart
        {
            get { return GetProperty(PlanStartProperty); }
            set { SetProperty(PlanStartProperty, value); }
        }
        #endregion

        #region 计划结束时间 PlanEnd
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> PlanEndProperty = P<ProjectKeyItemPlan>.Register(e => e.PlanEnd);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEnd
        {
            get { return GetProperty(PlanEndProperty); }
            set { SetProperty(PlanEndProperty, value); }
        }
        #endregion

        #region 实际开始时间 ActualSart
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> ActualSartProperty = P<ProjectKeyItemPlan>.Register(e => e.ActualSart);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualSart
        {
            get { return GetProperty(ActualSartProperty); }
            set { SetProperty(ActualSartProperty, value); }
        }
        #endregion

        #region 实际开始人 ActualStartPeople
        /// <summary>
        /// 实际开始人Id
        /// </summary>
        [Label("实际开始人")]
        public static readonly IRefIdProperty ActualStartPeopleIdProperty = P<ProjectKeyItemPlan>.RegisterRefId(e => e.ActualStartPeopleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> ActualStartPeopleProperty = P<ProjectKeyItemPlan>.RegisterRef(e => e.ActualStartPeople, ActualStartPeopleIdProperty);

        /// <summary>
        /// 实际开始人
        /// </summary>
        public Employee ActualStartPeople
        {
            get { return GetRefEntity(ActualStartPeopleProperty); }
            set { SetRefEntity(ActualStartPeopleProperty, value); }
        }
        #endregion

        #region 实际结束时间 ActualEnd
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [Label("实际结束时间")]
        public static readonly Property<DateTime?> ActualEndProperty = P<ProjectKeyItemPlan>.Register(e => e.ActualEnd);

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? ActualEnd
        {
            get { return GetProperty(ActualEndProperty); }
            set { SetProperty(ActualEndProperty, value); }
        }
        #endregion

        #region 实际结束人 ActaulEndPeople
        /// <summary>
        /// 实际结束人Id
        /// </summary>
        [Label("实际结束人")]
        public static readonly IRefIdProperty ActaulEndPeopleIdProperty = P<ProjectKeyItemPlan>.RegisterRefId(e => e.ActaulEndPeopleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> ActaulEndPeopleProperty = P<ProjectKeyItemPlan>.RegisterRef(e => e.ActaulEndPeople, ActaulEndPeopleIdProperty);

        /// <summary>
        /// 实际结束人
        /// </summary>
        public Employee ActaulEndPeople
        {
            get { return GetRefEntity(ActaulEndPeopleProperty); }
            set { SetRefEntity(ActaulEndPeopleProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProjectKeyItemPlan>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 责任人 Principal
        /// <summary>
        /// 责任人Id
        /// </summary>
        public static readonly IRefIdProperty PrincipalIdProperty = P<ProjectKeyItemPlan>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<ProjectKeyItemPlan>.RegisterRef(e => e.Principal, PrincipalIdProperty);

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
        public static readonly Property<WorkStatus> WorkStatusProperty = P<ProjectKeyItemPlan>.Register(e => e.WorkStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public WorkStatus WorkStatus
        {
            get { return GetProperty(WorkStatusProperty); }
            set { SetProperty(WorkStatusProperty, value); }
        }
        #endregion

        #region 项目关键事项 ProjectKeyItem
        /// <summary>
        /// 项目关键事项Id
        /// </summary>
        public static readonly IRefIdProperty ProjectKeyItemIdProperty = P<ProjectKeyItemPlan>.RegisterRefId(e => e.ProjectKeyItemId, ReferenceType.Parent);

        /// <summary>
        /// 项目关键事项Id
        /// </summary>
        public double ProjectKeyItemId
        {
            get { return (double)GetRefId(ProjectKeyItemIdProperty); }
            set { SetRefId(ProjectKeyItemIdProperty, value); }
        }

        /// <summary>
        /// 项目关键事项
        /// </summary>
        public static readonly RefEntityProperty<ProjectKeyItem> ProjectKeyItemProperty = P<ProjectKeyItemPlan>.RegisterRef(e => e.ProjectKeyItem, ProjectKeyItemIdProperty);

        /// <summary>
        /// 项目关键事项
        /// </summary>
        public ProjectKeyItem ProjectKeyItem
        {
            get { return GetRefEntity(ProjectKeyItemProperty); }
            set { SetRefEntity(ProjectKeyItemProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<ProjectKeyItemPlan>.RegisterView(e => e.ApprovalStatus, p => p.ProjectKeyItem.Project.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 事项计划 实体配置
    /// </summary>
    internal class ProjectKeyItemPlanConfig : EntityConfig<ProjectKeyItemPlan>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PROJ_KEY_PLAN").MapAllProperties();
            Meta.Property(ProjectKeyItemPlan.PlanNodeProperty).ColumnMeta.HasLength(160);
            Meta.Property(ProjectKeyItemPlan.PlanStartProperty).ColumnMeta.HasLength(160);
            Meta.Property(ProjectKeyItemPlan.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}