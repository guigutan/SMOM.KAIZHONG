using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Calendars.CalendarSchemes.Commands
{
    /// <summary>
    /// 修改
    /// </summary>
    [Command(ImageName = "EditEntity", Label = "修改", GroupType = CommandGroupType.Edit)]
    public class CalendarWeekEditCommand : ListEditCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && view.Parent.Current != null && (view.Parent.Current as CalendarSchemeWeek).ActiveDate > DateTime.Today;
        }
    }
}
