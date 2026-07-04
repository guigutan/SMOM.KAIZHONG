using SIE.MetaModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备权限员工
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipEmployeeCriteria))]
    public class EquipEmployee : SIE.Resources.Employees.Employee
    {
        

    }
}
