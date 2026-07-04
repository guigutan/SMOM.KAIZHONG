using SIE.Warehouses;

namespace SIE.WPF.Warehouses
{
    /// <summary>
    /// 库区查询视图配置
    /// </summary>
    internal class StorageLocationCriteriaVierConfig : WPFViewConfig<StorageLocationCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("库区");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.LibraryType).Show(ShowInWhere.All);
                View.Property(p => p.Warehouse).Show(ShowInWhere.All);
                View.Property(p => p.Area).Show(ShowInWhere.All);
                View.Property(p => p.IsFrozen).Show(ShowInWhere.All).UseCheckDropDownEditor();
                View.Property(p => p.ErpInvOrg).Show(ShowInWhere.All);
                View.Property(p => p.ErpSubLibrary).Show(ShowInWhere.All);
                View.Property(p => p.ErpLocation).Show(ShowInWhere.All);
            }
        }
    }
}
