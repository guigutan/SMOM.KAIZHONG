using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.Barcodes
{
    /// <summary>
    /// 条码生成后事件
    /// </summary>
    [Serializable]
    public class BarcodeGeneratedEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workOrderId">MES工单ID</param>
        /// <param name="barcodeInfos">条码集合</param>
        public BarcodeGeneratedEvent(double workOrderId, List<BarcodeInfo> barcodeInfos)
        {
            WorkOrderId = workOrderId;
            BarcodeInfos.AddRange(barcodeInfos);
        }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<BarcodeInfo> BarcodeInfos { get; } = new List<BarcodeInfo>();
    }
}