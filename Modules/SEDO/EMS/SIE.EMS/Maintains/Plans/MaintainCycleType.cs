using SIE.ObjectModel;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养周期类型
    /// </summary>
    public enum MaintainCycleType
    {
        /// <summary>
        /// 周
        /// </summary>
        [Label("周")]
        Week = 0,

        /// <summary>
        /// 月
        /// </summary>
        [Label("月")]
        Month = 1,
    }
}
