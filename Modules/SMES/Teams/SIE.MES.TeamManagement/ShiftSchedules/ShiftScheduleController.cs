using SIE.Common.DataSync;
using SIE.Core.Common.Models;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ShiftSchedules.Models;
using SIE.ObjectModel;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 班组排班控制器
    /// </summary>
    public partial class ShiftScheduleController : DomainController
    {
        /// <summary>
        /// 获取班组排班表
        /// </summary>
        /// <param name="criteria">班组排班表查询实体</param>
        /// <returns>班组排班列表</returns>
        public virtual EntityList<ShiftSchedule> GetShiftSchedules(ShiftScheduleTableCriteria criteria)
        {
            var query = Query<ShiftSchedule>();
            if (criteria.WorkGroupId.HasValue)
                query.Where(p => p.WorkGroupId == criteria.WorkGroupId.Value);
            if (criteria.WorkShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.WorkShopId.Value);
            if (criteria.WipResourceId.HasValue)
                query.Where(p => p.WipResourceId == criteria.WipResourceId.Value);
            if (criteria.ScheduleDate.BeginValue.HasValue)
                query.Where(p => p.ScheduleDate >= criteria.ScheduleDate.BeginValue.Value);
            if (criteria.ScheduleDate.EndValue.HasValue)
                query.Where(p => p.ScheduleDate <= criteria.ScheduleDate.EndValue.Value);
            var pagingInfo = criteria.PagingInfo;
            pagingInfo.PageSize = int.MaxValue - 1;
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取班组排班信息
        /// </summary>
        /// <param name="criteria">排班查询实体</param>
        /// <returns>班组排班列表</returns>
        public virtual EntityList<ShiftSchedule> GetShiftSchedules(ScheduleCriteria criteria)
        {
            var query = Query<ShiftSchedule>();
            if (criteria.WorkShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.WorkShopId.Value);
            if (criteria.WipResourceId.HasValue)
                query.Where(p => p.WipResourceId == criteria.WipResourceId.Value);
            if (criteria.ScheduleDate.BeginValue.HasValue)
                query.Where(p => p.ScheduleDate >= criteria.ScheduleDate.BeginValue.Value);
            if (criteria.ScheduleDate.EndValue.HasValue)
                query.Where(p => p.ScheduleDate <= criteria.ScheduleDate.EndValue.Value);
            var pagingInfo = criteria.PagingInfo;
            pagingInfo.PageSize = int.MaxValue - 1;
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据日期获取班组班次关系（排除某些班组）
        /// </summary>
        /// <param name="scheduleDate">日期</param>
        /// <param name="workGroupIds">排除的班组</param>
        /// <returns>班组班次关系</returns>
        public virtual EntityList<ShiftSchedule> GetShiftSchedules(DateTime scheduleDate, List<double> workGroupIds)
        {
            return Query<ShiftSchedule>().Where(p => p.ScheduleDate == scheduleDate && !workGroupIds.Contains(p.WorkGroupId)).ToList();
        }

        /// <summary>
        /// 根据日期范围获取排班
        /// </summary>
        /// <param name="dr">日期范围</param>
        /// <returns>排班集合</returns>
        public virtual EntityList<ShiftSchedule> GetShiftSchedules(DateRange dr)
        {
            return Query<ShiftSchedule>().Where(p => p.ScheduleDate >= dr.BeginValue.Value && p.ScheduleDate <= dr.EndValue.Value).ToList();
        }

        /// <summary>
        /// 根据资源车间和日期获取排班列表
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="dt">日期</param>
        /// <returns>排班列表</returns>
        public virtual EntityList<ShiftSchedule> GetShiftSchedules(double resourceId, double workShopId, DateTime dt)
        {
            var itemList = Query<ShiftSchedule>()
                          .Where(p => p.WipResourceId == resourceId
                          && p.WorkShopId == workShopId
                          && p.ScheduleDate == dt)
                          .ToList();
            return itemList;
        }

        /// <summary>
        /// 根据班组和日期获取班组班次关系
        /// </summary>
        /// <param name="scheduleDate">日期</param>
        /// <param name="workGroupId">班组</param>
        /// <returns>班组班次关系</returns>
        public virtual ShiftSchedule GetShiftSchedule(DateTime scheduleDate, double workGroupId)
        {
            return Query<ShiftSchedule>().Where(p => p.ScheduleDate == scheduleDate && p.WorkGroupId == workGroupId).FirstOrDefault();
        }

        /// <summary>
        /// 获取产线班次排班信息
        /// </summary>
        /// <param name="wipResourceId">资源ID</param>
        /// <param name="shiftId">班次ID</param>
        /// <param name="scheduleDate">排班日期，不带时间</param>
        /// <returns>班组排班信息</returns>
        public virtual ShiftSchedule GetShiftSchedule(double wipResourceId, double shiftId, DateTime scheduleDate)
        {
            return Query<ShiftSchedule>().Where(p => p.WipResourceId == wipResourceId && p.ScheduleDate == scheduleDate && p.ShiftId == shiftId).FirstOrDefault();
        }


        /// <summary>
        /// 获取排班表信息
        /// </summary>
        /// <param name="criteria">班组排班表查询实体</param>
        /// <returns>排班表信息列表</returns>
        public virtual List<ShiftScheduleInfo> GetShiftScheduleTables(ShiftScheduleTableCriteria criteria)
        {
            if (!criteria.ScheduleDate.BeginValue.HasValue || !criteria.ScheduleDate.EndValue.HasValue)
                throw new ValidationException("开始日期不能为空".L10N());
            DateTime beginDate = criteria.ScheduleDate.BeginValue.Value.Date;
            DateTime endDate = criteria.ScheduleDate.EndValue.Value.Date;
            var scheduleList = RT.Service.Resolve<ShiftScheduleController>().GetShiftSchedules(criteria);
            List<ShiftScheduleInfo> schedules = new List<ShiftScheduleInfo>();
            Dictionary<double, string> dicBackground = new Dictionary<double, string>();
            string[] backgrounds = new string[] { "#ED865B", "#388476", "#B26C99", "#776BA9", "#FFBE71", "#566A91", "#DF578E", "#5DA470", "#8470D3", "#A571BD" };
            var groupWorkGroups = scheduleList.GroupBy(p => p.WorkGroupId);
            foreach (var groupWorkGroup in groupWorkGroups)
            {
                //按照班组分组
                var schedule1List = groupWorkGroup.ToList();
                var groupWipResources = schedule1List.GroupBy(p => p.WipResourceId);
                foreach (var groupWipResource in groupWipResources)
                {
                    //按照资源分组
                    var dicSchedule = groupWipResource.Distinct((x, y) => x.ScheduleDate == y.ScheduleDate).ToDictionary(p => p.ScheduleDate, p => p);
                    bool isNewResource = false;
                    var schedule = new ShiftScheduleInfo();
                    for (DateTime i = beginDate.AddDays(-1); i <= endDate;)
                    {
                        i = i.AddDays(1);
                        ShiftSchedule shiftSchedule = null;
                        dicSchedule.TryGetValue(i, out shiftSchedule);
                        if (shiftSchedule == null)
                        {
                            continue;
                        }
                        if (!isNewResource)
                        {
                            schedule.WorkGroup = shiftSchedule.WorkGroup?.Name;
                            schedule.WorkShop = shiftSchedule.WorkShop?.Name;
                            schedule.WipResource = shiftSchedule.WipResource?.Name;
                        }

                        var shift = shiftSchedule.Shift;
                        schedule.DetailList.Add(new ShiftScheduleDetailInfo()
                        {
                            ScheduleDate = i,
                            StrDate = i.ToString("yyyy-MM-dd"),
                            Shift = shift?.Name,
                            ShiftId = shift == null ? 0 : shift.Id,
                            ShiftTime = "{0}~{1}".FormatArgs(shift?.BeginTime.ToString("t"), shift?.EndTime.ToString("t")),
                            Background = GetBackground(dicBackground, backgrounds, shift == null ? 0 : shift.Id)
                        });
                    }

                    schedules.Add(schedule);
                }
            }

            return schedules;
        }

        /// <summary>
        /// 导出排班表
        /// </summary>
        /// <param name="criteria">班组排班表查询实体</param>
        /// <returns>导出数据</returns>
        public virtual ExportDataTable ExportShiftSchedule(ShiftScheduleTableCriteria criteria)
        {
            var exportDates = RT.Service.Resolve<ShiftScheduleController>().GetShiftScheduleTables(criteria);
            var beginDate = criteria.ScheduleDate.BeginValue.Value.Date;
            var endDate = criteria.ScheduleDate.EndValue.Value.Date;
            ExportDataTable exportDataTable = new ExportDataTable();
            Dictionary<int, List<DateTime>> dicDataTimes = new Dictionary<int, List<DateTime>>();
            for (DateTime j = beginDate; j <= endDate;)
            {
                var key = int.Parse(j.ToString("yyyyMM"));
                if (dicDataTimes.ContainsKey(key))
                    dicDataTimes[key].Add(j);
                else
                    dicDataTimes[key] = new List<DateTime>() { j };
                j = j.AddDays(1);
            }
            dicDataTimes.OrderBy(p => p.Key).ForEach(dicDataTime =>
            {
                DataTable dataTable = new DataTable();
                List<DateTime> dateTimes = dicDataTime.Value;
                DateTime monthStartData = dateTimes.Min();
                DateTime monthEndData = dateTimes.Max();
                int startDay = monthStartData.Day;
                var dataColumnCount = monthEndData.Day - monthStartData.Day + 1; //数据列数  
                string[] columns = new string[dataColumnCount + 3];
                columns[0] = "班组";
                columns[1] = "资源";
                columns[2] = "年-月";
                dataTable.Columns.Add(columns[0]);
                dataTable.Columns.Add(columns[1]);
                dataTable.Columns.Add(columns[2]);
                for (int i = 0; i < dataColumnCount; i++)
                {
                    columns[i + 3] = $"{startDay++}号";
                    dataTable.Columns.Add(columns[i + 3]);
                }
                //行数据
                exportDates.ForEach(exportDate =>
                {
                    var dicShift = exportDate.DetailList.Distinct((x, y) => x.ScheduleDate == y.ScheduleDate).ToDictionary(p => p.ScheduleDate, p => p.Shift);
                    var row = dataTable.NewRow();
                    row[0] = exportDate.WorkGroup;
                    row[1] = exportDate.WipResource;
                    row[2] = $"{monthStartData.Year}-{monthStartData.Month}";
                    int columnIndex = 3;
                    for (DateTime i = monthStartData; i <= monthEndData;)
                    {
                        string shift = string.Empty;
                        dicShift.TryGetValue(i, out shift);
                        row[columnIndex++] = shift;
                        i = i.AddDays(1);
                    }
                    dataTable.Rows.Add(row);
                });
                exportDataTable.SheetNames.Add(monthStartData.ToString("Y"));
                exportDataTable.Tables.Add(dataTable);
                exportDataTable.Columns.Add(columns);
            });
            return exportDataTable;
        }

        /// <summary>
        /// 获取背景颜色
        /// </summary>
        /// <param name="dicBackground">班次背景颜色字典</param>
        /// <param name="backgrounds">颜色数组</param>
        /// <param name="shiftId">班次ID</param>
        /// <returns>背景颜色</returns>
        string GetBackground(Dictionary<double, string> dicBackground, string[] backgrounds, double shiftId)
        {
            string background = string.Empty;
            if (!dicBackground.TryGetValue(shiftId, out background))
            {
                background = backgrounds[dicBackground.Count];
                dicBackground.Add(shiftId, background);
            }

            return background;
        }

        /// <summary>
        /// 获取排班信息列表
        /// </summary>
        /// <param name="criteria">班组排班查询实体</param>
        /// <param name="syncId">最大同步ID，区分预排班记录</param>
        /// <returns>排班信息列表</returns>
        public virtual List<ScheduleStore> GetScheduleStores(ScheduleCriteria criteria, long? syncId = null)
        {
            ValidateScheduleCriteria(criteria);
            List<ScheduleStore> stores = new List<ScheduleStore>();
            var schedules = GetShiftSchedules(criteria);
            var beginDate = criteria.ScheduleDate.BeginValue.Value.Date;
            var endDate = criteria.ScheduleDate.EndValue.Value.Date;
            ////获取日期班制信息
            var dicSchedule = schedules.GroupBy(p => p.ScheduleDate).ToDictionary(p => p.Key, p => p.ToList());
            var dicShiftType = RT.Service.Resolve<CalendarSchemeController>().GetShiftTypesByCalendarSchemeAndDataRange(criteria.WipResource.Scheme, beginDate, endDate);
            int id = 0;
            double wipResourceId = criteria.WipResourceId.Value;
            double workShopId = criteria.WipResource.WorkShopId.Value;
            for (DateTime i = beginDate.AddDays(-1); i <= endDate;)
            {
                i = i.AddDays(1);
                ////获取资源当日的班制，不存在则生成事件，存在则根据班次数量生成对应的事件，
                ////未排班的班次Title=null,Leisure=true 做占位处理
                ShiftType shiftType = null;
                if (!dicShiftType.TryGetValue(i, out shiftType))
                    continue;
                List<ShiftSchedule> res = null;
                dicSchedule.TryGetValue(i, out res);
                var count = shiftType.ShiftList.Count;
                var shiftList = shiftType.ShiftList.OrderBy(s => s.BeginTime.TimeOfDay.TotalMilliseconds);
                shiftList.ForEach(shift =>
                {
                    double shiftTypeId = shift.ShiftTypeId;
                    ShiftSchedule schedule = res?.FirstOrDefault(p => p.ShiftTypeId == shiftTypeId && p.ShiftId == shift.Id);
                    var store = new ScheduleStore()
                    {
                        AllDay = true,
                        CalendarId = 0,
                        Color = "#FB0202",
                        StartDate = i,
                        EndDate = i.AddDays(1).AddSeconds(-1),
                        ShiftId = shift.Id,
                        Id = schedule != null ? (int)schedule.Id : --id,
                        Title = schedule != null ? schedule.WorkGroup?.Name : " ",
                        WorkGroupId = schedule != null ? schedule.WorkGroupId : 0,
                        WorkGroupName = schedule != null ? schedule.WorkGroup?.Name : string.Empty,
                        WipResourceId = wipResourceId,
                        WorkShopId = workShopId,
                        ShiftTypeId = shiftTypeId,
                        Leisure = schedule == null,
                        IsNew = schedule != null && syncId != null && schedule.GetProperty(DataSyncExtension.SYNC_IDProperty) > syncId,
                        ShiftCount = count,
                    };
                    stores.Add(store);
                });
            }

            return stores;
        }

        /// <summary>
        /// 验证排班查询条件
        /// </summary>
        /// <param name="criteria">排班查询条件</param>
        private void ValidateScheduleCriteria(ScheduleCriteria criteria)
        {
            if (!criteria.ScheduleDate.BeginValue.HasValue || !criteria.ScheduleDate.EndValue.HasValue)
            {
                throw new ValidationException("排班日期不能为空".L10N());
            }
            if (!criteria.WipResourceId.HasValue || criteria.WipResourceId == 0)
            {
                throw new ValidationException("资源不能为空".L10N());
            }
            var workShopId = criteria.WipResource.WorkShopId;
            if (!workShopId.HasValue || workShopId == 0)
            {
                throw new ValidationException("车间不能为空".L10N());
            }
        }

        /// <summary>
        /// 获取排班班次
        /// </summary>
        /// <param name="criteria">班组排班查询实体</param>
        /// <returns>班次集合</returns>
        public virtual List<SchduleShift> GetScheduleShifts(ScheduleCriteria criteria)
        {
            var beginDate = criteria.ScheduleDate.BeginValue.Value.Date;
            var endDate = criteria.ScheduleDate.EndValue.Value.Date;
            ////获取日期班制信息，当前日期及之前的所有班次标志为过期，排班时过滤掉过期班次，因为过期日期是无法排班，显示没意义
            var dicShiftType = RT.Service.Resolve<CalendarSchemeController>().GetShiftTypesByCalendarSchemeAndDataRange(criteria.WipResource.Scheme, beginDate, endDate);
            var dbDate = RF.Find<Shift>().GetDbTime().Date;
            ////过期班次
            var expireShift = dicShiftType.Where(p => p.Key <= dbDate).Select(p => p.Value).SelectMany(p => p.ShiftList).Distinct((x, y) => x.Id == y.Id);
            ////可排班班次
            var scheduleShift = dicShiftType.Where(p => p.Key > dbDate).Select(p => p.Value).SelectMany(p => p.ShiftList).Distinct((x, y) => x.Id == y.Id);
            List<SchduleShift> schduleShifts = new List<SchduleShift>();
            scheduleShift.ForEach(e =>
            {
                schduleShifts.Add(new SchduleShift() { Shift = e, ShiftId = e.Id });
            });
            ////排除掉过期班次与可排班班次的交集，及为不可排班班次
            expireShift.ForEach(e =>
            {
                if (!schduleShifts.Any(p => p.ShiftId == e.Id))
                {
                    schduleShifts.Add(new SchduleShift() { Shift = e, IsExpire = true, ShiftId = e.Id });
                }
            });
            return schduleShifts;
        }

        /// <summary>
        /// 保存排班信息
        /// 如果班次id为0，表示取消排班
        /// 1、如果班次或者班制id为0的情况，则取消班组今天在当前产线上的排班
        /// 2、班组今天在当前产线上没有排班信息，则新增今天班次排班信息
        /// 3、班组今天在当前产线上有排班信息，如果班次跟新排班班次一致的话，不做处理；不一致则替换班次
        /// </summary>
        /// <param name="schedule">班组排班</param>
        internal void SaveShiftSchedule(ShiftSchedule schedule)
        {
            if (schedule.ShiftId == 0 || schedule.ShiftTypeId == 0)
            {
                CancelShiftSchedule(schedule, false);  ////取消排班
            }
            else
            {
                //首先找下这个班组是否在别的资源上有排班，有则要删除
                CancelShiftSchedule(schedule, true);  ////取消排班 

                //某一天的当排班班次的排班情况
                var itemList = GetShiftSchedules(schedule.WipResourceId, schedule.WorkShopId, schedule.ScheduleDate);
                var item = itemList.FirstOrDefault(p => p.ShiftId == schedule.ShiftId);

                //该资源下是否有班组已经使用了这个班次
                if (item == null)
                {
                    //否，则看当前保存的班组是否已经排班，如果排班了要替换，否则增加
                    var hasShift = itemList.FirstOrDefault(p => p.WorkGroupId == schedule.WorkGroupId);
                    if (hasShift != null)
                    {
                        hasShift.ShiftId = schedule.ShiftId;
                        RF.Save(hasShift);
                    }
                    else if (schedule.WorkGroupId == 0)
                    {
                        schedule.PersistenceStatus = PersistenceStatus.Deleted;
                    }
                    else
                    {
                        schedule.PersistenceStatus = PersistenceStatus.New;
                        schedule.GenerateId();
                        RF.Save(schedule);
                    }
                }
                else if (item.WorkGroupId != schedule.WorkGroupId)
                {
                    //班次被使用，而且不是导入的班组使用
                    //如果该导入的班组已经有排班，需要先清除
                    var hasShift = itemList.FirstOrDefault(p => p.WorkGroupId == schedule.WorkGroupId);
                    if (hasShift != null)
                    {
                        CancelShiftSchedule(schedule, false);
                    }

                    //将占着班次的班组替换（等于是旧的班组删除，新的班组加入）
                    item.WorkGroupId = schedule.WorkGroupId;
                    if (schedule.WorkGroupId != 0)
                    {
                        RF.Save(item);
                    }
                }
                else
                {
                    //
                }
            }
        }

        /// <summary>
        /// 取消排班
        /// </summary>
        /// <param name="schedule">排班信息</param>
        /// <param name="notInResource">取消不在新记录资源上的</param>
        private void CancelShiftSchedule(ShiftSchedule schedule, bool notInResource)
        {
            var query = Query<ShiftSchedule>().Where(p => p.WorkGroupId == schedule.WorkGroupId
                                                            && p.ScheduleDate == schedule.ScheduleDate);
            if (notInResource)
            {
                query.Where(p => p.WipResourceId != schedule.WipResourceId);
            }
            else
            {
                query.Where(p => p.WipResourceId == schedule.WipResourceId);
            }

            var res = query.FirstOrDefault();
            if (res == null)
            {
                return;
            }
            res.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(res);
        }

        /// <summary>
        /// 班组预排班（不保存数据库）
        /// </summary>
        /// <param name="data">排班信息</param>
        /// <returns>排班信息列表</returns>
        public virtual List<ScheduleStore> HeforehandSchedule(ScheduleData data)
        {
            using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
            {
                WipResource wipResource = ValidateScehduleData(data);
                var dicShiftType = RT.Service.Resolve<CalendarSchemeController>().GetShiftTypesByCalendarSchemeAndDataRange(wipResource.Scheme, data.BeginDate, data.EndDate);
                var maxSyncId = GetMaxShiftScheduleSyncId();
                ////只能排今天之后的班
                DateTime beginDate = DateTime.Today;
                if (data.BeginDate.Date > beginDate)
                    beginDate = data.BeginDate.Date.AddDays(-1);
                for (DateTime i = beginDate; i <= data.EndDate;)
                {
                    i = i.AddDays(1);
                    Week week = (Week)data.Week;
                    var dayofweek = i.DayOfWeek.ToString();
                    var res = (Week)(EnumViewModel.LabelToEnum(dayofweek, typeof(Week)));
                    if ((week & res) != 0)
                    {
                        ShiftType shiftType;
                        dicShiftType.TryGetValue(i, out shiftType); //没有班制不排班
                        if (shiftType == null)
                            continue;
                        var shift = shiftType.ShiftList.FirstOrDefault(p => p.Id == data.ShiftId);  //班次不匹配，不排班
                        if (shift == null)
                            continue;
                        var schedule = new ShiftSchedule()
                        {
                            ScheduleDate = i,
                            WorkGroupId = data.WorkGroupId,
                            ShiftId = shift.Id,
                            ShiftTypeId = shiftType.Id,
                            WipResourceId = data.WipResourceId,
                            WorkShopId = data.WorkShopId,
                        };
                        SaveShiftSchedule(schedule);
                    }
                }

                var criteria = new ScheduleCriteria()
                {
                    WorkShopId = data.WorkShopId,
                    WipResourceId = data.WipResourceId,
                    ScheduleDate = new DateRange()
                    {
                        EndValue = data.EndDate,
                        BeginValue = data.BeginDate,
                    },
                };
                ////预排班不保存不提交事务
                var stores = GetScheduleStores(criteria, maxSyncId);
                return stores;
            }
        }

        /// <summary>
        /// 验证排班信息
        /// </summary>
        /// <param name="data">排班信息</param>
        /// <returns>资源</returns>
        private WipResource ValidateScehduleData(ScheduleData data)
        {
            var wipResource = RF.GetById<WipResource>(data.WipResourceId);
            if (wipResource == null)
                throw new EntityNotFoundException(typeof(WipResource), data.WipResourceId);
            if (wipResource.WorkShopId != data.WorkShopId)
                throw new ValidationException("车间与资源不匹配，请核对".L10N());
            var workGroup = RF.GetById<WorkGroup>(data.WorkGroupId);
            if (workGroup == null)
                throw new EntityNotFoundException(typeof(WorkGroup), data.WorkGroupId);
            return wipResource;
        }

        /// <summary>
        /// 保存排班
        /// </summary>
        /// <param name="stores">排班信息集合</param>
        public virtual void SaveShiftSchedules(List<ScheduleStore> stores)
        {
            using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
            {
                stores.ForEach(store =>
                {
                    SaveShiftSchedule(store);
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存排班
        /// </summary>
        /// <param name="store">排班信息</param>
        private void SaveShiftSchedule(ScheduleStore store)
        {
            var schedule = new ShiftSchedule()
            {
                ScheduleDate = store.StartDate.Date,
                WorkGroupId = store.WorkGroupId,
                ShiftId = store.ShiftId,
                ShiftTypeId = store.ShiftTypeId,
                WipResourceId = store.WipResourceId,
                WorkShopId = store.WorkShopId,
            };
            SaveShiftSchedule(schedule);
        }

        /// <summary>
        /// 获取最大同步ID
        /// </summary>
        /// <returns>同步ID</returns>
        public virtual long GetMaxShiftScheduleSyncId()
        {
            var shiftSchedule = Query<ShiftSchedule>().OrderByDescending(p => p.GetProperty(DataSyncExtension.SYNC_IDProperty)).FirstOrDefault();
            if (shiftSchedule == null)
                return 0;
            else
                return shiftSchedule.GetSyncID();
        }

        /// <summary>
        /// 更新排班信息
        /// 1、班组为空，清除当天班次的排班信息
        /// 2、班组不为空，修改当天的排班信息
        /// </summary>
        /// <param name="store">排班信息</param>
        public virtual void UpdateShiftSchedule(ScheduleStore store)
        {
            if (store.WorkGroupId == 0)
            {
                var shiftSchedule = GetShiftSchedule(store.WipResourceId, store.ShiftId, store.StartDate.Date);
                if (shiftSchedule == null)
                    throw new ValidationException("修改班组失败，未找到当天班次排班信息".L10N());
                shiftSchedule.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(shiftSchedule);
            }
            else
            {
                SaveShiftSchedule(store);
            }
        }
    }
}