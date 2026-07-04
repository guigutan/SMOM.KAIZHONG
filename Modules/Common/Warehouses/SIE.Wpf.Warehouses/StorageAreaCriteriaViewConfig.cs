using SIE.Warehouses;

namespace SIE.WPF.Warehouses
{
    /// <summary>
    /// 库区查询视图配置
    /// </summary>
    internal class StorageAreaCriteriaViewConfig : WPFViewConfig<StorageAreaCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.LibraryType).Show(ShowInWhere.All);
                View.Property(p => p.Warehouse).Show(ShowInWhere.All);
                View.Property(p => p.IsFrozen).Show(ShowInWhere.All).UseCheckDropDownEditor();
            }
        }
    }
}
