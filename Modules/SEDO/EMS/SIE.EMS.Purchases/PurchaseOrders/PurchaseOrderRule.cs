using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    #region 付款条件非重复验证规则
    /// <summary>
    /// 付款条件非重复验证规则
    /// </summary>
    [DisplayName("付款条件非重复验证规则")]
    [Description("付款条件非重复验证规则")]
    public class PaymentTermsNotDuplicateRule : NotDuplicateRule<PaymentTerms>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PaymentTermsNotDuplicateRule()
        {
            Properties.Add(PaymentTerms.PurchaseOrderIdProperty);
            Properties.Add(PaymentTerms.PhaseProperty);
            MessageBuilder = (e) => { return "付款阶段不能重复".L10N(); };
        }
    }
    #endregion

    #region 采购订单明细非重复验证规则
    /// <summary>
    /// 采购订单明细非重复验证规则
    /// </summary>
    [DisplayName("采购订单明细非重复验证规则")]
    [Description("采购订单明细非重复验证规则")]
    public class PurchaseOrderItemNotDuplicateRule : NotDuplicateRule<PurchaseOrderItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PurchaseOrderItemNotDuplicateRule()
        {
            Properties.Add(PurchaseOrderItem.PurchaseOrderIdProperty);
            Properties.Add(PurchaseOrderItem.PurchaseRequisitionItemIdProperty);
            MessageBuilder = (e) => { return "同一个申请单行只能选一次".L10N(); };
        }
    }
    #endregion
}
