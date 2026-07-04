using SIE.Resources.Employees;
using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 员工信息API类
    /// </summary>
    [Serializable]
    public class EmployeeInfo
    {

        /// <summary>
        /// 员工Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 员工工号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 员工性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /* /// <summary>
        /// 员工状态-在职-离职
        /// </summary>
        public EmployeeStatus EmployeeStatus { get; set; } */

        /// <summary>
        /// 员工类型--组长--班长--班组长
        /// </summary>
        public EmployeeType? EmployeeType { get; set; }

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId { get; set; }
    }
}
