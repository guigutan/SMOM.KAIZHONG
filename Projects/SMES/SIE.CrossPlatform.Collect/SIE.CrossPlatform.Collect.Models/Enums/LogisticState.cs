using SIE.CrossPlatform.Collect.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.Enums
{
    /// <summary>
    /// 物流状态
    /// </summary>
    public enum LogisticState
    {
        /// <summary>
        /// 未打印
        /// </summary>
        [Label("未打印")]
        UnPrinted = 0,

        /// <summary>
        /// 已打印
        /// </summary>
        [Label("已打印")]
        Printed = 1,
    }
}
