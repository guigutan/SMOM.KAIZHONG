using SIE.ObjectModel;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 批次属性数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        Date,

        /// <summary>
        /// 文本
        /// </summary>
        [Label("文本")]
        Text,

        /// <summary>
        /// 数值
        /// </summary>
        [Label("数值")]
        Numerical,

        /// <summary>
        /// 布尔
        /// </summary>
        [Label("布尔")]
        Boole,
    }
}
