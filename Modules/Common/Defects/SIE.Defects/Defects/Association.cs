using SIE.ObjectModel;

namespace SIE.Defects.Defects
{
    /// <summary>
    /// 严重度
    /// </summary>
    public enum DefectSeverity
    {
        /// <summary>
        /// 致命
        /// </summary>
        [Label("致命")]
        deadly = 0,

        /// <summary>
        /// 高
        /// </summary>
        [Label("高")]
        high = 1,

        /// <summary>
        /// 中
        /// </summary>
        [Label("中")]
        middle = 2,

        /// <summary>
        /// 轻
        /// </summary>
        [Label("轻")]
        light = 3,
    }

    /// <summary>
    /// 严重度默认值
    /// </summary>
    public static class DefectSeverityHelper
    {
        /// <summary>
        /// 最低级严重度
        /// </summary>
        public static DefectSeverity LowestDefectSeverity
        {
            get { return DefectSeverity.light; }
        }

        /// <summary>
        /// 最高级严重度
        /// </summary>
        public static DefectSeverity HighestDefectSeverity
        {
            get { return DefectSeverity.deadly; }
        }

        /// <summary>
        /// 判断a是否比b严重度高
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsHigherThan(DefectSeverity a, DefectSeverity b)
        {
            return a < b;
        }

        /// <summary>
        /// 判断a是否比b严重度低
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsLightThan(DefectSeverity a, DefectSeverity b)
        {
            return a > b;
        }
    }

}
