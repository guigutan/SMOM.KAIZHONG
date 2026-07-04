using SIE.EMS.EarlierStage.Projects;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 变更内容-界面
    /// </summary>
    public class ProjectChangeContentViewConfig : WebViewConfig<ProjectChangeContent>
    {
        /// <summary>
        /// 项目视图
        /// </summary>
        public const string ProjectView = "ProjectView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProjectView);
            if (ViewGroup == ProjectView)
            {
                ConfigProjectView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.ChangeOperate).ShowInList(80);
            View.Property(p => p.ChangeType).ShowInList(80);
            View.Property(p => p.ChangeExplain).ShowInList(1000);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 项目视图
        /// </summary>
        protected void ConfigProjectView()
        {
            View.AssignAuthorize(typeof(Project));
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectChangeNo).ShowInList(150);
                View.Property(p => p.ChangeOperate).ShowInList(80);
                View.Property(p => p.ChangeType).ShowInList(80);
                View.Property(p => p.ChangeExplain).ShowInList(700);
                View.Property(p => p.CreateByName).HasLabel("申请人").ShowInList();
                View.Property(p => p.CreateDate).HasLabel("申请时间").ShowInList(150);
                View.Property(p => p.ApprovalStatus).ShowInList(80);
                View.Property(p => p.ApprovalTime).ShowInList(150);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
