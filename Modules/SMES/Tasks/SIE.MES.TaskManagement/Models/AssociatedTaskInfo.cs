using System;

namespace SIE.MES.TaskManagement.Models
{
    /// <summary>
    /// 关联任务信息
    /// </summary>
    [Serializable]
    public class AssociatedTaskInfo
    {
        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrder { get; set; }

        /// <summary>
        /// 任务数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal OkQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }
    }
}