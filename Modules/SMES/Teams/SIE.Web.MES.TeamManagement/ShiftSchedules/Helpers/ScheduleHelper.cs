using SIE.MES.TeamManagement.ShiftSchedules.Models;
using SIE.Web.Json;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules.Helpers
{
    /// <summary>
    /// 排班帮助类
    /// </summary>
    public static class ScheduleHelper
    {
        /// <summary>
        /// 排班信息转EntityJson
        /// </summary>
        /// <param name="stores">排班信息列表</param>
        /// <param name="shiftConfig">班次颜色配置</param>
        /// <returns>Json列表</returns>
        public static List<EntityJson> GetStoreEntityJsons(List<ScheduleStore> stores, Dictionary<double, string> shiftConfig)
        {
            if (stores == null)
            {
                return new List<EntityJson>();
            }
            List<EntityJson> res = new List<EntityJson>();
            stores.ForEach(store =>
            {
                shiftConfig.TryGetValue(store.ShiftId, out string color);
                EntityJson node = new EntityJson();
                node.SetProperty("id", store.Id);
                node.SetProperty("startDate", store.StartDate.ToString("yyyy-MM-dd") + "T00:00:00.000Z");
                node.SetProperty("endDate", store.StartDate.AddDays(1).ToString("yyyy-MM-dd") + "T00:00:00.000Z");
                node.SetProperty("title", store.Title);
                node.SetProperty("allDay", store.AllDay);
                node.SetProperty("color", color.IsNullOrEmpty()? "#74CF7E" : color);
                node.SetProperty("calendarId", 1);
                node.SetProperty("ShiftId", store.ShiftId);
                node.SetProperty("WorkGroupId", store.WorkGroupId);
                node.SetProperty("WorkGroupName", store.WorkGroupName);
                node.SetProperty("WipResourceId", store.WipResourceId);
                node.SetProperty("WorkShopId", store.WorkShopId);
                node.SetProperty("ShiftTypeId", store.ShiftTypeId);
                node.SetProperty("IsNew", store.IsNew);
                node.SetProperty("Leisure", store.Leisure);
                node.SetProperty("Count", store.ShiftCount);
                res.Add(node);
            });
            return res;
        }

        /// <summary>
        /// 获取排班班次信息
        /// </summary>
        /// <param name="schduleShifts">班次列表</param>
        /// <returns>班次信息json列表</returns>
        public static List<EntityJson> GetShiftEntityJsons(List<SchduleShift> schduleShifts)
        {
            List<EntityJson> res = new List<EntityJson>();
            string[] background = new string[] { "#ED865B", "#388476", "#B26C99", "#776BA9", "#FFBE71", "#566A91", "#DF578E", "#5DA470", "#8470D3", "#A571BD" };
            for (int i = 0; i < schduleShifts.Count; i++)
            {
                var schduleShift = schduleShifts[i];
                var shift = schduleShift.Shift;
                EntityJson node = new EntityJson();
                node.SetProperty("id", shift.Id);
                node.SetProperty("shiftName", shift.Name);
                node.SetProperty("shiftTime", "{0}~{1}".FormatArgs(shift.BeginTime.ToString("t"), shift.EndTime.ToString("t")));
                node.SetProperty("background", background[i]);
                node.SetProperty("isExpire", schduleShift.IsExpire);
                res.Add(node);
            }

            return res;
        }
    }
}