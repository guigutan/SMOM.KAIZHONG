using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.Barcodes
{
    /// <summary>
    /// 包装号信息
    /// </summary>
    [Serializable]
    public class PackingCode
    {
        /// <summary>
        /// 包装号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackUnitName { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse { get; set; }
    }

    /// <summary>
    /// 打印的包装号信息
    /// </summary>
    [Serializable]
    public class PackingBarcodeInfo
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public List<PackingCode> PackingCodeList { get; set; } = new List<PackingCode>();
    }
}
