using SIE.EMS.EquipRepair.EquipRepairs.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels.Employees
{
    /// <summary>
    /// 员工视图
    /// </summary>
    public class EmployeeByRepairAccountViewConfig :WebViewConfig<EmployeeByRepairAccount>
    {
        /// <summary>
        /// 主配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(EmployeeByRepairAccount.NameProperty);
        }
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p=>p.Code);
            View.Property(p=>p.Name);
        }
    }
}
