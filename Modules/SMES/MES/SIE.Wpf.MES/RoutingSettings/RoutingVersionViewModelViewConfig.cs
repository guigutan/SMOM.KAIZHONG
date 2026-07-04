using SIE.MES.RoutingSettings;

namespace SIE.Wpf.MES.RoutingSettings
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class RoutingVersionViewModelViewConfig : WPFViewConfig<RoutingVersionViewModel>
    {
        /// <summary>
        /// 工艺路线版本ViewModel的列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.VersionName);
            View.Property(p => p.IsDefault).UseEnumEditor();
        }
    }
}