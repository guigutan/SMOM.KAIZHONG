using SIE.MES.TaskManagement.FeedingRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class FeedingRecordCriteriaViewConfig : WebViewConfig<FeedingRecordCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.WorkOrder).Show();
                //View.Property(p => p.TaskNo).Show();
                //View.Property(p => p.Process).Show();
                View.Property(p => p.WipResource).Show();
                View.Property(p => p.WipResourceName).Show();
                View.Property(p => p.FeedingAreaCode).Show();
                View.Property(p => p.FeedingAreaName).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.Label).Show();
                View.Property(p => p.ItemLabelLot).Show();
                View.Property(p => p.CreateDate).Show().UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
                });
            }
        }
    }
}
