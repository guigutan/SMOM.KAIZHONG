using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Projects;
using SIE.Equipments.WorkFlows;
using SIE.Resources.Employees;
using SIE.Web.Common;
using SIE.Web.Core;
using SIE.Web.EMS.EarlierStage.Projects.Commands;
using SIE.Web.EMS.EarlierStage.WorkFlows;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目视图配置
    /// </summary>
    public class ProjectViewConfig : WebViewConfig<Project>
    {
        /// <summary>
        /// 附加查看视图
        /// </summary>
        public const string SeeView = "SeeView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SeeView);
            if (ViewGroup == SeeView)
            {
                ConfigSeeView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectListBehavior");
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Common.Scripts.ApprovalBehavior");
            View.UseCommands("SIE.Web.EMS.EarlierStage.Projects.Commands.AddProjectCommand", "SIE.Web.EMS.EarlierStage.Projects.Commands.EditProjectCommand",
                typeof(DeleteProjectCommand).FullName, typeof(SubmitProjectCommand).FullName, typeof(CancelProjectCommand).FullName, typeof(ExamineProjectCommand).FullName,
                typeof(ProjectImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand", "SIE.Web.EMS.EarlierStage.Projects.Commands.ExportProjectCommand",
                "SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectChangeCommand", "SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectCloseCommand");
            View.Property(p => p.FactoryId).ShowInList(120).FixColumn();
            View.Property(p => p.DepartmentId).ShowInList(120).FixColumn();
            View.Property(p => p.Code).ShowInList(160);
            View.Property(p => p.Name).ShowInList(200);
            View.Property(p => p.ProjectStatus).ShowInList(80);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.PlanType).ShowInList(80);
            View.Property(p => p.Year).UseYearEditor().ShowInList(80);
            View.Property(p => p.ProjectType).UseCatalogEditor(e => { e.CatalogType = Project.ProjectClassify;e.CatalogReloadData = true; }).ShowInList(120);
            View.Property(p => p.PrincipalId).ShowInList(100);
            View.Property(p => p.Amount).HasLabel("项目预算").ShowInList(130);
            View.Property(p => p.ActualAmount).ShowInList(130);
            View.Property(p => p.LaborCost).ShowInList(80);
            View.Property(p => p.ProjectDate).UseDateEditor().ShowInList(130);
            View.Property(p => p.ParentProjectId).HasLabel("父项目编码").ShowInList(160);
            View.Property(p => p.ParentProjectName).ShowInList(200);
            View.Property(p => p.ContentAndBasis).ShowInList(220);
            View.Property(p => p.GoalAndBenefit).ShowInList(220);
            View.Property(p => p.Remark).ShowInList(220);
            View.Property(p => p.InitialAcceptanceDate).ShowInList(150);
            View.Property(p => p.AcceptanceDate).ShowInList(150);
            View.Property(p => p.WarrantyAcceptanceDate).ShowInList(150);
            View.ChildrenProperty(p => p.KeyItemList).UseViewGroup(ProjectKeyItemViewConfig.ProjectSeeView).HasLabel("项目事项").HasOrderNo(2);
            View.ChildrenProperty(p => p.ProjectMemberList).HasLabel("项目成员").HasOrderNo(3);
            View.ChildrenProperty(p => p.ProjectWorkItemList).HasLabel("项目计划").Show( ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(ProjectWorkItem), (e) =>
            {
                var parent = e.Parent as Project;
                if (parent == null)
                {
                    return new EntityList<ProjectWorkItem>();
                }
                return RT.Service.Resolve<ProjectController>().GetWorkItemsByProjectIds(parent.Id);
            }).HasLabel("项目计划").HasOrderNo(1);

            View.AttachChildrenProperty(typeof(ProjectChangeContent), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Project>();
                if (parent == null)
                {
                    return new EntityList<ProjectChangeContent>();
                }
                return RT.Service.Resolve<ProjectChangeController>().GetChangeContentsByProId(parent.Id, args.PagingInfo);
            }, ProjectChangeContentViewConfig.ProjectView).HasLabel("变更记录").HasOrderNo(5);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Project>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(Project).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.SeeView).HasLabel("审核记录").HasOrderNo(6);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("项目附件").HasOrderNo(7);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectFormBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(SaveProjectCommand).FullName);
            View.UseDetail(8);
            View.Property(p => p.FactoryId).ShowInDetail(columnSpan: 2).UseFactoryEditor().Cascade(p => p.DepartmentId, null).Cascade(p => p.ParentProjectId, null);
            View.Property(p => p.DepartmentId).ShowInDetail(columnSpan: 2).UseUserBudgetDepartmentEditor().Cascade(p => p.ParentProjectId, null);
            View.Property(p => p.Year).ShowInDetail(columnSpan: 2).UseYearEditor();
            View.Property(p => p.ProjectDate).UseDateEditor().ShowInDetail(columnSpan: 2);
            View.Property(p => p.Code).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.Name).ShowInDetail(columnSpan: 4);
            View.Property(p => p.PlanType).ShowInDetail(columnSpan: 2);
            View.Property(p => p.ProjectType).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Project.ProjectClassify; e.CatalogReloadData = true; });
            View.Property(p => p.PrincipalId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).ShowInDetail(columnSpan: 2);
            View.Property(p => p.ParentProjectId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var proj = source as Project;
                if (proj == null)
                {
                    return new EntityList<Project>();
                }
                return RT.Service.Resolve<ProjectController>().GetParentProjects(proj.FactoryId, proj.DepartmentId, pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ParentProjectName), nameof(e.ParentProject.Name));
                m.DicLinkField = keyValues;
            }).ShowInDetail(columnSpan: 2).HasLabel("父项目编码");
            View.Property(p => p.ParentProjectName).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.Amount).Readonly().UseSpinEditor(p => p.DecimalPrecision = 2).ShowInDetail(columnSpan: 2);
            View.Property(p => p.ContentAndBasis).ShowInDetail(columnSpan: 6);
            View.Property(p => p.GoalAndBenefit).ShowInDetail(columnSpan: 4);
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 4);
            View.ChildrenProperty(p => p.KeyItemList).UseViewGroup(ProjectKeyItemViewConfig.EditView).HasLabel("项目事项").HasOrderNo(2);
            View.ChildrenProperty(p => p.ProjectMemberList).UseViewGroup(ProjectMemberViewConfig.EditView).HasLabel("项目成员").HasOrderNo(3);
            View.ChildrenProperty(p => p.ProjectWorkItemList).UseViewGroup(ProjectWorkItemViewConfig.EditView).HasLabel("项目计划").HasOrderNo(1);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList(160);
            View.Property(p => p.Name).ShowInList(200);
            View.Property(p => p.ProjectStatus).ShowInList(80);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.FactoryNameImport).HasLabel("工厂编码");
            View.Property(p => p.DepartmentNameImport).HasLabel("部门编码");
            View.Property(p => p.ProjectYear);
            View.Property(p => p.ProjectDate);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.PlanType);
            View.Property(p => p.ProjectType).ImportCatalogType(Project.ProjectClassify, Catalog.NameProperty.Name);
            View.Property(p => p.PrincipalCodeImport).HasLabel("项目负责人编码");
            View.PropertyRef(p => p.ParentProject.Code).HasLabel("父项目编码");
            View.Property(p => p.ContentAndBasis);
            View.Property(p => p.GoalAndBenefit);
            View.Property(p => p.Remark);
        }

        /// <summary>
        /// 附加查看视图
        /// </summary>
        void ConfigSeeView()
        {
            View.AssignAuthorize(typeof(Budget));
            View.AssignAuthorize(typeof(ProjectKeyItem));
            View.DisableEditing();
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(160);
                View.Property(p => p.Name).ShowInList(200);
                View.Property(p => p.ProjectStatus).ShowInList(80);
                View.Property(p => p.ProjectType).UseCatalogEditor(e => { e.CatalogType = Project.ProjectClassify;e.CatalogReloadData = true; }).ShowInList(120);
                View.Property(p => p.PrincipalId).ShowInList(100);
                View.Property(p => p.Amount).HasLabel("项目预算").ShowInList(130);
                View.Property(p => p.ActualAmount).ShowInList(130);
                View.Property(p => p.ProjectDate).UseDateEditor().ShowInList(130);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.KeyItemList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProjectMemberList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProjectWorkItemList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}