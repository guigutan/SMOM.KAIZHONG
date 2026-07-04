using SIE.MES.TaskManagement.FeedingRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class ScrapWeighingRecordCriteriaViewConfig : WebViewConfig<ScrapWeighingRecordCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).Show();
                View.Property(p => p.Lot).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
            }
        }
    }
}
