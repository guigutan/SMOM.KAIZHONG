using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Maintains.Plans;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 设备成本分析控制器
    /// </summary>
    public class EquipCostAnalyseController : DomainController
    {
        /// <summary>
        /// 查询设备成本分析
        /// </summary>
        /// <param name="criteria">设备成本分析查询实体</param>
        /// <returns>设备成本分析</returns>
        public virtual EquipCostAnalysesInfo CriteriaEquipCostAnalyse(EquipCostAnalyseCriteria criteria)
        {
            var equipCostAnalysesInfo = new EquipCostAnalysesInfo();

            var query = Query<EquipAccount>();
            if (criteria.EquipTypeId.HasValue && criteria.EquipTypeId != 0)
            {
                query.Where(w => w.EquipModel.EquipTypeId == criteria.EquipTypeId);
            }

            if (criteria.EquipModelId.HasValue && criteria.EquipModelId != 0)
            {
                query.Where(w => w.EquipModelId == criteria.EquipModelId);
            }

            if (criteria.EquipAccountId.HasValue && criteria.EquipAccountId != 0)
            {
                query.Where(w => w.Id == criteria.EquipAccountId);
            }

            if (criteria.DepartmentId.HasValue && criteria.DepartmentId != 0)
            {
                query.Where(w => w.UseDepartmentId == criteria.DepartmentId);
            }

            if (criteria.WorkShopId.HasValue && criteria.WorkShopId != 0)
            {
                query.Where(w => w.WorkShopId == criteria.WorkShopId);
            }

            if (criteria.ResourceId.HasValue && criteria.ResourceId != 0)
            {
                query.Where(w => w.ResourceId == criteria.ResourceId);
            }

            if (!criteria.EquipName.IsNullOrEmpty() && !criteria.EquipAccountId.HasValue)
            {
                query.Where(w => w.Name == criteria.EquipName);
            }

            var equipments = query.ToList();
            if (!equipments.Any())
            {
                throw new ValidationException("查询无设备".L10N());
            }

            var equipmentIds = equipments.Select(p => p.Id).ToList();

            if (criteria.BeginMonth.HasValue && criteria.EndMonth.HasValue)
            {
                if ((int)criteria.BeginMonth > (int)criteria.EndMonth)
                {
                    throw new ValidationException("查询输入不正确,【开始月份】必须小于等于【结束月份】".L10N());
                }
            }

            if (!criteria.Year.HasValue)
            {
                throw new ValidationException("请输入年份!".L10N());
            }

            var startDate = new DateTime(criteria.Year.Value.Year, criteria.BeginMonth.HasValue ? (int)criteria.BeginMonth.Value : 1, 1);
            var endDate = new DateTime(criteria.Year.Value.Year, (criteria.EndMonth.HasValue ? (int)criteria.EndMonth.Value : 12), 1);

            var equipCostInfos = GetEquipCostInfoByMonth(equipmentIds, startDate, endDate.AddMonths(1));

            var equipCostInfosStatistics = GetEquipCostInfo(equipCostInfos);

            equipCostAnalysesInfo.MonthlyCostInfoList = GetMonthlyCostInfos(equipCostInfos);
            equipCostAnalysesInfo.EquipCostInfoInfo = equipCostInfosStatistics;
            equipCostAnalysesInfo.EquipmentCount = (equipCostInfosStatistics.Count > 0 ? equipCostInfosStatistics.Count : 0).ToString();
            return equipCostAnalysesInfo;

        }


        /// <summary>
        /// 获取所选的设备开始到结束的统计信息
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <returns></returns>
        private EntityList<EquipCostInfo> GetEquipCostInfoByMonth(List<double> equipmentIds, DateTime fromTime, DateTime toTime)
        {
            DateTime endTime = fromTime;
            var infos = new EntityList<EquipCostInfo>();
            var beginTime = fromTime;
            var equipments = GetEquipAccounts(equipmentIds);
            if (equipments == null) return infos;
            do
            {
                endTime = endTime.AddMonths(1);
                var resultInfos = GetCostAnalysisReport(equipments, beginTime, endTime);
                if (resultInfos.Any())
                    infos.AddRange(resultInfos);
                beginTime = endTime;
            } while (
            endTime < toTime);

            return infos;
        }

        /// <summary>
        /// 获取设备成本信息
        /// </summary>
        /// <param name="equipCostInfos">统计设备成本信息</param>
        /// <returns></returns>
        private EntityList<EquipCostInfo> GetEquipCostInfo(EntityList<EquipCostInfo> equipCostInfos)
        {
            var result = new EntityList<EquipCostInfo>();
            if (equipCostInfos.Any())
            {
                var dicInfos = equipCostInfos.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var group in dicInfos)
                {
                    if (group.Value.Any())
                    {
                        var equipCostInfo = new EquipCostInfo();
                        equipCostInfo.Month = group.Value.FirstOrDefault().Month;
                        equipCostInfo.DepreciationCost = group.Value.Sum(m => m.DepreciationCost);
                        equipCostInfo.MaintainCost = group.Value.Sum(m => m.MaintainCost);

                        equipCostInfo.OutsourceCost = group.Value.Sum(m => m.OutsourceCost);
                        equipCostInfo.RepairCost = group.Value.Sum(m => m.RepairCost);
                        equipCostInfo.EnergyConsumptionCost = group.Value.Sum(m => m.EnergyConsumptionCost);

                        equipCostInfo.EquipAccount = group.Value.First().EquipAccount;
                        equipCostInfo.EquipName = group.Value.First().EquipAccount.Name;
                        equipCostInfo.EquipCode = group.Value.First().EquipAccount.Code;
                        equipCostInfo.SparePartCost = group.Value.Sum(m => m.SparePartCost);
                        equipCostInfo.TotalCost = group.Value.Sum(m => m.TotalCost);
                        equipCostInfo.TotalWokerHourCost = group.Value.Sum(m => m.TotalWokerHourCost);
                        result.Add(equipCostInfo);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取月度成本统计信息
        /// </summary>
        /// <returns></returns>
        private EntityList<MonthlyCostInfo> GetMonthlyCostInfos(EntityList<EquipCostInfo> infos)
        {
            var monthlyCostInfos = new EntityList<MonthlyCostInfo>();

            if (infos.Any())
            {

                //组合数据
                var fields = typeof(EquipCostInfo).GetFields();
                var notMatchFields = new List<string>(){
                    "EquipAccountIdProperty",
                    "EquipAccountProperty",
                        "MonthProperty",
                        "EquipNameProperty",
                        "EquipCodeProperty"
                };//不需要生成列的字段

                const int decimals = 1;

                foreach (var field in fields)
                {
                    var fieldName = field.Name;
                    if (notMatchFields.Contains(fieldName)) continue;

                    var eMRM = new MonthlyCostInfo();
                    var label = field.CustomAttributes.FirstOrDefault(m => m.AttributeType.FullName == typeof(LabelAttribute).FullName);
                    eMRM.CostItem = label != null ? label.ConstructorArguments.First().Value.ToString().L10N() : "";

                    eMRM.January = decimal.Round(infos.Where(m => m.Month == 1)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.February = decimal.Round(infos.Where(m => m.Month == 2)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.March = decimal.Round(infos.Where(m => m.Month == 3)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.April = decimal.Round(infos.Where(m => m.Month == 4)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.May = decimal.Round(infos.Where(m => m.Month == 5)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.June = decimal.Round(infos.Where(m => m.Month == 6)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.July = decimal.Round(infos.Where(m => m.Month == 7)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.August = decimal.Round(infos.Where(m => m.Month == 8)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.September = decimal.Round(infos.Where(m => m.Month == 9)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.October = decimal.Round(infos.Where(m => m.Month == 10)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.November = decimal.Round(infos.Where(m => m.Month == 11)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);

                    eMRM.December = decimal.Round(infos.Where(m => m.Month == 12)
                        .Sum(m => AnalyseCommonHelper.GetStatisticProperties(m, field.Name)), decimals);
                    monthlyCostInfos.Add(eMRM);
                }
            }
            return monthlyCostInfos;
        }

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <returns></returns>
        private EntityList<EquipAccount> GetEquipAccounts(List<double> equipmentIds)
        {
            return equipmentIds.SplitContains(tempIds => Query<EquipAccount>().Where(m => tempIds.Contains(m.Id)).ToList());
        }


        /// <summary>
        /// 获取费用工时计算
        /// </summary>
        /// <param name="equipments"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <returns></returns>

        private EntityList<EquipCostInfo> GetCostAnalysisReport(EntityList<EquipAccount> equipments, DateTime fromTime, DateTime toTime)
        {
            const int decimals = 2;

            EntityList<EquipCostInfo> equipCostInfos = new EntityList<EquipCostInfo>();
            var equipmentCodes = equipments.Select(m => m.Code).ToList();

            //获取时间内的所有满足条件的设备的维修单据
            var equipRepairBillList = new EntityList<EquipRepairBill>();
            equipmentCodes.SplitDataExecute((equipCodes) =>
            {
                equipRepairBillList.AddRange(Query<EquipRepairBill>()
                .Where(p => equipCodes.Contains(p.EquipAccount.Code) && p.RepairBeginDate >= fromTime && p.RepairBeginDate < toTime).ToList());
            });


            EntityList<EquipRepairWorkingHours> equipRepairWorkingHours = new EntityList<EquipRepairWorkingHours>();
            if (equipRepairBillList.Any())//费点内存取出所有的维修工时
            {
                var equipRepairBillIds = equipRepairBillList.Select(p => p.Id);
                equipRepairBillIds.SplitDataExecute(equipBillIds =>
                {
                    equipRepairWorkingHours.AddRange(
                         Query<EquipRepairWorkingHours>().Where(p => equipBillIds.Contains(p.EquipRepairBillId)
              && p.BeginTime != null && p.EndTime != null && p.EndTime >= p.BeginTime).ToList());
                });

            }
            ///获取所有设备当前时间段的保养计划表
            var maintainPlans = new EntityList<MaintainPlan>();
            equipmentCodes.SplitDataExecute(equipCodes =>
            {
                maintainPlans.AddRange(Query<MaintainPlan>().Where(p => equipCodes.Contains(p.EquipAccount.Code) && p.ActBeginDate >= fromTime && p.ActBeginDate < toTime).ToList());
            });
            foreach (var equipment in equipments)
            {
                var repairCost = 0d;

                var equipRepairBillIds = equipRepairBillList.Where(m => m.EquipAccountId == equipment.Id).Select(y => y.Id);
                if (equipRepairBillIds.Any())//维修工时成本
                    repairCost = equipRepairWorkingHours
                        .Where(m => equipRepairBillIds.Contains(m.EquipRepairBillId))
                        .Sum(p => ((TimeSpan)(p.EndTime - p.BeginTime)).TotalHours);

                //保养工时成本
                var maintainCost = 0d;
                var maintainPlanIds = maintainPlans.Where(m => m.EquipAccountId == equipment.Id).ToList();
                if (maintainPlanIds.Any())
                {
                    maintainCost = maintainPlanIds.Sum(m => m.SumWorkHours);

                }

                //委外维修成本
                var outsourceList = equipRepairBillList.Where(p => p.RepairWay == EquipRepairWay.OuterRepair && p.RepairCosts != null && p.EquipAccountId == equipment.Id).ToList();
                var outsourceCost = 0m;
                if (outsourceList.Any())
                    outsourceCost = outsourceList.Sum(p => p.RepairCosts.HasValue ? p.RepairCosts.Value : 0);


                var equipRepairSparePartChgList = Query<EquipRepairSparePartChg>().Join<EquipRepairBill>((x, y) => x.EquipRepairBillId == y.Id && y.EquipAccountId == equipment.Id && y.UpdateDate >= fromTime && y.UpdateDate < toTime).ToList();
                var checkPlanSparePartList = Query<CheckPlanSparePart>().Join<CheckPlan>((x, y) => x.CheckPlanId == y.Id && y.EquipAccountId == equipment.Id && y.UpdateDate >= fromTime && y.UpdateDate < toTime).ToList();
                var maintainPlanSparePartList = Query<MaintainPlanSparePart>().Join<MaintainPlan>((x, y) => x.MaintainPlanId == y.Id && y.EquipAccountId == equipment.Id && y.UpdateDate >= fromTime && y.UpdateDate < toTime).ToList();
                //备件成本
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

                //能耗成本
                const decimal energyConsumptionCost = 0m;

                //折旧成本
                const decimal depreciationCost = 0m;

                var equipCostInfo = new EquipCostInfo();
                equipCostInfo.Month = fromTime.Month;
                equipCostInfo.DepreciationCost = decimal.Round(depreciationCost, decimals);
                equipCostInfo.MaintainCost = decimal.Round((decimal)maintainCost, decimals);

                equipCostInfo.OutsourceCost = decimal.Round(outsourceCost, decimals);
                equipCostInfo.RepairCost = decimal.Round((decimal)repairCost, decimals);
                equipCostInfo.EnergyConsumptionCost = decimal.Round(energyConsumptionCost, decimals);

                equipCostInfo.EquipAccount = equipment;
                equipCostInfo.EquipName = equipment.Name;
                equipCostInfo.EquipCode = equipment.Code;
                equipCostInfo.SparePartCost = decimal.Round((decimal)sparePartUnitCost, decimals);
                equipCostInfo.TotalCost = decimal.Round(equipCostInfo.SparePartCost + equipCostInfo.OutsourceCost + equipCostInfo.EnergyConsumptionCost + equipCostInfo.DepreciationCost, decimals);
                equipCostInfo.TotalWokerHourCost = decimal.Round(equipCostInfo.MaintainCost + equipCostInfo.RepairCost, decimals);

                equipCostInfos.Add(equipCostInfo);
            }
            return equipCostInfos;
        }
    }
}
