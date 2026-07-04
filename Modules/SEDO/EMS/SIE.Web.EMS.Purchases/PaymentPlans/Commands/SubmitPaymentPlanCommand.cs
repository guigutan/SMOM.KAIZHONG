using SIE.EMS.Purchases.PaymentPlans;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PaymentPlans.Commands
{
    /// <summary>
    /// 提交付款计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PaymentPlans.Commands.SubmitPaymentPlanCommand")]
    public class SubmitPaymentPlanCommand : ViewCommand
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
            RT.Service.Resolve<PaymentPlanController>().SubmitPaymentPlan(selectedIds);
            return true;
        }
    }
}
