using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配规则子规则关系
    /// </summary>
    public enum RelationType
    {
        /// <summary>
        /// 相互独立
        /// </summary>
        [Label("相互独立")]
        Independence = 0,

        /// <summary>
        /// 相互配合 
        /// </summary>
        [Label("相互配合 ")]
        Cooperation = 1,
    }
}
