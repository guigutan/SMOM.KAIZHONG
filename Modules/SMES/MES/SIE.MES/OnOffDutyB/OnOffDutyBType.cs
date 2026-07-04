using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OnOffDutyB
{
    /// <summary>
    /// B上下岗类型
    /// </summary>
    public enum OnOffDutyBType
    {
        /// <summary>
        /// 上岗
        /// </summary>
        [Label("上岗")]
        OnDuty = 0,


        /// <summary>
        /// 下岗
        /// </summary>

        [Label("下岗")]
        OffDuty = 1
    }
}
