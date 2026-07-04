using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Enums
{
    public enum InterfaceState
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Label("未处理")]
        Unprocessed = 0,

        /// <summary>
        /// 处理中
        /// </summary>
        [Label("处理中")]
        Processing = 1,

        /// <summary>
        /// 已处理
        /// </summary>
        [Label("已处理")]
        Processed = 2,

        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        Failed = 3,
    }
}
