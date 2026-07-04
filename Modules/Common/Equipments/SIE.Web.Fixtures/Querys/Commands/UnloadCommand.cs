using SIE.Fixtures;
using SIE.Fixtures.Querys.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Querys.Commands
{
    /// <summary>
    /// 出库
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Querys.Commands.UnloadCommand")]
    public class UnloadCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行在库出库
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var unloadVM = args.Data.ToJsonObject<UnloadViewModel>();
            try
            {
                errMsg = RT.Service.Resolve<CoreFixtureController>().SaveUnload(unloadVM);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }
    }
}
