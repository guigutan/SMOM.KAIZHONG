using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 设备类型信息
    /// </summary>
    [Serializable]
    public class ItemLabelRemainQtyData
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }

        /// <summary>
        /// 标签状态
        /// </summary>
        public LabelStatus LabelStatus { get; set; }
    }
}