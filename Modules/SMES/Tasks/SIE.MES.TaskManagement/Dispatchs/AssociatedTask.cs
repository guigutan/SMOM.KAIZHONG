using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 关联任务单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("派工任务")]
    public partial class AssociatedTask : DataEntity
    {
        #region 派工任务（主单） DispatchTask
        /// <summary>
        /// 派工任务（主单）Id
        /// </summary>
        [Label("派工任务（主单）")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<AssociatedTask>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Parent);

        /// <summary>
        /// 派工任务（主单）Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工任务（主单）
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<AssociatedTask>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务（主单）
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion 

        #region 关联任务单 Task
        /// <summary>
        /// 关联任务单Id
        /// </summary>
        [Label("关联任务单")]
        public static readonly IRefIdProperty TaskIdProperty =
            P<AssociatedTask>.RegisterRefId(e => e.TaskId, ReferenceType.Normal);

        /// <summary>
        /// 关联任务单Id
        /// </summary>
        public double TaskId
        {
            get { return (double)this.GetRefId(TaskIdProperty); }
            set { this.SetRefId(TaskIdProperty, value); }
        }

        /// <summary>
        /// 关联任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> TaskProperty =
            P<AssociatedTask>.RegisterRef(e => e.Task, TaskIdProperty);

        /// <summary>
        /// 关联任务单
        /// </summary>
        public DispatchTask Task
        {
            get { return this.GetRefEntity(TaskProperty); }
            set { this.SetRefEntity(TaskProperty, value); }
        }
        #endregion

        #region 视图显示
        #region 关联关系 TaskAssociated
        /// <summary>
        /// 关联关系
        /// </summary>
        [Label("关联关系")]
        public static readonly Property<Associated?> TaskAssociatedProperty = P<AssociatedTask>.RegisterView(e => e.TaskAssociated, p => p.Task.Associated);

        /// <summary>
        /// 关联关系
        /// </summary>
        public Associated? TaskAssociated
        {
            get { return this.GetProperty(TaskAssociatedProperty); }
        }
        #endregion

        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> TaskNoProperty = P<AssociatedTask>.RegisterView(e => e.TaskNo, p => p.Task.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
        }
        #endregion

        #region 任务数量 TaskNo
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> TaskDispatchQtyProperty = P<AssociatedTask>.RegisterView(e => e.TaskDispatchQty, p => p.Task.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal TaskDispatchQty
        {
            get { return this.GetProperty(TaskDispatchQtyProperty); }
        }

        #endregion

        #region 已报工数量 TaskReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> TaskReportQtyProperty = P<AssociatedTask>.RegisterView(e => e.TaskReportQty, p => p.Task.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal TaskReportQty
        {
            get { return this.GetProperty(TaskReportQtyProperty); }
        }

        #endregion

        #region 不合格数量 TaskNgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> TaskNgQtyProperty = P<AssociatedTask>.RegisterView(e => e.TaskNgQty, p => p.Task.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal TaskNgQty
        {
            get { return this.GetProperty(TaskNgQtyProperty); }
        }

        #endregion

        #region 任务单状态 TaskTaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<DispatchTaskStatus> TaskTaskStatusProperty = P<AssociatedTask>.RegisterView(e => e.TaskTaskStatus, p => p.Task.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus TaskTaskStatus
        {
            get { return this.GetProperty(TaskTaskStatusProperty); }
        }

        #endregion

        #region 优先级 TaskPriority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<DispatchTaskPriority> TaskPriorityProperty = P<AssociatedTask>.RegisterView(e => e.TaskPriority, p => p.Task.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public DispatchTaskPriority TaskPriority
        {
            get { return this.GetProperty(TaskPriorityProperty); }
        }

        #endregion

        #region 任务执行对象 TaskTaskPerformer
        /// <summary>
        /// 任务执行对象
        /// </summary>
        [Label("任务执行对象")]
        public static readonly Property<string> TaskTaskPerformerProperty = P<AssociatedTask>.RegisterView(e => e.TaskTaskPerformer, p => p.Task.TaskPerformer);

        /// <summary>
        /// 任务执行对象
        /// </summary>
        public string TaskTaskPerformer
        {
            get { return this.GetProperty(TaskTaskPerformerProperty); }
        }

        #endregion

        #region 计划开始时间 TaskPlanBeginTime
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<string> TaskPlanBeginTimeProperty = P<AssociatedTask>.RegisterView(e => e.TaskPlanBeginTime, p => p.Task.PlanBeginTime);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string TaskPlanBeginTime
        {
            get { return this.GetProperty(TaskPlanBeginTimeProperty); }
        }

        #endregion

        #region 计划完成时间 TaskPlanEndTime
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<string> TaskPlanEndTimeProperty = P<AssociatedTask>.RegisterView(e => e.TaskPlanEndTime, p => p.Task.PlanEndTime);

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public string TaskPlanEndTime
        {
            get { return this.GetProperty(TaskPlanEndTimeProperty); }
        }

        #endregion

        #region 合并状态 MergedStatus
        /// <summary>
        /// 合并状态
        /// </summary>
        [Label("合并状态")]
        public static readonly Property<MergedStatus> MergedStatusProperty = P<AssociatedTask>.RegisterView(e => e.MergedStatus, p => p.Task.MergedStatus);

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public MergedStatus MergedStatus
        {
            get { return this.GetProperty(MergedStatusProperty); }
        }

        #endregion               
        #endregion
    }

    /// <summary>
    /// 关联任务单 实体配置
    /// </summary>
    internal class AssociatedTaskEntityConfig : EntityConfig<AssociatedTask>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_ASSOCIATE_TASK").MapAllProperties();
            Meta.Property(AssociatedTask.TaskIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}