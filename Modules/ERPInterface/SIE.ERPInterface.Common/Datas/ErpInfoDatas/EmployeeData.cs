using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public class EmployeeData : ErpInfoData
    {
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// 员工状态
        /// </summary>
        public int EmployeeStatus { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否创建用户
        /// </summary>
        public bool IsCreateAccount { get; set; }

        /// <summary>
        /// 账户编码
        /// </summary>
        public string AccountCode { get; set; }
    }
}
