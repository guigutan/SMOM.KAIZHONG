using SIE.MES.RoutingSettings;

namespace SIE.Wpf.MES.RoutingSettings
{
    /// <summary>
    /// 默认版本工艺路线视图类
    /// </summary>
    internal class DefaultRoutingViewModelViewConfig : WPFViewConfig<DefaultRoutingViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary> 
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseDetail(2);
            View.Property(p => p.DefaultVersion).UseDefaultRoutingDisplayEditor().ShowInDetail(height: 0, hideLabel: true, columnSpan: 2);
        }
    }
}