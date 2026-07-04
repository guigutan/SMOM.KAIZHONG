using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘 工序工位关系类
    /// </summary>
    [Serializable]
    public class EdgeProcessStation
    {
        /// <summary>
        /// 工艺路线工序Id
        /// </summary>
        public string RouteProcessId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 对应工位信息
        /// </summary>
        public Dictionary<string, EdgeStation> StationList { get; set; } = new Dictionary<string, EdgeStation>();

        /// <summary>
        /// 工位权限范围职员
        /// </summary>
        public Dictionary<string, EdgeEmployee> EmployeeList { get; set; } = new Dictionary<string, EdgeEmployee>();

    }
}
