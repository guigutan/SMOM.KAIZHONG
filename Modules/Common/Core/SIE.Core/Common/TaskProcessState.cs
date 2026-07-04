using SIE.ObjectModel;

namespace SIE.Core.Common
{
    /// <summary>
    /// 处理状态
    /// </summary>
    public enum TaskProcessState
	{
		/// <summary>
		/// 处理中
		/// </summary>
		[Label("处理中")]
		Doing,
		/// <summary>
		/// 待处理
		/// </summary>
		[Label("待处理")]
		ToDo,
		/// <summary>
		/// 已处理
		/// </summary>
		[Label("已处理")]
		Done,
	}
}
