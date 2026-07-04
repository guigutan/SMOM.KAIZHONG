using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.LES.Commons
{
    /// <summary>
    /// 触发方式
    /// </summary>
    public enum TriggerMode
    {
        /// <summary>
        /// 安全水位
        /// </summary>
        [Label("安全水位")]
        BelowSafe = 0,

        /// <summary>
        /// 计划开始前
        /// </summary>
        [Label("计划开始前")]
        [Category("Push")]
        XHoursBefore = 1,

        /// <summary>
        /// 警戒水位
        /// </summary>
        [Label("警戒水位")]
        [Category("Push")]
        InvBelowSafeLevelToBeat = 2,

        /// <summary>
        /// 手工
        /// </summary>
        [Label("手工")]
        ManualModel = 3
    }
}
