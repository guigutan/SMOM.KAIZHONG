using SIE.Common.WorkFlow.Models;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.PDCA
{
    [Serializable]
    public class EfeectVerficationModel : TriggerModel
    {
        /// <summary>
        /// 
        /// </summary>
        public double FlowInstanceId { get; set; }
    }
}
