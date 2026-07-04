using System;

namespace SIE.EventMessages.MES.Inspection.Models
{
    /// <summary>
    /// 报检条码信息
    /// </summary>
    [Serializable]
    public class InspBarcodeInfo
    {
        /// <summary>
        /// 报检条码ID
        /// </summary>
        public double InspBarcodeId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectData { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string Batch { get; set; }
    }
}