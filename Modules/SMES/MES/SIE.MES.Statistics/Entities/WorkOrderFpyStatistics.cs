using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 工单合格率统计
    /// </summary>
    [Serializable]
    public class WorkOrderFpyStatistics
    {
        /// <summary>
        /// 合格率
        /// </summary>
        public decimal Fpy { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }
    }
}