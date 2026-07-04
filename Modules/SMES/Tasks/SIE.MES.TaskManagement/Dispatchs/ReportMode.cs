using SIE.ObjectModel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 报工方式
    /// </summary>
    public enum ReportMode
    {
        /// <summary>
        /// 自动
        /// </summary>
        [Label("自动")]
        Auto,

        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Manual,
    }
}