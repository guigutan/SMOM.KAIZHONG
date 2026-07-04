using SIE.EventMessages.APS.SaleOrderEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 监听 回写销售订单 承诺交期
    /// </summary>
    public class CallBackPromiseDeliveryListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static CallBackPromiseDeliveryListener Instance = new CallBackPromiseDeliveryListener();

        /// <summary>
        /// 发布事件总线
        /// </summary>
        public void Start()
        {
            CallBackPromiseDelivery();
        }

        /// <summary>
        /// 事件监听(回写销售订单承诺交期)
        /// </summary>
        private void CallBackPromiseDelivery()
        {
            RT.EventBus.Subscribe<CallBackPromiseDeliveryEvent>(this, e =>
            {
                RT.Service.Resolve<SaleOrderDetailController>().SavePromiseDelivery(e);
            });
        }
    }
}