using SIE.ObjectModel;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 副资产调拨状态
    /// </summary>
    public enum TransferStatus
	{
		/// <summary>
		/// 未调拨
		/// </summary>
		[Label("未调拨")]
		NotYet = 10,
		/// <summary>
		/// 已调拨
		/// </summary>
		[Label("已调拨")]
		Done = 20,
		/// <summary>
		/// 部分调拨
		/// </summary>
		[Label("部分调拨")]
		Partial = 30,
		/// <summary>
		/// 关闭
		/// </summary>
		[Label("关闭")]
		Close = 40,
	}
}