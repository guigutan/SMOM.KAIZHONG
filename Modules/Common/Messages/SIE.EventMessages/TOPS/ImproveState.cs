using SIE.ObjectModel;

namespace SIE.EventMessages.QMS.Models
{
    /// <summary>
    /// 改善状态
    /// </summary>
    public enum ImproveState
	{
		/// <summary>
		/// 未关闭
		/// </summary>
		[Label("未关闭")]
		UnClose = 0,
		/// <summary>
		/// 改善中
		/// </summary>
		[Label("改善中")]
		Improving = 1,
		/// <summary>
		/// 已关闭
		/// </summary>
		[Label("已关闭")]
		Closed = 2,
	}
}