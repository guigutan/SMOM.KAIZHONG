using SIE.MES.Workbench.Experiences;

namespace SIE.Wpf.MES.Workbench.Experiences
{
    /// <summary>
    /// 查询界面
    /// </summary>
    internal class ExperienceItemModelCriteriaViewConfig : WPFViewConfig<ExperienceItemCriteria>
    {
        /// <summary>
        /// 查询
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Code).ShowInDetail();
            View.Property(p => p.Name).ShowInDetail();
            View.Property(p => p.Type).ShowInDetail();
        }
    }
}
