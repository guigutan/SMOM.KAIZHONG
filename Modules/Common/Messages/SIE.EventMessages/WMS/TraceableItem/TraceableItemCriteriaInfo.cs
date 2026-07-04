using System;

namespace SIE.EventMessages.WMS.TraceableItem
{
	/// <summary>
	/// 发运订单参数
	/// </summary>
	[Serializable]
    public class TraceableItemCriteriaInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemBatch { get; set; }

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
        public DateTime? CreateTimeStart { get; set; }

        /// <summary>
        /// 收货结束时间
        /// </summary>
        public DateTime? CreateTimeEnd { get; set; }
    }
}
