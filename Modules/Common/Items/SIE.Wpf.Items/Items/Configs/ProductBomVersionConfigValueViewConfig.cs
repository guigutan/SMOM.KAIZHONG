using SIE.Items.Configs;

namespace SIE.Wpf.Items.Items.Configs
{
    /// <summary>
    /// 产品BOM版本配置值视图配置
    /// </summary>
    class ProductBomVersionConfigValueViewConfig : WPFViewConfig<ProductBomVersionConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Version).Show(ShowInWhere.All);
        }
    }
}
