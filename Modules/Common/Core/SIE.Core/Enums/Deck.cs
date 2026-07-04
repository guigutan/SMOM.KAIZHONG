using SIE.ObjectModel;

namespace SIE.Core.Enums
{
    /// <summary>
    /// 板面
    /// </summary>
    public enum Deck
    {
        /// <summary>
        /// 整体(T/B)
        /// </summary>
        [Label("整体(T/B)")]
        All = 0,

        /// <summary>
        /// 正面(TOP)
        /// </summary>
        [Label("正面(TOP)")]
        TOP = 5,

        /// <summary>
        /// 背面(BOT)
        /// </summary>
        [Label("背面(BOT)")]
        BOT = 10,
    }
}
