using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 发运单数据
    /// </summary>
    public class ShippingOrderData : ErpInfoData
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// ERPId
        /// </summary>
        public double? ErpId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string ShippingWareHouseCode { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 交接人
        /// </summary>
        public string Connecter { get; set; }

        /// <summary>
        /// 收货地址 当订单类型为销售出库,供应商退货时,收货地址必填
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 客户编码 订单类型为销售出库,客户编码必填
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 部门编码 订单类型为工单发料时,部门编码必填
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 供应商编码 订单类型为供应商退货,供应商编码必填
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 运输公司
        /// </summary>
        public string TransportCompany { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string TransportNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int OrderState { get; set; }

        /// <summary>
        /// 优先级 0-普通;1-紧急;默认普通
        /// </summary>
        public int PriorityType { get; set; }

        /// <summary>
        /// 取消原因 当状态为取消时,该栏位必填
        /// </summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 明细数据列表
        /// </summary>
        public List<ShippingOrderDetailData> DetailList { get; set; }
    }

    /// <summary>
    /// 明细数据
    /// </summary>
    public class ShippingOrderDetailData : ErpInfoData
    {
        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingOrderNo { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 订货数量
        /// </summary>
        public decimal ExpectQty { get; set; }

        /// <summary>
        /// 记录下载的单据ID
        /// </summary>
        public double? ErpId { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关发票
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 采购订单单号 订单类型为供应商退货,采购订单单号必填
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// 采购订单行号 订单类型为供应商退货,采购订单行号必填
        /// </summary>
        public string PoDetailLineNo { get; set; }

        /// <summary>
        /// 指定库位编码
        /// </summary>
        public string AppointStorageLocationCode { get; set; }

        /// <summary>
        /// 指定LPN
        /// </summary>
        public string AppointLpn { get; set; }

        /// <summary>
        /// 指定批次
        /// </summary>
        public string AppointLotCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int OrderState { get; set; }

        /// <summary>
        /// 体积(CM³)
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// 净重(G)
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 金额(CNY)
        /// </summary>
        public decimal? Amount { get; set; }
    }
}