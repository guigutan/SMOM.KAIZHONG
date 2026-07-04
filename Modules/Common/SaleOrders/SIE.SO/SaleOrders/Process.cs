using SIE.ObjectModel;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 特殊工艺
    /// </summary>
    public enum Process
	{
		/// <summary>
		/// 铜厚
		/// </summary>
		[Label("铜厚")]
		CopperThick = 0,
		/// <summary>
		/// 埋铜孔
		/// </summary>
		[Label("埋铜孔")]
		BuriedCopper = 1,
		/// <summary>
		/// 板厚
		/// </summary>
		[Label("板厚")]
		PlateThick = 2,
		/// <summary>
		/// 层数
		/// </summary>
		[Label("层数")]
		NumberLayer = 3,
	}
}