using SIE.Resources.ShiftTypes;
using SIE.Web.Command;

namespace SIE.Web.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    [JsCommand("SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeWeekAddCommand")]
    public class CalendarSchemeWeekAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var shiftType = RT.Service.Resolve<ShiftTypeController>().GetDefaultShiftType();
            return shiftType;
        }
    }
}
