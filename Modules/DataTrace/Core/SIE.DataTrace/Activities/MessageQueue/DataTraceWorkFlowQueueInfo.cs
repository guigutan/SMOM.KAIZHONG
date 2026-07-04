using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Activities.MessageQueue
{
    /// <summary>
    /// 数据追溯流程触发信息
    /// </summary>
    [Serializable]
    public class DataTraceWorkFlowQueueInfo
    {
        /// <summary>
        /// 追溯主数据ID
        /// </summary>
        public double TraceMainDataId { get; set; }
        /// <summary>
        /// 流程进度ID
        /// </summary>
        public double FlowInstanceId { get; set; }
        /// <summary>
        /// 节点ID
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrg { get; set; }

    }

}
