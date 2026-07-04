using SIE.Core.Configs;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    ///  看板数据源配置项视图
    /// </summary>
    public class DashboardDataSourceConfigValueViewConfig : WebViewConfig<DashboardDataSourceConfigValue>
    {
        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.DashboardDataSourceType).Show();
        }
    }
}
