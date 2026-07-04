using SIE.ERPInterface.Common.Logs;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 接口任务时间戳查询实体配置视图
    /// </summary>
    public class DownloadJobTimeCriteriaViewConfig : WebViewConfig<DownloadJobTimeCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
			using (View.OrderProperties())
			{
                View.Property(p => p.JobType).Show();
                View.Property(p => p.RequestDate).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; }).Show();
            }
		}
    }
}