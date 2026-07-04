using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.ExperienceDepots;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.EMS.EquipRepair;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS.EquipRepair
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
            CommonModel.Modules.AddModules(
                new WebModuleMeta(){
                    EntityType = typeof(ExperienceDepot),
                    Label = "维修经验库".L10N()
                },
                 new WebModuleMeta()
                 {
                     EntityType = typeof(EquipRepairBill),
                     Label = "维修管理".L10N()
                 },
                 new WebModuleMeta()
                 {
                     EntityType = typeof(PlanRepair),
                     Label = "计划维修".L10N()
                 }
            );
        }
    }
}
