using SIE.MES.WorkOrders.Configs;

namespace SIE.Wpf.MES.WorkOrders.Configs
{
    /// <summary>
    /// 生成工序BOM配置值视图配置
    /// </summary>
    class GenerateProcessBomsConfigValueViewConfig : WPFViewConfig<GenerateProcessBomsConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.YesNo).Show(ShowInWhere.All);
            }
        }
    }
}
