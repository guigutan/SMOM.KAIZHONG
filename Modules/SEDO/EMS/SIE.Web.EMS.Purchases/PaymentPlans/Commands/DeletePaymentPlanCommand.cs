using SIE.Domain;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Purchases.PaymentPlans.Commands
{
    /// <summary>
    /// 删除付款计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PaymentPlans.Commands.DeletePaymentPlanCommand")]
    public class DeletePaymentPlanCommand : DeleteCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            base.OnSaving(data);
            var ids = new List<double>();
            data.DeletedList.ForEach(p =>
            {
                var model = p as PaymentPlan;
                if (model != null)
                    ids.Add(model.Id);
            });
            RT.Service.Resolve<PaymentPlanController>().DeletePaymentPlan(ids);
        }
    }
}
