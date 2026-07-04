using SIE.ObjectModel;

namespace SIE.ProductIntfc.InspSettings
{
    /// <summary>
    /// 报检维度
    /// </summary>
    public enum InspDimension
    {
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        BatchQty = 0,

        /// <summary>
        /// 时间(分钟)
        /// </summary>
        [Label("时间(分钟)")]
        Time = 1
    }
}