using SIE.Barcodes;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Reports;
using System;

namespace SIE.Web.MES.TaskManagement.Configs
{
    /// <summary>
    /// 报工记录配置值视图配置
    /// </summary>
    internal class ReportRecordConfigValueViewConfig : WebViewConfig<ReportRecordConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.ReportBatchNoRule).Show();
            View.Property(p => p.ReportPrintTemplate).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(typeof(ReportRecordPrintable).GetQualifiedName(), p, r);
            }).Show();

            View.Property(p => p.IsValidateReportSingleTask).Show().UseListSetting(p => p.HelpInfo = "报工时是否校验只有一个任务单在执行中（资源+工序+物料编码");

            View.Property(p => p.AllowMultiTaskReportProcess).Show().UseListSetting(p => p.HelpInfo = "多个工序编码使用英文逗号分隔");
        }
    }
}
