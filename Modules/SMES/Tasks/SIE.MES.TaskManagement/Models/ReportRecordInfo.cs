using System;

namespace SIE.MES.TaskManagement.Models
{
    /// <summary>
    /// 报工记录信息
    /// </summary>
    [Serializable]
    public class ReportRecordInfo
    {
        /// <summary>
        /// 责任人
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// 报工数
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
        /// 统计工时
        /// </summary>
        public decimal Hour { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }
    }
}