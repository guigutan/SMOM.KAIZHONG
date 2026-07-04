using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.MES.TaskManagement;
using SIE.Wpf.MES.TaskManagement.KZReports;
using SIE.Wpf.MES.TaskManagement.Pressure;
using SIE.Wpf.MES.TaskManagement.Reports;
using SIE.Wpf.MES.TaskManagement.Reports.Editors;
using SIE.Wpf.MES.WIP.Pressure;
using System;
[assembly: Module(typeof(Module))]

namespace SIE.Wpf.MES.TaskManagement
{
    /// <summary>
    /// 配置模块
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
            CommonModel.Modules.AddModules(
                //new WPFModuleMeta()
                //{
                //    Label = "任务单报工",
                //    EntityType = typeof(TaskReportViewModel),
                //    BlocksTemplate = typeof(TaskReportUITemplate)
                //},
                new WPFModuleMeta()
                {
                    Label = "生产报工",
                    EntityType = typeof(KZTaskReportViewModel),
                    BlocksTemplate = typeof(KZTaskReportUITemplate)
                }, new WPFModuleMeta()
                {
                    Label = "生产报工(押出)",
                    EntityType = typeof(KZTaskReportExtrusionViewModel),
                    BlocksTemplate = typeof(KZTaskReportExtrusionUITemplate)
                }, new WPFModuleMeta()
                {
                    Label = "生产报工(共模)",
                    EntityType = typeof(KZTaskReportCommonModeViewModel),
                    BlocksTemplate = typeof(KZTaskReportCommonModeUITemplate)
                }, new WPFModuleMeta()
                {
                    Label = "生产报工(多工位)",
                    EntityType = typeof(KZTaskReportMultiStationViewModel),
                    BlocksTemplate = typeof(KZTaskReportMultiStationUITemplate)
                }, new WPFModuleMeta()
                {
                    Label = "生产报工(过程数采)",
                    EntityType = typeof(KZTaskReportProcessViewModel),
                    BlocksTemplate = typeof(KZTaskReportProcessUITemplate)
                }, new WPFModuleMeta()
                {
                    Label = "条码打印(KZ)",
                    EntityType = typeof(PressureSnPrintViewModel),
                    BlocksTemplate = typeof(PressureSnPrintUITemplate)
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
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ReportDefectEditor.EditorName, typeof(ReportDefectEditor));
            };
        }
    }
}