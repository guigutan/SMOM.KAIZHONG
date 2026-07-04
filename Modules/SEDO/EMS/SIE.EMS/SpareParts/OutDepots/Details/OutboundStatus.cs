using SIE.ObjectModel;

namespace SIE.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum OutboundStatus
	{
		/// <summary>
		/// 拣货
		/// </summary>
		[Label("已拣货")]
		Picked = 10,
		/// <summary>
		/// 发货
		/// </summary>
		[Label("已发货")]
		Shipped = 20,
		/// <summary>
		/// 退货
		/// </summary>
		[Label("退货")]
		Returned = 30,
	}
}