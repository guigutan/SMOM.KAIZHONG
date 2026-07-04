using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单工序bom信息
    /// </summary>
    [Serializable]
    public class WoProcessBomInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoOrderId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPro { get; set; }

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty { get; set; }
    }
}
