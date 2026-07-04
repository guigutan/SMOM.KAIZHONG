using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Common.Traces
{
    /// <summary>
    /// 
    /// </summary>
    public enum TraceType
    {
        /// <summary>
        /// 序列号管理
        /// </summary>
        [Label("序列号管理")]
        SerialNumber = 0,

        /// <summary>
        /// 批次管理
        /// </summary>
        [Label("批次管理")]
        Batch = 1,

        /// <summary>
        /// 非序列号、非批次管理
        /// </summary>
        [Label("非序列号、非批次管理")]
        None = 2,
    }
}
