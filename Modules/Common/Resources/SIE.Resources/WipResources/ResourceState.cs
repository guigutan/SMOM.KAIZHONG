using SIE.ObjectModel;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 启用状态
    /// </summary>
    public enum ResourceState
    {
        /// <summary>
        /// 已启用
        /// </summary>
        [Label("已启用")]
        Actived = 1,

        /// <summary>
        /// 未启用
        /// </summary>
        [Label("未启用")]
        Unused = 2,

        /// <summary>
        /// 停用
        /// </summary>
        [Label("停用")]
        Stop = 4,
        /// <summary>
		/// 失效
		/// </summary>
		[Label("失效")]
        Diseffect = 5,
    }
}