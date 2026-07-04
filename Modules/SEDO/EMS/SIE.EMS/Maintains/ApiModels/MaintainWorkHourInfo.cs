using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养工时信息实体
    /// </summary>
    [Serializable]
    public class MaintainWorkHourInfo
    {
        /// <summary>
        /// 工时登记ID
        /// </summary>
        public double MaintainWorkHourId { get; set; }

        /// <summary>
        /// 执行人ID
        /// </summary>
        public double? EmployeeId { get; set; }

        /// <summary>
        /// 执行人编码
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// 执行人名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 保养开始日期
        /// </summary>
        public string BeginDay { get; set; }

        /// <summary>
        /// 保养结束日期
        /// </summary>
        public string EndDay { get; set; }

        /// <summary>
        /// 工时(H)
        /// </summary>
        public double WorkHours { get; set; }       
    }
}
