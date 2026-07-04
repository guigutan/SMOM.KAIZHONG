using SIE.MES.TaskManagement.Completion;

namespace SIE.Web.MES.TaskManagement.Completion
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class ProductCompletionCriteriaViewConfig : WebViewConfig<ProductCompletionCriteria>
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
                View.Property(p => p.WorkOrder).Show();
                View.Property(p => p.MrpController).Show();
                View.Property(p => p.Classes).Show();
                View.Property(p => p.PlanBeginTime).Show().HasLabel("计划生产时间").UseDateRangeEditor(p =>
                {
                    p.DateFormat = "Y-m-d";
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                }).Show();
            }
        }
    }
}
