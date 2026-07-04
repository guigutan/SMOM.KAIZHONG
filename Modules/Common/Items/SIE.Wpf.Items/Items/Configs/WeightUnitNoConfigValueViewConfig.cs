using SIE.Items.Items.Configs;
using SIE.Wpf.Common;

namespace SIE.Wpf.Items.Items.Configs
{
    /// <summary>
    /// 重量单位 视图配置
    /// </summary>
    public class WeightUnitNoConfigValueViewConfig : WPFViewConfig<WeightUnitNoConfigValue>
    {
        /// <summary>
        /// 默认视图 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WeightTypeCode).UseCatalogEditor(p => p.CatalogType = "UNIT_TYPE").Show(ShowInWhere.All);
            }
        }
    }
}
