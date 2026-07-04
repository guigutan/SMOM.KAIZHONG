using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库区操作管理 视图配置
    /// </summary>
    internal class StorageLocationOperationViewConfig : WebViewConfig<StorageLocationOperation>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.IsLayIn).DefaultValue(true).ShowInDetail(columnSpan: 1);
                View.Property(p => p.IsPick).DefaultValue(true).ShowInDetail(columnSpan: 1);
                View.Property(p => p.IsFocus).ShowInDetail(columnSpan: 1);
                View.Property(p => p.IsTemporary).ShowInDetail(columnSpan: 1);
                View.Property(p => p.UpProcess).ShowInDetail(columnSpan: 1);
                View.Property(p => p.UpOrderIndex).DefaultValue(1).ShowInDetail(columnSpan: 1);
                View.Property(p => p.PickProcess).ShowInDetail(columnSpan: 1);
                View.Property(p => p.PickOrderIndex).DefaultValue(1).ShowInDetail(columnSpan: 1);
            }
        }
    }
}
