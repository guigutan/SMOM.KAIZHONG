using SIE.Inventory.Onhands;
using SIE.Items.Events;
using SIE.Warehouses.Events;

namespace SIE.Inventory.Events
{
    /// <summary>
    /// 监听事件
    /// </summary>
    class HasItemStokEventListener
    {
        /// <summary>
        /// 监听器
        /// </summary>
        public static readonly HasItemStokEventListener Instance = new HasItemStokEventListener();

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<HasItemStockEvent>(this, e =>
            {
                RT.Service.Resolve<InvOnhandController>().HasItemStock(e.IetmId);
            });

            RT.EventBus.Subscribe<HasLocationStockEvent>(this, e =>
            {
                RT.Service.Resolve<InvOnhandController>().HasLocationOnhands(e.LocId);
            });
        }
    }
}
