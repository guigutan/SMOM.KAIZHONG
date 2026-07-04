using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 占位符类型
    /// </summary>
    public enum PlaceholderType
    {
        /// <summary>
        /// 数值
        /// </summary>
        [Label("数值")]
        Number = 5,
        /// <summary>
        /// 字符
        /// </summary>
        [Label("字符")]
        String = 10,
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        DateTime = 15,
    }
}