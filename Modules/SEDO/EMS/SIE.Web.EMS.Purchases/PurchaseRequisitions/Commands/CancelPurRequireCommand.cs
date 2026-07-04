using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands
{
    /// <summary>
    /// 撤回采购
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.CancelPurRequireCommand")]
    public class CancelPurRequireCommand : ViewCommand
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
            RT.Service.Resolve<PurchaseRequisitionController>().Withdraw(selectedIds);
            return true;
        }
    }
}
