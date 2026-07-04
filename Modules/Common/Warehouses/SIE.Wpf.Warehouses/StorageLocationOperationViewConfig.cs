using SIE.Warehouses;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 库区操作管理 视图配置
    /// </summary>
    internal class StorageLocationOperationViewConfig : WPFViewConfig<StorageLocationOperation>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(StorageLocationItemList.IdProperty);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(9);
            using (View.OrderProperties())
            {
                View.Property(p => p.UpProcess).UseFormSetting(p => p.ColumnsSpan = 2);
                View.Property(p => p.PickProcess).UseFormSetting(p => p.ColumnsSpan = 2);
                View.Property(p => p.IsLayIn).UseFormSetting(p => p.ColumnsSpan = 1);
                View.Property(p => p.IsPick).UseFormSetting(p => p.ColumnsSpan = 1);
                View.Property(p => p.IsFocus).UseFormSetting(p => p.ColumnsSpan = 1);
                View.Property(p => p.IsTemporary).UseFormSetting(p => p.ColumnsSpan = 1);
                View.Property(p => p.UpOrderIndex).UseFormSetting(p => p.ColumnsSpan = 2);
                View.Property(p => p.PickOrderIndex).UseFormSetting(p => p.ColumnsSpan = 2);
            }
        }
    }
}
