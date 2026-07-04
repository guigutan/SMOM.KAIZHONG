using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.Resources.ShiftTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.CalendarSchemes
{
    /// <summary>
    /// 日历方案控制器
    /// </summary>
    public class CalendarSchemeController : DomainController
    {
        #region 日历方案       
        /// <summary>
        /// 停用日历方案
        /// </summary>
        /// <param name="calendarSchemes">待启用的日历方案列表</param>
        public virtual void DisableCalendarScheme(List<CalendarScheme> calendarSchemes)
        {
            var calendarSchemeList = new EntityList<CalendarScheme>();
            calendarSchemeList.AddRange(calendarSchemes);
            calendarSchemeList.ForEach(o => o.IsEnable = YesNo.No);
            RF.Save(calendarSchemeList);
        }

        /// <summary>
        /// 启用日历方案
        /// </summary>
        /// <param name="calendarSchemes">待启用的日历方案列表</param>
        public virtual void EnableCalendarScheme(List<CalendarScheme> calendarSchemes)
        {
            var calendarSchemeList = new EntityList<CalendarScheme>();
            calendarSchemeList.AddRange(calendarSchemes);
            calendarSchemeList.ForEach(o => o.IsEnable = YesNo.Yes);
            RF.Save(calendarSchemeList);
        }

        /// <summary>
        /// 获取缺省日历方案
        /// </summary>
        /// <returns>日历方案</returns>
        public virtual CalendarScheme GetDefaultCalendar()
        {
            return Query<CalendarScheme>().Where(p => p.IsDefault == YesNo.Yes).FirstOrDefault();
        }

        /// <summary>
        /// 发送日历方案变更通知
        /// </summary>
        /// <param name="modifiedCalendarSchemeIds">日历方案ID列表</param>
        public virtual void SendCalendarSchemeModifyMessage(List<double> modifiedCalendarSchemeIds)
        {
            if (modifiedCalendarSchemeIds.Any())
            {
                CalendarSchemeModifyEvent calendarSchemeModifyEvent = new CalendarSchemeModifyEvent()
                {
                    CalendarSchemeIds = modifiedCalendarSchemeIds
                };

                RT.EventBus.Publish<CalendarSchemeModifyEvent>(calendarSchemeModifyEvent);
            }
        }

        /// <summary>
        /// 获取可用的日历方案
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>可用日历方案列表</returns>
        public virtual EntityList<CalendarScheme> GetEnableCalendarSchemeList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<CalendarScheme>();
            query.Where(p => p.IsEnable == YesNo.Yes)
                 .WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 设置默认日历方案,并将之前默认方案设置为false
        /// </summary>
        /// <param name="calendarScheme">日历方案</param>
        public virtual void SetDefault(CalendarScheme calendarScheme)
        {
            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                DB.Update<CalendarScheme>().Set(p => p.IsDefault, YesNo.No).Where(p => p.IsDefault == YesNo.Yes).Execute();
                DB.Update<CalendarScheme>().Set(p => p.IsDefault, YesNo.Yes).Where(p => p.Id == calendarScheme.Id).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取缺省默认日历方案
        /// </summary>
        /// <returns>日历方案</returns>
        public virtual CalendarScheme GetDefaultCalendarScheme()
        {
            return Query<CalendarScheme>().Where(p => p.IsDefault == YesNo.Yes).FirstOrDefault();
        }

        /// <summary>
        /// 获取日历方案
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>日历方案</returns>
        public virtual CalendarScheme GetCalendarScheme(string name)
        {
            return Query<CalendarScheme>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 获取当前库存组织下的日历方案【周方案，班制】
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<CalendarScheme> GetCurrCalendarScheme(List<double> ids = null)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(CalendarScheme.SchemeWeeksProperty);
            elo.LoadWith(CalendarSchemeWeek.ShiftTypeProperty);
            var query = Query<CalendarScheme>();
            if (ids != null)
            {
                var exp = ids.CreateContainsExpression<CalendarScheme>("x", "Id");
                query.Where(exp);
            }

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 是否已经存在缺省的日历方案
        /// </summary>
        /// <param name="id">日历方案ID</param>
        /// <returns>返回是否存在缺省的日历方案</returns>
        public virtual bool ExistsDefault(double id)
        {
            return Query<CalendarScheme>().Where(p => p.IsDefault == YesNo.Yes && p.Id != id).Count() > 0;
        }

        #endregion

        #region 周方案
        /// <summary>
        /// 获取有效的周方案（生效时间最近的一笔）
        /// </summary>
        /// <param name="calendarId">日历方案ID</param>
        /// <returns>周方案</returns>
        public virtual CalendarSchemeWeek GetNewestCalSchWeek(double calendarId)
        {

            var lastDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59,DateTimeKind.Unspecified);//截至时间
            return Query<CalendarSchemeWeek>().Where(p => p.SchemeId == calendarId && p.ActiveDate <= lastDate).OrderByDescending(p => p.ActiveDate).FirstOrDefault();
            //Query<CalendarSchemeWeek>().Where(p => p.SchemeId == calendarId && p.ActiveDate <=DateTime.Today.AddDays(1)).OrderByDescending(p => p.ActiveDate).FirstOrDefault(); fixBy csp 2024/7/16 正星项目反馈
        }

        /// <summary>
        /// 发送资源日历例外变更通知
        /// </summary>
        /// <param name="newCalendarSchemeExcept">资源日历例外</param>
        public virtual void SendCalendarSchemeExceptionModifyMessage(
            CalendarSchemeExcept newCalendarSchemeExcept)
        {
            if (newCalendarSchemeExcept != null)
            {
                CalendarSchemeExceptionModifyEvent calendarSchemeExceptionModifyEvent
                    = new CalendarSchemeExceptionModifyEvent()
                    {
                        SchemeId = newCalendarSchemeExcept.SchemeId,
                        CalendarDay = newCalendarSchemeExcept.CalendarDay,
                        ShiftTypeId = newCalendarSchemeExcept.ShiftTypeId
                    };

                RT.EventBus.Publish<CalendarSchemeExceptionModifyEvent>(calendarSchemeExceptionModifyEvent);
            }
        }

        /// <summary>
        /// 按日历方案ID列表获取周方案列表
        /// </summary>
        /// <param name="schemeId">日历方案ID</param>
        /// <returns>周方案列表</returns>
        public virtual EntityList<CalendarSchemeWeek> GetCalendarSchemeWeeksBySchemeIds(double schemeId)
        {
            return Query<CalendarSchemeWeek>().Where(p => p.SchemeId == schemeId).ToList();
        }

        /// <summary>
        /// 获取周方案列表
        /// </summary>
        /// <param name="schemeId">日历方案列表</param>
        /// <returns>周方案列表</returns>
        public virtual EntityList<CalendarSchemeWeek> GetCalendarSchemeWeeks(double schemeId)
        {
            return Query<CalendarSchemeWeek>().Where(p => p.SchemeId == schemeId).ToList();
        }

        /// <summary>
        /// 根据班制ID获取周方案列表
        /// </summary>
        /// <param name="shiftTypeId">班制ID</param>
        /// <returns>周方案列表</returns>
        public virtual EntityList<CalendarSchemeWeek> GetCalendarSchemeWeeksByShiftTypeId(double shiftTypeId)
        {
            return Query<CalendarSchemeWeek>().Where(p => p.ShiftTypeId == shiftTypeId).ToList();
        }

        /// <summary>
        /// 判断是否休息日
        /// </summary>
        /// <param name="date">日期</param> 
        /// <param name="calSalWeek">周方案</param>
        [IgnoreProxy]
        public virtual bool IsHoliday(DateTime date, CalendarSchemeWeek calSalWeek)
        {
            bool isHoliday = false;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    isHoliday = !calSalWeek.Mon;
                    break;
                case DayOfWeek.Tuesday:
                    isHoliday = !calSalWeek.Tue;
                    break;
                case DayOfWeek.Wednesday:
                    isHoliday = !calSalWeek.Wed;
                    break;
                case DayOfWeek.Thursday:
                    isHoliday = !calSalWeek.Thu;
                    break;
                case DayOfWeek.Friday:
                    isHoliday = !calSalWeek.Fri;
                    break;
                case DayOfWeek.Saturday:
                    isHoliday = !calSalWeek.Sat;
                    break;
                case DayOfWeek.Sunday:
                    isHoliday = !calSalWeek.Sun;
                    break;
            }
            return isHoliday;
        }
        #endregion

        #region 日历方案例外
        /// <summary>
        /// 根据日历方案ID列表 获取日历方案例外列表
        /// </summary>
        /// <param name="schemeId">日历方案ID</param>
        /// <param name="beginDate">开始生效时间</param>
        /// <param name="endDate">结束生效时间</param>
        /// <returns>日历方案例外集合</returns>
        public virtual EntityList<CalendarSchemeExcept> GetCalendarSchemeExceptsBySchemeIds(double schemeId, DateTime beginDate, DateTime endDate)
        {
            return Query<CalendarSchemeExcept>().Where(p => p.SchemeId == schemeId && p.CalendarDay >= beginDate && p.CalendarDay <= endDate).ToList();
        }

        /// <summary>
        /// 获取日历方案例外
        /// </summary>
        /// <param name="calendarId">日历方案ID</param>
        /// <param name="date">日历日期（日期格式）</param>
        /// <returns>日历方案例外</returns>
        public virtual CalendarSchemeExcept GetCalendarSchemeExcept(double calendarId, DateTime date)
        {
            return Query<CalendarSchemeExcept>().Where(p => p.SchemeId == calendarId && p.CalendarDay == date).FirstOrDefault();
        }

        /// <summary>
        /// 获取日历方案例外班制Id
        /// </summary>
        /// <param name="calendarId">日历方案ID</param>
        /// <param name="date">日历日期（日期格式）</param>
        /// <returns>日历方案例外</returns>
        public virtual double? GetCalendarSchemeExceptShiftTypeId(double calendarId, DateTime date)
        {
            return Query<CalendarSchemeExcept>().Where(p => p.SchemeId == calendarId && p.CalendarDay == date).Select(t => t.ShiftTypeId).FirstOrDefault()?.ShiftTypeId;
        }

        #endregion

        /// <summary>
        /// 获取某一天的班制
        /// </summary>
        /// <param name="calendarScheme">日历方案</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="allowNullShiftType">允许空的班制</param>
        /// <returns>
        /// 班制
        /// </returns>
        public virtual Dictionary<DateTime, ShiftType> GetShiftTypesByCalendarSchemeAndDataRange(CalendarScheme calendarScheme,
            DateTime startDate, DateTime endDate, bool allowNullShiftType = false)
        {
            Dictionary<DateTime, ShiftType> dicShiftTypeOfDates = new Dictionary<DateTime, ShiftType>();
            var calendarSchemeCtl = RT.Service.Resolve<CalendarSchemeController>();
            //日历方案例外
            var calendarSchemeExcepts = calendarSchemeCtl.GetCalendarSchemeExceptsBySchemeIds(
                calendarScheme.Id, startDate, endDate);
            var dicCalendarExcept = calendarSchemeExcepts.ToDictionary(p => p.CalendarDay, p => p);
            //周方案
            var calendarSchemeWeeks = GetCalendarSchemeWeeksBySchemeIds(calendarScheme.Id);
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                //当天班制ID
                var shiftType = GetShiftTypeID(calendarSchemeWeeks, dicCalendarExcept, date, calendarScheme, allowNullShiftType);
                if (shiftType == null)
                    continue;
                dicShiftTypeOfDates.Add(date, shiftType);
            }

            return dicShiftTypeOfDates;
        }

        /// <summary>
        /// 获取班制ID
        /// </summary>
        /// <param name="calendarSchemeWeeks">周方案列表</param>
        /// <param name="dicCalendarSchemeExcept">日历方案例外</param>
        /// <param name="date">日期</param>
        /// <param name="calendarScheme">日历方案</param>
        /// <param name="allowNullShiftType">允许空的班制</param>
        /// <returns>班制</returns>
        private ShiftType GetShiftTypeID(EntityList<CalendarSchemeWeek> calendarSchemeWeeks, Dictionary<DateTime, CalendarSchemeExcept> dicCalendarSchemeExcept, DateTime date, CalendarScheme calendarScheme,
            bool allowNullShiftType = false)
        {
            ShiftType shiftType = null;
            //判断有无日历方案例外 
            if (dicCalendarSchemeExcept.ContainsKey(date))
            {
                shiftType = dicCalendarSchemeExcept[date].ShiftType;
            }
            else
            {
                //取生效时间小于当前日期最早的周方案
                var activeCalendarSchemeWeek = calendarSchemeWeeks.Where(x => x.ActiveDate <= date)
                    .OrderByDescending(x => x.ActiveDate)
                    .FirstOrDefault();

                if (activeCalendarSchemeWeek == null)
                {
                    if (!allowNullShiftType)
                    {
                        throw new ValidationException("日历方案【{0}】未找到符合规则的周方案配置，请检查周方案的预启用日期是否小于等于日期【{1}】".L10nFormat(calendarScheme.Name, date));
                    }
                    else
                    {
                        return null;
                    }
                }

                var isHoliday = RT.Service.Resolve<CalendarSchemeController>().IsHoliday(date, activeCalendarSchemeWeek);  //20200706 BUG#B0024155

                if (!isHoliday)
                {
                    shiftType = activeCalendarSchemeWeek.ShiftType;
                }
            }

            return shiftType;
        }
    }
}
