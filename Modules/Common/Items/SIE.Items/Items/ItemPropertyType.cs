using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料属性类型
    /// </summary>
    [Flags]
    public enum ItemPropertyType
    {
        /// <summary>
        /// 快码
        /// </summary>
        [Label("快码")]
        Catalog,

        /// <summary>
        /// 文本
        /// </summary>
        [Label("文本")]
        Text,

        /// <summary>
        /// 数字
        /// </summary>
        [Label("数字")]
        Number,
    }
}