using SIE.ObjectModel;

namespace SIE.Items
{
    /// <summary>
    /// 标签类型
    /// </summary>
    public enum ItemLabelType
    {
        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("物料标签")]
        ItemLabel,

        /// <summary>
        /// 物料批次
        /// </summary>
        [Label("物料批次")]
        ItemBatch,

        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        ItemCode,
    }
}