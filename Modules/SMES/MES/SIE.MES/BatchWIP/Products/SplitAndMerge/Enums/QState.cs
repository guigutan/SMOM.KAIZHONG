using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP.Products.SplitAndMerge.Enums
{
    /// <summary>
    /// 质量状态
    /// </summary>
    public enum QState
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        Pass = 0,

        /// <summary>
        /// 不良
        /// </summary>
        [Label("不良")]
        UnPass = 1,
    }
}
