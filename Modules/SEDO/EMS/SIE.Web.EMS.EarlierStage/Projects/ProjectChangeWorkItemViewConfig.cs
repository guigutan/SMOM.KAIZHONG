using SIE.EMS.EarlierStage.Projects;
using SIE.MetaModel.View;
using SIE.Resources.Employees;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更计划界面
    /// </summary>
    public class ProjectChangeWorkItemViewConfig : WebViewConfig<ProjectChangeWorkItem>
    {
        /// <summary>
        /// 查看视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadonlyView);
            if (ViewGroup == ReadonlyView)
                ConfigReadonlyView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.UseCommands(WebCommandNames.Add, "SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteKeyItemPlanCommand");
            View.Property(p => p.WorkItem).ShowInList(400);
            View.Property(p => p.PlanStart).HasLabel("计划开始时间*").ShowInList(150);
            View.Property(p => p.PlantEnd).HasLabel("计划结束时间*").ShowInList(150);
            View.Property(p => p.ActualStart).Readonly().ShowInList(150);
            View.Property(p => p.ActaulEnd).Readonly().ShowInList(150);
            View.Property(p => p.WorkStatus).Readonly().ShowInList(80);
            View.Property(p => p.PrincipalId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).ShowInList(80);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkItem).ShowInList(400).Readonly();
                View.Property(p => p.PlanStart).ShowInList(150).Readonly();
                View.Property(p => p.PlantEnd).ShowInList(150).Readonly();
                View.Property(p => p.ActualStart).Readonly().ShowInList(150);
                View.Property(p => p.ActaulEnd).Readonly().ShowInList(150);
                View.Property(p => p.WorkStatus).Readonly().ShowInList(80);
                View.Property(p => p.PrincipalId).ShowInList(80).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.WorkItem).ShowInList(200).Readonly();
            View.Property(p => p.PlanStart).UseDateEditor().ShowInList(150).Readonly();
            View.Property(p => p.PlantEnd).UseDateEditor().ShowInList(150).Readonly();
            View.Property(p => p.ActualStart).UseDateEditor().ShowInList(150).Readonly();
            View.Property(p => p.ActaulEnd).UseDateEditor().ShowInList(150).Readonly();
            View.Property(p => p.PrincipalId).ShowInList(80).HasLabel("责任人");
        }
    }
}
