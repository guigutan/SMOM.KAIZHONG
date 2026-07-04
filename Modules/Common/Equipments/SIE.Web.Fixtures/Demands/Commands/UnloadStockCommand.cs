using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Demands.Commands
{
    /// <summary>
    /// 出库库存
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Demands.Commands.UnloadStockCommand")]
    public class UnloadStockCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行出库库存
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            UnloadInfo unloadInfo = null;
            var unloadStockInfo = args.Data.ToJsonObject<UnloadInfo>();
            try
            {
                unloadInfo = RT.Service.Resolve<ElecFixtureController>().SaveUnloadStockInfo(unloadStockInfo);
            }
            catch (Exception ex)
            {
                unloadInfo.ErrMsg = ex.Message;
            }

            return unloadInfo;
        }
    }
}
