using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WorkFlows.QMS.TOPS
{
    /// <summary>
    /// 撤回8D申请-工作流-Model
    /// </summary>
    [Serializable]
    public class WithDrawWorkflowModel
    {
        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 撤回原因
        /// </summary>
        public string Reason { get; set; }
    }
}
