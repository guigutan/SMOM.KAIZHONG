using SIE.Resources.ProcessTechs;

namespace SIE.Wpf.Resources.ProcessTechs
{
    /// <summary>
    /// 制程查询视图
    /// </summary>
    public class ProcessTechCriteriaViewConfig : WPFViewConfig<ProcessTechCriteria>
    {
        /// <summary>
        /// 列表逻辑视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.ProcessTechType).Show(ShowInWhere.All);
                //View.Property(p => p.ProcessTechState).Show(ShowInWhere.All);
            }
        }
    }
}
