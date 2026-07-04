using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.Kit.IQC;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Kit.IQC
{
    /// <summary>
    /// UI模块
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            //WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 配置菜单
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
         
        }
    }
}
