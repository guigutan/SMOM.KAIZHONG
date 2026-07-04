using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Enums;
using SIE.Equipments.Enums;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using SIE.Domain;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 导入项目关键事项
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectKeyItemImportCommand")]
    public class ProjectKeyItemImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 行数据读取, 默认按视图配置把数据读取后，还可以进行自定义处理
        /// </summary>
        /// <param name="data">行数据</param>
        /// <param name="cache">缓存数据</param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var keyItem = data.Entity as ProjectKeyItem;
            if (keyItem.Project == null)
                throw new ValidationException("项目编码为空或用户没有项目权限".L10N());
            var project = keyItem.Project;

            if (project.ApprovalStatus != ApprovalStatus.Draft
                && project.ApprovalStatus != ApprovalStatus.Reject)
            {
                throw new ValidationException("项目{0}状态不是【待提交】或【驳回】".L10nFormat(project.Code));
            }

            if (project.PlanType == PlanType.Out)
            {
                if (keyItem.BudgetId.HasValue)
                {
                    throw new ValidationException("项目【{0}】计划类型为【计划外】计，不能关联预算".L10nFormat(project.Code));
                }
            }
            else
            {
                if (keyItem.BudgetId == null)
                    throw new ValidationException("预算编码不能为空".L10N());
                var budget = RT.Service.Resolve<BudgetController>().GetBudgetById(keyItem.BudgetId.Value);
                if (budget == null)
                    throw new ValidationException("获取不到预算编码".L10N());
                if (budget.FactoryId != project.FactoryId)
                    throw new ValidationException("预算{0}和项目{1}工厂或部门不匹配".L10nFormat(budget.BudgetNo, project.Code));
                if (budget.DepartmentId != project.DepartmentId)
                    throw new ValidationException("预算{0}和项目{1}工厂或部门不匹配".L10nFormat(budget.BudgetNo, project.Code));
                if (budget.ApprovalStatus != ApprovalStatus.Audited)
                    throw new ValidationException("预算的审核状态不是已审批".L10N());
                var now = RF.Find<Budget>().GetDbTime();
                var year = RT.Service.Resolve<BudgetController>().GetFiscalYear(now);
                if (budget.Year < year)
                    throw new ValidationException("预算已失效".L10N());
            }

            if (keyItem.NullBudgetAmount == null)
            {
                throw new ValidationException("事项预算不能为空".L10N());
            }

            keyItem.BudgetAmount = Math.Round(keyItem.NullBudgetAmount.Value, 2);
            keyItem.WorkStatus = WorkStatus.NotStarted;
            keyItem.FactoryId = project.FactoryId;
            keyItem.DepartmentId = project.DepartmentId;
        }

        /// <summary>
        /// 复写保存方法
        /// </summary>
        /// <param name="batch">数据</param>
        protected override void OnSave(IList<RowData> batch)
        {
            importResult.MessageList.AddRange(RT.Service.Resolve<ProjectController>().SaveImportKeyItmes(batch));
        }
    }
}
