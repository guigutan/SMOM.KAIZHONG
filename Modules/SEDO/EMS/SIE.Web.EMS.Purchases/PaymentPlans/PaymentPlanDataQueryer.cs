using SIE.EMS.Purchases.PaymentPlans;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Web.Data;

namespace SIE.Web.EMS.Purchases.PaymentPlans
{
    /// <summary>
    /// 付款计划查询器
    /// </summary>
    public class PaymentPlanDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的付款计划
        /// </summary>
        /// <returns>新的付款计划</returns>
        public PaymentPlan GetNewPaymentPlan()
        {
            return RT.Service.Resolve<PaymentPlanController>().GetNewPaymentPlan();
        }

        /// <summary>
        /// 获取采购订单信息
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns>采购订单信息</returns>
        public PurchaseOrder GetPurOrderInfo(double orderId)
        {
            return RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrderById(orderId);
        }
    }
}
