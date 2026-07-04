using SIE.ObjectModel;

namespace SIE.ProductIntfc.InspSettings
{
    /// <summary>
    /// 工序类型
    /// </summary>
    public enum InspProcess
    {
        /// <summary>
        /// 首个工序
        /// </summary>
        [Label("首个工序")]
        First = 0,

        /// <summary>
        /// 最后工序
        /// </summary>
        [Label("最后工序")]
        Last = 1,

        /// <summary>
        /// 自定义工序
        /// </summary>
        [Label("自定义工序")]
        Custom = 2
    }
}