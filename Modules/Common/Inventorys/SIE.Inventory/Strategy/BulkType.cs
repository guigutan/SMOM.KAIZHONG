using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 散件优先配置类型
    /// </summary>
    public enum BulkType
    {
        /// <summary>
        /// 强制
        /// </summary>
        [Label("强制")]
        Force = 10,

        /// <summary>
        /// 推荐
        /// </summary>
        [Label("推荐")]
        Recommend = 20,
    }
}
