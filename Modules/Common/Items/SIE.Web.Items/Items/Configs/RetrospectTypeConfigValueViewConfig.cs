using SIE.Items.Items.Configs;

namespace SIE.Web.Items.Items.Configs
{
    /// <summary>
    /// 产品追溯方式配置值视图
    /// </summary>
    internal class RetrospectTypeConfigValueViewConfig : WebViewConfig<RetrospectTypeConfigValue>
    {
        /// <summary>
        /// 配置试图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.RetrospectType).Show();
        }
    }
}