using SIE.ObjectModel;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 订单来源
    /// </summary>
    public enum OrderSourceType
	{
		/// <summary>
		/// 自建
		/// </summary>
		[Label("自建")]
		Manual = 0,
		/// <summary>
		/// 导入
		/// </summary>
		[Label("导入")]
		Import = 10,
		/// <summary>
		/// 接口
		/// </summary>
		[Label("接口")]
		Interface = 20,
	}
}
