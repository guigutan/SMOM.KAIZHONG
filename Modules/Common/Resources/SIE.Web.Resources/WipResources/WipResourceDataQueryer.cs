using SIE.Core;
using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.WipResources;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Resources.WipResources
{
    /// <summary>
    /// 生产资源数据查询
    /// </summary>
    public class WipResourceDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取日历信息
        /// SIE.Web.Resource.WipResource.WipResourceLayout 调用
        /// </summary>
        /// <returns>班制日历Json列表</returns>
        public List<EntityJson> GetShiftCalendar()
        {
            return new List<EntityJson>();
        }

        /// <summary>
        /// 获取日历信息
        /// SIE.Web.Resource.WipResource.WipResourceLayout 调用
        /// </summary>
        /// <param name="resourceId">生产资源id</param>
        /// <param name="calenderDate">当前显示日历日期，通过该日期计算当前显示的日期范围</param>
        /// <returns>日历信息</returns>
        public ShiftCalenderInfo GetShiftCalendarInfo(double resourceId, DateTime calenderDate)
        {
            var monthFirstDate = new DateTime(calenderDate.Year, calenderDate.Month, 1);
            DateTime beginDate;
            DateTime endDate;
            if (monthFirstDate.DayOfWeek == DayOfWeek.Sunday)
                beginDate = monthFirstDate;
            else
                beginDate = monthFirstDate.AddDays(-(int)monthFirstDate.DayOfWeek);
            endDate = beginDate.AddDays(41);
            ShiftCalenderInfo info = new ShiftCalenderInfo();
            var wipResource = RF.GetById<WipResource>(resourceId);
            var weeks = RT.Service.Resolve<CalendarSchemeController>().GetCalendarSchemeWeeks(wipResource.SchemeId);
            weeks.ForEach(week =>
            {
                var weekInfo = new EntityJson();
                weekInfo.SetProperty("Name", week.Name);
                weekInfo.SetProperty("ShiftType", week.ShiftType?.Name);
                weekInfo.SetProperty("ActiveDate", week.ActiveDate.ToString(DateTimeFormat.YYYMMdd2));
                info.WeekList.Add(weekInfo);
            });
            var reslut = RT.Service.Resolve<WipResourceController>().GetShiftTypeInfo(wipResource, beginDate, endDate);
            info.ShiftList.AddRange(GetStoreEntityJsons(reslut));
            return info;
        }

        /// <summary>
        /// 工作日信息转EntityJson
        /// </summary>
        /// <param name="dicShiftType">班次颜色配置</param>
        /// <returns>Json列表</returns>
        List<EntityJson> GetStoreEntityJsons(Dictionary<DateTime, ShiftTypeInfo> dicShiftType)
        {
            List<EntityJson> res = new List<EntityJson>();
            string[] colors = new string[] { "#EBECEF", "#7DD1F3" };
            int id = 1;
            DateTime today = DateTime.Today;
            dicShiftType.ForEach(item =>
            {
                var date = item.Key;
                var value = item.Value;
                var content = value.Content;
                var color = "#FFFFFF";
                if (string.IsNullOrEmpty(content))
                {
                    content = " ";
                }

                if (date < today && content != " " || value.IsHoliday)
                {
                    color = colors[0];
                }
                else if (date >= today && content != " ")
                {
                    color = colors[1];
                }

                EntityJson node = new EntityJson();
                node.SetProperty("id", id++);
                node.SetProperty("startDate", date.ToString(DateTimeFormat.YYYMMdd2)); //// + "T00:00:00.000Z");
                node.SetProperty("endDate", date.AddDays(1).ToString(DateTimeFormat.YYYMMdd2));//// + "T00:00:00.000Z");
                node.SetProperty("title", content);
                node.SetProperty("allDay", true);
                node.SetProperty("color", color);
                node.SetProperty("calendarId", 1);
                node.SetProperty("IsHoliday", value.IsHoliday);
                node.SetProperty("IsActived", value.IsActived);
                node.SetProperty("Count", 1);
                res.Add(node);
            });

            return res;
        }

        /// <summary>
        /// 获取资源同步设置
        /// SIE.Web.Resources.WipResources.Commands.SynWipResSettingCommand 调用
        /// </summary>
        /// <returns>资源同步设置</returns>
        public EntityList<SynWipResSetting> GetSynWipResSettings()
        {
            return RT.Service.Resolve<WipResourceController>().GetSynWipResSettings();
        }
    }

    /// <summary>
    /// 班制日历信息
    /// </summary>
    public class ShiftCalenderInfo
    {
        /// <summary>
        /// 周日历集合
        /// </summary>
        public List<EntityJson> WeekList { get; } = new List<EntityJson>();

        /// <summary>
        /// 班制集合
        /// </summary>
        public List<EntityJson> ShiftList { get; } = new List<EntityJson>();
    }
}