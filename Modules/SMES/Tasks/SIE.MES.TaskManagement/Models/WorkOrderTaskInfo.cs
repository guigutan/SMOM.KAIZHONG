using System;
using System.Collections.Generic;

namespace SIE.MES.TaskManagement.Models
{
    /// <summary>
    /// 工单任务信息
    /// </summary>
    [Serializable]
    public class WorkOrderTaskInfo
    {
        /// <summary>
        /// 待处理任务数
        /// </summary>
        public int PendingQty { get; set; }

        /// <summary>
        /// 处理中任务数
        /// </summary>
        public int ProcessingQty { get; set; }

        /// <summary>
        /// 已完成任务数
        /// </summary>
        public int CompletedQty { get; set; }

        /// <summary>
        /// 任务类型   0待处理任务、1处理中任务、2已完成任务
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 派工任务信息列表
        /// </summary>
        public List<DispatchTaskInfo> TaskInfos { get; } = new List<DispatchTaskInfo>();
    }
}