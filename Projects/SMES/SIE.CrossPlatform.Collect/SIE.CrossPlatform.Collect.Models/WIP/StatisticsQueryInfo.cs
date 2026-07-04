using System;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 工位统计数据查询条件
    /// </summary>
    [Serializable]
    public class StatisticsQueryInfo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public double OperatorId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }
    }
}
