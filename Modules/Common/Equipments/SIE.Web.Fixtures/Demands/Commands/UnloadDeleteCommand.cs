using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Demands.Commands
{
    /// <summary>
    /// 删除出库数据
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Demands.Commands.UnloadDeleteCommand")]
    public class UnloadDeleteCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            UnloadInfo unloadInfo = null;
            var deleteUnloadInfo = args.Data.ToJsonObject<UnloadInfo>();
            try
            {
                unloadInfo = RT.Service.Resolve<ElecFixtureController>().UpdateUnloadStockInfo(deleteUnloadInfo);
            }
            catch (Exception ex)
            {
                unloadInfo.ErrMsg = ex.Message;
            }

            return unloadInfo;
        }
    }
}
