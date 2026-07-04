using SIE.CSM.Customers;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.CSM.Suppliers;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.CSM;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.CSM
{
    /// <summary>
    /// 菜单模型
    /// </summary>
    class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app"></param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 菜单初始化
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    EntityType = typeof(Supplier),
                    Label = "供应商".L10N(),
                }, new WebModuleMeta()
                {
                    EntityType = typeof(Customer),
                    Label = "客户".L10N(),
                }, new WebModuleMeta()
                {
                    EntityType = typeof(ItemInspCharacteristics),
                    Label = "物料检验特性".L10N(),
                }
                );
        }
    }
}
