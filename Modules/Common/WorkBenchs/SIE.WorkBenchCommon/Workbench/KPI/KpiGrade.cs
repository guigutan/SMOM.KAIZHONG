using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 
    /// </summary>
    public enum KpiGrade
	{
		/// <summary>
		/// 优秀
		/// </summary>
		[Label("优秀")]
		Great = 0,

		/// <summary>
		/// 好
		/// </summary>
		[Label("好")]
		Good = 1,

		/// <summary>
		/// 差
		/// </summary>
		[Label("差")]
		Poor = 2,
	}
}