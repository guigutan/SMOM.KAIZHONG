using SIE.EventMessages;
using SIE.EventMessages.MES.Traces;
using SIE.Modules;
using SIE.MES.Traces;

[assembly: Module(typeof(Module))]

namespace SIE.MES.Traces
{
    /// <summary>
    /// 报表Module
    /// </summary>
    public class Module : DomainModule
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
        /// 注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register<ITrace, TraceController>();
        }
    }
}
