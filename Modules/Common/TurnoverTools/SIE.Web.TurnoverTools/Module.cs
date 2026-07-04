using SIE.TurnoverTools.TurnoverTools;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Kit.TurnoverTools;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Kit.TurnoverTools
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

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                EntityType = typeof(TurnoverToolModel),
                Label = "周转工具型号维护",
            }, new WebModuleMeta()
            {
                EntityType = typeof(TurnoverTool),
                Label = "周转工具",
            });
        }
    }
}
