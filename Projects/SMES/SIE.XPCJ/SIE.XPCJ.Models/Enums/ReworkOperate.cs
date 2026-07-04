using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.Enums
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum ReworkOperate
    {
        /// <summary>
        /// 条码置换
        /// </summary>
        [Label("条码置换")]
        Permute,

        /// <summary>
        /// 条码解绑关键件
        /// </summary>
        [Label("条码解绑关键件")]
        PermuteUnbound,

        /// <summary>
        /// 关键件解绑
        /// </summary>
        [Label("关键件解绑")]
        Unbound,
    }
}
