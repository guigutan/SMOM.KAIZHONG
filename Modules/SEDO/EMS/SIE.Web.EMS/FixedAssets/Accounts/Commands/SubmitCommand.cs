using SIE.EMS.FixedAssets.Accounts;
using SIE.Web.Command;

namespace SIE.Web.EMS.FixedAssets.Accounts.Commands
{
    /// <summary>
    /// 提交
    /// </summary>
    [JsCommand("SIE.Web.EMS.FixedAssets.Accounts.Commands.SubmitCommand")]
    public class SubmitCommand : ViewCommand
    {
        /// <summary>
        /// 提交保存
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scop</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var fixedAssetsAccount = args.Data.ToJsonObject<FixedAssetsAccount>();
            return RT.Service.Resolve<FixedAssetsAccountController>().Submit(fixedAssetsAccount.Id);
        }
    }
}
