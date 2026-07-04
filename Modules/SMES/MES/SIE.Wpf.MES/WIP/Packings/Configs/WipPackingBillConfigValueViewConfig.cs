using SIE.MES.WIP.Packings.Configs;

namespace SIE.Wpf.MES.WIP.Packings.Configs
{
    /// <summary>
    /// 包装生成单据配置值视图配置
    /// </summary>
    internal class WipPackingBillConfigValueViewConfig : WPFViewConfig<WipPackingBillConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Warehouse).Show(ShowInWhere.All);
            }
        }
    }
}
