using SIE.WorkFlow.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯节点-信息
    /// </summary>
    [Serializable]
    public class TraceWorkflowPanelInfo: WorkflowPanelInfo
    {
        /// <summary>
        /// 打印模板类型
        /// </summary>
        public string PrintEntityType
        {
            get;
            set;
        }

        /// <summary>
        /// 工程名
        /// </summary>
        public string AssemblyName
        {
            get;
            set;
        }
    }
}
