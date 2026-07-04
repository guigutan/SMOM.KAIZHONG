using SIE.MES.TaskManagement.StockDeducRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.StockDeducRecords
{
    public class StockDeducRecordCriteriaViewConfig : WebViewConfig<StockDeducRecordCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.TaskNo).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.BatchNo).Show();
                View.Property(p => p.Resource).Show();
                View.Property(p => p.Process).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ItemShortDescription).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.LastMonth);
            }
        }
    }
}
