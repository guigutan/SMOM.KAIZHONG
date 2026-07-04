using SIE.EMS.SpecialEquipment.RegularInspections.Criterias;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Criterias
{
    internal class SelInspectionProjectCriteriaViewConfig : WebViewConfig<SelInspectionProjectCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name).UseTextEditor(p => p.MaxLength = 250).Show(ShowInWhere.All);
            View.Property(p => p.ProjectType).UseTextEditor(p => p.MaxLength = 250).Show(ShowInWhere.All);
            View.Property(p => p.SpecialEquipmentAccountId).Show(ShowInWhere.Hide);
            View.Property(p => p.InspectionRuleId).Show(ShowInWhere.Hide);
        }
    }
}
