using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public enum SourceType
    {
        /// <summary>
        /// MES生成
        /// </summary>
        [Label("MES生成")]
        Mes=1,

        /// <summary>
        /// 排程导入
        /// </summary>
        [Label("排程导入")]
        SchedulingInf = 2,

        /// <summary>
        /// 拆分
        /// </summary>
        [Label("拆分")]
        Split = 3

    }
}
