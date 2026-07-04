using SIE.MES.TeamManagement.ShiftSchedules;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班表视图配置
    /// </summary>
    internal class ShiftScheduleViewConfig : WebViewConfig<ShiftSchedule>
    {
        /// <summary>
        /// 班组排班，界面客制，命令不变
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleCommand", typeof(ScheduleImportCommand).FullName)
                .UseCommands("SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleExportCommand");
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}