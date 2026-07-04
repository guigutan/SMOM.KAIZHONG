using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.EmployeeReports.ClockingIns
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("员工出勤查询实体")]
    public class EmployeeClockInReportCriteria : EmployeeClockInCriteria
    {
        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <returns>员工出勤列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EmployeeReportController>().GetEmployeeClockIns(this);
        }
    }
}
