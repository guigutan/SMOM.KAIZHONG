using SIE.MetaModel;
using SIE.Modules;
using SIE.ShipPlan;
using SIE.Web.ShipPlan;
using System;

[assembly: Module(typeof(SIE.Web.ShipPlan.Module))]

namespace SIE.Web.ShipPlan
{
    /// <summary>
    /// Module
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app"></param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    EntityType = typeof(AssignWarehouseRule),
                    Label = "分配仓库规则",
                }, new WebModuleMeta()
                {
                    EntityType = typeof(DeliveryPlan),
                    Label = "发货计划",
                }
                );
        }
    }
}