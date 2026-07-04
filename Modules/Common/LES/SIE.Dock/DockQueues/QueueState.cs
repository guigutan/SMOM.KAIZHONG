using SIE;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockQueues
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum QueueState
    {
        /// <summary>
        /// 等候
        /// </summary>
        [Label("等候")]
        Waiting,

        /// <summary>
        /// 装卸中
        /// </summary>
        [Label("装卸中")]
        Handling,

        /// <summary>
        /// 完工
        /// </summary>
        [Label("完工")]
        Finish,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel,
    }
}