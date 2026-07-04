using SIE.ObjectModel;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 盘点明细盘点结果
    /// </summary>
    public enum LesStockCountDetailResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal = 0,

        /// <summary>
        /// 异常
        /// </summary>
        [Label("异常")]
        Abnormal = 10,
    }
}