using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;

namespace SIE.Web.Equipments.EquipAccounts.Commands
{
    /// <summary>
    /// 设备台升级命令
    /// </summary>
    [JsCommand("SIE.Web.Equipments.EquipAccounts.Commands.UpgradeAccountCommand")]
    public class UpgradeAccountCommand : ViewCommand
    {
        /// <summary>
        /// 执行升级
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var accountId = args.Data.ToJsonObject<double>();
            RT.Service.Resolve<EquipAccountController>().UpgradeEquipAccount(accountId);
            return true;
        }
    }
}
