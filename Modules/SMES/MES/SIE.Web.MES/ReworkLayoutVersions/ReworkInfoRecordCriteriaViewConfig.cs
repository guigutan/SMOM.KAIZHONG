using SIE.MES.ReworkLayoutVersions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ReworkLayoutVersions
{
    public class ReworkInfoRecordCriteriaViewConfig : WebViewConfig<ReworkInfoRecordCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.State).Show().UseEnumEditor(p => p.AllowBlank = true);
                View.Property(p => p.Factory).Show();
                View.Property(p => p.ItemId).Show();
                View.Property(p => p.BeginDateTime).UseDateRangeEditor().Show();
                View.Property(p => p.EndDateTime).UseDateRangeEditor().Show();
                View.Property(p => p.ReworkLayoutVersionId).Show();
                View.Property(p => p.UniqueCode).Show();
                View.Property(p => p.Department).Show();
                View.Property(p => p.ProductOrder).Show();
                View.Property(p => p.Msg).Show();
                View.Property(p => p.Sn).Show();
                View.Property(p => p.WorkOrderNo).Show();
            }
        }
    }
}
