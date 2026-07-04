using SIE;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockQueues
{
    /// <summary>
    /// 优先级
    /// </summary>
    public enum QueuePriority
    {
        /// <summary>
        /// 最靠后
        /// </summary>
        [Label("最靠后")]
        Last,

        /// <summary>
        /// 普通
        /// </summary>
        [Label("普通")]
        Normal,

        /// <summary>
        /// 紧急
        /// </summary>
        [Label("紧急")]
        Urgent,

        /// <summary>
        /// 越库
        /// </summary>
        [Label("越库")]
        Surpass,

        /// <summary>
        /// 最优先
        /// </summary>
        [Label("最优先")]
        Highest,
    }
}