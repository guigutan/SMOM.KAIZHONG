using SIE.LES.StockOrders.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 推式调度多触发方式同时满足的处理方式
    /// </summary>
    public class PushedSchedulingMethodsConfigValueViewConfig : WebViewConfig<PushedSchedulingMethodsConfigValue>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.PushedSchedulingMethod).Show(ShowInWhere.All);
            }
        }
    }
}
