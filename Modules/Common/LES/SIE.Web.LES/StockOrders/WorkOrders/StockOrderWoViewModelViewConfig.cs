using SIE.LES.StockOrders.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.StockOrders.WorkOrders
{
    /// <summary>
    /// 备料单查询工单视图配置
    /// </summary>
    public class StockOrderWoViewModelViewConfig : WebViewConfig<StockOrderWoViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.WoNo).Readonly().ShowInList(150);
            View.Property(p => p.ProductCode).Readonly().ShowInList(150);
            View.Property(p => p.ProductName).Readonly().ShowInList(150);
            View.Property(p => p.Factory).Readonly().ShowInList(150);
            View.Property(p => p.Workshop).Readonly().ShowInList(150);
            View.Property(p => p.WipResource).Readonly().ShowInList(150);
            View.Property(p => p.PlanQty).Readonly().ShowInList(150);
            View.Property(p => p.WoState).Readonly().ShowInList(150);
            View.Property(p => p.PlanBeginDate).Readonly().ShowInList(150);
            View.Property(p => p.PlanEndDate).Readonly().ShowInList(150);
        }
    }
}
