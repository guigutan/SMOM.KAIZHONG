using SIE.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports
{
    /// <summary>
    /// 异常时效看板视图配置
    /// </summary>
    internal class TimelinessAbnormityReportViewConfig : WebViewConfig<TimelinessAbnormityReportsViewModel>
    {

        protected override void ConfigListView()
        {
            View.Property(p => p.TaskState);

        }
    }
}