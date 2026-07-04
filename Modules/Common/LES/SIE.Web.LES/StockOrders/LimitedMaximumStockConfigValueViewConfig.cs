using SIE.LES.StockOrders.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 推式备料是否限制最高存量
    /// </summary>
    public class LimitedMaximumStockConfigValueViewConfig : WebViewConfig<LimitedMaximumStockConfigValue>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsLimited).Show(ShowInWhere.All);
            }
        }
    }
}
