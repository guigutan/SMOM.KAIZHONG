using SIE.Resources.Configs;

namespace SIE.Wpf.Resources.Configs
{
    /// <summary>
    /// 资源编码值配置视图类
    /// </summary>
    public class ResourceNoConfigValueViewConfig : WPFViewConfig<ResourceNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRule).Show(ShowInWhere.All);
        }
    }
}