using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.Andon;
using SIE.Wpf.Andon.Editors;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 模块
    /// </summary>
    class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            AddNewPropertyEditor(app);
        }

        /// <summary>
        /// 模块操作
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_ModuleOperations(object sender, System.EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "安灯管理",
                EntityType = typeof(AndonManageViewModel),
                BlocksTemplate = typeof(AndonManageUITemplate)
            }
            , new WPFModuleMeta()
            {
                Label = "安灯区域管理",
                EntityType = typeof(AndonRegionViewModel),
                BlocksTemplate = typeof(AndonRegionUITemplate)
            }
            );
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        private void AddNewPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(DefectMultiSelectEditor.EditorName, typeof(DefectMultiSelectEditor));
            };
        }
    }
}