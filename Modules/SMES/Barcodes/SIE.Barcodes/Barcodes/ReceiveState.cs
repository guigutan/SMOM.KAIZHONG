using SIE.ObjectModel;

namespace SIE.Barcodes
{
    /// <summary>
    /// 领用状态
    /// </summary>
    public enum ReceiveState
    {
        /// <summary>
        /// 未领用
        /// </summary>
        [Label("未领用")]
        NoReceive = 0,

        /// <summary>
        /// 已领用
        /// </summary>
        [Label("已领用")]
        Received = 1,
    }
}