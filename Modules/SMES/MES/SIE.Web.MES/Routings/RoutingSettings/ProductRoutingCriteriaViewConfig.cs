using SIE.Items;
using SIE.MES.Routings.RoutingSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Routings.RoutingSettings
{
    public class ProductRoutingCriteriaViewConfig : WebViewConfig<ProductRoutingCriteria>
    {
        protected override void ConfigQueryView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.Product).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ItemController>().GetProductItems(keyword, pagingInfo);
            });
            View.Property(p => p.Routing);
            View.Property(p => p.StartDate).UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Month;
                e.DateFormat = "Y/m/d";
            });
            View.Property(p => p.EndDate).UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Month;
                e.DateFormat = "Y/m/d";
            });
        }
    }
}
