using SIE.Warehouses.Configs;

namespace SIE.Web.Warehouses.Configs
{
    /// <summary>
    /// 库位配置 视图
    /// </summary>
    class StorageLocationCodeConfigValueViewConfig : WebViewConfig<StorageLocationCodeConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.StorageLocationCodeRule).Show(ShowInWhere.All);
                View.Property(p => p.StorageLocationPrintRule).UsePrintTemplateEditor().Show(ShowInWhere.All)
                .UseListSetting(e => { e.HelpInfo = "显示当前编码规则的打印模板"; });
            }
        }
    }
}
