using SIE.MetaModel;
using SIE.Modules;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.Holidays;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Wpf.Resources;
using SIE.Wpf.Resources.CalendarSchemes.Editors;
using SIE.Wpf.Resources.CalendarSchemes.Templates;
using SIE.Wpf.Resources.Editors;
using SIE.Wpf.Resources.WipResources;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.Resources
{
    /// <summary>
    /// 视图插件
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 重写插件初始化方法
        /// </summary>
        /// <param name="app">IApp</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            AddNewPropertyEditor(app);
        }

        /// <summary>
        /// 程序模块初始化
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "企业模型".L10N(),
                EntityType = typeof(Enterprise)
            }, new WPFModuleMeta()
            {
                Label = "企业层级".L10N(),
                EntityType = typeof(EnterpriseLevel)
            }, new WPFModuleMeta()
            {
                Label = "员工维护".L10N(),
                EntityType = typeof(Employee)
            },
            //new WPFModuleMeta()
            //{
            //    Label = "班组维护".L10N(),
            //    EntityType = typeof(WorkGroup)
            //},
            new WPFModuleMeta
            {
                Label = "班制".L10N(),
                EntityType = typeof(ShiftType)
            }, new WPFModuleMeta
            {
                Label = "日历方案".L10N(),
                EntityType = typeof(CalendarScheme),
                BlocksTemplate = typeof(CalendarSchemeTemplate)
            }, new WPFModuleMeta
            {
                Label = "生产资源".L10N(),
                EntityType = typeof(WipResource),
                BlocksTemplate = typeof(ResourceTemplate)
            }, new WPFModuleMeta
            {
                Label = "法定假期".L10N(),
                EntityType = typeof(Holiday),
            }, new WPFModuleMeta
            {
                Label = "制程工艺类型".L10N(),
                EntityType = typeof(ProcessTechType),
            }, new WPFModuleMeta
            {
                Label = "制程工艺".L10N(),
                EntityType = typeof(ProcessTech),
            }, new WPFModuleMeta()
            {
                Label = "工段".L10N(),
                EntityType = typeof(ProcessSegment)
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
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ImagePropertyEditor.EditorName, typeof(ImagePropertyEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(LineLookUpEditor.EditorName, typeof(LineLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ShopLookUpEditor.EditorName, typeof(ShopLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(DepartmentpLookUpEditor.EditorName, typeof(DepartmentpLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(EnterpriseEditor.EditorName, typeof(EnterpriseEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ResourceEditor.EditorName, typeof(ResourceEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(AreaLookUpEditor.EditorName, typeof(AreaLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ShiftTypeEditor.EditorName, typeof(ShiftTypeEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ResourceWorkShopEditor.EditorName, typeof(ResourceWorkShopEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CustomResourceEditor.EditorName, typeof(CustomResourceEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(EquipmentResourceEditor.EditorName, typeof(EquipmentResourceEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(EnterpriseResourceEditor.EditorName, typeof(EnterpriseResourceEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(WipResourceEditor.EditorName, typeof(WipResourceEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(CustomEnumEditor.EditorName, typeof(CustomEnumEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(SchemeLookUpEditor.EditorName, typeof(SchemeLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(WipResourceCodeLookUpEditor.EditorName, typeof(WipResourceCodeLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(EnterpriseEquipmentResourceEditor.EditorName, typeof(EnterpriseEquipmentResourceEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(FactoryLookUpEditor.EditorName, typeof(FactoryLookUpEditor));
            };
        }
    }
}