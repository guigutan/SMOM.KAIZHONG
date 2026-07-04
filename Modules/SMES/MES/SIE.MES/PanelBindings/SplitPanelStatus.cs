using SIE.ObjectModel;

namespace SIE.MES.PanelBindings
{
    /// <summary>
    /// 分板状态
    /// </summary>
    public enum SplitPanelStatus
    {
        /// <summary>
        /// 未分板
        /// </summary>
        [Label("未分板")]
        NotSplit = 0,

        /// <summary>
        /// 已分板
        /// </summary>
        [Label("已分板")]
        AlreadySplit = 1,
    }
}
