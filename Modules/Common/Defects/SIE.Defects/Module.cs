using SIE.Defects;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.Defects
{
    /// <summary>
    /// 模块配置
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
        }
    }
}
