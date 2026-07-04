using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 周日历删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", GroupType = CommandGroupType.Edit)]
    public class CalendarSchemeWeekDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var calendarSchemeWeek = view.Current as CalendarSchemeWeek;
            if (calendarSchemeWeek == null || calendarSchemeWeek.ActiveDate <= DateTime.Today
                || (view.Data.Count == 1 && (view.Parent.Current as CalendarScheme).IsEnable == YesNo.Yes))
            {
                return false;
            }

            return true;
        }
    }
}
