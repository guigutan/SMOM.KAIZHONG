using SIE.EMS.Maintains.AlertPlugs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养计划提前预警视图
    /// </summary>
    internal class MaintainPlanAdvanceAlertPlugConfigViewConfig : WebViewConfig<MaintainPlanAdvanceAlertPlugConfig>
    {
        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.AlertValue)
                .UseFormSetting(e => { e.HelpInfo = "该值应在严重等级取值范围内，若不在则无法触发保养计划提前预警"; });
        }
    }
}
