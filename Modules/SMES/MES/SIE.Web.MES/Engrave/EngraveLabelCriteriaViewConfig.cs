using SIE.MES.Engrave;
using SIE.MES.WIP.Pressure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Engrave
{
    public class EngraveLabelCriteriaViewConfig : WebViewConfig<EngraveLabelCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderId).Show();
                View.Property(p => p.ResourceId).Show();
                View.Property(p => p.BatchNo).Show();
                View.Property(p => p.Product).Show();
                View.Property(p => p.Sn).Show();

            }
        }
    }
}
