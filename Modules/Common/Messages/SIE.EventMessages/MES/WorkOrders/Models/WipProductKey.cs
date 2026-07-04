using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 单体关键件清单
    /// </summary>
    [Serializable]
    public class WipProductKeyItem
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
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 用料量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }
    }
}
