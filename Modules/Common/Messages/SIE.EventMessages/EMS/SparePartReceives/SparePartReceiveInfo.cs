using System;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.SparePartReceives
{
    /// <summary>
    /// 备件入库接收单更新信息
    /// </summary>
    [Serializable]
    public class SparePartReceiveInfo
    {
        /// <summary>
        /// 接收单号
        /// </summary>
        public string ReceiveNo { get; set; }

        /// <summary>
        /// 验收单号
        /// </summary>
        public string AcceptanceNo { get; set; }

        /// <summary>
        /// 备件入库接收明细信息
        /// </summary>
        public List<SparePartReceiveDtlInfo> SparePartReceiveDtlInfoList { get; set; }
    }

    /// <summary>
    /// 备件入库接收明细信息
    /// </summary>
    [Serializable]
    public class SparePartReceiveDtlInfo
    {
        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public int InboundQty { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo { get; set; }

        /// <summary>
        /// 采购单行号
        /// </summary>
        public string PurchaseOrderLineNo { get; set; }
    }
}
