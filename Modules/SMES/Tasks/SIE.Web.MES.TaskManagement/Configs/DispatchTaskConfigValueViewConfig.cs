using SIE.MES.TaskManagement.Configs;

namespace SIE.Web.MES.TaskManagement.Configs
{
    /// <summary>
    /// 派工任务单配置值视图配置
    /// </summary>
    internal class DispatchTaskConfigValueViewConfig : WebViewConfig<DispatchTaskConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsGenerate);
            View.Property(p => p.GenerateMode).UseEnumEditor(p => p.AllowBlank = true).Readonly(p => !p.IsGenerate)
                        .UseListSetting(e => { e.HelpInfo = "生成工单任务单可编辑"; });
            View.Property(p => p.NumberRule);
            View.Property(p => p.ReportOrder);
            View.Property(p => p.PrintBillRule).Show().UseDispatchTaskBillEditor();
        }
    }
}