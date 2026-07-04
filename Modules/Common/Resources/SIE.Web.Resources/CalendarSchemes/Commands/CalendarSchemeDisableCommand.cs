using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 禁用
    /// </summary>
    [JsCommand("SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeDisableCommand")]
    public class CalendarSchemeDisableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var calendarSchemes = args.Data.ToJsonObject<List<CalendarScheme>>();
            RT.Service.Resolve<CalendarSchemeController>().DisableCalendarScheme(calendarSchemes);
            calendarSchemes.ForEach(x => { x.IsEnable = YesNo.No; x.PersistenceStatus = PersistenceStatus.Unchanged; });
            return true;
        }
    }
}
