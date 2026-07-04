using SIE.Kit.ReCheck;
using SIE.Kit.ReCheck.RecheckEvents;
using SIE.Kit.ReCheck.RecheckInspBills;
using SIE.Modules;
using SIE.Recheck.RecheckEvents;
using SIE.Recheck.RecheckInspBills;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Kit.ReCheck
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
        }
        /// <summary>
        /// app启动后事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            RT.Service.Register<RecheckBillEventController, KitRecheckBillEventController>();      //注入套件-超期报检控制器
            RT.Service.Register<RecheckInspBillController, KitRecheckInspBillController>();      //注入套件-超期复检控制器
        }
    }
}
