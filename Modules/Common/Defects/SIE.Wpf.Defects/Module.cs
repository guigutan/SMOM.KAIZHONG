using SIE.Defects;
using SIE.Defects.InspectionItems;
using SIE.Defects.Measures;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.Defects;
using SIE.Wpf.Defects.Editors;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.Defects
{
    /// <summary>
    /// UI模块
    /// </summary>
    class Module : UIModule
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
        /// 注册属性编辑器
        /// </summary>
        /// <param name="app">app</param>
        private void AddNewPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(LimitMaxEditor.EditorName, typeof(LimitMaxEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(LimitLowEditor.EditorName, typeof(LimitLowEditor));
            };
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "缺陷代码".L10N(),
                EntityType = typeof(Defect)
            }, new WPFModuleMeta
            {
                Label = "缺陷代码分类".L10N(),
                EntityType = typeof(DefectCategory)
            }, new WPFModuleMeta
            {
                Label = "缺陷责任".L10N(),
                EntityType = typeof(DefectResponsibility)
            }, new WPFModuleMeta
            {
                Label = "缺陷责任分类".L10N(),
                EntityType = typeof(DefectResponsibilityCategory)
            },
            new WPFModuleMeta
            {
                Label = "检验方式".L10N(),
                EntityType = typeof(InspectionMode)
            }, new WPFModuleMeta
            {
                Label = "维修措施".L10N(),
                EntityType = typeof(RepairMeasure)
            });
        }
    }
}