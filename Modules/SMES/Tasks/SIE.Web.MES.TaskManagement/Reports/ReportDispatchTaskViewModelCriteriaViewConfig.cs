using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Reports
{
    public class ReportDispatchTaskViewModelCriteriaViewConfig : WebViewConfig<ReportDispatchTaskViewModelCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // View.UseDefaultCommands();
            // View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.TaskManagement.Reports.Commands.ReportCriteriaCommand");
            View.UseCommand("SIE.Web.MES.TaskManagement.Reports.Commands.ReportCriteriaCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkShopId).Show(ShowInWhere.Detail);
                View.Property(p => p.ResourceId).Show(ShowInWhere.Detail);
                View.Property(p => p.ProductId).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderState).UseEnumEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.No).Show(ShowInWhere.Detail).HasLabel("任务单");
                View.Property(p => p.TaskStatus).UseEnumEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.WorkGroupId).Show(ShowInWhere.Detail);
                View.Property(p => p.EmployeeId).Show(ShowInWhere.Detail);
                View.Property(p => p.BeginTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/M/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Custom;
                });
            }
        }
    }
}
