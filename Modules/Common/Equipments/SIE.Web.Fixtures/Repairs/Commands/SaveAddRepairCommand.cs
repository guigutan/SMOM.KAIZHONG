using SIE.Fixtures;
using SIE.Fixtures.Repairs.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Repairs.Commands
{
    /// <summary>
    /// 保存工治具报修
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Repairs.Commands.SaveAddRepairCommand")]
    public class SaveAddRepairCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行保存工治具报修
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
                errMsg = RT.Service.Resolve<ElecFixtureController>().SaveAddRepairInfo(fixRepairInfo);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }
    }
}
