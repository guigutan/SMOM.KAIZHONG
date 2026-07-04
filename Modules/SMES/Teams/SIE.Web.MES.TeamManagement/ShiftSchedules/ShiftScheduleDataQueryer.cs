using SIE.Core.Common.Models;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.MES.TeamManagement.ShiftSchedules.Models;
using SIE.Web.Data;
using SIE.Web.Json;
using SIE.Web.MES.TeamManagement.ShiftSchedules.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班数据查询器
    /// </summary>  
    public class ShiftScheduleDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取排班表信息
        /// SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleQuery.js 调用
        /// </summary>
        /// <param name="criteria">班组排班表查询实体</param> 
        /// <returns>排班表信息列表</returns> 
        public List<ShiftScheduleInfo> GetShiftScheduleTables(ShiftScheduleTableCriteria criteria)
        {
            return RT.Service.Resolve<ShiftScheduleController>().GetShiftScheduleTables(criteria);
        }

        /// <summary>
        /// 获取排班信息，日历默认请求,解决切换月份数据重复
        /// SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleCommand.js调用
        /// </summary>
        /// <returns>排班信息Json列表</returns>
        public List<EntityJson> GetSchedules()
        {
            return new List<EntityJson>();
        }

        /// <summary>
        /// 获取排班信息
        /// SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleCommand.js调用
        /// </summary>
        /// <param name="criteria">排班查询实体</param>
        /// <param name="shiftConfig">班次颜色配置</param>
        /// <returns>排班信息Json列表</returns>  
        public List<EntityJson> GetShiftSchedules(ScheduleCriteria criteria, Dictionary<double, string> shiftConfig)
        {
            var stores = RT.Service.Resolve<ShiftScheduleController>().GetScheduleStores(criteria);
            return ScheduleHelper.GetStoreEntityJsons(stores, shiftConfig);
        }

        /// <summary>
        /// 获取排班班次信息
        /// SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleCommand.js调用
        /// </summary>
        /// <param name="criteria">排班查询实体</param>
        /// <returns>班次信息json列表</returns>
        public List<EntityJson> GetShifts(ScheduleCriteria criteria)
        {
            var shifts = RT.Service.Resolve<ShiftScheduleController>().GetScheduleShifts(criteria);
            return ScheduleHelper.GetShiftEntityJsons(shifts);
        }

        /// <summary>
        /// 保存班组排班信息
        /// SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleCommand.js 调用
        /// </summary>
        /// <param name="stores">排班信息集合</param> 
        public void SaveShiftSchedules(List<ScheduleStore> stores)
        {
            RT.Service.Resolve<ShiftScheduleController>().SaveShiftSchedules(stores);
        }

        /// <summary>
        /// 导出排班表
        /// SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleExportCommand.js调用
        /// </summary>
        /// <param name="criteria">班组排班表查询实体</param>
        /// <returns>导出数据</returns>
        public ExportDataTable ExportShiftSchedule(ShiftScheduleTableCriteria criteria)
        {
            return RT.Service.Resolve<ShiftScheduleController>().ExportShiftSchedule(criteria);
        }

        /// <summary>
        /// 切换排班班组
        /// </summary>
        /// <param name="store">排班信息</param>
        public void ChangeWorkGroup(ScheduleStore store)
        {
            RT.Service.Resolve<ShiftScheduleController>().UpdateShiftSchedule(store);
        }

        /// <summary>
        /// 获取预处理数据列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <param name="week">周</param>
        /// <param name="workGroupId">班组</param>
        /// <param name="ShiftId">班次</param>
        /// <param name="shiftConfig">班次配置</param>
        /// <returns>预处理数据列表</returns>
        public List<HeforehandScheduleInfo> GetHeforehandScheduleInfos(ScheduleCriteria criteria, int week, double workGroupId, double ShiftId, Dictionary<double, string> shiftConfig)
        {
            if (criteria == null)
            {
                return new List<HeforehandScheduleInfo>();
            }
            var scheduleData = new ScheduleData
            {
                WorkShopId = criteria.WorkShopId.Value,
                WipResourceId = criteria.WipResourceId.Value,
                BeginDate = criteria.ScheduleDate.BeginValue.Value,
                EndDate = criteria.ScheduleDate.EndValue.Value,
                WorkGroupId = workGroupId,
                ShiftId = ShiftId,
                Week = week
            };
            shiftConfig.ForEach(p =>
            {
                scheduleData.ShiftConfig.Add(p.Key, p.Value);
            });

            var heforehandScheduleInfos = new List<HeforehandScheduleInfo>();
            var stores = RT.Service.Resolve<ShiftScheduleController>().HeforehandSchedule(scheduleData);
            stores.ForEach(store =>
            {
                //默认色
                shiftConfig.TryGetValue(store.ShiftId, out string color);
                var heforehandScheduleInfo = new HeforehandScheduleInfo()
                {
                    id = store.Id,
                    startDate = store.StartDate.ToString("yyyy-MM-dd") + "T00:00:00.000Z",
                    endDate = store.StartDate.AddDays(1).ToString("yyyy-MM-dd") + "T00:00:00.000Z",
                    title = store.Title,
                    allDay = store.AllDay,
                    color = color.IsNullOrEmpty() ? "#74CF7E" : color,
                    calendarId = 1,
                    ShiftId = store.ShiftId,
                    WorkGroupId = store.WorkGroupId,
                    WorkGroupName = store.WorkGroupName,
                    WipResourceId = store.WipResourceId,
                    WorkShopId = store.WorkShopId,
                    ShiftTypeId = store.ShiftTypeId,
                    IsNew = store.IsNew,
                    Leisure = store.Leisure,
                    Count = store.ShiftCount,

                };
                heforehandScheduleInfos.Add(heforehandScheduleInfo);
            });

            return heforehandScheduleInfos;
        }
    }

    /// <summary>
    /// 预处理数据信息
    /// </summary>
    public class HeforehandScheduleInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool allDay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int calendarId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double WorkGroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkGroupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double WipResourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ShiftTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Leisure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }
    }
}