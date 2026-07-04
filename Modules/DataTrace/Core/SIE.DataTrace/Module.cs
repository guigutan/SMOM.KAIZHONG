using SIE.DataTrace;
using SIE.DataTrace.Activities.MessageQueue;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.DataTrace
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
            app.AllModulesIntialized += All_ModulesIntialized;
        }

        /// <summary>
        /// app启动后事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void All_ModulesIntialized(object sender, EventArgs e)
        {
            DataTraceWorkFlowListener.Instance.Start();
        }
    }
}
