using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Equipments.EquipAccounts.Commands
{
    /// <summary>
    /// 设备台降级命令
    /// </summary>
    [JsCommand("SIE.Web.Equipments.EquipAccounts.Commands.DowngradeAccountCommand")]
    public class DowngradeAccountCommand : ViewCommand
    {
        /// <summary>
        /// 执行降级
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var parentAccountId = args.Data.ToJsonObject<double>();
            var accountId = args.SelectedIds.First();
            RT.Service.Resolve<EquipAccountController>().DowngradeAccountCommand(accountId, parentAccountId);
            return true;
        }
    }
}
