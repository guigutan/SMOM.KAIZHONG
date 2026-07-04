using SIE.Api;
using SIE.Core;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.API.APIModels;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.AlarmStates;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.Equipments.RunningStates;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Maintains.Plans;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.DeviceIOTParas.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.EventMessages.EAP.Equipments;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.API.APIs
{
    /// <summary>
    /// 设备API 
    /// </summary>
    [ApiName("EquipmentController")]
    public partial class EquipmentController : DomainController
    {
        private const string EQUIPMENT_CODE_IS_EMPTY = "设备编码不能为空！";
        private const string TIME_QUANTUM_KEY = "timeQuantum";
        private const string START_TIME_KEY = "startTime";
        private const string END_TIME_KEY = "endTime";

        /// <summary>
        /// 获取当前设备的物联参数
        /// </summary>
        /// <returns></returns>
        [ApiService("获取当前设备的物联参数")]
        [return: ApiReturn("设备的物联参数 EquipEapRTValuePara")]
        public virtual EquipRTValuePara GetEquipmentParameters([ApiParameter("设备编码")] string equipCode)
        {
            if (equipCode == null)
            {
                throw new ValidationException(EQUIPMENT_CODE_IS_EMPTY.L10N());
            }
            var equipAccount = Query<EquipAccount>().Where(p => p.Code == equipCode).FirstOrDefault();

            var equipEapRTValuePara = new EquipRTValuePara()
            {
                EquipmentCode = equipCode,
                Paras = new Dictionary<string, string>()
            };

            if (equipAccount != null)
            {
                var equipAccountPhysicalUnionList = RT.Service.Resolve<DeviceIOTParaController>().GetPhysicalUnions(equipAccount.Id, new List<OrderInfo>(), null)
                    .Where(p => p.Enable && p.From == FromType.Interface && p.MDCVariableName != null);

                foreach (var item in equipAccountPhysicalUnionList)
                {
                    equipEapRTValuePara.Paras.Add(item.PararCode, item.ParaName);
                }
            }

            return equipEapRTValuePara;
        }

        /// <summary>
        /// 获取当前设备物联监控信息
        /// </summary>
        /// <returns></returns>
        [ApiService("获取当前设备物联监控信息")]
        [return: ApiReturn("设备参数列表 List<EquipmentParameter>")]
        public virtual List<EquipmentParameter> GetEquipmentParametersRealTimeData([ApiParameter("设备实时值参数")] EquipRTValuePara equipRTValuePara)
        {
            if (equipRTValuePara == null)
            {
                return new List<EquipmentParameter>();
            }

            if (equipRTValuePara.EquipmentCode == null)
            {
                throw new ValidationException(EQUIPMENT_CODE_IS_EMPTY.L10N());
            }

            //如果equipEapRTValuePara的设备参数为空，则表示拿全部的设备参数
            if (equipRTValuePara.Paras == null || equipRTValuePara.Paras.Count == 0)
            {
                equipRTValuePara = GetEquipmentParameters(equipRTValuePara.EquipmentCode);
            }

            //获取指定设备指定参数名称的物联参数(目的是为了拿到单位)
            var equipAccountPhysicalUnionList = Query<PhysicalUnion>().Where(p => equipRTValuePara.Paras.Keys.Contains(p.PararCode)).ToList();

            #region 构造返回值
            var list = new List<EquipmentParameter>();
            var para = new EquipEapRTValuePara()
            {
                EquipmentCode = equipRTValuePara.EquipmentCode,
                Paras = equipRTValuePara.Paras.Keys.ToList()
            };

            try
            {
                var rtn = RT.Service.Resolve<IEquipmentEap>().GetEquipEapRTValueInfo(para);
                if (rtn?.Data != null && rtn.Data.Count > 0)
                {
                    foreach (var item in equipRTValuePara.Paras)
                    {
                        list.Add(new EquipmentParameter()
                        {
                            Code = item.Key,
                            Name = item.Value,
                            RealTimeValue = rtn.Data.FirstOrDefault(x => x.Tag == item.Key).Value,
                            Unit = equipAccountPhysicalUnionList.Where(p => p.PararCode == item.Key).Select(p => p.Unit).FirstOrDefault()
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw new ValidationException("接口异常：".L10N()+"从MDC获取实时值失败!".L10N());
            }
            #endregion

            return list;
        }

        /// <summary>
        /// 获取指定设备某个时间段的运行状态
        /// </summary>
        /// <returns></returns>
        [ApiService("获取指定设备某个时间段的运行状态")]
        [return: ApiReturn("设备运行状态列表 List<EquipmentRunningStateInfo>")]
        public virtual List<EquipmentRunningStateInfo> GetEquipRunningStateRecordList([ApiParameter("设备编码")] string equipmentCode, [ApiParameter("最近多长时间的记录(小时)")] int hour = 12, [ApiParameter("记录状态间隔(秒)")] int second = 1)
        {
            if (equipmentCode == null)
            {
                throw new ValidationException(EQUIPMENT_CODE_IS_EMPTY.L10N());
            }

            var toTime = RF.Find<EquipRunningStateRecord>().GetDbTime();
            var fromTime = toTime.AddHours(-hour);

            return GetEquipRunningStateRecordList1(equipmentCode, fromTime, toTime, second);
        }

        /// <summary>
        /// 获取指定设备某个时间段的运行状态
        /// </summary>
        /// <returns></returns>
        [ApiService("获取指定设备某个时间段的运行状态")]
        [return: ApiReturn("设备运行状态列表 List<EquipmentRunningStateInfo>")]
        public virtual List<EquipmentRunningStateInfo> GetEquipRunningStateRecordList1([ApiParameter("设备编码")] string equipmentCode, [ApiParameter("开始时间")] DateTime fromTime, [ApiParameter("结束时间")] DateTime toTime, [ApiParameter("记录状态间隔(秒)")] int second = 1)
        {
            if (equipmentCode == null)
            {
                throw new ValidationException(EQUIPMENT_CODE_IS_EMPTY.L10N());
            }

            var equipAccount = Query<EquipAccount>().Where(p => p.Code == equipmentCode).FirstOrDefault();

            if (equipAccount == null)
            {
                throw new ValidationException("该编码对应的设备不存在！".L10N());
            }

            if (fromTime > toTime)
            {
                throw new ValidationException("开始时间不能大于结束时间！".L10N());
            }

            var leftTime = fromTime;
            var rightTime = leftTime;
            var lastTime = leftTime;

            var list = new List<EquipmentRunningStateInfo>();

            //找开始时间及之前的最后一条数据作为第一条数据
            var firstItem = Query<EquipRunningStateRecord>()
                .Where(p => p.EquipAccountId == equipAccount.Id && p.AtWhatTime <= fromTime).OrderByDescending(p => p.AtWhatTime)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();

            //找时间区间内的数据
            var qList = Query<EquipRunningStateRecord>()
                .Where(p => p.EquipAccountId == equipAccount.Id && p.AtWhatTime > fromTime && p.AtWhatTime < toTime).OrderBy(p => p.AtWhatTime)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            for (int i = 0; i < (toTime - fromTime).TotalSeconds / second; i++)
            {
                if (list.Count == 0)
                {
                    if (firstItem == null)
                    {
                        list.Add(new EquipmentRunningStateInfo()
                        {
                            Id = equipAccount.Id,
                            Code = equipAccount.Code,
                            Name = equipAccount.Name,
                            State = EquipRunningState.Unknown,
                            Time = second < 60 ? leftTime.ToString("G") : leftTime.ToString("g")
                        });
                    }
                    else
                    {
                        list.Add(new EquipmentRunningStateInfo()
                        {
                            Id = equipAccount.Id,
                            Code = equipAccount.Code,
                            Name = equipAccount.Name,
                            State = firstItem.EquipRunningState,
                            Time = second < 60 ? leftTime.ToString("G") : leftTime.ToString("g")
                        });
                    }
                    rightTime = rightTime.AddSeconds(second);
                    lastTime = leftTime;

                    continue;
                }

                leftTime = leftTime.AddSeconds(second);
                rightTime = rightTime.AddSeconds(second);

                if (qList == null || qList.Count == 0)
                {
                    list.Add(new EquipmentRunningStateInfo()
                    {
                        Id = list.FirstOrDefault().Id,
                        Code = list.FirstOrDefault().Code,
                        Name = list.FirstOrDefault().Name,
                        State = list.FirstOrDefault().State,
                        Time = second < 60 ? leftTime.ToString("G") : leftTime.ToString("g")
                    });
                    continue;
                }

                var firstOfThese = qList.FirstOrDefault(p => p.AtWhatTime >= leftTime && p.AtWhatTime < rightTime);
                if (firstOfThese == null)
                {
                    var lastOne = list.Find(p => p.Time == (second < 60 ? lastTime.ToString("G") : lastTime.ToString("g")));

                    list.Add(new EquipmentRunningStateInfo()
                    {
                        Id = lastOne.Id,
                        Code = lastOne.Code,
                        Name = lastOne.Name,
                        State = lastOne.State,
                        Time = second < 60 ? leftTime.ToString("G") : leftTime.ToString("g")
                    });
                }
                else
                {
                    list.Add(new EquipmentRunningStateInfo()
                    {
                        Id = firstOfThese.EquipAccountId,
                        Code = firstOfThese.EquipAccountCode,
                        Name = firstOfThese.EquipAccountName,
                        State = firstOfThese.EquipRunningState,
                        Time = second < 60 ? leftTime.ToString("G") : leftTime.ToString("g")
                    });
                }

                lastTime = leftTime;
            }

            return list;
        }

        /// <summary>
        /// 获取指定设备某个时间段的运行状态时长
        /// </summary>
        /// <returns></returns>
        [ApiService("获取指定设备某个时间段的运行状态总时长")]
        [return: ApiReturn("设备运行状态列表 List<EquipmentRunningStateInfo>")]
        public virtual EquipmentRunningStateTotalTime GetEquipRunningStateTotalTime([ApiParameter("设备编码")] string equipmentCode, [ApiParameter("统计开始时间")] DateTime fromTime, [ApiParameter("统计结束时间")] DateTime toTime)
        {
            if (equipmentCode == null)
            {
                throw new ValidationException(EQUIPMENT_CODE_IS_EMPTY.L10N());
            }

            //拿到该设备实体
            var equipAccount = Query<EquipAccount>().Where(p => p.Code == equipmentCode).FirstOrDefault();
            if (equipAccount == null)
            {
                throw new ValidationException("找不到设备：".L10N() + equipmentCode);
            }

            var qList = Query<EquipRunningStateRecord>()
                .Where(p => p.EquipAccount.Code == equipmentCode && p.AtWhatTime >= fromTime && p.AtWhatTime < toTime).OrderBy(p => p.AtWhatTime)
                //.OrderBy(p => p.AtWhatTime)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var qArr = qList.ToArray();
            var dic = new Dictionary<int, double>();

            //以EquipRunningState的5个状态值为key，构建该设备的状态Dictionary，初始时长为0。
            for (int i = 0; i < 5; i++)
            {
                dic.Add(i, 0);
            }

            //遍历qArr统计每个状态的时长，前后相邻的两条记录的时间间隔为前一条记录的状态的持续时间，
            for (int i = 1; i < qArr.Length; i++)
            {
                dic[(int)qArr[i - 1].EquipRunningState] += (qArr[i].AtWhatTime - qArr[i - 1].AtWhatTime).TotalSeconds;
            }

            //构建该设备的返回值结构
            var entity = new EquipmentRunningStateTotalTime()
            {
                Id = equipAccount.Id,
                Code = equipAccount.Code,
                Name = equipAccount.Name,
                States = new List<TotalTimeOfState>()
            };

            #region 求所有状态的总时长，目的是为了求占比
            double statesTotalTime = 0;
            foreach (var item in dic)
            {
                statesTotalTime += item.Value;
            }
            #endregion

            //遍历状态字典里的元素，填充返回值的状态列表。
            foreach (var item in dic)
            {
                entity.States.Add(new TotalTimeOfState()
                {
                    State = (EquipRunningState)item.Key,
                    StateName = ((EquipRunningState)item.Key).ToLabel(),
                    TotalSeconds = item.Value,
                    TotalHours = Math.Round(item.Value / 3600, 2).ToString(),
                    Percent = (statesTotalTime == 0 ? 0.ToString() : Math.Round(item.Value * 100 / statesTotalTime, 2).ToString()) + "%"
                });
            }

            return entity;
        }

        /// <summary>
        /// 查询维修单汇总概况
        /// </summary>
        /// <returns></returns>
        [ApiService("查询维修单汇总概况")]
        [return: ApiReturn("维修单汇总信息 Dictionary<int, int>,key是维修状态，value是数量")]
        public virtual List<RepairBillsOverviewResult> GetRepairBillsOverview(
            [ApiParameter("设备编码")] string equipAccountCode,
            [ApiParameter("维修状态对应的路径，ApplyRepairPath：报修，WaitRepairPath：待维修，Repairing：维修中，Suspending：暂停，WaitConfirm：待确认，WaitScore：工程评分")] RepairBillsOverviewParas Paths
            )
        {
            var equipRepairBills = Query<EquipRepairBill>()
                .Where(p => p.EquipAccount.Code == equipAccountCode)
                .ToList();

            #region 维修记录
            //报修的记录
            var repairRecords_ApplyRepair = equipRepairBills.Where(p => p.RepairState == EquipRepairState.ApplyRepair).ToList();

            //待维修的记录
            var repairRecords_WaitRepair = equipRepairBills.Where(p => p.RepairState == EquipRepairState.WaitRepair).ToList();

            //维修中的记录
            var repairRecords_Repairing = equipRepairBills.Where(p => p.RepairState == EquipRepairState.Repairing).ToList();

            //暂停的记录
            var repairRecords_Suspending = equipRepairBills.Where(p => p.RepairState == EquipRepairState.Suspending).ToList();

            //待确认的记录
            var repairRecords_WaitConfirm = equipRepairBills.Where(p => p.RepairState == EquipRepairState.WaitConfirm).ToList();

            //待评分的记录
            var repairRecords_WaitScore = equipRepairBills.Where(p => p.RepairState == EquipRepairState.WaitScore).ToList();

            #endregion

            var list = new List<RepairBillsOverviewResult>();

            list.Add(new RepairBillsOverviewResult()
            {
                State = EquipRepairState.ApplyRepair,
                Name = EquipRepairState.ApplyRepair.ToLabel(),
                Path = Paths.ApplyRepairPath,
                Count = repairRecords_ApplyRepair.FirstOrDefault() == null ? 0 : repairRecords_ApplyRepair.Count
            });

            list.Add(new RepairBillsOverviewResult()
            {
                State = EquipRepairState.WaitRepair,
                Name = EquipRepairState.WaitRepair.ToLabel(),
                Path = Paths.WaitRepairPath,
                Count = repairRecords_WaitRepair.FirstOrDefault() == null ? 0 : repairRecords_WaitRepair.Count
            });

            list.Add(new RepairBillsOverviewResult()
            {
                State = EquipRepairState.Repairing,
                Name = EquipRepairState.Repairing.ToLabel(),
                Path = Paths.RepairingPath,
                Count = repairRecords_Repairing.FirstOrDefault() == null ? 0 : repairRecords_Repairing.Count
            });

            list.Add(new RepairBillsOverviewResult()
            {
                State = EquipRepairState.Suspending,
                Name = EquipRepairState.Suspending.ToLabel(),
                Path = Paths.SuspendingPath,
                Count = repairRecords_Suspending.FirstOrDefault() == null ? 0 : repairRecords_Suspending.Count
            });

            list.Add(new RepairBillsOverviewResult()
            {
                State = EquipRepairState.WaitConfirm,
                Name = EquipRepairState.WaitConfirm.ToLabel(),
                Path = Paths.WaitConfirmPath,
                Count = repairRecords_WaitConfirm.FirstOrDefault() == null ? 0 : repairRecords_WaitConfirm.Count
            });

            list.Add(new RepairBillsOverviewResult()
            {
                State = EquipRepairState.WaitScore,
                Name = EquipRepairState.WaitScore.ToLabel(),
                Path = Paths.WaitScorePath,
                Count = repairRecords_WaitScore.FirstOrDefault() == null ? 0 : repairRecords_WaitScore.Count
            });

            return list;
        }

        /// <summary>
        /// 同步设备报警信息
        /// </summary>
        /// <returns></returns>
        [ApiService("同步设备报警信息")]
        public virtual void SyncEquipAlarmStateRecord()
        {
            RT.Service.Resolve<EquipController>().SyncEquipAlarmRecord();
        }

        /// <summary>
        /// 获取设备故障统计报告
        /// </summary>
        /// <returns></returns>
        [ApiService("获取设备故障统计报告")]
        [return: ApiReturn("设备故障统计报告 List<EquipmentFaultStatistics>")]
        public virtual List<EquipmentFaultStatistics> GetFaultStatisticsReport([ApiParameter("设备编码")] string equipmentCode)
        {
            var dic = new Dictionary<int, EquipmentFaultStatistics>();
            var now = RF.Find<EquipRepairBill>().GetDbTime();
            var currenYear = now.Year;
            var currenMonth = now.Month;

            for (int i = 1; i <= 12; i++)
            {
                dic.Add(i, new EquipmentFaultStatistics()
                {
                    Month = currenYear + "-" + i
                });
            }

            for (int i = 1; i <= currenMonth; i++)
            {
                var firstDate = DateTime.Parse(currenYear.ToString() + "-" + i.ToString()).Date;
                var list = Query<EquipRepairBill>().Where(p => p.EquipAccount.Code == equipmentCode && p.RepairState != EquipRepairState.Cancel && p.ApplyRepairDate >= firstDate && p.ApplyRepairDate < firstDate.AddMonths(1)).ToList();
                dic[i].Count = list.Count();

                var list1 = list.Where(p => (p.RepairState == EquipRepairState.Completed || p.RepairState == EquipRepairState.WaitScore) && p.ProduceState == ProduceState.StopWork).ToList();
                double faultTimeTotal = 0;
                foreach (var item in list1)
                {
                    var operationRecords = Query<EquipRepairOperationRec>().Where(p => p.EquipRepairBillId == item.Id && (p.OperationType == RepairOperationType.HandoverConfirm || p.OperationType == RepairOperationType.Completed)).OrderByDescending(p => p.OperationDate).ToList();

                    #region 故障时长
                    var lastOperationRecord = operationRecords.FirstOrDefault(p => p.OperationType == RepairOperationType.HandoverConfirm);
                    if (lastOperationRecord == null)
                    {
                        lastOperationRecord = operationRecords.FirstOrDefault(p => p.OperationType == RepairOperationType.Completed);
                    }
                    faultTimeTotal += (lastOperationRecord.OperationDate - item.ApplyRepairDate).TotalSeconds;//单位：秒
                    #endregion

                    #region 维修时长
                    dic[i].RepairTimeTotal = faultTimeTotal;//单位：秒
                    var arr = operationRecords.Where(p => p.OperationType == RepairOperationType.Pause || p.OperationType == RepairOperationType.Continue).OrderBy(p => p.OperationDate).ToArray();
                    for (int j = 0; j < arr.Length / 2; j += 2)
                    {
                        dic[i].RepairTimeTotal -= (arr[j + 1].OperationDate - arr[j].OperationDate).TotalSeconds;
                    }

                    dic[i].RepairTimeTotal = Math.Round(dic[i].RepairTimeTotal / 60);//单位：分钟
                    #endregion
                }

                var states = GetEquipRunningStateTotalTime(equipmentCode, firstDate, firstDate.AddMonths(1))?.States;
                dic[i].Mtbf = dic[i].Count == 0 ? 0 : Math.Round(states.FirstOrDefault(p => p.State == EquipRunningState.Running).TotalSeconds / 3600 / dic[i].Count);//运行时长/故障次数
                dic[i].Mttr = dic[i].Count == 0 ? 0 : faultTimeTotal / dic[i].Count;//故障时长/故障次数
            }

            return dic.Values.ToList();
        }

        /// <summary>
        /// 获取设备运营成本分析报告
        /// </summary>
        /// <returns></returns>
        [ApiService("获取设备运营成本分析报告")]
        [return: ApiReturn("设备运营成本分析报告 List<EquipmentCostAnalysis>")]
        public virtual Dictionary<string, List<EquipmentCostAnalysis>> GetCostAnalysisReport([ApiParameter("设备编码")] string equipmentCode, [ApiParameter("分析开始时间")] DateTime fromTime, [ApiParameter("分析结束时间")] DateTime toTime)
        {
            var equipRepairBillList = Query<EquipRepairBill>().Where(p => p.EquipAccount.Code == equipmentCode && p.RepairBeginDate >= fromTime && p.RepairBeginDate < toTime).ToList();
            var equipment = Query<EquipAccount>().Where(m => m.Code == equipmentCode).FirstOrDefault();
            if (equipment == null)
            {
                return new Dictionary<string, List<EquipmentCostAnalysis>>();
            }

            var equipRepairBillIds = equipRepairBillList.Select(y => y.Id);
            var repairCost = new EquipmentCostAnalysis()
            {
                CostName = "维修工时成本".L10N(),
                Cost = (Decimal)(Query<EquipRepairWorkingHours>().Where(p => equipRepairBillIds.Contains(p.EquipRepairBillId) && p.BeginTime != null && p.EndTime != null && p.EndTime >= p.BeginTime).ToList().Sum(p => Math.Round(((TimeSpan)(p.EndTime - p.BeginTime)).TotalHours, 2))),
                Unit = "小时".L10N()
            };

            var maintainPlanIds = Query<MaintainPlan>().Where(p => p.EquipAccount.Code == equipmentCode && p.ActBeginDate >= fromTime && p.ActBeginDate < toTime).ToList();
            var maintainCost = new EquipmentCostAnalysis()
            {
                CostName = "保养工时成本".L10N(),
                Cost = maintainPlanIds.Any() ? decimal.Round((decimal)maintainPlanIds.Sum(m => m.SumWorkHours), 2) : 0m,
                Unit = "小时".L10N()
            };

            var outsourceList = equipRepairBillList.Where(p => p.RepairWay == EquipRepairWay.OuterRepair && p.RepairCosts != null);
            var outsourceCost = new EquipmentCostAnalysis()
            {
                CostName = "委外维修成本".L10N(),
                Cost = decimal.Round((decimal)outsourceList.Sum(p => p.RepairCosts), 2),
                Unit = "元".L10N()
            };

            var equipRepairSparePartChgList = Query<EquipRepairSparePartChg>().Join<EquipRepairBill>((x, y) => x.EquipRepairBillId == y.Id && y.EquipAccountId == equipment.Id && y.UpdateDate >= fromTime && y.UpdateDate < toTime).ToList();
            var checkPlanSparePartList = Query<CheckPlanSparePart>().Join<CheckPlan>((x, y) => x.CheckPlanId == y.Id && y.EquipAccountId == equipment.Id && y.UpdateDate >= fromTime && y.UpdateDate < toTime).ToList();
            var maintainPlanSparePartList = Query<MaintainPlanSparePart>().Join<MaintainPlan>((x, y) => x.MaintainPlanId == y.Id && y.EquipAccountId == equipment.Id && y.UpdateDate >= fromTime && y.UpdateDate < toTime).ToList();
            var sparePartUnitCost = 0d;
            if (equipRepairSparePartChgList.Any())
            {
                equipRepairSparePartChgList.ForEach(item =>
                {
                    if (item.PartOutDepotDetail != null)
                        sparePartUnitCost += item.PartOutDepotDetail.UnitPrice;
                });
            }
            if (checkPlanSparePartList.Any())
            {
                checkPlanSparePartList.ForEach(item =>
                {
                    if (item.PartOutDepotDetail != null)
                        sparePartUnitCost += item.PartOutDepotDetail.UnitPrice;
                });
            }
            if (maintainPlanSparePartList.Any())
            {
                maintainPlanSparePartList.ForEach(item =>
                {
                    if (item.PartOutDepotDetail != null)
                        sparePartUnitCost += item.PartOutDepotDetail.UnitPrice;
                });
            }

            var sparePartCost = new EquipmentCostAnalysis()
            {
                CostName = "备件成本".L10N(),
                Cost = decimal.Round((decimal)sparePartUnitCost, 2),
                Unit = "元".L10N()
            };

            var energyConsumptionCost = new EquipmentCostAnalysis()
            {
                CostName = "能耗成本".L10N(),
                Cost = 0,
                Unit = "元".L10N()
            };

            var depreciationCost = new EquipmentCostAnalysis()
            {
                CostName = "折旧成本".L10N(),
                Cost = 0,
                Unit = "元".L10N()
            };

            var items = new List<EquipmentCostAnalysis>();
            items.Add(repairCost);
            items.Add(maintainCost);
            items.Add(outsourceCost);
            items.Add(sparePartCost);
            items.Add(energyConsumptionCost);
            items.Add(depreciationCost);

            var sums = new List<EquipmentCostAnalysis>();
            sums.Add(new EquipmentCostAnalysis()
            {
                CostName = "人事成本（小时）".L10N(),
                Cost = repairCost.Cost + maintainCost.Cost,
                Unit = "小时".L10N()
            });
            sums.Add(new EquipmentCostAnalysis()
            {
                CostName = "费用成本（元）".L10N(),
                Cost = decimal.Round(outsourceCost.Cost + sparePartCost.Cost + energyConsumptionCost.Cost + depreciationCost.Cost, 2),
                Unit = "元".L10N()
            });

            var dic = new Dictionary<string, List<EquipmentCostAnalysis>>();
            dic.Add("items", items);
            dic.Add("sums", sums);

            return dic;
        }

        /// <summary>
        /// 根据编码获取设备详细信息
        /// </summary>
        /// <param name="code">设备编码</param>
        /// <returns>设备详细信息</returns>
        [ApiService("根据编码获取设备详细信息")]
        [return: ApiReturn("设备详细信息 EquipTabDetailInfo")]
        public virtual EquipTabDetailInfo GetEquipTabDetailInfo([ApiParameter("设备编码")] string code)
        {
            var info = new EquipTabDetailInfo();
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccountsByCode(code);
            if (equip != null)
            {
                info.EquipId = equip.Id;
                info.Code = equip.Code;
                info.Name = equip.Name;
                info.AssetCode = equip.AssetCode;
                info.UseDepartment = equip.UseDepartmentName;
                info.EquipModel = equip.ModelName;
                info.EquipType = equip.EquipTypeName;
                info.WorkShop = equip.WorkShopName;
                info.ResourceName = equip.ResourceName;

                info.Location = equip.InstallationLocation;

                info.Process = equip.ProcessName;
                info.ResPerson = equip.ResPersonName;
                info.IOTState = equip.EquipOnLineState.ToLabel().L10N();
                info.IOTStateValue = equip.EquipOnLineState;
                info.AccountState = equip.State.ToLabel().L10N();
                info.AccountStateValue = equip.State;

                #region 实时从MDC取数据
                //try
                //{
                //    var runningState = RT.Service.Resolve<EquipmentSmdcController>().GetDeviceRunStateByAssetCode(new string[] { code });
                //    //MDC如果没有登记这个code，则取不到这个code的值。
                //    info.AccountState = ((EquipRunningState)runningState[code]).ToLabel();
                //}
                //catch
                //{
                //    info.AccountState = EquipRunningState.Unknown.ToLabel();
                //}

                //try
                //{
                //    var onLineState = RT.Service.Resolve<EquipmentSmdcController>().GetDeviceIsOnLineByAssetCode(new string[] { code });
                //    //MDC如果没有登记这个code，则取不到这个code的值。
                //    info.IOTState = onLineState[code] ? EquipOnLineState.OnLine.ToLabel() : EquipOnLineState.OffLine.ToLabel();
                //}
                //catch
                //{
                //    info.IOTState = EquipOnLineState.OffLine.ToLabel();
                //}
                #endregion

                var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
                var attachment = Query<EquipAccountAttachment>().Where(p => p.OwnerId == equip.Id && exts.Contains(p.FileExtesion))
                    .OrderByDescending(p => p.UpdateDate).FirstOrDefault();
                if (attachment != null && attachment.Content != null)
                {
                    var strPhoto = Convert.ToBase64String(attachment.Content);
                    var strSop = @"data:image/png;base64,";
                    strSop += strPhoto;
                    info.Picture = strSop;
                }

                #region 30天故障与报警次数
                var now = RF.Find<EquipRepairBill>().GetDbTime();
                var list = Query<EquipRepairBill>().Where(p => p.EquipAccountId == equip.Id && p.ApplyRepairDate >= now.Date.AddDays(-29) && p.RepairState != EquipRepairState.Cancel).ToList();
                info.FaultQty = list.Count();
                info.RepairQty = list.Count(p => p.RepairState == EquipRepairState.Repairing
                    || p.RepairState == EquipRepairState.Suspending
                    || p.RepairState == EquipRepairState.WaitConfirm
                    || p.RepairState == EquipRepairState.WaitScore
                    || p.RepairState == EquipRepairState.Completed
                    || p.RepairState == EquipRepairState.Closed);

                info.ShutDownQty = list.Count(p => p.ProduceState == ProduceState.StopWork);
                info.WarningQty = Query<EquipAlarmRecord>().Where(p => p.EquipAccountId == equip.Id && p.AlarmTime >= now.Date.AddDays(-29)).Count();
                #endregion

                #region TPM记录
                var count = RT.Service.Resolve<CheckController>().GetNotPerformedCheckPlansCount(new List<CheckExeState> { CheckExeState.NotPerformed }, equip.Id);
                info.CheckDisplay = count >= 1 ? "待点检".L10N() : "已点检".L10N();

                var deptIds = RT.Service.Resolve<DevicePurController>().GetDutyDepartments(RT.Identity.UserId).Select(p => p.Id).ToList<double>();
                var mpList = Query<MaintainPlan>().Where(p => p.ExeState == MaintExeState.NotPerformed && p.EquipAccount.Code == code && deptIds.Contains(p.Department.Id)).ToList();
                foreach (var item in mpList)
                {
                    if (item.PrecisePlanBeginDate != null)
                    {
                        item.PlanBeginDate = (DateTime)item.PrecisePlanBeginDate;
                    }
                }
                var firstItem = mpList.Where(p => p.PlanEndDate >= now).OrderBy(p => p.PlanBeginDate).FirstOrDefault();
                info.MaintainDisplay = "下次保养日期：".L10N() + (firstItem == null ? "无保养计划".L10N() : firstItem.PlanBeginDate.ToString("d"));

                info.RepairDisplay = list
                    .Any(p => p.RepairState == EquipRepairState.ApplyRepair
                        || p.RepairState == EquipRepairState.WaitRepair
                        || p.RepairState == EquipRepairState.Repairing
                        || p.RepairState == EquipRepairState.Suspending
                        || p.RepairState == EquipRepairState.WaitConfirm) ? "待维修".L10N() : null;
                if (info.RepairDisplay == "待维修".L10N())
                {
                    info.RepairDisplayValue = 1;
                }
                var lastAlarm = Query<EquipAlarmRecord>().Where(p => p.EquipAccountId == equip.Id).OrderByDescending(p => p.AlarmTime).FirstOrDefault()?.AlarmTime.ToString("G");
                info.WarningDisplay = "最近报警：".L10N() + (string.IsNullOrEmpty(lastAlarm) ? "无".L10N() : lastAlarm);
                #endregion
            }
            return info;
        }

        /// <summary>
        /// 获取时间段枚举
        /// </summary>
        /// <returns>时间段枚举</returns>
        [ApiService("获取时间段枚举")]
        [return: ApiReturn("时间段枚举 []")]
        public virtual List<Dictionary<string, string>> GetTimeQuantum()
        {
            var timeQuantums = new List<Dictionary<string, string>>();

            var now = RF.Find<Enterprise>().GetDbTime();

            var last30Days = new Dictionary<string, string>();
            last30Days.Add(TIME_QUANTUM_KEY, "最近30天".L10N());
            last30Days.Add(START_TIME_KEY, now.Date.AddDays(-29).ToString(DateTimeFormat.LongDateString2));
            last30Days.Add(END_TIME_KEY, now.Date.AddDays(1).ToString(DateTimeFormat.LongDateString2));

            var today = new Dictionary<string, string>();
            today.Add(TIME_QUANTUM_KEY, "今日".L10N());
            today.Add(START_TIME_KEY, now.Date.ToString(DateTimeFormat.LongDateString2));
            today.Add(END_TIME_KEY, now.Date.AddDays(1).ToString(DateTimeFormat.LongDateString2));

            var currentWeek = new Dictionary<string, string>();
            currentWeek.Add(TIME_QUANTUM_KEY, "本周".L10N());
            currentWeek.Add(START_TIME_KEY, now.Date.AddDays(Convert.ToDouble((0 - Convert.ToInt16(now.DayOfWeek)))).ToString(DateTimeFormat.LongDateString2));
            currentWeek.Add(END_TIME_KEY, now.Date.AddDays(6).ToString(DateTimeFormat.LongDateString2));

            var currentMonth = new Dictionary<string, string>();
            currentMonth.Add(TIME_QUANTUM_KEY, "本月".L10N());
            currentMonth.Add(START_TIME_KEY, DateTime.Parse(now.ToString("Y")).Date.ToString(DateTimeFormat.LongDateString2));
            currentMonth.Add(END_TIME_KEY, DateTime.Parse(now.ToString("Y")).Date.AddMonths(1).ToString(DateTimeFormat.LongDateString2));

            var last7Days = new Dictionary<string, string>();
            last7Days.Add(TIME_QUANTUM_KEY, "最近7天".L10N());
            last7Days.Add(START_TIME_KEY, now.Date.AddDays(-6).ToString(DateTimeFormat.LongDateString2));
            last7Days.Add(END_TIME_KEY, now.Date.AddDays(1).ToString(DateTimeFormat.LongDateString2));

            var last3Month = new Dictionary<string, string>();
            last3Month.Add(TIME_QUANTUM_KEY, "最近3个月".L10N());
            last3Month.Add(START_TIME_KEY, DateTime.Parse(currentMonth[START_TIME_KEY]).AddMonths(-2).ToString(DateTimeFormat.LongDateString2));
            last3Month.Add(END_TIME_KEY, DateTime.Parse(currentMonth[END_TIME_KEY]).ToString(DateTimeFormat.LongDateString2));

            var last6Month = new Dictionary<string, string>();
            last6Month.Add(TIME_QUANTUM_KEY, "最近6个月".L10N());
            last6Month.Add(START_TIME_KEY, DateTime.Parse(currentMonth[START_TIME_KEY]).AddMonths(-5).ToString(DateTimeFormat.LongDateString2));
            last6Month.Add(END_TIME_KEY, DateTime.Parse(currentMonth[END_TIME_KEY]).ToString(DateTimeFormat.LongDateString2));

            timeQuantums.Add(last30Days);
            timeQuantums.Add(today);
            timeQuantums.Add(currentWeek);
            timeQuantums.Add(currentMonth);
            timeQuantums.Add(last7Days);
            timeQuantums.Add(last3Month);
            timeQuantums.Add(last6Month);

            return timeQuantums;
        }

    }
}