using SIE.Kit.EventMessages.UrgentOrder;
using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.Threading;
using System;
using System.Threading.Tasks;

namespace SIE.Kit.UrgentOrder.Events
{
    /// <summary>
    /// 物料加急单事件监听
    /// </summary>
    public class ItemUrgentOrderListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static ItemUrgentOrderListener Instance = new ItemUrgentOrderListener();

        /// <summary>
        /// 订阅事件总线
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<ItemUrgentOrderReceiveEvent>(this, e =>
            {
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<ItemUrgentOrderController>().UpdateItemUrgentOrder(e);
                }).WithCurrentThreadContext());
            });
        }
    }
}
