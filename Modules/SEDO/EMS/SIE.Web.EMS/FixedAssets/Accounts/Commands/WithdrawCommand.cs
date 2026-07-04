using SIE.EMS.FixedAssets.Accounts;
using SIE.Web.Command;

namespace SIE.Web.EMS.FixedAssets.Accounts.Commands
{
    /// <summary>
    /// 撤回
    /// </summary>
    [JsCommand("SIE.Web.EMS.FixedAssets.Accounts.Commands.WithdrawCommand")]
    public class WithdrawCommand : ViewCommand
    {
        /// <summary>
        /// 撤回保存
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scop</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var fixedAssetsAccount = args.Data.ToJsonObject<FixedAssetsAccount>();
            return RT.Service.Resolve<FixedAssetsAccountController>().Withdraw(fixedAssetsAccount.Id);
        }
    }
}
