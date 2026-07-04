using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Enums;
using SIE.EMS.EarlierStage.Projects;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更关键事项-界面
    /// </summary>
    public class ProjectChangeKeyItemViewConfig : WebViewConfig<ProjectChangeKeyItem>
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
            {
                ConfigReadonlyView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ProjectChange));
            View.AddBehavior("SIE.Web.EMS.EarlierStage.Projects.ProjectChangeKeyItemBehavior");
            View.UseCommands("SIE.Web.EMS.EarlierStage.Projects.Commands.AddProChangeKeyCommand", "SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteProChangeKeyCommand");
            View.Property(p => p.Description).ShowInList(200);
            View.Property(p => p.BudgetId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var keyItem = source as ProjectChangeKeyItem;
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
            }).HasLabel("预算编码").Readonly(p => p.PlanType == PlanType.Out || p.ActualCost > 0).ShowInList(160);
            View.Property(p => p.BudgetName).Readonly().ShowInList(200);
            View.Property(p => p.CanUseAmount).Readonly().ShowInList(130);
            View.Property(p => p.BudgetAmount).UseSpinEditor(p =>
            {
                p.DecimalPrecision = 2;
                p.MinValue = 0;
            }).ShowInList(130);
            View.Property(p => p.ActualCost).Readonly().ShowInList(130);
            View.Property(p => p.WorkStatus).Readonly().ShowInList(80);
            View.Property(p => p.ProjectChangeWorkItemId).UseDataSource((e, c, r) =>
            {
                var item = e as ProjectChangeKeyItem;
                if (item == null)
                {
                    return new EntityList<ProjectChangeWorkItem>();
                }
                return RT.Service.Resolve<ProjectChangeController>().GetWorkItemsByProjectChangeIds(item.ProjectChangeId, c, r);
            }).HasLabel("计划节点").Readonly(p=>p.PersistenceStatus== PersistenceStatus.New).ShowInList(200);
            View.Property(p => p.LaborCost).Readonly().ShowInList(130);
            View.Property(p => p.Remark).ShowInList(200);
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
                View.Property(p => p.Description).ShowInList(200).Readonly();
                View.Property(p => p.BudgetId).HasLabel("预算编码").Readonly().ShowInList(160);
                View.Property(p => p.BudgetName).Readonly().ShowInList(200);
                View.Property(p => p.CanUseAmount).Readonly().ShowInList(130);
                View.Property(p => p.BudgetAmount).UseSpinEditor(p =>
                {
                    p.DecimalPrecision = 2;
                    p.MinValue = 0;
                }).ShowInList(130).Readonly();
                View.Property(p => p.ActualCost).Readonly().ShowInList(130);
                View.Property(p => p.WorkStatus).Readonly().ShowInList(80);
                View.Property(p => p.ProjectChangeWorkItemId).Readonly().ShowInList(80);
                View.Property(p => p.LaborCost).Readonly().ShowInList(130);
                View.Property(p => p.Remark).ShowInList(200).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
