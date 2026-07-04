using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Defects.InspectionItems
{
    /// <summary>
    /// 比较方式
    /// </summary>
    public enum CompareType
    {
        #region 小于
        /// <summary>
        /// 小于
        /// </summary>
        [Category("Less")]
        [Label("＜")]
        LessThan = 1,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Category("Less")]
        [Label("≤")]
        LessThanOrEqual = 2,
        #endregion

        #region 大于
        /// <summary>
        /// 大于
        /// </summary>
        [Category("Greater")]
        [Label("＞")]
        GreaterThan = 3,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Category("Greater")]
        [Label("≥")]
        GreaterThanOrEqual = 4,
        #endregion
    }
}