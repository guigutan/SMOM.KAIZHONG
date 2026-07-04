using SIE.MES.Report.BatchTracebacks;
using SIE.MES.Report.BatchWipProducts;
using SIE.MES.Report.EmployeeReports.ClockingIns;
using SIE.MES.Report.EmployeeReports.Vacancies;
using SIE.MES.Report.WipProducts;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.MES.Report;
using SIE.Web.MES.Report.EmployeeReports;
using SIE.Web.MES.TeamManagement.ClockingIns;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.MES.Report
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "生产通用报表".L10N(),
                EntityType = typeof(WipProductVersionReport)
            }, new WebModuleMeta()
            {
                Label = "批次生产通用报表".L10N(),
                EntityType = typeof(BatchWipProductVersionReport)
            }, new WebModuleMeta
            {
                Label = "员工出勤".L10N(),
                EntityType = typeof(EmployeeClockInReport),
                ViewGroup = EmployeeClockInViewConfig.CusListView
            }, new WebModuleMeta
            {
                Label = "班组缺编统计".L10N(),
                EntityType = typeof(WorkGroupVacancyReport),
                ViewGroup = WorkGroupVacancyReportViewConfig.WorkGroupReportView
            }, new WebModuleMeta
            {
                EntityType = typeof(BatchTracebackReport),
                Label = "批次追溯通用报表".L10N()
            });
        }
    }
}
