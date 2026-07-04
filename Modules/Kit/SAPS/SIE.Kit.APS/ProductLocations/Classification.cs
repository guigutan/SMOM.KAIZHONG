using SIE.ObjectModel;

namespace SIE.Kit.APS.ProductLocations
{
    /// <summary>
    /// 类型分组
    /// </summary>
    public enum Classification
	{
		/// <summary>
		/// 行业类型
		/// </summary>
		[Label("行业类型")]
		Industry = 0,
		/// <summary>
		/// 产品类型
		/// </summary>
		[Label("产品类型")]
		Product = 1,
	}
}