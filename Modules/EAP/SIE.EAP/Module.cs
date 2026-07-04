using SIE.EAP;
using SIE.EAP.Common.Controller;
using SIE.EventMessages.EAP.Infs;
using SIE.EventMessages.WMS.StereoWarhouses;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.EAP
{
    public class Module: DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
            RegisterService();
        }

        /// <summary>
        /// IOC注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register<IWcsController, WCSBaseController>();
            RT.Service.Register<IEapController, EapBaseController>();
        }
    }
}
