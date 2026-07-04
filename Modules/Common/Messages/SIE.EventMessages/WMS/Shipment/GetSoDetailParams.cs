using SIE.ObjectModel;
using System;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 获取发运单明细参数
    /// </summary>
    [Serializable]
    public class GetSoDetailParams
    {
        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateRange ShippingDate { get; set; }

        /// <summary>
        /// 发运单明细Id集合,英文逗号隔开
        /// </summary>
        public string SoDtlIds { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }
    }
}
