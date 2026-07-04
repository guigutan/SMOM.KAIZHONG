using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.SaleOrderEvents
{
    /// <summary>
    /// 回写工厂交期 对象
    /// </summary>
    public class CallBackPromiseDeliveryEvent
    {
        /// <summary>
        /// 回写工厂交期信息
        /// </summary>
        public List<CallBackPromiseDeliveryInfo> InfoList { get; set; }
    }

    /// <summary>
    /// 回写工厂交期信息
    /// </summary>
    public class CallBackPromiseDeliveryInfo
    {
        /// <summary>
        /// 销售订单编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 销售订单明细行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 承诺交期
        /// </summary>
        public DateTime? PromiseDelivery { get; set; }
    }
}