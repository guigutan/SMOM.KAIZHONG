using SIE.EMS.Purchases.FixtureReceives;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Purchases.FixtureReceives.Commands
{
    /// <summary>
    /// 提交工治具接收
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.FixtureReceives.Commands.SubmitFixtureReceiveCommand")]
    public class SubmitFixtureReceiveCommand : ViewCommand
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
            RT.Service.Resolve<FixtureReceiveSnController>().SubmitReceive(selectedIds);
            return true;
        }
    }
}
