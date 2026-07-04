using SIE.ObjectModel;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 点检周期类型
    /// </summary>
    public enum CheckCycleType
    {
        /// <summary>
        /// 日
        /// </summary>
        [Label("日")]
        Day = 0,
        /// <summary>
        /// 班
        /// </summary>
        [Label("班")]
        DayShift = 1,
        /// <summary>
        /// 频次
        /// </summary>
        [Label("频次")]
        NightShift = 2,
    }
}