using SIE.EMS.Checks.Configs;
using SIE.EMS.Checks.Plans;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Checks.Configs
{
    /// <summary>
    /// 配置值视图
    /// </summary>
    public class CheckPlanTypeConfigValueViewConfig : WebViewConfig<CheckPlanTypeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.CalendarScheme);
            View.Property(p => p.CheckPlanType);
            View.Property(p => p.Frequency).DefaultValue(8).UseSpinEditor(p => { p.MinValue = 1; p.MaxValue = 24; p.AllowNegative = false; p.AllowDecimals = false; p.AllowBlank = false; p.Step = 1; }).Readonly(p=>p.CheckPlanType != CheckPlanType.Time).Visibility(p => p.CheckPlanType == CheckPlanType.Time);
        }
    }
}
