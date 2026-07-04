using SIE.EMS.InventoryPlans;
using SIE.Web.Command;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 提交
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.SubmitInventoryPlanCommand")]
    public class SubmitInventoryPlanCommand : ViewCommand
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
            //只能单选提交
            RT.Service.Resolve<InventoryPlanController>().SubmitInventoryPlan(args.SelectedIds[0]);
            return true;
        }
    }
}
