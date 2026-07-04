using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Completion
{
    /// <summary>
    /// 资源生产明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("资源生产明细")]
    public class ProductCompletionDetail: ViewModel
    {
        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> TaskNoProperty = P<ProductCompletionDetail>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 任务数量 TaskQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> TaskQtyProperty = P<ProductCompletionDetail>.Register(e => e.TaskQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal TaskQty
        {
            get { return this.GetProperty(TaskQtyProperty); }
            set { this.SetProperty(TaskQtyProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProductCompletionDetail>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProductCompletionDetail>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ProductCompletionDetail>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProductCompletionDetail>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProductCompletionDetail>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 已报工数量 ReportedQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportedQtyProperty = P<ProductCompletionDetail>.Register(e => e.ReportedQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportedQty
        {
            get { return this.GetProperty(ReportedQtyProperty); }
            set { this.SetProperty(ReportedQtyProperty, value); }
        }
        #endregion

        #region 合格数量 OkQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<decimal> OkQtyProperty = P<ProductCompletionDetail>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get { return this.GetProperty(OkQtyProperty); }
            set { this.SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<ProductCompletionDetail>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 返工数量 ReworkQty
        /// <summary>
        /// 返工数量
        /// </summary>
        [Label("返工数量")]
        public static readonly Property<decimal> ReworkQtyProperty = P<ProductCompletionDetail>.Register(e => e.ReworkQty);

        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal ReworkQty
        {
            get { return this.GetProperty(ReworkQtyProperty); }
            set { this.SetProperty(ReworkQtyProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<ProductCompletionDetail>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<ProductCompletionDetail>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<ProductCompletionDetail>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 班次 Classes
        /// <summary>
        /// 班次
        /// </summary>
        [Label("班次")]
        public static readonly Property<ClassesType?> ClassesProperty = P<ProductCompletionDetail>.Register(e => e.Classes);

        /// <summary>
        /// 班次
        /// </summary>
        public ClassesType? Classes
        {
            get { return this.GetProperty(ClassesProperty); }
            set { this.SetProperty(ClassesProperty, value); }
        }
        #endregion

        #region 排程开始时间 ScheduleStartTime
        /// <summary>
        /// 排程开始时间
        /// </summary>
        [Label("排程开始时间")]
        public static readonly Property<DateTime?> ScheduleStartTimeProperty = P<ProductCompletionDetail>.Register(e => e.ScheduleStartTime);

        /// <summary>
        /// 排程开始时间
        /// </summary>
        public DateTime? ScheduleStartTime
        {
            get { return this.GetProperty(ScheduleStartTimeProperty); }
            set { this.SetProperty(ScheduleStartTimeProperty, value); }
        }
        #endregion

        #region 排程完成时间 ScheduleEndTime
        /// <summary>
        /// 排程完成时间
        /// </summary>
        [Label("排程完成时间")]
        public static readonly Property<DateTime?> ScheduleEndTimeProperty = P<ProductCompletionDetail>.Register(e => e.ScheduleEndTime);

        /// <summary>
        /// 排程完成时间
        /// </summary>
        public DateTime? ScheduleEndTime
        {
            get { return this.GetProperty(ScheduleEndTimeProperty); }
            set { this.SetProperty(ScheduleEndTimeProperty, value); }
        }
        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<ProductCompletionDetail>.Register(e => e.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
            set { this.SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        #region 班次数量 ShiftCount
        /// <summary>
        /// 班次数量
        /// </summary>
        [Label("班次数量")]
        public static readonly Property<int> ShiftCountProperty = P<ProductCompletionDetail>.Register(e => e.ShiftCount);

        /// <summary>
        /// 班次数量
        /// </summary>
        public int ShiftCount
        {
            get { return this.GetProperty(ShiftCountProperty); }
            set { this.SetProperty(ShiftCountProperty, value); }
        }
        #endregion
    }
}
