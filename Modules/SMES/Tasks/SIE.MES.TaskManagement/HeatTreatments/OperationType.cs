using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.HeatTreatments
{
    /// <summary>
    /// 作业类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 入炉
        /// </summary>
        [Label("入炉")]
        In = 1,
        /// <summary>
        /// 出炉
        /// </summary>
        [Label("出炉")]
        Out = 2,
    }
}
