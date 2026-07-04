using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.UserAgreement;
using SIE.Core;
using SIE.Core.ApiLogs;
using SIE.Core.Common.IService;
using SIE.Core.Common.Service;
using SIE.Core.PrintExt;
using SIE.Core.UserAgreements;
using SIE.Modules;
using SIE.Prints;

[assembly: Module(typeof(Module))]

namespace SIE.Core
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
            ApiLogAttrManager.Init();
        }

        /// <summary>   
        /// 注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register(typeof(ISysConfigService), typeof(SysConfigService), Services.ServiceLifeStyle.Singleton);
            RT.Service.Register(typeof(IRepositoryFactoryService), typeof(RepositoryFactoryService), Services.ServiceLifeStyle.Singleton);
            RT.Service.Register(typeof(NumberRuleController), Services.ServiceLifeStyle.Singleton);
            RT.Service.Register(typeof(IUserAgreement), typeof(UserAgreementController), Services.ServiceLifeStyle.Singleton);
            RT.Service.Register<ApiLogDbLogger>(Services.ServiceLifeStyle.Singleton);

            RT.Service.Register(typeof(PrintsController), typeof(PrintExtController));
            RT.Service.Register(typeof(IPrintsSerivce), typeof(ExtPrintsSerivce));
        }
    }
}
