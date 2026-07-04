using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 归还类型
    /// </summary>
    public enum ReturnType
	{
		/// <summary>
		/// 实物归还
		/// </summary>
		[Label("实物归还")]
		Real = 10,
		/// <summary>
		/// 无实物归还
		/// </summary>
		[Label("无实物归还")]
		None = 20,
	}
}