using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Reports
{
    public class WeightOfSamplingReportCriteriaViewConfig:WebViewConfig<WeightOfSamplingReportCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.Process).Show();
            }
        }
    }
}
