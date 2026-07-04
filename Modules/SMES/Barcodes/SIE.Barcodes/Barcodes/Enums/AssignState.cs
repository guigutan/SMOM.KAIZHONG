using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Barcodes.Barcodes.Enums
{
    /// <summary>
    /// 指派状态
    /// </summary>
    public enum AssignState
    {
        /// <summary>
        /// 未指派
        /// </summary>
        [Label("未指派")]
        UnAssign = 0,

        /// <summary>
        /// 部分指派
        /// </summary>
        [Label("部分指派")]
        PartAssign = 1,

        /// <summary>
        /// 已指派
        /// </summary>
        [Label("已指派")]
        Assigned = 2,
    }
}
