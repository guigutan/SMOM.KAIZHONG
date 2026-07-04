using SIE.MES.TeamManagement.OnLoans;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.MES.TeamManagement;
using System;
[assembly: Module(typeof(Module))]

namespace SIE.Wpf.MES.TeamManagement
{
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
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
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "借调单",
                EntityType = typeof(WorkGroupOnLoan)
            }
            );
        }
    }
}