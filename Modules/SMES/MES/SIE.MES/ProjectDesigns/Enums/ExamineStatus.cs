using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Enums
{   
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum ExamineStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        [Label("未审核")]
        UnExamine = 0,

        /// <summary>
        /// 已审核
        /// </summary>
        [Label("已审核")]
        Examined = 1,
    }
}
