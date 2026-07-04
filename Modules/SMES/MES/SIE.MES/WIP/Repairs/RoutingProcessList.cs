using SIE.Tech.Processs;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 工艺路线工序列表
    /// </summary>
    [Serializable]
    public class RoutingProcessList : List<Process>
    {
        /// <summary>
        /// 下一工序Id
        /// </summary>
        public double? NextProcessId { get; set; }
    }
}
