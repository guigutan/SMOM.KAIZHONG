using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 绩效运算符
    /// </summary>
    public enum KpiOperators
	{
		/// <summary>
		/// =
		/// </summary>
		[Label("=")]
		Equal = 0,
		/// <summary>
		/// >
		/// </summary>
		[Label(">")]
		Greater = 1,
		/// <summary>
		/// 小于
		/// </summary>
		[Label("<")]
		Less = 2,
		/// <summary>
		/// ≥
		/// </summary>
		[Label("≥")]
		GreaterOrEqual = 3,
		/// <summary>
		/// ≤
		/// </summary>
		[Label("≤")]
		LessOrEqual = 4,
	}
}