using SIE.Fixtures;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Demands.Commands
{
    /// <summary>
    /// 提交需求单
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Demands.Commands.SubmitDemandsCommand")]
    public class SubmitDemandsCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<CoreFixtureController>().SubmitDemandsCommand(selectedIds);
            return true;
        }
    }
}
