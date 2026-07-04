using SIE.Items.Items.Configs;

namespace SIE.Web.Items.Items.Configs
{
    /// <summary>
    /// 物料编码 视图配置
    /// </summary>
    public class ItemCodeNoConfigValueViewConfig : WebViewConfig<ItemCodeNoConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCodeRule).UsePagingLookUpEditor(p => { p.DisplayField = "Name"; }).Show(ShowInWhere.All);
            }
        }
    }
}
