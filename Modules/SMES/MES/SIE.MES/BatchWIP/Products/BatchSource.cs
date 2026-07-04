using SIE.ObjectModel;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次来源
    /// </summary>
    public enum BatchSource
    {
        /// <summary>
        /// 拆批
        /// </summary>
        [Label("拆批")]
        Split,

        /// <summary>
        /// 合批
        /// </summary>
        [Label("合批")]
        Merge,
    }
}