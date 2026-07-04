using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 排序方式
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// 升序
        /// </summary>
        [Label("升序")]
        Asc = 10,

        /// <summary>
        /// 降序
        /// </summary>
        [Label("降序")]
        Desc = 20,

        /// <summary>
        /// 不限
        /// </summary>
        [Label("不限")]
        None = 30,
    }
}