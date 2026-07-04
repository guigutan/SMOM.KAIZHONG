using System;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// 库存物料信息-查询实体
    /// </summary>
    [Serializable]
    public class WmsItemCriteria
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot { get; set; }

        /// <summary>
        /// 生产周期开始时间
        /// </summary>
        public DateTime? ProductDateStart { get; set; }

        /// <summary>
        /// 生产周期结束时间
        /// </summary>
        public DateTime? ProductDateEnd { get; set; }

        /// <summary>
        /// 收货开始时间
        /// </summary>
        public DateTime? ReceiptDateStart { get; set; }

        /// <summary>
        /// 收货结束时间
        /// </summary>
        public DateTime? ReceiptDateEnd { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
