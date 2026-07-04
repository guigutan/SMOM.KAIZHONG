using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 上架处理类型
    /// </summary>
    public enum UpProcessType
    {
        /// <summary>
        /// 混合上架
        /// </summary>
        [Label("混合上架")]
        MixedUp,

        /// <summary>
        /// 按件上架
        /// </summary>
        [Label("按件上架")]
        PieceUp,

        /// <summary>
        /// 按箱上架
        /// </summary>
        [Label("按箱上架")]
        BoxUp,

        /// <summary>
        /// 按托上架
        /// </summary>
        [Label("按托上架")]
        TrayUp,
    }
}
