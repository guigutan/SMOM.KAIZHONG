using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘在制用户数据
    /// </summary>
    [Serializable]
    public class WipEmployeeInfo
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 用户范权限范围内的产线数据
        /// </summary>
        public List<EdgeResource> Resources { get; set; } = new List<EdgeResource>();

        /// <summary>
        /// 用户权限范围内的工序信息
        /// </summary>
        public List<EdgeProcess> Processes { get; set; } = new List<EdgeProcess>();

        /// <summary>
        /// 用户权限范围内的工位信息
        /// </summary>
        public List<EdgeStation> Stations { get; set; } = new List<EdgeStation>();
    }
}
