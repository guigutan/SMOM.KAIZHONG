using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 检验状态
    /// </summary>
    public enum InspectionStatus
	{
		/// <summary>
		/// 待检验
		/// </summary>
		[Label("待检验")]
		Pending = 10,
		/// <summary>
		/// 检验中
		/// </summary>
		[Label("检验中")]
		Under = 20,
		/// <summary>
		/// 已校验
		/// </summary>
		[Label("已校验")]
		Calirated = 30,
		/// <summary>
		/// 关闭
		/// </summary>
		[Label("关闭")]
		Closed = 40,
	}
}