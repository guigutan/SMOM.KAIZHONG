using SIE.ObjectModel;

namespace SIE.Tech.Processs.Scripts
{
    /// <summary>
    /// 比较符
    /// </summary>
    public enum Operators
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Label("=")]
        Equal = 5,

        /// <summary>
        /// 不等于
        /// </summary>
        [Label("!=")]
        Unequal = 10,

        /// <summary>
        /// 小于
        /// </summary>
        [Label("<")]
        LessThan = 15,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Label("<=")]

        LessThanEqual = 20,
        /// <summary>
        /// 大于
        /// </summary>
        [Label(">")]
        GreaterThan = 25,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Label(">=")]
        GreaterThanEqual = 30,

        /// <summary>
        /// Like
        /// </summary>
        [Label("Like")]
        Like = 35,
    }
}