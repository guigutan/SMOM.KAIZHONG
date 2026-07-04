using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 归还状态
    /// </summary>
    public enum ReturnStatus
	{
		/// <summary>
		/// 待归还
		/// </summary>
		[Label("待归还")]
		ToBe = 10,
		/// <summary>
		/// 部份归还
		/// </summary>
		[Label("部份归还")]
		Partial = 20,
		/// <summary>
		/// 已归还
		/// </summary>
		[Label("已归还")]
		Done = 30,
	}
}