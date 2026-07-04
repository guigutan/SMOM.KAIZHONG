using SIE.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.RunningStates;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.CalendarSchemes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 设备综合统计报表控制器
    /// </summary>
    public class EquipmentIntegrateStatisticController : DomainController
    {
        /// <summary>
        /// 查询设备综合统计数据
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>        
        public virtual EquipStaticViewModel GetEquipmentIntegrateStatisticViewModels(
            EquipmentIntegrateStatisticCriteria criteria)
        {
            EquipStaticViewModel equipStaticViewModel = new EquipStaticViewModel();

            Dictionary<string, EquipmentIntegrateStatisticInfoModel> viewModelsDictionary
                 = new Dictionary<string, EquipmentIntegrateStatisticInfoModel>();

            var query = Query<EquipAccount>();

            //查询条件过滤
            if (criteria.EquipTypeId.HasValue && criteria.EquipTypeId != 0)
                query.Where(w => w.EquipModel.EquipTypeId == criteria.EquipTypeId);

            if (criteria.EquipModelId.HasValue && criteria.EquipModelId != 0)
                query.Where(w => w.EquipModelId == criteria.EquipModelId);

            if (criteria.EquipCodeId.HasValue && criteria.EquipCodeId != 0)
                query.Where(w => w.Id == criteria.EquipCodeId);

            if (criteria.UseDepartmentId.HasValue && criteria.UseDepartmentId != 0)
                query.Where(w => w.UseDepartmentId == criteria.UseDepartmentId);

            if (criteria.WorkShopId.HasValue && criteria.WorkShopId != 0)
                query.Where(w => w.WorkShopId == criteria.WorkShopId);

            if (criteria.WipResourceId.HasValue && criteria.WipResourceId != 0)
                query.Where(w => w.ResourceId == criteria.WipResourceId);

            if (!criteria.EquipName.IsNullOrEmpty() && !criteria.EquipCodeId.HasValue)
                query.Where(w => w.Name == criteria.EquipName);
            var equipments = query.ToList();
            if (!equipments.Any())
                throw new ValidationException("查询无设备".L10N());
            var equipAccountIds = equipments.Select(p => p.Id).ToList();

            if (criteria.UtilizationRate <= 0)
            {
                throw new ValidationException("查询【利用率标准】输入不正确,请输入正确".L10N());
            }

            if (equipAccountIds.Count <= 0
                && (criteria.EquipTypeId.HasValue
                || criteria.EquipModelId.HasValue
                || criteria.EquipCodeId.HasValue
                || criteria.UseDepartmentId.HasValue
                || criteria.WorkShopId.HasValue
                || criteria.WipResourceId.HasValue))
            {
                throw new ValidationException("筛选条件未能找到任何设备!请检查".L10N());
            }
            if (!criteria.Year.HasValue)
            {
                throw new ValidationException("请输入年份".L10N());
            }

            equipStaticViewModel.Year = criteria.Year.Value.Year;
            equipStaticViewModel.Month = (int)criteria.Month;

            //设备台数
            equipStaticViewModel.EquipmentCount = equipAccountIds.Count;

            var startDate = new DateTime(criteria.Year.Value.Year, (int)criteria.Month, 1);
            var endDate = startDate.AddMonths(1);
            var statistics = GetStatistics(equipAccountIds, startDate, endDate).OrderBy(m => m.StatisticDate);

            decimal totalRunningTime = 0;
            decimal totalNumberOfFailures = 0;
            decimal totalFailureTime = statistics.Sum(m => m.EquipMentFailureTime);

            var dates = statistics.Select(m => DateTimeFormat.ToShortFormat2(m.StatisticDate)).Distinct();

            foreach (var date in dates)
            {
                var statisticViewModel = new EquipmentIntegrateStatisticInfoModel();
                statisticViewModel.DownTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.DownTime);
                statisticViewModel.FailureTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.FailureTime);
                statisticViewModel.NumberOfFailures = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.NumberOfFailures);
                statisticViewModel.PlanningTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.PlanningTime);
                statisticViewModel.OfflineTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.OfflineTime);

                statisticViewModel.RepairTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.RepairTime);
                statisticViewModel.RunningTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.RunningTime);
                statisticViewModel.UnkownTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.UnkownTime);
                statisticViewModel.StandbyTime = statistics
                    .Where(m => DateTimeFormat.ToShortFormat2(m.StatisticDate) == date).Sum(m => m.StandbyTime);

                statisticViewModel.StatisticDate = date.Split('-')[2];

                totalNumberOfFailures += statisticViewModel.NumberOfFailures;
                totalRunningTime += statisticViewModel.RunningTime;
                statisticViewModel.TargetRate = (decimal)(criteria.UtilizationRate);
                viewModelsDictionary.Add(date, statisticViewModel);
            }

            //3）平均无故障工作时间 MTBF = 设备运行时长/ 故障次数  单位：h
            equipStaticViewModel.Mtbf = Math.Round(totalNumberOfFailures != 0 ? totalRunningTime / totalNumberOfFailures : 0, 1);

            //4）平均修复时间 MTTR = 设备故障总时间 / 故障次数
            equipStaticViewModel.Mttr = Math.Round(totalNumberOfFailures != 0 ? totalFailureTime / totalNumberOfFailures : 0, 1);

            equipStaticViewModel.EquipStaticChart = new EquipStaticChart();
            equipStaticViewModel.EquipStaticChart.EquipmentIntegrateStatisticInfoModelList
                = viewModelsDictionary.Values.ToList();

            List<EquipmentIntegrateStatisticViewModel> viewModels = CreateEquipmentIntegrateStatisticViewModels(viewModelsDictionary);

            equipStaticViewModel.EquipStaticMatrix = new EquipStaticMatrix();
            equipStaticViewModel.EquipStaticMatrix.EquipmentIntegrateStatisticViewModelList
                = viewModels;

            return equipStaticViewModel;
        }

        /// <summary>
        /// 生成设备综合统计数据
        /// </summary>
        /// <param name="viewModelsDictionary"></param>
        /// <returns></returns>
        private List<EquipmentIntegrateStatisticViewModel> CreateEquipmentIntegrateStatisticViewModels(
            Dictionary<string, EquipmentIntegrateStatisticInfoModel> viewModelsDictionary)
        {
            List<EquipmentIntegrateStatisticViewModel> viewModels
                = new List<EquipmentIntegrateStatisticViewModel>();
            foreach (var infoModel in viewModelsDictionary.Values.ToList())
            {
                infoModel.UtilizationRate = Math.Round((infoModel.PlanningTime != 0 ? infoModel.RunningTime / infoModel.PlanningTime : 0) * 100, 1);
                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 1,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.PlanningTime, 1).ToString(),
                    ValueTitle = "计划时间（h)".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 2,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.RunningTime, 1).ToString(),
                    ValueTitle = "运行时长（h）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 3,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.StandbyTime, 1).ToString(),
                    ValueTitle = "停机时长（h）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 4,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.DownTime, 1).ToString(),
                    ValueTitle = "关机时长（h）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 5,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.FailureTime, 1).ToString(),
                    ValueTitle = "故障时长（h）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 6,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.UnkownTime, 1).ToString(),
                    ValueTitle = "未知时长（h）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 7,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.OfflineTime, 1).ToString(),
                    ValueTitle = "设备离线时长（h）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 8,
                    StatisticDate = infoModel.StatisticDate,
                    Value = infoModel.NumberOfFailures.ToString(),
                    ValueTitle = "故障次数（次）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 9,
                    StatisticDate = infoModel.StatisticDate,
                    Value = Math.Round(infoModel.RepairTime, 1).ToString(),
                    ValueTitle = "维修时长（h）".L10N()
                });

                viewModels.Add(new EquipmentIntegrateStatisticViewModel()
                {
                    IndexSeq = 10,
                    StatisticDate = infoModel.StatisticDate,
                    Value = infoModel.UtilizationRate.ToString() + "%",
                    ValueTitle = "设备利用率".L10N()
                });
            }

            return viewModels;
        }

        /// <summary>
        /// 获取设备综合统计数据
        /// </summary>
        /// <param name="equipAccountIds">设备ID列表</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<EquipmentIntegrateStatistic> GetStatistics(List<double> equipAccountIds, DateTime startDate, DateTime EndDate)
        {
            var dateTimeOfCurrent = RF.Find<EquipAccount>().GetDbTime().Date;

            if (!equipAccountIds.Any())
            {
                return new EntityList<EquipmentIntegrateStatistic>();
            }

            //根据日历方案算出点检计划白班的开始时间和结束时间
            CalendarScheme calendarScheme = RT.Service.Resolve<CheckController>().GetCalendarScheme();

            var dicInfo = RT.Service.Resolve<CalendarSchemeController>()
               .GetShiftTypesByCalendarSchemeAndDataRange(calendarScheme, startDate, EndDate, allowNullShiftType: true);

            //设备运行状态记录
            var runningStateRecords = RT.Service.Resolve<EquipController>()
               .GetEquipRunningStateRecords(equipAccountIds, startDate, EndDate);

            var runningStateRecordsDictionary = runningStateRecords.GroupBy(x => x.EquipAccountId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.AtWhatTime).ToList());

            EntityList<EquipmentIntegrateStatistic> statistics = new EntityList<EquipmentIntegrateStatistic>();
            for (DateTime currentDate = startDate.Date; currentDate < EndDate.Date; currentDate = currentDate.AddDays(1))
            {
                if (currentDate > dateTimeOfCurrent)
                {
                    break;
                }

                decimal planHours = ComputePlanHours(dicInfo, currentDate);

                DateTime startTimeOfCurrentDate = currentDate;
                DateTime endTimeOfCurrentDate = currentDate.AddDays(1);

                foreach (var equipAccountId in equipAccountIds)
                {
                    EquipmentIntegrateStatistic equipmentIntegrateStatistic = new EquipmentIntegrateStatistic();
                    equipmentIntegrateStatistic.StatisticDate = currentDate;
                    equipmentIntegrateStatistic.EquipAccountId = equipAccountId;
                    equipmentIntegrateStatistic.PlanningTime = planHours;
                    statistics.Add(equipmentIntegrateStatistic);

                    if (runningStateRecordsDictionary.ContainsKey(equipAccountId))
                    {
                        var runningStateRecordsOfCurrent = runningStateRecordsDictionary[equipAccountId];

                        var prevStateRecord = runningStateRecordsOfCurrent
                            .OrderByDescending(x => x.AtWhatTime).FirstOrDefault(x => x.AtWhatTime <= startTimeOfCurrentDate);

                        if (prevStateRecord == null)
                        {
                            //默认运行状态为 未知
                            prevStateRecord = new EquipRunningStateRecord()
                            {
                                AtWhatTime = startTimeOfCurrentDate,
                                EquipRunningState = EquipRunningState.Unknown,
                                EquipOnLineState = EquipOnLineState.OffLine
                            };
                        }

                        var runningStateRecordsOfCurrentDate = runningStateRecordsOfCurrent
                            .Where(x => x.AtWhatTime > startTimeOfCurrentDate && x.AtWhatTime <= endTimeOfCurrentDate).ToList();

                        foreach (var runningStateRecord in runningStateRecordsOfCurrentDate)
                        {
                            if (prevStateRecord.AtWhatTime < startTimeOfCurrentDate)
                                prevStateRecord.AtWhatTime = startTimeOfCurrentDate;

                            ComputeRunningTime(equipmentIntegrateStatistic, prevStateRecord,
                                runningStateRecord);

                            prevStateRecord = runningStateRecord;
                        }

                        //计算最后一个状态到当天结束
                        if (prevStateRecord.AtWhatTime < startTimeOfCurrentDate)
                            prevStateRecord.AtWhatTime = startTimeOfCurrentDate;
                        ComputeRunningTime(equipmentIntegrateStatistic, prevStateRecord,
                               new EquipRunningStateRecord() { AtWhatTime = endTimeOfCurrentDate });
                    }
                    else
                    {
                        //默认运行状态为 未知
                        var prevStateRecord = new EquipRunningStateRecord()
                        {
                            AtWhatTime = startTimeOfCurrentDate,
                            EquipRunningState = EquipRunningState.Unknown,
                            EquipOnLineState = EquipOnLineState.OffLine
                        };

                        //计算最后一个状态到当天结束
                        ComputeRunningTime(equipmentIntegrateStatistic, prevStateRecord,
                               new EquipRunningStateRecord() { AtWhatTime = currentDate.AddDays(1) });
                    }
                }

                if (statistics.Any())
                {
                    var ids = equipAccountIds.Select(m => (double?)m).ToList();
                    ComputeRepairTime(startTimeOfCurrentDate, endTimeOfCurrentDate, ids, statistics.Last());
                }
            }

            return statistics;
        }

        /// <summary>
        /// 计算计划时长
        /// </summary>
        /// <param name="dicInfo"></param>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        private decimal ComputePlanHours(Dictionary<DateTime, Resources.ShiftTypes.ShiftType> dicInfo, DateTime currentDate)
        {
            decimal planHours = 0;

            if (dicInfo.ContainsKey(currentDate))
            {
                var shiftType = dicInfo[currentDate];
                foreach (var shift in shiftType.ShiftList)
                {
                    DateTime dateTimeStart = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day,
                        shift.BeginTime.Hour, shift.BeginTime.Minute, shift.BeginTime.Second);

                    DateTime dateTimeEnd = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day,
                        shift.EndTime.Hour, shift.EndTime.Minute, shift.EndTime.Second);

                    if (shift.IsOverDay)
                    {
                        dateTimeEnd = new DateTime(currentDate.AddDays(1).Year, currentDate.AddDays(1).Month,
                            currentDate.AddDays(1).Day, shift.EndTime.Hour, shift.EndTime.Minute, shift.EndTime.Second);
                    }

                    var totalHours = (decimal)(dateTimeEnd - dateTimeStart).TotalHours;
                    decimal totalRestHours = 0;

                    foreach (var shiftRest in shift.ShiftRestList)
                    {
                        totalRestHours += (decimal)(shiftRest.EndTime - shiftRest.BeginTime).TotalHours;
                    }

                    planHours += Math.Round(totalHours - totalRestHours, 1);
                }
            }

            return planHours;
        }

        /// <summary>
        /// 计算设备故障总时长
        /// </summary>
        /// <param name="startTimeOfCurrentDate"></param>
        /// <param name="endTimeOfCurrentDate"></param>
        /// <param name="accountIds"></param>
        /// <param name="equipmentIntegrateStatistic"></param>
        private void ComputeRepairTime(DateTime startTimeOfCurrentDate, DateTime endTimeOfCurrentDate, List<double?> accountIds, EquipmentIntegrateStatistic equipmentIntegrateStatistic)
        {
            var faultTimeTotal = 0d;
            var repairTimeTotal = 0d;
            //维修单记录
            var repairList = accountIds.SplitContains(tempIds =>
            {
                return Query<EquipRepairBill>().Where(p => tempIds.Contains(p.EquipAccountId) && p.RepairState != EquipRepairState.Cancel
                && p.ApplyRepairDate >= startTimeOfCurrentDate && p.ApplyRepairDate <= endTimeOfCurrentDate).ToList();
            });

            //维修单操作记录
            var repairOperList = accountIds.SplitContains(tempIds =>
            {
                return Query<EquipRepairOperationRec>().Where(p => tempIds.Contains(p.EquipRepairBill.EquipAccountId)
                && p.EquipRepairBill.RepairState != EquipRepairState.Cancel
                && p.EquipRepairBill.ApplyRepairDate >= startTimeOfCurrentDate
                && p.EquipRepairBill.ApplyRepairDate <= endTimeOfCurrentDate).ToList();
            });

            var computeList = repairList.Where(p => p.ApplyRepairDate >= startTimeOfCurrentDate && p.ApplyRepairDate <= endTimeOfCurrentDate).ToList();
            foreach (var item in computeList)
            {
                var operationRecords = repairOperList.Where(p => p.EquipRepairBillId == item.Id).OrderByDescending(p => p.OperationDate).ToList();
                DateTime? lastTime = null;
                lastTime = operationRecords.FirstOrDefault(p => p.OperationType == RepairOperationType.HandoverConfirm)?.OperationDate;
                if (lastTime == null)
                {
                    lastTime = operationRecords.FirstOrDefault(p => p.OperationType == RepairOperationType.Completed)?.OperationDate;
                }
                if (lastTime == null)
                {
                    lastTime = DateTime.Now;
                }
                faultTimeTotal += ((DateTime)lastTime - item.ApplyRepairDate).TotalHours;//单位：小时
                if (item.RepairBeginDate != null)
                {
                    repairTimeTotal += ((DateTime)lastTime - (DateTime)item.RepairBeginDate).TotalHours;//单位：小时
                    var arr = operationRecords.Where(p => p.OperationType == RepairOperationType.Pause || p.OperationType == RepairOperationType.Continue)
                        .OrderBy(p => p.OperationDate).ToList();
                    if (arr.Count % 2 != 0)
                    {
                        arr.Add(new EquipRepairOperationRec() { OperationDate = DateTime.Now });//已暂停，但未重新开始
                    }
                    for (int j = 0; j < arr.Count / 2; j += 2)
                    {
                        repairTimeTotal -= (arr[j + 1].OperationDate - arr[j].OperationDate).TotalHours;
                    }
                }
                else
                {
                    repairTimeTotal += 0;
                }

            }
            equipmentIntegrateStatistic.NumberOfFailures = computeList.Count;
            equipmentIntegrateStatistic.RepairTime = (decimal)repairTimeTotal;
            equipmentIntegrateStatistic.EquipMentFailureTime = (decimal)faultTimeTotal;
        }

        /// <summary>
        /// 计算时长
        /// </summary>
        /// <param name="equipmentIntegrateStatistic">统计结果</param>
        /// <param name="prevStateRecord">前一状态记录</param>
        /// <param name="runningStateRecord">后一状态记录</param>
        private void ComputeRunningTime(EquipmentIntegrateStatistic equipmentIntegrateStatistic,
                EquipRunningStateRecord prevStateRecord,
                EquipRunningStateRecord runningStateRecord)
        {
            var totalHours = (decimal)(runningStateRecord.AtWhatTime - prevStateRecord.AtWhatTime).TotalHours;
            switch (prevStateRecord.EquipRunningState)
            {
                case EquipRunningState.Unknown:
                    equipmentIntegrateStatistic.UnkownTime += totalHours;
                    break;
                case EquipRunningState.Running:
                    equipmentIntegrateStatistic.RunningTime += totalHours;
                    break;
                case EquipRunningState.Halted://停机
                    equipmentIntegrateStatistic.StandbyTime += totalHours;
                    break;
                case EquipRunningState.Breakdown://故障
                    equipmentIntegrateStatistic.FailureTime += totalHours;
                    break;
                case EquipRunningState.Shutdowned://关机
                    equipmentIntegrateStatistic.DownTime += totalHours;
                    break;
                default:
                    break;
            }

            if (prevStateRecord.EquipOnLineState == EquipOnLineState.OffLine)
            {
                equipmentIntegrateStatistic.OfflineTime += totalHours > 24 ? totalHours - 24 : totalHours;
            }
        }
    }
}
