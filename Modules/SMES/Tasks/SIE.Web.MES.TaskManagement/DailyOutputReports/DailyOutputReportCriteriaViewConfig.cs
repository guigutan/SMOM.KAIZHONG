using SIE.MES.TaskManagement.DailyOutputReports;

namespace SIE.Web.MES.TaskManagement.DailyOutputReports
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class DailyOutputReportCriteriaViewConfig : WebViewConfig<DailyOutputReportCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ResourceCode).Show();
                View.Property(p => p.ResourceName).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.Process).Show();
                //View.Property(p => p.WorkOrder).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.Division).Show();
                View.Property(p => p.Department).Show();
                View.Property(p => p.PlanBeginTime).Show().HasLabel("日期").UseDateRangeEditor(p =>
                {
                    p.DateFormat = "Y-m-d";
                    p.DateRangeType = ObjectModel.DateRangeType.Today;
                }).Show();
            }
        }
    }
}
