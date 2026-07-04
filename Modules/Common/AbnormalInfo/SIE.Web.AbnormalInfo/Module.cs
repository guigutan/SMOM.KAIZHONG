using SIE.AbnormalInfo.AbnormalInfos;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.AbnormalInfo;
using SIE.Web.AbnormalInfo.Reports;
using SIE.Web.AbnormalInfo.Reports.ViewModels;
using System;
using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports;

[assembly: Module(typeof(Module))]
namespace SIE.Web.AbnormalInfo
{
    /// <summary>
    /// UI模块
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 配置菜单
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    Label = "异常来源",
                    EntityType = typeof(SIE.AbnormalInfo.AbnormalMonitors.AbnormalSource)
                },
                new WebModuleMeta()
                {
                    Label = "异常定义",
                    EntityType = typeof(AbnormalInfoDefinition)
                },
                 new WebModuleMeta()
                 {
                     Label = "异常管理",
                     EntityType = typeof(AbnormalInfor)
                 },
                 new WebModuleMeta()
                 {
                     Label = "异常报表",
                     EntityType = typeof(AbnormalInfoReportViewModel),
                     BlocksTemplate = typeof(AbnormalInfoReportUITemplate)
                 },
                 new WebModuleMeta()
                 {
                     Label = "异常判定规则",
                     EntityType = typeof(AbnormalDecisionRule)
                 },
                 new WebModuleMeta()
                 {
                     Label = "异常定义(新)",
                     EntityType = typeof(AbnormalDefine)
                 },
                 new WebModuleMeta()
                 {
                     Label = "异常时效看板".L10N(),
                     EntityType = typeof(TimelinessAbnormityReportsViewModel),
                     UIGenerator = "SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.Scripts.TimelinessAbnormityReportUIGenerator"
                 },
                new WebModuleMeta()
                {
                    Label = "异常清单",
                    EntityType = typeof(AbnormalMonitorInventory)
                },
                 new WebModuleMeta()
                 {
                     Label = "异常任务",
                     EntityType = typeof(AbnormalMonitorTask)
                 },
                 new WebModuleMeta()
                 {
                     Label = "异常预警定义",
                     EntityType = typeof(AbnormalWarnDefine)
                 }
                );
        }
    }
}
