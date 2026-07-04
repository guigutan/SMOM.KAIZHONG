using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Demands.Commands
{
    /// <summary>
    /// 保存库存出库
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Demands.Commands.UnloadSaveCommand")]
    public class UnloadSaveCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行保存
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>保存结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;

            try
            {
                var unloadInfo = args.Data.ToJsonObject<UnloadInfo>();
                errMsg = RT.Service.Resolve<ElecFixtureController>().SaveFixtureUnloadList(unloadInfo);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }
    }
}
