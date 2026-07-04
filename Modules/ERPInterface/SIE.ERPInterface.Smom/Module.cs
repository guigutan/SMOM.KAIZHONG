using SIE.ERPInterface.Smom;
using SIE.ERPInterface.Smom.IOT;
using SIE.EventMessages.IOT;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.ERPInterface.Smom
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

            RT.Service.Register<IIotTaskReport, IotController>();
        }
    }
}
