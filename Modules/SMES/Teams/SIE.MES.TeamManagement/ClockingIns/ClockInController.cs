using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Common.Models;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.OnLoans;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工出勤控制器
    /// </summary>
    public partial class ClockInController : DomainController
    {
        private const string TIME_FORMAT = "HH:mm";
        #region 员工出勤 
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">实体</param>
        /// <returns>数据</returns>
        public virtual EntityList<EmployeeClockIn> GetEmployeeClockIns(EmployeeClockInCriteria criteria)
        {
            var query = Query<EmployeeClockIn>();
            if (criteria.ShiftRange.BeginValue.HasValue)
            {
                query.Where(p => p.ShiftBegin >= criteria.ShiftRange.BeginValue.Value);
            }
            if (criteria.ShiftRange.EndValue.HasValue)
            {
                query.Where(p => p.ShiftBegin <= criteria.ShiftRange.EndValue.Value);
            }
            if (!criteria.EmployeeCode.IsNullOrEmpty())
                query.Where(p => p.Employee.Code == criteria.EmployeeCode);
            if (!criteria.EmployeeName.IsNullOrEmpty())
                query.Where(p => p.Employee.Name == criteria.EmployeeName);
            if (criteria.WorkGroupId.HasValue)
                query.Where(p => p.WorkGroupId == criteria.WorkGroupId.Value);
            if (criteria.OnDutyState.HasValue)
                query.Where(p => p.OnDutyState == criteria.OnDutyState);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 员工出勤统计查询
        /// </summary>
        /// <param name="criteria">实体</param>
        /// <returns>数据</returns>
        public virtual EntityList<EmployeeClockInAttent> GetEmployeeClockIns(EmployeeClockInAttentCriteria criteria)
        {
            var query = Query<EmployeeClockInAttent>().Where(p => p.WorkGroupId > 0);
            if (criteria.EmployeeDate.HasValue)
                query.Where(p => p.ClockInDate == criteria.EmployeeDate && (p.Employee.HireDate == null || p.Employee.HireDate <= criteria.EmployeeDate));
            if (!criteria.EmployeeCode.IsNullOrEmpty())
                query.Where(p => p.Employee.Code == criteria.EmployeeCode);
            if (!criteria.EmployeeName.IsNullOrEmpty())
                query.Where(p => p.Employee.Name == criteria.EmployeeName);
            if (criteria.WorkGroupId.HasValue)
                query.Where(p => p.WorkGroupId == criteria.WorkGroupId.Value);
            if (criteria.OnDutyState.HasValue)
                query.Where(p => p.OnDutyState == criteria.OnDutyState);
            var model = query.OrderBy(p => criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var emp = RF.GetById<Employee>(RT.IdentityId);
            if (emp != null)
            {
                model.ForEach(p => p.UserEmpType = emp.EmployeeType);
            }

            return model;
        }

        /// <summary>
        /// 员工出勤统计查询导出查询数据
        /// </summary>
        /// <param name="criteria">实体</param>
        /// <param name="dr">出勤时间范围</param>
        /// <returns>数据</returns>
        public virtual EntityList<EmployeeClockInAttent> GetEmployeeClockIns(EmployeeClockInAttentCriteria criteria, DateRange dr)
        {
            var query = Query<EmployeeClockInAttent>().Where(p => p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue)
                .Join<Employee>((x, y) => x.EmployeeId == y.Id && (y.HireDate == null || y.HireDate <= x.ClockInDate));
            if (!criteria.EmployeeCode.IsNullOrEmpty())
                query.Where(p => p.Employee.Code == criteria.EmployeeCode);
            if (!criteria.EmployeeName.IsNullOrEmpty())
                query.Where(p => p.Employee.Name == criteria.EmployeeName);
            if (criteria.WorkGroupId.HasValue)
                query.Where(p => p.WorkGroupId == criteria.WorkGroupId.Value);
            if (criteria.OnDutyState.HasValue)
                query.Where(p => p.OnDutyState == criteria.OnDutyState);
            var model = query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var emp = RF.GetById<Employee>(RT.IdentityId);
            if (emp != null)
            {
                model.ForEach(p => p.UserEmpType = emp.EmployeeType);
            }

            return model;
        }

        /// <summary>
        /// 获取员工考勤信息集合
        /// </summary>
        /// <param name="workGroupId">班组Id</param>
        /// <param name="curDate">考勤日期</param>
        /// <param name="onDutyStates">出勤状态集合</param>
        /// <returns>员工考勤信息集合</returns>
        public virtual EntityList<EmployeeClockIn> GetEmployeeClockIns(double workGroupId, DateTime curDate, List<int> onDutyStates)
        {
            var employeeClockIns = Query<EmployeeClockIn>().Where(a => a.ClockInDate == curDate && a.WorkGroupId == workGroupId
                                    && a.OnDutyState != null && onDutyStates.Contains((int)a.OnDutyState)).ToList();
            return employeeClockIns;
        }

        /// <summary>
        /// 获取出勤数据
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <param name="workGroupId">班组ID</param>
        /// <returns>出勤数据集合</returns>
        public virtual EntityList<EmployeeClockIn> GetEmployeeClockIns(DateRange dr, double? workGroupId = null)
        {
            var query = Query<EmployeeClockIn>().Where(p => p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue);
            if (workGroupId.HasValue)
            {
                query = query.Where(p => p.WorkGroupId == workGroupId.Value);
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取出勤数据
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <param name="workGroupIds">班组Id集合</param>
        /// <returns>出勤数据</returns>
        public virtual EntityList<EmployeeClockIn> GetEmployeeClockIns(DateRange dr, List<double> workGroupIds)
        {
            return Query<EmployeeClockIn>().Where(p => p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue && workGroupIds.Contains(p.WorkGroupId)).ToList();
        }

        /// <summary>
        /// 获取人员出勤导出数据
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="criter">人员出勤查询实体</param>
        /// <returns>导出数据表</returns>
        public virtual ExportDataTable GetExportEmployeeClockIns(int year, int month, int day, EmployeeClockInAttentCriteria criter)
        {
            string[] columns = new string[] { "日期", "工号", "员工类型", "姓名", "性别", "班组", "班次时间", "上班打卡时间", "下班打卡时间", "出勤状态", "工时(小时)", "是否借调", "修改人", "修改时间" };
            if (year == 0 || month == 0)
                throw new ValidationException("年月不能为空".L10N());
            ExportDataTable exportDataTable = new ExportDataTable();
            exportDataTable.Columns.Add(columns);
            List<DataTable> dataTables = new List<DataTable>();
            var begin = DateTime.Parse(year + "-" + month + "-01");
            var end = begin.AddMonths(1).AddDays(-1);
            if (day > 0)
            {
                ////只导出一天的数据
                begin = begin.AddDays(day - 1);
                end = begin;
            }
            var now = RF.Find<Employee>().GetDbTime();
            if (end > now)
                end = now.Date;
            ////结束日期比今天大则取今天为结束
            DateRange dr = new DateRange() { BeginValue = begin, EndValue = end };
            var attentList = RT.Service.Resolve<ClockInController>().GetEmployeeClockIns(criter, dr);
            var shiftList = attentList.Where(p => p.ShiftId > 0).Select(p => p.Shift).AsEntityList();
            var shiftDic = GetShiftDic(shiftList);
            for (DateTime i = begin; i <= end;)
            {
                var data = i.ToShortDateString();
                DataTable dataTable = new DataTable();
                columns.ForEach(column => dataTable.Columns.Add(column));
                var daylist = attentList.Where(p => p.ClockInDate == i).AsEntityList();
                dataTables.Add(dataTable);
                exportDataTable.SheetNames.Add(i.ToString("D"));
                i = i.AddDays(1);
                if (daylist.Count == 0)
                    continue;

                daylist.GroupBy(p => p.WorkGroupId).ForEach(f =>
                {
                        ////以班组分组放数据
                        f.ForEach(p =>
                    {
                        string empType = string.Empty;
                        string shifTime = string.Empty;
                        string attState = string.Empty;
                        string sex = p.EmployeeSex.ToLabel();
                        if (p.EmployeeType.HasValue)
                            empType = p.EmployeeType.ToLabel();
                        if (p.ShiftId.HasValue)
                            shiftDic.TryGetValue(p.ShiftId.Value, out shifTime);
                        if (p.OnDutyState.HasValue)
                            attState = p.OnDutyState.ToLabel();
                        DataRow row = dataTable.NewRow();
                        row[0] = data;
                        row[1] = p.EmployeeCode;
                        row[2] = empType;
                        row[3] = p.EmployeeName;
                        row[4] = sex;
                        row[5] = p.WorkGroupName;
                        row[6] = shifTime;
                        row[7] = p.OnDutyDate;
                        row[8] = p.OffDutyDate;
                        row[9] = attState;
                        row[10] = p.AttentHour;
                        row[11] = p.IsLoan == YesNo.Yes ? "是" : "否";
                        row[12] = p.UpdateByName;
                        row[13] = p.UpdateDate;
                        dataTable.Rows.Add(row);
                    });
                });
            }
            exportDataTable.Tables.AddRange(dataTables);
            return exportDataTable;
        }

        /// <summary>
        /// 设置班次与班次时间字典
        /// </summary>
        /// <param name="list">数据</param>
        /// <returns>字典</returns>
        [IgnoreProxy]
        public virtual Dictionary<double, string> GetShiftDic(EntityList<Shift> list)
        {
            var dic = new Dictionary<double, string>();
            list.ForEach(item =>
            {
                string shiftTime = string.Empty;
                if (item.IsOverDay && item.EndTime < item.BeginTime)
                {
                    shiftTime = item.BeginTime.ToString(TIME_FORMAT) + "-(次日)" + item.EndTime.ToString(TIME_FORMAT);
                }
                else
                {
                    shiftTime = item.BeginTime.ToString(TIME_FORMAT) + "-" + item.EndTime.ToString(TIME_FORMAT);
                }

                string r = string.Empty;
                if (!dic.TryGetValue(item.Id, out r))
                {
                    dic.Add(item.Id, shiftTime);
                }
            });
            return dic;
        }

        /// <summary>
        /// 获取出勤取数配置
        /// </summary>
        /// <returns>配置项</returns>
        public virtual EmployeeClockInSetConfigValue GetEmployeeClockInSetConfig()
        {
            var config = ConfigService.GetConfig(new EmployeeClockInSetConfig(), typeof(EmployeeClockIn));
            if (config == null)
            {
                throw new ValidationException("未找到员工出勤配置，请检查".L10N());
            }

            return config;
        }

        /// <summary>
        /// 第一步调度执行获取有效员工数据并同步到员工出勤表
        /// </summary>
        /// <returns>提示信息</returns>
        public virtual string GetEffectEmployee()
        {
            try
            {
                var date = RF.Find<EmployeeClockIn>().GetDbTime().Date;
                DateRange dr = new DateRange() { BeginValue = date, EndValue = date };
                var existEmp = GetEmployeeClockIns(dr);
                List<double> existIds = new List<double>();
                if (existEmp.Count > 0) existIds = existEmp.Select(p => p.EmployeeId).ToList();

                var employeeList = RT.Service.Resolve<EmployeeController>().GetEmployeeListOnJob(existIds);
                if (employeeList.Count == 0) return string.Empty; //"没有需要同步的员工信息";
                EntityList<EmployeeClockIn> rstList = new EntityList<EmployeeClockIn>();
                Dictionary<double, ShiftSchedule> dic = new Dictionary<double, ShiftSchedule>();
                var workGroupIds = employeeList.Select(p => p.WorkGroupId).Distinct().ToList();
                foreach (var workGroupId in workGroupIds)
                {
                    var shiftSchedule = RT.Service.Resolve<ShiftScheduleController>().GetShiftSchedule(date, workGroupId.Value);
                    if (shiftSchedule != null)
                        dic.Add(workGroupId.Value, shiftSchedule);
                }

                foreach (var emp in employeeList)
                {
                    EmployeeClockIn item = new EmployeeClockIn();
                    item.ClockInDate = date;
                    item.EmployeeId = emp.Id;
                    item.EmployeeCode = emp.Code;
                    item.WorkGroupId = emp.WorkGroupId.Value;
                    ShiftSchedule shift;
                    dic.TryGetValue(item.WorkGroupId, out shift);
                    ////无排班==休息
                    if (shift == null || shift.ShiftId == 0) item.OnDutyState = OnDutyState.Rest;
                    else
                    {
                        item.ShiftId = shift.ShiftId;
                        item.WipResourceId = shift.WipResourceId;
                        item.WorkShopId = shift.WorkShopId;
                        item.ShiftBegin = date + shift.Shift.BeginTime.TimeOfDay;
                        item.ShiftEnd = date + shift.Shift.EndTime.TimeOfDay;
                    }

                    rstList.Add(item);
                }

                RF.Save(rstList);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 员工更新班组同步数据
        /// </summary>
        /// <param name="newEmp">员工</param>
        public virtual void UpdateEmployeeClockIn(Employee newEmp)
        {
            var date = RF.Find<EmployeeClockIn>().GetDbTime().Date;
            var emp = GetEmployeeClockIn(date, newEmp.Id);
            if (emp == null)
            {
                emp = new EmployeeClockIn();
                emp.EmployeeId = newEmp.Id;
                emp.EmployeeCode = newEmp.Code;
                emp.ClockInDate = date;
            }

            var schedule = RT.Service.Resolve<ShiftScheduleController>().GetShiftSchedule(date, newEmp.WorkGroupId.Value);
            if (schedule == null)
            {
                emp.OnDutyState = OnDutyState.Rest;
                emp.ShiftId = null;
                emp.ShiftBegin = null;
                emp.ShiftEnd = null;
            }
            else
            {
                emp.ShiftId = schedule.ShiftId;
                emp.ShiftBegin = date + schedule.Shift.BeginTime.TimeOfDay;
                emp.ShiftEnd = date + schedule.Shift.EndTime.TimeOfDay;
            }

            emp.WorkGroupId = newEmp.WorkGroupId.Value;
            RF.Save(emp);
        }

        /// <summary>
        /// 检查补录的考勤数据是否有效
        /// </summary>
        /// <param name="list">考勤数据</param>
        public virtual void CheckAttentEditData(EntityList<EmployeeClockInAttent> list)
        {
            var config = GetEmployeeClockInSetConfig();
            var shiftBeginBefore = config.ShiftBeginBefore;
            var shiftBeginAfter = config.ShiftBeginAfter;
            var shiftEndBefore = config.ShiftEndBefore;
            var shiftEndAfter = config.ShiftEndAfter;
            foreach (var item in list)
            {
                if (item.Shift == null) continue;
                var beginValue = (item.ClockInDate.Date + item.Shift.BeginTime.TimeOfDay).AddMinutes(-shiftBeginBefore);
                var endValue = (item.ClockInDate.Date + item.Shift.BeginTime.TimeOfDay).AddMinutes(shiftBeginAfter);
                if (beginValue > endValue) { beginValue = beginValue.AddDays(-1); }
                if (item.OnDutyDate.HasValue && !(item.OnDutyDate >= beginValue && item.OnDutyDate <= endValue))
                {
                    throw new ValidationException(item.Employee.Name + "(" + item.Employee.Code + ")上班打卡时间不在有效范围".L10N());
                }

                if (!item.OffDutyDate.HasValue)
                    continue;

                beginValue = (item.ClockInDate.Date + item.Shift.EndTime.TimeOfDay).AddMinutes(-shiftEndBefore);
                endValue = (item.ClockInDate.Date + item.Shift.EndTime.TimeOfDay).AddMinutes(shiftEndAfter);
                if (item.Shift.BeginTime.TimeOfDay > item.Shift.EndTime.TimeOfDay)
                {
                    beginValue = beginValue.AddDays(1);
                    endValue = endValue.AddDays(1);
                }
                if (beginValue > endValue) { endValue = endValue.AddDays(1); }
                if (!(item.OffDutyDate >= beginValue && item.OffDutyDate <= endValue))
                    throw new ValidationException(item.Employee.Name + "(" + item.Employee.Code + ")下班打卡时间不在有效范围".L10N());

            }
        }

        /// <summary>
        /// 更新日期范围内的考勤数据
        /// </summary>
        /// <param name="dr">日期范围Date大于等于Begin并且Date小于等于End</param>
        /// <returns>msg</returns>
        public virtual string ExeEmployeeClockInState(DateRange dr)
        {
            var config = GetEmployeeClockInSetConfig();
            var onDutyType = config.OnDutyTimeType;
            var offDutyType = config.OffDutyTimeTypeType;
            var shiftBeginBefore = config.ShiftBeginBefore;
            var shiftBeginAfter = config.ShiftBeginAfter;
            var shiftEndBefore = config.ShiftEndBefore;
            var shiftEndAfter = config.ShiftEndAfter;
            var effectData = GetEmployeeEffectClockIns(dr);
            var nowtime = RF.Find<EmployeeClockIn>().GetDbTime();
            bool isExcToday = false;

            if (effectData.Count == 0) return "没有需要计算的出勤员工记录";
            var shiftList = effectData.Where(p => p.ShiftId.HasValue).Select(p => p.Shift).Distinct();
            var dicBegin = new Dictionary<double, DateRange>();
            var dicEnd = new Dictionary<double, DateRange>();
            foreach (var shiftId in shiftList.Select(p => p.Id).Distinct())
            {
                var shift = shiftList.FirstOrDefault(p => p.Id == shiftId);
                dicBegin.Add(shiftId, new DateRange() { BeginValue = shift.BeginTime.AddMinutes(-shiftBeginBefore), EndValue = shift.BeginTime.AddMinutes(shiftBeginAfter) });
                if (shift.IsOverDay)
                {
                    //跨日的结束打卡时间范围
                    shift.EndTime = shift.BeginTime.AddDays(1).Date + shift.EndTime.TimeOfDay;
                }

                dicEnd.Add(shiftId, new DateRange() { BeginValue = shift.EndTime.AddMinutes(-shiftEndBefore), EndValue = shift.EndTime.AddMinutes(shiftEndAfter) });
            }

            effectData.Where(p => !p.ShiftId.HasValue).ForEach(p => { p.OnDutyState = OnDutyState.Rest; p.AttentHour = 0; });

            effectData.Where(p => p.ShiftId.HasValue).ForEach(p =>
            {
                isExcToday = false;
                if (p.ClockInDate == nowtime.Date)
                    isExcToday = true;
                ////跨日，结束时间+1
                var beginRange = new DateRange();
                dicBegin.TryGetValue(p.ShiftId.Value, out beginRange);
                ////当天考勤状态还没到上班时间不用计算
                if (isExcToday && nowtime.TimeOfDay < beginRange.BeginValue.Value.TimeOfDay)
                {
                    return;
                }
                var endRange = new DateRange();
                dicEnd.TryGetValue(p.ShiftId.Value, out endRange);
                var onTimeList = p.ClockInDetail.Where(e => e.ClockInDate.TimeOfDay >= beginRange.BeginValue.Value.TimeOfDay && e.ClockInDate.TimeOfDay <= beginRange.EndValue.Value.TimeOfDay).AsEntityList();
                var offTimeList = p.ClockInDetail.Where(e => e.ClockInDate.TimeOfDay >= endRange.BeginValue.Value.TimeOfDay && e.ClockInDate.TimeOfDay <= endRange.EndValue.Value.TimeOfDay).AsEntityList();
                var onDutyDate = p.OnDutyDate;
                var offDutyDate = p.OffDutyDate;
                var state = p.OnDutyState;
                if (onTimeList.Count > 0)
                {
                    if (onDutyType == OnDutyType.Earliest)
                        p.OnDutyDate = onTimeList.Min(e => e.ClockInDate);
                    else
                        p.OnDutyDate = onTimeList.Max(e => e.ClockInDate);
                }

                if (offTimeList.Count > 0)
                {
                    if (offDutyType == OnDutyType.Earliest)
                        p.OffDutyDate = offTimeList.Min(e => e.ClockInDate);
                    else
                        p.OffDutyDate = offTimeList.Max(e => e.ClockInDate);
                }
                ////如果是计算当天的考勤，在上班后-最晚下班前有打上班卡不算异常，在下班后缺卡算异常
                if ((p.OnDutyDate.HasValue && p.OffDutyDate.HasValue) || (isExcToday && p.OnDutyDate.HasValue && nowtime.TimeOfDay < endRange.EndValue.Value.TimeOfDay))
                    p.OnDutyState = OnDutyState.Normal;
                else
                    p.OnDutyState = OnDutyState.Absence;

                if (p.OnDutyDate != onDutyDate || p.OffDutyDate != offDutyDate || p.OnDutyState != state)
                {
                    p.PersistenceStatus = PersistenceStatus.Modified;
                }
                p.IsLoan = RT.Service.Resolve<OnLoanController>().IsExistOnLoan(p.EmployeeId, p.ClockInDate) ? YesNo.Yes : YesNo.No;

                if (p.OnDutyState == OnDutyState.Absence)
                {
                    p.AttentHour = 0;
                }
                else
                {
                    //当天而且还没到下班时间，出勤时长不用算,其他情况都要算
                    if (!(isExcToday && nowtime.TimeOfDay < endRange.EndValue.Value.TimeOfDay))
                    {
                        var curEffectShift = shiftList.FirstOrDefault(e => e.Id == p.ShiftId);
                        double restHour = 0;
                        curEffectShift.ShiftRestList.ForEach(e =>
                        {
                            if (curEffectShift.IsOverDay && e.EndTime < e.BeginTime)
                            {
                                    //跨天而且休息结束时间比开始时间少
                                    e.EndTime = e.BeginTime.Date.AddDays(1) + e.EndTime.TimeOfDay;
                            }

                            restHour += (e.EndTime - e.BeginTime).TotalHours;
                        });
                        var hour = (curEffectShift.EndTime - curEffectShift.BeginTime).TotalHours - restHour;
                        p.AttentHour = decimal.Parse(hour.ToString("F1"));
                    }
                }

            });
            RF.Save(effectData);
            return string.Empty;
        }

        /// <summary>
        /// 获取需要更新的出勤数据
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <returns>出勤数据集合</returns>
        public virtual EntityList<EmployeeClockIn> GetEmployeeEffectClockIns(DateRange dr)
        {
            return Query<EmployeeClockIn>().Where(p => p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue
            && (p.OnDutyState != OnDutyState.Rest || p.OffDutyDate == null)).ToList();
        }

        /// <summary>
        /// 根据时间和状态获取出勤员工信息
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <param name="state">状态</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>员工信息集合</returns>
        public virtual EntityList<EmployeeClockIn> GetEmployeeClockInByState(DateRange dr, OnDutyState state, PagingInfo pagingInfo = null)
        {
            return Query<EmployeeClockIn>().Where(p => p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue && p.OnDutyState == state).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据时间和状态获取出勤员工信息
        /// </summary>
        /// <param name="workGroupId">班组ID</param>
        /// <param name="dr">时间范围</param>
        /// <param name="state">出勤状态</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>员工信息集合</returns>
        public virtual EntityList<EmployeeClockIn> GetEmployeeClockInByWorkGroup(double workGroupId, DateRange dr, OnDutyState state, PagingInfo pagingInfo = null)
        {
            return Query<EmployeeClockIn>().Where(p => p.WorkGroupId == workGroupId && p.ClockInDate >= dr.BeginValue && p.ClockInDate <= dr.EndValue && p.OnDutyState == state).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取出勤员工信息ByCode
        /// </summary>
        /// <param name="code">工号</param>
        /// <param name="dt">日期</param>
        /// <returns>出勤信息</returns>
        public virtual EmployeeClockIn GetEmployeeClockIn(string code, DateTime dt)
        {
            return Query<EmployeeClockIn>().NotExists<ClockInDetail>((x, y) => y.Where(e => e.ClockInId == x.Id && e.ClockInDate == dt)).Where(p => p.EmployeeCode == code && p.ClockInDate == dt.Date).FirstOrDefault();
        }

        /// <summary>
        /// 获取出勤数据
        /// </summary>
        /// <param name="dt">时间日期</param>
        /// <param name="employeeId">员工Id</param>
        /// <returns>出勤数据</returns>
        public virtual EmployeeClockIn GetEmployeeClockIn(DateTime dt, double employeeId)
        {
            return Query<EmployeeClockIn>().Where(p => p.ClockInDate == dt && p.EmployeeId == employeeId).FirstOrDefault();
        }

        /// <summary>
        /// 获取出勤数据
        /// </summary>
        /// <param name="employeeIds">员工Id列表</param>
        /// <returns>出勤数据</returns>
        public virtual EntityList<EmployeeClockInAttent> GetEmployeeClockInAttentList(List<double> employeeIds)
        {
            return Query<EmployeeClockInAttent>().Where(p => p.ShiftId != null && employeeIds.Contains(p.EmployeeId)).ToList();
        }

        /// <summary>
        /// 更新出勤数据的借调状态
        /// </summary>
        /// <param name="employeeIds"></param>
        public virtual void UpdateEmployeeClockInLoan(List<double> employeeIds)
        {
            var effectDataList = GetEmployeeClockInAttentList(employeeIds);
            foreach (var effectData in effectDataList)
            {
                effectData.IsLoan = RT.Service.Resolve<OnLoanController>().IsExistOnLoan(effectData.EmployeeId, effectData.ClockInDate) ? YesNo.Yes : YesNo.No;
                effectData.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(effectData);
            }
        }
        #endregion

        #region 考勤机
        /// <summary>
        /// 连接并获取考勤机信息
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        /// <returns>考勤机信息</returns>
        public virtual ClockInMachine ReadMachineInfo(string ip, int port)
        {
            try
            {
                //MES考勤机
                //zkemkeeper.CZKEMClass zke = new zkemkeeper.CZKEMClass();

                //bool rst = zke.Connect_Net(ip, port);
                //if (rst)
                //{
                //    ClockInMachine item = new ClockInMachine();
                //    item.IpAddress = ip;
                //    item.Port = port;
                //    string sn = string.Empty;
                //    string model = string.Empty;
                //    zke.GetSerialNumber(1, out sn);
                //    item.SN = sn;
                //    zke.GetProductCode(1, out model);
                //    item.Model = model;
                //    return item;
                //}
                //else
                //{
                //    throw new ValidationException("考勤机没连接成功，请输入正确的考勤机IP或端口\r\n在考勤机->通讯设置->网络设置中查看".L10N());
                //}
                return null;
            }
            catch (Exception ex)
            {
                throw new ValidationException("连接考勤机失败：".L10N() + ex.Message);
            }
            ////return new ClockInMachine();
        }

        /// <summary>
        /// 获取卡机列表
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual EntityList<ClockInMachine> GetMachineList()
        {
            return Query<ClockInMachine>().Where(p => p.Id > 0).ToList();
        }

        #endregion
    }
}