using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Web.Common;

namespace SIE.Web.Packages.Boxs.Configs
{
    /// <summary>
    /// RFID类型配置值视图配置
    /// </summary>
    class ProductTrunoverBoxTypeConfigValueViewConfig : WebViewConfig<ProductTrunoverBoxTypeConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.BoxType).Show()
                .UseCatalogEditor(p => { p.CatalogType = TurnoverBox.BoxTypeCatalog;p.CatalogReloadData = true; })
                .UseListSetting(e => { e.HelpInfo = "周转箱快码类型(BOX_TYPE)"; });
        }
    }
}
