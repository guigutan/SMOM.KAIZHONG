using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts.ViewModels;
using SIE.Web.Command;

namespace SIE.Web.Fixtures.Accounts.Commands
{
    /// <summary>
    /// 保存ID类工治具台账
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Accounts.Commands.SaveIDAccountCommand")]
    public class SaveIDAccountCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 保存ID类工治具台账
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var model = args.Data.ToJsonObject<FixtureAccountViewModel>();
            RT.Service.Resolve<CoreFixtureController>().SaveFixtureIDAccount(model);
            return true;
        }
    }
}
