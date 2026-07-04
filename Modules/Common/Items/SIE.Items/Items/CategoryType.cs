using SIE.ObjectModel;

namespace SIE.Items.Items
{
    /// <summary>
    /// 分类类型
    /// </summary>
    public enum CategoryType
    {
        /// <summary>
        /// 库存类别
        /// </summary>
        [Label("库存类别")]
        Item,

        /// <summary>
        /// 质量类别
        /// </summary>
        [Label("质量类别")]
        Quality,

        /// <summary>
        /// 齐套类别
        /// </summary>
        [Label("齐套类别")]
        Kit,
    }
}