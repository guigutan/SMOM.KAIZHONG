using System;

namespace SIE.EventMessages.StationStorage
{
    /// <summary>
    /// 工位物料库存事件
    /// </summary>
    [Serializable]
    public class StationItemStoreEvent
    {
        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 实际库存数，正数为添加，负数为扣减
        /// </summary>
        public decimal ActStoreQty { get; set; }

        /// <summary>
        /// 预库存，正数为添加，负数为扣减
        /// </summary>
        public decimal BudgetQty { get; set; }

        /// <summary>
        /// 在途数量，正数为添加，负数为扣减
        /// </summary>
        public decimal SendingQty { get; set; }
    }
}