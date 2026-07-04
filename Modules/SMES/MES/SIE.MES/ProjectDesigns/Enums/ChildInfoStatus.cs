using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Enums
{
    /// <summary>
    /// 子表建立属性
    /// </summary>
    public enum ChildInfoStatus
    {
        /// <summary>
        /// 未建立
        /// </summary>
        [Label("未建立")]
        UnFill = 0,

        /// <summary>
        /// 已补充
        /// </summary>
        [Label("已补充")]
        HasFilled = 1,
    }
}
