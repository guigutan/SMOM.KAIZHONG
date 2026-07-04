using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.Web.MES.TeamManagement.ShiftSchedules.ViewModels;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班视图模型视图配置
    /// </summary>
    internal class ShiftScheduleViewModelViewConfig : WebViewConfig<ShiftScheduleViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ShiftSchedule));
            View.RequierModels(typeof(ChangeWorkGroupViewModel));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.TeamManagement.ShiftSchedules.RestartCommand"
                , "SIE.Web.MES.TeamManagement.ShiftSchedules.SaveScheduleCommand");
        }
    }
}