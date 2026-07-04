using SIE.ObjectModel;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 单据来源
    /// </summary>
    public enum BillSource
    {
        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Manual,

        /// <summary>
        /// 自动
        /// </summary>
        [Label("自动")]
        Automatic,

        /// <summary>
        /// 外部
        /// </summary>
        [Label("外部")]
        External,
    }
}