using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Projects.Enums
{
    /// <summary>
    /// 参数值类型
    /// </summary>
    public enum ProcessStDtlValueType
    {
        /// <summary>
        /// 单值
        /// </summary>
        [Label("单值")]
        Single = 0,

        /// <summary>
        /// 范围值
        /// </summary>
        [Label("范围值")]
        Range = 1,
    }
}
