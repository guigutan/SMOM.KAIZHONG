using SIE.MES.TaskManagement.FeedingRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class DeductionRecordCriteriaViewConfig : WebViewConfig<DeductionRecordCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.TaskNo).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ItemLabel).HasLabel("上料标签").Show();
                View.Property(p => p.ItemLabelLot).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.BatchNo).Show();
                View.Property(p => p.Licha).Show();
                View.Property(p => p.Resource).Show();
                View.Property(p => p.Process).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ItemShortDescription).Show();
                View.Property(p => p.Mblnr).ShowInList(200);
                View.Property(p => p.Mjahr).ShowInList(200);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.LastMonth);
                View.Property(p => p.UploadResult).Show();
            }
        }
    }
}
