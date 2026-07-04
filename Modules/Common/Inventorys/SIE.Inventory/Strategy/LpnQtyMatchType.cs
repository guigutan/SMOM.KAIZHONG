using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// LPN数量匹配规则
    /// </summary>
    public enum LpnQtyMatchType
    {
        /// <summary>
        /// 从低到高
        /// </summary>
        [Label("从低到高")]
        LowToHigh = 10,

        /// <summary>
        /// 精确匹配
        /// </summary>
        [Label("精确匹配")]
        Exact = 20,

        /// <summary>
        /// 超额匹配
        /// </summary>
        [Label("超额匹配")]
        Excess = 30,

        ///// <summary>
        ///// 最佳匹配
        ///// </summary>
        //[Label("最佳匹配")]
        //Best = 40,

        /// <summary>
        /// 从高到低
        /// </summary>
        [Label("从高到低")]
        HighToLow = 50,
    }
}