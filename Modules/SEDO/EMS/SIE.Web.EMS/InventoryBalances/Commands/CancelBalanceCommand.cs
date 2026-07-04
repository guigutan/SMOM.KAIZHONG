using SIE.EMS.InventoryBalances;
using SIE.Web.Command;

namespace SIE.Web.EMS.InventoryBalances.Commands
{
    /// <summary>
    /// 撤回
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryBalances.Commands.CancelBalanceCommand")]
    public class CancelBalanceCommand : ViewCommand
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
            RT.Service.Resolve<InventoryBalanceController>().CancelBalance(args.SelectedIds[0]);
            return true;
        }
    }
}
