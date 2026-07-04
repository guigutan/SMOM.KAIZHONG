using SIE.ObjectModel;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 接收状态
    /// </summary>
    public enum ReceiveState
    {
        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        Received,

        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        ToReceive,
    }
}