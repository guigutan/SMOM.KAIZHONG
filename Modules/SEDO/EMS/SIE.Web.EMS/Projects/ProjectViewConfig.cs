using SIE.EMS.Projects;

namespace SIE.Web.EMS.Projects
{
    /// <summary>
    /// 项目视图配置
    /// </summary>
    internal class ProjectViewConfig : WebViewConfig<Project>
    {
        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList(160);
            View.Property(p => p.Name).ShowInList(200);
            View.Property(p => p.Amount).ShowInList(80);
            View.Property(p => p.Principal).ShowInList(100);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
        }
    }
}
