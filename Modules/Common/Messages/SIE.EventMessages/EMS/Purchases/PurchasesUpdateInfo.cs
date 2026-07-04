using System;

namespace SIE.EventMessages.EMS.Purchases
{
    /// <summary>
    /// 采购单更新信息
    /// </summary>
    [Serializable]
    public class PurchasesUpdateInfo
    {
        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// 采购单行号
        /// </summary>
        public int PoNoLineNo { get; set; }

        /// <summary>
        /// 接收单号
        /// </summary>
        public string RecNo { get; set; }

        /// <summary>
        /// 验收单号
        /// </summary>
        public string AccNo { get; set; }


        /// <summary>
        /// 入库数量
        /// </summary>
        public int InboundQty { get; set; }

    }
}
