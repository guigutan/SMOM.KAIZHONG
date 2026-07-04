using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班视图模型
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ScheduleCriteria))]
    [Label("排班")]
    public class ShiftScheduleViewModel : ViewModel
    {
    }
}