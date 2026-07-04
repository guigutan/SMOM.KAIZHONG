using SIE.ObjectModel;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 特殊事务标记
    /// </summary>
    public enum SpecialTransMark
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal,

        /// <summary>
        /// 超发
        /// </summary>
        [Label("超发")]
        OverStock,

        /// <summary>
        /// 合并工单
        /// </summary>
        [Label("合并工单")]
        MergeWo,

        /// <summary>
        /// 超退
        /// </summary>
        [Label("超退")]
        OverReturn,

        /// <summary>
        /// 合并超发
        /// </summary>
        [Label("合并超发")]
        MergeOverStock,

        /// <summary>
        /// 合并超退
        /// </summary>
        [Label("合并超退")]
        MergeOverReturn,

        /// <summary>
        /// 合并工单退料
        /// </summary>
        [Label("合并工单退料")]
        MergeWoReturn
    }
}
