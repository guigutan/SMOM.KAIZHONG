using SIE.ObjectModel;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 分类
    /// </summary>
    public enum SortOut
    {
        /// <summary>
        /// PO
        /// </summary>
        [Label("PO")]
        PO,

        /// <summary>
        /// INV
        /// </summary>
        [Label("INV")]
        INV,

        /// <summary>
        /// MOVE
        /// </summary>
        [Label("MOVE")]
        MOVE,

        /// <summary>
        /// WIP
        /// </summary>
        [Label("WIP")]
        WIP,
    }
}