using SIE.EventMessages;
using SIE.Kit.APS;
using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.Kit.APS.EngineerPlans;
using SIE.Kit.EventMessages.EngineerPlans;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Kit.APS
{
    /// <summary>
    /// 通用权限
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
            RegisterService();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register<ITaskCompleteSentBackToMIPlan, EngineerPlanController>();
            RT.Service.Register<ICustomerCreated, CustLevelSettingController>();
        }

        /// <summary>
        /// 程序启动完成事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
        }
    }
}


