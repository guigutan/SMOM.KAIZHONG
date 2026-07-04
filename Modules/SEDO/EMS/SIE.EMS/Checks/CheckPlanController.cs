using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.ApiModels;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Configs;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Data;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Common.Utils;
using SIE.EMS.DataAuth;
using SIE.EMS.DevicePurs;
using SIE.EMS.DevicePurs.ApiModels;
using SIE.EMS.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.Tpms;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.EventMessages.EAP.Equipments;
using SIE.EventMessages.EMS.Repairs;
using SIE.Rbac.Users;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Enterprises;
using SIE.Resources.Holidays;
using SIE.Resources.ShiftTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SIE.EMS.Checks
{
    /// <summary>
    /// 点检计划维护控制器
    /// </summary>
    public partial class CheckPlanController : DomainController
    {
        private const string ShiftError = "配置的日历方案的周方案的班别的班次为空";
        private const string NotConfigCalendarSchema = "未找到配置的日历方案";

        #region 添加点检计划
        /// <summary>
        /// 自动生成点检计划
        /// </summary>
        public virtual void AutoCreateCheckPlans(CheckPlanCrtJobParameter param)
        {
            // 获取调度参数下的、不分权限过滤的设备台账
            List<BaseDataInfo> equipList = new List<BaseDataInfo>();
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                equipList = Query<EquipAccount>().Where(p => p.UseState == AccountUseState.Using)
                    .WhereIf(param.FactoryId != null && param.FactoryId != 0, p => p.FactoryId == param.FactoryId)
                    .WhereIf(param.WorkShopId != null && param.WorkShopId != 0, p => p.WorkShopId == param.WorkShopId)
                    .WhereIf(param.UseDptId != null && param.UseDptId != 0, p => p.UseDepartmentId == param.UseDptId)
                    .WhereIf(param.ResourceId != null && param.ResourceId != 0, p => p.ResourceId == param.ResourceId)
                    .Select(p => new
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                    })
                    .ToList<BaseDataInfo>().ToList();
            }
            if (equipList.Count > 0)
            {
                AddCheckPlanViewModel model = new AddCheckPlanViewModel();
                model.EquipCheckType = EquipCheckType.BatchAdd;
                model.BeginDate = DateTime.Now.Date;
                model.EndDate = DateTime.Now.Date;
                model.CheckTime = 30;
                BatchAddCheckPlan(model, equipList, CheckSourceType.PLAN);
            }
        }

        /// <summary>
        /// 重新格式化开始时间和结束时间
        /// </summary>
        /// <param name="model"></param>
        public virtual void RechangeDateTime(AddCheckPlanViewModel model)
        {
            model.BeginDate = new DateTime(model.BeginDate.Year, model.BeginDate.Month, model.BeginDate.Day, 0, 0, 0);
            model.EndDate = new DateTime(model.EndDate.Year, model.EndDate.Month, model.EndDate.Day, 23, 59, 59);
        }

        /// <summary>
        /// 添加点检计划
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        /// <param name="sourceType">来源</param>
        public virtual CheckPlan AddCheckPlan(AddCheckPlanViewModel model, CheckSourceType sourceType)
        {
            if (model == null)
            {
                return new CheckPlan();
            }
            try
            {
                //获取点检类型(日、班、频次)
                CheckPlanType checkType = RT.Service.Resolve<CheckController>().GetCheckPlanType();
                var equip = RT.Service.Resolve<EquipAccountController>().GetEquipAccountBaseInfo(model.EquipAccountId);
                CheckPlan plan = null;
                switch (checkType)
                {
                    case CheckPlanType.Day:
                        plan = AddCheckPlanForDay(model, new List<BaseDataInfo>() { equip }, sourceType)?.FirstOrDefault();
                        break;
                    case CheckPlanType.Shift:
                        plan = AddCheckPlanForShift(model, new List<BaseDataInfo>() { equip }, sourceType)?.FirstOrDefault();
                        break;
                    case CheckPlanType.Time:
                        plan = AddCheckPlanForTime(model, new List<BaseDataInfo>() { equip }, sourceType)?.FirstOrDefault();
                        break;
                    default:
                        break;
                }
                return plan;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 批量添加点检计划
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        /// <param name="equipList">设备列表</param>
        /// <param name="sourceType">来源</param>
        public virtual void BatchAddCheckPlan(AddCheckPlanViewModel model, List<BaseDataInfo> equipList, CheckSourceType sourceType)
        {
            try
            {
                //获取点检类型(日、班、频次)
                CheckPlanType checkType = RT.Service.Resolve<CheckController>().GetCheckPlanType();
                switch (checkType)
                {
                    case CheckPlanType.Day:
                        AddCheckPlanForDay(model, equipList, sourceType);
                        break;
                    case CheckPlanType.Shift:
                        AddCheckPlanForShift(model, equipList, sourceType);
                        break;
                    case CheckPlanType.Time:
                        AddCheckPlanForTime(model, equipList, sourceType);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 添加日点检计划
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        /// <param name="equipList">设备列表</param>
        /// <param name="sourceType">来源</param>
        public virtual EntityList<CheckPlan> AddCheckPlanForDay(AddCheckPlanViewModel model, List<BaseDataInfo> equipList, CheckSourceType sourceType)
        {
            try
            {
                // 格式化计划区间
                RT.Service.Resolve<CheckPlanController>().RechangeDateTime(model);

                //是否按部门进行点检
                bool isDepartmentCheck = RT.Service.Resolve<CheckController>().IsDepartmentCheck();

                //用于保存新点检计划的集合
                EntityList<CheckPlan> planList = new EntityList<CheckPlan>();

                //获取周方案
                CalendarScheme calendarScheme = RT.Service.Resolve<CheckController>().GetCalendarScheme();

                if (calendarScheme == null)
                {
                    throw new ValidationException(NotConfigCalendarSchema.L10N());
                }

                var dicInfo = RT.Service.Resolve<CalendarSchemeController>()
                    .GetShiftTypesByCalendarSchemeAndDataRange(calendarScheme, model.BeginDate.Date, model.EndDate.Date);

                var holidays = RT.Service.Resolve<HolidayController>().GetIntersectHoliday(model.BeginDate, model.EndDate);

                //查询日期范围内已生成的点检计划
                var equipAccountIds = equipList.Select(x => x.Id).Distinct().ToList();
                EntityList<CheckPlan> checkPlans = GetExeCheckPlanList(equipAccountIds, CheckPlanType.Day, model.BeginDate, model.EndDate);

                //获取所有设备的点检项目
                var checkProjectsOfAccounts = RT.Service.Resolve<EquipController>()
                    .GetCheckProjectsOfAccounts(equipAccountIds, CheckPlanType.Day);


                var dayQty = (model.EndDate.Date - model.BeginDate.Date).TotalDays + 1;
                var checkNoList = CountSingleDayCheckPlanNoList((int)dayQty, equipList, checkProjectsOfAccounts, isDepartmentCheck);//一次获取多个单号，避免多次访问数据库
                var indexNo = 0;

                ParallelOptions paraOP = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
                object lockObj = new object();//线程锁

                ParallelLoopResult result = Parallel.ForEach<BaseDataInfo>(equipList, paraOP, equip =>
                {
                    //查询该台设备生成过的计划
                    List<CheckPlan> plans = checkPlans.Where(p => p.EquipAccountId == equip.Id).ToList();

                    for (DateTime date = model.BeginDate; date.Date <= model.EndDate.Date; date = date.AddDays(1))
                    {
                        //根据日历方案算出点检计划白班的开始时间和结束时间
                        if (!dicInfo.ContainsKey(date))
                        {
                            continue;
                        }

                        //法定假日
                        if (holidays.Any(x => date >= x.BeginDate && date <= x.EndDate))
                        {
                            continue;
                        }

                        if (plans.Any(p => p.CheckBeginDate.Date == date.Date))
                        {
                            continue;
                        }

                        var shiftType = dicInfo[date];
                        if (shiftType.ShiftList == null || !shiftType.ShiftList.Any())
                        {
                            throw new ValidationException(ShiftError.L10N());
                        }

                        var beginDate = shiftType.ShiftList.Min(p => p.BeginTime);
                        var endDate = shiftType.ShiftList.First(p => p.BeginTime == beginDate).EndTime;

                        if (isDepartmentCheck) // 按责任部门生成
                        {
                            // 责任部门Ids
                            var deptIds = checkProjectsOfAccounts.Where(p => p.EquipAccountId == equip.Id && p.DepartmentId != null && p.DepartmentId != 0).Select(p => (double)p.DepartmentId).Distinct().ToList<double>();
                            if (deptIds.Count <= 0)
                            {
                                continue;
                            }
                            CreateCheckPlanSingleDayWithDept(planList, model, sourceType, equip, deptIds, date, beginDate, endDate, checkNoList, ref indexNo, lockObj);
                        }
                        else
                        {
                            CreateCheckPlanSingleDay(planList, model, sourceType, equip, date, beginDate, endDate, checkNoList, ref indexNo, lockObj);
                        }
                        
                    }
                });

                if (!planList.Any())
                {
                    throw new ValidationException("没有可生成的点检计划！".L10N());
                }
                if (result.IsCompleted)
                {
                    RF.BatchInsert(planList);
                }
                return planList;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 计算按日生成所需的单号数
        /// </summary>
        /// <param name="day">天数</param>
        /// <param name="equipList">设备台账</param>
        /// <param name="equipAccountCheckProjects">点检项目</param>
        /// <param name="isDepartmentCheck">是否按部门生成点检</param>
        /// <returns></returns>
        private List<string> CountSingleDayCheckPlanNoList(int day, List<BaseDataInfo> equipList, EntityList<EquipAccountCheckProject> equipAccountCheckProjects, bool isDepartmentCheck)
        {
            if (!isDepartmentCheck)
            {
                return RT.Service.Resolve<CheckController>().GetCheckPlanNoList(day * equipList.Count);
            }
            else
            {
                int toCount = 0;
                foreach (var equip in equipList)
                {
                    var deptCount = equipAccountCheckProjects.Where(p => p.EquipAccountId == equip.Id && p.DepartmentId != null && p.DepartmentId != 0).Select(p => (double)p.DepartmentId).Distinct().Count();
                    if (deptCount <= 0)
                    {
                        continue;
                    }
                    toCount += day * deptCount;
                }
                return RT.Service.Resolve<CheckController>().GetCheckPlanNoList(toCount);
            }
        }

        /// <summary>
        /// 创建单天的点检计划
        /// </summary>
        /// <param name="planList">点检计划保存列表</param>
        /// <param name="model">点检计划信息</param>
        /// <param name="sourceType">数据来源</param>
        /// <param name="equip">设备信息</param>
        /// <param name="deptIds">责任部门</param>
        /// <param name="date">当天日期</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="checkNoList">点检单号</param>
        /// <param name="indexNo">下标</param>
        /// <param name="lockObj">线程锁</param>
        private static void CreateCheckPlanSingleDayWithDept(EntityList<CheckPlan> planList, AddCheckPlanViewModel model, CheckSourceType sourceType, BaseDataInfo equip,
             List<double> deptIds, DateTime date, DateTime beginDate, DateTime endDate, List<string> checkNoList, ref int indexNo, object lockObj)
        {
            if (deptIds.Any())
            {
                foreach (var deptId in deptIds)
                {
                    CheckPlan plan = new CheckPlan();
                    plan.CheckSourceType = sourceType;
                    plan.EquipAccountId = equip.Id;
                    plan.CheckCycleType = CheckCycleType.Day;
                    plan.CheckPlanType = CheckPlanType.Day;
                    plan.WhetherAcrossDay = YesNo.No;
                    plan.ExeState = CheckExeState.NotPerformed;
                    plan.EquipCheckType = model.EquipCheckType;
                    plan.CheckBeginDate = date.Date.AddHours(beginDate.Hour).AddMinutes(beginDate.Minute);
                    plan.CheckEndDate = date.Date.AddHours(endDate.Hour).AddMinutes(endDate.Minute);
                    plan.CheckTime = model.CheckTime;
                    plan.DepartmentId = deptId;
                    lock (lockObj)
                    {
                        plan.CheckPlanNo = checkNoList[indexNo++];
                        planList.Add(plan);
                    }
                }
            }
        }

        /// <summary>
        /// 创建单天的点检计划
        /// </summary>
        /// <param name="planList">点检计划保存列表</param>
        /// <param name="model">点检计划信息</param>
        /// <param name="sourceType">数据来源</param>
        /// <param name="equip">设备信息</param>
        /// <param name="date">当天日期</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="checkNoList">点检单号</param>
        /// <param name="indexNo">下标</param>
        /// <param name="lockObj">线程锁</param>
        private static void CreateCheckPlanSingleDay(EntityList<CheckPlan> planList, AddCheckPlanViewModel model, CheckSourceType sourceType, BaseDataInfo equip,
             DateTime date, DateTime beginDate, DateTime endDate, List<string> checkNoList, ref int indexNo, object lockObj)
        {
            CheckPlan plan = new CheckPlan();
            plan.CheckSourceType = sourceType;
            plan.EquipAccountId = equip.Id;
            plan.CheckCycleType = CheckCycleType.Day;
            plan.CheckPlanType = CheckPlanType.Day;
            plan.WhetherAcrossDay = YesNo.No;
            plan.ExeState = CheckExeState.NotPerformed;
            plan.EquipCheckType = model.EquipCheckType;
            plan.CheckBeginDate = date.Date.AddHours(beginDate.Hour).AddMinutes(beginDate.Minute);
            plan.CheckEndDate = date.Date.AddHours(endDate.Hour).AddMinutes(endDate.Minute);
            plan.CheckTime = model.CheckTime;
            lock (lockObj)
            {
                plan.CheckPlanNo = checkNoList[indexNo++];
                planList.Add(plan);
            }
        }

        private EntityList<Shift> GetShiftList()
        {
            CalendarScheme scheme = RT.Service.Resolve<CheckController>().GetCalendarScheme();

            if (scheme == null)
            {
                throw new ValidationException(NotConfigCalendarSchema.L10N());
            }

            CalendarSchemeWeek schemeWeek = scheme.SchemeWeeks.OrderByDescending(p => p.ActiveDate).FirstOrDefault(p => p.ActiveDate <= DateTime.Now.Date);

            if (schemeWeek == null)
            {
                throw new ValidationException("日历方案【{0}】未找到符合规则的周方案配置，请检查周方案的预启用日期是否小于等于当前日期".L10nFormat(scheme.Name));
            }

            if (schemeWeek.ShiftType == null)
            {
                throw new ValidationException("周方案【{0}】的班制为空，请检查。".L10nFormat(schemeWeek.Name));
            }

            EntityList<Shift> shiftList = schemeWeek.ShiftType.ShiftList;

            if (!schemeWeek.ShiftType.ShiftList.Any())
            {
                throw new ValidationException("班制【{0}】的班次列表为空，请检查。".L10nFormat(schemeWeek.ShiftType.Name));
            }

            return shiftList;
        }

        /// <summary>
        /// 添加班点检计划
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        /// <param name="equipList">设备列表</param>
        /// <param name="sourceType">来源</param>
        public virtual EntityList<CheckPlan> AddCheckPlanForShift(AddCheckPlanViewModel model, List<BaseDataInfo> equipList, CheckSourceType sourceType)
        {
            try
            {
                // 格式化计划区间
                RT.Service.Resolve<CheckPlanController>().RechangeDateTime(model);

                //是否按部门进行点检
                bool isDepartmentCheck = RT.Service.Resolve<CheckController>().IsDepartmentCheck();

                //获取周方案
                CalendarScheme calendarScheme = RT.Service.Resolve<CheckController>().GetCalendarScheme();

                if (calendarScheme == null)
                {
                    throw new ValidationException(NotConfigCalendarSchema.L10N());
                }

                var dicInfo = RT.Service.Resolve<CalendarSchemeController>()
                    .GetShiftTypesByCalendarSchemeAndDataRange(calendarScheme, model.BeginDate, model.EndDate);

                var holidays = RT.Service.Resolve<HolidayController>().GetIntersectHoliday(model.BeginDate, model.EndDate);

                //用于保存新点检计划的集合
                EntityList<CheckPlan> planList = new EntityList<CheckPlan>();

                //查询日期范围内已生成的点检计划
                var equipAccountIds = equipList.Select(x => x.Id).Distinct().ToList();
                EntityList<CheckPlan> checkPlans = GetExeCheckPlanList(equipAccountIds, CheckPlanType.Shift, model.BeginDate, model.EndDate);

                //获取所有设备的点检项目
                var checkProjectsOfAccounts = RT.Service.Resolve<EquipController>().GetCheckProjectsOfAccounts(equipAccountIds, CheckPlanType.Shift);


                ParallelOptions paraOP = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
                object lockObj = new object();//线程锁

                ParallelLoopResult result = Parallel.ForEach<BaseDataInfo>(equipList, paraOP, equip =>
                {
                    //查询该台设备生成过的计划
                    List<CheckPlan> plans = checkPlans.Where(p => p.EquipAccountId == equip.Id).ToList();

                    for (DateTime date = model.BeginDate; date.Date <= model.EndDate.Date; date = date.AddDays(1))
                    {
                        //根据日历方案算出点检计划白班的开始时间和结束时间
                        if (!dicInfo.ContainsKey(date))
                        {
                            continue;
                        }

                        //法定假日
                        if (holidays.Any(x => date >= x.BeginDate && date <= x.EndDate))
                        {
                            continue;
                        }

                        // 是否已生成
                        if (plans.Any(p => p.CheckBeginDate.Date == date.Date))
                        {
                            continue;
                        }

                        // 当天的班制信息
                        var shiftType = dicInfo[date];
                        if (shiftType.ShiftList == null || !shiftType.ShiftList.Any())
                        {
                            throw new ValidationException(ShiftError.L10N());
                        }
                        var shiftCount = shiftType.ShiftList.Count;


                        if (isDepartmentCheck) // 按点检项目责任部门生成点检单
                        {
                            // 责任部门ids
                            var deptIds = checkProjectsOfAccounts.Where(x => x.EquipAccountId == equip.Id && x.DepartmentId != null && x.DepartmentId != 0).Select(p => (double)p.DepartmentId).Distinct().ToList<double>();
                            if (deptIds.Count <= 0)
                            {
                                continue;
                            }
                            // 单号
                            var checkNoList = RT.Service.Resolve<CheckController>().GetCheckPlanNoList(shiftCount * deptIds.Count);//一次获取多个单号，避免多次访问数据库
                            int indexNo = 0;
                            shiftType.ShiftList.ForEach(shift =>
                            {
                                CreateCheckPlanShiftWithDept(planList, model, sourceType, equip, shift, deptIds, date, checkNoList, ref indexNo, lockObj);
                            });
                        }
                        else
                        {
                            // 单号
                            var checkNoList = RT.Service.Resolve<CheckController>().GetCheckPlanNoList(shiftCount);//一次获取多个单号，避免多次访问数据库
                            int indexNo = 0;
                            shiftType.ShiftList.ForEach(shift =>
                            {
                                CreateCheckPlanShift(planList, model, sourceType, equip, shift, date, checkNoList, ref indexNo, lockObj);
                            });
                        }
                        
                    }

                });
                if (!planList.Any())
                {
                    throw new ValidationException("没有可生成的点检计划！".L10N());
                }
                if (result.IsCompleted)
                {
                    RF.BatchInsert(planList);
                }
                return planList;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 按责任部门生成点检单
        /// </summary>
        /// <param name="planList">点检单保存列表</param>
        /// <param name="model">点检计划信息</param>
        /// <param name="sourceType">单据来源方式</param>
        /// <param name="equip">设备信息</param>
        /// <param name="shift">班次信息</param>
        /// <param name="deptIds">部门信息</param>
        /// <param name="date">日期</param>
        /// <param name="checkNoList">点检单号</param>
        /// <param name="indexNo">下标</param>
        /// <param name="lockObj">线程锁</param>
        private void CreateCheckPlanShiftWithDept(EntityList<CheckPlan> planList, AddCheckPlanViewModel model, CheckSourceType sourceType, BaseDataInfo equip, Shift shift,
             List<double> deptIds, DateTime date, List<string> checkNoList, ref int indexNo, object lockObj)
        {
            if (deptIds.Any())
            {
                var list = new EntityList<CheckPlan>();
                foreach (var deptId in deptIds)
                {
                    var beginDate = shift.BeginTime;
                    var endDate = shift.EndTime;
                    CheckPlan plan = new CheckPlan();
                    plan.CheckPlanNo = checkNoList[indexNo++];
                    plan.CheckSourceType = sourceType;
                    plan.EquipAccountId = equip.Id;
                    plan.CheckCycleType = CheckCycleType.DayShift;
                    plan.CheckPlanType = CheckPlanType.Shift;
                    plan.WhetherAcrossDay = YesNo.No;
                    plan.ExeState = CheckExeState.NotPerformed;
                    plan.EquipCheckType = model.EquipCheckType;
                    plan.CheckBeginDate = date.Date.AddHours(beginDate.Hour).AddMinutes(beginDate.Minute);
                    var checkEndDate = date.Date.AddHours(endDate.Hour).AddMinutes(endDate.Minute);
                    plan.CheckEndDate = shift.IsOverDay ? checkEndDate.AddDays(1) : checkEndDate;
                    plan.CheckTime = model.CheckTime;
                    plan.DepartmentId = deptId;
                    list.Add(plan);
                }
                lock (lockObj)
                {
                    planList.AddRange(list);
                }
            }
        }

        /// <summary>
        /// 按责任部门生成点检单
        /// </summary>
        /// <param name="planList">点检单保存列表</param>
        /// <param name="model">点检计划信息</param>
        /// <param name="sourceType">单据来源方式</param>
        /// <param name="equip">设备信息</param>
        /// <param name="shift">班次信息</param>
        /// <param name="date">日期</param>
        /// <param name="checkNoList">点检单号</param>
        /// <param name="indexNo">下标</param>
        /// <param name="lockObj">线程锁</param>
        private void CreateCheckPlanShift(EntityList<CheckPlan> planList, AddCheckPlanViewModel model, CheckSourceType sourceType, BaseDataInfo equip, Shift shift,
             DateTime date, List<string> checkNoList, ref int indexNo, object lockObj)
        {
            var beginDate = shift.BeginTime;
            var endDate = shift.EndTime;
            CheckPlan plan = new CheckPlan();
            plan.CheckPlanNo = checkNoList[indexNo++];
            plan.CheckSourceType = sourceType;
            plan.EquipAccountId = equip.Id;
            plan.CheckCycleType = CheckCycleType.DayShift;
            plan.CheckPlanType = CheckPlanType.Shift;
            plan.WhetherAcrossDay = YesNo.No;
            plan.ExeState = CheckExeState.NotPerformed;
            plan.EquipCheckType = model.EquipCheckType;
            plan.CheckBeginDate = date.Date.AddHours(beginDate.Hour).AddMinutes(beginDate.Minute);
            var checkEndDate = date.Date.AddHours(endDate.Hour).AddMinutes(endDate.Minute);
            plan.CheckEndDate = shift.IsOverDay ? checkEndDate.AddDays(1) : checkEndDate;
            plan.CheckTime = model.CheckTime;
            lock (lockObj)
            {
                planList.Add(plan);
            }
        }

        /// <summary>
        /// 添加频次点检计划
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        /// <param name="equipList">设备列表</param>
        /// <param name="sourceType">来源</param>
        public virtual EntityList<CheckPlan> AddCheckPlanForTime(AddCheckPlanViewModel model, List<BaseDataInfo> equipList, CheckSourceType sourceType)
        {
            // 格式化计划区间
            RT.Service.Resolve<CheckPlanController>().RechangeDateTime(model);

            //是否按部门进行点检
            bool isDepartmentCheck = RT.Service.Resolve<CheckController>().IsDepartmentCheck();

            //获取点检频次(小时)
            int time = RT.Service.Resolve<CheckController>().GetCheckFrequency();

            //获取周方案
            CalendarScheme calendarScheme = RT.Service.Resolve<CheckController>().GetCalendarScheme();

            if (calendarScheme == null)
            {
                throw new ValidationException(NotConfigCalendarSchema.L10N());
            }

            var dicInfo = RT.Service.Resolve<CalendarSchemeController>()
                .GetShiftTypesByCalendarSchemeAndDataRange(calendarScheme, model.BeginDate.Date, model.EndDate.Date);

            var holidays = RT.Service.Resolve<HolidayController>().GetIntersectHoliday(model.BeginDate, model.EndDate);

            //用于保存新点检计划的集合
            EntityList<CheckPlan> planList = new EntityList<CheckPlan>();

            //查询日期范围内已生成的点检计划
            var equipAccountIds = equipList.Select(x => x.Id).Distinct().ToList();
            EntityList<CheckPlan> checkPlans = GetExeCheckPlanList(equipAccountIds, CheckPlanType.Shift, model.BeginDate, model.EndDate);

            //获取所有设备的点检项目
            var checkProjectsOfAccounts = RT.Service.Resolve<EquipController>().GetCheckProjectsOfAccounts(equipAccountIds, CheckPlanType.Shift);

            ParallelOptions paraOP = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
            object lockObj = new object();//线程锁

            ParallelLoopResult result = Parallel.ForEach<BaseDataInfo>(equipList, paraOP, equip =>
            {
                //查询该台设备生成过的计划
                List<CheckPlan> plans = checkPlans.Where(p => p.EquipAccountId == equip.Id).ToList();

                for (System.DateTime date = model.BeginDate; date.Date <= model.EndDate.Date; date = date.AddDays(1))
                {
                    //根据日历方案算出点检计划白班的开始时间和结束时间
                    if (!dicInfo.ContainsKey(date))
                    {
                        continue;
                    }

                    //法定假日
                    if (holidays.Any(x => date >= x.BeginDate && date <= x.EndDate))
                    {
                        continue;
                    }

                    //查询该台设备生成过的计划
                    if (plans.Any(p => p.CheckBeginDate.Date == date.Date))
                    {
                        continue;
                    }

                    var shiftType = dicInfo[date];

                    if (shiftType.ShiftList == null || !shiftType.ShiftList.Any())
                    {
                        throw new ValidationException(ShiftError.L10N());
                    }

                    var beginTime = shiftType.ShiftList.Min(p => p.BeginTime);
                    var endTime = shiftType.ShiftList.Max(p => p.EndTime);

                    //当天的开始时间
                    var beginDate = date.AddHours(beginTime.Hour).AddMinutes(beginTime.Minute);
                    var endDate = date.AddHours(endTime.Hour).AddMinutes(endTime.Minute);

                    //计算一天内所有点检开始时间
                    List<DateTime> dateTimeList = new List<DateTime>();

                    for (DateTime dateTime = beginDate; dateTime <= endDate;)
                    {
                        dateTimeList.Add(dateTime);
                        dateTime = dateTime.AddHours((double)time);
                    }

                    if (!dateTimeList.Any())
                    {
                        continue;
                    }
                    var dateTimeCount = dateTimeList.Count;

                    if (isDepartmentCheck)
                    {
                        // 责任部门ids
                        var deptIds = checkProjectsOfAccounts.Where(x => x.EquipAccountId == equip.Id && x.DepartmentId != null && x.DepartmentId != 0).Select(p => (double)p.DepartmentId).Distinct().ToList<double>();
                        if (deptIds.Count <= 0)
                        {
                            continue;
                        }
                        // 单号
                        var checkNoList = RT.Service.Resolve<CheckController>().GetCheckPlanNoList(dateTimeCount * deptIds.Count);//一次获取多个单号，避免多次访问数据库
                        int indexNo = 0;

                        dateTimeList.ForEach(dateTime =>
                        {
                            CreateCheckPlanSingleTimeWithDept(planList, model, sourceType, equip, dateTime, time, deptIds, checkNoList, ref indexNo, lockObj);
                        });
                    }
                    else
                    {
                        // 单号
                        var checkNoList = RT.Service.Resolve<CheckController>().GetCheckPlanNoList(dateTimeCount);
                        int indexNo = 0;
                        dateTimeList.ForEach(dateTime =>
                        {
                            CreateCheckPlanSingleTime(planList, model, sourceType, equip, dateTime, time, checkNoList, ref indexNo, lockObj);
                        });
                    }
                }

            });

            if (!planList.Any())
            {
                throw new ValidationException("没有可生成的点检计划！".L10N());
            }

            if (result.IsCompleted)
            {
                RF.Save(planList);
            }

            return planList;

        }

        /// <summary>
        /// 单个频次的点检计划
        /// </summary>
        /// <param name="planList">点检计划保存列表</param>
        /// <param name="model">点检信息</param>
        /// <param name="sourceType">数据来源</param>
        /// <param name="equip">设备信息</param>
        /// <param name="dateTime">当天</param>
        /// <param name="time">频次时间</param>
        /// <param name="deptIds">责任部门</param>
        /// <param name="checkNoList">点检单号</param>
        /// <param name="indexNo">下标</param>
        /// <param name="lockObj">线程锁</param>
        private void CreateCheckPlanSingleTimeWithDept(EntityList<CheckPlan> planList, AddCheckPlanViewModel model, CheckSourceType sourceType, BaseDataInfo equip,
            DateTime dateTime, int time, List<double> deptIds, List<string> checkNoList, ref int indexNo, object lockObj)
        {
            if (deptIds.Any())
            {
                var list = new EntityList<CheckPlan>();
                foreach (var deptId in deptIds)
                {
                    CheckPlan plan = new CheckPlan();
                    plan.CheckPlanNo = checkNoList[indexNo++];
                    plan.CheckSourceType = sourceType;
                    plan.EquipAccountId = equip.Id;
                    plan.CheckCycleType = CheckCycleType.NightShift;//NightShift旧枚举是晚班，新枚举代表的是频次
                    plan.CheckPlanType = CheckPlanType.Time;
                    plan.WhetherAcrossDay = YesNo.No;
                    plan.ExeState = CheckExeState.NotPerformed;
                    plan.EquipCheckType = model.EquipCheckType;
                    plan.CheckBeginDate = dateTime;
                    plan.CheckEndDate = dateTime.AddHours((double)time);
                    plan.CheckTime = model.CheckTime;
                    plan.DepartmentId = deptId;
                    list.Add(plan);
                }
                lock (lockObj)
                {
                    planList.AddRange(list);
                }
            }
        }

        /// <summary>
        /// 单个频次的点检计划
        /// </summary>
        /// <param name="planList">点检计划保存列表</param>
        /// <param name="model">点检信息</param>
        /// <param name="sourceType">数据来源</param>
        /// <param name="equip">设备信息</param>
        /// <param name="dateTime">当天</param>
        /// <param name="time">频次时间</param>
        /// <param name="deptIds">责任部门</param>
        /// <param name="checkNoList">点检单号</param>
        /// <param name="indexNo">下标</param>
        /// <param name="lockObj">线程锁</param>
        private void CreateCheckPlanSingleTime(EntityList<CheckPlan> planList, AddCheckPlanViewModel model, CheckSourceType sourceType, BaseDataInfo equip,
            DateTime dateTime, int time, List<string> checkNoList, ref int indexNo, object lockObj)
        {
            CheckPlan plan = new CheckPlan();
            plan.CheckPlanNo = checkNoList[indexNo++];
            plan.CheckSourceType = sourceType;
            plan.EquipAccountId = equip.Id;
            plan.CheckCycleType = CheckCycleType.NightShift;//NightShift旧枚举是晚班，新枚举代表的是频次
            plan.CheckPlanType = CheckPlanType.Time;
            plan.WhetherAcrossDay = YesNo.No;
            plan.ExeState = CheckExeState.NotPerformed;
            plan.EquipCheckType = model.EquipCheckType;
            plan.CheckBeginDate = dateTime;
            plan.CheckEndDate = dateTime.AddHours((double)time);
            plan.CheckTime = model.CheckTime;
            lock (lockObj)
            {
                planList.Add(plan);
            }
        }

        /// <summary>
        /// 生成点检计划点检项目
        /// </summary>
        /// <param name="checkPlanId"></param>
        public virtual void GeneratePlanProject(double checkPlanId)
        {
            //行锁数据，防止并发
            DB.Lock<CheckPlan>(checkPlanId);

            //校验单据是否未执行状态，如果不是未执行，退出生成点检项目逻辑                
            var checkPlan = Query<CheckPlan>()
                .Where(p => p.Id == checkPlanId && p.ExeState == CheckExeState.NotPerformed)
                .FirstOrDefault();

            if (checkPlan == null)
            {
                return;
            }

            // 清空原有的点检单点检项目
            var oldCheckPlanProjects = Query<CheckProject>().Where(p => p.CheckPlanId == checkPlanId).ToList();
            oldCheckPlanProjects.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
            });

            // 重新按照设备台账当前的点检项目生成
            EntityList<CheckProject> newCheckPlanProjects = new EntityList<CheckProject>();

            //是否带出设备子项的检验项目
            var isChildProject = RT.Service.Resolve<CheckController>().IsBringChildCheckProject();

            //获取设备及子设备ID
            var equipIds = new List<double>();
            if (isChildProject)
            {
                RT.Service.Resolve<EquipController>().GetEquipAccountTreeUnderIds(checkPlan.EquipAccountId, equipIds);
            }
            else
            {
                equipIds.Add(checkPlan.EquipAccountId);
            }

            if (equipIds.Count <= 0)
            {
                throw new ValidationException("数据异常，设备不能为空".L10N());
            }

            //获取设备点检项目
            var checkProjects = this.GetEquipAccountCheckProjects(checkPlan, equipIds);

            //获取设备物联项目
            var physicalUnions = Query<EquipAccountPhysicalUnion>()
                .Where(p => equipIds.Contains(p.EquipAccountId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //需求有排序要求，主设备排最前，最大最小值不为空排前面; 根据上面ID列表递归逻辑，主ID一定最前
            equipIds.ForEach(p =>
            {
                var equipCheckProjects = checkProjects
                    .Where(x => x.EquipAccountId == p)
                    .OrderBy(x => x.MinValue)
                    .ThenBy(x => x.MaxValue)
                    .ToList();

                //生成点检项目
                foreach (var checkProject in equipCheckProjects)
                {
                    GenerateCheckProject(checkPlan, checkProject, newCheckPlanProjects);
                }


                var equipPhysicalUnions = physicalUnions
                    .Where(x => x.EquipAccountId == p)
                    .OrderBy(x => x.MinValue)
                    .ThenBy(x => x.MaxValue)
                    .ToList();

                //生成设备物联项目
                foreach (var physicalUnion in equipPhysicalUnions)
                {
                    GenerateCheckProject(checkPlan, physicalUnion, newCheckPlanProjects);
                }
            });

            if (newCheckPlanProjects.Count == 0)
            {
                throw new ValidationException("该设备不存在[{0}]点检项目，请联系设备管理员！".L10nFormat(checkPlan.CheckCycleType.ToLabel().L10N()));
            }

            using(var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(oldCheckPlanProjects);
                RF.Save(newCheckPlanProjects);
                tran.Complete();
            }
            
        }

        /// <summary>
        /// 根据点检单获取设备点检项目
        /// </summary>
        /// <param name="checkPlan"></param>
        /// <param name="equipIds"></param>
        /// <returns></returns>
        private EntityList<EquipAccountCheckProject> GetEquipAccountCheckProjects(CheckPlan checkPlan, List<double> equipIds)
        {
            //根据日历方案算出点检计划白班的开始时间和结束时间
            EntityList<Shift> shiftList = GetShiftList();
            if (shiftList == null || !shiftList.Any())
            {
                throw new ValidationException(ShiftError.L10N());
            }

            var firstShift = shiftList.OrderBy(p => p.BeginTime).FirstOrDefault();

            //获取设备台账点检项目
            var projectQuery = Query<EquipAccountCheckProject>();
            projectQuery.Where(p => p.ProjectType == ProjectType.Check);
            projectQuery.Where(p => equipIds.Contains(p.EquipAccountId));
            // 获取对应部门的点检项目+部门为空的点检项目
            projectQuery.WhereIf(checkPlan.DepartmentId != null && checkPlan.DepartmentId != 0, p => p.DepartmentId == checkPlan.DepartmentId || p.DepartmentId == null);
            

            switch (checkPlan.CheckPlanType)
            {
                case CheckPlanType.Day:
                    {
                        projectQuery.Where(p => p.CycleType == CycleType.Day);
                        break;
                    }
                case CheckPlanType.Shift:
                    {
                        //第一班，查询班和日点检项目，其余查询班点检
                        if (firstShift.BeginTime <= DateTime.Now && firstShift.EndTime >= DateTime.Now)
                        {
                            projectQuery.Where(p => p.CycleType == CycleType.Day || p.CycleType == CycleType.Class);
                        }
                        else
                        {
                            projectQuery.Where(p => p.CycleType == CycleType.Class);
                        }
                        break;
                    }
                case CheckPlanType.Time:
                    {
                        //获取点检频次(小时)
                        int? time = RT.Service.Resolve<CheckController>().GetCheckFrequency();
                        if (time == null)
                        {
                            throw new ValidationException("点检频次为空，请配置".L10N());
                        }
                        //第一频次开始结束时间
                        var beginDate = DateTime.Now.Date.AddHours(firstShift.BeginTime.Hour).AddMinutes(firstShift.BeginTime.Minute);
                        var endDate = beginDate.AddHours((int)time);

                        //当前为第一频次查询日和班点检，其余查班点检
                        if (beginDate <= DateTime.Now && endDate >= DateTime.Now)
                        {
                            projectQuery.Where(p => p.CycleType == CycleType.Day || p.CycleType == CycleType.Class);
                        }
                        else
                        {
                            projectQuery.Where(p => p.CycleType == CycleType.Class);
                        }
                        break;
                    }
                default:
                    break;
            }
            var checkProjects = projectQuery.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return checkProjects;
        }

        #endregion

        #region 添加点检计划(旧)
        ///// <summary>
        ///// 添加点检计划
        ///// </summary>
        ///// <param name="checkPlanList">点检计划列表</param>
        ///// <param name="uiCheckProjectList">界面设备台帐点检项目列表</param>
        //public virtual void AddCheckPlan(EntityList<CheckPlan> checkPlanList, EntityList<EquipAccountCheckProject> uiCheckProjectList)
        //{
        //    try
        //    {
        //        //验证点检计划和点检计划项目是否合法
        //        var planList = ValidateCheckPlanRelateInfo(checkPlanList, uiCheckProjectList);
        //        //获取即将保存的设备台账点检项目
        //        var toSaveCheckProjects = GetToSaveCheckProjects(uiCheckProjectList);
        //        //保存
        //        SaveCheckPlanRelateInfo(toSaveCheckProjects, planList);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ValidationException(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 验证点检计划和点检计划项目是否合法
        ///// </summary>
        ///// <param name="checkPlanList">点检计划列表</param>
        ///// <param name="uiCheckProjectList">界面设备台帐点检项目列表</param>
        ///// <returns>点检计划列表</returns>
        //private EntityList<CheckPlan> ValidateCheckPlanRelateInfo(EntityList<CheckPlan> checkPlanList, EntityList<EquipAccountCheckProject> uiCheckProjectList)
        //{
        //    var firstCheckPlan = checkPlanList.FirstOrDefault();
        //    if (firstCheckPlan == null)
        //        throw new ValidationException("计划明细至少维护一笔数据!".L10N());
        //    var accountId = firstCheckPlan.EquipAccountId;
        //    var checkCycleType = firstCheckPlan.CheckCycleType;
        //    var minCheckDate = checkPlanList.Min(p => p.CheckDate);
        //    var maxCheckDate = checkPlanList.Max(p => p.CheckDate);
        //    uiCheckProjectList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Unchanged);

        //    //检查设备是否存在点检任务
        //    var plan = RT.Service.Resolve<CheckPlanController>().GetCheckPlanByCheckDate(accountId, minCheckDate, maxCheckDate, checkCycleType);
        //    if (plan != null)
        //    {
        //        throw new ValidationException("已存在该设备{0}、{1}的点检计划!".L10nFormat(plan.CheckDate.ToString("yyyy/MM/dd"), EnumViewModel.EnumToLabel(plan.CheckCycleType)));
        //    }

        //    var planList = GetCheckPlanList(checkPlanList, uiCheckProjectList);
        //    if (planList.Count <= 0)
        //    {
        //        throw new ValidationException("点检任务没有符合条件的项目!".L10N());
        //    }

        //    return planList;
        //}

        ///// <summary>
        ///// 获取点检计划列表
        ///// </summary>
        ///// <param name="checkPlanList">界面的点检计划列表</param>
        ///// <param name="uiCheckProjectList">界面设备台账点检项目列表</param>
        ///// <returns>点检计划列表</returns>
        //private EntityList<CheckPlan> GetCheckPlanList(EntityList<CheckPlan> checkPlanList, EntityList<EquipAccountCheckProject> uiCheckProjectList)
        //{
        //    var planList = new EntityList<CheckPlan>();
        //    foreach (var checkPlan in checkPlanList)
        //    {
        //        //循环点检项目 周期类型为班
        //        if (checkPlan.CheckCycleType != CheckCycleType.Day)
        //            GenerateCheckProjectsWithShift(uiCheckProjectList, checkPlan);
        //        else
        //            GenerateCheckProjects(uiCheckProjectList, checkPlan);
        //        planList.AddRange(CreateCheckPlanByCheckPrjs(checkPlan));
        //    }

        //    return planList;
        //}

        ///// <summary>
        ///// 创建点检计划
        ///// </summary>
        ///// <param name="checkPlan">界面的点检计划</param>
        ///// <returns>点检计划</returns>
        //private EntityList<CheckPlan> CreateCheckPlanByCheckPrjs(CheckPlan checkPlan)
        //{
        //    var planList = new EntityList<CheckPlan>();
        //    //有项目才保存
        //    if (checkPlan.CheckProjectList.Count > 0)
        //    {
        //        checkPlan.GenerateId();
        //        checkPlan.CheckPlanNo = RT.Service.Resolve<CheckController>().GetCheckPlanNo();
        //        planList.Add(checkPlan);
        //    }

        //    return planList;
        //}

        ///// <summary>
        ///// 生成点检计划的点检项目列表
        ///// </summary>
        ///// <param name="uiCheckProjectList">界面设备台账点检项目列表</param>
        ///// <param name="checkPlan">点检计划</param>
        //private void GenerateCheckProjects(IEnumerable<EquipAccountCheckProject> uiCheckProjectList, CheckPlan checkPlan)
        //{
        //    foreach (var uiCheckProject in uiCheckProjectList)
        //    {
        //        if (uiCheckProject.LastCheckDate == null)
        //            GenerateCheckProject(checkPlan, uiCheckProject);
        //        else
        //            GenerateCheckProjectWithLastCheckDate(checkPlan, uiCheckProject);
        //    }
        //}

        ///// <summary>
        ///// 生成点检计划的点检项目(当设备台账点检项目的最后点检时间不为空时）
        ///// </summary>
        ///// <param name="checkPlan">点检计划</param>
        ///// <param name="uiCheckProject">界面设备台账点检项目</param>
        //private void GenerateCheckProjectWithLastCheckDate(CheckPlan checkPlan, EquipAccountCheckProject uiCheckProject)
        //{
        //    DateTime nextMDate = uiCheckProject.LastCheckDate.Value.AddDays(1);
        //    if (checkPlan.CheckCycleType == CheckCycleType.Day)
        //        nextMDate = uiCheckProject.LastCheckDate.Value.AddDays((int)uiCheckProject.ProjectCycle);
        //    if (nextMDate <= checkPlan.CheckDate)
        //        GenerateCheckProject(checkPlan, uiCheckProject);
        //}

        /// <summary>
        /// 生成点检计划的点检项目(当设备台账点检项目的最后点检时间为空时）
        /// </summary>
        /// <param name="checkPlan">点检计划</param>
        /// <param name="uiCheckProject">界面设备台账点检项目</param>
        /// <param name="newCheckPlanProjects">新生成点检项目</param>
        private void GenerateCheckProject(CheckPlan checkPlan, EquipAccountCheckProject uiCheckProject, EntityList<CheckProject> newCheckPlanProjects)
        {
            uiCheckProject.LastCheckDate = checkPlan.CheckBeginDate.Date;
            var checkProject = CreatCheckProject(uiCheckProject, checkPlan);
            newCheckPlanProjects.Add(checkProject);
        }


        /// <summary>
        /// 生成点检计划的点检项目-设备物联参数
        /// </summary>
        /// <param name="checkPlan">点检计划</param>
        /// <param name="equipPhysicalUnion">设备物联参数</param>
        /// <param name="newCheckPlanProjects">新生成点检项目</param>
        private void GenerateCheckProject(CheckPlan checkPlan, EquipAccountPhysicalUnion equipPhysicalUnion, EntityList<CheckProject> newCheckPlanProjects)
        {
            var checkProject = new CheckProject();
            checkProject.CheckPlan = checkPlan;
            checkProject.EquipAccountId = equipPhysicalUnion.EquipAccountId;
            checkProject.EquipPhysicalUnionId = equipPhysicalUnion.Id;

            //设备点检项目赋值点检计划项
            checkProject.ParaCode = equipPhysicalUnion.PhysicalUnion?.PararCode;
            checkProject.ParaName = equipPhysicalUnion.PhysicalUnion?.ParaName;
            checkProject.MinValue = (decimal?)equipPhysicalUnion.PhysicalUnion?.MinValue;
            checkProject.MaxValue = (decimal?)equipPhysicalUnion.PhysicalUnion?.MaxValue;
            checkProject.Unit = equipPhysicalUnion.PhysicalUnion?.Unit;
            checkProject.EquipParamSource = equipPhysicalUnion.EquipPara == EquipPara.AutomaticValue ? Enums.EquipParamSource.AutomaticValue : Enums.EquipParamSource.ManualValue;
            checkProject.ProjectType = ProjectType.Check;
            checkProject.CycleType = checkPlan.CheckCycleType == CheckCycleType.Day ? CycleType.Day : CycleType.Class;

            checkProject.CheckResult = null;
            newCheckPlanProjects.Add(checkProject);
        }

        ///// <summary>
        ///// 生成点检计划的点检项目列表(当点检周期类型为班时)
        ///// </summary>
        ///// <param name="checkPrjsOfAccount">设备台账点检项目列表</param>
        ///// <param name="checkPlan">点检计划</param>
        //private void GenerateCheckProjectsWithShift(IEnumerable<EquipAccountCheckProject> checkPrjsOfAccount, CheckPlan checkPlan)
        //{
        //    foreach (var checkPrjOfAccount in checkPrjsOfAccount)
        //    {
        //        var checkPrj = CreatCheckProject(checkPrjOfAccount, checkPlan);
        //        checkPlan.CheckProjectList.Add(checkPrj);
        //    }
        //}

        /// <summary>
        /// 创建点检项目
        /// </summary>
        /// <param name="uiCheckProject">界面设备台账点检项目</param>
        /// <param name="checkPlan">点检计划</param>
        /// <returns>点检项目</returns>
        protected virtual CheckProject CreatCheckProject(EquipAccountCheckProject uiCheckProject, CheckPlan checkPlan)
        {
            if (uiCheckProject == null)
            {
                return new CheckProject();
            }
            var checkProject = new CheckProject();
            checkProject.CheckPlan = checkPlan;
            checkProject.EquipAccountId = uiCheckProject.EquipAccountId;
            checkProject.EquipCheckProjectId = uiCheckProject.Id;
            checkProject.EquipParamSource = Enums.EquipParamSource.NoValue;

            //设备点检项目赋值点检计划项
            checkProject.ProjectName = uiCheckProject.ProjectName;
            checkProject.Part = uiCheckProject.Part;
            checkProject.ProjectConsumable = uiCheckProject.Consumable;
            checkProject.Method = uiCheckProject.Method;
            checkProject.Standard = uiCheckProject.Standard;
            checkProject.MinValue = uiCheckProject.MinValue;
            checkProject.MaxValue = uiCheckProject.MaxValue;
            checkProject.Unit = uiCheckProject.Unit;
            checkProject.UseTime = uiCheckProject.UseTime;
            checkProject.ProjectType = uiCheckProject.ProjectType;
            checkProject.CycleType = uiCheckProject.CycleType;
            checkProject.CheckResult = null;
            return checkProject;
        }

        #endregion

        #region 查询 Get

        /// <summary>
        /// 获取点检计划
        /// </summary>
        /// <param name="idsList"></param>
        /// <returns></returns>
        public virtual EntityList<CheckPlan> GetCheckPlanList(List<double> idsList)
        {
            return idsList.SplitContains((ids) =>
            {
                return Query<CheckPlan>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 添加点检计划重复校验
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        /// <param name="equipList">设备列表</param>
        public virtual AddCheckPlanResultInfo AddCheckPlanToVerifyRepeat(AddCheckPlanViewModel model, List<BaseDataInfo> equipList)
        {
            try
            {
                RT.Service.Resolve<CheckPlanController>().RechangeDateTime(model);// 格式化计划区间
                AddCheckPlanResultInfo retInfo = new AddCheckPlanResultInfo();
                List<string> strList = new List<string>();
                // 计划开始日期
                var planBeginDate = model.BeginDate.Date;

                // 计划结束日期
                var planEndDate = model.EndDate.Date;

                //查询日期范围内已生成的点检计划
                EntityList<CheckPlan> checkPlans = Query<CheckPlan>().Where(p => p.CheckBeginDate >= model.BeginDate && p.CheckBeginDate <= model.EndDate).ToList();
                equipList.ForEach(equip =>
                {
                    //查询该台设备生成过的计划
                    List<CheckPlan> plans = checkPlans.Where(p => p.EquipAccountId == equip.Id).ToList();
                    for (DateTime date = planBeginDate; date <= planEndDate; date = date.AddDays(1))
                    {
                        if (plans.Any(p => p.CheckBeginDate.Date == date.Date))
                        {
                            strList.Add("设备[{1}]在[{0}]已生成过点检计划".L10nFormat(date.Date.ToString("yyyy/MM/dd"), equip.Code));
                        }
                    }
                });
                retInfo.ErrMsg = strList.Any() ? string.Join("<br>", strList) : "";
                return retInfo;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 获取点检计划主表列信息
        /// </summary>
        /// <returns>点检计划主表列信息</returns>
        public virtual List<CheckPlanColumn> GetCheckPlanColumns(DateTime date)
        {
            //获取点检类型(日、班、频次)
            CheckPlanType checkType = RT.Service.Resolve<CheckController>().GetCheckPlanType();

            int time = 0;
            if (checkType == CheckPlanType.Time)
            {
                //获取点检频次(小时)
                time = RT.Service.Resolve<CheckController>().GetCheckFrequency();
            }

            //根据日历方案算出点检计划的开始时间
            EntityList<Shift> shiftList = GetShiftList();
            if (shiftList == null || !shiftList.Any())
            {
                throw new ValidationException(ShiftError.L10N());
            }

            var beginDate = shiftList.Min(p => p.BeginTime);
            var ShiftList = shiftList.OrderBy(p => p.BeginTime);

            //当天的开始时间
            beginDate = DateTime.Now.Date.AddHours(beginDate.Hour).AddMinutes(beginDate.Minute);
            //当天的结束时间
            DateTime endDate = DateTime.Now.Date.AddDays(1);

            int days = DateTime.DaysInMonth(date.Year, date.Month);
            List<CheckPlanColumn> columnList = new List<CheckPlanColumn>();
            for (int i = 1; i <= days; i++)
            {
                CheckPlanColumn column = new CheckPlanColumn();
                column.DayNum = i;
                if (checkType == CheckPlanType.Shift)
                {
                    ShiftList.ForEach(shift =>
                    {
                        CheckPlanColumn newColumn = new CheckPlanColumn();
                        newColumn.DayNum = i;
                        newColumn.BeginTime = shift.BeginTime.ToString("HH:mm");
                        newColumn.EndTime = shift.EndTime.ToString("HH:mm");
                        newColumn.ShiftName = shift.Name;
                        columnList.Add(newColumn);
                    });
                }
                else if (checkType == CheckPlanType.Time)
                {
                    for (DateTime dateTime = beginDate; dateTime <= endDate;)
                    {
                        CheckPlanColumn newColumn = new CheckPlanColumn();
                        newColumn.DayNum = i;
                        newColumn.BeginTime = dateTime.ToString("HH:mm");
                        dateTime = dateTime.AddHours((double)time);
                        newColumn.EndTime = dateTime.ToString("HH:mm");
                        if (dateTime <= endDate)
                        {
                            columnList.Add(newColumn);
                        }
                    }
                }
                else
                {
                    columnList.Add(column);
                }
            }
            return columnList;
        }

        /// <summary>
        /// 获取相关设备的点检计划列表(新)
        /// </summary>
        /// <param name="criteria">点检计划查询实体</param>
        /// <returns>点检计划返回信息</returns>
        public virtual EntityList<CheckPlanViewModel> GetEquipCheckPlans(CheckPlanCriteria criteria)
        {
            EntityList<CheckPlanViewModel> records = null;
            var equipCt = RT.Service.Resolve<EquipController>();
            var checkCt = RT.Service.Resolve<CheckController>();
            var accounts = equipCt.CriteriaEquipForCheckPlans(criteria);
            var accountIds = accounts.Select(p => p.Id).ToList();
            var checkPlans = checkCt.GetCheckPlanListByAccountIds(criteria, accountIds);
            var dicCheckPlans = checkPlans.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());
            records = CreateCheckPlanMainInfos(accounts, dicCheckPlans, criteria);

            records.SetTotalCount(accounts.TotalCount);
            return records;
        }

        /// <summary>
        /// 创建点检计划主信息列表（新）
        /// </summary>
        /// <param name="accounts">设备台账列表</param>
        /// <param name="dicCheckPlans">点检计划字典</param>
        ///  <param name="criteria"></param>
        /// <returns>点检计划主信息列表</returns>
        private EntityList<CheckPlanViewModel> CreateCheckPlanMainInfos(EntityList<EquipAccountSelect> accounts, Dictionary<double, List<CheckPlan>> dicCheckPlans, CheckPlanCriteria criteria)
        {
            EntityList<CheckPlanViewModel> records = new EntityList<CheckPlanViewModel>();

            foreach (var account in accounts)
            {
                var record = new CheckPlanViewModel();

                record.Id = Guid.NewGuid().ToString("N").ToUpper();
                record.YearAndMonth = criteria.Month;
                record.EquipAccountId = account.Id;
                record.EquipAccountCode = account.Code;
                record.EquipAccountName = account.Name;
                record.EquipModelName = account.EquipModel?.Name;
                record.EquipTypeName = account.EquipModel?.EquipType?.TypeName;
                record.UseState = account.UseState;
                record.WorkShopName = account.WorkShop?.Name;
                record.ResourceName = account.Resource?.Name;
                record.ProcessName = account.Process?.Name;

                List<CheckPlan> plans = null;
                List<string> ColumnNames = new List<string>();
                if (dicCheckPlans.TryGetValue(account.Id, out plans))
                {
                    List<CheckPlanColumn> columns = new List<CheckPlanColumn>();

                    plans.ForEach((Action<CheckPlan>)(plan =>
                    {
                        var list = plans.Where(p => p.CheckBeginDate == plan.CheckBeginDate).ToList();

                        CheckPlanColumn column = new CheckPlanColumn();
                        column.DayNum = plan.CheckBeginDate.Day;
                        column.BeginTime = plan.CheckPlanType == CheckPlanType.Day ? "" : ("<br>(" + plan.CheckBeginDate.ToString("HH:mm") + "-");
                        column.ColumnName = column.DayNum + column.BeginTime;
                        if (!ColumnNames.Contains(column.ColumnName))
                        {
                            ColumnNames.Add(column.ColumnName);
                            //执行结果
                            if (list.All(p => p.ExeResult == ExeResult.Successed))
                            {
                                column.ExeResult = "OK";
                            }
                            else if (list.Any(p => p.ExeResult == ExeResult.Failed))
                            {
                                column.ExeResult = "NG";
                            }
                            else
                            {
                                column.ExeResult = "";
                            }
                            //执行状态
                            if (list.Any(p => p.ExeState == CheckExeState.NotPerformed))
                            {
                                column.ExeState = plan.CheckEndDate < DateTime.Now ? 2 : 0;//2：超期，0：未执行
                            }
                            else if (list.Any(p => p.ExeState == CheckExeState.Performing))
                            {
                                column.ExeState = plan.CheckEndDate < DateTime.Now ? 2 : 4;//2：超期，4：执行中
                            }
                            else if (list.Any(p => p.ExeState == CheckExeState.NotConfirm))
                            {
                                column.ExeState = 5;//待确认
                            }
                            else
                            {
                                column.ExeState = 1;//已执行(已评分)
                            }

                            columns.Add(column);
                        }
                    }));

                    record.DataJsonString = JsonConvert.SerializeObject(columns);
                }
                else
                {
                    List<CheckPlanColumn> columns = new List<CheckPlanColumn>();
                    record.DataJsonString = JsonConvert.SerializeObject(columns);
                }
                records.Add(record);
                ColumnNames.Clear();
            }
            return records;
        }

        /// <summary>
        /// 获取点检TPM管理信息
        /// </summary>
        /// <param name="equip"></param>
        /// <returns></returns>
        public virtual EntityList<TpmViewModel> GetCheckPlanTpmViewModel(double equip)
        {
            var now = RF.Find<CheckPlan>().GetDbTime();
            var beginDateDay = now.Date;

            var executedTime = Query<CheckPlan>()
                .Where(p => (p.ExeState == CheckExeState.Performed || p.ExeState == CheckExeState.Scored || p.ExeState == CheckExeState.Confirmed) && p.EquipAccountId == equip)
                .OrderByDescending(p => p.CheckBeginDate)
                .Select(p => p.CheckBeginDate)
                .ToList(new PagingInfo(1, 1)).FirstOrDefault()?.CheckBeginDate;

            var toBeExecutTimes = Query<CheckPlan>()
                .Where(p => (p.ExeState == CheckExeState.NotPerformed) && p.EquipAccountId == equip)
                .Where(p => p.CheckBeginDate >= beginDateDay)
                .OrderBy(p => p.CheckBeginDate)
                .Select(p => p.CheckBeginDate)
                .ToList<DateTime>(new PagingInfo(1, 2));

            var vms = new EntityList<TpmViewModel>();
            var vm = new TpmViewModel();
            if (executedTime != null)
            {
                vm.LastExecuteTime = executedTime;
            }
            if (toBeExecutTimes.Count > 0)
            {
                vm.CurrentToBeExecuteTime = toBeExecutTimes.OrderBy(p => p).FirstOrDefault();
                if (toBeExecutTimes.Count > 1)
                {
                    vm.NextToBeExecuteTime = toBeExecutTimes.OrderBy(p => p).LastOrDefault();
                }
            }
            if (vm.LastExecuteTime != null || vm.CurrentToBeExecuteTime != null || vm.NextToBeExecuteTime != null)
            {
                vms.Add(vm);
            }
            return vms;
        }

        #endregion

        #region 点检执行

        /// <summary>
        /// 获取点击执行数据
        /// </summary>
        /// <param name="equipIds">设备Ids</param>
        /// <param name="checkPlanType">点检计划类型</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="beforeNow">当前时间以前</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlan> GetExeCheckPlanList(List<double> equipIds, CheckPlanType checkPlanType, DateTime beginDate, DateTime endDate, bool beforeNow = false)
        {
            //点检单查询器
            var queryer = Query<CheckPlan>();

            // 未到的日期不能执行
            var nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            queryer.Where(p => p.CheckBeginDate >= beginDate && p.CheckBeginDate < endDate);
            if (checkPlanType == CheckPlanType.Day)
            {
                queryer.Where(p => p.CheckCycleType == CheckCycleType.Day);
            }
            else
            {
                queryer.Where(p => p.CheckCycleType != CheckCycleType.Day);
            }

            // 是否允许获取未到执行时间的
            if (beforeNow)
            {
                queryer.Where(p => p.CheckBeginDate < nowDate);
            }

            //贪懒加载评分项
            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            return equipIds.SplitContains(tempIds =>
            {
                return queryer.Where(p => tempIds.Contains(p.EquipAccountId)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据条件获取点检计划
        /// </summary>
        /// <param name="equipId">设备台账Id</param>
        /// <param name="checkPlanType">点检周期类型</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>点检计划</returns>
        public virtual IEnumerable<CheckPlanInfos> GetCheckPlans(double equipId, CheckPlanType checkPlanType, DateTime beginDate, DateTime endDate)
        {
            // 上传至前端因为格式问题都转成24小时计时
            beginDate = DateTime.Parse(beginDate.ToString("yyyy-MM-dd HH:mm:ss"));
            endDate = DateTime.Parse(endDate.ToString("yyyy-MM-dd HH:mm:ss"));
            if (endDate <= beginDate) // 跨日班组
            {
                endDate = endDate.AddDays(1);
            }
            var list = new List<CheckPlanInfos>();

            //点检单列表
            var checkPlanList = GetExeCheckPlanList(new List<double> { equipId }, checkPlanType, beginDate, endDate, true);
            var checkPlanIds = checkPlanList.Select(p => p.Id).ToList();
            if (checkPlanIds.Count <= 0)
            {
                throw new ValidationException("当前日期不存在点检单或点检单没到执行时间".L10N());
            }

            // 当前登陆人权限责任部门
            var nowDepts = GetUserDepts();

            // 当前登陆人权限
            var nowDevpurs = GetCheckConfirmUser();

            // 点检确认项
            var checkConfirmItems = GetCheckPlanConfirmItemByPlanIds(checkPlanIds, true);

            foreach (var checkPlan in checkPlanList)
            {
                if (checkPlan.ExeState != CheckExeState.NotConfirm && checkPlan.ExeState != CheckExeState.Scored && checkPlan.ExeState != CheckExeState.Confirmed) // 点检执行数据
                {
                    if (checkPlan.DepartmentId != null && nowDepts.FirstOrDefault(p => p.EnterpriseId == checkPlan.DepartmentId) == null) continue;
                    GetExecuteInfo(list, checkPlan);
                }
                else // 点检确认数据
                {
                    var planConfirmItems = checkConfirmItems.Where(p => p.CheckPlanId == checkPlan.Id).AsEntityList();

                    GetConfirmInfo(list, checkPlan, planConfirmItems, nowDevpurs, nowDepts);
                }
            }
            return list;

        }


        /// <summary>
        /// 根据计划id获取点检确认项目
        /// </summary>
        /// <param name="planId">计划id</param>
        /// <param name="loadView">是否加载视图属性</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanConfirmItem> GetCheckPlanConfirmItemByPlanId(double planId, bool loadView = false)
        {
            //贪懒加载评分项
            var elo = new EagerLoadOptions();
            if (loadView)
            {
                elo.LoadWithViewProperty();
            }
            return Query<CheckPlanConfirmItem>().Where(p => p.CheckPlanId == planId).ToList(null, elo);
        }

        /// <summary>
        /// 根据计划id获取点检确认项目
        /// </summary>
        /// <param name="planIds">计划ids</param>
        /// <param name="loadView">是否加载视图属性</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanConfirmItem> GetCheckPlanConfirmItemByPlanIds(List<double> planIds, bool loadView = false)
        {
            //贪懒加载评分项
            var elo = new EagerLoadOptions();
            if (loadView)
            {
                elo.LoadWithViewProperty();
            }
            return planIds.SplitContains(tempIds =>
            {
                return Query<CheckPlanConfirmItem>().Where(p => tempIds.Contains(p.CheckPlanId)).ToList(null, elo);
            }); 
        }

        /// <summary>
        /// 获取当前登陆人的责任部门清单
        /// </summary>
        /// <returns></returns>
        private List<DeviceDepa> GetUserDepts()
        {
            var query = Query<DeviceDepa>().As("dd").Join<DevicePur>("dp", (dd, dp) => dd.DevicePurId == dp.Id)
                .LeftJoin<DevicePur, UserInUserGroup>("uug", (dp, uug) => dp.UserGroupId == uug.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((dd, dp, uug) => dp.UserId == RT.Identity.UserId || uug.UserId == RT.Identity.UserId)
                .ToList();
            return query.ToList();
        }

        /// <summary>
        /// 获取当前登录人是否有保养确认权限
        /// </summary>
        /// <returns></returns>
        private List<DevicePur> GetCheckConfirmUser()
        {
            var query = Query<DevicePur>().As("dp").LeftJoin<UserInUserGroup>("uug", (dp, uug) => dp.UserGroupId == uug.UserGroupId)
                .Where<UserInUserGroup>((dp, uug) => (dp.UserId == RT.Identity.UserId || uug.UserId == RT.Identity.UserId))
                .ToList();
            return query.ToList();
        }

        /// <summary>
        /// 生成点检执行信息
        /// </summary>
        /// <param name="checkPlanInfos">点检信息</param>
        /// <param name="plan">点检计划</param>
        private void GetExecuteInfo(List<CheckPlanInfos> checkPlanInfos, CheckPlan plan)
        {
            checkPlanInfos.Add(new CheckPlanInfos()
            {
                Id = plan.Id,                                      //点检计划id
                CheckBeginDate = plan.CheckBeginDate.ToString(),   //计划执行时间
                CheckCycleType = plan.CheckCycleType.ToLabel(),    //类型
                No = plan.CheckPlanNo,                             //点检单号
                Qty = plan.CheckProjectList.Count(),               //项目数量
                EquipId = plan.EquipAccountId,                     //设备ID 
                EquipCode = plan.EquipAccountCode,                //设备编码
                EquipName = plan.EquipAccountName,                //设备名称
                DepartmentId = plan.DepartmentId,                  //部门ID
                DepartmentCode = plan.DepartmentCode,            //部门编码
                DepartmentName = plan.DepartmentName,            //部门名称
                State = (int)plan.ExeState,
                StateName = plan.ExeState.ToLabel(),
                Shop = plan.WorkShopName,
                Line = plan.ResourceName,
                CheckTime = plan.CheckTime,
                CheckSummary = plan.CheckSummary
            });
        }

        /// <summary>
        /// 生成点检确认信息
        /// </summary>
        /// <param name="checkPlanInfos">点检信息</param>
        /// <param name="plan">点检计划</param>
        /// <param name="checkPlanConfirmItems">点检确认信息</param>
        /// <param name="nowDevpurs">设备与权限</param>
        /// <param name="nowDepts">当前责任部门</param>
        private void GetConfirmInfo(List<CheckPlanInfos> checkPlanInfos, CheckPlan plan, EntityList<CheckPlanConfirmItem> checkPlanConfirmItems, List<DevicePur> nowDevpurs
            , List<DeviceDepa> nowDepts)
        {
            foreach (var item in checkPlanConfirmItems)
            {
                // 责任部门
                var deptPurIds = nowDepts.Where(p => p.EnterpriseId == item.DepartmentId).Select(p => p.DevicePurId).ToList();
                if (!deptPurIds.Any())
                {
                    continue;
                }
                // 是否点检确认人
                var isConfirm = nowDevpurs.Where(p => deptPurIds.Contains(p.Id)).Any(p => p.CheckConfirm);
                checkPlanInfos.Add(new CheckPlanInfos()
                {
                    Id = plan.Id,                                      //点检计划id
                    CheckBeginDate = plan.CheckBeginDate.ToString(),   //计划执行时间
                    CheckCycleType = plan.CheckCycleType.ToLabel().L10N(),    //类型
                    No = plan.CheckPlanNo,                             //点检单号
                    Qty = plan.CheckProjectList.Count(),               //项目数量
                    EquipId = plan.EquipAccountId,                     //设备ID 
                    EquipCode = plan.EquipAccountCode,                //设备编码
                    EquipName = plan.EquipAccountName,                //设备名称
                    DepartmentId = item.DepartmentId,                      //部门ID
                    DepartmentCode = item.DeptCode,                 //部门编码
                    DepartmentName = item.DeptName,                 //部门名称
                    State = (int)item.CheckExeState,
                    StateName = item.CheckExeState.ToLabel().L10N(),
                    Shop = plan.WorkShopName,
                    Line = plan.ResourceName,
                    CheckTime = plan.CheckTime,
                    CheckSummary = plan.CheckSummary,
                    CheckConfirm = isConfirm,
                });
            }
        }

        /// <summary>
        /// 点检确认项展示
        /// </summary>
        /// <param name="checkPlanId">点检计划id</param>
        /// <returns></returns>
        public virtual List<CheckPlanInfos> GetCheckPlansDisplay(double checkPlanId)
        {
            var list = new List<CheckPlanInfos>();
            // 点检计划
            var checkPlan = RF.GetById<CheckPlan>(checkPlanId);

            if (checkPlan.CheckBeginDate > DateTime.Now)
            {
                throw new ValidationException("点检计划未到执行时间".L10N());
            }

            // 当前登陆人权限责任部门
            var nowDepts = GetUserDepts();

            // 当前登陆人权限
            var nowDevpurs = GetCheckConfirmUser();

            // 点检执行数据
            if (checkPlan.ExeState != CheckExeState.NotConfirm && checkPlan.ExeState != CheckExeState.Scored && checkPlan.ExeState != CheckExeState.Confirmed)
            {
                // 当前登陆人要有责任部门权限
                if (checkPlan.DepartmentId == null || (checkPlan.DepartmentId != null && nowDepts.FirstOrDefault(p => p.EnterpriseId == checkPlan.DepartmentId) != null))
                {
                    GetExecuteInfo(list, checkPlan);
                }
            }
            else // 点检确认数据
            {
                // 点检确认项
                var checkConfirmItems = GetCheckPlanConfirmItemByPlanId(checkPlanId, true);

                GetConfirmInfo(list, checkPlan, checkConfirmItems, nowDevpurs, nowDepts);
            }
            return list;
        }

        /// <summary>
        /// 获取上次点检小结
        /// </summary>
        /// <param name="accountId">设备台账ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        public virtual string GetLastCheckSummary(double accountId, double? departmentId)
        {
            var q = Query<CheckPlan>();
            q.Where(p => p.EquipAccountId == accountId);
            q.Where(p => p.DepartmentId == departmentId);
            q.Where(p => p.ExeState == CheckExeState.Performed || p.ExeState == CheckExeState.Scored);
            q.OrderByDescending(p => p.CheckEndDate);

            return q.ToList(new PagingInfo(1, 1)).FirstOrDefault()?.CheckSummary;
        }

        /// <summary>
        /// 获取设备实时数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual EquipEapRTValueInfo GetProjectRealTimeData(CheckPlanEapData data)
        {
            if (data.ProjectDetailIds.Count <= 0)
            {
                throw new ValidationException("没有检验项目".L10N());
            }
            //获取点检设备物联项目
            var projects = Query<CheckProject>().Where(p => data.ProjectDetailIds.Contains(p.Id) && p.EquipParamSource == Enums.EquipParamSource.AutomaticValue).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (projects.Count <= 0)
            {
                throw new ValidationException("没有自动取值的设备物联项目".L10N());
            }
            //Tag列表
            List<EquipAccountPhysicalUnion> projectList = projects.Where(p => p.EquipPhysicalUnionId != 0 && p.EquipPhysicalUnionId != null && p.EquipPhysicalUnion.PhysicalUnion != null).Select(p => p.EquipPhysicalUnion).ToList();
            var tags = projectList.Select(p => p.PhysicalUnion.MDCVariableName).Distinct().ToList();

            //构建TAG参数
            var para = new EquipEapRTValuePara();
            para.Paras = new List<string>();
            para.EquipmentCode = data.EquipmentCode;
            para.Paras.AddRange(tags);

            //获取设备数
            try
            {
                var rtn = RT.Service.Resolve<IEquipmentEap>().GetEquipEapRTValueInfo(para);
                if (rtn?.Data != null && rtn.Data.Count > 0)
                {
                    projects.ForEach(p =>
                    {
                        var rtnData = rtn.Data.FirstOrDefault(x => x.Tag == p.EquipPhysicalUnion.PhysicalUnion.MDCVariableName);
                        if (rtnData != null)
                        {
                            rtnData.ProjectDetailId = p.Id;
                        }
                    });
                }
                return rtn;
            }
            catch (Exception)
            {
                throw new ValidationException("接口异常：".L10N() + "从MDC获取实时值失败!".L10N());
            }
        }

        /// <summary>
        /// 保存点检单
        /// </summary>
        /// <param name="info">点检计划保存提交实体数据</param>
        /// <param name="plan">点检计划</param>
        /// <returns></returns>
        public virtual void SaveCheckPlan(CheckSaveSubmitInfo info, CheckPlan plan)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存主单数据
                var entityUpdate = DB.Update<CheckPlan>().Where(p => p.Id == info.CheckPlanId)
                .Set(p => p.CheckSummary, info.CheckSummary)
                .Set(p => p.ExeState, CheckExeState.Performing)
                .Set(p => p.CheckEmployeeId, RT.IdentityId);

                if (plan.CheckDate == null)
                {
                    var nowDateTime = RF.Find<CheckPlan>().GetDbTime();

                    entityUpdate.Set(p => p.CheckDate, nowDateTime);
                }

                entityUpdate.Execute();

                //保存点检项目
                info.ProjectDetails.ForEach(x =>
                {
                    if (x.ProjectId <= 0)
                    {
                        throw new ValidationException("存在提交的检验项目ID为0的数据".L10N());
                    }
                    CheckMaintainResult reslut = (CheckMaintainResult)x.Result;

                    decimal? valueNull = null;
                    var value = x.ActualValue.IsNullOrEmpty() ? valueNull : decimal.Parse(x.ActualValue);
                    DB.Update<CheckProject>().Where(p => p.Id == x.ProjectId)
                        .Set(p => p.ActualValue, value)
                        .Set(p => p.ExeState, CheckExeState.Performing)
                        .Set(p => p.CheckResult, reslut)
                        .Set(p => p.DefectDesc, x.DefectDesc)
                        .Execute();
                });

                //保存备件更换数据
                SaveSparePartChangeInfo(info);

                //保存备件申请数据
                SaveSparePartApplyInfo(info);

                //保存图片
                var hepler = new FileUrlHelper();
                var attachments = new EntityList<CheckPlanAttachment>();
                //先删除没有记录的图片
                var MaintainPlanAttachment = Query<CheckPlanAttachment>().Where(p => p.OwnerId == info.CheckPlanId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var ExitPhotoIds = MaintainPlanAttachment.Select(p => p.Id).ToList();
                var submitPhotoIds = info.Photoes.Where(p => p.Id.HasValue).Select(p => p.Id).ToList();
                if (ExitPhotoIds.Count > 0)
                {
                    var DeleteIds = ExitPhotoIds.Where(x => !submitPhotoIds.Any(a => x == a)).ToList();
                    if (DeleteIds.Count > 0)
                    {
                        DeleteIds.ForEach(P =>
                        {
                            DB.Delete<CheckPlanAttachment>().Where(x => x.Id == P).Execute();
                        });
                    }
                }
                info.Photoes.ForEach(p =>
                {
                    if (p.Id == null)
                    {
                        var attachment = hepler.GenerateAttachmentBase64StringContent(new CheckPlanAttachment(), p.Content, p.FileName) as CheckPlanAttachment;
                        attachment.OwnerId = info.CheckPlanId;
                        attachments.Add(attachment);
                    }
                });
                RF.Save(attachments);
                trans.Complete();
            }
        }

        private void SaveSparePartApplyInfo(CheckSaveSubmitInfo info)
        {
            info.SparePartAplDetails.ForEach(x =>
            {
                if (x.CheckSparePartId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的备件申请项目ID为0的数据".L10N());
                }

                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var checkPlanSparePartApl = new CheckPlanSparePartApl();
                            checkPlanSparePartApl.SparePartId = x.SparePartId;
                            checkPlanSparePartApl.ApplyQty = x.ApplyQty;
                            checkPlanSparePartApl.OutStockWarehouseId = x.OutStockWarehouseId;
                            checkPlanSparePartApl.ApplyDetailId = x.AppDtlId;
                            checkPlanSparePartApl.Remark = x.Remark;
                            checkPlanSparePartApl.CheckPlanId = info.CheckPlanId;
                            checkPlanSparePartApl.GenerateId();
                            RF.Save(checkPlanSparePartApl);
                            break;
                        }
                    case 1:
                        {
                            //修改
                            DB.Update<CheckPlanSparePartApl>().Where(p => p.Id == x.CheckSparePartId && !p.IsApply)
                                .Set(p => p.SparePartId, x.SparePartId)
                                .Set(p => p.ApplyQty, x.ApplyQty)
                                .Set(p => p.OutStockWarehouseId, x.OutStockWarehouseId)
                                .Set(p => p.ApplyDetailId, x.AppDtlId)
                                .Set(p => p.Remark, x.Remark)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除(已申请的不允许删除)
                            DB.Delete<CheckPlanSparePartApl>().Where(p => p.Id == x.CheckSparePartId && !p.IsApply).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// PDA保存点检备件申请
        /// </summary>
        /// <param name="info"></param>
        public virtual void SaveCheckPlanApplyInfo(CheckSaveSubmitInfo info)
        {
            // 未申请的记录
            var applyRecord = RT.Service.Resolve<CheckController>().GetCheckPlanSparePartApls(info.CheckPlanId, false);

            // pda申请数据
            var infoApplyList = info.SparePartAplDetails;    

            foreach (var apply in applyRecord)
            {
                var applyInfo = infoApplyList.FirstOrDefault(p => p.SparePartId == apply.SparePartId);
                if (applyInfo == null)
                {
                    continue;
                }

                apply.ApplyQty = applyInfo.ApplyQty;
                apply.OutStockWarehouseId = applyInfo.OutStockWarehouseId;
            }
            RF.Save(applyRecord);
        }

        /// <summary>
        /// PDA保存点检备件更换
        /// </summary>
        /// <param name="info"></param>
        public virtual void SaveCheckPlanChangeInfo(CheckSaveSubmitInfo info)
        {
            // 未完成的记录
            var changeRecord = RT.Service.Resolve<CheckController>().GetCheckPlanSpareParts(info.CheckPlanId, ChangeSparePartState.New);

            // pda更换数据
            var infoChangeList = info.SparePartDetails;

            foreach (var change in changeRecord)
            {
                var changeInfo = infoChangeList.FirstOrDefault(p => p.SparePartId == change.SparePartId);
                if (changeInfo == null)
                {
                    continue;
                }

                change.ChangeQty = changeInfo.ChangeQty;
                change.PartOutDepotDetailId = changeInfo.OutDtlId;
            }
            RF.Save(changeRecord);
        }

        private void SaveSparePartChangeInfo(CheckSaveSubmitInfo info)
        {
            info.SparePartDetails.ForEach(x =>
            {
                if (x.CheckSparePartId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的备件更换项目ID为0的数据".L10N());
                }

                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var checkPlanSparePart = new CheckPlanSparePart();
                            checkPlanSparePart.SparePartId = x.SparePartId;
                            checkPlanSparePart.PartOutDepotDetailId = x.OutDtlId;
                            checkPlanSparePart.ChangeQty = x.ChangeQty;
                            checkPlanSparePart.Remark = x.Remark;
                            checkPlanSparePart.CheckPlanId = info.CheckPlanId;
                            checkPlanSparePart.GenerateId();
                            RF.Save(checkPlanSparePart);
                            break;
                        }
                    case 1:
                        {
                            //修改(已更换的不允许删除)
                            DB.Update<CheckPlanSparePart>().Where(p => p.Id == x.CheckSparePartId && p.State == ChangeSparePartState.New)
                                .Set(p => p.SparePartId, x.SparePartId)
                                .Set(p => p.PartOutDepotDetailId, x.OutDtlId)
                                .Set(p => p.ChangeQty, x.ChangeQty)
                                .Set(p => p.Remark, x.Remark)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除(已更换的不允许删除)
                            DB.Delete<CheckPlanSparePart>().Where(p => p.Id == x.CheckSparePartId && p.State == ChangeSparePartState.New).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// 提交点检单校验
        /// </summary>
        /// <param name="checkPlan"></param>
        private void SubmitCheckPlanValidation(CheckPlan checkPlan)
        {
            if (!CheckPlanIsExeState(checkPlan.Id))
            {
                throw new ValidationException("点检项目已在其他端操作，不允许提交。".L10N());
            }
            //校验
            if (checkPlan.CheckProjectList.Count == 0)
            {
                throw new ValidationException("不存在点检项目，不允许提交。".L10N());
            }
            if (checkPlan.CheckProjectList.Any(p => p.CheckResult == null))
            {
                throw new ValidationException("点检项目未点检完成，不允许提交。".L10N());
            }
            if (checkPlan.CheckProjectList.Any(p => p.CheckResult == CheckMaintainResult.NG && p.DefectDesc.IsNullOrEmpty()))
            {
                throw new ValidationException("NG点检项目没有填写缺陷描述，不允许提交。".L10N());
            }
            if (checkPlan.CheckPlanSparePartList.Count > 0 && checkPlan.CheckPlanSparePartList.Any(p => p.State != Enums.ChangeSparePartState.Finished))
            {
                throw new ValidationException("当前设备存在未更换完成的备件，不允许提交。".L10N());
            }
            if (checkPlan.CheckPlanSparePartAplList.Count > 0 && checkPlan.CheckPlanSparePartAplList.Any(p => !p.IsApply))
            {
                throw new ValidationException("当前设备存在未申请完成的备件，不允许提交。".L10N());
            }
        }

        private EquipAccountResume CreateResume(CheckPlan checkPlan)
        {
            return new EquipAccountResume
            {
                No = checkPlan.CheckPlanNo,
                Changed = "",
                EquipAccountId = checkPlan.EquipAccountId,
                ResumeType = ResumeType.Checked,
                State = checkPlan.EquipAccount == null ? AccountState.Running : checkPlan.EquipAccount.State,
            };
        }

        /// <summary>
        /// 提交点检单
        /// </summary>
        /// <param name="checkPlan"></param>
        /// <returns></returns>
        public virtual CheckPlan SubmitCheckPlan(CheckPlan checkPlan)
        {
            if (checkPlan == null)
            {
                return new CheckPlan();
            }
            // 校验
            SubmitCheckPlanValidation(checkPlan);

            //判断提交后状态是待确认还是已执行
            //通过配置项得到部门列表，列表为空则不进行确认，部门不空则进行确认
            List<double> departmentIdList = new List<double>();
            List<double> scoreProjectIds = new List<double>();
            bool IsMarkScore = false;
            var config = ConfigService.GetConfig(new CheckConfirmDepartConfig(), typeof(CheckPlanViewModel));

            if (config != null && !config.DepartmentIds.IsNullOrEmpty())
            {
                departmentIdList.AddRange(config.DepartmentIds.Split(',').Select(x => Convert.ToDouble(x)));
                IsMarkScore = config.IsMarkScore;
            }
            departmentIdList = departmentIdList.Distinct().ToList();
            checkPlan.ExeState = departmentIdList.Any() ? CheckExeState.NotConfirm : CheckExeState.Performed;
            if (checkPlan.ExeState == CheckExeState.NotConfirm && IsMarkScore)
            {
                scoreProjectIds = GetTpmWeekInspectScores(ScoreType.Check);
                if (!scoreProjectIds.Any())
                {
                    throw new ValidationException("未维护类型为{0}的TPM评分项！".L10nFormat(ScoreType.Check.ToLabel().L10N()));
                }
            }

            //判断点检结果
            if (checkPlan.ExeResult == null)
            {
                checkPlan.ExeResult = checkPlan.CheckProjectList
                    .All(p => p.CheckResult == CheckMaintainResult.OK || p.CheckResult == CheckMaintainResult.Unright) ? ExeResult.Successed : ExeResult.Failed;
            }

            // 判断是否已报修
            checkPlan.WhetherRepair = RT.Service.Resolve<IEquipRepairBill>().CheckPlanWithRepairBill(checkPlan.EquipAccountId, checkPlan.CheckPlanNo, 0) ? YesNo.Yes : YesNo.No;


            //点检执行时间赋值开始时间（兼容补录）(字段用途未知，先按计划时间赋值)
            checkPlan.ActCheckBeginDate = checkPlan.CheckBeginDate;
            checkPlan.ActCheckEndDate = checkPlan.CheckEndDate;
            checkPlan.CheckProjectList.ForEach(p => p.ExeState = checkPlan.ExeState);

            if (checkPlan.CheckDate == null)
            {
                var nowDateTime = RF.Find<CheckPlan>().GetDbTime();
                checkPlan.CheckDate = nowDateTime;
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存检验单提交结果
                RF.Save(checkPlan);

                //保存设备履历
                using (SIE.DataAuth.DataAuths.LoadAll())
                {
                    var equip = Query<EquipAccount>().Where(p => p.Id == checkPlan.EquipAccountId).Select(p => new
                    {
                        Id = p.Id,
                        State = p.State,
                    }).FirstOrDefault<EquipInfo>();
                    if (equip != null && equip.Id != 0)
                    {
                        RT.Service.Resolve<EquipController>().GenerateEquipAccountResume(equip.Id, ResumeType.Checked, equip.State, checkPlan.CheckPlanNo);
                    }
                }

                //更新备件更换记录标记
                if (checkPlan.ExeState == CheckExeState.Performed)
                {
                    RT.Service.Resolve<SparePartController>().UpdateSparePartChangedRecordFlag(FromType.SpotCheck, checkPlan.Id);
                }

                // 如果要确认，则生成点检确认单以及评分项
                if (checkPlan.ExeState == CheckExeState.NotConfirm)
                {
                    //生成所有匹配部门的点检确认单
                    GenerateCheckConfirmation(checkPlan.Id, departmentIdList, scoreProjectIds,IsMarkScore);
                }

                //需要推送异常信息
                if (checkPlan.IsAbnormalInfoPush && checkPlan.ExeResult == ExeResult.Failed)
                {
                    GenerateEquipCheckAbnormalInfo(checkPlan);
                }
                trans.Complete();
            }

            return checkPlan;
        }

        /// <summary>
        /// 生成设备点检异常信息
        /// </summary>
        /// <param name="checkPlan"></param>
        private void GenerateEquipCheckAbnormalInfo(CheckPlan checkPlan)
        {
            //获取设备点检类型的异常信息定义
            var define = RT.Service.Resolve<AbnormalInfoController>().GetAbnormalDefinition(AbnormalSource.EquipCheck);
            if (define == null)
            {
                throw new ValidationException("不存在[设备点检]的异常信息定义，请现在[异常信息定义]界面维护。".L10N());
            }
            //收集异常点检项目
            var abnormalProjects = checkPlan.CheckProjectList.Where(p => p.CheckResult == CheckMaintainResult.NG);
            //收集异常点检名称
            var abnormalProjectNames = abnormalProjects.Select(p => p.ProjectName);
            var abnormalProjectNamesStr = string.Join(";", abnormalProjectNames);
            //收集异常点检缺陷描述
            var defectDescs = abnormalProjects.Select(p => p.DefectDesc);
            var defectDescsStr = string.Join(";", defectDescs);

            var abnormal = new AbnormalInfor()
            {
                No = RT.Service.Resolve<AbnormalInfoController>().GetNewAbnormalInfoNo(),
                AbnormalStatus = AbnormalStatus.ToProcess,
                IsSendPdca = false,
                IsRectificationTask = false,
                AbnormalInfoDefinitionId = define.Id,
                InspectionNo = checkPlan.CheckPlanNo,
                EquipmentId = checkPlan.EquipAccountId,
                ProjectDesc = defectDescsStr,
                ProjectNg = abnormalProjectNamesStr
            };

            RF.Save(abnormal);

        }

        /// <summary>
        /// UI执行备件更换
        /// </summary>
        /// <param name="uiCheckPlanSpareParts"></param>
        public virtual void UIChangeCheckPlanSparePart(List<CheckPlanSparePart> uiCheckPlanSpareParts)
        {
            if (uiCheckPlanSpareParts == null)
            {
                return;
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiCheckPlanSpareParts.ForEach(p => RF.Save(p));
                var checkPlan = uiCheckPlanSpareParts.FirstOrDefault().CheckPlan;
                ChangeCheckPlanSparePart(checkPlan.Id);
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行备件更换逻辑
        /// </summary>
        /// <param name="checkPlanId"></param>
        public virtual void ChangeCheckPlanSparePart(double checkPlanId)
        {
            try
            {
                var datas = RT.Service.Resolve<CheckController>().GetCheckPlanSpareParts(checkPlanId, EMS.Enums.ChangeSparePartState.New);
                if (datas.Count <= 0)
                {
                    throw new ValidationException("没有备件更换数据".L10N());
                }
                var list = datas.Where(p => p.PartOutDepotDetail != null).ToList();
                if (list.Count <= 0)
                {
                    throw new ValidationException("存在备件更换数据没有选择备件出库单".L10N());
                }
                if (list.Any(p => p.ChangeQty <= 0))
                {
                    throw new ValidationException("存在备件更换数据更换数量为0".L10N());
                }

                using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {

                    //回写备件申请单使用数量
                    list.ForEach(p =>
                    {
                        if (p.PartOutDepotDetail.UseCount + p.ChangeQty > p.PartOutDepotDetail.OutDepotCount)
                        {
                            throw new ValidationException("备件[{0}]更换数量不能大于剩余数量".L10nFormat(p.SparePart.SparePartCode));
                        }
                        //回写申请单
                        DB.Update<PartOutDepotDetail>().Where(x => x.Id == p.PartOutDepotDetailId).Set(x => x.UseCount, x => x.UseCount + p.ChangeQty).Execute();
                        //修改备件更换状态
                        DB.Update<CheckPlanSparePart>().Where(x => x.Id == p.Id).Set(x => x.State, Enums.ChangeSparePartState.Finished).Execute();
                        //修改序列号状态
                        DB.Update<StoreSummaryDetail>().Where(x => x.Id == p.PartOutDepotDetail.SeriaNoRefId).Set(x => x.StoreStatus, OrdNumStoreStatus.Using).Execute();
                        //插入备件履历
                        var record = new SparePartChangedRecord()
                        {
                            EquipAccountId = p.CheckPlan.EquipAccountId,
                            Qty = p.ChangeQty,
                            OldSerialNumber = p.OldSequence,
                            BatchNumber = p.PartOutDepotDetail?.BatchNo,
                            SerialNumber = p.PartOutDepotDetail?.SeriaNo,
                            Source = FromType.SpotCheck,
                            SourceNo = p.CheckPlan.CheckPlanNo,
                            SourceId = p.CheckPlanId,
                            SparePartId = p.SparePartId
                        };
                        RF.Save(record);
                        trans.Complete();
                    });
                }
            }
            catch (Exception ex)
            {
                //清空未完成的更换单的出库单
                double? value = null;
                DB.Update<CheckPlanSparePart>().Where(p => p.CheckPlanId == checkPlanId && p.State == ChangeSparePartState.New).Set(p => p.PartOutDepotDetailId, value).Execute();
                throw new ValidationException(ex.GetBaseException().Message);
            }

        }

        /// <summary>
        /// 获取某个点检计划的点检项目
        /// </summary>
        /// <param name="checkPlanId">点检计划Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<CheckProject> GetCheckProjectList(double checkPlanId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<CheckProject>().Where(p => p.CheckPlanId == checkPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            return list;
        }

        /// <summary>
        /// 获取点检计划备件更换列表
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <param name="checkNo">点检计划单号</param>
        /// <param name="equipId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanSparePart> GetCheckPlanSpareParts(double checkPlanId, string checkNo, double equipId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<CheckPlanSparePart>();
            q.Where(p => p.CheckPlanId == checkPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(CheckPlanSparePart.PartOutDepotDetailProperty);
            elo.LoadWith(PartOutDepotDetail.OutDepotProperty);
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            //查询、赋值申请单明细
            if (list.Count > 0)
            {
                var outDtls = RT.Service.Resolve<OutDepotController>().GetPartOutDepotDetailDtl(checkNo, SpareParts.OutDepots.Enums.OutDepotType.Check);
                list.ForEach(p =>
                {
                    if (p.State == ChangeSparePartState.New)
                    {
                        //赋值UI属性
                        var outDtl = outDtls.FirstOrDefault(x => x.SparePartId == p.SparePartId);
                        if (outDtl != null)
                        {
                            p.PartOutDepotDetailId = outDtl.Id;
                            p.OutDepotNoView = outDtl.OutDepot.No;
                            p.SeriaNoView = outDtl.SeriaNoRef?.OrderNumberCode;
                            p.BatchNoView = outDtl.BatchNoRef?.BatchNumber;
                            p.RemainingQty = outDtl.OutDepotCount - outDtl.UseCount;
                        }
                    }
                    else
                    {
                        p.RemainingQty = p.PartOutDepotDetail.OutDepotCount - p.PartOutDepotDetail.UseCount;
                    }
                });
            }

            return list;
        }

        /// <summary>
        /// 获取点检计划备件申请列表
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <param name="equipId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanSparePartApl> GetCheckPlanSparePartApls(double checkPlanId, double equipId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<CheckPlanSparePartApl>();
            q.Where(p => p.CheckPlanId == checkPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(CheckPlanSparePartApl.ApplyDetailProperty);
            elo.LoadWith(ApplyDetail.SparePartAppProperty);
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            //查询、赋值备件库存
            if (list.Count > 0)
            {
                var partIds = list.Select(p => p.SparePartId).ToList();
                var whIds = list.Where(p => p.OutStockWarehouseId != null).Select(p => (double)p.OutStockWarehouseId).ToList();
                var whInfos = RT.Service.Resolve<SparePartController>().GetStoreSummaryDepots(partIds, whIds);
                list.ForEach(p =>
                {
                    //赋值库存
                    var whInfo = whInfos.FirstOrDefault(x => x.SparePartId == p.SparePartId && x.WarehouseId == p.OutStockWarehouseId);
                    p.StoreQty = whInfo?.StoreQty ?? 0;
                });
            }
            list.MarkSaved();
            return list;
        }

        /// <summary>
        /// 获取某个点检计划的执行图片
        /// </summary>
        /// <param name="checkPlanId">点检计划Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<CheckPlanAttachment> GetCheckPlanAttachmentList(double checkPlanId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<CheckPlanAttachment>().Where(p => p.OwnerId == checkPlanId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            return list;
        }

        /// <summary>
        /// 获取某个点检计划指定的对应确认部门的点检确认单（评分项），如果点检确认部门ID为null则拿到全部的评分项。
        /// </summary>
        /// <param name="checkPlanId">点检计划Id</param>
        /// <param name="confirmDeptId">点检确认部门ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<CheckConfirmation> GetCheckConfirmationList(double checkPlanId, double? confirmDeptId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<CheckConfirmation>().Where(p => p.OwnerId == checkPlanId);

            if (confirmDeptId != null && confirmDeptId != 0)
            {
                q.Where(p => p.ConfirmDeptId == confirmDeptId);
            }

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            return list;
        }

        /// <summary>
        /// 更换点检计划状态
        /// </summary>
        /// <param name="checkPlanId"></param>
        /// <param name="state"></param>
        public virtual void ChangeCheckPlanState(double checkPlanId, CheckExeState state)
        {
            var entityUpdate = DB.Update<CheckPlan>().Where(p => p.Id == checkPlanId).Set(p => p.ExeState, state);

            var plan = RF.GetById<CheckPlan>(checkPlanId);

            if (plan != null && plan.CheckDate == null)
            {
                var nowDateTime = RF.Find<CheckPlan>().GetDbTime();

                entityUpdate.Set(p => p.CheckDate, nowDateTime);
            }

            entityUpdate.Execute();
        }

        #endregion

        #region 预警
        /// <summary>
        /// 获取提前预警的点检计划单
        /// </summary>
        /// <returns>提前预警的点检计划单</returns>
        public virtual EntityList<CheckPlan> GetAlertTimeOutCheckPlanList()
        {
            //获取点检类型(日、班、频次)
            CheckPlanType checkType = RT.Service.Resolve<CheckController>().GetCheckPlanType();
            //获取提前预警时间（小时）
            int alertTimeOut = (int)RT.Service.Resolve<CheckController>().GetCheckAlertTime();
            var date = DateTime.Now.AddHours(alertTimeOut);
            var beginDate = DateTime.Now;
            var q = Query<CheckPlan>().Where(p => p.CheckBeginDate <= date
                                && p.CheckBeginDate>beginDate && p.ExeState == CheckExeState.NotPerformed && p.CheckPlanType == checkType);
            return q.ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取超时的点检计划单
        /// </summary>
        /// <returns>超时的点检计划单</returns>
        public virtual EntityList<CheckPlan> GetTimeOutCheckPlanList()
        {
            //获取点检类型(日、班、频次)
            CheckPlanType checkType = RT.Service.Resolve<CheckController>().GetCheckPlanType();
            //获取超时预警时间（小时）
            int timeOut = (int)RT.Service.Resolve<CheckController>().GetCheckExpiredTime();
            var date = DateTime.Now.AddHours(-timeOut);
            var date1 = date.AddHours(-timeOut);
            var q = Query<CheckPlan>().Where(p => p.CheckEndDate <= date && p.CheckEndDate > date1 && (p.ExeState == CheckExeState.NotPerformed || p.ExeState == CheckExeState.Performing) && p.CheckPlanType == checkType);
            return q.ToList(new PagingInfo { PageSize = 99999 }, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 点检确认
        /// <summary>
        /// 为点检确认提供指定点检计划Id所对应的点检计划详情
        /// </summary>
        /// <param name="checkPlanId">点检计划Id</param>
        /// <param name="confirmDeptId">确认部门Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<CheckConfirmation> GetCheckPlanConfirmations(double checkPlanId, double confirmDeptId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<CheckConfirmation>().Where(p => p.OwnerId == checkPlanId && p.ConfirmDeptId == confirmDeptId);

            var elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(null, elo);

            return list;
        }

        /// <summary>
        /// 获取当前登录用户所属部门全部需要点检确认的检验单信息
        /// </summary>
        /// <param name="keyword">设备的编码或者名称的关键字</param>
        /// <param name="departmentId">部门ID</param>
        /// <param name="pagingInfo">分页实体</param>
        /// <returns></returns>
        public virtual IEnumerable<CheckPlanInfos> GetNotConfirmedCheckPlans(string keyword, double? departmentId, PagingInfo pagingInfo)
        {
            var list = new List<CheckPlanInfos>();

            //用户是否有点检确认的权限，没有的话返回空
            var query = Query<DevicePur>()
                 .LeftJoin<UserInUserGroup>((x, y) => x.UserGroupId == y.UserGroupId)
                 .Where<UserInUserGroup>((x, y) => (x.UserId == RT.Identity.UserId || y.UserId == RT.Identity.UserId) && x.CheckConfirm);

            var hasCheckConfirmCount = query.Count();

            if (hasCheckConfirmCount <= 0)
            {
                return list;
            }


            //过滤 用户有权限点检确认的部门ID
            var deviceInfo = RT.Service.Resolve<DevicePurController>().GetDepartmentsForConfirmMaintain(RT.Identity.UserId);

            var deptIds = deviceInfo.Select(x => x.DeptId).Cast<double?>();

            if (departmentId != null)
            {
                deptIds = deptIds.Where(p => p == departmentId).ToList();
            }

            // 找出待确认的点检单
            var queryer = Query<CheckPlan>()
                .Where(p => p.ExeState == CheckExeState.NotConfirm)
                .WhereIf(keyword.IsNotEmpty(), p => p.EquipAccount.Code.Contains(keyword) || p.EquipAccount.Name.Contains(keyword))
                .OrderBy(p => p.CheckDate);
            var iquery = queryer.ToQuery();
            iquery.QueryWithEquipAccountPermissions(CheckPlan.EquipAccountIdProperty.Name);
            var queryerList = queryer.Repository.QueryList(iquery, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());

            // 找出这些单的确认项
            var checkPlanIds = new List<double>();
            var checkPlanList = new List<CheckPlan>();
            foreach (var e in queryerList)
            {
                var plan = e as CheckPlan;
                checkPlanList.Add(plan);
                checkPlanIds.Add(plan.Id);
            }
            var checkPlanConfirmItems = checkPlanIds.SplitContains(tempIds =>
            {
                return Query<CheckPlanConfirmItem>().Where(p => p.CheckExeState == CheckExeState.NotConfirm && tempIds.Contains(p.CheckPlanId) && deptIds.Contains(p.DepartmentId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            //当前用户可管理的设备台账 过滤设备 有权限的设备
            var departmentList = RT.Service.Resolve<EnterpriseController>().GetEnterpriseByIds(deviceInfo.Select(p => p.DeptId).Distinct().ToList());

            foreach (var item in checkPlanConfirmItems)
            {
                // 部门权限信息
                var infos = deviceInfo.Where(x => x.DeptId == item.DepartmentId).ToList();

                // 点检单
                CheckPlan plan = checkPlanList.FirstOrDefault(x => x.Id == item.CheckPlanId);

                // 部门
                var department = departmentList.FirstOrDefault(p => p.Id == item.DepartmentId);
                if (infos.Any(p => p.IsConfirm))
                {
                    list.Add(new CheckPlanInfos()
                    {
                        Id = plan.Id,                                      //点检计划id
                        CheckBeginDate = plan.CheckBeginDate.ToString("yyyy/MM/dd HH:mm:ss"),   //计划执行时间
                        CheckCycleType = plan.CheckCycleType.ToLabel().L10N(),    //类型
                        No = plan.CheckPlanNo,                             //点检单号
                        Qty = plan.CheckProjectList.Count(),               //项目数量
                        EquipId = plan.EquipAccountId,                     //设备ID 
                        EquipCode = plan.EquipAccountCode,                //设备编码
                        EquipName = plan.EquipAccountName,                //设备名称
                        DepartmentId = item.DepartmentId,                      //部门ID
                        DepartmentCode = department?.Code,                 //部门编码
                        DepartmentName = department?.Name,                 //部门名称
                        State = (int)item.CheckExeState,
                        StateName = item.CheckExeState.ToLabel().L10N(),
                        Shop = plan.WorkShopName,
                        Line = plan.ResourceName,
                        CheckTime = plan.CheckTime,
                        CheckSummary = plan.CheckSummary,
                        WhetherRepair = (int)plan.WhetherRepair,
                        CheckDate = plan.CheckDate.HasValue ? plan.CheckDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                        CheckEmployeeName = plan.CheckEmployeeName,
                    });
                }
            }
            

            return list;
        }

        /// <summary>
        /// 依据指定点检计划单及某个点检确认部门生成点检确认单（评分项）
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <param name="confirmDeptIds">点检计划单Ids</param>
        /// <param name="scoreProjectIds">评分项目Id</param>
        /// <param name="IsMarkScore">是否评分</param>
        public virtual void GenerateCheckConfirmation(double checkPlanId, List<double> confirmDeptIds, List<double> scoreProjectIds, bool IsMarkScore)
        {
            //行锁数据，防止并发
            DB.Update<CheckPlan>().Where(p => p.Id == checkPlanId).Set(p => p.UpdateBy, RT.IdentityId).Execute();

            // 点检确认项 、评分项
            EntityList<CheckPlanConfirmItem> checkPlanConfirmItems = new EntityList<CheckPlanConfirmItem>();
            EntityList<CheckConfirmation> checkConfirmations = new EntityList<CheckConfirmation>();
            foreach (var confirmDeptId in confirmDeptIds)
            {
                CheckPlanConfirmItem checkPlanConfirmItem = new CheckPlanConfirmItem
                {
                    CheckPlanId = checkPlanId,
                    DepartmentId = confirmDeptId,
                    CheckExeState = CheckExeState.NotConfirm,
                };
                checkPlanConfirmItems.Add(checkPlanConfirmItem);
                //生成评分项
                if (IsMarkScore)
                {
                    foreach (var projectId in scoreProjectIds)
                    {
                        var checkConfirmation = new CheckConfirmation();
                        checkConfirmation.OwnerId = checkPlanId;
                        checkConfirmation.TpmScoreProjectId = projectId;
                        checkConfirmation.ConfirmDeptId = confirmDeptId;
                        checkConfirmation.FileName = "_";//平台规定不能为空，先用“_”占位，然后再置空。
                        checkConfirmations.Add(checkConfirmation);
                    }
                }
                
            }
            RF.BatchInsert(checkPlanConfirmItems);
            RF.BatchInsert(checkConfirmations);
        }

        /// <summary>
        /// 获取TPM评分项
        /// </summary>
        /// <returns></returns>
        private List<double> GetTpmWeekInspectScores(ScoreType scoreType)
        {
            //获取TPM评分项
            var query = Query<TpmWeekInspectScore>()
                .Where(p => p.ScoreType == scoreType)
                .Select(p => new {p.Id})
                .ToList<double>().ToList();
            return query;
        }

        /// <summary>
        /// 是否具有点检执行权限
        /// </summary>
        /// <param name="checkPlanId">点检计划ID</param>
        /// <returns></returns>
        public virtual bool CanExeCheckConfirmation(double checkPlanId)
        {
            //点检计划
            var checkPlan = Query<CheckPlan>().Where(p => p.Id == checkPlanId).FirstOrDefault();

            //判断是否按部门进行保养
            bool isNeedDepartment = RT.Service.Resolve<CheckController>().IsDepartmentCheck();

            //匹配人员权限 
            var devicePurQueryer = Query<DevicePur>();
            if (isNeedDepartment)
            {
                devicePurQueryer.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                    .LeftJoin<DeviceDepa>((a, d) => a.Id == d.DevicePurId)
                    .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)
                    .Where<DeviceDepa>((a, d) => d.EnterpriseId == checkPlan.DepartmentId);
            }

            if (checkPlan.DepartmentId == null || checkPlan.DepartmentId == 0)
            {
                return true;
            }

            var list = devicePurQueryer.ToList();
            return !(list.Count == 0);

        }

        /// <summary>
        /// 获取是否评分配置项
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNeedMarkScore()
        {
            // 是否启用评分
            var needScoreConfig = ConfigService.GetConfig<CheckConfirmDepartConfigValue>(new CheckConfirmDepartConfig(), typeof(CheckPlanViewModel));
            bool isNeedScore = false;
            if (needScoreConfig != null)
            {
                isNeedScore = needScoreConfig.IsMarkScore;
            }
            return isNeedScore;
        }

        /// <summary>
        /// 是否具有点检确认权限
        /// </summary>
        /// <param name="checkPlanId">点检计划ID</param>
        /// <param name="confirmDeptId">确认部门ID</param>
        public virtual bool CanSubmitCheckConfirmation(double checkPlanId, double confirmDeptId)
        {
            //点检计划
            var checkPlan = Query<CheckPlan>().Where(p => p.Id == checkPlanId).FirstOrDefault();

            //判断是否按部门进行保养
            bool isNeedDepartment = RT.Service.Resolve<CheckController>().IsDepartmentCheck();

            //匹配人员权限 
            var devicePurQueryer = Query<DevicePur>();
            if (isNeedDepartment)
            {
                devicePurQueryer.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId && a.CheckConfirm)
                    .LeftJoin<DeviceDepa>((a, d) => a.Id == d.DevicePurId)
                    .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)
                    .Where<DeviceDepa>((a, d) => d.EnterpriseId == confirmDeptId);
            }

            if (checkPlan.DepartmentId == null || checkPlan.DepartmentId == 0)
            {
                devicePurQueryer.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId && a.CheckConfirm)
                    .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId);
            }

            var list = devicePurQueryer.ToList();
            return !(list.Count == 0);
        }
        #endregion

        #region 删除点检计划

        /// <summary>
        /// 删除点检计划
        /// </summary>
        /// <param name="ids"></param>
        public virtual void DeleteCheckPlan(List<double> ids)
        {
            var CheckPlans = GetCheckPlanList(ids);
            if (CheckPlans.Any(p => p.ExeState != CheckExeState.NotPerformed))
            {
                throw new ValidationException("只有点检状态为【未执行】的数据才能操作".L10N());
            }
            CheckPlans.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
            });
            RF.Save(CheckPlans);
        }

        #endregion


        /// <summary>
        /// 选择备件申请
        /// </summary>
        /// <param name="sparePartList">备件集合</param>
        public virtual void SelSparePart(List<CheckPlanSparePart> sparePartList)
        {

            EntityList<CheckPlanSparePart> checkPlanSpareParts = new EntityList<CheckPlanSparePart>();
            sparePartList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                p.ChangeQty = 1;
                checkPlanSpareParts.Add(p);
            });
            RF.Save(checkPlanSpareParts);
            if (sparePartList.FirstOrDefault()?.CheckPlan?.ExeState == CheckExeState.NotPerformed)
            {
                RT.Service.Resolve<CheckPlanController>().ChangeCheckPlanState(sparePartList.FirstOrDefault().CheckPlanId, CheckExeState.Performing);
            }
        }

        /// <summary>
        /// 根据点检执行状态检查点检计划是否在其他端提交
        /// </summary>
        /// <param name="checkPlanId"></param>
        /// <returns>提交过则返回false</returns>
        public virtual bool CheckPlanIsExeState(double checkPlanId)
        {
            var checkPlan = Query<CheckPlan>().Where(p => p.Id == checkPlanId).FirstOrDefault();
            if (checkPlan == null)
            {
                //查询不到单据信息也可能为未提交状态
                return true;
            }
            return checkPlan.ExeState == CheckExeState.NotPerformed || checkPlan.ExeState == CheckExeState.Performing;
        }
    }
}