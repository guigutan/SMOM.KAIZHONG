using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 任务详情ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("任务详情")]
    public class TaskDetailViewModel : ViewModel
    {
        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<TaskDetailViewModel>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<TaskDetailViewModel>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return GetProperty(DispatchQtyProperty); }
            set { SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<TaskDetailViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<TaskDetailViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<DispatchTaskPriority> PriorityProperty = P<TaskDetailViewModel>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public DispatchTaskPriority Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<TaskDetailViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<TaskDetailViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 已报工数量 ReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<TaskDetailViewModel>.Register(e => e.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return GetProperty(ReportQtyProperty); }
            set { SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<DispatchTaskStatus> TaskStatusProperty = P<TaskDetailViewModel>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus TaskStatus
        {
            get { return GetProperty(TaskStatusProperty); }
            set { SetProperty(TaskStatusProperty, value); }
        }
        #endregion

        #region 规格件编码 SpecificationCode
        /// <summary>
        /// 规格件编码
        /// </summary>
        [Label("规格件编码")]
        public static readonly Property<string> SpecificationCodeProperty = P<TaskDetailViewModel>.Register(e => e.SpecificationCode);

        /// <summary>
        /// 规格件编码
        /// </summary>
        public string SpecificationCode
        {
            get { return GetProperty(SpecificationCodeProperty); }
            set { SetProperty(SpecificationCodeProperty, value); }
        }
        #endregion

        #region 规格件名称 SpecificationName
        /// <summary>
        /// 规格件名称
        /// </summary>
        [Label("规格件名称")]
        public static readonly Property<string> SpecificationNameProperty = P<TaskDetailViewModel>.Register(e => e.SpecificationName);

        /// <summary>
        /// 规格件名称
        /// </summary>
        public string SpecificationName
        {
            get { return GetProperty(SpecificationNameProperty); }
            set { SetProperty(SpecificationNameProperty, value); }
        }
        #endregion

        #region 报工方式 ReportMode
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<ReportMode> ReportModeProperty = P<TaskDetailViewModel>.Register(e => e.ReportMode);

        /// <summary>
        /// 报工方式
        /// </summary>
        public ReportMode ReportMode
        {
            get { return GetProperty(ReportModeProperty); }
            set { SetProperty(ReportModeProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginTime
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginTimeProperty = P<TaskDetailViewModel>.Register(e => e.PlanBeginTime);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginTime
        {
            get { return GetProperty(PlanBeginTimeProperty); }
            set { SetProperty(PlanBeginTimeProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndTime
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateTime> PlanEndTimeProperty = P<TaskDetailViewModel>.Register(e => e.PlanEndTime);

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndTime
        {
            get { return GetProperty(PlanEndTimeProperty); }
            set { SetProperty(PlanEndTimeProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime?> BeginTimeProperty = P<TaskDetailViewModel>.Register(e => e.BeginTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get { return GetProperty(BeginTimeProperty); }
            set { SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 完成时间 EndTime
        /// <summary>
        /// 完成时间
        /// </summary>
        [Label("完成时间")]
        public static readonly Property<DateTime?> EndTimeProperty = P<TaskDetailViewModel>.Register(e => e.EndTime);

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return GetProperty(EndTimeProperty); }
            set { SetProperty(EndTimeProperty, value); }
        }
        #endregion
    }
}
