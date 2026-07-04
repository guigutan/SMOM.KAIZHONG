using SIE.MES.WIP.Packings.Configs;

namespace SIE.Wpf.MES.WIP.Packings.Configs
{
    /// <summary>
    /// 包装采集配置值视图配置
    /// </summary>
    internal class WipPackingConfigValueViewConfig : WPFViewConfig<WipPackingConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.AutoDoPackingMode).Show(ShowInWhere.All);
                View.Property(p => p.IsAutoPrintPackageLabel).Show(ShowInWhere.All);
                View.Property(p => p.IsContinuityControl).Show(ShowInWhere.All);
                View.Property(p => p.PackingRuleValidMode).Show(ShowInWhere.All).HasLabel("验证方式");
            }
        }
    }
}