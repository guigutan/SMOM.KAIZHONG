using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Interfaces.Datas
{
    /// <summary>
    /// 预约状态
    /// </summary>
    public enum ApointStatus
    {
        /// <summary>
        /// 有效
        /// </summary>
        [Label("有效")]
        Effective,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel,

        /// <summary>
        /// 失效
        /// </summary>
        [Label("失效")]
        Failure,

        /// <summary>
        /// 已排队
        /// </summary>
        [Label("已排队")]
        OnLine,
    }
}
