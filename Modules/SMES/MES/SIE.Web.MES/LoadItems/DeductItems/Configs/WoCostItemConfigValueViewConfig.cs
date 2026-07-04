using SIE.MES.LoadItems.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.LoadItems.DeductItems.Configs
{
    /// <summary>
    /// 工单耗用单配置项视图配置
    /// </summary>
    public class WoCostItemConfigValueViewConfig : WebViewConfig<WoCostItemNoConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.CostNoRule).Show();
            }
        }
        
    }
}
