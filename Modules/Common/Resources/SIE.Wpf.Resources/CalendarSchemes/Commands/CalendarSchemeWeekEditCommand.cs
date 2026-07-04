using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 修改
    /// </summary>
    [Command(ImageName = "EditEntity", Label = "修改", GroupType = CommandGroupType.Edit)]
    public class CalendarSchemeWeekEditCommand : ListEditCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var calendarSchemeWeek = view.Current as CalendarSchemeWeek;
            return calendarSchemeWeek != null && calendarSchemeWeek.ActiveDate > DateTime.Today;
        }
    }
}
