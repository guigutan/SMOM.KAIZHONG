using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.PaymentPlans
{
    #region 相同采购订单号付款阶段不能重复
    /// <summary>
    /// 相同采购订单号付款阶段不能重复
    /// </summary>
    [DisplayName("相同采购订单号付款阶段不能重复")]
    [Description("相同采购订单号付款阶段不能重复")]
    public class PaymentPlanNotDuplicateRule : NotDuplicateRule<PaymentPlan>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PaymentPlanNotDuplicateRule()
        {
            Properties.Add(PaymentPlan.PurchaseOrderIdProperty);
            Properties.Add(PaymentPlan.PaymentTermsIdProperty);
            MessageBuilder = (e) => { return "相同采购订单号付款阶段不能重复".L10N(); };
        }
    }
    #endregion
}
