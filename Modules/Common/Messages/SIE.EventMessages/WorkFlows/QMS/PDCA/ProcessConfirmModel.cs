using SIE.Common.WorkFlow.Models;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.PDCA
{
    [Serializable]
    public class ProcessConfirmModel : TriggerModel
    {
        /// <summary>
        /// SIE工作流Id
        /// </summary>
        public double FlowInstanceId { get; set; }
    }
}
