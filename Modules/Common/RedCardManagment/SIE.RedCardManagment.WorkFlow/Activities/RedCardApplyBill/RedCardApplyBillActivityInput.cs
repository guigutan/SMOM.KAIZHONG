using SIE.RedCardManagment.WorkFlow.Variables;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.WorkFlow.Activities.RedCardApplyBill
{
    /// <summary>
    /// 红牌申请-单据信息节点-输入Input
    /// </summary>
    [Serializable]
    public class RedCardApplyBillActivityInput
    {
        /// <summary>
        /// 变量
        /// </summary>
        public RedCardApplyVariable Variable { get; set; }
    }
}
