using SIE.EMS.Report.EquipCostAnalyses;
using SIE.EMS.Report.EquipmentIntegrateStatistics;
using SIE.EMS.Report.EquipmentMixReport;
using SIE.EMS.Report.FaultCountReports;
using SIE.EMS.Report.MttrAndMtbfReports;
using SIE.EMS.Report.SparePartMitReports;
using SIE.EMS.Report.WorkOrderExcuteReports;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.EMS.Report;
using SIE.Web.EMS.Report.EquipCostAnalyses;
using SIE.Web.EMS.Report.EquipmentIntegrateStatistics;
using SIE.Web.EMS.Report.MttrAndMtbfReports;
using SIE.Web.EMS.Report.SparePartMitReports;
using SIE.Web.EMS.Report.WorkOrderExcuteReports;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS.Report
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
            WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                 new WebModuleMeta()
                 {
                     Label = "设备综合统计报表(年)".L10N(),
                     EntityType = typeof(EquipmentMixReportMonViewModel),
                     BlocksTemplate = typeof(EquipmentMixReport.MixReportUITemplate)
                 },
                new WebModuleMeta()
                {
                    EntityType = typeof(MttrAndMtbfReportViewModel),
                    BlocksTemplate = typeof(MttrAndMtbfReportUITemplate),
                    Label = "MTTR/MTBF统计报表".L10N()
                }, new WebModuleMeta()
                {
                    Label = "设备综合统计".L10N(),
                    EntityType = typeof(EquipmentIntegrateStatistic),
                    BlocksTemplate = typeof(EquipIntStatisticsUITemplate)
                },
                 new WebModuleMeta()
                 {
                     Label = "设备成本分析".L10N(),
                     EntityType = typeof(EquipCostAnalyse),
                     BlocksTemplate = typeof(EquipCostAnalysesReportUITemplate)
                 }, new WebModuleMeta()
                 {
                     Label = "故障统计报表".L10N(),
                     EntityType = typeof(FaultCountReport)
                 },
                 new WebModuleMeta()
                 {
                     Label = "备件库综合报表".L10N(),
                     EntityType = typeof(SparePartMixtReportViewModel),
                     BlocksTemplate = typeof(SparePartMitReportsUITemplate)
                 },
                 new WebModuleMeta()
                 {
                     Label = "工单执行统计报表".L10N(),
                     EntityType = typeof(WorkOrderExcuteReportViewModel),
                     BlocksTemplate = typeof(WorkOrderExcuteReportUITemplate)
                 }
            );
        }
    }
}
