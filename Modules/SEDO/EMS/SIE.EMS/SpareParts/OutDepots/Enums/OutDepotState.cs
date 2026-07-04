using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.OutDepots.Enums
{
    /// <summary>
    /// 状态
    /// </summary>
    [Label("状态")]
    public enum OutDepotState
    {
        /// <summary>
        /// 待出库
        /// </summary>
        [Label("待出库")]
        Ing = 0,
        /// <summary>
        /// 已出库
        /// </summary>
        [Label("已出库")]
        Ed = 1,
        /// <summary>
        /// 部分出库
        /// </summary>
        [Label("部分出库")]
        PartOut = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Label("已关闭")]
        Close = 3,
    }
}
