using SIE.Equipments;
using SIE.Equipments.WorkFlows;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.Equipments
{
    /// <summary>
    /// 业务相关的设备等
    /// </summary>
    internal class Module : DomainModule
    {
        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="app">IApp</param>
        public override void Initialize(IApp app)
        {
            RegisterService();
        }

        /// <summary>                                                                                                      
        /// 注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register<IWorkFlow, WorkFlowController>();
        }
    }
}
