using SIE.EMS.EquipRepair.EquipRepairs.ViewModels.Criterias;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels.Criterias
{
    /// <summary>
    /// 员工可以维护该台账查询器视图
    /// </summary>
    public class EmployeeByRepairAccountCriteriaViewConfig:WebViewConfig<EmployeeByRepairAccountCriteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p=>p.EquipAccount).Show( ShowInWhere.Hide);
        }
    }
}
