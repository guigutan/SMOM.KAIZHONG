using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 发货确认明细状态
    /// </summary>
    public enum OutboundConfirmDetailState
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Create = 0,

        /// <summary>
        /// 已发货
        /// </summary>
        [Label("已发货")]
        Delivery = 1,

        /// <summary>
        /// 退回
        /// </summary>
        [Label("退回")]
        Return = 2
    }
}
