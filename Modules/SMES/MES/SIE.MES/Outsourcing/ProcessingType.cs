using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    public enum ProcessingType
    {
        /// <summary>
        /// 良品
        /// </summary>
        [Label("良品")]
        Good = 1,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap = 2,

        /// <summary>
        /// 返工
        /// </summary>
        [Label("返工")]
        Rework = 3,

        /// <summary>
        /// 可疑品
        /// </summary>
        [Label("可疑品")]
        Sup = 4
    }
}
