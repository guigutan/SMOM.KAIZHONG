using SIE.ObjectModel;

namespace SIE.LES
{
    /// <summary>
    /// 推式触发方式
    /// </summary>
    public enum PushTriggerMode
    {
        /// <summary>
        /// 计划开始前
        /// </summary>
        [Label("计划开始前")]
		XHoursBefore,

        /// <summary>
        /// 警戒水位
        /// </summary>
        [Label("警戒水位")]
        InvBelowSafeLevelToBeat,

        /// <summary>
        /// 手工
        /// </summary>
        [Label("手工")]
        ManualModel,
    }
}