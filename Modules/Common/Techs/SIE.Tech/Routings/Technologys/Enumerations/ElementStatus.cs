using SIE.ObjectModel;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 元素状态
	/// </summary>
	public enum ElementState
    {
        /// <summary>
        /// 新增
        /// </summary>
        [Label("新增")]
        New,

        /// <summary>
        /// 修改
        /// </summary>
        [Label("修改")]
        Modified,

        /// <summary>
        /// 已删除
        /// </summary>
        [Label("已删除")]
        Deleted,

        /// <summary>
        /// 未改动
        /// </summary>
        [Label("未改动")]
        Unchanged,
    }
}
