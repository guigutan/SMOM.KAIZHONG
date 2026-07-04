using SIE.ObjectModel;

namespace SIE.SO.SaleOrders
{
	/// <summary>
	/// Mi状态
	/// </summary>
	public enum MiState
	{
		/// <summary>
		/// 未完成
		/// </summary>
		[Label("未完成")]
		ReviewCancel = 0,

		/// <summary>
		/// 已完成
		/// </summary>
		[Label("已完成")]
		ReviewNot = 1,
	}
}