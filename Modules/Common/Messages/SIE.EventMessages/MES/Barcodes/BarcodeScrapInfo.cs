using System;

namespace SIE.EventMessages.MES.Barcodes
{
    /// <summary>
    /// 条码报废信息
    /// </summary>
    [Serializable]
    public class BarcodeScrapInfo
    {
        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public double? workOrderId { get; set; }
    }
}
