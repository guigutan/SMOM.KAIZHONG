using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 借调单审核API信息类
    /// </summary>
    [Serializable]
    public class ApproveInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ApproveInfo()
        {
            EmployeeList = new string[] { };
        }

        /// <summary>
        /// 审核结果
        /// 0为同意、1为拒绝
        /// </summary>
        public int ApproveResult { get; set; }

        /// <summary>
        /// 借调单ID
        /// </summary>
        public double OnLoanId { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 借调员工
        /// </summary>
        public string[] EmployeeList { get; set; }
    }
}
