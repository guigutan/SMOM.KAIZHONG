using SIE.Kit.MES.CallMaterials.Alerts;

namespace SIE.Web.Kit.MES.CallMaterials.Alerts.StationShortageAlerts
{
    /// <summary>
    /// 工位缺料预警插件配置视图类
    /// </summary>
    internal class StationShortageAlertConfigViewConfig : WebViewConfig<StationShortageAlertConfig>
    {
        /// <summary>
        /// 基本视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 基本视图配置
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Line)/*.UseWipResourceCodeLookUpEditor(p => { p.ValueMember = WipResource.CodeProperty.Name; p.DisplayMember = WipResource.NameProperty.Name; })*/.Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Line)/*.UseWipResourceCodeLookUpEditor(p => { p.ValueMember = WipResource.CodeProperty.Name; p.DisplayMember = WipResource.NameProperty.Name; })*/.Show(ShowInWhere.All);
            }
        }
    }
}
