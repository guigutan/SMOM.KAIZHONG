using SIE.Modules;
using SIE.RedCardManagment.WorkFlow;
using SIE.RedCardManagment.WorkFlow.Events;

[assembly: Module(typeof(Module))]
namespace SIE.RedCardManagment.WorkFlow
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            RedCardManagmentListener.Instance.Start();
        }
    }
}
