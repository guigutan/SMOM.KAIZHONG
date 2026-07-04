using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.DevicePurs.ApiModels
{
    /// <summary>
    /// 部门信息
    /// </summary>
    [Serializable]
    public class DepartmentInfo
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
    }
}
