using SIE.Common.WorkFlow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Activities.Core
{
    /// <summary>
    /// 追溯主数据上下文变量类型
    /// </summary>
    [Serializable]
    public class DataTraceVariable: VariableBase
    {
        /// <summary>
        /// 追溯主数据ID
        /// </summary>
        public double? TraceMainDataId { get; set; }
    }
}
