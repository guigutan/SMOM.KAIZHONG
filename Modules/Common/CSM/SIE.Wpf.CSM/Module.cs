using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.CSM.Edtors;
using System;

[assembly: Module(typeof(SIE.Wpf.CSM.Module))]

namespace SIE.Wpf.CSM
{
    /// <summary>
    /// UI
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 程序
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            AddNewPropertyEditor(app);
        }

        /// <summary>
        /// 配置菜单
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "客户".L10N(),
                EntityType = typeof(Customer)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "供应商".L10N(),
                EntityType = typeof(Supplier)
            });
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">IApp</param>
        private static void AddNewPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(RegionalInfoEditor.EditorName, typeof(RegionalInfoEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CustomerLookupEditor.EditorName, typeof(CustomerLookupEditor));
            };
        }
    }
}