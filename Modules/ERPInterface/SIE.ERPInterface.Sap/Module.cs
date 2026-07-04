using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap;
using SIE.ERPInterface.Sap.Controller;
using SIE.EventMessages.ErpCommon;
using SIE.EventMessages.ErpSap;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.ERPInterface.Sap
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
            RT.Service.Register<IErpSapController, ISapController>();
        }
    }
}
