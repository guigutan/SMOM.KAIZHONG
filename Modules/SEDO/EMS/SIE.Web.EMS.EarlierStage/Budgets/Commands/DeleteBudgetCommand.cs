using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 删除预算
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.DeleteBudgetCommand")]
    public class DeleteBudgetCommand : DeleteCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            base.OnSaving(data);
            var ids = new List<double>();
            data.DeletedList.ForEach(p =>
            {
                var model = p as Budget;
                if (model != null)
                    ids.Add(model.Id);
            });
            RT.Service.Resolve<BudgetController>().DeleteCheckBudgetState(ids);
        }
    }
}
