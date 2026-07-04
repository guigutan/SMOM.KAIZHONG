using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs
{
    public enum ClassesType
    {
        /// <summary>
        /// 白班
        /// </summary>
        [Label("白班")]
        Day = 1,

        /// <summary>
        /// 晚班
        /// </summary>
        [Label("晚班")]
        Night = 2
    }
}
