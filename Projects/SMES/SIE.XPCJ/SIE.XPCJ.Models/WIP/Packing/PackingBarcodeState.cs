using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Packing
{
    /// <summary>
    /// 条码状态
    /// </summary>
    public enum BarcodeState
    {
        /// <summary>
        /// 未打印
        /// </summary>
        [Label("未打印")]
        Notprint = 0,

        /// <summary>
        /// 已打印
        /// </summary>
        [Label("已打印")]
        Printed = 1,
    }
}
