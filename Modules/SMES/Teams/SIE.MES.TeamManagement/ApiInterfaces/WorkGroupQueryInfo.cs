using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 班组考勤查询条件API信息类
    /// </summary>
    [Serializable]
    public class WorkGroupQueryInfo
    {
        /// <summary>
        /// 页号
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }
    }
}
