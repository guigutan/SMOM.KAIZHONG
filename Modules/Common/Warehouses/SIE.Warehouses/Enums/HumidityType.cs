using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 储存湿度类型
    /// </summary>
    public enum HumidityType
    {
        /// <summary>
        /// 常湿
        /// </summary>
        [Label("常湿")]
        Normal,

        /// <summary>
        /// 低湿
        /// </summary>
        [Label("低湿")]
        Low,

        /// <summary>
        /// 干燥
        /// </summary>
        [Label("干燥")]
        Dry,

        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自定义")]
        Custom,
    }
}
