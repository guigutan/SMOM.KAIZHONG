using SIE.ObjectModel;

namespace SIE.Core.Items
{
    /// <summary>
    /// 追溯方式
    /// </summary>
    public enum RetrospectType
    {
        /// <summary>
        /// 单体追溯
        /// </summary>
        [Label("单体追溯")]
        Single,
        /// <summary>
        /// 批次追溯
        /// </summary>
        [Label("批次追溯")]
        Batch,
    }

}
