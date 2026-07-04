using SIE.Items.Configs;

namespace SIE.Web.Items.Items.Configs
{
    /// <summary>
    /// 产品BOM版本配置值视图配置
    /// </summary>
    class ProductBomVersionConfigValueViewConfig : WebViewConfig<ProductBomVersionConfigValue>
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
