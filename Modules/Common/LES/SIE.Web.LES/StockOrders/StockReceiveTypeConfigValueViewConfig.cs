using SIE.LES.StockOrders.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 备料单APP接收方式配置项视图配置
    /// </summary>
    public class StockReceiveTypeConfigValueViewConfig : WebViewConfig<StockReceiveTypeConfigValue>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ReceiveType).Show(ShowInWhere.All);
            }
        }
    }
}
