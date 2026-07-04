using SIE.Resources.Holidays;
using SIE.Web.Command;
using System;

namespace SIE.Web.Resources.Holidays.Commands
{
    /// <summary>
    /// 法定假期添加命令
    /// </summary>
    public class HolidayAddCommand : ViewCommand
    {
        /// <summary>
        /// 法定假期添加命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var holiday = args.Data.ToJsonObject<Holiday>();
            holiday.BeginDate = DateTime.Today;
            holiday.EndDate = DateTime.Today.AddDays(1);
            return holiday;
        }
    }
}
