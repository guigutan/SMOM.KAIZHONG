using SIE.EMS.AlertPlugs;

namespace SIE.Web.EMS.Alerts
{
    /// <summary>
    /// 设备保养任务超时提醒视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class MaintainTimeOutAlertConfigViewConfig : WebViewConfig<EmsMaintainTimeOutAlertPlugConfig>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Enterprise);
                View.Property(p => p.Process);
                View.Property(p => p.EarlyStartDate).UseTimeEditor();
                View.Property(p => p.NightStartDate).UseTimeEditor();
                View.Property(p => p.TimeOut);
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
                View.Property(p => p.Enterprise);
                View.Property(p => p.Process);
                View.Property(p => p.EarlyStartDate);
                View.Property(p => p.NightStartDate);
                View.Property(p => p.TimeOut);
            }
        }
    }
}
