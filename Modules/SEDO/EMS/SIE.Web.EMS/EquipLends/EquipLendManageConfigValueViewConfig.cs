using SIE.EMS.EquipLends.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipLends
{
    /// <summary>
    /// 设备借还配置项视图配置
    /// </summary>
    public class EquipLendManageConfigValueViewConfig : WebViewConfig<EquipLendManageConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.NoRule).Show();
                View.Property(p => p.LendExamine).Show();
                View.Property(p => p.ReturnExamine).Show();
            }
                
        }
    }
}
