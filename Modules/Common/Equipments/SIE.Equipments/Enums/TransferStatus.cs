using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 调拨状态
    /// </summary>
    public enum TransferStatus
	{
		/// <summary>
		/// 未发货
		/// </summary>
		[Label("未发货")]
		NotShipped = 10,
		/// <summary>
		/// 待收货
		/// </summary>
		[Label("待收货")]
		Pending = 20,

		/// <summary>
		/// 已接收
		/// </summary>
		[Label("已接收")]
		Received =30
	}
}