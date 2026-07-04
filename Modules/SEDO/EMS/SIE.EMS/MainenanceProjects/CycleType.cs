using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 周期类型
    /// </summary>
    public enum CycleType
    {
        #region EDO_周期类型枚举增加

        /// <summary>
        /// 班
        /// </summary>
        [Category("Check")]
        [Label("班")]
        Class = 0,

        /// <summary>
        /// 日
        /// </summary>
        [Category("Check")]
        [Label("日")]
        Day = 1,

        /// <summary>
        /// 周
        /// </summary>
       [Category("Maintain")]
        [Label("周")]
        Week = 2,

        /// <summary>
        /// 双周
        /// </summary>
        [Category("Maintain")]
        [Label("双周")]
        DoubleWeek = 3,

        /// <summary>
        /// 月
        /// </summary>
        [Category("Maintain")]
        [Label("月")]
        Month = 4,

        /// <summary>
        /// 双月
        /// </summary>
        [Category("Maintain")]
        [Label("双月")]
        DoubleMonth = 5,

        /// <summary>
        /// 季
        /// </summary>
        [Category("Maintain")]
        [Label("季")]
        Season = 6,

        /// <summary>
        /// 半年
        /// </summary>
        [Category("Maintain")]
        [Label("半年")]
        HalfYear = 7,

        /// <summary>
        /// 年
        /// </summary>
        [Category("Maintain")]
        [Label("年")]
        Year = 8,
        #endregion

    }
}
