using SIE.MES.FeedingIntervals;


namespace SIE.Web.MES.FeedingIntervals
{
    /// <summary>
    /// 上料间隔时间 WEB查询界面 配置
    /// </summary>
    internal class FeedingIntervalCriteriaViewConfig : WebViewConfig<FeedingIntervalCriteria>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList(width: 150);
            }
        }

    }
}
