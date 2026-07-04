using SIE.ObjectModel;

namespace SIE.Kit.MES.SingleLabels
{
    /// <summary>
    /// 标签状态
    /// </summary>
    public enum LabelState
    {
        /// <summary>
        /// 可用
        /// </summary>
        [Label("可用")]
        Normal,

        /// <summary>
        /// 已用
        /// </summary>
        [Label("已用")]
        Used,

        /// <summary>
        /// 冻结
        /// </summary>
        [Label("冻结")]
        Freeze,
    }
}