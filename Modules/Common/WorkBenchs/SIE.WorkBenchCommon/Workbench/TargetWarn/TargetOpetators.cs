using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 目标条件
    /// </summary>
    public enum TargetOpetators
	{
        /// <summary>
        /// 小于或等于
        /// </summary>
        [Label("小于或等于")]
		LessOrEqual = 0,

		/// <summary>
		/// 介于
		/// </summary>
		[Label("介于")]
		Between = 1,

        /// <summary>
        /// 大于或等于
        /// </summary>
        [Label("大于或等于")]
		GreaterOrEqual = 2,
	}
}