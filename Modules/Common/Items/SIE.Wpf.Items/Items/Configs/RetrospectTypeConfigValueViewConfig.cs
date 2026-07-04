using SIE.Items.Items.Configs;

namespace SIE.Wpf.Items.Items.Configs
{
    /// <summary>
    /// 产品追溯方式配置值视图
    /// </summary>
    public class RetrospectTypeConfigValueViewConfig : WPFViewConfig<RetrospectTypeConfigValue>
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
