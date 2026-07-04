using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.WorkOrders
{
    /// <summary>
    /// 备料类型
    /// </summary>
    public enum WorkOrderMpType
    {
        /// <summary>
        /// 发运订单直发
        /// </summary>
        [Label("发运订单直发")]
        Direct = 0,

        /// <summary>
        /// 备料单
        /// </summary>
        [Label("备料单")]
        ByOrder = 1,
    }
}
