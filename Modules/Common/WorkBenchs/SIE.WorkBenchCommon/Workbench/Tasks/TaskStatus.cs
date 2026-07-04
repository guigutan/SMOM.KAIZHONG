using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
	{
		/// <summary>
		/// 未完成
		/// </summary>
		[Label("未完成")]
		Padding = 0,

		/// <summary>
		/// 完成
		/// </summary>
		[Label("完成")]
		Finish = 1,

		/// <summary>
		/// 关闭
		/// </summary>
		[Label("关闭")]
		Closed = 2,

		/// <summary>
		/// 取消
		/// </summary>
		[Label("取消")]
		Cancel = 3,
	}
}