using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 上料事件，更新货位库存
    /// </summary>
    [Serializable]
    public class LoadItemEvent
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal DemandQty { get; set; }

        /// <summary>
        /// 已扫数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 物料标签
        /// </summary>
        public string ItemLable { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }
    }
}
