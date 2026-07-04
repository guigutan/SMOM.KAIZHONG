using SIE.Items;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.Items;
using SIE.Wpf.Items.Editors;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
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
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemCateLevelEditor.EditorName, typeof(ItemCateLevelEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CatalogLookUpEditor.EditorName, typeof(CatalogLookUpEditor)); AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PropertyValueEditor.EditorName, typeof(PropertyValueEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(AlternativeEditor.EditorName, typeof(AlternativeEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PBDPDefinitionLookUpEditor.EditorName, typeof(PBDPDefinitionLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PBDPValueLookUpEditor.EditorName, typeof(PBDPValueLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PBPDefinitionLookUpEditor.EditorName, typeof(PBPDefinitionLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PBPValueLookUpEditor.EditorName, typeof(PBPValueLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemSmallCategoryEditor.EditorName, typeof(ItemSmallCategoryEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemQualitySmallCategoryEditor.EditorName, typeof(ItemQualitySmallCategoryEditor));
                //AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ProductQualitySmallCategoryEditor.EditorName, typeof(ProductQualitySmallCategoryEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemTypeEditor.EditorName, typeof(ItemTypeEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemPropertyEditor.EditorName, typeof(ItemPropertyEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(UnitInputEditor.EditorName, typeof(UnitInputEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(WorkGroupEmployeeLookUpEditor.EditorName, typeof(WorkGroupEmployeeLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemCategoryLookUpEditor.EditorName, typeof(ItemCategoryLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(NumberRuleLookupEditor.EditorName, typeof(NumberRuleLookupEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BarcodeModelLookupEditor.EditorName, typeof(BarcodeModelLookupEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PackageModelLookupEditor.EditorName, typeof(PackageModelLookupEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ProductModelLineCapacityLookUpEditor.EditorName, typeof(ProductModelLineCapacityLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(RangeUnitInputEditor.EditorName, typeof(RangeUnitInputEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CatalogProductGradeLookupEditor.EditorName, typeof(CatalogProductGradeLookupEditor));
            };
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "单位".L10N(),
                EntityType = typeof(Unit)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "分类层级".L10N(),
                EntityType = typeof(ItemCategoryLevel)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "分类".L10N(),
                EntityType = typeof(ItemCategory)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "物料属性定义".L10N(),
                EntityType = typeof(ItemPropertyDefinition)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "产品族".L10N(),
                EntityType = typeof(ProductFamily)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "产品机型".L10N(),
                EntityType = typeof(ProductModel)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "物料".L10N(),
                EntityType = typeof(Item)
            });
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "产品BOM".L10N(),
                EntityType = typeof(ProductBom)
            });
        }
    }
}
