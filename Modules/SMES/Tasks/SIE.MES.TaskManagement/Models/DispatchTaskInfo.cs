using SIE.MES.TaskManagement.Dispatchs;
using System;

namespace SIE.MES.TaskManagement.Models
{
    /// <summary>
    /// 派工任务信息
    /// </summary>
    [Serializable]
    public class DispatchTaskInfo
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatus { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public DispatchTaskStatus TaskStatusValue { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 任务数
        /// </summary>
        public decimal TaskQty { get; set; }

        /// <summary>
        /// 任务单累计报工数
        /// </summary>
        public decimal ReportQty { get; set; }
        /// <summary>
        /// 剩余可报工数
        /// </summary>
        public decimal RemainQty { get; set; }
        /// <summary>
        /// 最大剩余可报工数
        /// </summary>
        public decimal MaxRemainQty { get; set; }

        /// <summary>
        /// 工序最大剩余可报工数
        /// </summary>
        public decimal ProcessMaxRemainQty { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// 优先级(0普通,1紧急)
        /// </summary>
        public DispatchTaskPriority PriorityValue { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualBeginDate { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? ActualEndDate { get; set; }

        /// <summary>
        /// 任务进度 0正常、1异常
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 是否可开工
        /// </summary>
        public bool CanStart { get; set; }

        /// <summary>
        /// 是否共模任务
        /// </summary>
        public bool IsSyntype { get; set; }

        /// <summary>
        /// 是否可以快速报工
        /// </summary>
        public bool IsCanQuickReport { get; set; }

        /// <summary>
        /// 是否末工序
        /// </summary>
        public bool? EndProcess { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 计划数
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode { get; set; }

        /// <summary>
        /// 报工校验
        /// </summary>
        public bool IsReportValid { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldItemCode { get; set; }
    }
}