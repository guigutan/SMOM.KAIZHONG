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
    /// 项目变更-界面
    /// </summary>
    public class ProjectChangeViewConfig : WebViewConfig<ProjectChange>
    {
        /// <summary>
        /// 项目暂停
        /// </summary>
        public const string SuspendView = "SuspendView";

        /// <summary>
        /// 项目恢复
        /// </summary>
        public const string RecoveryView = "RecoveryView";

        /// <summary>
        /// 项目暂停只读
        /// </summary>
        public const string SuspendReadonlyView = "SuspendReadonlyView";

        /// <summary>
        /// 项目恢复只读
        /// </summary>
        public const string RecoveryReadonlyView = "RecoveryReadonlyView";

        /// <summary>
        /// 查看视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SuspendView, RecoveryView, SuspendReadonlyView, RecoveryReadonlyView, ReadonlyView);
            if (ViewGroup == SuspendView)
            {
                ConfigSuspendView();
            }
            else if (ViewGroup == RecoveryView)
            {
                ConfigRecoveryView();
            }
            else if (ViewGroup == SuspendReadonlyView)
            {
                ConfigSuspendReadonlyView();
            }
            else if (ViewGroup == RecoveryReadonlyView)
            {
                ConfigRecoveryReadonlyView();
            }
            else if (ViewGroup == ReadonlyView)
            {
                ConfigReadonlyView();
            }
            else
            {
                //无内容
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Common.Scripts.ApprovalBehavior");
            View.UseCommands(WebCommandNames.Add, "SIE.Web.EMS.EarlierStage.Projects.Commands.EditProjectCommand", typeof(DeleteProjectChangeCommand).FullName,
                "SIE.Web.EMS.EarlierStage.Projects.Commands.SeeProjectChangeCommand", typeof(SubmitProjectChangeCommand).FullName, typeof(CancelProjectChangeCommand).FullName,
               typeof(ExamineProjectChangeCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120).FixColumn();
            View.Property(p => p.DepartmentId).ShowInList(120).FixColumn();
            View.Property(p => p.No).ShowInList(160).FixColumn();
            View.Property(p => p.ApprovalStatus).ShowInList(80).FixColumn();
            View.Property(p => p.ProjectId).HasLabel("项目编号").ShowInList(160).FixColumn();
            View.Property(p => p.ProjectName).ShowInList(200);
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
            View.ChildrenProperty(p => p.KeyItemList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ProjectMemberList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ProjectWorkItemList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ChangeContentList).HasLabel("变更内容").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<ProjectChange>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(ProjectChange).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.SeeView).HasLabel("审核进度").HasOrderNo(2);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectChangeBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(SaveProjectChangeCommand).FullName);
            View.UseDetail(8);
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.DepartmentName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Year).ShowInDetail(columnSpan: 2).UseYearEditor().Readonly();
                View.Property(p => p.ProjectDate).HasLabel("立项时间").UseDateEditor().ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ProjectId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ProjectController>().GetChangeProjects(pagingInfo, keyword);
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
                    keyValues.Add(nameof(e.Remark), nameof(e.Project.Remark));
                    m.DicLinkField = keyValues;
                }).HasLabel("项目编号").ShowInDetail(columnSpan: 2).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
                View.Property(p => p.ProjectName).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.PlanType).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ProjectType).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Project.ProjectClassify; e.ReloadDataOnPopping = true; }).Readonly();
                View.Property(p => p.PrincipalName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ParentProjectCode).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ParentProjectName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Amount).HasLabel("预算金额").UseSpinEditor(p => p.DecimalPrecision = 2).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ProjectStatus).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ContentAndBasis).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.GoalAndBenefit).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.Remark).ShowInDetail(columnSpan: 4).Readonly();
            }
            View.ChildrenProperty(p => p.KeyItemList).HasLabel("关键事项").HasOrderNo(1);
            View.ChildrenProperty(p => p.ProjectMemberList).HasLabel("项目成员").HasOrderNo(2);
            View.ChildrenProperty(p => p.ProjectWorkItemList).HasLabel("项目计划").HasOrderNo(3);
            View.AttachDetailChildrenProperty(typeof(ProjectChange), (c) =>
            {
                var model = c.Parent as ProjectChange;
                model = RF.GetById<ProjectChange>(model.Id);
                return model;
            }, SuspendView).Show(ChildShowInWhere.Detail).HasLabel("项目暂停").HasOrderNo(4);
            View.AttachDetailChildrenProperty(typeof(ProjectChange), (c) =>
            {
                var model = c.Parent as ProjectChange;
                model = RF.GetById<ProjectChange>(model.Id);
                return model;
            }, RecoveryView).Show(ChildShowInWhere.Detail).HasLabel("项目恢复").HasOrderNo(5);
        }

        /// <summary>
        /// 项目暂停
        /// </summary>
        protected void ConfigSuspendView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.ClearCommands();
            View.UseDetail(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.IsSuspend).ShowInDetail(columnSpan: 3).Cascade(p => p.SuspendReason, null);
                View.Property(p => p.SuspendReason).ShowInDetail(columnSpan: 3).UseMemoEditor().Readonly(p => p.IsSuspend == false || p.IsSuspend == null);
            }
        }

        /// <summary>
        /// 项目恢复
        /// </summary>
        protected void ConfigRecoveryView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.ClearCommands();
            View.UseDetail(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.IsRecovery).ShowInDetail(columnSpan: 3).Cascade(p => p.RecoveryExplain, null);
                View.Property(p => p.RecoveryExplain).ShowInDetail(columnSpan: 3).UseMemoEditor().Readonly(p => p.IsRecovery == false || p.IsRecovery == null);
            }
        }

        /// <summary>
        /// 项目暂停只读
        /// </summary>
        protected void ConfigSuspendReadonlyView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.ClearCommands();
            View.UseDetail(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.IsSuspend).ShowInDetail(columnSpan: 3).Readonly();
                View.Property(p => p.SuspendReason).ShowInDetail(columnSpan: 3).UseMemoEditor().Readonly();
            }
        }

        /// <summary>
        /// 项目恢复只读
        /// </summary>
        protected void ConfigRecoveryReadonlyView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.ClearCommands();
            View.UseDetail(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.IsRecovery).ShowInDetail(columnSpan: 3).Readonly();
                View.Property(p => p.RecoveryExplain).ShowInDetail(columnSpan: 3).UseMemoEditor().Readonly();
            }
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectChangeBehavior");
            View.ClearCommands();
            View.UseDetail(8);
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.DepartmentName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Year).ShowInDetail(columnSpan: 2).UseYearEditor().Readonly();
                View.Property(p => p.ProjectDate).HasLabel("立项时间").UseDateEditor().ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ProjectId).HasLabel("项目编号").ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ProjectName).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.PlanType).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ProjectType).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = Project.ProjectClassify; e.ReloadDataOnPopping = true; }).Readonly();
                View.Property(p => p.PrincipalName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ParentProjectCode).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ParentProjectName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Amount).HasLabel("预算金额").UseSpinEditor(p => p.DecimalPrecision = 2).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ProjectStatus).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ContentAndBasis).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.GoalAndBenefit).ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.Remark).ShowInDetail(columnSpan: 4).Readonly();
                View.ChildrenProperty(p => p.KeyItemList).UseViewGroup(ReadonlyView).HasLabel("关键事项").HasOrderNo(1).Show(ChildShowInWhere.Detail);
                View.ChildrenProperty(p => p.ProjectMemberList).UseViewGroup(ReadonlyView).HasLabel("项目成员").HasOrderNo(2).Show(ChildShowInWhere.Detail);
                View.ChildrenProperty(p => p.ProjectWorkItemList).UseViewGroup(ReadonlyView).HasLabel("项目计划").HasOrderNo(3).Show(ChildShowInWhere.Detail);
                View.AttachDetailChildrenProperty(typeof(ProjectChange), (c) =>
                {
                    var model = c.Parent as ProjectChange;
                    model = RF.GetById<ProjectChange>(model.Id);
                    return model;
                }, SuspendReadonlyView).Readonly().Show(ChildShowInWhere.Detail).HasLabel("项目暂停").HasOrderNo(4).Show(ChildShowInWhere.Detail);
                View.AttachDetailChildrenProperty(typeof(ProjectChange), (c) =>
                {
                    var model = c.Parent as ProjectChange;
                    model = RF.GetById<ProjectChange>(model.Id);
                    return model;
                }, RecoveryReadonlyView).Readonly().Show(ChildShowInWhere.Detail).HasLabel("项目恢复").HasOrderNo(5).Show(ChildShowInWhere.Detail);
            }
        }
    }
}
