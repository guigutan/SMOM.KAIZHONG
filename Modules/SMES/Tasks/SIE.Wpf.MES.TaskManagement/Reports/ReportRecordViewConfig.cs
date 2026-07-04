using SIE.MES.TaskManagement.Reports;
using SIE.Wpf.Common.Configs;
using SIE.Wpf.MES.TaskManagement.Reports.Commands;

namespace SIE.Wpf.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录 视图配置
    /// </summary>
    internal class ReportRecordViewConfig : WPFViewConfig<ReportRecord> 
    {
        protected override void ConfigListView() 
        {
            View.DisableEditing();
            View.RemoveCommands(typeof(ModuleConfigCommand));
            View.UseCommands(typeof(ReportRecordPrintCommand));
            View.Property(p => p.DispatchTask.No).Show().HasLabel("任务单");
            View.Property(p => p.Principal.Name).Show().HasLabel("责任人");
            View.Property(p => p.ReportQty).Show().HasLabel("报工数");
            View.Property(p => p.OkQty).Show().HasLabel("合格数");
            View.Property(p => p.NgQty).Show().HasLabel("不合格数");
            View.Property(p => p.Hour).Show().HasLabel("统计工时");
            View.Property(p => p.Station).Show().HasLabel("工位");
            View.Property(p => p.BatchNo).Show().HasLabel("批次号");
            View.Property(p => p.WorkGroup.Name).Show().HasLabel("班组");
            View.Property(p => p.Shift.Name).Show().HasLabel("班次");
            View.Property(p => p.ReportTime).Show().HasLabel("报工时间");
            View.Property(p => p.Remark).Show().HasLabel("备注");
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.Defects).Show(ChildShowInWhere.Hide);
        }
    }
}
