using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders
{
    /// <summary>
	/// 加急单状态
	/// </summary>
	public enum UrgentOrderState
    {
        /// <summary>
        /// 无效
        /// </summary>
        [Label("无效")]
        InValid = 0,
        /// <summary>
        /// 有效
        /// </summary>
        [Label("有效")]
        Valid = 10,
    }
}
