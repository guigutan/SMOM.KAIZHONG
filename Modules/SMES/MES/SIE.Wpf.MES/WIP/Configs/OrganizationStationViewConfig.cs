using SIE.MES.WIP.Configs;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class OrganizationStationViewConfig : WPFViewConfig<ResourceStation>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.Level).Show(ShowInWhere.All);
            }
        }
    }
}