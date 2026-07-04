using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单APP接收方式
    /// </summary>
    public enum StockReceiveType
    {
        /// <summary>
        /// 手动接收
        /// </summary>
        [Label("手动接收")]
        Hand = 0,

        /// <summary>
        /// 自动接收
        /// </summary>
        [Label("自动接收")]
        Auto = 1,
    }
}
