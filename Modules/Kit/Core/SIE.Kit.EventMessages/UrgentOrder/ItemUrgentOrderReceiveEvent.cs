using System;
using System.Collections.Generic;

namespace SIE.Kit.EventMessages.UrgentOrder
{
    /// <summary>
    /// 物料加急单发送事件
    /// </summary>
    [Serializable]
    public class ItemUrgentOrderReceiveEvent
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemUrgentOrderReceiveEvent()
        {
            ItemUrgentOrderDataList = new List<ItemUrgentOrderData>();
        }

        /// <summary>
        /// 物料加急单数据
        /// </summary>
        public List<ItemUrgentOrderData> ItemUrgentOrderDataList { get; set; }
    }
}
