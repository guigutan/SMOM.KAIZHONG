using SIE.ERPInterface.Api;
using SIE.ERPInterface.Api.WebApi;
using SIE.ERPInterface.Api.WebApi.KaiZhong;
using SIE.EventMessages.MES.Dispatchs;
using SIE.EventMessages.WebApis;
using SIE.MES.Edge;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.ERPInterface.Api
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
            RT.Service.Register<IWebApi, IWebApiController>();
            RT.Service.Register<IDispatchsErp, KzBaseDateInfController>();
        }
    }
}
