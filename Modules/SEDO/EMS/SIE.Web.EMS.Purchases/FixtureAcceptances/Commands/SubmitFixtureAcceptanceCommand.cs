using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances.Commands
{
    /// <summary>
    /// 提交备件验收
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.SubmitFixtureAcceptanceCommand")]
    public class SubmitFixtureAcceptanceCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<FixtureAcceptancesController>().SubmitFixtureAcceptances(selectedIds);
            return true;
        }
    }

}
