using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.StockOrders.WorkOrders
{
    /// <summary>
    /// 
    /// </summary>
    public class StockOrderItemViewModelCriteriaViewConfig : WebViewConfig<StockOrderItemViewModelCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExecuteQuery);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.ConsumeMode).Readonly(p => p.StockType != SIE.LES.PrepareItemType.OverBom).UseEnumEditor(p => p.FilterCategoery = "PullPush").Show();
            }
        }
    }
}
