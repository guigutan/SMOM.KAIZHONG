using SIE.ObjectModel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 优先级
    /// </summary>
    public enum DispatchTaskPriority
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Label("普通")]
        Normal,

        /// <summary>
        /// 紧急
        /// </summary>
        [Label("紧急")]
        Urgency,
    }
}