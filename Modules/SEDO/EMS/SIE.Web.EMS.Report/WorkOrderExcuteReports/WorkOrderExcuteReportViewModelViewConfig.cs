using SIE.EMS.Report.WorkOrderExcuteReports;

namespace SIE.Web.EMS.Report.WorkOrderExcuteReports
{
    /// <summary>
    /// 工单执行统计报表
    /// </summary>
    public class WorkOrderExcuteReportViewModelViewConfig : WebViewConfig<WorkOrderExcuteReportViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrderExcuteReportViewModel));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
        }
    }
}
