using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料model
    /// </summary>
    [Serializable]
    public class ItemInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel { get; set; }

        /// <summary>
        /// 物料消耗方式
        /// </summary>
        public ConsumeMode ConsumeMode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        public bool IsEnableItemExtProp { get; set; }
    }
}
