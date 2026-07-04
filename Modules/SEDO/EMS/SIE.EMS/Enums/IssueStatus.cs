using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 发放状态
    /// </summary>
    public enum IssueStatus
	{
		/// <summary>
		/// 未发放
		/// </summary>
		[Label("未发放")]
		ToBe = 10,
		/// <summary>
		/// 已发放
		/// </summary>
		[Label("已发放")]
		Done = 20,
		/// <summary>
		/// 部分发放
		/// </summary>
		[Label("部分发放")]
		PartDone = 30,

	}
}