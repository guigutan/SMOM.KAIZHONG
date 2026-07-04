using SIE.ObjectModel;

namespace SIE.Core.Labels
{
    /// <summary>
    /// 标签状态
    /// </summary>
    public enum LabelState
    {
        /// <summary>
        /// 未关联
        /// </summary>
        [Label("未关联")]
        NotRelated,

        /// <summary>
        /// 已关联
        /// </summary>
        [Label("已关联")]
        Related,
    }
}