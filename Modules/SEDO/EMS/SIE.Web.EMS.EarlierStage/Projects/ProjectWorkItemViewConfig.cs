using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.EarlierStage.Projects;
using SIE.EMS.Projects.Enums;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.EMS.EarlierStage.Projects.Commands;
using System;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 工作明细视图配置
    /// </summary>
    public class ProjectWorkItemViewConfig : WebViewConfig<ProjectWorkItem>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectWorkItemBehavior");
            View.UseCommands(typeof(StartWorkItemCommand).FullName, typeof(CompleteWorkItemCommand).FullName,
                WebCommandNames.Save, typeof(ImportWorkItemCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.WorkItem).ShowInList(400).Readonly();
            View.Property(p => p.PlanStart).UseDateEditor().ShowInList(150).Readonly();
            View.Property(p => p.PlantEnd).UseDateEditor().ShowInList(150).Readonly();
            View.Property(p => p.ActualStart).UseDateEditor(p =>
            {
                p.Format = "Y-m-d";
                p.MaxValue = DateTime.Today.ToString();
            }).Readonly(p => p.ProjectStatus == ProjectStatus.Closed || p.WorkStatus == WorkStatus.NotStarted).ShowInList(150);
            View.Property(p => p.ActualStartPeopleId).Readonly();
            View.Property(p => p.ActaulEnd).UseDateEditor(p =>
            {
                p.Format = "Y-m-d";
                p.MaxValue = DateTime.Today.ToString();
            }).Readonly(p => p.ProjectStatus == ProjectStatus.Closed || p.WorkStatus == WorkStatus.NotStarted).ShowInList(150);
            View.Property(p => p.ActaulEndPeopleId).Readonly();
            View.Property(p => p.WorkStatus).ShowInList(80).Readonly();
            View.Property(p => p.PrincipalId).ShowInList(80).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Project.Code).HasLabel("项目编码");
            View.Property(p => p.WorkItem);
            View.Property(p => p.PlanStart);
            View.Property(p => p.PlantEnd);
            View.PropertyRef(p => p.Principal.Code).HasLabel("责任人");
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        void ConfigEditView()
        {
            View.AssignAuthorize(typeof(Project));
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkItem).ShowInList(400);
                View.Property(p => p.PlanStart).HasLabel("计划开始时间".L10N()+"*").UseDateEditor().ShowInList(150);
                View.Property(p => p.PlantEnd).HasLabel("计划结束时间".L10N() + "*").UseDateEditor().ShowInList(150);
                View.Property(p => p.ActualStart).UseDateEditor().Readonly().ShowInList(150);
                View.Property(p => p.ActualStartPeopleId).Readonly();
                View.Property(p => p.ActaulEnd).UseDateEditor().Readonly().ShowInList(150);
                View.Property(p => p.ActaulEndPeopleId).Readonly();
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