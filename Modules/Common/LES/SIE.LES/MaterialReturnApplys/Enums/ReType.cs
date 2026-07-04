using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys.Enums
{
    /// <summary>
    /// 退料类型
    /// </summary>
    public enum ReType
    {
        /// <summary>
        /// 生产退料
        /// </summary>
        [Label("生产退料")]
        WorkOrderReturn = 0,


        /// <summary>
        /// 车间退料
        /// </summary>
        [Label("车间退料")]
        WorkShopReturn = 1,
    }
}
