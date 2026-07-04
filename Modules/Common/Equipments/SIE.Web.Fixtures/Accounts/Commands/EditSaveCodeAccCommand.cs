using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Accounts.Commands
{
    /// <summary>
    /// 保存编码类工治具台帐
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Accounts.Commands.EditSaveCodeAccCommand")]
    public class EditSaveCodeAccCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行保存操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var account = args.Data.ToJsonObject<FixtureCodeAccount>();
            try
            {
                errMsg = RT.Service.Resolve<CoreFixtureController>().SaveFixtureCodeAccount(account);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }
    }
}
