using SIE.MES.TeamManagement.ClockingIns;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.EmployeeReports.ClockingIns
{
    /// <summary>
    /// 员工出勤统计
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EmployeeClockInAttentReportCriteria))]
    [Label("员工出勤统计")]
    public class EmployeeClockInAttentReport : EmployeeClockInAttent
    {
    }
}
