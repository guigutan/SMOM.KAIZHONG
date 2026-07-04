using SIE.EMS.WorkFlow;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.EMS.WorkFlow
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    internal class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序对象</param>
        public override void Initialize(IApp app)
        { }
    }
}
