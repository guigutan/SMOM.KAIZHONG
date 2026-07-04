using SIE.MetaModel;
using SIE.Modules;
using SIE.SO.SaleOrders;
using SIE.Web.SO;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.SO
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
                Label = "销售订单",
                EntityType = typeof(SaleOrder)
            });
        }
    }
}

