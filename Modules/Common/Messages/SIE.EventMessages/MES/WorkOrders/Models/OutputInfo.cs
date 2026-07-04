using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 产量信息
    /// </summary>
    [Serializable]
    public class OutputInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string HourDate { get; set; }

        /// <summary>
        /// (实际)开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// (实际)结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// (计划)开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// (计划)结束时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 计划产量
        /// </summary>
        public decimal PlanQty { get; set; }
    }
}
