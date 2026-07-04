using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP.Products.SplitAndMerge.Enums
{
    /// <summary>
    /// 出入类型
    /// </summary>
    public enum InOut
    {
        /// <summary>
        /// 入站
        /// </summary>
        [Label("入站")]
        In = 0,

        /// <summary>
        /// 出站
        /// </summary>
        [Label("出站")]
        Out = 1,
    }
}
