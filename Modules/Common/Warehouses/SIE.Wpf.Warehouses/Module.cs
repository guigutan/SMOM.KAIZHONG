using SIE.MetaModel;
using System;
using SIE.Modules;
using SIE.Warehouses;
using SIE.Wpf.Warehouses.Editors;

[assembly: Module(typeof(SIE.Wpf.Warehouses.Module))]

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    internal class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations; //模块
            AddNewPropertyEditor(app);
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">IApp</param>
        private static void AddNewPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(AreaLocationLookUpEditor.EditorName, typeof(AreaLocationLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(WarehouseLookUpEditor.EditorName, typeof(WarehouseLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(AvailableWarehouseEditor.EditorName, typeof(AvailableWarehouseEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(RoHsGradeEditor.EditorName, typeof(RoHsGradeEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(StorageTemperatureEditor.EditorName, typeof(StorageTemperatureEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(StorageHumidityEditor.EditorName, typeof(StorageHumidityEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(StorageAreaEditor.EditorName, typeof(StorageAreaEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PrintTemplateEditor.EditorName, typeof(PrintTemplateEditor));
            };
        }

        /// <summary>
        /// 模块菜单定义
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "仓库".L10N(),
                EntityType = typeof(Warehouse)
            },
            new WPFModuleMeta()
            {
                Label = "库位".L10N(),
                EntityType = typeof(StorageLocation)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "库区".L10N(),
                EntityType = typeof(StorageArea)
            });
        }
    }
}
