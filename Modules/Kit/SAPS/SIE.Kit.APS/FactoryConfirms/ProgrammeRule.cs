using SIE.ObjectModel;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 分配规则
    /// </summary>
    public enum ProgrammeRule
    {
        /// <summary>
        /// 产品定位
        /// </summary>
        [Label("产品定位")]
        ProductLocation = 0,
        /// <summary>
        /// 目标产能
        /// </summary>
        [Label("目标产能")]
        TargetCapacity = 1,
        /// <summary>
        /// 历史最近
        /// </summary>
        [Label("历史最近")]
        HistoryLately = 2,
    }
}