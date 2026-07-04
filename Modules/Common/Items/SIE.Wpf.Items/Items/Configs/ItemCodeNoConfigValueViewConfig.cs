using SIE.Items.Items.Configs;

namespace SIE.Wpf.Items.Items.Configs
{
    /// <summary>
    /// 物料编码 视图配置
    /// </summary>
    public class ItemCodeNoConfigValueViewConfig : WPFViewConfig<ItemCodeNoConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCodeRule).Show(ShowInWhere.All);
            }
        }
    }
}
