using SIE.ObjectModel;

namespace SIE.EMS.Purchases.Enums
{
    /// <summary>
    /// 验收类型
    /// </summary>
    public enum AcceptanceType
    {
        /// <summary>
        /// 单台验收
        /// </summary>
        [Label("单台验收")]
        Single = 10,
        /// <summary>
        /// 批量验收
        /// </summary>
        [Label("批量验收")]
        Batch = 20,
    }
}