using SIE.Modules;
using SIE.Web.Tech.Common;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Tech.Common
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">程序</param>
        public override void Initialize(IApp app)
        {
        }
    }
}