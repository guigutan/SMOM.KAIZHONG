using SIE.EMS.Checks.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Checks.Configs
{
    /// <summary>
    /// 配置值视图
    /// </summary>
    public class CheckPlanScoreConfigValueViewConfig : WebViewConfig<CheckPlanScoreConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsNeedCheckScore);
        }
    }
}
