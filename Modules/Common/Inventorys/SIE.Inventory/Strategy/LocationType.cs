using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 库位形式
    /// </summary>
    public enum LocationType
    {
        /// <summary>
        /// 地面
        /// </summary>
        [Label("地面")]
        Ground = 10,

        /// <summary>
        /// 货架
        /// </summary>
        [Label("货架")]
        Shelves = 20,

        /// <summary>
        /// 冰柜
        /// </summary>
        [Label("冰柜")]
        Freezer = 30,
    }
}