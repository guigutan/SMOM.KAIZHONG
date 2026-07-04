using SIE.Kit.MES.CallMaterials.Alerts;

namespace SIE.Web.Kit.MES.CallMaterials.Alerts.CallMaterialBillAlerts
{
    /// <summary>
    /// 工叫料单配送超时邮件推送插件配置视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class DeliveryDelayEmailSenderConfigViewConfig : WebViewConfig<DeliveryDelayEmailSenderConfig>
    {
        /// <summary>
        /// 基本视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 基本视图配置
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Host);
                View.Property(p => p.Port);
                View.Property(p => p.UserName);
                View.Property(p => p.Password).UsePasswordEditor();
                View.Property(p => p.SendFrom);
                View.Property(p => p.SendFromDisplayName);
                View.Property(p => p.EnableSSL);
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Host).Show(ShowInWhere.All);
                View.Property(p => p.Port).Show(ShowInWhere.All);
                View.Property(p => p.UserName).Show(ShowInWhere.All);
                View.Property(p => p.Password).UsePasswordEditor().Show(ShowInWhere.All);
                View.Property(p => p.SendFrom).Show(ShowInWhere.All);
                View.Property(p => p.SendFromDisplayName).Show(ShowInWhere.All);
                View.Property(p => p.EnableSSL).Show(ShowInWhere.All);
            }
        }
    }
}
