using SIE.ObjectModel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 关联关系
    /// </summary>
    public enum Associated
    {
        /// <summary>
        /// 合并
        /// </summary>
        [Label("合并")]
        Combine,

        /// <summary>
        /// 拆分
        /// </summary>
        [Label("拆分")]
        Split,

        /// <summary>
        /// 共模
        /// </summary>
        [Label("共模")]
        Syntype,
    }
}