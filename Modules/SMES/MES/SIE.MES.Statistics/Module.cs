using SIE.EventMessages.MES.ProcessStatistics;
using SIE.MES.Statistics;
using SIE.MES.Statistics.WIP;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.MES.Statistics
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.StartupCompleted += App_StartupCompleted;
            app.Exit += App_Exit;
            RT.Service.Register<IProcessStatistics, WipStatisticsController>();
        }

        /// <summary>
        /// 程序退出，设置程序退出标识
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_Exit(object sender, EventArgs e)
        {
            WipCollectedManager.Instance.Flag = true;
        }

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            WipStatisticsListenter.Instance.Start();
        }
    }
}
