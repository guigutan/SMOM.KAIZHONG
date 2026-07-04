using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Kit.UrgentOrder;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Kit.UrgentOrder
{
    /// <summary>
    /// UIModule
    /// </summary>
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
            CommonModel.Modules.AddModules(
            new WebModuleMeta()
            {
                Label = "物料加急单维护",
                EntityType = typeof(ItemUrgentOrder)
            });
        }
    }
}
