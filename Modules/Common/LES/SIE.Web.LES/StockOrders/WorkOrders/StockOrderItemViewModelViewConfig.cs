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
    public class StockOrderItemViewModelViewConfig : WebViewConfig<StockOrderItemViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using(View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().ShowInList(150);
                View.Property(p => p.Name).Readonly().ShowInList(150);
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList(150);
                View.Property(p => p.SpecificationModel).Readonly().ShowInList(150);
                View.Property(p => p.ConsumeMode).Readonly().ShowInList(150);
                View.Property(p => p.Unit).Readonly().ShowInList(150);
            }
        }
    }
}
