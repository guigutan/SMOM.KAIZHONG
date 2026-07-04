using SIE.Warehouses.Configs;

namespace SIE.Wpf.Warehouses.Configs
{
    /// <summary>
    /// 库位配置 视图
    /// </summary>
    class StorageLocationCodeConfigValueViewConfig : WPFViewConfig<StorageLocationCodeConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.StorageLocationCodeRule).Show(ShowInWhere.All);
                View.Property(p => p.StorageLocationPrintRule).UsePrintTemplateEditor(p => p.ReloadDataOnPopping = true).Show(ShowInWhere.All);
            }
        }
    }
}
