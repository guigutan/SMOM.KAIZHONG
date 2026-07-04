using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Base.TraceMainDatas.Datas
{
    /// <summary>
    /// 追溯主数据DTO
    /// </summary>
    [Serializable]
    public class DataMainTraceDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 流程进度ID
        /// </summary>
        public double FlowInstanceId { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public string Context { get; set; }
    }
}
