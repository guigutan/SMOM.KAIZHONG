using SIE.EMS.Common.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Common
{

    /// <summary>
    /// 配置值视图
    /// </summary>
    public class IsDepartmentPlanConfigValueViewConfig : WebViewConfig<IsDepartmentPlanConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsDepartmentPlan);
        }
    }
}
