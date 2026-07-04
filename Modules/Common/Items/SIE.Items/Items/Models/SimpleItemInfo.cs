using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料model
    /// </summary>
    [Serializable]
    public class SimpleItemInfo
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
        /// 制程工艺类型Id
        /// </summary>
        public double? ProcessTechTypeId { get; set; }

        /// <summary>
        /// 产品机型ID
        /// </summary>
        public double? ModelId { get; set; }
    }
}