using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库位冻结原因视图配置
    /// </summary>
    internal class StorageLocationFrozenReasonViewConfig : WebViewConfig<StorageLocationFrozenReason>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(StorageLocation));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.FrozenReason);
                View.Property(p => p.ReasonDesc);
            }
        }
    }
}
