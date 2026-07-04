using SIE.Common.WorkFlow.Models;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.PDCA
{
    /// <summary>
    /// 原因分析Model
    /// </summary>
    [Serializable]
    public class ReasonAnalysisModel : TriggerModel
    {
        /// <summary>
        /// SIE工作流Id
        /// </summary>
        public double FlowInstanceId { get; set; }
    }
}
