using System;

namespace SIE.EventMessages.WMS.TraceableItem
{
    /// <summary>
    /// ReelID信息
    /// </summary>
    [Serializable]
    public class TraceableInfo
    {        
        /// <summary>
        /// Asn单号+Asn明细行号（唯一值）
        /// </summary>
        public string WmsKey { get; set; }

        /// <summary>
        /// SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemBatch { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 生产时间
        /// </summary>
        public DateTime? ProductDate { get; set; }

        
       
    }
}
