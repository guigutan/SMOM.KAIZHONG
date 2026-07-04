using SIE.ObjectModel;

namespace SIE.LES
{
    /// <summary>
    /// 拉式触发方式
    /// </summary>
    public enum PullTriggerMode
    {
        /// <summary>
        /// 库存低于安全水位
        /// </summary>
        [Label("库存低于安全水位")]
        BelowSafe,
    }
}