using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工出勤统计
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EmployeeClockInAttentCriteria))]
    [Label("员工出勤统计")]
    public partial class EmployeeClockInAttent : EmployeeClockIn
    {
    }
}