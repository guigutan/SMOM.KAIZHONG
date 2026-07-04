using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Piles.Configs
{
    public class LpnRuleConfigValueViewConfig : WebViewConfig<LpnRuleConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("移动端码盘创建LPN格式验证");
            using (View.OrderProperties())
            {
                View.Property(p => p.LpnRuleText).Show().UseListSetting(e => { e.HelpInfo = "例子：A_BC_D，代表编码需要以A开头，包含BC，以D结束（大小写敏感）"; });
            }
        }
    }
}
