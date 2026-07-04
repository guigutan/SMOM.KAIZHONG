using SIE.ObjectModel;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 分析结果
    /// </summary>
    public enum AnalysisResult
    {
        /// <summary>
        /// 库存差异
        /// </summary>
        [Label("库存差异")]
        Diff = 0,

        /// <summary>
        /// 库存差异
        /// </summary>
        [Label("产线占用")]
        Line = 10,

        /// <summary>
        /// 库存差异
        /// </summary>
        [Label("其他")]
        Other = 20,
    }
}
