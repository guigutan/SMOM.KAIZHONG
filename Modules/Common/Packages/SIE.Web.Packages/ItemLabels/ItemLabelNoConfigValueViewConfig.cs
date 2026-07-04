using SIE.Packages.ItemLabels.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签号配置值视图配置
    /// </summary>
    class ItemLabelNoConfigValueViewConfig : WebViewConfig<ItemLabelNoConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BacodeRule).Show(ShowInWhere.All);
            }
        }
    }
}
