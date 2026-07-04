using SIE.DataAuth;
using SIE.Modules;
using SIE.Security;

[assembly: Module(typeof(Module))]
namespace SIE.DataAuth
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
           DataAuthInterceptor.Intercept();
           RT.Service.Register<IVersion, SVersion>();
        }


    }
}
