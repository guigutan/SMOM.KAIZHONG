using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.TaskExtensions
{

    [RootEntity]
    [Label("报工任务")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ReportTaskViewModel”的 XML 注释
    public class ReportTaskViewModel : ViewModel
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ReportTaskViewModel”的 XML 注释
    {
        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<ReportTaskViewModel>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<ReportTaskViewModel>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return this.GetProperty(DispatchQtyProperty); }
            set { this.SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 已报工数量 ReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<ReportTaskViewModel>.Register(e => e.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
            set { this.SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 待报工 ToReportQty
        /// <summary>
        /// 待报工
        /// </summary>
        [Label("待报工")]
        public static readonly Property<decimal> ToReportQtyProperty = P<ReportTaskViewModel>.Register(e => e.ToReportQty);

        /// <summary>
        /// 待报工
        /// </summary>
        public decimal ToReportQty
        {
            get { return this.GetProperty(ToReportQtyProperty); }
            set { this.SetProperty(ToReportQtyProperty, value); }
        }
        #endregion 

        #region 合格数量 OkQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<decimal> OkQtyProperty = P<ReportTaskViewModel>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get { return GetProperty(OkQtyProperty); }
            set { SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<ReportTaskViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<ReportTaskViewModel>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 工单编号 WorkOrderNo
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ReportTaskViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ReportTaskViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<string> PriorityProperty = P<ReportTaskViewModel>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority
        {
            get { return this.GetProperty(PriorityProperty); }
            set { this.SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ReportTaskViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<ReportTaskViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 任务状态 TaskStatus
        /// <summary>
        /// 任务状态
        /// </summary>
        [Label("任务状态")]
        public static readonly Property<string> TaskStatusProperty = P<ReportTaskViewModel>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatus
        {
            get { return this.GetProperty(TaskStatusProperty); }
            set { this.SetProperty(TaskStatusProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime?> BeginTimeProperty = P<ReportTaskViewModel>.Register(e => e.BeginTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get { return this.GetProperty(BeginTimeProperty); }
            set { this.SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 结束时间 EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime?> EndTimeProperty = P<ReportTaskViewModel>.Register(e => e.EndTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 发布时间 CreateDate
        /// <summary>
        /// 发布时间
        /// </summary>
        [Label("发布时间")]
        public static readonly Property<DateTime> CreateDateProperty = P<ReportTaskViewModel>.Register(e => e.CreateDate);

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion 

        #region 计划开始时间 PlanBeginTime
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginTimeProperty = P<ReportTaskViewModel>.Register(e => e.PlanBeginTime);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginTime
        {
            get { return this.GetProperty(PlanBeginTimeProperty); }
            set { this.SetProperty(PlanBeginTimeProperty, value); }
        }
        #endregion

        #region 计划结束时间 PlanEndTime
        /// <summary>
        /// 注释
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> PlanEndTimeProperty = P<ReportTaskViewModel>.Register(e => e.PlanEndTime);

        /// <summary>
        /// 注释
        /// </summary>
        public DateTime PlanEndTime
        {
            get { return this.GetProperty(PlanEndTimeProperty); }
            set { this.SetProperty(PlanEndTimeProperty, value); }
        }
        #endregion
    }
}
