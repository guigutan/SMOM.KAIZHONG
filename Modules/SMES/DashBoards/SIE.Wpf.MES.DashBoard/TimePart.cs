using SIE.ObjectModel;

namespace SIE.Wpf.MES.DashBoard
{
    /// <summary>
    /// 时间单位
    /// </summary>
    public enum TimePart
    {
        /// <summary>
        /// 小时
        /// </summary>
        [Label("小时")]
        Hours,

        /// <summary>
        /// 分
        /// </summary>
        [Label("分")]
        Minutes,

        /// <summary>
        /// 秒
        /// </summary>
        [Label("秒")]
        Seconds,

        /// <summary>
        /// 天
        /// </summary>
        [Label("天")]
        Days
    }
}
