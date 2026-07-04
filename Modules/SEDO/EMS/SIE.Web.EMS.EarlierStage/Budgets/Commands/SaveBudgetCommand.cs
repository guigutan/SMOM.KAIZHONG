using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Web.Command;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 保存预算
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.SaveBudgetCommand")]
    public class SaveBudgetCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnSaving(Entity entity)
        {
            base.OnSaving(entity);
            var budget = entity as Budget;
            if (budget != null && budget.PersistenceStatus == PersistenceStatus.Modified)
            {
                RT.Service.Resolve<BudgetController>().EditCheckBudgetState(budget.Id);
            }
        }
    }
}
