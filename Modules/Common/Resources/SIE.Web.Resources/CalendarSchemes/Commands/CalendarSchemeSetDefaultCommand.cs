using SIE.Resources.CalendarSchemes;
using SIE.Web.Command;
using System;

namespace SIE.Web.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 设置缺省
    /// </summary>
    [JsCommand("SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeSetDefaultCommand")]
    public class CalendarSchemeSetDefaultCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var calendarScheme = args.Data.ToJsonObject<CalendarScheme>();
            if (null == calendarScheme)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(calendarScheme)));
            }
            RT.Service.Resolve<CalendarSchemeController>().SetDefault(calendarScheme);
            return true;
        }
    }
}
