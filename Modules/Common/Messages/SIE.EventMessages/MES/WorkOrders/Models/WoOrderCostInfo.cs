using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单耗用单
    /// </summary>
    [Serializable]
    public class WoOrderCostInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoOrderId { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPro { get; set; }
        
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
