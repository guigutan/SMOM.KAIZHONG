using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.Enums
{
    /// <summary>
    /// 置换后处理方式
    /// </summary>
    public enum ReplaceItemHandleMethod
    {
        [Label("置换后作废")]
        Scrap = 10,
        [Label("置换后正常下料")]
        Recycle = 20,
        [Label("置换后不良下料")]
        NGRecycle = 30
    }
}
