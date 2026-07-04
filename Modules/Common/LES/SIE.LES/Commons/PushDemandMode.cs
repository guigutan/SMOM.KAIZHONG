using SIE.ObjectModel;

namespace SIE.LES
{
    /// <summary>
    /// 推式需求计算方式
    /// </summary>
    public enum PushDemandMode
	{
        /// <summary>
        /// 手工填写
        /// </summary>
        [Label("手工填写")]
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
        /// 手工填写-按套数备料
        /// </summary>
        [Label("手工填写-按套数备料")]
        ManualSetQuantity = 6,
    }
}