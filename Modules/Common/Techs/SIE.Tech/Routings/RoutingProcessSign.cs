using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序标记
    /// </summary>
    [Flags]
    public enum RoutingProcessSign
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal = 1,

        /// <summary>
        /// 开始
        /// </summary>
        [Label("开始")]
        Start = 2,

        /// <summary>
        /// 结束
        /// </summary>
        [Label("结束")]
        End = 4,
    }
}