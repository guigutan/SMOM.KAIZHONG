using SIE.ESop.Displays;
using SIE.Wpf.Resources;

namespace SIE.Wpf.ESop.Displays
{
    /// <summary>
    /// 显示点视图配置
    /// </summary>
    internal class DisplayPointCriteriaViewConfig : WPFViewConfig<DisplayPointCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.Resource).UseEnterpriseResourceEditor().Show(ShowInWhere.All);
                View.Property(p => p.Process).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Resource).UseEnterpriseResourceEditor();
            View.Property(p => p.Process);
        }
    }
}