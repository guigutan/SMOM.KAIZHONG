using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Logs;
using SIE.EventMessages.ErpCommon;
using SIE.EventMessages.IOT;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.ERPInterface.Common
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
            RT.Service.Register<IUploadLogControllercs, UploadLogControllercs>();
        }
    }
}
