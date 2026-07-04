using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Enums
{
    /// <summary>
    /// 检验状态
    /// </summary>
    public enum InspectionStatus
    {
        /// <summary>
        /// 待检
        /// </summary>
        [Label("待检")]
        WaitInspection,

        /// <summary>
        /// 检验中
        /// </summary>
        [Label("检验中")]
        Inspecting,

        /// <summary>
        /// 已检
        /// </summary>
        [Label("已检")]
        Inspectioned,
    }
}
