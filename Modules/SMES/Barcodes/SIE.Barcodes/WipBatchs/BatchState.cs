using SIE.ObjectModel;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次状态
    /// </summary>
    public enum BatchState
    {
        /// <summary>
        /// 已生成
        /// </summary>
        [Label("已生成")]
        Generated = 0,

        /// <summary>
        /// 已入站
        /// </summary>
        [Label("已入站")]
        In = 1,

        /// <summary>
        /// 已移除
        /// </summary>
        [Label("已移除")]
        Removed = 2,

        /// <summary>
        /// 已出站
        /// </summary>
        [Label("已出站")]
        Out = 3,
    }
}