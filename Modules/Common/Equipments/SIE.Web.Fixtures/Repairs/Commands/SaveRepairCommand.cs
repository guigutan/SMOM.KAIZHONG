using SIE.Fixtures;
using SIE.Fixtures.Repairs.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Repairs.Commands
{
    /// <summary>
    /// 执行维修保存命令
    /// </summary>
    public class SaveRepairCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行维修保存命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            string errMsg = string.Empty;
            var fixRepairInfo = args.Data.ToJsonObject<FixtureRepairInfo>();
            try
            {
                errMsg = RT.Service.Resolve<ElecFixtureController>().SaveRepairInfo(fixRepairInfo);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }
    }
}
