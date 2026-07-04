using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public enum TaskNotifications
	{
		/// <summary>
		/// 系统
		/// </summary>
		[Label("系统")]
		System = 0,

		/// <summary>
		/// 邮件
		/// </summary>
		[Label("邮件")]
		EMail = 1,

		/// <summary>
		/// 微信
		/// </summary>
		[Label("微信")]
		WeChat = 2,

		/// <summary>
		/// 短信
		/// </summary>
		[Label("短信")]
		SMS = 3,
	}
}