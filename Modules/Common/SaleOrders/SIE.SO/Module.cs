using SIE.Modules;
using SIE.SO;
using SIE.SO.SaleOrders;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.SO
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
            app.StartupCompleted += App_StartupCompleted;
        }

        /// <summary>
        /// 程序启动完成事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            InitEventListener();
        }

        /// <summary>
        /// 初始化事件监听
        /// </summary>
        private void InitEventListener()
        {
            CallBackPromiseDeliveryListener.Instance.Start();
        }
    }
}