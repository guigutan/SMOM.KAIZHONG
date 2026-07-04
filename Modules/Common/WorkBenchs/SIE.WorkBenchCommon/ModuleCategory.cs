using SIE.ObjectModel;

namespace SIE.WorkBenchCommon
{
    /// <summary>
    /// 产品模块分类
    /// </summary>
    public enum ModuleCategory
	{
		/// <summary>
		/// 品质类
		/// </summary>
		[Label("品质")]
		QMS = 0,

		/// <summary>
		/// 生产执行类
		/// </summary>
		[Label("生产执行")]
		MES = 1,

		/// <summary>
		/// 计划类
		/// </summary>
		[Label("计划")]
		APS = 2,

		/// <summary>
		/// 仓库类
		/// </summary>
		[Label("仓库")]
		WMS = 3,
	}
}