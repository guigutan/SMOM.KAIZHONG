using SIE.ObjectModel;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 工序状态
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// 未过站
        /// </summary>
        [Label("未过站")]
        Not = 0,

        /// <summary>
        /// 已过站
        /// </summary>
        [Label("已过站")]
        Has = 1,

        /// <summary>
        /// 当前工序
        /// </summary>
        [Label("当前工序")]
        Current = 2,
    }
}
