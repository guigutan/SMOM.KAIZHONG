using SIE.MES.WorkOrders.Configs;

namespace SIE.Wpf.MES.WorkOrders.Configs
{
    /// <summary>
    /// 打印设置配置值视图配置
    /// </summary>
    internal class PrintTemplateConfigValueViewConfig : WPFViewConfig<PrintTemplateConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsNeed).Show(ShowInWhere.All);
            }
        }
    }
}
