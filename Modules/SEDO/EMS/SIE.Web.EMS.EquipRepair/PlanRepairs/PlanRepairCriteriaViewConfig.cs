using SIE.Web.EMS.EquipRepair.PlanRepairs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.PlanRepairs
{
    /// <summary>
    ///计划维修查询视图
    /// </summary>
    public class PlanRepairCriteriaViewConfig : WebViewConfig<PlanRepairCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No).Show();
            View.Property(p => p.EquipAccountId).Show();
            View.Property(p => p.StandardType).Show();
            View.Property(p => p.CreateId).Show();
            View.Property(p => p.ApprovalStatus).Show();
            View.Property(p => p.PlanDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
