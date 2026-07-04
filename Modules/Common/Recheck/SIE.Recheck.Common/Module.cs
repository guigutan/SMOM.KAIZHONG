using SIE.Modules;
using SIE.Recheck.Common;

[assembly: Module(typeof(Module))]

namespace SIE.Recheck.Common
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
        }
    }
}