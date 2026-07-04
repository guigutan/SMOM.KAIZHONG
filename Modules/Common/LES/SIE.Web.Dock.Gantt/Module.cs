using SIE.Modules;
using SIE.Web.Dock.Gantt;
using SIE.Web.Configs;
using SIE.Web.Handlers;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Dock.Gantt
{
    /// <summary>
    /// 模块配置
    /// </summary>
    class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
            JavascriptHandler_Modules.JsResourcesFilterModules.Add(GetType());
            WebResourceConfig.AddResourceEmbeddedModule(GetType());
        }
    }
}