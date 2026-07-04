using SIE.MetaModel;
using SIE.Modules;
using SIE.WorkBenchCommon.Workbench.Chatting;
using SIE.WorkBenchCommon.Workbench.Concerns;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using SIE.WorkBenchCommon.Workbench.Tasks;
using SIE.Wpf.WorkBenchCommon;
using SIE.Wpf.WorkBenchCommon.Editors;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Wpf.WorkBenchCommon
{
    /// <summary>
    /// 模块定义
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
            AddPropertyEditor(app);
        }

        /// <summary>
        /// 模块操作
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "KPI目标",
                EntityType = typeof(KpiModel),
            }, new WPFModuleMeta()
            {
                Label = "工作任务",
                EntityType = typeof(TaskInfo),
            }, new WPFModuleMeta()
            {
                Label = "工作任务类型",
                EntityType = typeof(TaskType),
            }, new WPFModuleMeta()
            {
                Label = "工作台关注",
                EntityType = typeof(ConcernsInfo),
            }, new WPFModuleMeta()
            {
                Label = "工作台聊天消息",
                EntityType = typeof(ChatRecord),
            }, new WPFModuleMeta()
            {
                Label = "KPI目标设定".L10N(),
                EntityType = typeof(QuotaTargetSetting)
            }, new WPFModuleMeta()
            {
                Label = "目标达成率预警设定".L10N(),
                EntityType = typeof(TargetWarnSetting)
            });
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        private void AddPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(DimensionEditor.EditorName, typeof(DimensionEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(QuotaCategoryEditor.EditorName, typeof(QuotaCategoryEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(QuotaNameEditor.EditorName, typeof(QuotaNameEditor));
            };
        }
    }
}