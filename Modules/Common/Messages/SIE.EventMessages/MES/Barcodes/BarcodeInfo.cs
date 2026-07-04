using SIE.Core.Barcodes;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.Barcodes
{
    /// <summary>
    /// 条码信息
    /// </summary>
    [Serializable]
    public class BarcodeInfo
    {
        /// <summary>
        /// 条码ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn { get; set; }
    }

    /// <summary>
    /// 条码信息
    /// </summary>
    [Serializable]
    public class BarCodeInfoWithQty : BarcodeInfo
    {
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 打印的条码信息
    /// </summary>
    [Serializable]
    public class PrintBarcodeInfo
    {
        /// <summary>
        /// 消息类型（3、打印和导入；4、条码关联）
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public List<Barcode> BarcodeList { get; set; } = new List<Barcode>();
    }
}