using SIE.Resources.WipResources;

namespace SIE.Wpf.Resources.WipResources
{
    /// <summary>
    /// 生产资源查询视图配置类
    /// </summary>
    public class WipResourceCriteriaViewConfig : WPFViewConfig<WipResourceCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.SourceType).Show(ShowInWhere.All);
                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.ProcessTechType).Show(ShowInWhere.All);
                View.Property(p => p.CalendarScheme).UseSchemeLookUpEditor().Show(ShowInWhere.All);
                View.Property(p => p.WorkShop).UseShopEditor().Show(ShowInWhere.All);
                View.Property(p => p.Factory).UseFactoryEditor().Show(ShowInWhere.All);
            }
        }
    }
}
