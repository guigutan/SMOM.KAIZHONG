using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 保存预算变更
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.SaveBudgetChangeCommand")]
    public class SaveBudgetChangeCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            var entity = list.Count > 0 ? list[0] : null;
            if (entity != null)
            {
                var budgetChange = entity as BudgetChange;
                if (budgetChange.Budget != null)
                {
                    if (budgetChange.NewAmount == budgetChange.Budget.BudgetAmount)
                    {
                        throw new ValidationException("变更后预算金额不能等于预算金额".L10N());
                    }
                }
                RT.Service.Resolve<BudgetChangeController>().SaveBudgetChange(budgetChange);
            }
            return entity;
        }
    }
}
