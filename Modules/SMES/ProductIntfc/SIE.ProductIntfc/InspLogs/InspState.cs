using SIE.ObjectModel;

namespace SIE.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检状态
    /// </summary>
    public enum InspState
    {
        /// <summary>
        /// 未报检
        /// </summary>
        [Label("未报检")]
        UnInspection,

        /// <summary>
        /// 已报检
        /// </summary>
        [Label("已报检")]
        Inspection,
    }
}