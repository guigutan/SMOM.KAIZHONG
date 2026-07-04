using SIE.MES.WIP.Configs;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 上料自动刷新视图配置
    /// </summary>
    class RefreshLoadItemConfigValueViewConfig : WPFViewConfig<RefreshLoadItemConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.RefreshTime).Show(ShowInWhere.All);
            }
        }
    }
}
