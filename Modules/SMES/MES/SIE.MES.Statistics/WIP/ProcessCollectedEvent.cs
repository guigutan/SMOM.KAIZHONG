namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 工序统计
    /// </summary>
    [System.Serializable]
    public class ProcessCollectedEvent
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 成功数量，重复条码不累加
        /// </summary>
        public decimal QtyPass { get; set; }

        /// <summary>
        /// 失败数量，重复条码不累加
        /// </summary>
        public decimal QtyFailed { get; set; }

        /// <summary>
        /// 成功台次
        /// </summary>
        public decimal QtyTimes { get; set; }

        /// <summary>
        /// 失败台次
        /// </summary>
        public decimal QtyFaildTimes { get; set; }
    }
}
