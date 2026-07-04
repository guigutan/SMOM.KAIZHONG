using SIE.ObjectModel;

namespace SIE.Barcodes
{
    /// <summary>
    /// 打印类型
    /// </summary>
    public enum BarcodeLogType
    {
        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scraped = 0,

        /// <summary>
        /// 补打
        /// </summary>
        [Label("补打")]
        Remedy = 1,

        /// <summary>
        /// 外箱条码打印
        /// </summary>
        [Label("外箱条码打印")]
        OutBox = 2,

        /// <summary>
        /// 挂起
        /// </summary>
        [Label("挂起")]
        Pending = 3,

        /// <summary>
        /// 恢复
        /// </summary>
        [Label("恢复")]
        Resume = 4,
    }
}