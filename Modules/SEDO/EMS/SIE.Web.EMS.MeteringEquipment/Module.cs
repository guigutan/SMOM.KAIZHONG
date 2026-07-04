using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.EMS.MeteringEquipment;
using SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS.MeteringEquipment
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
            WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                 new WebModuleMeta()
                 {
                     Label = "计量设备台账".L10N(),
                     EntityType = typeof(MeteringEquipmentAccount),
                     ViewGroup = MeteringEquipmentAccountViewConfig.MeteringEquipmentAccountGroup
                 },
                 new WebModuleMeta()
                 {
                     Label = "计量设备定检".L10N(),
                     EntityType = typeof(Calibration),
                 }
            );
        }
    }
}
