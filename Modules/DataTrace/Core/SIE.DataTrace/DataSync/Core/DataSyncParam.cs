using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.DataSync.Core
{
    /// <summary>
    /// 数据同步参数
    /// </summary>
    [Serializable]
    public class DataSyncParam
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
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }
    }
}
