using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.Kit.APS.EngineerPlans;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Kit.APS.FactoryPlanQtys;
using SIE.Kit.APS.ProductLocations;
using SIE.Kit.APS.TargetCapacitys;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Kit.APS;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Kit.APS
{
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 配置菜单
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "工程节假日维护",
                EntityType = typeof(HolidaySetting),
            });

            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "客户等级设置",
                EntityType = typeof(CustLevelSetting),
            });

            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "等级日排产上限",
                EntityType = typeof(CustLevel),
            });

            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "工程计划",
                EntityType = typeof(EngineerPlan)
            });
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "产品定位",
                EntityType = typeof(ProductLocation)
            });
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "目标产能",
                EntityType = typeof(TargetCapacity)
            });
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "厂别确认",
                EntityType = typeof(FactoryConfirmsViewModel),
                UIGenerator = "SIE.Web.Kit.APS.FactoryConfirms.FactoryConfirmUIGenerator"

            });
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "工厂计划数配置",
                EntityType = typeof(FactoryPlanQty),
            });
        }
    }
}
