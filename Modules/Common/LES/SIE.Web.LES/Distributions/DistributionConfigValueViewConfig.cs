using SIE.LES.Distributions.Configs;

namespace SIE.Web.LES.Distributions
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class DistributionConfigValueViewConfig : WebViewConfig<DistributionConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.DistributionNoRule).Show();
                View.Property(p => p.IsNoDistribution).Show();
            }
        }
    }
}