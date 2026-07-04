using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PurchaseOrders.Commands
{
    /// <summary>
    /// 提交采购
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PurchaseOrders.Commands.SubmitPurOrderCommand")]
    public class SubmitPurOrderCommand : ViewCommand
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
            RT.Service.Resolve<PurchaseOrderController>().SubmitPurOrder(selectedIds);
            return true;
        }
    }
}
