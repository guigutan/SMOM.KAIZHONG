using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.ShiftTypes;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 添加周方案命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = CommandGroupType.Edit)]
    class CalendarSchemeWeekAddCommand : ListAddCommand
    {
        /// <summary>
        /// 实体创建后
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            var calendarSchemeWeek = entity.CastTo<CalendarSchemeWeek>();
            calendarSchemeWeek.ActiveDate = DateTime.Today.AddDays(1);
            calendarSchemeWeek.ShiftType= RT.Service.Resolve<ShiftTypeController>().GetDefaultShiftType();
        }
    }
}
