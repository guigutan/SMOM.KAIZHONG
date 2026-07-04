using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 库位库存
    /// </summary>
    public enum LocationState
    {
        /// <summary>
        /// 空库位
        /// </summary>
        [Label("空库位")]
        EmptyLoc = 10,

        /// <summary>
        /// 非空库位
        /// </summary>
        [Label("非空库位")]
        NotEmptyLoc = 20,
    }
}