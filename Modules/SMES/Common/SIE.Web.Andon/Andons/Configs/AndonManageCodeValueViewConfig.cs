using SIE.Andon.Andons.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Configs
{
    /// <summary>
    /// 安灯管理事件编码配置项视图配置
    /// </summary>
    public class AndonManageCodeValueViewConfig: WebViewConfig<AndonManageCodeConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonManageCodeRuleId).HasLabel("事件编码默认值").Show();
            }
        }
    }
}
