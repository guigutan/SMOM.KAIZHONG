using SIE.MES.WIP.Packings.Configs;

namespace SIE.Wpf.MES.WIP.Packings.Configs
{
    /// <summary>
    /// 是否称重配置值视图配置
    /// </summary>
    class WeightConfigValueViewConfig : WPFViewConfig<WeightConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsWeight);
        }
    }
}