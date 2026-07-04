using SIE.MES.Outsourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing
{
    public class OutboundConfirmDetailCriteriaViewConfig : WebViewConfig<OutboundConfirmDetailCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FlowNo).Show();
                View.Property(p => p.InitiatorFactory).Show();
                View.Property(p => p.OutFactory).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.DeliveryDate).Show().UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.LastMonth);
                View.Property(p => p.Sn).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
            }
        }
    }
}
