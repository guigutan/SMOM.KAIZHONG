using SIE.ObjectModel;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养类型
    ///</summary>
    public enum MaintainType
    {
        /// <summary>
        /// 周
        /// </summary>
        [Label("周")]
        Week = 0,

        /// <summary>
        /// 双周
        /// </summary>
        [Label("双周")]
        DbWeek = 1,

        /// <summary>
        /// 月
        /// </summary>
        [Label("月")]
        Month = 2,

        /// <summary>
        /// 双月
        /// </summary>
        [Label("双月")]
        DbMonth = 3,

        /// <summary>
        /// 季
        /// </summary>
        [Label("季")]
        Season = 4,

        /// <summary>
        /// 半年
        /// </summary>
        [Label("半年")]
        HalfYear = 5,

        /// <summary>
        /// 年
        /// </summary>
        [Label("年")]
        Year = 6
    }
}
