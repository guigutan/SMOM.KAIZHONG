using SIE.DataTrace.Base;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.DataTrace.Base
{
    /// <summary>
    /// 模块设置
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
            app.StartupCompleted += App_StartupCompleted;
        }

        /// <summary>
        /// app启动后事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {

        }
    }
}
