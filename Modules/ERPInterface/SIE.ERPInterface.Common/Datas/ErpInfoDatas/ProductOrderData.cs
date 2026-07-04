using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 生产订单BOM
    /// </summary>
    [Serializable]
    public class ProductOrderData : ErpInfoData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 制程工艺路线编码
        /// </summary>
        public string RouteCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public int? OrderType { get; set; }

        /// <summary>
        /// 指定工厂Id
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 销售订单编号
        /// </summary>
        public string SaleNo { get; set; }
        
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery { get; set; }

        /// <summary>
        /// 工厂交期
        /// </summary>
        public DateTime PromiseDelivery { get; set; }

        /// <summary>
        /// 齐料日期
        /// </summary>
        public DateTime? RawMaterialDate { get; set; }

        /// <summary>
        /// 建议开工日期
        /// </summary>
        public DateTime? SuggestStart { get; set; }

        /// <summary>
        /// 建议结束日期
        /// </summary>
        public DateTime? SuggestEnd { get; set; }
    }
}