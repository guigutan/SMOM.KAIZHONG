using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Kit.EventMessages.UrgentOrder
{
    /// <summary>
    /// 物料加急单发送事件
    /// </summary>
    [Serializable]
    public class ItemUrgentOrderSendEvent
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemUrgentOrderSendEvent()
        {
            ItemUrgentOrderDataList = new List<ItemUrgentOrderData>();
        }

        /// <summary>
        /// 物料加急单数据
        /// </summary>
        public List<ItemUrgentOrderData> ItemUrgentOrderDataList { get; set; }
    }

    /// <summary>
    /// 物料加急单数据
    /// </summary>
    [Serializable]
    public class ItemUrgentOrderData
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public UrgentOrderType Type { get; set; }
    }

    /// <summary>
    /// 加急单类型
    /// </summary>
    [Serializable]
    public enum UrgentOrderType
    {
        /// <summary>
        /// 收料
        /// </summary>
        [Label("收料")]
        IsReceive = 0,
        /// <summary>
        /// IQC检验
        /// </summary>
        [Label("IQC检验")]
        IsInspectIqc = 1,
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        IsInstorage = 2,
        /// <summary>
        /// 备料
        /// </summary>
        [Label("备料")]
        IsStockUp = 3
    }
}
