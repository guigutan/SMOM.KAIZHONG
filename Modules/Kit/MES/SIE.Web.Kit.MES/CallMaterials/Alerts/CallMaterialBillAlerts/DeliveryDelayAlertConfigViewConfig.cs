using SIE.Kit.MES.CallMaterials.Alerts;

namespace SIE.Web.Kit.MES.CallMaterials.Alerts.CallMaterialBillAlerts
{
    /// <summary>
    /// 叫料单配送超时预警插件配置试图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class DeliveryDelayAlertConfigViewConfig : WebViewConfig<DeliveryDelayAlertConfig>
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
                View.Property(p => p.Line)/*.UseWipResourceCodeLookUpEditor(p => { p.ValueMember = WipResource.CodeProperty.Name; p.DisplayMember = WipResource.NameProperty.Name; }).Show(ShowInWhere.All)*/;
            }
        }
    }
}
