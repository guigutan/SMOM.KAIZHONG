using System;
using System.Globalization;

namespace SIE.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// 通用控制器
    /// </summary>
    public class CommonController : DomainController
    {
        /// <summary>
        /// 获取指定日期在一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrns>返回第几周</reutrns>
        public virtual int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        /// <summary>
        /// 根据一年中的第几周获取该周的开始日期与结束日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="weekNumber">周</param>
        /// <param name="culture">区域</param>
        /// <returns>日期组</returns>
        public virtual Tuple<DateTime, DateTime> GetFirstEndDayOfWeek(int year, int weekNumber)
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
            DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber - 1);
            const DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

            while (targetDay.DayOfWeek != firstDayOfWeek)
            {
                targetDay = targetDay.AddDays(firstDayOfWeek - targetDay.DayOfWeek);
            }

            return Tuple.Create(targetDay, targetDay.AddDays(6));
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns>第一天</returns>
        public virtual DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        //// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns>最后一天</returns>
        public virtual DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

    }
}
