using SIE.Kit.UrgentOrder;
using SIE.Kit.UrgentOrder.Events;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Kit.UrgentOrder
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    public class Module : DomainModule 
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序对象</param>
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
            ItemUrgentOrderListener.Instance.Start();
        }
    }
}
