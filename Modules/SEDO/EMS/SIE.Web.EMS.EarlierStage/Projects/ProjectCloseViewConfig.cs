using SIE.Domain;
using SIE.EMS.EarlierStage.Projects;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Core;
using SIE.Web.EMS.EarlierStage.Projects.Commands;
using SIE.Web.EMS.EarlierStage.WorkFlows;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目结项-界面
    /// </summary>
    public class ProjectCloseViewConfig : WebViewConfig<ProjectClose>
    {
        /// <summary>
        /// 项目指标界面
        /// </summary>
        public const string NormView = "NormView";

        /// <summary>
        /// 项目编辑指标界面
        /// </summary>
        public const string EditNormView = "EditNormView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(NormView, EditNormView);
            if (ViewGroup == NormView)
            {
                ConfigNormView();
            }
            if (ViewGroup == EditNormView)
            {
                ConfigEditNormView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Common.Scripts.ApprovalBehavior");
            View.UseCommands(WebCommandNames.Add, "SIE.Web.EMS.EarlierStage.Projects.Commands.EditProjectCommand", typeof(DeleteProjectCloseCommand).FullName,
                typeof(SubmitProjectCloseCommand).FullName, typeof(CancelProjectCloseCommand).FullName, typeof(ExamineProjectCloseCommand).FullName,
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120).FixColumn();
            View.Property(p => p.DepartmentId).ShowInList(120).FixColumn();
            View.Property(p => p.ProjectId).HasLabel("项目编码").ShowInList(160).FixColumn();
            View.Property(p => p.ProjectName).ShowInList(200).FixColumn();
            View.Property(p => p.ApprovalStatus).ShowInList(80).FixColumn();
            View.Property(p => p.CloseItemType).ShowInList(80);
            View.Property(p => p.PlanType).ShowInList(80);
            View.Property(p => p.Year).UseYearEditor().ShowInList(80);
            View.Property(p => p.ProjectType).ShowInList(120);
            View.Property(p => p.PrincipalName).ShowInList(100);
            View.Property(p => p.Amount).ShowInList(130);
            View.Property(p => p.ActualAmount).ShowInList(130);
            View.Property(p => p.ProjectDate).UseDateEditor().ShowInList(120);
            View.Property(p => p.ParentProjectCode).ShowInList(160);
            View.Property(p => p.ParentProjectName).ShowInList(200);
            View.Property(p => p.ApprovalTime).ShowInList(150);
            View.AttachDetailChildrenProperty(typeof(ProjectClose), (c) =>
            {
                var model = c.Parent as ProjectClose;
                if (model == null)
                {
                    return model;
                }
                return RT.Service.Resolve<ProjectChangeController>().GetProjectCloseById(model.Id);
            }, NormView).Show(ChildShowInWhere.List).HasLabel("项目指标").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<ProjectClose>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(ProjectClose).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.SeeView).HasLabel("审核记录").HasOrderNo(2);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(3);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectCloseBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(SaveProjectCloseCommand).FullName);
            View.UseDetail(8);
            View.Property(p => p.FactoryName).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.DepartmentName).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.Year).ShowInDetail(columnSpan: 2).UseYearEditor().Readonly();
            View.Property(p => p.ProjectDate).HasLabel("立项时间").UseDateEditor().ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ProjectId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProjectController>().GetCloseProjects(pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.FactoryId), nameof(e.Project.FactoryId));
                keyValues.Add(nameof(e.DepartmentId), nameof(e.Project.DepartmentId));
                keyValues.Add(nameof(e.FactoryName), nameof(e.Project.FactoryName));
                keyValues.Add(nameof(e.DepartmentName), nameof(e.Project.DepartmentName));
                keyValues.Add(nameof(e.Year), nameof(e.Project.Year));
                keyValues.Add(nameof(e.ProjectDate), nameof(e.Project.ProjectDate));
                keyValues.Add(nameof(e.ProjectName), nameof(e.Project.Name));
                keyValues.Add(nameof(e.PlanType), nameof(e.Project.PlanType));
                keyValues.Add(nameof(e.ProjectType), nameof(e.Project.ProjectType));
                keyValues.Add(nameof(e.PrincipalName), nameof(e.Project.PrincipalName));
                keyValues.Add(nameof(e.ParentProjectCode), nameof(e.Project.ParentProjectCode));
                keyValues.Add(nameof(e.ParentProjectName), nameof(e.Project.ParentProjectName));
                keyValues.Add(nameof(e.Amount), nameof(e.Project.Amount));
                keyValues.Add(nameof(e.ProjectStatus), nameof(e.Project.ProjectStatus));
                keyValues.Add(nameof(e.ContentAndBasis), nameof(e.Project.ContentAndBasis));
                keyValues.Add(nameof(e.GoalAndBenefit), nameof(e.Project.GoalAndBenefit));
                keyValues.Add(nameof(e.ActualAmount), nameof(e.Project.ActualAmount));
                keyValues.Add(nameof(e.LaborCost), nameof(e.Project.LaborCost));
                m.DicLinkField = keyValues;
            }).HasLabel("项目编号").ShowInDetail(columnSpan: 2).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.ProjectName).ShowInDetail(columnSpan: 4).Readonly();
            View.Property(p => p.PlanType).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ProjectType).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Project.ProjectClassify;e.CatalogReloadData = true; }).Readonly();
            View.Property(p => p.PrincipalName).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ParentProjectCode).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ParentProjectName).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.Amount).HasLabel("预算金额").UseSpinEditor(p => p.DecimalPrecision = 2).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ProjectStatus).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.CloseItemType).ShowInDetail(columnSpan: 2);
            View.AttachDetailChildrenProperty(typeof(ProjectClose), (c) =>
            {
                var model = c.Parent as ProjectClose;
                if (model == null)
                {
                    return model;
                }
                return RT.Service.Resolve<ProjectChangeController>().GetProjectCloseById(model.Id);
            }, EditNormView).Show(ChildShowInWhere.Detail).HasLabel("项目指标").HasOrderNo(1);
        }

        /// <summary>
        /// 项目指标界面
        /// </summary>
        protected void ConfigNormView()
        {
            View.AssignAuthorize(typeof(ProjectClose));
            View.ClearCommands();
            View.UseDetail(8);
            using (View.OrderProperties())
            {
                View.Property(p => p.ContentAndBasis).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.GoalAndBenefit).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.ActualAmount).ShowInDetail(columnSpan: 4).HasLabel("项目金额成本").Readonly();
                View.Property(p => p.LaborCost).ShowInDetail(columnSpan: 4).HasLabel("实际人天工时").Readonly();
                View.Property(p => p.BenefitAnalysis).ShowInDetail(columnSpan: 8).Readonly();
                View.Property(p => p.InvestmentEfficiency).ShowInDetail(columnSpan: 8).Readonly();
                View.Property(p => p.PaybackForecast).ShowInDetail(columnSpan: 8).Readonly();
                View.Property(p => p.Summary).ShowInDetail(columnSpan: 8).Readonly();
                View.Property(p => p.Experience).ShowInDetail(columnSpan: 8).Readonly();
            }
        }

        /// <summary>
        /// 项目编辑指标界面
        /// </summary>
        protected void ConfigEditNormView()
        {
            View.AssignAuthorize(typeof(ProjectClose));
            View.ClearCommands();
            View.UseDetail(8);
            using (View.OrderProperties())
            {
                View.Property(p => p.ContentAndBasis).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.GoalAndBenefit).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.ActualAmount).ShowInDetail(columnSpan: 4).HasLabel("项目金额成本").Readonly();
                View.Property(p => p.LaborCost).ShowInDetail(columnSpan: 4).HasLabel("实际人天工时").Readonly();
                View.Property(p => p.BenefitAnalysis).ShowInDetail(columnSpan: 8);
                View.Property(p => p.InvestmentEfficiency).ShowInDetail(columnSpan: 8);
                View.Property(p => p.PaybackForecast).ShowInDetail(columnSpan: 8);
                View.Property(p => p.Summary).ShowInDetail(columnSpan: 8);
                View.Property(p => p.Experience).ShowInDetail(columnSpan: 8);
            }
        }
    }
}
