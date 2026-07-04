using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 储存温度类型
    /// </summary>
    public enum TemperatureType
    {
        /// <summary>
        /// 常温
        /// </summary>
        [Label("常温")]
        Normal,

        /// <summary>
        /// 低温
        /// </summary>
        [Label("低温")]
        Low,

        /// <summary>
        /// 冷藏
        /// </summary>
        [Label("冷藏")]
        Cold,

        /// <summary>
        /// 冷冻
        /// </summary>
        [Label("冷冻")]
        Freezing,

        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自定义")]
        Custom,
    }
}
