using SIE.AbnormalInfo;
using SIE.AbnormalInfo.AbnormalEvent;
using SIE.AbnormalInfo.AbnormalMonitors.Events;
using SIE.EventMessages.AbnormalInfos;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.AbnormalInfo
{
    /// <summary>
    /// 模块配置
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
            RT.Service.Register<IAbnormalMonitor, AbnormalEventController>();
        }
        /// <summary>
        /// app启动后事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            AbnormalMonitorTaskEventListener.Instance.Start();
        }
    }
}
