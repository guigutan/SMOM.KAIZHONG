using SIE.Common.WorkFlow.Models;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.PDCA
{
    /// <summary>
    /// 填写对策Model
    /// </summary>
    [Serializable]
    public class FillInMeasureModel : TriggerModel
    {
        /// <summary>
        /// SIE工作流Id
        /// </summary>
        public double FlowInstanceId { get; set; }
    }
}
