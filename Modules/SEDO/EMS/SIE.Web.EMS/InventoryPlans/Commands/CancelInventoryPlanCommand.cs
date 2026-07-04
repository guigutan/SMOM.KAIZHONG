using SIE.EMS.InventoryPlans;
using SIE.Web.Command;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 撤回
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.CancelInventoryPlanCommand")]
    public class CancelInventoryPlanCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null || args.SelectedIds.Length == 0)
            {
                return false;
            }
            //只能单选
            RT.Service.Resolve<InventoryPlanController>().CancelInventoryPlan(args.SelectedIds[0]);
            return true;
        }
    }
}
