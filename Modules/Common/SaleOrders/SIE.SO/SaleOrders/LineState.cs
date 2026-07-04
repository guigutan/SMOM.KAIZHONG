using SIE.ObjectModel;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 行状态
    /// </summary>
    public enum LineState
	{
		/// <summary>
		/// 新建
		/// </summary>
		[Label("新建")]
		NEW = 0,
		/// <summary>
		/// 确认
		/// </summary>
		[Label("确认")]
		CONFIRMED = 10,
		/// <summary>
		/// 生产
		/// </summary>
		[Label("生产")]
		PRODUCTION = 20,
		/// <summary>
		/// 完成
		/// </summary>
		[Label("完成")]
		COMPLETE = 30,
	}
}
