using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Activities.Core
{
    /// <summary>
    /// 数据追溯节点输出信息
    /// </summary>
    [Serializable]
    public class DataTraceActivityInfo
    {
        /// <summary>
        /// 追溯节点ID
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// 追溯实体类型
        /// </summary>
        public string EntityType { get; set; }

      
    }
}
