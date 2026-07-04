using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Records;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.EMS.EquipMaint;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS.EquipMaint
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                EntityType = typeof(MaintainPlanViewModel),
                Label = "设备保养计划维护".L10N()
            }, new WebModuleMeta()
            {
                EntityType = typeof(MaintainRecord),
                Label = "设备保养记录".L10N()
            }
            );
        }
    }
}
