using SIE.EMS.Purchases;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.EventMessages.EMS.Fixtures;
using SIE.EventMessages.EMS.Purchases;
using SIE.EventMessages.EMS.SparePartReceives;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.EMS.Purchases
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序对象</param>
        public override void Initialize(IApp app)
        {
            RegisterService();
        }

        private void RegisterService()
        {
            RT.Service.Register(typeof(IPurchases), typeof(PurchaseOrderController));
            RT.Service.Register(typeof(IFixtureAcceptance), typeof(FixtureAcceptancesController));
            RT.Service.Register(typeof(ISparePartReceives), typeof(SparePartReceiveController));
        }
    }
}
