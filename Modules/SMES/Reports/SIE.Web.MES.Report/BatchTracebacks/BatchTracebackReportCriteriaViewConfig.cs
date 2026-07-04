using SIE.MES.Report.BatchTracebacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    public class BatchTracebackReportCriteriaViewConfig : WebViewConfig<BatchTracebackReportCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.BatchType).Show();
                View.Property(p => p.WorkShopName).Show();
                View.Property(p => p.ProcessId).Show();
                View.Property(p => p.NextProcessId).Show();
                View.Property(p => p.IsFinish).Show();
                View.Property(p => p.ItemLabelLot).Show();
                View.Property(p => p.BatchNo).Show();
                View.Property(p => p.ToolCode).Show();
                View.Property(p => p.UnitBarcode).Show();
                View.Property(p => p.BlueLabel).Show();
            }
        }
    }
}
