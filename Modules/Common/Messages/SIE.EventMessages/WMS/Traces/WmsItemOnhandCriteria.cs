using SIE.EventMessages.Common.Traces;
using System;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// 库存追溯-库存信息-查询实体
    /// </summary>
    [Serializable]
    public class WmsItemOnhandCriteria
    {
        /// <summary>
        /// 追溯类型
        /// </summary>
        public TraceType TraceType { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
