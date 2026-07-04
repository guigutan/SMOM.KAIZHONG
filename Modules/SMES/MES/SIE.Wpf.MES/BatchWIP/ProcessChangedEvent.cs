using SIE.Core.Barcodes;
using SIE.Tech.Processs;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// 工序变更事件
    /// </summary>
    public class ProcessChangedEvent
    {
        /// <summary>
        /// 出站条码类型
        /// </summary>
        public BarcodeType? type { get; set; }
    }
}