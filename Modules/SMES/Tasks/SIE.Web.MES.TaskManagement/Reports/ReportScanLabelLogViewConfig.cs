using SIE.MES.TaskManagement.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Reports
{
    public class ReportScanLabelLogViewConfig : WebViewConfig<ReportScanLabelLog>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderId).Show().Readonly();
                View.Property(p => p.DispatchTaskId).Show().Readonly().HasLabel("派工单");
                View.Property(p => p.ProcessId).Show().Readonly();
                View.Property(p => p.LabelNo).Show().Readonly();
                View.Property(p => p.LabelQty).Show().Readonly();
                View.Property(p => p.GoodQty).Show().Readonly();
                View.Property(p => p.SuspectQty).Show().Readonly();
                View.Property(p => p.Remark).Show().Readonly();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderId).Show();
                View.Property(p => p.DispatchTaskId).Show().HasLabel("派工单");
                View.Property(p => p.ProcessId).Show();
                View.Property(p => p.LabelNo).Show();
                View.Property(p => p.LabelQty).Show();
                View.Property(p => p.GoodQty).Show();
                View.Property(p => p.SuspectQty).Show();
                View.Property(p => p.Remark).Show();
            }
        }

    }
}
