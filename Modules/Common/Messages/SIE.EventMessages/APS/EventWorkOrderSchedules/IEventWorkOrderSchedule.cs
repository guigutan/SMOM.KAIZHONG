using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.EventWorkOrderSchedules
{
    /// <summary>
    /// 工单事件进度接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyEventWorkOrderSchedule))]
    public interface IEventWorkOrderSchedule
    {
        /// <summary>
        /// 工单事件进度
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        IReadOnlyList<WorkOrderScheduleInfo> EventWorkOrderSchedule(List<string> workOrderNo);
    }

    /// <summary>
    /// 工单事件进度接口默认实现
    /// </summary>
    public class EmptyEventWorkOrderSchedule : IEventWorkOrderSchedule
    {
        /// <summary>
        /// 工单事件进度
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        public IReadOnlyList<WorkOrderScheduleInfo> EventWorkOrderSchedule(List<string> workOrderNo)
        {
            return new List<WorkOrderScheduleInfo>();
        }
    }

    /// <summary>
    /// 工单事件进度数据列表
    /// </summary>
    [Serializable]
    public class WorkOrderScheduleInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 制程工艺单号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }

        /// <summary>
        /// 关键事件编码
        /// </summary>
        public string CriticalEventCode { get; set; }

        /// <summary>
        /// 关键事件名称
        /// </summary>
        public string CriticalEventName { get; set; }

        /// <summary>
        /// 要求完成时间
        /// </summary>
        public DateTime? GuidanceFinishDate { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? PlanFinishDate { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualFinishDate { get; set; }
    }
}
