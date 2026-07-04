using SIE.Tech.Processs;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序加工时长查询条件视图配置
    /// </summary>
    public class ProcessDurationCriteriaViewConfig : WebViewConfig<ProcessDurationCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Product).Show(ShowInWhere.All);
            View.Property(p => p.ProductName).Show(ShowInWhere.All);
            View.Property(p => p.Process).Show(ShowInWhere.All);
            View.Property(p => p.ProcessName).Show(ShowInWhere.All);
        }
    }
}