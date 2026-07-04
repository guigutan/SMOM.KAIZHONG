using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// ASN单数据
    /// </summary>
    public class AsnData : ErpInfoData
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
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

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
        /// 客户编码 订单类型为销售退货,客户编码必填
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 部门编码 订单类型为成品入库,半成品入库,生产退料时,部门编码必填
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 供应商编码 订单类型为采购入库,供应商编码必填
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 货主编码 订单类型为VMI入库,货主编码必填
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        //public AsnState AsnState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 明细数据列表
        /// </summary>
        public List<AsnDetailData> DetailList { get; set; }
    }

    /// <summary>
    /// 明细数据
    /// </summary>
    public class AsnDetailData : ErpInfoData
    {
        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }
      
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 预期数量
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
        /// 采购订单单号 订单类型为采购入库,采购订单单号必填
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// 采购订单行号 订单类型为采购入库,采购订单行号必填
        /// </summary>
        public string PoDetailLineNo { get; set; }

        /// <summary>
        /// 收货库位编码
        /// </summary>
        public string ReceiveStorageLocationCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        //public AsnState AsnState { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? LotAtt01 { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? LotAtt02 { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string LotAtt04 { get; set; }

        /// <summary>
        /// 批次属性05
        /// </summary>
        public decimal? LotAtt05 { get; set; }

        /// <summary>
        /// 批次属性06
        /// </summary>
        public decimal? LotAtt06 { get; set; }

        /// <summary>
        /// 批次属性07
        /// </summary>
        public bool? LotAtt07 { get; set; }

        /// <summary>
        /// 批次属性08
        /// </summary>
        public string LotAtt08 { get; set; }

        /// <summary>
        /// 批次属性09
        /// </summary>
        public string LotAtt09 { get; set; }

        /// <summary>
        /// 批次属性10
        /// </summary>
        public string LotAtt10 { get; set; }

        /// <summary>
        /// 批次属性11
        /// </summary>
        public DateTime? LotAtt11 { get; set; }

        /// <summary>
        /// 批次属性11
        /// </summary>
        public DateTime? LotAtt12 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}