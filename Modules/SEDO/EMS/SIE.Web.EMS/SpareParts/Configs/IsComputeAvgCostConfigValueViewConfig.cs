using SIE.EMS.SpareParts.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.Configs
{
    /// <summary>
    ///  视图配置
    /// </summary>
    public class IsComputeAvgCostConfigValueViewConfig : WebViewConfig<IsComputeAvgCostConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsComputeAvgCost);
        }
    }
}
