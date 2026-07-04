using SIE.ObjectModel;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 排程类型
    /// </summary>
    public enum SchedulingType
    {
        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Manual,

        /// <summary>
        /// 自动
        /// </summary>
        [Label("自动")]
        Auto,
    }
}