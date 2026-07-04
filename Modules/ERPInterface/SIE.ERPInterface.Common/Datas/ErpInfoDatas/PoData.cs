using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 采购数据
    /// </summary>
    public class PoData : ErpInfoData
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 明细数据
        /// </summary>
        public List<PoDetailData> DetailList { get; set; }
    }

    /// <summary>
    /// 采购明细数据
    /// </summary>
    public class PoDetailData : ErpInfoData
    {
        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 采购单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// 采购单位
        /// </summary>
        public string UnitCode { get; set; }
    }
}
