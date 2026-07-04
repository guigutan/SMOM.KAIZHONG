using SIE.Common.WorkFlow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.WorkFlow.Variables
{
    /// <summary>
    /// 红牌申请流程变量类
    /// </summary>
    [Serializable]
    public class RedCardApplyVariable : VariableBase
    {
        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }
    }
}
