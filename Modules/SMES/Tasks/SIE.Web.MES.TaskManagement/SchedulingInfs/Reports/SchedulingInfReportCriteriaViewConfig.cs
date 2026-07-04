using SIE.MES.TaskManagement.SchedulingInfs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.Reports
{
    public class SchedulingInfReportCriteriaViewConfig : WebViewConfig<SchedulingInfReportCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsImport).Show();
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ProcessCode).Show();
                View.Property(p => p.ProcessName).Show();
                View.Property(p => p.Mrb).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.UpdateDate).Show().UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
                });
                View.Property(p => p.ImportTime).Show().UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                });
                View.Property(p => p.IsSchedulingInfReturn).Show();
                View.Property(p => p.IsCheck).Show();
                View.Property(p => p.IsGenerateTask).Show();
            }
        }
    }
}
