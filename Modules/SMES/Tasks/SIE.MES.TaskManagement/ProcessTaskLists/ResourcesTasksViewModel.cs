using SIE.Domain;
using SIE.LES.MaterialPreparations;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessTaskLists
{

    /// <summary>
    /// 已排任务单
    /// </summary>
    [RootEntity, Serializable]
    [Label("已排任务单")]
    public class ResourcesTasksViewModel : ViewModel
    {

        [Label("任务单Id")]
        public static readonly Property<double> DispatchTaskIdProperty = P<ResourcesTasksViewModel>.Register(e => e.DispatchTaskId);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return GetProperty(DispatchTaskIdProperty); }
            set { SetProperty(DispatchTaskIdProperty, value); }
        }

        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<ResourcesTasksViewModel>.Register(e => e.No);

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
        public static readonly Property<decimal> DispatchQtyProperty = P<ResourcesTasksViewModel>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return GetProperty(DispatchQtyProperty); }
            set { SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 已报工数量 ReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<ResourcesTasksViewModel>.Register(e => e.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return GetProperty(ReportQtyProperty); }
            set { SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 任务执行对象 TaskPerformer
        /// <summary>
        /// 任务执行对象
        /// </summary>
        [MaxLength(1000)]
        [Label("任务执行对象")]
        public static readonly Property<string> TaskPerformerProperty = P<ResourcesTasksViewModel>.Register(e => e.TaskPerformer);

        /// <summary>
        /// 任务执行对象
        /// </summary>
        public string TaskPerformer
        {
            get { return GetProperty(TaskPerformerProperty); }
            set { SetProperty(TaskPerformerProperty, value); }
        }
        #endregion

        #region 关联工单 AssociatedWorkOrder
        /// <summary>
        /// 关联工单
        /// </summary>
        [Label("关联工单")]
        public static readonly Property<string> AssociatedWorkOrderProperty = P<ResourcesTasksViewModel>.Register(e => e.AssociatedWorkOrder);

        /// <summary>
        /// 关联工单
        /// </summary>
        public string AssociatedWorkOrder
        {
            get { return GetProperty(AssociatedWorkOrderProperty); }
            set { SetProperty(AssociatedWorkOrderProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ResourcesTasksViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ResourcesTasksViewModel>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion


        #region 附加合计工时（分） ProcessHourSum
        /// <summary>
        /// 附加合计工时（分）
        /// </summary>
        [Label("附加合计工时（分）")]
        public static readonly Property<decimal?> ProcessHourSumProperty = P<ResourcesTasksViewModel>.Register(e => e.ProcessHourSum);

        /// <summary>
        /// 附加合计工时
        /// </summary>
        public decimal? ProcessHourSum
        {
            get { return GetProperty(ProcessHourSumProperty); }
            set { SetProperty(ProcessHourSumProperty, value); }
        }
        #endregion

        #region 预计生产时长（H） ExpectedProductionTime
        /// <summary>
        /// 预计生产时长（H）
        /// </summary>
        [Label("预计生产时长（H）")]
        public static readonly Property<decimal?> ExpectedProductionTimeProperty = P<ResourcesTasksViewModel>.Register(e => e.ExpectedProductionTime);

        /// <summary>
        /// 预计生产时长（H）
        /// </summary>
        public decimal? ExpectedProductionTime
        {
            get { return GetProperty(ExpectedProductionTimeProperty); }
            set { SetProperty(ExpectedProductionTimeProperty, value); }
        }
        #endregion

        #region 工序标准工时（分） ProcessStandardHour
        /// <summary>
        /// 工序标准工时（分）
        /// </summary>
        [Label("工序标准工时（分）")]
        public static readonly Property<decimal?> ProcessStandardHourProperty = P<ResourcesTasksViewModel>.Register(e => e.ProcessStandardHour);

        /// <summary>
        /// 工序标准工时
        /// </summary>
        public decimal? ProcessStandardHour
        {
            get { return GetProperty(ProcessStandardHourProperty); }
            set { SetProperty(ProcessStandardHourProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ResourcesTasksViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<Dispatchs.DispatchTaskStatus> TaskStatusProperty = P<ResourcesTasksViewModel>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public Dispatchs.DispatchTaskStatus TaskStatus
        {
            get { return GetProperty(TaskStatusProperty); }
            set { SetProperty(TaskStatusProperty, value); }
        }
        #endregion

        #region 修改时间 UpdateDate
        /// <summary>
        /// 修改时间
        /// </summary>
        [Label("修改时间")]
        public static readonly Property<DateTime?> UpdateDateProperty = P<ResourcesTasksViewModel>.Register(e => e.UpdateDate);

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { SetProperty(UpdateDateProperty, value); }
        }
        #endregion

        #region 修改人 UpdateByName
        /// <summary>
        /// 修改人
        /// </summary>
        [Label("修改人")]
        public static readonly Property<string> UpdateByNameProperty = P<ResourcesTasksViewModel>.Register(e => e.UpdateByName);

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateByName
        {
            get { return this.GetProperty(UpdateByNameProperty); }
            set { SetProperty(UpdateByNameProperty, value); }
        }
        #endregion

        #region 生产剩余时长（H） ProcessStandardHour
        /// <summary>
        /// 生产剩余时长（H）
        /// </summary>
        [Label("生产剩余时长（H）")]
        public static readonly Property<decimal> RemainingTimeProperty = P<ResourcesTasksViewModel>.RegisterReadOnly(
            e => e.RemainingTime, e => e.GetRemainingTime(), DispatchQtyProperty, ReportQtyProperty, ProcessStandardHourProperty);
        /// <summary>
        /// 待收数
        /// </summary>

        public decimal RemainingTime
        {
            get { return this.GetProperty(RemainingTimeProperty); }
        }
        private decimal GetRemainingTime()
        {
            return ((DispatchQty - ReportQty) * (ProcessStandardHour.HasValue ? ProcessStandardHour.Value : 0))/60;
        }
        #endregion
    }
}
