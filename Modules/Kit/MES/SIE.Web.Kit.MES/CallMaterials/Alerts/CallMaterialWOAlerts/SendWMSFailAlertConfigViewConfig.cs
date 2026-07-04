using SIE.Kit.MES.CallMaterials.Alerts;

namespace SIE.Web.Kit.MES.CallMaterials.Alerts.CallMaterialWOAlerts
{
    /// <summary>
    /// 工单叫料发送WMS失败预警插件配置视图配置类
    /// </summary>
    internal class SendWMSFailAlertConfigViewConfig : WebViewConfig<SendWMSFailAlertConfig>
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
                View.Property(p => p.Line)/*.UseWipResourceCodeLookUpEditor(p => { p.ValueMember = WipResource.CodeProperty.Name; p.DisplayMember = WipResource.NameProperty.Name; }).Show(ShowInWhere.All)*/;
            }
        }

        /// <summary>
        /// 明细视图配置
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
