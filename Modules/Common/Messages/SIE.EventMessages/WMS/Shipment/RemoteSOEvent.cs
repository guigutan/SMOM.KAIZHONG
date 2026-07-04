using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// MES工单到仓库请求物料发货
    /// </summary>
    public class RemoteWorkFeedSOEvent
    {
        /// <summary>
        /// MES工单发送WMS数据列表
        /// </summary>
        public List<RemoteSOEvent> RemoteSOEventList { get; set; }
    }

    /// <summary>
    /// WMS发货单主信息
    /// </summary>
    public class RemoteSOEvent
    {
        /// <summary>
        /// 请求号
        /// </summary>
        public String RequireNo { get; set; }

        /// <summary>
        /// 订单类型
        /// 0：采购入库，10：成品入库，20:半成品入库,30:生产退料,40:销售退货,50:VMI入库,60:其他入库,70:销售出库,80:工单发料,90:其他出库,100：供应商退货，110：库存移动，120：库存调拨
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 单据小类
        /// </summary>
        public string TransCode { get; set; }

        /// <summary>
        /// 优先级类型
        /// 0：普通，1：紧急
        /// </summary>
        public int PriorityType { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public double? ShippingWareHouseId { get; set; }

        /// <summary>
        /// 生产部门
        /// 必填情况：工单发料、其他出库
        /// </summary>
        public double? EnterpriseId { get; set; }

        /// <summary>
        /// 是否部分发货
        /// </summary>
        public bool IsPartShipping { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 供应商
        /// 必填情况：供应商退货
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactsNumber { get; set; }

        /// <summary>
        /// 运输公司
        /// </summary>
        public string TransportCompany { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string TransportNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string ProcessSegment { get; set; }

        /// <summary>
        /// 领料货区
        /// </summary>
        public string PickingArea { get; set; }

        /// <summary>
        /// 明细信息
        /// </summary>
        public List<RemoteSODTLEvent> DetailList { get; set; }
    }

    /// <summary>
    /// WMS发货单明细信息
    /// </summary>
    public class RemoteSODTLEvent
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkNo { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 订货数
        /// </summary>
        public decimal ExpectQty { get; set; }

        /// <summary>
        /// 指定库区
        /// </summary>
        public double? AppointStorageAreaId { get; set; }

        /// <summary>
        /// 指定库位
        /// </summary>
        public double? AppointStorageLocationId { get; set; }

        /// <summary>
        /// 指定LPN
        /// </summary>
        public string AppointLpn { get; set; }

        /// <summary>
        /// 指定批次
        /// </summary>
        public string AppointLotCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }
    }
}
