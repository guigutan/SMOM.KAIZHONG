using SIE.ObjectModel;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 接收记录状态
    /// </summary>
    public enum ReceiveState
    {
        /// <summary>
        /// 拣配中
        /// </summary>
        [Label("拣配中")]
        PickStocking,

        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        TobeReceived,

        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        Received,
    }
}
