using SIE.ObjectModel;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 盘点细度
    /// </summary>
    public enum CountDimension
    {
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        Lot = 10,

        /// <summary>
        /// 标签
        /// </summary>
        [Label("标签")]
        Label = 20,

        /// <summary>
        /// 标签+库位
        /// </summary>
        [Label("标签+库位")]
        Location = 30,

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        Item = 40,
    }
}