using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 空间限制
    /// </summary>
    public enum SpaceLimit
    {
        /// <summary>
        /// 重量
        /// </summary>
        [Label("重量")]
        Weigth = 10,

        /// <summary>
        /// 体积
        /// </summary>
        [Label("体积")]
        Volume = 20,

        /// <summary>
        /// 箱数
        /// </summary>
        [Label("箱数")]
        BoxCount = 30,

        /// <summary>
        /// 托数
        /// </summary>
        [Label("托数")]
        TrayCount = 40,

        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        Qty = 50,

        /// <summary>
        /// 尺寸
        /// </summary>
        [Label("尺寸")]
        Size = 60,
    }
}