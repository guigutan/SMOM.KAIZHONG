using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// KPI目标值格式
    /// </summary>
    public enum KPIValueType
    {
        /// <summary>
        /// 百分比
        /// </summary>
        [Label("百分比")]
        Percent = 0,

        /// <summary>
        /// 数值
        /// </summary>
        [Label("数值")]
        Number = 1
    }
}
