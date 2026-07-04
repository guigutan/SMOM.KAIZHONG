using SIE.Domain.Query;
using SIE.EventMessages.RealTimeInventory;
using SIE.EventMessages.WMS.Inventory;
using SIE.Inventory;
using SIE.Inventory.Events;
using SIE.Inventory.Interfaces;
using SIE.Inventory.Piles;
using SIE.Inventory.Transactions;
using SIE.Modules;
using SIE.Packages.Boxs;
using System;
using System.Linq;

[assembly: Module(typeof(Module))]

namespace SIE.Inventory
{
    /// <summary>
    /// Domian
    /// </summary>
    /// <seealso cref="SIE.Modules.DomainModule" />
    public class Module : DomainModule
    {
        /// <summary>
        /// 模块的初始化方法。
        /// 框架会在启动时根据启动级别顺序调用本方法。
        /// 方法有两个职责：
        /// 1.依赖注入。
        /// 2.注册 app 生命周期中事件，进行特定的初始化工作。
        /// </summary>
        /// <param name="app">应用程序对象。</param>
        public override void Initialize(IApp app)
        {
            app.StartupCompleted += App_StartupCompleted;             
            RT.Service.Register<IRealTimeInventory, RealTimeInventoryController>();
            RT.Service.Register<IGetLotLpnOnhand, IInvOnhandController>();            
        }

          
        /// <summary>
        /// APP启动后
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            HasItemStokEventListener.Instance.Start();
        }
    }
}
