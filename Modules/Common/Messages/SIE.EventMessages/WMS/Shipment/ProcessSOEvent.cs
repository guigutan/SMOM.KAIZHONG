using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 将WMS发货数据回传到MES
    /// </summary>
    public class ProcessSOToWorkFeedEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessSOToWorkFeedEvent()
        {
            ProcessSOEventList = new List<ProcessSOEvent>();
        }

        /// <summary>
        /// WMS发货数据
        /// </summary>
        public List<ProcessSOEvent> ProcessSOEventList { get; set; }
    }

    /// <summary>
    /// WMS收货单处理信息
    /// </summary>
    public class ProcessSOEvent
    {
        /// <summary>
        /// 请求号
        /// (每次传输不能重复)
        /// </summary>
        public String RequireNo { get; set; }

        /// <summary>
        /// 订单类型
        /// 0：采购入库，10：成品入库，20:半成品入库,30:生产退料,40:销售退货,50:VMI入库,60:其他入库,70:销售出库,80:工单发料,90:其他出库,100：供应商退货，110：库存移动，120：库存调拨
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 处理结果
        /// 0:取消，10：发货成功, 20:分配
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// 明细信息
        /// </summary>
        public List<ProcessSODTLEvent> DetailList { get; set; }
    }

    /// <summary>
    /// WMS回传发货单明细信息
    /// </summary>
    public class ProcessSODTLEvent
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 发货数
        /// </summary>
        public decimal ShippingQty { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime ShippingDTLDate { get; set; }

        /// <summary>
        /// 序列号数据
        /// </summary>
        public List<string> SnList { get; set; } = new List<string>();
    }
}
