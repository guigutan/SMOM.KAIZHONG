using SIE.MES.Common.HomeMenusConfigs;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.MES.Common;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.MES.Common
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "触摸屏首页设置".L10N(),
                EntityType = typeof(HomeMenusConfig),
            });
        }
    }
}