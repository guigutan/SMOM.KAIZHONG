using SIE.Common.Prints;
using SIE.Core.Prints;
using SIE.KZ.Print;
using SIE.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Module(typeof(Module))]
namespace SIE.KZ.Print
{
    /// <summary>
    /// 模块配置
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
            HostReportFactory.Current.Register(new SieDevHostReport());
            RT.Service.Register<IPrint, PrinterController>();

            app.StartupCompleted += App_StartupCompleted;
            app.Exit += App_Exit;
        }

        /// <summary>
        /// 程序退出
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_Exit(object sender, EventArgs e)
        {
            PrintListenter.Instance.Stop();
        }

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            PrintListenter.Instance.Start();
        }
    }
}
