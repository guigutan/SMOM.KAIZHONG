using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    ///  查询数据库时临时对象（没实际意义 需要根据上下文解析）
    /// </summary>
    [Serializable]
    public class SaleOrderDetailObject
    {
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public double SaleOrderId { get; set; }
        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        public double SaleOrderDetailId { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public String LineNo { get; set; }

        /// <summary>
        /// 承诺交期
        /// </summary>
        public DateTime? PromiseDelivery { get; set; }
    }

}
