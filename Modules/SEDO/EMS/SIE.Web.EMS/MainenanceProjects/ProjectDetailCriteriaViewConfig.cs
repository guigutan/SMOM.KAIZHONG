using SIE.EMS.MainenanceProjects;
using SIE.Web.Common;

namespace SIE.Web.EMS.Projects
{
    /// <summary>
    /// 点检保养项目查询视图
    /// </summary>
    public class ProjectDetailCriteriaViewConfig : WebViewConfig<ProjectDetailCriteria>
    {
        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Name).HasLabel("项目名称").Show(ShowInWhere.All);
                View.Property(p => p.ProjectType).Show(ShowInWhere.All).Readonly(p => p.IsReadonly);
                View.Property(p => p.CycleType).Show(ShowInWhere.All);
                View.Property(p => p.Part).Show(ShowInWhere.All);
                
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
