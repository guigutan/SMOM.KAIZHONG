using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using System;

namespace SIE.xUnit.Resources.CalendarSchemes
{
    /// <summary>
    /// 日历方案测试 控制器
    /// </summary>
    public class CalendarSchemeTestController : DomainController
    {
        /// <summary>
        /// 根据日历方案的名称获取日历方案
        /// </summary>
        /// <param name="name">日历方案的名称</param>
        /// <returns>返回日历方案</returns>
        public virtual CalendarScheme GetCalendarScheme(string name)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(CalendarScheme.SchemeWeeksProperty);
            var query = Query<CalendarScheme>().Where(p => p.Name == name);

            return query.FirstOrDefault(elo);
        }

        /// <summary>
        /// 创建日历方案
        /// </summary>
        /// <param name="isDefault">是否创建默认的日历方案</param>
        /// <returns>返回日历方案</returns>
        public virtual CalendarScheme CreateCalendarScheme(bool isDefault)
        {
            string defaultName = "常规日历";
            string weekName = "常规周方案";
            CalendarScheme calendarScheme = null;
            if (isDefault)
            {
                calendarScheme = GetCalendarScheme(defaultName);
            }

            if (calendarScheme == null)
            {
                // 创建日历方案
                calendarScheme = new CalendarScheme();
                calendarScheme.GenerateId();
                calendarScheme.Name = isDefault ? defaultName : (defaultName + calendarScheme.Id);

                // 创建周方案
                CalendarSchemeWeek schWeek = new CalendarSchemeWeek();
                schWeek.GenerateId();
                schWeek.SchemeId = calendarScheme.Id;
                schWeek.Name = isDefault ? weekName : (weekName + schWeek.Id);
                schWeek.ActiveDate = DateTime.Now.AddDays(1);
                schWeek.Mon = true;
                schWeek.Tue = true;
                schWeek.Wed = true;
                schWeek.Thu = true;
                schWeek.Fri = true;
                calendarScheme.SchemeWeeks.Add(schWeek);
            }

            return calendarScheme;
        }
    }
}