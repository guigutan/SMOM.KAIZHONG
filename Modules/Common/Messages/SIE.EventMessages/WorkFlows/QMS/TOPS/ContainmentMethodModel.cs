using SIE.Common.WorkFlow.Models;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.TOPS
{
    /// <summary>
    /// 围堵应对Model
    /// </summary>
    [Serializable]
    public class ContainmentMethodModel : TriggerModel
    {
        /// <summary>
        /// SIE工作流Id
        /// </summary>
        public double FlowInstanceId { get; set; }
    }
}
