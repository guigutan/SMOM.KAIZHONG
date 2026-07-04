using SIE.Modules;
using SIE.Rbac.Security.Authenticates;
using SIE.Web.KZ.RBAC;
using SIE.Web.KZ.RBAC.SSO;
using System;
[assembly: Module(typeof(Module))]

namespace SIE.Web.KZ.RBAC
{

    /// <summary>
    /// 视图插件
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 重写插件初始化方法
        /// </summary>
        /// <param name="app">IApp</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }
        /// <summary>
        /// 程序模块初始化
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
        }
    }
}
