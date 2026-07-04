using SIE.ObjectModel;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 活动类型
	/// </summary>
	public enum ActivityType
    {
        /// <summary>
        /// 初态
        /// </summary>
        [Label("初态")]
        Initial,

        /// <summary>
        /// 交互
        /// </summary>
        [Label("交互")]
        Interaction,

        /// <summary>
        /// 终态
        /// </summary>
        [Label("终态")]
        Completion,
    }
}
