using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels.Criterias;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels.Employees
{

    /// <summary>
    /// 员工
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [ConditionQueryType(typeof(EmployeeByRepairAccountCriteria))]
    [Label("员工")]
    //[DisplayMember(nameof(Name))]
    public class EmployeeByRepairAccount : Employee
    {
    }
    /// <summary>
    /// 员工 实体配置
    /// </summary>
    internal class EmployeeByRepairAccountConfig : EntityConfig<EmployeeByRepairAccount>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_EMP").MapAllProperties();
            //Meta.Property(EmployeeByRepairAccount.UserIdProperty).MapColumn().IgnoreFK();
            //Meta.Property(EmployeeByRepairAccount.SexProperty).MapColumn().IsNullable();//SIE.Resources.Employee没有此字段，数据库需要可空
            //Meta.Property(EmployeeByRepairAccount.RemarkProperty).MapColumn().HasLength(1200);
            Meta.DisableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
