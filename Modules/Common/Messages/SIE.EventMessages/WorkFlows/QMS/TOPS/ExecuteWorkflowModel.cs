using SIE.Common.WorkFlow.Models;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.TOPS
{
    /// <summary>
    /// 开始8D-工作流-Model
    /// </summary>
    [Serializable]
    public class ExecuteWorkflowModel : TriggerModel
    {
        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// SIE工作流Id
        /// </summary>
        public double FlowInstanceId { get; set; }
    }
}
