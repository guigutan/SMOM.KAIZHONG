using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Enums
{
    /// <summary>
    /// 打印标签类型
    /// </summary>
    public enum PrintLabelType
    {
        /// <summary>
        /// 良品
        /// </summary>
        [Label("良品")]
        Good=1,

        /// <summary>
        /// 可疑品
        /// </summary>
        [Label("可疑品")]
        Suspect = 2,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap = 3,

        /// <summary>
        /// 返工
        /// </summary>
        [Label("返工")]
        Rework = 3,
    }
}
