using System;

namespace SIE.WorkBenchCommon
{
    /// <summary>
    /// 日期类
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// 根据一年中的第几周获取该周的开始日期与结束日期
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="weekNumber">周数</param>
        /// <returns>返回开始日期与结束日期</returns>
        public static Tuple<DateTime, DateTime> GetFirstEndDayOfWeek(int year, int weekNumber)
        {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InstalledUICulture;
            System.Globalization.Calendar calendar = culture.Calendar;
            DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
            DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber - 1);
            DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
            while (targetDay.DayOfWeek != firstDayOfWeek)
            {
                targetDay = targetDay.AddDays(-1);
            }

            return Tuple.Create<DateTime, DateTime>(targetDay, targetDay.AddDays(6));
        }
    }
}
