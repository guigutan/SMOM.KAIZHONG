using SIE.Modules;
using SIE.Warehouses;

[assembly: Module(typeof(Module))]

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库Module
    /// </summary>
    internal class Module : DomainModule
    {
        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {            
        }
    }
}
