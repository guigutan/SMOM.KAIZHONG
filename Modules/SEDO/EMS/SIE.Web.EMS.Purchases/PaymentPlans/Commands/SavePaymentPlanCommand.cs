using SIE.EMS.Purchases.PaymentPlans;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.PaymentPlans.Commands
{
    /// <summary>
    /// 保存付款计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PaymentPlans.Commands.SavePaymentPlanCommand")]
    public class SavePaymentPlanCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            var entity = list.Count > 0 ? list[0] : null;
            if (entity != null)
            {
                var pur = entity as PaymentPlan;
                RT.Service.Resolve<PaymentPlanController>().SavePaymentPlan(pur);
            }
            return entity;
        }
    }
}
