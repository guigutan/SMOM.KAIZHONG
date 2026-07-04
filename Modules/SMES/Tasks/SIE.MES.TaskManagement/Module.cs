using SIE.Core.Algorithms.KZ;
using SIE.EventMessages.MES.Dispatchs;
using SIE.EventMessages.MES.Inspection;
using SIE.EventMessages.MES.SuspectProductLabel;
using SIE.EventMessages.Release;
using SIE.MES.Interfaces.TaskManages;
using SIE.MES.TaskManagement;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Events;
using SIE.MES.TaskManagement.Interfaces;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WIP.TaskExtensions;
using SIE.MES.WorkOrderArchives;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.MES.TaskManagement
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
            RT.Service.Register<ITaskManage, TaskManage>();
            RT.Service.Register<IWipTaskReport, WipTaskReport>();
            RT.Service.Register<IWorkOrderTask, ApsTaskController>();
            RT.Service.Register<ITaskReport, ReportController>();
            RT.Service.Register<IWoArchive, ReportArchiveController>();
            RT.Service.Register<ISuspectProductLabel, SuspectProductLabelController>();
            RT.Service.Register<IBydCode, CustomCodeController>();
            RT.Service.Register<IDispatchs, DispatchController>();
            RT.Service.Register<ITaskReportKZ, WipTaskReportKZ>();
            app.StartupCompleted += App_StartupCompleted;
        }

        /// <summary>
        /// 程序启动完成事件
        /// </summary>
        /// <param name="sender">App</param>
        /// <param name="e">参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            TaskManagerListener.Instance.Start();
        }
    }
}