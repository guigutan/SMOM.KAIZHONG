using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
	/// 周期类型
	/// </summary>
	public enum DateType
    {
        /// <summary>
		/// 年
		/// </summary>
		[Label("年")]
        [Category("NEW")]
        YEAR = 0,

        /// <summary>
        /// 季
        /// </summary>
        [Label("季")]
        SEASON = 1,

        /// <summary>
        /// 月
        /// </summary>
        [Label("月")]
        [Category("NEW")]
        MONTH = 2,

        /// <summary>
        /// 周
        /// </summary>
        [Label("周")]
        [Category("NEW")]
        WEEK = 3,
    }
}
