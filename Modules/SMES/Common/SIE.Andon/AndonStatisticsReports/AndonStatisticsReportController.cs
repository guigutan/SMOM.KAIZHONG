using SIE.Andon.Andons;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Andon.AndonStatisticsReports
{
    /// <summary>
    /// 安灯统计报表控制器
    /// </summary>
    public class AndonStatisticsReportController : DomainController
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual AndonReportInfos GetReportData(AndonStatisticsViewModelCriteria criteria)
        {
            var query = Query<AndonManage>().Where(m => m.State != Andons.Enum.AndonManageState.Cancel);
            var andonReportInfos = new AndonReportInfos();
            if (criteria.AndonClass.HasValue)
            {
                query.Where(m => m.AndonManageClass == criteria.AndonClass);
            }
            if (criteria.AndonNameId.HasValue)
            {
                query.Where(m => m.AndonId == criteria.AndonNameId);
            }
            if (criteria.AndonTypeId.HasValue)
            {
                query.Where(m => m.AndonTypeId == criteria.AndonTypeId);
            }
            if (criteria.FactoryId.HasValue)
            {
                query.Where(m => m.FactoryId == criteria.FactoryId);
            }
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(m => m.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.WipResourceId.HasValue)
            {
                query.Where(m => m.WipResourceId == criteria.WipResourceId);
            }
            if (criteria.EquipAccountId.HasValue)
            {
                query.Where(m => m.EquipAccountId == criteria.EquipAccountId);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(m => m.Andon.DepartmentId == criteria.DepartmentId);
            }
            if (criteria.CreateTime.BeginValue.HasValue && criteria.CreateTime.EndValue.HasValue)
            {
                query.Where(m => m.CreateDate >= criteria.CreateTime.BeginValue && m.CreateDate <= criteria.CreateTime.EndValue);
            }
            else
            {
                if (criteria.CreateTime.BeginValue.HasValue)
                {
                    query.Where(m => m.CreateDate >= criteria.CreateTime.BeginValue);
                }
                if (criteria.CreateTime.EndValue.HasValue)
                {
                    query.Where(m => m.CreateDate <= criteria.CreateTime.EndValue);
                }
            }

            //先执行条件查询所有数据
            var res = query.ToList(null, new EagerLoadOptions().LoadWith(AndonManage.AbnormalCauseProperty).LoadWithViewProperty());
            //查询回来所有操作记录
            var oprateList = new EntityList<AndonManageOperateLog>();
            if (res.Any())
            {
                var andonManageIds = res.Select(p => p.Id).ToList();
                oprateList = andonManageIds.SplitContains(temp =>
                {
                    return Query<AndonManageOperateLog>().Where(m => temp.Contains(m.AndonManageId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                }); 
            }
            //按条件分组
            FiterGroupResultInfos(criteria, andonReportInfos, res, oprateList);
            //转换饼图换算成比例
            if (andonReportInfos.ChartsStatisticsDatas.Any())
            {
                GetPieChartsStatisticsDatas(andonReportInfos);
            }

            return andonReportInfos;
        }

        /// <summary>
        /// 获取饼图统计数据
        /// </summary>
        /// <param name="andonReportInfos"></param>
        private void GetPieChartsStatisticsDatas(AndonReportInfos andonReportInfos)
        {
            foreach (var item in andonReportInfos.ChartsStatisticsDatas)
            {
                var newItem = new ChartsStatistics()
                {
                    GroupName = item.GroupName,
                    AndonNum = item.AndonNum > 0 ? Math.Round(item.AndonNum / andonReportInfos.ChartsStatisticsDatas.Sum(m => m.AndonNum), 2)*100 : 0,
                    AndonStopLine = item.AndonStopLine > 0 ? Math.Round(item.AndonStopLine / andonReportInfos.ChartsStatisticsDatas.Sum(m => m.AndonStopLine), 2) * 100 : 0,

                    AndonStopNum = item.AndonStopNum > 0 ? Math.Round(item.AndonStopNum / andonReportInfos.ChartsStatisticsDatas.Sum(m => m.AndonStopNum), 2) * 100 : 0,
                    AndonTime = item.AndonTime > 0 ? Math.Round(item.AndonTime / andonReportInfos.ChartsStatisticsDatas.Sum(m => m.AndonTime), 2) * 100 : 0,
                    TriggerAccuracy = item.TriggerAccuracy > 0 ? Math.Round(item.TriggerAccuracy / andonReportInfos.ChartsStatisticsDatas.Sum(m => m.TriggerAccuracy), 2) * 100 : 0,
                };
                andonReportInfos.PieChartsStatisticsDatas.Add(newItem);
            }
        }

        /// <summary>
        /// 将查询结果按条件分组
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void FiterGroupResultInfos(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res,
            EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            switch (criteria.GroupLevel)
            {
                //工厂、车间、产线、设备、责任部门、产品、触发人
                case GroupLevel.Factory:
                    GetFactory(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;

                case GroupLevel.ProductionLine:
                    ProductionLine(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Department:
                    GetDepartment(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Equipment:
                    GetEquipment(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Workshop:
                    GetWorkshop(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Product:
                    GetProduct(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
                case GroupLevel.Trigger:
                    GetTrigger(criteria, andonReportInfos, res, andonManageOperateLogs);
                    break;
            }
        }

        /// <summary>
        /// 触发人分组
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetTrigger(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res, EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.TriggerId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Trigger = equipmentItemInfos[key].First().TriggerByName;
                    modelItem.AndonClass = equipmentItemInfos[key].First().AndonManageClass.ToLabel().L10N();
                    statistics.GroupName = modelItem.Trigger + "—" + modelItem.AndonClass;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);

                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.TriggerId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Trigger = equipmentItemInfos[key].First().TriggerByName;
                    modelItem.AndonName = equipmentItemInfos[key].First().AndonName;
                    statistics.GroupName = modelItem.Trigger + "—" + modelItem.AndonName;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.TriggerId, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Trigger = equipmentItemInfos[key].First().TriggerByName;
                    modelItem.AndonType = equipmentItemInfos[key].First().AndonTypeName;
                    statistics.GroupName = modelItem.Trigger + "—" + modelItem.AndonType.L10N();
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
        }

        /// <summary>
        /// 产品分组
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetProduct(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res, EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.ProductCode }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Product = equipmentItemInfos[key].First().ProductCode;
                    modelItem.AndonClass = equipmentItemInfos[key].First().AndonManageClass.ToLabel().L10N();
                    statistics.GroupName = modelItem.Product + "—" + modelItem.AndonClass;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);

                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.ProductCode }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Product = equipmentItemInfos[key].First().ProductCode;
                    modelItem.AndonName = equipmentItemInfos[key].First().AndonName;
                    statistics.GroupName = modelItem.Product + "—" + modelItem.AndonName;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.ProductCode, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Product = equipmentItemInfos[key].First().ProductCode;
                    modelItem.AndonType = equipmentItemInfos[key].First().AndonTypeName;
                    statistics.GroupName = modelItem.Product + "—" + modelItem.AndonType.L10N();
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
        }

        /// <summary>
        /// 获取工厂分组数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetFactory(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res,
            EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.FactoryId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Factory = equipmentItemInfos[key].First().FactoryName;
                    modelItem.AndonClass = equipmentItemInfos[key].First().AndonManageClass.ToLabel().L10N();
                    statistics.GroupName = modelItem.Factory + "—" + modelItem.AndonClass;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);

                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.FactoryId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Factory = equipmentItemInfos[key].First().FactoryName;
                    modelItem.AndonName = equipmentItemInfos[key].First().AndonName;
                    statistics.GroupName = modelItem.Factory + "—" + modelItem.AndonName;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.FactoryId, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.Factory = equipmentItemInfos[key].First().FactoryName;
                    modelItem.AndonType = equipmentItemInfos[key].First().AndonTypeName;
                    statistics.GroupName = modelItem.Factory + "—" + modelItem.AndonType.L10N();
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
        }




        /// <summary>
        /// 获取设备级别统计数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetEquipment(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res,
            EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonManageClass, p.EquipAccountId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.EquipmentName = equipmentItemInfos[key].First().EquipAccountName;
                    modelItem.AndonClass = equipmentItemInfos[key].First().AndonManageClass.ToLabel().L10N();
                    statistics.GroupName = modelItem.EquipmentName + "—" + modelItem.AndonClass;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.AndonId, p.EquipAccountId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.EquipmentName = equipmentItemInfos[key].First().EquipAccountName;
                    modelItem.AndonName = equipmentItemInfos[key].First().AndonName;
                    statistics.GroupName = modelItem.EquipmentName + "—" + modelItem.AndonName;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }

            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var equipmentItemInfos = res.GroupBy(p => new { p.EquipAccountId, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in equipmentItemInfos.Keys)
                {
                    if (!equipmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    double andonStopLine = GetAndonStopLine(equipmentItemInfos[key]);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(equipmentItemInfos[key], andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.EquipmentName = equipmentItemInfos[key].First().EquipAccountName;
                    modelItem.AndonType = equipmentItemInfos[key].First().AndonTypeName;
                    statistics.GroupName = modelItem.EquipmentName + "—" + modelItem.AndonType.L10N();
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
        }
        /// <summary>
        /// 获取车间统计层级数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>

        private void GetWorkshop(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res,
            EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var workshopItemInfos = res.GroupBy(p => new { p.WorkShopId, p.AndonManageClass }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in workshopItemInfos.Keys)
                {
                    if (!workshopItemInfos[key].Any())
                    {
                        continue;
                    }
                    var workshopItemInfosList = workshopItemInfos[key];
                    double andonStopLine = GetAndonStopLine(workshopItemInfosList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(workshopItemInfosList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.AndonClass = workshopItemInfosList.First().AndonManageClass.ToLabel().L10N();
                    modelItem.WorkShop = workshopItemInfosList.First().WorkShopName;
                    statistics.GroupName = modelItem.WorkShop + "—" + modelItem.AndonClass;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var workshopItemInfos = res.GroupBy(p => new { p.WorkShopId, p.AndonId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in workshopItemInfos.Keys)
                {
                    if (!workshopItemInfos[key].Any())
                    {
                        continue;
                    }
                    var workshopItemInfosList = workshopItemInfos[key];
                    double andonStopLine = GetAndonStopLine(workshopItemInfosList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(workshopItemInfosList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.AndonName = workshopItemInfosList.First().AndonName;
                    modelItem.WorkShop = workshopItemInfosList.First().WorkShopName;
                    statistics.GroupName = modelItem.WorkShop + "—" + modelItem.AndonName;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var workshopItemInfos = res.GroupBy(p => new { p.WorkShopId, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in workshopItemInfos.Keys)
                {
                    if (!workshopItemInfos[key].Any())
                    {
                        continue;
                    }
                    var workshopItemInfosList = workshopItemInfos[key];
                    double andonStopLine = GetAndonStopLine(workshopItemInfosList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(workshopItemInfosList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.AndonType = workshopItemInfosList.First().AndonTypeName;
                    modelItem.WorkShop = workshopItemInfosList.First().WorkShopName;
                    statistics.GroupName = modelItem.WorkShop + "—" + modelItem.AndonType.L10N();
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
        }

        /// <summary>
        /// 获取部门分组的
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void GetDepartment(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res,
            EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var departmentItemInfos = res.GroupBy(p => new { p.Andon.DepartmentId, p.AndonManageClass }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in departmentItemInfos.Keys)
                {
                    if (!departmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var departmentItemInfosList = departmentItemInfos[key];
                    double andonStopLine = GetAndonStopLine(departmentItemInfosList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(departmentItemInfosList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.AndonClass = departmentItemInfosList.First().AndonManageClass.ToLabel().L10N();
                    modelItem.Department = departmentItemInfosList.First().Andon.Department.Name;
                    statistics.GroupName = modelItem.Department + "—" + modelItem.AndonClass;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var departmentItemInfos = res.GroupBy(p => new { p.Andon.DepartmentId, p.AndonId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in departmentItemInfos.Keys)
                {
                    if (!departmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var departmentItemInfosList = departmentItemInfos[key];
                    double andonStopLine = GetAndonStopLine(departmentItemInfosList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(departmentItemInfosList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.AndonName = departmentItemInfosList.First().AndonName;
                    modelItem.Department = departmentItemInfosList.First().Andon.DepartmentName;
                    statistics.GroupName = modelItem.Department + "—" + modelItem.AndonName;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);

                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var departmentItemInfos = res.GroupBy(p => new { p.Andon.DepartmentId, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in departmentItemInfos.Keys)
                {
                    if (!departmentItemInfos[key].Any())
                    {
                        continue;
                    }
                    var departmentItemInfosList = departmentItemInfos[key];
                    double andonStopLine = GetAndonStopLine(departmentItemInfosList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(departmentItemInfosList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.AndonType = departmentItemInfosList.First().AndonTypeName;
                    modelItem.Department = departmentItemInfosList.First().Andon.DepartmentName;
                    statistics.GroupName = modelItem.Department + "—" + modelItem.AndonType.L10N();
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
        }

        /// <summary>
        /// 获取分组层级为产线的
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="andonReportInfos"></param>
        /// <param name="res"></param>
        /// <param name="andonManageOperateLogs"></param>
        private void ProductionLine(AndonStatisticsViewModelCriteria criteria, AndonReportInfos andonReportInfos, EntityList<AndonManage> res, EntityList<AndonManageOperateLog> andonManageOperateLogs)
        {
            if (criteria.SummaryDimension == SummaryDimension.AndonClass)
            {
                var productionLineItemInfos = res.GroupBy(p => new { p.WipResourceId, p.AndonManageClass }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in productionLineItemInfos.Keys)
                {
                    if (!productionLineItemInfos[key].Any())
                    {
                        continue;
                    }
                    var productionLineItemList = productionLineItemInfos[key];
                    double andonStopLine = GetAndonStopLine(productionLineItemList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(productionLineItemList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.WipResource = productionLineItemList.First().WipResourceName;
                    modelItem.AndonClass = productionLineItemList.First().AndonManageClass.ToLabel().L10N();
                    statistics.GroupName = modelItem.WipResource + "—" + modelItem.AndonClass;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonType)
            {
                var productionLineItemInfos = res.GroupBy(p => new { p.WipResourceId, p.AndonTypeId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in productionLineItemInfos.Keys)
                {
                    if (!productionLineItemInfos[key].Any())
                    {
                        continue;
                    }
                    var productionLineItemList = productionLineItemInfos[key];
                    double andonStopLine = GetAndonStopLine(productionLineItemList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(productionLineItemList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.WipResource = productionLineItemList.First().WipResourceName;
                    modelItem.AndonType = productionLineItemList.First().AndonTypeName;
                    statistics.GroupName = modelItem.WipResource + "—" + modelItem.AndonType.L10N();
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
            if (criteria.SummaryDimension == SummaryDimension.AndonCode)
            {
                var productionLineItemInfos = res.GroupBy(p => new { p.WipResourceId, p.AndonId }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in productionLineItemInfos.Keys)
                {
                    if (!productionLineItemInfos[key].Any())
                    {
                        continue;
                    }
                    var productionLineItemList = productionLineItemInfos[key];
                    double andonStopLine = GetAndonStopLine(productionLineItemList);
                    var statistics = new ChartsStatistics();
                    var modelItem = SetStatisticsItemValue(productionLineItemList, andonManageOperateLogs, andonStopLine, statistics);
                    modelItem.WipResource = productionLineItemList.First().WipResourceName;
                    modelItem.AndonName = productionLineItemList.First().AndonName;
                    statistics.GroupName = modelItem.WipResource + "—" + modelItem.AndonName;
                    andonReportInfos.StatisticsResultList.Add(modelItem);
                    andonReportInfos.ChartsStatisticsDatas.Add(statistics);
                }
            }
        }

        /// <summary>
        /// 计算停线时间
        /// </summary>
        /// <param name="equipmentItemInfos"></param>
        /// <returns></returns>
        private double GetAndonStopLine(List<AndonManage> equipmentItemInfos)
        {
            //计算停线时间
            var abnormalCauseList = equipmentItemInfos.Where(m => m.LineStop && m.AbnormalCauseId.HasValue).Select(m => m.AbnormalCause).ToList();
            var andonStopLine = 0d;
            abnormalCauseList.ForEach(item =>
            {
                var endtime = item.EndDate.HasValue ? item.EndDate.Value : DateTime.Now;
                andonStopLine += (endtime - item.BeginDate).TotalSeconds / 3600;//除3600
            });
            return andonStopLine;
        }

        /// <summary>
        /// 设置统计项的值
        /// </summary>
        /// <param name="GroupListItemInfos"></param>
        /// <param name="andonManageOperateLogs"></param>
        /// <param name="andonStopLine"></param>
        /// <param name="statistics"></param>
        /// <returns></returns>
        private AndonStatisticsViewModel SetStatisticsItemValue(List<AndonManage> GroupListItemInfos,
            EntityList<AndonManageOperateLog> andonManageOperateLogs, double andonStopLine, ChartsStatistics statistics)
        {
            var oprateHadChangeNames = andonManageOperateLogs.Where(p => GroupListItemInfos.Select(m => m.Id).Contains(p.AndonManageId) && p.OperateType== Andons.Enum.AndonManageOperateType.AndonNameChange).ToList();
            var triggerAccuracy = oprateHadChangeNames.Count > 0 ? Math.Round((double)oprateHadChangeNames.Count / GroupListItemInfos.Count * 100, 2) : 0;

            var modelItem = new AndonStatisticsViewModel()
            {
                AndonNum = GroupListItemInfos.Count,
                AndonStopNum = GroupListItemInfos.Count(m => m.LineStop),
                AndonTime =Math.Round( GroupListItemInfos.Where(m => m.LastTime.HasValue).Sum(m => m.LastTime.Value),2),
                AndonStopLine = Math.Floor(andonStopLine),//向上取整数
                TriggerAccuracy = triggerAccuracy,
            };
            statistics.AndonNum = modelItem.AndonNum;
            statistics.AndonStopNum = modelItem.AndonStopNum;
            statistics.AndonTime = Math.Round(modelItem.AndonTime,2);
            statistics.AndonStopLine = Math.Round(modelItem.AndonStopLine,2);
            statistics.TriggerAccuracy = Math.Round(triggerAccuracy,2);
            return modelItem;
        }
    }
}
