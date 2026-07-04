using System;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    ///  库存追溯-发运单追溯信息
    /// </summary>
    [Serializable]
    public class ShipmentTraceInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<ShipmentTraceData> Data { get; set; }= new List<ShipmentTraceData>();
    }


    /// <summary>
    /// 库存追溯-发运单追溯信息详细信息
    /// </summary>
    [Serializable]
    public class ShipmentTraceData
    {        

        /// <summary>
        /// 发运数量
        /// </summary>
        public decimal? ShippingQty { get; set; }

        /// <summary>
        /// 发运单
        /// </summary>
        public string ShippingOrderNo { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string ReceiveByName { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveTime { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName { get; set; }

    }
}
