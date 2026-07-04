using SIE.EMS.EarlierStage;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EventMessages.EMS.EarlierStages;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.EMS.EarlierStage
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

        /// <summary>                                                                                                      
        /// 注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register<IBudget, BudgetController>();
        }
    }
}
