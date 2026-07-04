using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Wpf.Common;

namespace SIE.Wpf.Packages.Boxs.Configs
{
    /// <summary>
    /// RFID类型配置值视图配置
    /// </summary>
    class ProductTrunoverBoxTypeConfigValueViewConfig : WPFViewConfig<ProductTrunoverBoxTypeConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.BoxType).Show().UseCatalogEditor(p => p.CatalogType = TurnoverBox.BoxTypeCatalog);
        }
    }
}
