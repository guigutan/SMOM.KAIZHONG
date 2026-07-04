using SIE.Barcodes.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Barcodes.Configs
{
    class CheckBcRangeStateConfigValueViewConfig : WebViewConfig<CheckBcRangeStateConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.IsCheck).Show();
        }
    }
}
