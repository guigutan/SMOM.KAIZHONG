using System;

namespace SIE.Core.WorkOrders.Models
{
    /// <summary>
    /// 工单BOM信息
    /// </summary>
    [Serializable]
    public class WorkOrderBomInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 需求数
        /// </summary>
        public decimal RequireQty { get; set; }
    }
}