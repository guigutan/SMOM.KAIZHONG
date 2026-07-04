using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys.Enums
{
    /// <summary>
    /// 良品状态
    /// </summary>
    public enum ReDetailQuality
    {
        /// <summary>
        /// 良品
        /// </summary>
        [Label("良品")]
        Good = 0,

        /// <summary>
        /// 不良品
        /// </summary>
        [Label("不良品")]
        NotGood = 1,
    }
}
