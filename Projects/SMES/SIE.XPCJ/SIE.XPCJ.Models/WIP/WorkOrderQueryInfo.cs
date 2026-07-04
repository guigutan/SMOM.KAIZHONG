using System;
using System.Collections.Generic;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 工单查询信息
    /// </summary>
    [Serializable]
    public class WorkOrderQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId { get; set; }
        /// <summary>
        /// 工单状态
        /// </summary>
        public List<int> StateList { get; set; }
    }
}
