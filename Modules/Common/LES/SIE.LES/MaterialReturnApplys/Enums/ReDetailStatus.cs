using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys.Enums
{
    /// <summary>
    /// 退料申请明细状态
    /// </summary>
    public enum ReDetailStatus
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Created = 0,

        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        ToReceive = 1,

        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        Received = 2,

        /// <summary>
        /// 已取消
        /// </summary>
        [Label("已取消")]
        Cancel = 3,
    }
}
