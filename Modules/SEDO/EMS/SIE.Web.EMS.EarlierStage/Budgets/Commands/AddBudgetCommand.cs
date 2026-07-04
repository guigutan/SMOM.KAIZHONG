using SIE.EMS.EarlierStage.Budgets;
using SIE.Web.Command;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.AddBudgetCommand")]
    public class AddBudgetCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return RT.Service.Resolve<BudgetController>().GetNewBudget();
        }
    }
}
