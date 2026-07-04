using System;

namespace SIE.EventMessages.MES.ProcessStatistics
{
    /// <summary>
    /// 工序采集信息
    /// </summary>
    [Serializable]
    public class ProcessStatisticsEventInfo
    {
        /// <summary>
        /// 工序顺序
        /// </summary>
        public int ProcessIndex { get; set; }
        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 过板数
        /// </summary>
        public decimal InputQty { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal PassQty { get; set; }

        /// <summary>
        /// 不良数
        /// </summary>
        public decimal FailedQty { get; set; }
        
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WOId { get; set; }
    }
}
