using SIE.ObjectModel;

namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 接收记录状态
    /// </summary>
    public enum ReceiveState
    {
        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        TobeReceived,

        /// <summary>
        /// 部分接收
        /// </summary>
        [Label("部分接收")]
        PartReceived,

        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        Received,

        /// <summary>
        /// 已拒收
        /// </summary>
        [Label("已拒收")]
        Rejected,
    }
}
