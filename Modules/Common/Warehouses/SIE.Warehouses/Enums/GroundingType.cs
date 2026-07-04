using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 优先上架库位
    /// </summary>
    public enum GroundingType
    {
        /// <summary>
        /// 入库优先
        /// </summary>
        [Label("入库口优先")]
        InFirst,

        /// <summary>
        /// 出库优先
        /// </summary>
        [Label("出库口优先")]
        OutFirst,
    }
}
