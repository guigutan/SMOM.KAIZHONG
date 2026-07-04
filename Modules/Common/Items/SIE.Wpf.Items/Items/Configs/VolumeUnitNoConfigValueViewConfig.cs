using SIE.Items.Items.Configs;
using SIE.Wpf.Common;

namespace SIE.Wpf.Items.Items.Configs
{
    /// <summary>
    /// 体积单位 视图配置
    /// </summary>
    public class VolumeUnitNoConfigValueViewConfig : WPFViewConfig<VolumeUnitNoConfigValue>
    {
        /// <summary>
        /// 默认视图 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.VolumeTypeCode).UseCatalogEditor(p => p.CatalogType = "UNIT_TYPE").Show(ShowInWhere.All);
            }
        }
    }
}
