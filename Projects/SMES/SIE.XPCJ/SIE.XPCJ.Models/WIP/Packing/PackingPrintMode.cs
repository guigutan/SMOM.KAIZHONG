using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Packing
{
    public enum PrintMode
    {
        /// <summary>
        /// 提前打印
        /// </summary>
        [Label("提前打印")]
        InAdvance,

        /// <summary>
        /// 在线打印
        /// </summary>
        [Label("在线打印")]
        Online
    }
}
