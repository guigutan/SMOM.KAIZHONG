using SIE.MES.Workbench.AndonAbnormals;
using SIE.MES.Workbench.EmployeeManages;
using SIE.MES.Workbench.EmployeeMarks;
using SIE.MES.Workbench.Experiences;
using SIE.MES.Workbench.KeyPerformances;
using SIE.MES.Workbench.ProductingReadies;
using SIE.MES.Workbench.StationChecks;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.MES.Workbench;
using SIE.Wpf.MES.Workbench.Editors;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Wpf.MES.Workbench
{
    /// <summary>
    /// 模块
    /// </summary>
    public class Module : UIModule
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
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "车间目标设置",
                EntityType = typeof(ShopTargetSetting)
            }, new WPFModuleMeta()
            {
                Label = "工位当班",
                EntityType = typeof(StationOnDuty)
            }, new WPFModuleMeta()
            {
                Label = "员工出勤",
                EntityType = typeof(EmployeeClockingIn)
            }, new WPFModuleMeta()
            {
                Label = "产前准备",
                EntityType = typeof(ProductingReady)
            }, new WPFModuleMeta()
            {
                Label = "工位点检结果",
                EntityType = typeof(StationCheckResult)
            }, new WPFModuleMeta()
            {
                Label = "个人作业评分",
                EntityType = typeof(EmployeeMark)
            }, new WPFModuleMeta()
            {
                Label = "历史经验库",
                EntityType = typeof(HistoryExperience)
            });
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        private void AddNewPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(AssemblyProcessEditor.EditorName, typeof(AssemblyProcessEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(InspProcessEditor.EditorName, typeof(InspProcessEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(AbnormalTypeCatalogEditor.EditorName, typeof(AbnormalTypeCatalogEditor));
            };
        }
    }
}