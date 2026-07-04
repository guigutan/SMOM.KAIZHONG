using SIE.EMS.Purchases.SparePartReceives;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.SparePartReceives.Commands
{
    /// <summary>
    /// 提交备件接收
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.SparePartReceives.Commands.SubmitSparePartReceiveCommand")]
    public class SubmitSparePartReceiveCommand : ViewCommand
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
            RT.Service.Resolve<SparePartReceiveSnController>().SubmitSparePartReceive(selectedIds);
            return true;
        }
    }
}
