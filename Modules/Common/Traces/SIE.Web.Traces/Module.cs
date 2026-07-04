using SIE.MetaModel;
using SIE.Modules;
using SIE.Traces.ForwardTraces;
using SIE.Traces.ReverseTraces;
using SIE.Web.Configs;
using SIE.Web.Traces;

[assembly: Module(typeof(Module))]

namespace SIE.Web.Traces
{
    /// <summary>
    /// Domain模块
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                  new WebModuleMeta
                  {
                      Label = "正向追溯",
                      EntityType = typeof(ForwardTraceViewModel)
                  },
                  new WebModuleMeta
                  {
                      Label = "反向追溯",
                      EntityType = typeof(ReverseTraceViewModel)
                  }
                  );
        }
    }
}
