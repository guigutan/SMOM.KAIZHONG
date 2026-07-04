using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务优先级
    /// </summary>
    public enum TaskLevel
    {
        /// <summary>
        /// 低
        /// </summary>
        [Category("IsVisable")]
        [Label("低")]
        Low = 0,

        /// <summary>
        /// 中
        /// </summary>
        [Category("IsVisable")]
        [Label("中")]
        Middle = 1,

        /// <summary>
        /// 高
        /// </summary>
        [Category("IsVisable")]
        [Label("高")]
        High = 2,

        /// <summary>
        /// 加急
        /// </summary>
        [Label("加急")]
        Urgent = 3,
    }
}
