using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.EarlierStage.Projects;
using SIE.MetaModel.View;
using SIE.Web.EMS.EarlierStage.Projects.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目关键事项视图配置
    /// </summary>
    public class ProjectKeyItemViewConfig : WebViewConfig<ProjectKeyItem>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 项目查看视图
        /// </summary>
        public const string ProjectSeeView = "ProjectSeeView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProjectKeyItem));
            View.DeclareExtendViewGroup(EditView, ProjectSeeView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
            if (ViewGroup == ProjectSeeView)
            {
                ConfigProjectSeeView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(ProjectKeyItemImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.ProjectCode).ShowInList(160);
            View.Property(p => p.Description).ShowInList(200);
            View.Property(p => p.ProjectName).ShowInList(200);
            View.Property(p => p.ProjectStatus).ShowInList(80);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.BudgetId).HasLabel("预算编码").ShowInList(160);
            View.Property(p => p.BudgetName).ShowInList(200);
            View.Property(p => p.CanUseAmount).ShowInList(130);
            View.Property(p => p.BudgetAmount).ShowInList(130);
            View.Property(p => p.ActualCost).ShowInList(130);
            View.Property(p => p.WorkStatus).ShowInList(80);
            View.Property(p => p.ProjectWorkItemId).ShowInList(80);
            View.Property(p => p.PlanStart).HasLabel("计划开始时间".L10N()+"*").UseDateEditor().ShowInList(100);
            View.Property(p => p.PlantEnd).HasLabel("计划结束时间".L10N() + "*").UseDateEditor().ShowInList(100);
            View.Property(p => p.LaborCost).ShowInList(80);
            View.Property(p => p.Remark).ShowInList(200);
            View.ChildrenProperty(p => p.MemberList).HasLabel("事项成员").HasOrderNo(1);
            View.ChildrenProperty(p => p.PlanList).HasLabel("事项计划").HasOrderNo(2);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(4);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Project.Code).HasLabel("项目编码").BeforeImportFunc((v) =>
            {
                var taskNo = v.ToString();
                if (taskNo.IsNullOrEmpty())
                {
                    throw new ValidationException("【项目编码】必须填写".L10N());
                }

                return v;
            }); 

            View.Property(p => p.Description).BeforeImportFunc((v) =>
            {
                var description = v.ToString();
                if (description.IsNullOrEmpty())
                {
                    throw new ValidationException("项目关键事项的【事项说明】必须填写".L10N());
                }

                return v;
            });
            View.PropertyRef(p => p.Budget.BudgetNo).HasLabel("预算编码");
            View.Property(p => p.NullBudgetAmount);
            View.Property(p => p.Remark);
            View.Property(p => p.ProjectWorkItemName);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        void ConfigEditView()
        {
            View.AssignAuthorize(typeof(Project));
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectKeyItemBehavior");
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectKeyItemListBehavior");
            View.UseCommands("SIE.Web.EMS.EarlierStage.Projects.Commands.AddProjectKeyItemCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Description).ShowInList(200);
                View.Property(p => p.BudgetId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var keyItem = source as ProjectKeyItem;
                    if (keyItem == null)
                    {
                        return new EntityList<Budget>();
                    }
                    return RT.Service.Resolve<BudgetController>().GetKeyItemBudgets(keyItem.FactoryId, keyItem.DepartmentId, pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.BudgetName), nameof(e.Budget.BudgetName));
                    keyValues.Add(nameof(e.CanUseAmount), nameof(e.Budget.CanUseAmount));
                    m.DicLinkField = keyValues;
                }).HasLabel("预算编码").Readonly(p => p.PlanType == PlanType.Out).ShowInList(160);
                View.Property(p => p.BudgetName).Readonly().ShowInList(200);
                View.Property(p => p.CanUseAmount).Readonly().ShowInList(130);
                View.Property(p => p.BudgetAmount).UseSpinEditor(p =>
                {
                    p.DecimalPrecision = 2;
                    p.MinValue = 0;
                }).ShowInList(130);
                View.Property(p => p.WorkStatus).Readonly().ShowInList(80);
                View.Property(p => p.ProjectWorkItemId).UseDataSource((e, c, r) =>
                {
                    var item = e as ProjectKeyItem;
                    if (item == null)
                    {
                        return new EntityList<ProjectWorkItem>();
                    }
                    return RT.Service.Resolve<ProjectController>().GetWorkItemsByProjectIds(item.ProjectId, c, r);
                }).HasLabel("计划节点".L10N() + "*").ShowInList(200);
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MemberList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.PlanList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 项目查看视图
        /// </summary>
        void ConfigProjectSeeView()
        {
            View.AssignAuthorize(typeof(Project));
            View.DisableEditing();
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-7, -3);
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectKeyItemListBehavior");
            View.UseCommands("SIE.Web.EMS.EarlierStage.Projects.Commands.GoToKeyItemCommand",
                typeof(ProjectKeyItemImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Description).ShowInList(200);
                View.Property(p => p.BudgetId).HasLabel("预算编码").ShowInList(160);
                View.Property(p => p.BudgetName).ShowInList(200);
                View.Property(p => p.CanUseAmount).ShowInList(130);
                View.Property(p => p.BudgetAmount).ShowInList(130);
                View.Property(p => p.ActualCost).ShowInList(130);
                View.Property(p => p.WorkStatus).ShowInList(80);
                View.Property(p => p.ProjectWorkItemId).ShowInList(80);
                View.Property(p => p.PlanStart).UseDateEditor().ShowInList(100);
                View.Property(p => p.PlantEnd).UseDateEditor().ShowInList(100);
                View.Property(p => p.LaborCost).ShowInList(80);
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件");
                View.ChildrenProperty(p => p.MemberList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.PlanList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}