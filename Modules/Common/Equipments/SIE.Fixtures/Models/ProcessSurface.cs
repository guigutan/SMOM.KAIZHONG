using SIE.ObjectModel;

namespace SIE.Fixtures.Models
{
    /// <summary>
    /// 工艺面
    /// </summary>
    public enum ProcessSurface
    {
        /// <summary>
        /// 正面(TOP)
        /// </summary>
        [Label("正面(TOP)")]
        TOP = 5,
        /// <summary>
        /// 反面(BOT)
        /// </summary>
        [Label("反面(BOT)")]
        BOT = 15,
        /// <summary>
        /// 整体（T/B）
        /// </summary>
        [Label("整体（T/B）")]
        TB = 20,
    }
}