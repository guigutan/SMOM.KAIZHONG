using SIE.Andon.Andons.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Configs
{
    /// <summary>
    /// 安灯推送配置项视图配置
    /// </summary>
    public class AndonPushPlugConfigValueViewConfig : WebViewConfig<AndonPushPlugConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.PushPugDefaultId).HasLabel("推送模块默认值").Show();
            }
        }
    }
}
