namespace SIE.Wpf.Packages.Packings
{
    /// <summary>
    /// 工作站视图配置
    /// </summary>
    public class WorkstationViewConfig : WPFViewConfig<Workstation>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDetailColumnsCount(4);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.User);
                View.Property(p => p.Warehouse);
                View.Property(p => p.StorageArea);
                View.Property(p => p.StorageLocation);
            }
        }
    }
}