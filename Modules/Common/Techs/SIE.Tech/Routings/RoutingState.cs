using SIE.ObjectModel;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺流程状态
    /// </summary>
    public enum RoutingState
    {
        /// <summary>
        /// 保存
        /// </summary>
        [Label("保存")]
        Save,

        /// <summary>
        /// 发布
        /// </summary>
        [Label("发布")]
        Release,

        /// <summary>
        /// 未保存
        /// </summary>
        [Label("未保存")]
        UnSave,
    }
}