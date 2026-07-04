using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 编辑界面-提交预算
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.EditSubmitBudgetCommand")]
    public class EditSubmitBudgetCommand : FormSaveCommand
    {
        /// <summary>
        /// 编辑界面-提交预算
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var budget = entity as Budget;
            if (budget == null)
            {
                throw new ValidationException("提交预算执行失败，数据错误".L10N());
            }

            RT.Service.Resolve<BudgetController>().EditSubmitBudget(budget);
        }
    }
}
