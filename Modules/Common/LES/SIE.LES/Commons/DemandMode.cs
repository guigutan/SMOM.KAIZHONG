using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.LES.Commons
{
    /// <summary>
    /// 需求计算方式
    /// </summary>
    public enum DemandMode
    {
        /// <summary>
        /// 手工填写
        /// </summary>
        [Label("手工填写")]
        [Category("Manual")]
        ManualFillIn = 0,

        /// <summary>
        /// 按工单剩余量
        /// </summary>
        [Label("按工单剩余量")]
        WoSurplusQty = 1,

        /// <summary>
        /// 备料到安全水位
        /// </summary>
        [Label("备料到安全水位")]
        StockToSafeLevelForBeat = 2,

        /// <summary>
        /// 备料量为安全水位
        /// </summary>
        [Label("备料量为安全水位")]
        StockIsSafeLevelForBeat = 3,

        /// <summary>
        /// 固定量
        /// </summary>
        [Label("固定量")]
        FixedQuantity = 4,

        /// <summary>
        /// 最高存量
        /// </summary>
        [Label("最高存量")]
        MaxStock = 5,

        /// <summary>
        /// 手工填写-按套数备料
        /// </summary>
        [Label("手工填写-按套数备料")]
        ManualSetQuantity = 6,
    }
}
