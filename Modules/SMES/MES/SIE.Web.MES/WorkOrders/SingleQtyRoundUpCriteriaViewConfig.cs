using SIE.MES.WorkOrders._Routing_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrders
{
    public class SingleQtyRoundUpCriteriaViewConfig : WebViewConfig<SingleQtyRoundUpCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ItemType).Show();
            }
        }
    }
}
