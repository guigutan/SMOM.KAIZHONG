using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单物料属性
    /// </summary>
    [Serializable]
    public class WoItemPropertyData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单物料属性
        /// </summary>
        public List<ItemPropertyData> ItemPropertyDatas { get; set; } = new List<ItemPropertyData>();
    }
}
