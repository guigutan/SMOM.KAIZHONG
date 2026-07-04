using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 重要程序
    /// </summary>
    public enum TaskImportance
	{
		/// <summary>
		/// 一般
		/// </summary>
		[Label("一般")]
		Normal = 0,

		/// <summary>
		/// 重要
		/// </summary>
		[Label("重要")]
		Medium = 1,

		/// <summary>
		/// 非常重要
		/// </summary>
		[Label("非常重要")]
		Senior = 2,
	}
}