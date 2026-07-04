using SIE.MES.WIP.Pressure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WIP.Pressure
{
    public class WipPressureCriteriaViewConfig : WebViewConfig<WipPressureCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.Sn).Show();
                View.Property(p => p.BatchNo).Show();
                View.Property(p => p.ProductCode).Show().Readonly(false);
                View.Property(p => p.ProductName).Show().Readonly(false);
                View.Property(p => p.ResourceCode).Show().Readonly(false);
                View.Property(p => p.ResourceName).Show().Readonly(false);
                View.Property(p => p.BeginTime).Show().UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Week; });
            }
        }
    }
}
