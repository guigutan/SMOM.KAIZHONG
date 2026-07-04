using SIE.Warehouses;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
	/// 库区操作管理视图配置
	/// </summary>
	internal class StorageAreaOperationViewConfig : WPFViewConfig<StorageAreaOperation>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.UpTransitLocation).UseAreaLocationLookUpEditor(p => p.ReloadDataOnPopping = true);
            View.Property(p => p.DownTransitLocation).UseAreaLocationLookUpEditor(p => p.ReloadDataOnPopping = true);
        }
    }
}
