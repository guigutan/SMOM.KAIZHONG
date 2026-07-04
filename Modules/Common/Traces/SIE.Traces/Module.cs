using SIE.Traces;
using SIE.Modules;
[assembly: Module(typeof(Module))]

namespace SIE.Traces
{
    /// <summary>
    /// Domain模块
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
        }
    }
}
