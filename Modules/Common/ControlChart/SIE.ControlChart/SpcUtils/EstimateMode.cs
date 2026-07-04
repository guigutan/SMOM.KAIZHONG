using SIE.ObjectModel;
using System;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 标准差估值方式
    /// </summary>
    [Serializable]
    public enum EstimateMode
    {
        /// <summary>
        /// Rbar 子组极差估算法
        /// </summary>
        [Label("Rbar 子组极差均值估算法")]
        RBar,
        /// <summary>
        /// Sbar 子组标准差估算法
        /// </summary>
        [Label("Sbar 子组标准差估算法")]
        SBar,
        /// <summary>
        /// 移动极差估算法
        /// </summary>
        [Label("移动极差均值估算法")]
        IMREstimateStdDeviation,

        /// <summary>
        /// 移动极差中位数估算法
        /// </summary>
        [Label("移动极差中位数估算法")]
        IMedianEstimateStdDeviation,
        /// <summary>
        /// 合并标准差
        /// </summary>
        [Label("合并标准差估算法")]
        CombinedStdDeviation
    }
}
