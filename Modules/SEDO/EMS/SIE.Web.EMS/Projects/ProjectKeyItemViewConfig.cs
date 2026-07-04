using SIE.EMS.Projects;

namespace SIE.Web.EMS.Projects
{
    /// <summary>
    /// 项目关键事项视图配置
    /// </summary>
    internal class ProjectKeyItemViewConfig : WebViewConfig<ProjectKeyItem>
    {
        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Description).ShowInList(200);
            View.Property(p => p.BudgetId).HasLabel("预算编码").ShowInList(160);
            View.Property(p => p.BudgetName).ShowInList(200);
            View.Property(p => p.CanUseAmount).ShowInList(130);
            View.Property(p => p.BudgetAmount).ShowInList(130);
        }
    }
}
