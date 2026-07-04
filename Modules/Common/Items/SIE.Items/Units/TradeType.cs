using SIE.ObjectModel;

namespace SIE.Items
{
    /// <summary>
    /// 单位来源
    /// </summary>
    public enum TradeType
	{
		/// <summary>
		/// 四舍五入
		/// </summary>
		[Label("四舍五入")]
		HalfAdjust,

		/// <summary>
		/// 舍位
		/// </summary>
		[Label("舍位")]
		Rounding,

		/// <summary>
		/// 进位
		/// </summary>
		[Label("进位")]
		Carry,
	}
}