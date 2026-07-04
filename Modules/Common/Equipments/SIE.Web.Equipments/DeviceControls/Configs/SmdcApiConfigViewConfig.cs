using SIE.Equipments.DeviceControls.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Equipments.DeviceControls.Configs
{
    /// <summary>
    /// SMDC 配置视图
    /// </summary>
    internal class SmdcApiConfigViewConfig : WebViewConfig<SmdcApiConfigValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 视图配置
            View.Property(p => p.ApiUrl).Show();
        }
    }
}
