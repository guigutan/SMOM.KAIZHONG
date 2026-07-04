using SIE.Fixtures;
using SIE.Fixtures.Repairs.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Repairs.Commands
{
    /// <summary>
    /// 添加工治具报修命令
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Repairs.Commands.AddFixtureRepairCommand")]
    public class AddFixtureRepairCommand : ViewCommand
    {
        /// <summary>
        /// 执行工治具报修命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            AddFixtureRepairInfo repairInfo = new AddFixtureRepairInfo();
            repairInfo.errMsg = string.Empty;
            try
            {
                repairInfo = RT.Service.Resolve<CoreFixtureController>().GetFixtureRepairInfo();
            }
            catch (Exception ex)
            {
                repairInfo.errMsg = ex.Message;
            }

            return repairInfo;
        }
    }
}
