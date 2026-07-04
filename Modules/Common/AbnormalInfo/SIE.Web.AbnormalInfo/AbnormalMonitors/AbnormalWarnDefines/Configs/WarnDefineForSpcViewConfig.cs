using SIE.AbnormalInfo.AbnormalMonitors.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalWarnDefines.Configs
{
    /// <summary>
    ///  预警定义-红牌管理配置值视图配置
    /// </summary>
    class WarnDefineForSpcViewConfig : WebViewConfig<WarnDefineForSpcConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.AbnormalWarnDefineId).Show(ShowInWhere.All);
            }
        }
    }
}
