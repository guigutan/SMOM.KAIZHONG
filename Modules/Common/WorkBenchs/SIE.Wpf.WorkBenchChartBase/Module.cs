using SIE.Modules;
using System;
using SIE.Wpf.WorkBenchChartBase;

[assembly: Module(typeof(Module))]
namespace SIE.Wpf.WorkBenchChartBase
{
    /// <summary>
    /// 模块定义
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
        }

        /// <summary>
        /// 模块操作
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            //模块操作
        }
    }
} 