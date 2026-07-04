using SIE.Domain;
using SIE.EventMessages;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.ShiftTypes
{
    /// <summary>
    /// 班制控制器
    /// </summary>
    public class ShiftTypeController : DomainController
    {
        #region 班制 
        /// <summary>
        /// 获取默认班制信息
        /// </summary>
        /// <returns>班制</returns>
        public virtual ShiftType GetDefaultShiftType()
        {
            return Query<ShiftType>().Where(x => x.IsDefault == YesNo.Yes).FirstOrDefault();
        }

        /// <summary>
        /// 获取班制
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>班制列表</returns>
        public virtual EntityList<ShiftType> GetShiftTypes(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<ShiftType>();
            query.WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            query.Where(p => p.IsWeekend == YesNo.Yes || p.ShiftList.Any());
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据班制编码获取班制列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>班制列表</returns>
        public virtual EntityList<ShiftType> GetShiftTypes(string keyword)
        {
            var query = Query<ShiftType>();
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword));
            return query.ToList();
        }

        /// <summary>
        /// 获取所有班制列表
        /// </summary>
        /// <returns>班制列表</returns>
        public virtual EntityList<ShiftType> GetShiftTypes()
        {
            return Query<ShiftType>().ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 保存班制修改
        /// </summary>
        /// <param name="shiftTypeIds">班制ID列表</param>
        public virtual void SendShiftTypeModifyMessage(List<double> shiftTypeIds)
        {
            if (shiftTypeIds.Any())
                RT.EventBus.Publish(new ShiftTypeModifyEvent() { ShiteTypeIds = shiftTypeIds });
        }
        /// <summary>
        /// 设置缺省班制
        /// </summary>
        /// <param name="shiftType">班制</param>
        public virtual void SetDefaultShiftType(ShiftType shiftType)
        {
            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                var defaultShiftType = GetDefaultShiftType();
                if (defaultShiftType != null)
                {
                    defaultShiftType.IsDefault = YesNo.No;
                    RF.Save(defaultShiftType);
                }
                shiftType.IsDefault = YesNo.Yes;
                RF.Save(shiftType);
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据班制获取有效工作时间（排除休息时间）
        /// </summary>
        /// <param name="shiftType">班制</param>
        /// <returns>有效工作时间</returns>
        public virtual double GetWorkHour(ShiftType shiftType)
        {
            double totalHours = 0;
            if (shiftType == null || shiftType.ShiftList.Count == 0)
                return totalHours;
            var shifts = shiftType.ShiftList;
            foreach (Shift shift in shifts)
            {
                var end = GetDateTimeNotSecond(shift.EndTime);
                if (shift.IsOverDay)
                    end = end.AddDays(1);
                totalHours += Math.Round((end - GetDateTimeNotSecond(shift.BeginTime)).TotalHours, 2);
                foreach (ShiftRest reset in shift.ShiftRestList)
                {
                    var beginReset = GetDateTimeNotSecond(reset.BeginTime);
                    var endReset = GetDateTimeNotSecond(reset.EndTime);
                    if (beginReset > endReset)
                        endReset = endReset.AddDays(1);
                    totalHours -= Math.Round((endReset - beginReset).TotalHours, 2);
                }
            }

            return totalHours;
        }

        /// <summary>
        /// 时间去除秒
        /// </summary>
        /// <param name="dateTime">旧时间</param>
        /// <returns>新时间</returns>
        DateTime GetDateTimeNotSecond(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }
        #endregion

        #region 班次 
        /// <summary>
        /// 获取所有班次列表
        /// </summary>
        /// <returns>班次列表</returns>
        public virtual EntityList<Shift> GetShifts()
        {
            return Query<Shift>().ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 获取所有班次列表
        /// </summary>
        /// <param name="shiftIds">班次Id列表</param>
        /// <returns>班次列表</returns>
        public virtual EntityList<Shift> GetShifts(List<double> shiftIds)
        {
            return Query<Shift>().Where(p => shiftIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取所有班次列表
        /// </summary>
        /// <param name="shiftTypeId">班制ID</param>
        /// <returns> 班次列表</returns>
        public virtual EntityList<Shift> GetShifts(double shiftTypeId)
        {
            return Query<Shift>().Where(x => x.ShiftTypeId == shiftTypeId).ToList();
        }

        /// <summary>
        /// 根据班次名称获取班次
        /// </summary>
        /// <param name="name">班次名称</param>
        /// <returns></returns>
        public virtual Shift GetShiftFromName(string name)
        {
            return Query<Shift>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 删除班次时 同时删除相关班次明细列表
        /// </summary>
        /// <param name="shiftId">班次Id</param>
        public virtual void DeleteShiftDetails(double shiftId)
        {
            var shiftDetails = Query<ShiftDetail>().Where(p => p.ShiftId == shiftId).ToList();
            if (shiftDetails.Count <= 0)
                return;
            shiftDetails.ForEach(e => e.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(shiftDetails);
        }

        /// <summary>
        /// 修改班次时 同时修改相关班次明细列表
        /// </summary>
        /// <param name="shift">班次</param>
        public virtual void UpdateShiftDetails(Shift shift)
        {
            var dataBaseShift = GetById<Shift>(shift.Id);
            if (dataBaseShift.BeginTime == shift.BeginTime && dataBaseShift.EndTime == shift.EndTime)
                return;
            DeleteShiftDetails(shift.Id);
        }

        /// <summary>
        /// 获取班次
        /// 开始时间大于等于查询时间小于结束时间
        /// </summary>
        /// <param name="shifts">班次列表</param>
        /// <param name="nowTime">日期</param>
        /// <returns>班次</returns>
        public virtual Shift GetShift(EntityList<Shift> shifts, DateTime nowTime)
        {
            foreach (var shift in shifts)
            {
                if (shift.IsOverDay)
                {
                    if ((nowTime.TimeOfDay >= shift.BeginTime.TimeOfDay && nowTime.TimeOfDay > shift.EndTime.TimeOfDay)
                        || (nowTime.TimeOfDay < shift.BeginTime.TimeOfDay && nowTime.TimeOfDay <= shift.EndTime.TimeOfDay))
                    {
                        return shift;
                    }
                }
                else
                {
                    if (nowTime.TimeOfDay >= shift.BeginTime.TimeOfDay && nowTime.TimeOfDay < shift.EndTime.TimeOfDay)
                        return shift;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取对应资源的班次列表
        /// </summary>
        /// <param name="wipResourceId">生产资源Id</param>
        /// <returns>班次列表</returns>
        public virtual EntityList<Shift> GetShiftList(double wipResourceId)
        {
            return Query<Shift>().Exists<ShiftType>(
                    (x, y) => y.Join<CalendarSchemeWeek>((c, d) => c.Id == d.ShiftTypeId)
                        .Join<CalendarSchemeWeek, CalendarScheme>((c, d) => c.SchemeId == d.Id)
                        .Join<CalendarScheme, WipResource>((c, d) => c.Id == d.SchemeId && d.Id == wipResourceId)
                        .Where(p => p.Id == x.ShiftTypeId)).ToList();
        }

        /// <summary>
        /// 获取班制日期
        /// </summary>
        /// <param name="shift">班次</param>
        /// <param name="nowTime">采集时间</param>
        /// <returns>班制日期</returns>
        [IgnoreProxy]
        public virtual DateTime GetShiftDate(Shift shift, DateTime nowTime)
        {
            if (shift.IsOverDay && nowTime.TimeOfDay < shift.BeginTime.TimeOfDay && nowTime.TimeOfDay <= shift.EndTime.TimeOfDay)
            {
                return nowTime.AddDays(-1).Date;
            }
            return nowTime.Date;
        }

        #region 班次休息
        /// <summary>
        /// 获取所有班次休息列表
        /// </summary>
        /// <returns>班次休息列表</returns>
        public virtual EntityList<ShiftRest> GetShiftRests()
        {
            return Query<ShiftRest>().ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 获取班次休息列表
        /// </summary>
        /// <param name="shiftId">班次ID</param>
        /// <returns>休息列表</returns>
        public virtual EntityList<ShiftRest> GetShiftRests(double shiftId)
        {
            return Query<ShiftRest>().Where(p => p.ShiftId == shiftId).ToList();
        }

        /// <summary>
        /// 按班次ID列表获取所有班次休息列表
        /// </summary>
        /// <param name="shiftIds">班次ID列表</param>
        /// <returns>班次休息列表</returns>
        public virtual EntityList<ShiftRest> GetAllShiftRestsByShiftIds(List<double> shiftIds)
        {
            return Query<ShiftRest>().Where(x => shiftIds.Contains(x.ShiftId)).ToList();
        }
        #endregion

        ///// <summary>
        ///// 时间的转化比较方法
        ///// 判断班次休息是否在此班次明细时间中
        ///// </summary>
        ///// <param name="dateTime1">班次明细时间开始时间</param>
        ///// <param name="dateTime2">休息时间开始时间</param>
        ///// <param name="dateTime3">班次明细时间结束时间</param>
        ///// <param name="dateTime4">休息时间结束时间</param>
        ///// <returns>bool</returns>
        //private bool CompareTime(DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4)
        //{
        //    var date1 = int.Parse(dateTime1.ToString("HHmm"));
        //    var date2 = int.Parse(dateTime2.ToString("HHmm"));
        //    var date3 = int.Parse(dateTime3.ToString("HHmm"));
        //    var date4 = int.Parse(dateTime4.ToString("HHmm"));
        //    date3 = (date3 == 0 ? 2400 : date3); //结束时间为0时转化为24点

        //    if (date1 <= date2 && date3 >= date4)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// 时间的转化比较方法
        ///// 判断此班次明细时间是否是零点两侧
        ///// </summary>
        ///// <param name="dateTime1">班次明细时间开始时间</param>
        ///// <param name="dateTime2">班次明细时间结束时间</param>
        ///// <returns>bool</returns>
        //private bool CompareTime(DateTime dateTime1, DateTime dateTime2)
        //{
        //    var date1 = int.Parse(dateTime1.ToString("HHmm"));
        //    var date2 = int.Parse(dateTime2.ToString("HHmm"));

        //    if (date1 == 0 || date2 == 0)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        /// <summary>
        /// 获取调整后的时间
        /// </summary>
        /// <param name="begintime">班次休息开始时间</param>
        /// <param name="endtime">班次休息结束时间</param>
        /// <param name="shiftBegintime">班次开始时间</param>
        /// <param name="shiftrestBegintime">调整后班次休息开始时间</param>
        /// <param name="shiftrestEndtime">调整后班次休息结束时间</param>
        public virtual void GetTime(DateTime begintime, DateTime endtime, DateTime shiftBegintime, out int shiftrestBegintime, out int shiftrestEndtime)
        {
            shiftrestBegintime = int.Parse(begintime.ToString("HHmm")); //休息开始时间
            shiftrestEndtime = int.Parse(endtime.ToString("HHmm")); //休息结束时间
            int shiftbegintimeInt = int.Parse(shiftBegintime.ToString("HHmm")); //班次开始时间
            if (shiftrestBegintime > shiftrestEndtime)  //判断是否跨日
            {
                shiftrestEndtime += 2400;
            }

            if (shiftbegintimeInt > shiftrestEndtime && shiftbegintimeInt > shiftrestBegintime)
            {
                shiftrestEndtime += 2400;
                shiftrestBegintime += 2400;
            }
        }

        /// <summary>
        /// 获取重复的字符串
        /// </summary>
        /// <param name="strList">字符串列表</param>
        /// <returns>重复的字符串</returns>
         public virtual string GetDuplicateStr(IEnumerable<string> strList)
        {
            var list = strList.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            return string.Join(",", list);
        }
    
        #endregion
    }
}