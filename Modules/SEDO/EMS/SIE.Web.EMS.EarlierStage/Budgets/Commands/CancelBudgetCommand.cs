using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 撤回预算
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.CancelBudgetCommand")]
    public class CancelBudgetCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
                throw new ValidationException("撤回预算数据参数不能为空".L10N());
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<BudgetController>().RecallBudget(selectedIds);
            return true;
        }
    }
}
