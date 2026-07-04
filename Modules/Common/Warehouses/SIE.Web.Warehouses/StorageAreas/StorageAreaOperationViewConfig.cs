using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库区操作管理视图配置
    /// </summary>
    internal class StorageAreaOperationViewConfig : WebViewConfig<StorageAreaOperation>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.UpTransitLocation).UseStorageLocationLookUpEditor()
                .UseListSetting(e => { e.HelpInfo = "根据当前库区显示库位列表"; });
            View.Property(p => p.DownTransitLocation).UseStorageLocationLookUpEditor()
                .UseListSetting(e => { e.HelpInfo = "根据当前库区显示库位列表"; });
        }
    }
}
