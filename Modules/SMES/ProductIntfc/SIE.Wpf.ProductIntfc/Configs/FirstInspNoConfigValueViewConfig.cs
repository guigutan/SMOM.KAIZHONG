using SIE.ProductIntfc.Configs;

namespace SIE.Wpf.ProductIntfc.Configs
{
    /// <summary>
    /// 报检单号配置值视图配置
    /// </summary>
    internal class FirstInspNoConfigValueViewConfig : WPFViewConfig<FirstInspNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.NumberRule).Show(ShowInWhere.All);
        }
    }
}