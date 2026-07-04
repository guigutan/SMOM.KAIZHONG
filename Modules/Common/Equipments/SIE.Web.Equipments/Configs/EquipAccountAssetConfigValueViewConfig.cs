using SIE.Equipments.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Equipments.Configs
{
    /// <summary>
    /// 是否固定资产
    /// </summary>
    public class EquipAccountAssetConfigValueViewConfig : WebViewConfig<EquipAccountAssetConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Asset).Show();
            View.Property(p => p.UseCard).Show();

        }
    }
}
