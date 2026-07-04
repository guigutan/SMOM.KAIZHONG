using SIE.ObjectModel;

namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 接收方式
    /// </summary>
    public enum ReceiveType
    {
        /// <summary>
        /// 手动接收
        /// </summary>
        [Label("手动接收")]
        Hand,

        /// <summary>
        /// 自动接收
        /// </summary>
        [Label("自动接收")]
        Auto,
    }
}
