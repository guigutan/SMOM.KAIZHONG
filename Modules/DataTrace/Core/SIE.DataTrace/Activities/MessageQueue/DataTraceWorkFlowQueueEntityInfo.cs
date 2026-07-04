using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Activities.MessageQueue
{
    /// <summary>
    /// 数据追溯流程触发信息-实体类型
    /// </summary>
    [Serializable]
    public class DataTraceWorkFlowQueueEntityInfo
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
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrg { get; set; }

    }
}
