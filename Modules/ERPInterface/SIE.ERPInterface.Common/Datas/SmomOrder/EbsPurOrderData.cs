using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.SmomOrder
{
    /// <summary>
    /// 发货计划分析数据
    /// </summary>
    [Serializable]
    public class EbsPurOrderData : EbsOrderBaseData
    {
        /// <summary>
        /// 采购单号
        /// </summary>
        public string No { get; set; }
            
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 发货方
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// ERP采购单号
        /// </summary>
        public string PoNo { get; set; }

        /// <summary>
        /// ERP行号
        /// </summary>
        public string ERPLineNo { get; set; }

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

        /// <summary>
        /// 采购订单接收是否回传
        /// </summary>
        public int IsReturnErp { get; set; }

        /// <summary>
        /// 采购订单状态
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 取消数量
        /// </summary>
        public decimal CancelQty { get; set; }

        /// <summary>
        /// 上线前已收数
        /// </summary>
        public decimal FirstReceiveQty { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo { get; set; }

        /// <summary>
        /// 允许超发数量
        /// </summary>
        public decimal AllowMoreQty { get; set; }

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string SecondUnitCode { get; set; }

        /// <summary>
        /// 辅助数量
        /// </summary>
        public decimal SecondQuantity { get; set; }

        /// <summary>
        /// 辅助单位ID
        /// </summary>
        public double? SecondUnitId { get; set; }
    }
}
