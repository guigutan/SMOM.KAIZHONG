using SIE.ObjectModel;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 盘点结果
    /// </summary>
    public enum LesStockCountResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal = 0,

        /// <summary>
        /// 差异复核
        /// </summary>
        [Label("差异复核")]
        DiffReview = 10,

        /// <summary>
        /// 差异调账
        /// </summary>
        [Label("差异调账")]
        DiffAdjust = 20,

        /// <summary>
        /// 差异不处理
        /// </summary>
        [Label("差异不处理")]
        NotDeal = 30,
    }
}