using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 发料状态
    /// </summary>
    public enum OutboundState
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Label("未开始")]
        NotStarted = 0,

        /// <summary>
        /// 部分发料
        /// </summary>
        [Label("部分发料")]
        PartOutbound = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Finish = 2
    }
}
