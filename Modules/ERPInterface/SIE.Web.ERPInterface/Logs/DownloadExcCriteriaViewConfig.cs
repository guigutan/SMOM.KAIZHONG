using SIE.ERPInterface.Common.Logs;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 接口下载异常报表查询视图
    /// </summary>
    public class DownloadExcCriteriaViewConfig : WebViewConfig<DownloadExcViewModelCriteria>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.ERPInterface.Common.Commands.ReportQueryCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.JobType).Show();
                View.Property(p => p.LogDate).Show();
            }
        }
    }
}
