using SIE.ObjectModel;

namespace SIE.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 图形预警等级
    /// </summary>
    public enum ChartAlertLevel
    {
        /// <summary>
        /// 无预警
        /// </summary>
        [Label("无预警")]
        None = 0,

        /// <summary>
        /// 绿色预警
        /// </summary>
        [Label("绿色预警")]
        Green = 1,

        /// <summary>
        /// 黄色预警
        /// </summary>
        [Label("黄色预警")]
        Yellow = 2,

        /// <summary>
        /// 红色预警
        /// </summary>
        [Label("红色预警")]
        Red = 3,

        /// <summary>
        /// 没有配置预警
        /// </summary>
        [Label("没有配置预警")]
        NoConfig = 4,
    }
}
